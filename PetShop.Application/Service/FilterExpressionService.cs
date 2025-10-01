using System.Linq.Expressions;
using PetShop.Application.Service.IService;
namespace PetShop.Application.Service;

public class FilterExpressionService : IFilterExpressionService
{
    private readonly IUserContextService _userContextService;
    private readonly IdentityServices _identityServices;

    public FilterExpressionService(IUserContextService userContextService, IdentityServices identityServices)
    {
        _userContextService = userContextService;
        _identityServices = identityServices;
    }

    public async Task<Expression<Func<T, bool>>> GetFilterByNameAndUserAsync<T, TKey>(
        string name, Expression<Func<T, string>> nameSelector, Expression<Func<T, TKey>> tutorIdSelector)
    {
        var user = await _userContextService.GetLoggerUserAsync();
        var isAdmin = await _identityServices.IsInRoleAsync(user);

        var param = Expression.Parameter(typeof(T), "x");

        var nameProp = Expression.Invoke(nameSelector, param);
        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var namePropToLower = Expression.Call(nameProp, toLowerMethod);
        var nameConstantToLower = Expression.Constant(name.ToLower());
        var nameEquals = Expression.Equal(namePropToLower, nameConstantToLower);

        Expression finalBody;

        if (isAdmin) { finalBody = nameEquals; }

        else
        {
            var tutorProp = Expression.Invoke(tutorIdSelector, param);
            var tutorIdConstant = Expression.Constant(user.TutorId);
            var tutorCondition = Expression.Equal(tutorProp, tutorIdConstant);
            finalBody = Expression.AndAlso(nameEquals, tutorCondition);
        }

        return Expression.Lambda<Func<T, bool>>(finalBody, param);
    }

    public async Task<Expression<Func<T, bool>>?> GetFilterByUserAsync<T, TKey>(Expression<Func<T, TKey>> tutorIdSelector)
    {
        var user = await _userContextService.GetLoggerUserAsync();
        var isAdmin = await _identityServices.IsInRoleAsync(user);

        if (isAdmin) return null;

        return Expression.Lambda<Func<T, bool>>(
            Expression.Equal(tutorIdSelector.Body, Expression.Constant(user.TutorId, typeof(TKey))),tutorIdSelector.Parameters);
    }
}
