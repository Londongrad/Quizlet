using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(sets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var set = await _setRepository.GetByIdAsync(id, GetUserId());
        if (set == null) return NotFound();
        return Ok(set);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Set set)
    {
        var userId = GetUserId();
        var newSet = new Set(set.Id, set.Name, userId)
        {
            ImageUrl = set.ImageUrl,
        };

        await _setRepository.AddAsync(newSet);
        return CreatedAtAction(nameof(Get), new { id = newSet.Id }, newSet);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Set updated)
    {
        var existing = await _setRepository.GetByIdAsync(id, GetUserId());
        if (existing == null) return NotFound();

        var updatedSet = new Set(id, updated.Name, existing.UserId)
        {
            ImageUrl = updated.ImageUrl,
            CreatedAt = existing.CreatedAt
        };

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

