using AutoMapper;
using Cinema.Application.Dtos;
using Cinema.Application.Mappings;
using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.ServiceAbstracts;
using Cinema.Domain.CustomResponse;
using Cinema.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Cinema.Application.ServiceImplementation
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _repository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public DirectorService(IDirectorRepository repository, IMovieRepository movieRepository, IMapper mapper)
        {
            _repository = repository;
            _movieRepository = movieRepository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<ServiceResponse<DirectorDto>> AddDirectorAsync(AddDirectorDto request)
        {
            var response = new ServiceResponse<DirectorDto>();

            var director = _mapper.Map<Director>(request);

            if ((await _repository.GetDirectorsAsync()).Any(d => d.FirstName == director.FirstName && d.LastName == director.LastName && d.BirthDate.Date == director.BirthDate.Date))
            {
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = "Director exists";

                return response;
            }

            if (!ValidDirector(director))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Input is incorrect";

                return response;
            }

            await _repository.AddDirectorAsync(director);

            var directorDto = _mapper.Map<DirectorDto>(director);

            response.StatusCode = HttpStatusCode.Created;
            response.Data = directorDto;

            return response;
        }

        private bool ValidDirector(Director director)
        {
            if (director.FirstName.Any(l => char.IsLetter(l) is false))
                return false;
            if (director.LastName.Any(l => char.IsLetter(l) is false))
                return false;
            if (director.BirthDate.Date > DateTime.UtcNow.Date)
                return false;
            return true;
        }

        public async Task<ServiceResponse<DirectorDto>> ChangeDirectorAsync(ChangeDirectorDto request)
        {
            var response = new ServiceResponse<DirectorDto>();

            if ((await _repository.GetDirectorsAsync()).Any(d => d.Id == request.Id) is false)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Director not found";

                return response;
            }

            var director = _mapper.Map<Director>(request);

            if (!ValidDirector(director))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Input is incorrect";

                return response;
            }

            await _repository.ChangeDirectorAsync(director);

            var directorDto = _mapper.Map<DirectorDto>(director);

            response.StatusCode = HttpStatusCode.OK;
            response.Data = directorDto;

            return response;
        }

        public async Task<ServiceResponse<DirectorDto>> DeleteDirectorAsync(Guid requsetId)
        {
            var response = new ServiceResponse<DirectorDto>();

            var director = await _repository.GetDirectorAsync(requsetId);

            if (director is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Director not found";

                return response;
            }

            await _repository.DeleteDirectorAsync(director);

            response.StatusCode = HttpStatusCode.NoContent;
            response.Data = _mapper.Map<DirectorDto>(director);

            return response;
        }

        public async Task<ServiceResponse<DirectorDto>> GetDirectorAsync(Guid requestId)
        {
            var response = new ServiceResponse<DirectorDto>();

            var director = await _repository.GetDirectorAsync(requestId);

            if (director is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Director not found";

                return response;
            }

            director.Movies = new List<Movie>();
            director.Movies = (await _movieRepository.GetMoviesAsync()).Where(m => m.DirectorId == director.Id).ToList();

            var directorDto = _mapper.Map<DirectorDto>(director);

            List<MoviesForDirectorDto> movies = new List<MoviesForDirectorDto>();
            foreach (var movie in director.Movies)
            {
                movies.Add(new MoviesForDirectorDto
                {
                    Id = movie.Id,
                    Title = movie.Title
                });
            }

            directorDto.Movies = new List<MoviesForDirectorDto>();
            directorDto.Movies = movies;

            response.StatusCode = HttpStatusCode.OK;
            response.Data = directorDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<DirectorDto>>> GetDirectorsAsync()
        {
            var response = new ServiceResponse<ICollection<DirectorDto>>();

            var directors = await _repository.GetDirectorsAsync();

            if (directors.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NoContent;
                response.Message = "Directors are empty";

                return response;
            }

            foreach (var director in directors)
            {
                var movies = (await _movieRepository.GetMoviesAsync()).Where(m => m.Id == director.Id).ToList();

                director.Movies = new List<Movie>();
                director.Movies = movies;
            }

            var directorsDto = directors.Select(d => _mapper.Map<DirectorDto>(d)).ToList();

            for (int i = 0; i < directors.Count; i++)
            {
                var movies = new List<MoviesForDirectorDto>();
                foreach (var movie in directors.ElementAt(i).Movies)
                {
                    movies.Add(new MoviesForDirectorDto
                    {
                        Id = movie.Id,
                        Title = movie.Title
                    });
                }

                directorsDto.ElementAt(i).Movies = new List<MoviesForDirectorDto>();
                directorsDto.ElementAt(i).Movies = movies;
                movies.Clear();
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = directorsDto;

            return response;
        }
    }
}
