﻿using Cinema.Domain.Abstracts.ServiceAbstracts;
using Cinema.Domain.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _service;
        private readonly ILogger<GenresController> _logger;
        public GenresController(IGenreService service, ILogger<GenresController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(AddGenreDto request)
        {
            var result = await _service.AddGenreAsync(request);

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
        public async Task<IActionResult> GetGenres()
        {
            var result = await _service.GetGenresAsync();

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
        public async Task<IActionResult> GetGenre([Required] Guid requestId)
        {
            var result = await _service.GetGenreAsync(requestId);

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
        public async Task<IActionResult> ChangeGenre(ChangeGenreDto request)
        {
            var result = await _service.ChangeGenreAsync(request);

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
        public async Task<IActionResult> DeleteGenre([Required] Guid requestId)
        {
            var result = await _service.DeleteGenreAsync(requestId);

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