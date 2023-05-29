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
            var result = await _service.AddGenreAsync(request);
            _logger.Log(LogLevel.Information, $"Genre {request.Name} added");
            return Created(Request.GetEncodedUrl(), result);
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _service.GetGenresAsync();
            _logger.Log(LogLevel.Information, $"All genres retrieved({genres.Count})");
            return Ok(genres);
        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetGenre([Required] Guid requestId)
        {
            var genre = await _service.GetGenreAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} genre retrieved");
            return Ok(genre);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeGenre(ChangeGenreDto request)
        {
            await _service.ChangeGenreAsync(request);
            _logger.Log(LogLevel.Information, "Genre changed");
            return Ok("Genre changed");
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteGenre([Required] Guid requestId)
        {
            await _service.DeleteGenreAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} Genre deleted");
            return NoContent();
        }
    }
}
