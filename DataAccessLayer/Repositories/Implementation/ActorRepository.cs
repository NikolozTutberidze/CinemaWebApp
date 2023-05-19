using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementation
{
    public class ActorRepository : IActorRepository
    {
        private readonly CinemaContext _database;
        public ActorRepository(CinemaContext database)
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
            var actor = await _database.Actors.Where(a => a.Id == requestId).FirstOrDefaultAsync();
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
