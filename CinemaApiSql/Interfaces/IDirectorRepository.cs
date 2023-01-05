using CinemaApiSql.Dtos;
using CinemaApiSql.Models;

namespace CinemaApiSql.Interfaces
{
    public interface IDirectorRepository
    {
        public void AddDirector(Director request);
        public DirectorDto GetDirector(Guid requestId);
        public ICollection<DirectorDto> GetDirectors();
        public void ChangeDirector(Director request);
        public void DeleteDirector(Guid requsetId);
        public bool CheckDirectorExisting(Guid Id);
        public bool CheckDirectorExisting(string name);
    }
}
