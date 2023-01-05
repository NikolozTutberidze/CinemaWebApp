using CinemaApiSql.Dtos;
using CinemaApiSql.Models;

namespace CinemaApiSql.Interfaces
{
    public interface IActorRepository
    {
        public void AddActor(Actor request);
        public ActorDto GetActor(Guid requestId);
        public ICollection<ActorDto> GetActors();
        public void ChangeActor(ChangeActorDto request);
        public void DeleteActor(Guid requestId);
        public bool CheckActorExisting(Guid requestId);
        public bool CheckActorExisting(string Name);
    }
}
