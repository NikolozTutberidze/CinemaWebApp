namespace CinemaApiSql.Models
{
    public class Director
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
