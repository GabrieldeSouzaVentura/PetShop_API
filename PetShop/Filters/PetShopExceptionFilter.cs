using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetShop.Filters;

public class PetShopExceptionFilter : IExceptionFilter
{
    private readonly ILogger<PetShopExceptionFilter> _logger;

    public PetShopExceptionFilter(ILogger<PetShopExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred: Status code 500");
        context.Result = new ObjectResult("There was a problem processing your request: Status code 500")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}