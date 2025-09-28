using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetShop.Application.Service.Exceptions;

namespace PetShop.PetShop.Api.Filters;

public class PetShopExceptionFilter : IExceptionFilter
{
    private readonly ILogger<PetShopExceptionFilter> _logger;

    public PetShopExceptionFilter(ILogger<PetShopExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string errorMessage = "There was a problem processing your request: Status code 500";

        var exceptionType = context.Exception.GetType();

        if (exceptionType == typeof(ResourceNotFoundException))
        {
            errorMessage = context.Exception.Message;
            statusCode = HttpStatusCode.NotFound;
        } else if (exceptionType == typeof(BusinessValidationException))
        {
            errorMessage = context.Exception.Message;
            statusCode = HttpStatusCode.BadRequest;
        }

        _logger.LogError(context.Exception, "An unhandled exception occurred: Status code 500");
        context.Result = new ObjectResult(errorMessage)
        {
            StatusCode = (int)statusCode
        };
    }
}