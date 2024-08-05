using InventoryControl.Application.Commands.Stocks;
using InventoryControl.Application.Notifications;
using InventoryControl.Domain;
using InventoryControl.Infra.Data.Abstractions;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryControl.Application.Commands.Consumptions
{
    public class StockCommandHandler : IRequestHandler<StockCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public StockCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(StockCommand request, CancellationToken cancellationToken)
        {
            var stock = (await _unitOfWork.Stocks.Query(s => s.ProductId == request.ProductId)).FirstOrDefault();

            var product = await _unitOfWork.Products.GetById(request.ProductId);
            if (product == null)
            {
                await _mediator.Publish(new NotificationCommand("Product", "Product not found"), cancellationToken);
                return Unit.Value;
            }

            if (request.Quantity == 0)
            {
                await _mediator.Publish(new NotificationCommand("Stock", "Cannot process zero quantity."), cancellationToken);
                return Unit.Value;
            }

            _unitOfWork.BeginTransaction();

            if (stock == null) // Produto não cadastrado no Stock
            {
                if (request.Quantity < 0)
                {
                    await _mediator.Publish(new NotificationCommand("Stock", "Cannot reduce stock for a non-existent product."), cancellationToken);
                    return Unit.Value;
                }

                stock = new Stock
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,
                    AveragePrice = request.UnitPrice,
                    Total = request.UnitPrice * request.Quantity
                };

                await _unitOfWork.Stocks.Insert(stock);
                _unitOfWork.Commit();
                return Unit.Value;
            }

            if (request.Quantity > 0) // Adicionar no estoque
            {
                stock.Quantity += request.Quantity;
                stock.Total += request.Quantity * request.UnitPrice; // Custo Total (Estoque)
                stock.AveragePrice = stock.Total / stock.Quantity; // Custo Médio por Unidade
                stock.UnitPrice = request.UnitPrice; // Ultimo Preço por Unidade
            }
            else // Remover do estoque
            {
                if (stock.Quantity < Math.Abs(request.Quantity))
                {
                    await _mediator.Publish(new NotificationCommand("Stock", "Insufficient stock available."), cancellationToken);
                    _unitOfWork.Rollback();
                    return Unit.Value;
                }

                var quantityToRemove = Math.Abs(request.Quantity);
                stock.Quantity -= quantityToRemove;
                stock.Total -= stock.AveragePrice * quantityToRemove; // Custo Total (Estoque)

                if (stock.Quantity == 0)
                {
                    stock.AveragePrice = 0;
                }
                else
                {
                    stock.AveragePrice = stock.Total / stock.Quantity; // Custo Médio por Unidade
                }

                await UpdateConsumption(stock.ProductId, quantityToRemove, stock.AveragePrice);
            }

            await _unitOfWork.Stocks.Update(stock);
            _unitOfWork.Commit();

            return Unit.Value;
        }

        private async Task UpdateConsumption(int productId, int quantityConsumed, decimal averagePrice)
        {
            var consumptionDate = DateTime.UtcNow.Date;
            var existingConsumption = (await _unitOfWork.Consumptions.Query(c => c.ProductId == productId && c.ConsumptionDate == consumptionDate)).FirstOrDefault();

            if (existingConsumption != null)
            {
                existingConsumption.QuantityConsumed += quantityConsumed;
                existingConsumption.TotalAveragePrice = (existingConsumption.TotalCost + (averagePrice * quantityConsumed)) / existingConsumption.QuantityConsumed;
                existingConsumption.TotalCost += averagePrice * quantityConsumed;
                await _unitOfWork.Consumptions.Update(existingConsumption);
            }
            else
            {
                var consumption = new Consumption
                {
                    ProductId = productId,
                    QuantityConsumed = quantityConsumed,
                    ConsumptionDate = consumptionDate,
                    TotalAveragePrice = averagePrice,
                    TotalCost = averagePrice * quantityConsumed
                };

                await _unitOfWork.Consumptions.Insert(consumption);
            }
        }
    }
}
