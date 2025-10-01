using PetShop.DTOs.JwtDtos;

namespace PetShop.PetShop.Application.Service.IService;

public interface IUserServices
{
    Task<IEnumerable<ResponseInformationsDto>> GetUserInformationsAsync();
    Task<ResponseInformationsDto> GetByNameUser(string name);
}
