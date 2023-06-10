using LogicLayer.Dtos;
using LogicLayer.Services.CustomResponse;

namespace LogicLayer.Services.Interfaces
{
    public interface IGenreService
    {
        Task<ServiceResponse<GenreDto>> AddGenreAsync(AddGenreDto request);
        Task<ServiceResponse<GenreDto>> GetGenreAsync(Guid requestId);
        Task<ServiceResponse<ICollection<GenreDto>>> GetGenresAsync();
        Task<ServiceResponse<GenreDto>> ChangeGenreAsync(ChangeGenreDto request);
        Task<ServiceResponse<GenreDto>> DeleteGenreAsync(Guid requestId);
    }
}
