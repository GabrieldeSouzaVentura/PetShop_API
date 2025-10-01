using System.Linq.Expressions;

namespace PetShop.Application.Service.IService;

public interface IFilterExpressionService
{
    Task<Expression<Func<T, bool>>?> GetFilterByUserAsync<T, TKey>(Expression<Func<T, TKey>> tutorIdSelector);
    Task<Expression<Func<T, bool>>> GetFilterByNameAndUserAsync<T, TKey>(
        string name, Expression<Func<T, string>> nameSelector, Expression<Func<T, TKey>> tutorIdSelector);

}
