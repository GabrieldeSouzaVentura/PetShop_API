using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace PetShop.Repositories.IRepositories;

public interface IRepository<T>
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}
