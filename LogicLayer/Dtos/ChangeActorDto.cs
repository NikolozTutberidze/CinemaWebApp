using System.ComponentModel.DataAnnotations;

namespace LogicLayer.Dtos
{
    public class ChangeActorDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Origin { get; set; }
    }
}
