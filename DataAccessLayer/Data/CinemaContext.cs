using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class CinemaContext : DbContext
    {
        public CinemaContext()
        {

        }
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MoviesActors { get; set; }
        public DbSet<MovieGenre> MoviesGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId });
            modelBuilder.Entity<MovieActor>()
                .HasOne(movie => movie.Movie)
                .WithMany(ma => ma.MoviesActors)
                .HasForeignKey(movie => movie.MovieId);
            modelBuilder.Entity<MovieActor>()
                .HasOne(actor => actor.Actor)
                .WithMany(ma => ma.MoviesActors)
                .HasForeignKey(actor => actor.ActorId);

            modelBuilder.Entity<MovieGenre>().HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.Entity<MovieGenre>()
                .HasOne(movie => movie.Movie)
                .WithMany(mg => mg.MoviesGenres)
                .HasForeignKey(movie => movie.MovieId);
            modelBuilder.Entity<MovieGenre>()
                .HasOne(genre => genre.Genre)
                .WithMany(mg => mg.MoviesGenres)
                .HasForeignKey(genre => genre.GenreId);
        }
    }
}
