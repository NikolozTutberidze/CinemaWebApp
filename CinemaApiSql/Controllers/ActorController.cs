using CinemaApiSql.Dtos;
using CinemaApiSql.Interfaces;
using CinemaApiSql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CinemaApiSql.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorRepository _repository;
        public ActorController(IActorRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddActor(AddActorDto request)
        {
            if (_repository.CheckActorExisting(request.FullName))
                return BadRequest("Actor exists");

            Actor actor = new()
            {
                FullName = request.FullName,
                BirthDate = request.BirthDate,
                Origin = request.Origin
            };

            _repository.AddActor(actor);
            return Ok("Actor added");
        }

        [HttpGet]
        public IActionResult GetActors()
        {
            var actors = _repository.GetActors();
            if (actors.Count == 0)
                return BadRequest("There are not actors");
            return Ok(actors);
        }

        [HttpGet]
        [Route("requestId")]
        public IActionResult GetActor([Required] Guid requestId)
        {
            if (!_repository.CheckActorExisting(requestId))
                return BadRequest("Actor not exists");
            var actor = _repository.GetActor(requestId);
            return Ok(actor);
        }

        [HttpPut]
        [Route("requestId")]
        public IActionResult ChangeActor(ChangeActorDto request)
        {
            if (!_repository.CheckActorExisting(request.Id))
                return BadRequest("Actor not exists");
            _repository.ChangeActor(request);
            return Ok("Actor changed");
        }

        [HttpDelete]
        [Route("requestId")]
        public IActionResult DeleteActor([Required] Guid requestId)
        {
            if (!_repository.CheckActorExisting(requestId))
                return BadRequest("Actor not exists");
            _repository.DeleteActor(requestId);
            return Ok("Actor deleted");
        }
    }
}
