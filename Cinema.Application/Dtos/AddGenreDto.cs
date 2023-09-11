using System.ComponentModel.DataAnnotations;

namespace Cinema.Application.Dtos
{
    public class AddGenreDto
    {
        [Required]
        public string Name { get; set; }
    }
}
