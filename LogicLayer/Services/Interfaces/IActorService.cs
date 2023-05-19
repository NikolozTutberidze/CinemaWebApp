using LogicLayer.Dtos;

namespace LogicLayer.Services.Interfaces
{
    public interface IActorService
    {
        Task<ActorDto> AddActorAsync(AddActorDto request);
        Task<ActorDto> GetActorAsync(Guid requestId);
        Task<ICollection<ActorDto>> GetActorsAsync();
        Task ChangeActorAsync(ChangeActorDto request);
        Task DeleteActorAsync(Guid requestId);
    }
}
