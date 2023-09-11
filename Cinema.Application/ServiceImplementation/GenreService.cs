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
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository repository, IMovieRepository movieRepository, IMapper mapper)
        {
            _repository = repository;
            _movieRepository = movieRepository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<ServiceResponse<GenreDto>> AddGenreAsync(AddGenreDto request)
        {
            var response = new ServiceResponse<GenreDto>();

            var genre = _mapper.Map<Genre>(request);

            if ((await _repository.GetGenresAsync()).Any(g => g.Name == genre.Name))
            {
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = "Genre exists";

                return response;
            }

            if (!ValidGenre(genre))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Input incorrect";

                return response;
            }

            await _repository.AddGenreAsync(genre);

            var genreDto = _mapper.Map<GenreDto>(genre);

            response.StatusCode = HttpStatusCode.Created;
            response.Data = genreDto;

            return response;
        }

        private bool ValidGenre(Genre genre)
        {
            if (genre.Name.Any(l => char.IsLetter(l) is false))
                return false;
            return true;
        }

        public async Task<ServiceResponse<GenreDto>> ChangeGenreAsync(ChangeGenreDto request)
        {
            var response = new ServiceResponse<GenreDto>();

            var genre = _mapper.Map<Genre>(request);

            if ((await _repository.GetGenresAsync()).Any(g => g.Id == genre.Id) is false)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Genre not found";

                return response;
            }

            if (!ValidGenre(genre))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Input is incorrect";

                return response;
            }

            await _repository.ChangeGenreAsync(genre);

            response.StatusCode = HttpStatusCode.OK;
            response.Data = _mapper.Map<GenreDto>(genre);

            return response;
        }

        public async Task<ServiceResponse<GenreDto>> DeleteGenreAsync(Guid requestId)
        {
            var response = new ServiceResponse<GenreDto>();

            var genre = await _repository.GetGenreAsync(requestId);

            if (genre is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Genre not found";

                return response;
            }

            await _repository.DeleteGenreAsync(genre);

            response.StatusCode = HttpStatusCode.NoContent;
            response.Data = _mapper.Map<GenreDto>(genre);

            return response;
        }

        public async Task<ServiceResponse<GenreDto>> GetGenreAsync(Guid requestId)
        {
            var response = new ServiceResponse<GenreDto>();

            var genre = await _repository.GetGenreAsync(requestId);

            if (genre == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Genre not found";

                return response;
            }

            genre.MoviesGenres = new List<MovieGenre>();
            genre.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.GenreId == genre.Id).ToList();

            var genreDto = _mapper.Map<GenreDto>(genre);

            List<MoviesForGenreDto> movies = new();
            foreach (var movieGenre in genre.MoviesGenres)
            {
                movies.Add(new MoviesForGenreDto
                {
                    Id = movieGenre.MovieId,
                    Title = (await _movieRepository.GetMovieByIdAsync(movieGenre.MovieId)).Title
                });
            }

            genreDto.Movies = new();
            genreDto.Movies = movies;

            response.StatusCode = HttpStatusCode.OK;
            response.Data = genreDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<GenreDto>>> GetGenresAsync()
        {
            var response = new ServiceResponse<ICollection<GenreDto>>();

            var genres = await _repository.GetGenresAsync();

            if (genres.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NoContent;
                response.Message = "Genres are empty";

                return response;
            }

            foreach (var genre in genres)
            {
                var movieGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.GenreId == genre.Id).ToList();

                genre.MoviesGenres = new();
                genre.MoviesGenres = movieGenres;
            }

            var genresDto = genres.Select(g => _mapper.Map<GenreDto>(g)).ToList();

            for (int i = 0; i < genres.Count; i++)
            {
                List<MoviesForGenreDto> movies = new();

                foreach (var movieGenre in genres.ElementAt(i).MoviesGenres)
                {
                    movies.Add(new MoviesForGenreDto
                    {
                        Id = movieGenre.MovieId,
                        Title = (await _movieRepository.GetMovieByIdAsync(movieGenre.MovieId)).Title
                    });
                }

                genresDto.ElementAt(i).Movies = new();
                genresDto.ElementAt(i).Movies = movies;

                movies.Clear();
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = genresDto;

            return response;
        }
    }
}
