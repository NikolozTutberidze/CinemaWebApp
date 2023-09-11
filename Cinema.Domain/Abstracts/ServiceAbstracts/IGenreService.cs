using Cinema.Domain.CustomResponse;
using LogicLayer.Dtos;

namespace Cinema.Domain.Abstracts.ServiceAbstracts
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
