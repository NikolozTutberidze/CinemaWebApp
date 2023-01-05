using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Dtos
{
    public class AddGenreDto
    {
        [Required]
        public string Name { get; set; }
    }
}
