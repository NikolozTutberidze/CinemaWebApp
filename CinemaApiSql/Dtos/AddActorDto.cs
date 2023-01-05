using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Dtos
{
    public class AddActorDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Origin { get; set; }
    }
}
