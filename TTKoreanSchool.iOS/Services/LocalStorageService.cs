using System;
using System.IO;

namespace TTKoreanSchool.iOS.Services
{
    public class LocalStorageService
    {
        private readonly string _root;
        private readonly string _vocabAudioDirectory;
        private readonly string _sentenceAudioDirectory;

        public LocalStorageService()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _root = Path.Combine(documents, "..", "Library");
            _vocabAudioDirectory = Path.Combine(_root, "VocabAudio");
            _sentenceAudioDirectory = Path.Combine(_root, "SentenceAudio");
        }

        public string GetVocabAudioFileUrl(string filename)
        {
            return Path.Combine(_vocabAudioDirectory, filename);
        }

        public string GetSentenceAudioFileUrl(string filename)
        {
            return Path.Combine(_sentenceAudioDirectory, filename);
        }
    }
}