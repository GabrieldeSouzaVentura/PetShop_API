using PetShop.Context;
using PetShop.Models;
using PetShop.Repositories.IRepositories;

namespace PetShop.Repositories;

public class TutorRepository : Repository<Tutor>, ITutorRepository
{
    public TutorRepository(AppDbContext context) : base(context)
    {
    }
}