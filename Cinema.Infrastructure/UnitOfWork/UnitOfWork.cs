using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.UnitOfWorkAbstract;
using Cinema.Domain.Models;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories;
using Cinema.Infrastructure.Repositories.OldRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Infrastructure.UnitOfWork
{
    public class UnitOfWork : ICinemaUnitOfWork
    {
        private readonly CinemaContext _context;

        #region Constructor

        public UnitOfWork(CinemaContext context)
        {
            _context = context;

            Actors = new ActorRepository(_context);
            Directors = new DirectorRepository(_context);
            Genres = new GenreRepository(_context);
            Movies = new MovieRepository(_context);
        }

        #endregion

        #region Properties

        public IRepository<Actor> Actors { get; private set; }

        public IRepository<Director> Directors { get; private set; }

        public IRepository<Genre> Genres { get; private set; }

        public IRepository<Movie> Movies { get; private set; }

        #endregion

        #region Methods

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
