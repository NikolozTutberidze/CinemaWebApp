using CinemaApiSql.Models;

namespace CinemaApiSql.Dtos
{
    public class DirectorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
