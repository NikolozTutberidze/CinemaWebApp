using CinemaApiSql.Dtos;
using CinemaApiSql.Interfaces;
using CinemaApiSql.Models;
using CinemaApiSql.Models.Joins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _repository;
        public MovieController(IMovieRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddMovie(AddMovieDto request)
        {
            if (_repository.CheckMovieExisting(request.Title))
                return BadRequest("Movie exists");

            _repository.AddMovie(request);
            return Ok("Movie added");
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = _repository.GetMovies();
            if (movies.Count == 0)
                return BadRequest("Movies are empty");
            return Ok(movies);
        }

        [HttpGet]
        [Route("requestId")]
        public IActionResult GetMovieById([Required] Guid requestId)
        {
            if (!_repository.CheckMovieExisting(requestId))
                return BadRequest("Movie not exists");
            var movie = _repository.GetMovieById(requestId);
            return Ok(movie);
        }

        [HttpGet]
        public IActionResult GetMovieByTitle([Required] string request)
        {
            var movies = _repository.GetMovieByTitle(request);
            if (movies.Count == 0)
                return BadRequest("Nothing found");
            return Ok(movies);
        }

        [HttpGet]
        public IActionResult GetMovieByIMDBRating([Required] double firstRating, [Required] double secondRating)
        {
            if (firstRating < 0 || firstRating > 10)
                return BadRequest("IMDB rating is from 0 to 10");
            if (secondRating < 0 || secondRating > 10)
                return BadRequest("IMDB rating is from 0 to 10");
            var movies = _repository.GetMovieByIMDB(firstRating, secondRating);
            return Ok(movies);
        }

        [HttpGet]
        public IActionResult GetMovieByDate([Required] int firstDate, [Required] int secondDate)
        {
            int minDateDb = _repository.MinimumMovieDate();
            int maxDateDb = _repository.MaximumMovieDate();
            if (firstDate < minDateDb || firstDate > maxDateDb)
                return BadRequest($"Movie date must be from {minDateDb} to {maxDateDb}");
            if (secondDate < minDateDb || secondDate > maxDateDb)
                return BadRequest($"Movie date must be from {minDateDb} to {maxDateDb}");
            var movies = _repository.GetMovieByYear(firstDate, secondDate);
            return Ok(movies);
        }

        [HttpPut]
        public IActionResult ChangeMovie(ChangeMovieDto request)
        {
            if (!_repository.CheckMovieExisting(request.Id))
                return BadRequest("Movie not exists");

            _repository.ChangeMovie(request);
            return Ok("Movie changed");
        }

        [HttpDelete]
        [Route("requestId")]
        public IActionResult DeleteMovie([Required] Guid requestId)
        {
            if (!_repository.CheckMovieExisting(requestId))
                return BadRequest("Movie not exists");
            _repository.DeleteMovie(requestId);
            return Ok("Movie deleted");
        }
    }
}
