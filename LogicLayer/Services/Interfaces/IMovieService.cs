using LogicLayer.Dtos;

namespace LogicLayer.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDto> AddMovieAsync(AddMovieDto request);
        Task<ICollection<MovieDto>> GetMoviesAsync();
        Task<MovieDto> GetMovieByIdAsync(Guid requestId);
        Task<ICollection<MovieDto>> GetMovieByTitleAsync(string requestTitle);
        Task<ICollection<MovieDto>> GetMovieByYearAsync(int firstYear, int secondYear);
        Task<ICollection<MovieDto>> GetMovieByIMDBAsync(double firstRating, double secondRating);
        Task ChangeMovieAsync(ChangeMovieDto request);
        Task DeleteMovieAsync(Guid requestId);
    }
}
