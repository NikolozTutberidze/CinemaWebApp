using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interfaces
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
