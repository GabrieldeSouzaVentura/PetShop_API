using System.Text.Json;

namespace PetShop.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Manssage { get; set; }
    public string? Trace { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}