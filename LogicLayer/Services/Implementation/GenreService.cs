using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.Services.Interfaces;

namespace LogicLayer.Services.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<GenreDto> AddGenreAsync(AddGenreDto request)
        {
            var genre = _mapper.Map<Genre>(request);

            await _repository.AddGenreAsync(genre);

            var genreDto = _mapper.Map<GenreDto>(genre);
            return genreDto;
        }

        public async Task ChangeGenreAsync(ChangeGenreDto request)
        {
            var genre = _mapper.Map<Genre>(request);

            await _repository.ChangeGenreAsync(genre);
        }

        public async Task DeleteGenreAsync(Guid requestId)
        {
            var genre = await _repository.GetGenreAsync(requestId);

            await _repository.DeleteGenreAsync(genre);
        }

        public async Task<GenreDto> GetGenreAsync(Guid requestId)
        {
            var genre = await _repository.GetGenreAsync(requestId);

            if (genre == null)
                throw new Exception("Genre does not exist");

            var genreDto = _mapper.Map<GenreDto>(genre);

            List<MoviesForGenreDto> movies = new List<MoviesForGenreDto>();
            foreach (var item in await _repository.GetMovieGenresAsync())
            {
                if (item.GenreId == genreDto.Id)
                {
                    movies.Add(new MoviesForGenreDto
                    {
                        Id = item.MovieId,
                        Title = item.Movie.Title
                    });
                }
            }

            genreDto.Movies = movies;

            return genreDto;
        }

        public async Task<ICollection<GenreDto>> GetGenresAsync()
        {
            var genres = await _repository.GetGenresAsync();

            if (genres.Count == 0)
                throw new Exception("Genres are empty");

            var genresDto = genres.Select(g => _mapper.Map<GenreDto>(g)).ToList();

            foreach (var genre in genresDto)
            {
                List<MoviesForGenreDto> movies = new List<MoviesForGenreDto>();
                foreach (var item in await _repository.GetMovieGenresAsync())
                {
                    if (item.GenreId == genre.Id)
                    {
                        movies.Add(new MoviesForGenreDto()
                        {
                            Id = item.MovieId,
                            Title = item.Movie.Title
                        });
                    }
                }

                genre.Movies = movies;
            }

            return genresDto;
        }
    }
}
