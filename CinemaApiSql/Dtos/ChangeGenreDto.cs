using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Dtos
{
    public class ChangeGenreDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
