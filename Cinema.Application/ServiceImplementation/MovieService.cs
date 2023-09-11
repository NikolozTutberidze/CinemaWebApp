using AutoMapper;
using Cinema.Application.Dtos;
using Cinema.Application.Mappings;
using Cinema.Domain.Abstracts.RepositoryAbstracts;
using Cinema.Domain.Abstracts.ServiceAbstracts;
using Cinema.Domain.CustomResponse;
using Cinema.Domain.Models;
using Cinema.Domain.Models.Joins;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Cinema.Application.ServiceImplementation
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IActorRepository _actorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;

        public MovieService
            (
            IMovieRepository repository,
            IActorRepository actorRepository,
            IGenreRepository genreRepository,
            IDirectorRepository directorRepository,
            IMapper mapper
            )
        {
            _repository = repository;
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _directorRepository = directorRepository;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = new Mapper(config);
        }

        public async Task<ServiceResponse<MovieDto>> AddMovieAsync(AddMovieDto request)
        {
            var response = new ServiceResponse<MovieDto>();

            var movie = _mapper.Map<Movie>(request);
            movie.Id = Guid.NewGuid();

            movie.MoviesActors = new();
            foreach (var actorId in request.ActorIds)
            {
                movie.MoviesActors.Add(new MovieActor
                {
                    ActorId = actorId,
                    MovieId = movie.Id
                });
            }

            movie.MoviesGenres = new();
            foreach (var genreId in request.GenreIds)
            {
                movie.MoviesGenres.Add(new MovieGenre
                {
                    GenreId = genreId,
                    MovieId = movie.Id
                });
            }

            if ((await _repository.GetMoviesAsync()).Any(m => m.Title == movie.Title && m.DirectorId == movie.DirectorId))
            {
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = "Movie exists";

                return response;
            }

            if (!(await ValidMovie(movie)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Incorrect input";

                return response;
            }

            var movieActors = new List<MovieActor>();
            foreach (var movieActor in movie.MoviesActors)
            {
                movieActors.Add(movieActor);
            }
            _repository.AddMovieActorsAsync(movieActors);


            List<MovieGenre> movieGenres = new();
            foreach (var movieGenre in movie.MoviesGenres)
            {
                movieGenres.Add(movieGenre);
            }
            _repository.AddMovieGenresAsync(movieGenres);

            await _repository.AddMovieAsync(movie);

            var movieDto = _mapper.Map<MovieDto>(movie);

            movieDto.Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movie.DirectorId));

            movieDto.Actors = new();
            foreach (var movieActor in movie.MoviesActors)
            {
                var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                movieDto.Actors.Add(new ActorsForMovieDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    BirthDate = actor.BirthDate,
                    Origin = actor.Origin
                });
            }

            movieDto.Genres = new();
            foreach (var movieGenre in movie.MoviesGenres)
            {
                var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                movieDto.Genres.Add(new GenresForMovieDto
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            response.StatusCode = HttpStatusCode.Created;
            response.Data = movieDto;

            return response;
        }

        private async Task<bool> ValidMovie(Movie movie)
        {

            if (movie.Title.Trim().Any(s => char.IsLetterOrDigit(s) is false))
                return false;

            if (movie.Date.Any(n => char.IsDigit(n) is false))
                return false;

            if (Convert.ToInt32(movie.Date.Trim()) > DateTime.UtcNow.Date.Year)
                return false;

            if (movie.IMDBRate < 0 || movie.IMDBRate > 10)
                return false;

            if (movie.Review.Length > 50)
                return false;

            if ((await _directorRepository.GetDirectorsAsync()).Any(d => d.Id == movie.DirectorId) is false)
                return false;

            foreach (var movieActor in movie.MoviesActors)
            {
                if ((await _actorRepository.GetActorsAsync()).Any(a => a.Id == movieActor.ActorId) is false)
                    return false;
            }

            foreach (var movieGenre in movie.MoviesGenres)
            {
                if ((await _genreRepository.GetGenresAsync()).Any(g => g.Id == movieGenre.GenreId) is false)
                    return false;
            }

            return true;
        }

        public async Task<ServiceResponse<MovieDto>> ChangeMovieAsync(ChangeMovieDto request)
        {
            var response = new ServiceResponse<MovieDto>();

            var movie = _mapper.Map<Movie>(request);

            if (request.ActorIds is not null)
            {
                movie.MoviesActors = new();
                foreach (var actorId in request.ActorIds)
                {
                    movie.MoviesActors.Add(new MovieActor
                    {
                        ActorId = actorId,
                        MovieId = movie.Id
                    });
                }
            }
            else movie.MoviesActors = null;

            if (request.GenreIds is not null)
            {
                movie.MoviesGenres = new();
                foreach (var genreId in request.GenreIds)
                {
                    movie.MoviesGenres.Add(new MovieGenre
                    {
                        GenreId = genreId,
                        MovieId = movie.Id
                    });
                }
            }
            else movie.MoviesGenres = null;

            if ((await _repository.GetMoviesAsync()).Any(m => m.Id == movie.Id) is false)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Movie not found";

                return response;
            }

            if (!(await ValidMovie(request)))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Incorrect input";

                return response;
            }

            if (movie.MoviesActors is not null)
            {
                var movieActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

                _repository.DeleteMovieActorsAsync(movieActors);

                _repository.AddMovieActorsAsync(movie.MoviesActors);
            }

            if (movie.MoviesGenres is not null)
            {
                var movieGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();

                _repository.DeleteMovieGenresAsync(movieGenres);

                _repository.AddMovieGenresAsync(movie.MoviesGenres);
            }

            await _repository.ChangeMovieAsync(movie);

            var movieDto = _mapper.Map<MovieDto>(movie);

            movieDto.Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movie.DirectorId));

            movieDto.Actors = new();
            foreach (var movieActor in movie.MoviesActors)
            {
                var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                movieDto.Actors.Add(new ActorsForMovieDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    BirthDate = actor.BirthDate,
                    Origin = actor.Origin
                });
            }

            movieDto.Genres = new();
            foreach (var movieGenre in movie.MoviesGenres)
            {
                var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                movieDto.Genres.Add(new GenresForMovieDto
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = movieDto;

            return response;
        }

        private async Task<bool> ValidMovie(ChangeMovieDto movie)
        {
            if (movie.Title is not null)
                if (movie.Title.Trim().Any(s => char.IsLetterOrDigit(s) is false))
                    return false;

            if (movie.Date is not null)
            {
                if (movie.Date.Any(n => char.IsDigit(n) is false))
                    return false;

                if (Convert.ToInt32(movie.Date.Trim()) > DateTime.UtcNow.Date.Year)
                    return false;
            }

            if (movie.IMDBRate is not null)
                if (movie.IMDBRate < 0 || movie.IMDBRate > 10)
                    return false;

            if (movie.Review is not null)
                if (movie.Review.Length > 50)
                    return false;

            if (movie.DirectorId is not null)
                if ((await _directorRepository.GetDirectorsAsync()).Any(d => d.Id == movie.DirectorId) is false)
                    return false;

            if (movie.ActorIds is not null)
                foreach (var actorId in movie.ActorIds)
                {
                    if ((await _actorRepository.GetActorsAsync()).Any(a => a.Id == actorId) is false)
                        return false;
                }

            if (movie.GenreIds is not null)
                foreach (var genreId in movie.GenreIds)
                {
                    if ((await _genreRepository.GetGenresAsync()).Any(g => g.Id == genreId) is false)
                        return false;
                }

            return true;
        }

        public async Task<ServiceResponse<MovieDto>> DeleteMovieAsync(Guid requestId)
        {
            var response = new ServiceResponse<MovieDto>();

            var movie = await _repository.GetMovieByIdAsync(requestId);

            if (movie == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Movie not found";

                return response;
            }

            movie.MoviesActors = new();
            movie.MoviesActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

            movie.MoviesGenres = new();
            movie.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();

            await _repository.DeleteMovie(movie);

            var movieDto = _mapper.Map<MovieDto>(movie);

            movieDto.Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movie.DirectorId));

            movieDto.Actors = new();
            foreach (var movieActor in movie.MoviesActors)
            {
                var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                movieDto.Actors.Add(new ActorsForMovieDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    BirthDate = actor.BirthDate,
                    Origin = actor.Origin
                });
            }

            movieDto.Genres = new();
            foreach (var movieGenre in movie.MoviesGenres)
            {
                var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                movieDto.Genres.Add(new GenresForMovieDto
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            response.StatusCode = HttpStatusCode.NoContent;
            response.Data = movieDto;

            return response;
        }

        public async Task<ServiceResponse<MovieDto>> GetMovieByIdAsync(Guid requestId)
        {
            var response = new ServiceResponse<MovieDto>();

            var movie = await _repository.GetMovieByIdAsync(requestId);

            if (movie == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Movie not found";

                return response;
            }

            movie.MoviesActors = new();
            movie.MoviesActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

            movie.MoviesGenres = new();
            movie.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();

            var movieDto = _mapper.Map<MovieDto>(movie);

            movieDto.Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movie.DirectorId));

            movieDto.Actors = new();
            foreach (var movieActor in movie.MoviesActors)
            {
                var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                movieDto.Actors.Add(new ActorsForMovieDto
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    BirthDate = actor.BirthDate,
                    Origin = actor.Origin
                });
            }

            movieDto.Genres = new();
            foreach (var movieGenre in movie.MoviesGenres)
            {
                var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                movieDto.Genres.Add(new GenresForMovieDto
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = movieDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesByIMDBAsync(double firstRating, double secondRating)
        {
            var response = new ServiceResponse<ICollection<MovieDto>>();

            double minRating = firstRating > secondRating ? secondRating : firstRating;
            double maxRating = minRating < firstRating ? firstRating : secondRating;

            if (minRating < 0 || maxRating > 10)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "IMDB Rating is out of boundaries";

                return response;
            }

            var movies = (await _repository.GetMoviesAsync()).Where(m => m.IMDBRate >= minRating && m.IMDBRate <= maxRating).ToList();

            if (movies.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Movie not found";

                return response;
            }

            foreach (var movie in movies)
            {
                movie.MoviesActors = new();
                movie.MoviesActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

                movie.MoviesGenres = new();
                movie.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();
            }

            var moviesDto = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();

            for (int i = 0; i < movies.Count; i++)
            {
                moviesDto.ElementAt(i).Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movies.ElementAt(i).DirectorId));

                moviesDto.ElementAt(i).Actors = new();
                foreach (var movieActor in movies.ElementAt(i).MoviesActors)
                {
                    var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                    moviesDto.ElementAt(i).Actors.Add(new ActorsForMovieDto
                    {
                        Id = actor.Id,
                        FirstName = actor.FirstName,
                        LastName = actor.LastName,
                        BirthDate = actor.BirthDate,
                        Origin = actor.Origin
                    });
                }

                moviesDto.ElementAt(i).Genres = new();
                foreach (var movieGenre in movies.ElementAt(i).MoviesGenres)
                {
                    var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                    moviesDto.ElementAt(i).Genres.Add(new GenresForMovieDto
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    });
                }
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = moviesDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesByTitleAsync(string requestTitle)
        {
            var response = new ServiceResponse<ICollection<MovieDto>>();

            var movies = (await _repository.GetMoviesAsync()).Where(m => m.Title.ToUpper().Contains(requestTitle.ToUpper())).ToList();

            if (movies.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Movie not found";

                return response;
            }

            foreach (var movie in movies)
            {
                movie.MoviesActors = new();
                movie.MoviesActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

                movie.MoviesGenres = new();
                movie.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();
            }

            var moviesDto = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();

            for (int i = 0; i < movies.Count; i++)
            {
                moviesDto.ElementAt(i).Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movies.ElementAt(i).DirectorId));

                moviesDto.ElementAt(i).Actors = new();
                foreach (var movieActor in movies.ElementAt(i).MoviesActors)
                {
                    var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                    moviesDto.ElementAt(i).Actors.Add(new ActorsForMovieDto
                    {
                        Id = actor.Id,
                        FirstName = actor.FirstName,
                        LastName = actor.LastName,
                        BirthDate = actor.BirthDate,
                        Origin = actor.Origin
                    });
                }

                moviesDto.ElementAt(i).Genres = new();
                foreach (var movieGenre in movies.ElementAt(i).MoviesGenres)
                {
                    var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                    moviesDto.ElementAt(i).Genres.Add(new GenresForMovieDto
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    });
                }
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = moviesDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesByYearAsync(int firstYear, int secondYear)
        {
            var response = new ServiceResponse<ICollection<MovieDto>>();

            int minYear = firstYear > secondYear ? secondYear : firstYear;
            int maxYear = firstYear == minYear ? secondYear : firstYear;

            var movies = (await _repository.GetMoviesAsync()).Where(m => Convert.ToInt32(m.Date) >= minYear && Convert.ToInt32(m.Date) <= maxYear).ToList();

            if (movies.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Movie not found";

                return response;
            }

            foreach (var movie in movies)
            {
                movie.MoviesActors = new();
                movie.MoviesActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

                movie.MoviesGenres = new();
                movie.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();
            }

            var moviesDto = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();

            for (int i = 0; i < movies.Count; i++)
            {
                moviesDto.ElementAt(i).Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movies.ElementAt(i).DirectorId));

                moviesDto.ElementAt(i).Actors = new();
                foreach (var movieActor in movies.ElementAt(i).MoviesActors)
                {
                    var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                    moviesDto.ElementAt(i).Actors.Add(new ActorsForMovieDto
                    {
                        Id = actor.Id,
                        FirstName = actor.FirstName,
                        LastName = actor.LastName,
                        BirthDate = actor.BirthDate,
                        Origin = actor.Origin
                    });
                }

                moviesDto.ElementAt(i).Genres = new();
                foreach (var movieGenre in movies.ElementAt(i).MoviesGenres)
                {
                    var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                    moviesDto.ElementAt(i).Genres.Add(new GenresForMovieDto
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    });
                }
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = moviesDto;

            return response;
        }

        public async Task<ServiceResponse<ICollection<MovieDto>>> GetMoviesAsync()
        {
            var response = new ServiceResponse<ICollection<MovieDto>>();

            var movies = await _repository.GetMoviesAsync();

            if (movies.IsNullOrEmpty())
            {
                response.StatusCode = HttpStatusCode.NoContent;
                response.Message = "Movies are empty";

                return response;
            }

            foreach (var movie in movies)
            {
                movie.MoviesActors = new();
                movie.MoviesActors = (await _repository.GetMovieActorsAsync()).Where(ma => ma.MovieId == movie.Id).ToList();

                movie.MoviesGenres = new();
                movie.MoviesGenres = (await _repository.GetMovieGenresAsync()).Where(mg => mg.MovieId == movie.Id).ToList();
            }

            var moviesDto = movies.Select(m => _mapper.Map<MovieDto>(m)).ToList();

            for (int i = 0; i < movies.Count; i++)
            {
                moviesDto.ElementAt(i).Director = _mapper.Map<DirectorDtoForMovies>(await _directorRepository.GetDirectorAsync(movies.ElementAt(i).DirectorId));

                moviesDto.ElementAt(i).Actors = new();
                foreach (var movieActor in movies.ElementAt(i).MoviesActors)
                {
                    var actor = await _actorRepository.GetActorAsync(movieActor.ActorId);

                    moviesDto.ElementAt(i).Actors.Add(new ActorsForMovieDto
                    {
                        Id = actor.Id,
                        FirstName = actor.FirstName,
                        LastName = actor.LastName,
                        BirthDate = actor.BirthDate,
                        Origin = actor.Origin
                    });
                }

                moviesDto.ElementAt(i).Genres = new();
                foreach (var movieGenre in movies.ElementAt(i).MoviesGenres)
                {
                    var genre = await _genreRepository.GetGenreAsync(movieGenre.GenreId);

                    moviesDto.ElementAt(i).Genres.Add(new GenresForMovieDto
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    });
                }
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Data = moviesDto;

            return response;
        }
    }
}
