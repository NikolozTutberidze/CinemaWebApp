using Cinema.Domain.CustomResponse;
using Cinema.Domain.Dtos;

namespace Cinema.Domain.Abstracts.ServiceAbstracts
{
    public interface IActorService
    {
        Task<ServiceResponse<ActorDto>> AddActorAsync(AddActorDto request);
        Task<ServiceResponse<ActorDto>> GetActorAsync(Guid requestId);
        Task<ServiceResponse<ICollection<ActorDto>>> GetActorsAsync();
        Task<ServiceResponse<ActorDto>> ChangeActorAsync(ChangeActorDto request);
        Task<ServiceResponse<ActorDto>> DeleteActorAsync(Guid requestId);
    }
}
