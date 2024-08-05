using InventoryControl.Domain;
using InventoryControl.Domain.Abstractions.Repositories;
using InventoryControl.Infra.Data.Abstractions;
using System.Data;

namespace InventoryControl.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Product> Products { get; private set; }
        public IRepository<Consumption> Consumptions { get; private set; }
        public IRepository<Stock> Stocks { get; private set; }

        private readonly IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private bool _disposed;

        public UnitOfWork(IDbConnection dbConnection,
                          IRepository<Product> productRepository,
                          IRepository<Consumption> consumptionRepository,
                          IRepository<Stock> stocks
                         )
        {
            _dbConnection = dbConnection;
            Products = productRepository;
            Consumptions = consumptionRepository;
            Stocks = stocks;
        }

        public void BeginTransaction()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            _dbTransaction = _dbConnection.BeginTransaction();
            SetTransactionForRepositories();
        }

        public void Commit()
        {
            try
            {
                _dbTransaction?.Commit();
            }
            catch
            {
                _dbTransaction?.Rollback();
                throw;
            }
            finally
            {
                _dbTransaction?.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                _dbTransaction?.Rollback();
            }
            finally
            {
                _dbTransaction?.Dispose();
            }
        }

        private void SetTransactionForRepositories()
        {
            if (Products is Repository<Product> productRepo)
            {
                productRepo.SetTransaction(_dbTransaction);
            }
            if (Consumptions is Repository<Consumption> consumptionRepo)
            {
                consumptionRepo.SetTransaction(_dbTransaction);
            }
            if (Stocks is Repository<Stock> stockRepo)
            {
                stockRepo.SetTransaction(_dbTransaction);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbTransaction?.Dispose();
                    _dbConnection?.Close();
                    _dbConnection?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
