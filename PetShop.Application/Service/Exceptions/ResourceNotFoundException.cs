namespace PetShop.Application.Service.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message){ }
    }
}
