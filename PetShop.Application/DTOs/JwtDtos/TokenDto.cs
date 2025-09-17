namespace PetShop.DTOs.JwtDtos;

public class TokenDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
