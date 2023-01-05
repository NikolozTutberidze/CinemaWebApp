using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Dtos
{
    public class AddDirectorDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
