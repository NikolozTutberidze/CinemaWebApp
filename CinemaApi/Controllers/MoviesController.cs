using LogicLayer.Dtos;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;
        private readonly ILogger<MoviesController> _logger;
        public MoviesController(IMovieService service, ILogger<MoviesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieDto request)
        {
            var result = await _service.AddMovieAsync(request);
            _logger.Log(LogLevel.Information, $"Movie {request.Title} added");
            return Created(Request.GetEncodedUrl(), result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _service.GetMoviesAsync();
            _logger.Log(LogLevel.Information, $"All movies retrieved({movies.Count})");
            return Ok(movies);

        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetMovieById([Required] Guid requestId)
        {
            var movie = await _service.GetMovieByIdAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} movie retrieved");
            return Ok(movie);
        }

        [HttpGet, Route("request")]
        public async Task<IActionResult> GetMovieByTitle([Required] string request)
        {
            var movies = await _service.GetMovieByTitleAsync(request);
            _logger.Log(LogLevel.Information, $"{request} movies retrieved");
            return Ok(movies);
        }

        [HttpGet, Route("By-IMDB-Rating")]
        public async Task<IActionResult> GetMovieByIMDBRating([Required] double firstRating, [Required] double secondRating)
        {
            var movies = await _service.GetMovieByIMDBAsync(firstRating, secondRating);
            _logger.Log(LogLevel.Information, $"{firstRating}-{secondRating}IMDB movies retrieved");
            return Ok(movies);
        }

        [HttpGet, Route("By-Date")]
        public async Task<IActionResult> GetMovieByDate([Required] int firstDate, [Required] int secondDate)
        {
            var movies = await _service.GetMovieByYearAsync(firstDate, secondDate);
            _logger.Log(LogLevel.Information, $"{firstDate}-{secondDate} movies retrieved");
            return Ok(movies);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeMovie(ChangeMovieDto request)
        {
            await _service.ChangeMovieAsync(request);
            _logger.Log(LogLevel.Information, "Movie changed");
            return Ok("Movie changed");
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteMovie([Required] Guid requestId)
        {
            await _service.DeleteMovieAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} Movie deleted");
            return NoContent();
        }
    }
}
