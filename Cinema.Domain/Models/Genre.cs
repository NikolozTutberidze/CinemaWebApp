using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
