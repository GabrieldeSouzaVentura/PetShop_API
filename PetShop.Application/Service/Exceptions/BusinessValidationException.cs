namespace PetShop.Application.Service.Exceptions;

public class BusinessValidationException : Exception
{
    public BusinessValidationException(string message) : base(message) { }
}
