using LangApp.Core.Data;
using LangApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LangApp.API.Services
{
    public class WordRepositoryService : IWordRepositoryService
    {
        private readonly AppDbContext _context;

        public WordRepositoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Word>> GetAllWordsAsync()
        {
            return await _context.Words.ToListAsync();
        }

        public async Task<Word> GetWordWithHistoryAsync(Guid wordId)
        {
            return await _context.Words
                .Include(w => w.ReviewHistories)
                .FirstOrDefaultAsync(w => w.Id == wordId);
        }

        public async Task CreateWordAsync(Word word)
        {
            await _context.Words.AddAsync(word);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWordAsync(Word word)
        {
            _context.Words.Update(word);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWordAsync(Guid wordId)
        {
            var word = await _context.Words.FindAsync(wordId);
            if (word != null)
            {
                _context.Words.Remove(word);
                await _context.SaveChangesAsync();
            }
        }
    }
}
