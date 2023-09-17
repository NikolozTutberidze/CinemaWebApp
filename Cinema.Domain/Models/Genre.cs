using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
