using CinemaApiSql.Data;
using CinemaApiSql.Dtos;
using CinemaApiSql.Interfaces;
using CinemaApiSql.Models;

namespace CinemaApiSql.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly CinemaContext _database;
        public ActorRepository(CinemaContext database)
        {
            _database = database;
        }

        public void AddActor(Actor request)
        {
            _database.Actors.Add(request);
            _database.SaveChanges();
        }

        public void ChangeActor(ChangeActorDto request)
        {
            Actor actor = new()
            {
                Id = request.Id,
                FullName = request.FullName,
                BirthDate = request.BirthDate,
                Origin = request.Origin
            };
            _database.Actors.Update(actor);
            _database.SaveChanges();
        }

        public bool CheckActorExisting(Guid requestId)
        {
            if (_database.Actors.Any(a => a.Id == requestId))
                return true;
            else
                return false;
        }

        public bool CheckActorExisting(string requestName)
        {
            if (_database.Actors.Any(a => a.FullName == requestName))
                return true;
            else
                return false;
        }

        public void DeleteActor(Guid requestId)
        {
            var actor = _database.Actors.Where(a => a.Id == requestId).First();
            _database.Actors.Remove(actor);
            _database.SaveChanges();
        }

        public ActorDto GetActor(Guid requestId)
        {
            var actor = _database.Actors.Where(a => a.Id == requestId).Select(a => new ActorDto
            {
                Id = a.Id,
                FullName = a.FullName,
                BirthDate = a.BirthDate,
                Origin = a.Origin,
                MoviesActors = a.MoviesActors
            }).First();
            return actor;
        }

        public ICollection<ActorDto> GetActors()
        {
            var actors = _database.Actors.Select(a => new ActorDto
            {
                Id = a.Id,
                FullName = a.FullName,
                BirthDate = a.BirthDate,
                Origin = a.Origin,
                MoviesActors = a.MoviesActors
            }).ToList();
            return actors;
        }
    }
}
