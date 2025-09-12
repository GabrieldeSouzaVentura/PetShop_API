using PetShop.Context;
using PetShop.Models;
using PetShop.Repositories.IRepositories;

namespace PetShop.Repositories;

public class PetRepository : Repository<Pet>, IPetRepository
{
    public PetRepository(AppDbContext context) : base(context) { }
}