using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizlet.Api.DTOs.Words;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;
using System.Security.Claims;

namespace Quizlet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WordController : ControllerBase
    {
        private readonly IWordRepository _wordRepository;

        public WordController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("set/{setId}")]
        public async Task<IActionResult> GetBySet(Guid setId, int skip = 0, int take = 20)
        {
            var words = await _wordRepository.GetWordsBySetIdAsync(setId, GetUserId(), skip, take);
            return Ok(words.Select(w => w.ToResponse()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var word = await _wordRepository.GetByIdAsync(id, GetUserId());
            return word == null ? NotFound() : Ok(word.ToResponse());
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var words = await _wordRepository.GetFavoriteWordsAsync(GetUserId());
            return Ok(words.Select(w => w.ToResponse()));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(Guid setId, string query)
        {
            var result = await _wordRepository.SearchWordsAsync(setId, GetUserId(), query);
            return Ok(result.Select(w => w.ToResponse()));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWordRequest request)
        {
            var word = new Word(
                Guid.NewGuid(),
                request.SetId,
                request.Name,
                request.Definition,
                request.IsFavorite,
                request.IsLastWord
            );
            await _wordRepository.AddAsync(word);
            return CreatedAtAction(nameof(Get), new { id = word.Id }, word.ToResponse());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWordRequest request)
        {
            var existing = await _wordRepository.GetByIdAsync(id, GetUserId());
            if (existing == null) return NotFound();

            var updated = new Word(
                id,
                existing.SetId,
                request.Name,
                request.Definition,
                request.IsFavorite,
                request.IsLastWord
            );

            await _wordRepository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _wordRepository.DeleteAsync(id, GetUserId());
            return NoContent();
        }
    }
}
