using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(words);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var word = await _wordRepository.GetByIdAsync(id, GetUserId());
            return word == null ? NotFound() : Ok(word);
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var words = await _wordRepository.GetFavoriteWordsAsync(GetUserId());
            return Ok(words);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(Guid setId, string query)
        {
            var result = await _wordRepository.SearchWordsAsync(setId, GetUserId(), query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Word word)
        {
            await _wordRepository.AddAsync(word);
            return CreatedAtAction(nameof(Get), new { id = word.Id }, word);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Word word)
        {
            if (id != word.Id) return BadRequest();
            await _wordRepository.UpdateAsync(word);
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
