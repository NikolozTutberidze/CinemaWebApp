using LogicLayer.Dtos;

namespace LogicLayer.Services.Interfaces
{
    public interface IDirectorService
    {
        Task<DirectorDto> AddDirectorAsync(AddDirectorDto request);
        Task<DirectorDto> GetDirectorAsync(Guid requestId);
        Task<ICollection<DirectorDto>> GetDirectorsAsync();
        Task ChangeDirectorAsync(ChangeDirectorDto request);
        Task DeleteDirectorAsync(Guid requsetId);
    }
}
