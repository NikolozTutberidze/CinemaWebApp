
using Cinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cinema.Infrastructure.Data
{
    public class CinemaContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public CinemaContext()
        {

        }
        public CinemaContext(DbContextOptions<CinemaContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MoviesActors { get; set; }
        public DbSet<MovieGenre> MoviesGenres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-K61LELU\\SQLEXPRESS;Initial Catalog=Cinema;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .ToTable("Movies", "movie")
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity<MovieActor>(
                    j => j
                        .HasOne(ma => ma.Actor)
                        .WithMany()
                        .HasForeignKey(ma => ma.ActorId),
                    j => j
                        .HasOne(ma => ma.Movie)
                        .WithMany()
                        .HasForeignKey(ma => ma.MovieId),
                    j =>
                    {
                        j.ToTable("MovieActors", "movie");
                        j.HasKey(ma => new { ma.MovieId, ma.ActorId });
                    });

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity<MovieGenre>(
                    j => j
                        .HasOne(mg => mg.Genre)
                        .WithMany()
                        .HasForeignKey(mg => mg.GenreId),
                    j => j
                        .HasOne(mg => mg.Movie)
                        .WithMany()
                        .HasForeignKey(mg => mg.MovieId),
                    j =>
                    {
                        j.ToTable("MovieGenres", "movie");
                        j.HasKey(mg => new { mg.MovieId, mg.GenreId });
                    });

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId);

            modelBuilder.Entity<Director>()
                .ToTable("Directors", "movie");

            modelBuilder.Entity<Genre>()
                .ToTable("Genres", "movie");

            modelBuilder.Entity<Actor>()
                .ToTable("Actors", "movie");
        }
    }
}
