using DataAccessLayer.Models.Joins;

namespace LogicLayer.Dtos
{
    public class GenreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MoviesForGenreDto> Movies { get; set; }
    }
}
