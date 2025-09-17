using PetShop.Models;
using PetShop.PetShop.Infrastructure.Context;
using PetShop.Repositories.IRepositories;

namespace PetShop.PetShop.Infrastructure.Repositories;

public class PetRepository : Repository<Pet>, IPetRepository
{
    public PetRepository(AppDbContext context) : base(context) { }
}