using Microsoft.AspNetCore.Mvc.Filters;

namespace PetShop.Filters;

public class PetShopLoggingFilter : IActionFilter
{
    private readonly ILogger<PetShopLoggingFilter> _logger;

    public PetShopLoggingFilter(ILogger<PetShopLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("-------------------------------------------");
        _logger.LogInformation("\tOnActionExecuting");
        _logger.LogInformation($"\t{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"\tSatatus Code : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("--------------------------------------------");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("-------------------------------------------");
        _logger.LogInformation("\tOnActionExecuted");
        _logger.LogInformation($"\t{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"\tModelStade : {context.ModelState.IsValid}");
        _logger.LogInformation("--------------------------------------------");
    }
}