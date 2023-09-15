using Cinema.Domain.Models;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>
    {
        public CinemaContext Context => (_context as CinemaContext)!;

        public MovieRepository(CinemaContext context) : base(context) { }
    }
}
