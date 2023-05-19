using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Joins;
using DataAccessLayer.Repositories.Interfaces;
using LogicLayer.Dtos;
using LogicLayer.Mappings;
using LogicLayer.Services.Interfaces;

namespace LogicLayer.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IActorRepository _actorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository repository, IActorRepository actorRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _repository = repository;
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<MovieDto> AddMovieAsync(AddMovieDto request)
        {
            var movie = _mapper.Map<Movie>(request);

            List<MovieActor> movieActors = new();
            foreach (var actorId in request.ActorIds)
            {
                movieActors.Add(new MovieActor
                {
                    Actor = await _actorRepository.GetActorAsync(actorId),
                    Movie = movie
                });
            }

            await _repository.AddMovieActorsAsync(movieActors);

            List<MovieGenre> movieGenres = new();
            foreach (var genreId in request.GenreIds)
            {
                movieGenres.Add(new MovieGenre
                {
                    Genre = await _genreRepository.GetGenreAsync(genreId),
                    Movie = movie
                });
            }

            await _repository.AddMovieGenresAsync(movieGenres);

            await _repository.AddMovieAsync(movie);

            var movieDto = _mapper.Map<MovieDto>(movie);
            return movieDto;
        }

        public async Task ChangeMovieAsync(ChangeMovieDto request)
        {
            var movie = _mapper.Map<Movie>(request);

            List<MovieActor> movieActors = new();
            if (request.ActorIds.Count > 0)
            {
                foreach (var actorId in request.ActorIds)
                {
                    movieActors.Add(new MovieActor
                    {
                        Actor = await _actorRepository.GetActorAsync(actorId),
                        Movie = movie
                    });
                }
                foreach (var item in await _repository.GetMovieActorsAsync())
                {
                    if (item.MovieId == movie.Id)
                        _repository.DeleteMovieActorAsync(item);
                }
                await _repository.AddMovieActorsAsync(movieActors);
            }

            List<MovieGenre> movieGenres = new();
            if (request.GenreIds.Count > 0)
            {
                foreach (var genreId in request.GenreIds)
                {
                    movieGenres.Add(new MovieGenre
                    {
                        Genre = await _genreRepository.GetGenreAsync(genreId),
                        Movie = movie
                    });
                }
                foreach (var item in await _repository.GetMovieGenresAsync())
                {
                    if (item.MovieId == movie.Id)
                        _repository.DeleteMovieGenreAsync(item);
                }
                await _repository.AddMovieGenresAsync(movieGenres);
            }

            await _repository.ChangeMovieAsync(movie);
        }

        public async Task DeleteMovieAsync(Guid requestId)
        {
            var movie = await _repository.GetMovieByIdAsync(requestId);

            if (movie == null)
                throw new Exception("Movie does not exits");

            await _repository.DeleteMovie(movie);
        }

        public async Task<MovieDto> GetMovieByIdAsync(Guid requestId)
        {
            var movie = await _repository.GetMovieByIdAsync(requestId);

            if (movie == null)
                throw new Exception("Movie does not exist");

            var movieDto = _mapper.Map<MovieDto>(movie);
            return movieDto;
        }

        public async Task<ICollection<MovieDto>> GetMovieByIMDBAsync(double firstRating, double secondRating)
        {
            if (firstRating < 0 || firstRating > 10)
                throw new Exception("IMDB Rating is out of boundaries");

            if (secondRating < 0 || secondRating > 10)
                throw new Exception("IMDB Rating is out of boundaries");

            double minRating = firstRating > secondRating ? secondRating : firstRating;
            double maxRating = minRating < firstRating ? firstRating : secondRating;


            var movies = (await _repository.GetMoviesAsync()).Where(m => m.IMDBRate >= minRating && m.IMDBRate <= maxRating).ToList();

            if (movies.Count == 0)
                throw new Exception($"Movies not exist");

            var moviesDto = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();
            return moviesDto;
        }

        public async Task<ICollection<MovieDto>> GetMovieByTitleAsync(string requestTitle)
        {
            var movies = (await _repository.GetMoviesAsync()).Where(m => m.Title.ToUpper().Contains(requestTitle.ToUpper())).ToList();

            if (movies.Count == 0)
                throw new Exception("Movies are empty");

            var movieDtos = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();
            return movieDtos;
        }

        public async Task<ICollection<MovieDto>> GetMovieByYearAsync(int firstYear, int secondYear)
        {
            int minYear = firstYear > secondYear ? secondYear : firstYear;
            int maxYear = firstYear == minYear ? secondYear : firstYear;

            var movies = (await _repository.GetMoviesAsync()).Where(m => Convert.ToInt32(m.Date) >= minYear && Convert.ToInt32(m.Date) <= maxYear).ToList();

            if (movies.Count == 0)
                throw new Exception($"Movies not exist");

            var moviesDto = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();
            return moviesDto;
        }

        public async Task<ICollection<MovieDto>> GetMoviesAsync()
        {
            var movies = await _repository.GetMoviesAsync();

            if (movies.Count == 0)
                throw new Exception("Movies are empty");

            var movieDtos = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();
            return movieDtos;
        }
    }
}
