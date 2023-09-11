using Cinema.Application.Dtos;
using Cinema.Domain.Abstracts.ServiceAbstracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
        public async Task<IActionResult> GetDirectors()
        {
            var result = await _service.GetDirectorsAsync();

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
        public async Task<IActionResult> GetDirector([Required] Guid requestId)
        {
            var result = await _service.GetDirectorAsync(requestId);

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
        public async Task<IActionResult> ChangeDirector(ChangeDirectorDto request)
        {
            var result = await _service.ChangeDirectorAsync(request);

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
        public async Task<IActionResult> DeleteDirector([Required] Guid requestId)
        {
            var result = await _service.DeleteDirectorAsync(requestId);

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