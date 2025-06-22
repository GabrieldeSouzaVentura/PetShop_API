using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PetShop.Service.IService;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _configuration);

    string GenerateRefreshToken();

    ClaimsPrincipal GetClaimPricipalFromExpiredToken(string token, IConfiguration _configuration);
}