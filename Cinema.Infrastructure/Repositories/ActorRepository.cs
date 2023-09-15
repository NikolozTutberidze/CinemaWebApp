﻿using Cinema.Domain.Models;
using Cinema.Infrastructure.Data;
using Cinema.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Infrastructure.Repositories
{
    public class ActorRepository : Repository<Actor>
    {
        public CinemaContext Context => (_context as CinemaContext)!;

        public ActorRepository(CinemaContext context) : base(context) { }
    }
}
