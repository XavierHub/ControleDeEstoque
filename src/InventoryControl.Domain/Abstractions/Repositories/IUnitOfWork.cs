using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;

namespace InventoryControl.Infra.Data.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        IRepository<Product> Products { get; }
        IRepository<Consumption> Consumptions { get; }
        IRepository<Stock> Stocks { get; }
    }
}
