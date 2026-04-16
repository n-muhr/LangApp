using LangApp.API.Services;
using LangApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LangApp.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordRepositoryService _wordRepository;

        public WordController(IWordRepositoryService wordRepository)
        {
            _wordRepository = wordRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Word>>> GetWords()
        {
            try
            {
                var words = await _wordRepository.GetAllWordsAsync();
                return Ok(words);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching words: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Word>> GetWord(Guid id)
        {
            try
            {
                var word = await _wordRepository.GetWordWithHistoryAsync(id);
                if (word == null)
                {
                    return NotFound($"Word with ID {id} not found.");
                }
                return Ok(word);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the word: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Word>> CreateWord([FromBody] Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _wordRepository.CreateWordAsync(word);
                return CreatedAtAction(nameof(GetWord), new { id = word.Id }, word);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the word: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWord(Guid id, [FromBody] Word updatedWord)
        {
            if (id != updatedWord.Id)
            {
                return BadRequest("The ID in the URL must match the ID in the request body.");
            }

            try
            {
                var existingWord = await _wordRepository.GetWordWithHistoryAsync(id);
                if (existingWord == null)
                {
                    return NotFound($"Word with ID {id} not found.");
                }

                await _wordRepository.UpdateWordAsync(updatedWord);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the word: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWord(Guid id)
        {
            try
            {
                await _wordRepository.DeleteWordAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the word: {ex.Message}");
            }
        }
    }
}
