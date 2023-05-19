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
            try
            {
                var result = await _service.AddMovieAsync(request);
                _logger.Log(LogLevel.Information, $"Movie {request.Title} added");
                return Created(Request.GetEncodedUrl(), result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            try
            {
                var movies = await _service.GetMoviesAsync();
                _logger.Log(LogLevel.Information, $"All movies retrieved({movies.Count})");
                return Ok(movies);

            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movies are empty")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetMovieById([Required] Guid requestId)
        {
            try
            {
                var movie = await _service.GetMovieByIdAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} movie retrieved");
                return Ok(movie);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movie does not exist")
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("request")]
        public async Task<IActionResult> GetMovieByTitle([Required] string request)
        {
            try
            {
                var movies = await _service.GetMovieByTitleAsync(request);
                _logger.Log(LogLevel.Information, $"{request} movies retrieved");
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movies are empty")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("By-IMDB-Rating")]
        public async Task<IActionResult> GetMovieByIMDBRating([Required] double firstRating, [Required] double secondRating)
        {
            try
            {
                var movies = await _service.GetMovieByIMDBAsync(firstRating, secondRating);
                _logger.Log(LogLevel.Information, $"{firstRating}-{secondRating}IMDB movies retrieved");
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movies not exist")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("By-Date")]
        public async Task<IActionResult> GetMovieByDate([Required] int firstDate, [Required] int secondDate)
        {
            try
            {
                var movies = await _service.GetMovieByYearAsync(firstDate, secondDate);
                _logger.Log(LogLevel.Information, $"{firstDate}-{secondDate} movies retrieved");
                return Ok(movies);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movies not exist")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeMovie(ChangeMovieDto request)
        {
            try
            {
                await _service.ChangeMovieAsync(request);
                _logger.Log(LogLevel.Information, "Movie changed");
                return Ok("Movie changed");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movie does not exist")
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteMovie([Required] Guid requestId)
        {
            try
            {
                await _service.DeleteMovieAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} Movie deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Movie does not exits")
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }
    }
}
