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
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _repository;
        public GenreController(IGenreRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddGenre(AddGenreDto request)
        {
            Genre genre = new()
            {
                Name = request.Name
            };
            if (_repository.CheckGenreExisting(genre.Name))
                return BadRequest("Genre exists");

            _repository.AddGenre(genre);
            return Ok("Genre added");
        }

        [HttpGet]
        public IActionResult GetGenres()
        {
            var genres = _repository.GetGenres();
            if (genres.Count == 0)
                return BadRequest("There is not Genres");
            return Ok(genres);
        }

        [HttpGet]
        [Route("requestId")]
        public IActionResult GetGenre([Required] Guid requestId)
        {
            if (!_repository.CheckGenreExisting(requestId))
                return BadRequest("Genre not exists");
            var genre = _repository.GetGenre(requestId);
            return Ok(genre);
        }

        [HttpPut]
        public IActionResult ChangeGenre(ChangeGenreDto request)
        {
            if (!_repository.CheckGenreExisting(request.Id))
                return BadRequest("Genre not exists");
            Genre genre = new()
            {
                Id = request.Id,
                Name = request.Name
            };
            _repository.ChangeGenre(genre);
            return Ok("Genre changed");
        }

        [HttpDelete]
        [Route("requestId")]
        public IActionResult DeleteGenre([Required] Guid requestId)
        {
            if (!_repository.CheckGenreExisting(requestId))
                return BadRequest("Genre not exists");
            _repository.DeleteGenre(requestId);
            return Ok("Genre deleted");
        }
    }
}
