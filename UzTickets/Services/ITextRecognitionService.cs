using System.Threading.Tasks;
using UzTickets.Models;

namespace UzTickets.Services
{
    public interface ITextRecognitionService
    {
        Task<RecognizedMessage> AnalyzeMessageAsync(string message);
    }
}
