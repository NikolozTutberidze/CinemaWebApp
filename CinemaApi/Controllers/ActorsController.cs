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
            var result = await _service.AddActorAsync(request);
            _logger.Log(LogLevel.Information, $"Actor {request.FullName} added");
            return Created(Request.GetEncodedUrl(), result);
        }

        [HttpGet]
        public async Task<IActionResult> GetActors()
        {
            var actors = await _service.GetActorsAsync();
            _logger.Log(LogLevel.Information, $"All actors retrieved({actors.Count})");
            return Ok(actors);
        }

        [HttpGet, Route("id")]
        public async Task<IActionResult> GetActor([Required] Guid id)
        {
            var actor = await _service.GetActorAsync(id);
            _logger.Log(LogLevel.Information, $"{id} actor retrieved");
            return Ok(actor);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeActor(ChangeActorDto request)
        {
            await _service.ChangeActorAsync(request);
            _logger.Log(LogLevel.Information, "Actor changed");
            return Ok("Actor changed");
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteActor([Required] Guid requestId)
        {
            await _service.DeleteActorAsync(requestId);
            _logger.Log(LogLevel.Information, $"{requestId} Actor deleted");
            return NoContent();
        }
    }
}
