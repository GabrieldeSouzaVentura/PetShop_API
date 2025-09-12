using PetShop.Context;
using PetShop.Repositories.IRepositories;

namespace PetShop.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private ITutorRepository? _tutorRepository;
    private IPetRepository? _petRepository;

    public AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IPetRepository PetRepository
    {
        get { return _petRepository = _petRepository ?? new PetRepository(_context); }
    }

    public ITutorRepository TutorRepository
    {
        get { return _tutorRepository = _tutorRepository ?? new TutorRepository(_context); }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}