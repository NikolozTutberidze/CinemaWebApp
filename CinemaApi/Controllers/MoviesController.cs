using Azure.Core;
using LogicLayer.Dtos;
using LogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
            var result = await _service.AddMovieAsync(request);

            if (result.StatusCode is HttpStatusCode.Created)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl() + "/" + result.Data.Id);

                return Created(Request.GetEncodedUrl() + "/" + result.Data.Id, result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var result = await _service.GetMoviesAsync();

            if (result.StatusCode is HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpGet, Route("requestId")]
        public async Task<IActionResult> GetMovieById([Required] Guid requestId)
        {
            var result = await _service.GetMovieByIdAsync(requestId);

            if (result.StatusCode is HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpGet, Route("request")]
        public async Task<IActionResult> GetMovieByTitle([Required] string request)
        {
            var result = await _service.GetMoviesByTitleAsync(request);

            if (result.StatusCode is HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpGet, Route("By-IMDB-Rating")]
        public async Task<IActionResult> GetMovieByIMDBRating([Required] double firstRating, [Required] double secondRating)
        {
            var result = await _service.GetMoviesByIMDBAsync(firstRating, secondRating);

            if (result.StatusCode is HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpGet, Route("By-Date")]
        public async Task<IActionResult> GetMovieByDate([Required] int firstDate, [Required] int secondDate)
        {
            var result = await _service.GetMoviesByYearAsync(firstDate, secondDate);

            if (result.StatusCode is HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeMovie(ChangeMovieDto request)
        {
            var result = await _service.ChangeMovieAsync(request);

            if (result.StatusCode is HttpStatusCode.OK)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return Ok(result.Data);
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpDelete, Route("requestId")]
        public async Task<IActionResult> DeleteMovie([Required] Guid requestId)
        {
            var result = await _service.DeleteMovieAsync(requestId);

            if (result.StatusCode is HttpStatusCode.NoContent)
            {
                _logger.LogInformation(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString() + '\n'
                    + '\t' + Request.GetEncodedUrl());

                return NoContent();
            }

            _logger.LogError(ControllerContext.ActionDescriptor.ControllerName + '\n'
                    + '\t' + ControllerContext.ActionDescriptor.ActionName + '\n'
                    + '\t' + result.StatusCode.ToString());

            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
