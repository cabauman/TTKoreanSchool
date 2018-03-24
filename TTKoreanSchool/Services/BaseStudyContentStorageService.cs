extern alias SplatAlias;

using SplatAlias::Splat;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.Services
{
    public abstract class BaseStudyContentStorageService : IStudyContentStorageService, IEnableLogger
    {
        private const string STORAGE_BUCKET = "gs://tt-korean-academy.appspot.com";

        private const string FIREBASE_PATH_VOCAB_IMAGES = "term-images";
        private const string FIREBASE_PATH_VOCAB_AUDIO = "term-audio";
        private const string FIREBASE_PATH_SENTENCE_AUDIO = "sentence-audio";

        protected abstract string LocalRootDirectory { get; }
    }
}