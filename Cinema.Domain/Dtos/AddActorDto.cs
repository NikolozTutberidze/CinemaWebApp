using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.Dtos
{
    public class AddActorDto
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [MinLength (2)]
        public string Origin { get; set; }
    }
}
