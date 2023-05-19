using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IActorRepository
    {
        Task AddActorAsync(Actor request);
        Task<Actor> GetActorAsync(Guid requestId);
        Task<ICollection<MovieActor>> GetMovieActorsAsync();
        Task<ICollection<Actor>> GetActorsAsync();
        Task ChangeActorAsync(Actor request);
        Task DeleteActorAsync(Actor request);
    }
}
