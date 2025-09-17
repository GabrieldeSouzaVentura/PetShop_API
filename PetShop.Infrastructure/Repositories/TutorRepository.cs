using PetShop.Models;
using PetShop.PetShop.Infrastructure.Context;
using PetShop.Repositories.IRepositories;

namespace PetShop.PetShop.Infrastructure.Repositories;

public class TutorRepository : Repository<Tutor>, ITutorRepository
{
    public TutorRepository(AppDbContext context) : base(context) { }
}