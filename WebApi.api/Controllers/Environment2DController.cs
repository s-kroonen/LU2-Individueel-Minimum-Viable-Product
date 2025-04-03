using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.api.Models;
using WebApi.api.Repositories;
using System;

namespace WebApi.api.Controllers;

[ApiController]
[Route("api/environments")]
public class Environment2DController : ControllerBase
{
    private readonly Environment2DRepository _environment2DRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Environment2DController> _logger;

    public Environment2DController(
        Environment2DRepository environment2DRepository,
        IAuthenticationService authenticationService,
        ILogger<Environment2DController> logger)
    {
        _environment2DRepository = environment2DRepository;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddEnvironment2D([FromBody] Environment2D environment2D)
    {
        _logger.LogInformation("Received request to add a new environment.");

        try
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Authenticated user: {UserId}", userId);

            if (environment2D == null)
            {
                _logger.LogWarning("Environment2D object was null in request body.");
                return BadRequest(new { message = "Invalid request body." });
            }

            await _environment2DRepository.InsertAsync(environment2D, userId);
            _logger.LogInformation("Environment {EnvironmentName} created by user {UserId}.", environment2D.Name, userId);

            return Ok(new
            {
                message = $"Environment '{environment2D.Name}' has been created successfully by User '{userId}'"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding environment.");
            return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }

    [HttpGet("{environmentId}")]
    public async Task<IActionResult> GetEnvironment2D(string environmentId)
    {
        _logger.LogInformation("Fetching environment with ID: {EnvironmentId}", environmentId);

        try
        {
            var environment2D = await _environment2DRepository.ReadAsync(environmentId);
            if (environment2D == null)
            {
                _logger.LogWarning("Environment with ID {EnvironmentId} not found.", environmentId);
                return NotFound(new { message = $"Environment with ID '{environmentId}' was not found." });
            }

            return Ok(environment2D);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching environment with ID {EnvironmentId}", environmentId);
            return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEnvironment2DByUser()
    {
        try
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Fetching environments for user {UserId}", userId);

            var environments = await _environment2DRepository.ReadByUserAsync(userId);
            if (environments == null || !environments.Any())
            {
                _logger.LogWarning("No environments found for user {UserId}", userId);
                return NotFound(new { message = "No environments found for the current user." });
            }

            return Ok(environments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching environments for user.");
            return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }

    [HttpPut("{environmentId}")]
    public async Task<IActionResult> UpdateEnvironment2D(string environmentId, [FromBody] Environment2D environment2D)
    {
        _logger.LogInformation("Updating environment {EnvironmentId}", environmentId);

        try
        {
            var existingEnvironment = await _environment2DRepository.ReadAsync(environmentId);
            if (existingEnvironment == null)
            {
                _logger.LogWarning("Environment with ID {EnvironmentId} not found.", environmentId);
                return NotFound(new { message = "Environment not found." });
            }

            await _environment2DRepository.UpdateAsync(environment2D);
            _logger.LogInformation("Environment {EnvironmentId} updated successfully.", environmentId);

            return Ok(new { message = $"Environment '{environment2D.Name}' has been updated successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating environment {EnvironmentId}", environmentId);
            return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }

    [HttpDelete("{environmentId}")]
    public async Task<IActionResult> DeleteEnvironment2D(Guid environmentId)
    {
        _logger.LogInformation("Deleting environment {EnvironmentId}", environmentId);

        try
        {
            await _environment2DRepository.DeleteAsync(environmentId);
            _logger.LogInformation("Environment {EnvironmentId} deleted successfully.", environmentId);

            return Ok(new { message = "Environment was deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting environment {EnvironmentId}", environmentId);
            return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
        }
    }
}
