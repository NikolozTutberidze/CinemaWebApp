namespace Cinema.Domain.Dtos
{
    public class DirectorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<MoviesForDirectorDto> Movies { get; set; }
    }
}
