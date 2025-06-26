using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizlet.Api.DTOs.Sets;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;
using System.Security.Claims;

namespace Quizlet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SetController : ControllerBase
{
    private readonly ISetRepository _setRepository;

    public SetController(ISetRepository setRepository)
    {
        _setRepository = setRepository;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sets = await _setRepository.GetAllByUserAsync(GetUserId());
        return Ok(sets.Select(s => s.ToResponse()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var set = await _setRepository.GetByIdAsync(id, GetUserId());
        if (set == null) return NotFound();
        return Ok(set.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSetRequest request)
    {
        var userId = GetUserId();
        var newSet = new Set(Guid.NewGuid(), userId, request.Title, request.Description);

        await _setRepository.AddAsync(newSet);
        return CreatedAtAction(nameof(Get), new { id = newSet.Id }, newSet.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSetRequest request)
    {
        var existing = await _setRepository.GetByIdAsync(id, GetUserId());
        if (existing == null) return NotFound();

        var updatedSet = new Set(id, existing.UserId, request.Title, request.Description);

        await _setRepository.UpdateAsync(updatedSet);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _setRepository.DeleteAsync(id, GetUserId());
        return NoContent();
    }
}

