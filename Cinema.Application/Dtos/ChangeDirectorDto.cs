using System.ComponentModel.DataAnnotations;

namespace Cinema.Application.Dtos
{
    public class ChangeDirectorDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
