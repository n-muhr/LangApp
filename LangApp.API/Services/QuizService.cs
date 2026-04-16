using LangApp.Core.Data;
using LangApp.Core.Models;

namespace LangApp.API.Services
{
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Word>> GetDailyQuiz(Guid userId, int count = 20)
        {
            // *******************************************************************
            // TODO: IMPLEMENT SRS LOGIC HERE.
            // 1. Find all words where the last review was within the required window (daily, or based on IntervalDays).
            // 2. Select the top 'count' words ensuring a balanced mix of difficulty.
            // 3. Return the list of Word objects.
            // *******************************************************************
            return new List<Word>();
        }

        public async Task SubmitQuizResult(Guid userId, Guid wordId, int performanceRating)
        {
            // *******************************************************************
            // TODO: IMPLEMENT SM-2 ALGORITHM HERE.
            // 1. Retrieve the ReviewHistory record for the wordId and userId.
            // 2. Update EasinessFactor and IntervalDays based on the performanceRating (1-5).
            // 3. Update LastReviewedDate = DateTime.UtcNow.
            // 4. Save the updated ReviewHistory record.
            // *******************************************************************
        }
    }
}
