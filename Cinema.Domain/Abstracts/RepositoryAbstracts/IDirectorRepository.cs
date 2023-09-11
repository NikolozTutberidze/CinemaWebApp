using Cinema.Domain.Models;

namespace Cinema.Domain.Abstracts.RepositoryAbstracts
{
    public interface IDirectorRepository
    {
        Task AddDirectorAsync(Director request);
        Task<Director> GetDirectorAsync(Guid requestId);
        Task<ICollection<Director>> GetDirectorsAsync();
        Task ChangeDirectorAsync(Director request);
        Task DeleteDirectorAsync(Director requset);
    }
}
