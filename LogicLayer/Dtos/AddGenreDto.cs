using System.ComponentModel.DataAnnotations;

namespace LogicLayer.Dtos
{
    public class AddGenreDto
    {
        [Required]
        public string Name { get; set; }
    }
}
