using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public Environment2DController(Environment2DRepository environment2DRepository, IAuthenticationService authenticationService)
    {
        _environment2DRepository = environment2DRepository;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddEnvironment2D([FromBody] Environment2D environment2D)
    {
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();

        await _environment2DRepository.InsertAsync(environment2D, userId);
        return Ok(new { message = "Environment: " + environment2D.Name + " has been created successfully, by User: " + environment2D.UserId });
    }

    [HttpGet("{environmentId}")]
    public async Task<IActionResult> GetEnvironment2D(string environmentId)
    {
        var environment2D = await _environment2DRepository.ReadAsync(environmentId);
        if (environment2D == null)
            return NotFound("Environment was not found.");

        return Ok(environment2D);
    }

    [HttpGet]
    public async Task<IActionResult> GetEnvironment2DByUser()
    {
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        var environment2D = await _environment2DRepository.ReadByUserAsync(userId);
        if (environment2D == null)
            return NotFound("User was not found or User doesn't have Environments");

        return Ok(environment2D);
    }

    [HttpPut("{environmentId}")]
    public async Task<IActionResult> UpdateEnvironment2D(string environmentId, [FromBody] Environment2D environment2D)
    {
        var existingEnvironment = await _environment2DRepository.ReadAsync(environmentId);
        if (existingEnvironment == null)
        {
            return NotFound(new { message = "Environment not found." });
        }

        await _environment2DRepository.UpdateAsync(environment2D);
        return Ok(new { message = $"Environment {environment2D.Name} has been updated successfully." });
    }

    [HttpDelete("{environmentId}")]
    public async Task<IActionResult> DeleteEnvironment2D(Guid environmentId)
    {
        await _environment2DRepository.DeleteAsync(environmentId);
        return Ok(new { message = "Environment was deleted successfully." });
    }
}