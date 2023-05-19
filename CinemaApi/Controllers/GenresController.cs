using LogicLayer.Dtos;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _service;
        private readonly ILogger<GenresController> _logger;
        public GenresController(IGenreService service, ILogger<GenresController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(AddGenreDto request)
        {
            try
            {
                var result = await _service.AddGenreAsync(request);
                _logger.Log(LogLevel.Information, $"Genre {request.Name} added");
                return Created(Request.GetEncodedUrl(), result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            try
            {
                var genres = await _service.GetGenresAsync();
                _logger.Log(LogLevel.Information, $"All genres retrieved({genres.Count})");
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Genres are empty")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetGenre([Required] Guid requestId)
        {
            try
            {
                var genre = await _service.GetGenreAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} genre retrieved");
                return Ok(genre);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Genre does not exist")
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeGenre(ChangeGenreDto request)
        {
            try
            {
                await _service.ChangeGenreAsync(request);
                _logger.Log(LogLevel.Information, "Genre changed");
                return Ok("Genre changed");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Genre does not exist")
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteGenre([Required] Guid requestId)
        {
            try
            {
                await _service.DeleteGenreAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} Genre deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Genre does not exist")
                    return NotFound(requestId);
                return BadRequest(ex.Message);
            }
        }
    }
}
