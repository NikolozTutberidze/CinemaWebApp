using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Models;
using Cinema.Domain.Models.Joins;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories.OldRepos
{
    public class ActorRepositoryOld : IActorRepository
    {
        private readonly CinemaContext _database;
        public ActorRepositoryOld(CinemaContext database)
        {
            _database = database;
        }

        public async Task AddActorAsync(Actor request)
        {
            _database.Actors.Add(request);
            await _database.SaveChangesAsync();
        }

        public async Task ChangeActorAsync(Actor request)
        {
            _database.Actors.Update(request);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteActorAsync(Actor request)
        {
            _database.Actors.Remove(request);
            await _database.SaveChangesAsync();
        }

        public async Task<Actor> GetActorAsync(Guid requestId)
        {
            var actor = await _database.Actors.FirstOrDefaultAsync(a => a.Id == requestId);
            return actor;
        }

        public async Task<ICollection<MovieActor>> GetMovieActorsAsync()
        {
            var movieActors = await _database.MoviesActors.ToListAsync();
            return movieActors;
        }

        public async Task<ICollection<Actor>> GetActorsAsync()
        {
            var actors = await _database.Actors.ToListAsync();
            return actors;
        }
    }
}
