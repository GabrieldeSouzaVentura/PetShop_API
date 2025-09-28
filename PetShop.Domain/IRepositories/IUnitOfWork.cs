namespace PetShop.Repositories.IRepositories;

public interface IUnitOfWork
{
    IPetRepository PetRepository { get; }
    ITutorRepository TutorRepository { get; }
    Task CommitAsync();
}