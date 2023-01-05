using CinemaApiSql.Models.Joins;
using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Dtos
{
    public class ChangeMovieDto
    {
        [Required]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Date { get; set; }
        public double? IMDBRate { get; set; }
        public string? Review { get; set; }
        public Guid? DirectorId { get; set; }
        public List<Guid>? ActorIds { get; set; }
        public List<Guid>? GenreIds { get; set; }
    }
}
