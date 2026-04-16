using LangApp.Core.Models;

namespace LangApp.API.Services
{
    public interface IWordRepositoryService
    {
        Task<List<Word>> GetAllWordsAsync();

        Task<Word> GetWordWithHistoryAsync(Guid wordId);

        Task CreateWordAsync(Word word);

        Task UpdateWordAsync(Word word);

        Task DeleteWordAsync(Guid wordId);
    }
}
