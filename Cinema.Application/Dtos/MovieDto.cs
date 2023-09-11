namespace Cinema.Application.Dtos
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public double IMDBRate { get; set; }
        public string Review { get; set; }
        public DirectorDtoForMovies Director { get; set; }
        public List<ActorsForMovieDto> Actors { get; set; }
        public List<GenresForMovieDto> Genres { get; set; }
    }
}
