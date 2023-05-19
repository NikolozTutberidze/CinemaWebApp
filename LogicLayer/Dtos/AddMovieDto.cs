using System.ComponentModel.DataAnnotations;

namespace LogicLayer.Dtos
{
    public class AddMovieDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public double IMDBRate { get; set; }
        [Required]
        public string Review { get; set; }
        [Required]
        public Guid DirectorId { get; set; }
        [Required]
        public List<Guid> ActorIds { get; set; }
        [Required]
        public List<Guid> GenreIds { get; set; }
    }
}
