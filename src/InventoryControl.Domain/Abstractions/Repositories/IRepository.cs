using System.Linq.Expressions;

namespace InventoryControl.Domain.Abstractions.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(object parm);
        public Task<T> Get(Expression<Func<T, bool>> expression);
        public Task<object> Insert(T entity);
        public Task<bool> Delete(T entity);
        public Task<bool> Update(T entity);
        public Task<IEnumerable<T>> Query(Expression<Func<T, bool>> expression);
    }
}
