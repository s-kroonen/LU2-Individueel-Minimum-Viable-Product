using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.api.Models;
using WebApi.api.Repositories;
using System;

namespace WebApi.api.Controllers;

[ApiController]
[Route("api/environments/{environmentId}/objects")]
public class Object2DController : ControllerBase
{
    private readonly Object2DRepository _object2DRepository;
    private readonly IAuthenticationService _authenticationService;

    public Object2DController(Object2DRepository object2DRepository, IAuthenticationService authenticationService)
    {
        _object2DRepository = object2DRepository;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddObject2D([FromBody] Object2D object2D, Guid environmentId)
    {
        var createdObject = await _object2DRepository.InsertAsync(object2D, environmentId);
        return Ok(createdObject);
    }

    [HttpGet]
    public async Task<IEnumerable<Object2D>> GetAllObjects2D()
    {
        return await _object2DRepository.ReadAllAsync();
    }

    [HttpPut("{objectId}")]
    public async Task<IActionResult> UpdateObject2D(Guid objectId, [FromBody] Object2D object2D)
    {
        var existingObject = await _object2DRepository.ReadAsync(objectId);
        if (existingObject == null)
        {
            return NotFound(new { message = "Object not found." });
        }

        await _object2DRepository.UpdateAsync(object2D);
        return Ok(new { message = $"Object {object2D.PrefabId} has been updated successfully." });
    }

    [HttpDelete("{objectId}")]
    public async Task<IActionResult> DeleteObject2D(Guid objectId)
    {
        await _object2DRepository.DeleteAsync(objectId);
        return Ok(new { message = "Object was deleted successfully." });
    }
}