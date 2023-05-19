using LogicLayer.Dtos;

namespace LogicLayer.Services.Interfaces
{
    public interface IGenreService
    {
        Task<GenreDto> AddGenreAsync(AddGenreDto request);
        Task<GenreDto> GetGenreAsync(Guid requestId);
        Task<ICollection<GenreDto>> GetGenresAsync();
        Task ChangeGenreAsync(ChangeGenreDto request);
        Task DeleteGenreAsync(Guid requestId);
    }
}
