using Cinema.Domain.Abstracts.ServiceAbstracts;
using Cinema.Domain.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

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
        public async Task<IActionResult> GetActors()
        {
            var result = await _service.GetActorsAsync();

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

        [HttpGet, Route("id")]
        public async Task<IActionResult> GetActor([Required] Guid id)
        {
            var result = await _service.GetActorAsync(id);

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
        public async Task<IActionResult> ChangeActor(ChangeActorDto request)
        {
            var result = await _service.ChangeActorAsync(request);

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
        public async Task<IActionResult> DeleteActor([Required] Guid requestId)
        {
            var result = await _service.DeleteActorAsync(requestId);

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
