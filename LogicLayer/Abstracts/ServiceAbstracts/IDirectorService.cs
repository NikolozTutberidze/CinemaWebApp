using Cinema.Domain.CustomResponse;

namespace Cinema.Domain.Abstracts.ServiceAbstracts
{
    public interface IDirectorService
    {
        Task<ServiceResponse<DirectorDto>> AddDirectorAsync(AddDirectorDto request);
        Task<ServiceResponse<DirectorDto>> GetDirectorAsync(Guid requestId);
        Task<ServiceResponse<ICollection<DirectorDto>>> GetDirectorsAsync();
        Task<ServiceResponse<DirectorDto>> ChangeDirectorAsync(ChangeDirectorDto request);
        Task<ServiceResponse<DirectorDto>> DeleteDirectorAsync(Guid requsetId);
    }
}
