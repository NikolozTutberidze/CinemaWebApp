using System.ComponentModel.DataAnnotations;

namespace LogicLayer.Dtos
{
    public class AddDirectorDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
