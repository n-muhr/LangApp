using LangApp.Core.Models;

namespace LangApp.API.Services
{
    public interface IQuizService
    {
        Task<List<Word>> GetDailyQuiz(Guid userId, int count = 20);

        Task SubmitQuizResult(Guid userId, Guid wordId, int performanceRating);
    }
}
