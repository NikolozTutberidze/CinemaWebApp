using DataAccessLayer.Models;

namespace LogicLayer.Dtos
{
    public class DirectorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<MoviesForDirectorDto> Movies { get; set; }
    }
}
