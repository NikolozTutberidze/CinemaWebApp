using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;
using DataAccessLayer.Repositories.Implementation;
using DataAccessLayer.Repositories.Interfaces;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.Services.CustomResponse;
using LogicLayer.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace LogicLayer.Services.Implementation
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository actorRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<ServiceResponse<ActorDto>> AddActorAsync(AddActorDto request)
        {
            var response = new ServiceResponse<ActorDto>();

            var actor = _mapper.Map<Actor>(request);

            if ((await _actorRepository.GetActorsAsync()).Any(a => a.FirstName == actor.FirstName && a.LastName == actor.LastName && a.Origin == actor.Origin && a.BirthDate.Date == actor.BirthDate.Date))
            {
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = "Actor exists";

                return response;
            }

            if (!ValidActor(actor))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Input is incorrect";

                return response;
            }

            await _actorRepository.AddActorAsync(actor);

            var actorDto = _mapper.Map<ActorDto>(actor);

            response.StatusCode = HttpStatusCode.Created;
            response.Data = actorDto;

            return response;
        }

        private bool ValidActor(Actor actor)
        {
            if (actor.FirstName.Any(l => char.IsLetter(l) is false))
                return false;
            if (actor.LastName.Any(l => char.IsLetter(l) is false))
                return false;
            if (actor.BirthDate.Date > DateTime.UtcNow.Date)
                return false;
            if (actor.Origin.Any(l => char.IsLetter(l) is false))
                return false;
            return true;
        }

        public async Task<ServiceResponse<ActorDto>> ChangeActorAsync(ChangeActorDto request)
        {
            var response = new ServiceResponse<ActorDto>();

            var actor = _mapper.Map<Actor>(request);

            if ((await _actorRepository.GetActorsAsync()).Any(a => a.Id == actor.Id) is false)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Actor not found";

                return response;
            }

            if (!ValidActor(actor))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = " Input is incorrect";

                return response;
            }

            await _actorRepository.ChangeActorAsync(actor);

            response.StatusCode = HttpStatusCode.OK;
            response.Data = _mapper.Map<ActorDto>(actor);

            return response;
        }

        public async Task<ServiceResponse<ActorDto>> DeleteActorAsync(Guid requestId)
        {
            var response = new ServiceResponse<ActorDto>();

            var actor = await _actorRepository.GetActorAsync(requestId);

            if (actor == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Actor not found";

                return response;
            }

            await _actorRepository.DeleteActorAsync(actor);

            response.StatusCode = HttpStatusCode.NoContent;
            response.Data = _mapper.Map<ActorDto>(actor);

            return response;
        }

        public async Task<ServiceResponse<ActorDto>> GetActorAsync(Guid requestId)
        {
            var response = new ServiceResponse<ActorDto>();

            var actor = await _actorRepository.GetActorAsync(requestId);

            if (actor is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Actor not found";
            }

            actor.MoviesActors = new List<MovieActor>();
            actor.MoviesActors = (await _actorRepository.GetMovieActorsAsync()).Where(ma => ma.ActorId == actor.Id).ToList();

            var actorDto = _mapper.Map<ActorDto>(actor);

            List<MoviesForActorDto> movies = new List<MoviesForActorDto>();
            foreach (var movieActor in actor.MoviesActors)
            {
                movies.Add(new MoviesForActorDto
                {
                    Id = movieActor.MovieId,
                    Title = (await _movieRepository.GetMovieByIdAsync(movieActor.MovieId)).Title
                });
            }

            actorDto.Movies = new();
            actorDto.Movies = movies;

            response.StatusCode = HttpStatusCode.OK;
            response.Data = actorDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<ActorDto>>> GetActorsAsync()
        {
            var response = new ServiceResponse<ICollection<ActorDto>>();

            var actors = await _actorRepository.GetActorsAsync();

            if (actors.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NoContent;
                response.Message = "Actors are empty";

                return response;
            }

            foreach (var actor in actors)
            {
                var moviesActors = (await _actorRepository.GetMovieActorsAsync()).Where(ma => ma.ActorId == actor.Id).ToList();

                actor.MoviesActors = new List<MovieActor>();
                actor.MoviesActors = moviesActors;
            }
            var actorsDto = actors.Select(a => _mapper.Map<ActorDto>(a)).ToList();

            for (int i = 0; i < actors.Count; i++)
            {
                var movies = new List<MoviesForActorDto>();
                foreach (var movieActor in actors.ElementAt(i).MoviesActors)
                {
                    movies.Add(new MoviesForActorDto
                    {
                        Id = movieActor.MovieId,
                        Title = (await _movieRepository.GetMovieByIdAsync(movieActor.MovieId)).Title
                    });
                }

                actorsDto.ElementAt(i).Movies = new List<MoviesForActorDto>();
                actorsDto.ElementAt(i).Movies = movies;

                movies.Clear();
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = actorsDto;

            return response;
        }
    }
}
