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
            try
            {
                var result = await _service.AddDirectorAsync(request);
                _logger.Log(LogLevel.Information, $"Director {request.FullName} added");
                return Created(Request.GetEncodedUrl(), result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            try
            {
                var directors = await _service.GetDirectorsAsync();
                _logger.Log(LogLevel.Information, $"All directors retrieved({directors.Count})");
                return Ok(directors);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Directors are empty")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetDirector([Required] Guid requestId)
        {
            try
            {
                var director = await _service.GetDirectorAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} director retrieved");
                return Ok(director);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Director does not exist")
                    return NotFound(requestId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeDirector(ChangeDirectorDto request)
        {
            try
            {
                await _service.ChangeDirectorAsync(request);
                _logger.Log(LogLevel.Information, "Director changed");
                return Ok("Director changed");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Director does not exist")
                    return NotFound(request.Id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteDirector([Required] Guid requestId)
        {
            try
            {
                await _service.DeleteDirectorAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} Director deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Director does not exist")
                    return NotFound(requestId);
                return BadRequest(ex.Message);
            }
        }
    }
}