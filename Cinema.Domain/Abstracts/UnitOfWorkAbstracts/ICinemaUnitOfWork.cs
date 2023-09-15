using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.UnitOfWorkAbstract;
using Cinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Abstracts.UnitOfWorkAbstract
{
    public interface ICinemaUnitOfWork : IUnitOfWork
    {
        IRepository<Actor> Actors { get; }
        IRepository<Director> Directors { get; }
        IRepository<Genre> Genres { get; }
        IRepository<Movie> Movies { get; }
    }
}
