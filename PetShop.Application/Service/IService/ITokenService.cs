using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PetShop.PetShop.Application.Service.IService;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _configuration);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _configuration);
}