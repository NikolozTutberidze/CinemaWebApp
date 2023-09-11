using Cinema.Domain.Models;
using Cinema.Domain.Models.Joins;

namespace Cinema.Domain.Abstracts.RepositoryAbstracts
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
