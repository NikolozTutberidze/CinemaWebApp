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
    public class DirectorController : ControllerBase
    {
        private readonly IDirectorRepository _repository;
        public DirectorController(IDirectorRepository repository)
        {
            _repository = repository;
        }
        [HttpPost]
        public IActionResult AddDirector(AddDirectorDto request)
        {
            Director director = new()
            {
                FullName = request.FullName,
                BirthDate = request.BirthDate
            };
            if (_repository.CheckDirectorExisting(director.FullName))
                return BadRequest("Director exists");

            _repository.AddDirector(director);
            return Ok("Director added");
        }

        [HttpGet]
        public IActionResult GetDirectors()
        {
            var directors = _repository.GetDirectors();
            if (directors.Count == 0)
                return BadRequest("There are not Directors");
            return Ok(directors);
        }

        [HttpGet]
        [Route("requestId")]
        public IActionResult GetDirector([Required] Guid requestId)
        {
            if (!_repository.CheckDirectorExisting(requestId))
                return BadRequest("Director not exists");
            var director = _repository.GetDirector(requestId);
            return Ok(director);
        }

        [HttpPut]
        [Route("RequestId")]
        public IActionResult ChangeDirector(ChangeDirectorDto request)
        {
            if (!_repository.CheckDirectorExisting(request.Id))
                return BadRequest("Director not exists");
            var director = new Director()
            {
                Id = request.Id,
                FullName = request.FullName,
                BirthDate = request.BirthDate
            };
            _repository.ChangeDirector(director);
            return Ok("Director changed");
        }

        [HttpDelete]
        [Route("requestId")]
        public IActionResult DeleteDirector([Required] Guid requestId)
        {
            if (!_repository.CheckDirectorExisting(requestId))
                return BadRequest("Director not exists");
            _repository.DeleteDirector(requestId);
            return Ok("Director deleted");
        }
    }
}
