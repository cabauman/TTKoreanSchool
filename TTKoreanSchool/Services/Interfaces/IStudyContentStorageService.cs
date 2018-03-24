using System.Threading.Tasks;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IStudyContentStorageService
    {
        Task<string> GetVocabImageUrl(string filename);

        Task<string> GetVocabAudioLocalPath(string filename);

        Task<string> GetSentenceAudioLocalPath(string filename);
    }
}