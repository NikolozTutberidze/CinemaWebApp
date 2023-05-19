using System.ComponentModel.DataAnnotations;

namespace LogicLayer.Dtos
{
    public class AddDirectorDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
