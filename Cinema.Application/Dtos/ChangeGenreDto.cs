using System.ComponentModel.DataAnnotations;

namespace Cinema.Application.Dtos
{
    public class ChangeGenreDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
