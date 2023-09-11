using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Dtos
{
    public class AddGenreDto
    {
        [Required]
        public string Name { get; set; }
    }
}
