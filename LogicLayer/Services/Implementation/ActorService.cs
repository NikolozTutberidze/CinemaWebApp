using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.Services.Interfaces;

namespace LogicLayer.Services.Implementation
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _repository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<ActorDto> AddActorAsync(AddActorDto request)
        {
            var actor = _mapper.Map<Actor>(request);

            await _repository.AddActorAsync(actor);

            var actorDto = _mapper.Map<ActorDto>(actor);
            return actorDto;
        }

        public async Task ChangeActorAsync(ChangeActorDto request)
        {
            var actor = _mapper.Map<Actor>(request);
            await _repository.ChangeActorAsync(actor);
        }

        public async Task DeleteActorAsync(Guid requestId)
        {
            var actor = await _repository.GetActorAsync(requestId);

            if (actor == null)
                throw new Exception("Actor does not exist");

            await _repository.DeleteActorAsync(actor);
        }

        public async Task<ActorDto> GetActorAsync(Guid requestId)
        {
            var actor = await _repository.GetActorAsync(requestId);

            if (actor == null)
                throw new Exception("Actor does not exist");

            var actorDto = _mapper.Map<ActorDto>(actor);

            List<MoviesForActorDto> movies = new List<MoviesForActorDto>();
            foreach (var item in await _repository.GetMovieActorsAsync())
            {
                if (item.ActorId == actor.Id)
                {
                    movies.Add(new MoviesForActorDto
                    {
                        Id = item.MovieId,
                        Title = item.Movie.Title
                    });
                }
            }

            actorDto.Movies = movies;

            return actorDto;
        }

        public async Task<ICollection<ActorDto>> GetActorsAsync()
        {
            var actors = await _repository.GetActorsAsync();

            if (actors.Count == 0)
                throw new Exception("Actors are empty");

            var actorsDto = actors.Select(a => _mapper.Map<ActorDto>(a)).ToList();

            foreach (var actor in actorsDto)
            {
                List<MoviesForActorDto> movies = new List<MoviesForActorDto>();
                foreach (var item in await _repository.GetMovieActorsAsync())
                {
                    if (item.ActorId == actor.Id)
                    {
                        movies.Add(new MoviesForActorDto
                        {
                            Id = item.MovieId,
                            Title = item.Movie.Title,
                        });
                    }
                }
                actor.Movies = movies;
            }

            return actorsDto;
        }
    }
}
