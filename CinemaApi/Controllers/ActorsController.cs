using LogicLayer.Dtos;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _service;
        private readonly ILogger<ActorsController> _logger;
        public ActorsController(IActorService service, ILogger<ActorsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddActor(AddActorDto request)
        {
            try
            {
                var result = await _service.AddActorAsync(request);
                _logger.Log(LogLevel.Information, $"Actor {request.FullName} added");
                return Created(Request.GetEncodedUrl(), result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetActors()
        {
            try
            {
                var actors = await _service.GetActorsAsync();
                _logger.Log(LogLevel.Information, $"All actors retrieved({actors.Count})");
                return Ok(actors);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                if (ex.Message == "Actors are empty")
                    return NoContent();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("id")]
        public async Task<IActionResult> GetActor([Required] Guid id)
        {
            try
            {
                var actor = await _service.GetActorAsync(id);
                _logger.Log(LogLevel.Information, $"{id} actor retrieved");
                return Ok(actor);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                if (ex.Message == "Actor does not exist")
                    return NotFound(id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeActor(ChangeActorDto request)
        {
            try
            {
                await _service.ChangeActorAsync(request);
                _logger.Log(LogLevel.Information, "Actor changed");
                return Ok("Actor changed");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                if (ex.Message == "Actor does not exist")
                    return NotFound(request.Id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteActor([Required] Guid requestId)
        {
            try
            {
                await _service.DeleteActorAsync(requestId);
                _logger.Log(LogLevel.Information, $"{requestId} Actor deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                if (ex.Message == "Actor does not exist")
                    return NotFound(requestId);
                return BadRequest(ex.Message);
            }
        }
    }
}
