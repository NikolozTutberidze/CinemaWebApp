namespace Cinema.Domain.Dtos
{
    public class ActorsForMovieDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Origin { get; set; }
    }
}
