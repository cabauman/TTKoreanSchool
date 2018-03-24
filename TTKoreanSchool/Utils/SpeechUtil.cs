using System.Threading.Tasks;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Utils
{
    public class SpeechUtil
    {
        private readonly IAudioService _audioService;
        private readonly ISpeechService _speechService;
        private readonly IStudyContentStorageService _storageService;

        public SpeechUtil(IAudioService audioService, ISpeechService speechService, IStudyContentStorageService storageService)
        {
            _audioService = audioService;
            _speechService = speechService;
            _storageService = storageService;
        }

        public async Task Speak(string filenameWithoutExtension, string text)
        {
            var localPath = await _storageService.GetVocabAudioLocalPath(filenameWithoutExtension);
            if(localPath != null)
            {
                _audioService.Play(localPath);
            }
            else
            {
                _speechService.Speak(text);
            }
        }
    }
}