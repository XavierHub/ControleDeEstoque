using Dapper.Dommel;
using InventoryControl.Domain.Abstractions.Repositories;
using System.Data;
using System.Linq.Expressions;

namespace InventoryControl.Infra.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public Repository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public Task<bool> Delete(T entity)
        {
            return _dbConnection.DeleteAsync(entity, _dbTransaction);
        }

        public Task<T> Get(Expression<Func<T, bool>> expression)
        {
            return _dbConnection.FirstOrDefaultAsync(expression, _dbTransaction);
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return _dbConnection.GetAllAsync<T>(_dbTransaction);
        }

        public Task<T> GetById(object parm)
        {
            return _dbConnection.GetAsync<T>(parm, _dbTransaction);
        }

        public Task<object> Insert(T entity)
        {
            return _dbConnection.InsertAsync(entity, _dbTransaction);
        }

        public Task<bool> Update(T entity)
        {
            return _dbConnection.UpdateAsync(entity, _dbTransaction);
        }

        public Task<IEnumerable<T>> Query(Expression<Func<T, bool>> expression)
        {
            return _dbConnection.SelectAsync(expression, _dbTransaction);
        }
    }
}
