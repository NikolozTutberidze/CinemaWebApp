using CinemaApiSql.Data;
using CinemaApiSql.Dtos;
using CinemaApiSql.Interfaces;
using CinemaApiSql.Models;

namespace CinemaApiSql.Repository
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly CinemaContext _database;
        public DirectorRepository(CinemaContext database)
        {
            _database = database;
        }
        public void AddDirector(Director request)
        {
            _database.Directors.Add(request);
            _database.SaveChanges();
        }

        public void ChangeDirector(Director request)
        {
            _database.Directors.Update(request);
            _database.SaveChanges();
        }

        public void DeleteDirector(Guid requsetId)
        {
            var director = _database.Directors.Where(d => d.Id == requsetId).First();
            _database.Directors.Remove(director);
            _database.SaveChanges();
        }

        public DirectorDto GetDirector(Guid requestId)
        {
            var director = _database.Directors.Where(d => d.Id == requestId).Select(d => new DirectorDto
            {
                Id = d.Id,
                FullName = d.FullName,
                BirthDate = d.BirthDate,
                Movies = d.Movies
            }).First();
            return director;
        }

        public ICollection<DirectorDto> GetDirectors()
        {
            var directors = _database.Directors.Select(d => new DirectorDto
            {
                Id = d.Id,
                FullName = d.FullName,
                BirthDate = d.BirthDate,
                Movies = d.Movies
            }).ToList();
            return directors;
        }
        public bool CheckDirectorExisting(Guid Id)
        {
            if (_database.Directors.Any(d => d.Id == Id))
                return true;
            else
                return false;
        }

        public bool CheckDirectorExisting(string name)
        {
            if (_database.Directors.Any(d => d.FullName == name))
                return true;
            else
                return false;
        }
    }
}
