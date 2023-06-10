using LogicLayer.Dtos;
using LogicLayer.Services.CustomResponse;

namespace LogicLayer.Services.Interfaces
{
    public interface IMovieService
    {
        Task<ServiceResponse<MovieDto>> AddMovieAsync(AddMovieDto request);
        Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesAsync();
        Task<ServiceResponse<MovieDto>> GetMovieByIdAsync(Guid requestId);
        Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesByTitleAsync(string requestTitle);
        Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesByYearAsync(int firstYear, int secondYear);
        Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesByIMDBAsync(double firstRating, double secondRating);
        Task<ServiceResponse<MovieDto>> ChangeMovieAsync(ChangeMovieDto request);
        Task<ServiceResponse<MovieDto>> DeleteMovieAsync(Guid requestId);
    }
}
