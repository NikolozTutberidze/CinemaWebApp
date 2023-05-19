using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implementation
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly CinemaContext _database;
        public DirectorRepository(CinemaContext database)
        {
            _database = database;
        }
        public async Task AddDirectorAsync(Director request)
        {
            _database.Directors.Add(request);
            await _database.SaveChangesAsync();
        }

        public async Task ChangeDirectorAsync(Director request)
        {
            _database.Directors.Update(request);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteDirectorAsync(Director request)
        {
            _database.Directors.Remove(request);
            await _database.SaveChangesAsync();
        }

        public async Task<Director> GetDirectorAsync(Guid requestId)
        {
            var director = await _database.Directors.Where(d => d.Id == requestId).FirstOrDefaultAsync();
            return director;
        }

        public async Task<ICollection<Director>> GetDirectorsAsync()
        {
            var directors = await _database.Directors.ToListAsync();
            return directors;
        }
    }
}
