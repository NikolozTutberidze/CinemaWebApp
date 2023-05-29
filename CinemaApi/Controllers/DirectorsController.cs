using LogicLayer.Dtos;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _service;
        private readonly ILogger<DirectorsController> _logger;
        public DirectorsController(IDirectorService service, ILogger<DirectorsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AddDirector(AddDirectorDto request)
        {
            var result = await _service.AddDirectorAsync(request);
            _logger.Log(LogLevel.Information, $"Director {request.FullName} added");
            return Created(Request.GetEncodedUrl(), result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            var directors = await _service.GetDirectorsAsync();
            _logger.Log(LogLevel.Information, $"All directors retrieved({directors.Count})");
            return Ok(directors);
        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetDirector([Required] Guid requestId)
        {
            var director = await _service.GetDirectorAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} director retrieved");
            return Ok(director);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeDirector(ChangeDirectorDto request)
        {
            await _service.ChangeDirectorAsync(request);
            _logger.Log(LogLevel.Information, "Director changed");
            return Ok("Director changed");
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteDirector([Required] Guid requestId)
        {
            await _service.DeleteDirectorAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} Director deleted");
            return NoContent();
        }
    }
}