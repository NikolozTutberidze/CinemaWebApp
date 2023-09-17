using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double IMDBRate { get; set; }
        public string Review { get; set; }
        public int DirectorId { get; set; }

        [ForeignKey("DirectorId")]
        public Director Director { get; set; }

        public ICollection<Actor> Actors { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
