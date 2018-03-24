//extern alias SplatAlias;

//using System;
//using System.IO;
//using System.Net;
//using System.Threading.Tasks;
//using Firebase.Storage;
//using SplatAlias::Splat;
//using TTKoreanSchool.Services.Interfaces;

//namespace TTKoreanSchool.Services
//{
//    public abstract class BaseStudyContentStorageService : IStudyContentStorageService, IEnableLogger
//    {
//        private const string STORAGE_BUCKET = "your-bucket.appspot.com";
//        private const string API_KEY = "api key";

//        private const string FIREBASE_PATH_VOCAB_IMAGES = "term-images";
//        private const string FIREBASE_PATH_VOCAB_AUDIO = "term-audio";
//        private const string FIREBASE_PATH_SENTENCE_AUDIO = "sentence-audio";

//        private FirebaseStorage _client;
//        private IStudyContentDataService _dataService;

//        public BaseStudyContentStorageService(IStudyContentDataService dataService)
//        {
//            LocalAudioDirectory = Path.Combine(LocalRootDirectory, "Audio");
//            FirebaseAuthService.AuthChanged += FirebaseAuthService_AuthChanged;
//        }

//        public string LocalAudioDirectory { get; }

//        protected abstract string LocalRootDirectory { get; }

//        /// <summary>
//        /// Get the download url for a vocab image.
//        /// </summary>
//        /// <param name="filenameWithoutExtension">The filename of the vocab image.</param>
//        /// <returns>The download url.</returns>
//        public async Task<string> GetVocabImageUrl(string filenameWithoutExtension)
//        {
//            // First check if the url is saved in the database.
//            string url = await _dataService.GetVocabImageUrl(filenameWithoutExtension);
//            if(!string.IsNullOrEmpty(url))
//            {
//                return url;
//            }

//            // Fetch the image from Firebase storage.
//            string firebasePath = Path.Combine(FIREBASE_PATH_VOCAB_IMAGES, filenameWithoutExtension, ".jpg");
//            url = await GetDownloadUrl(firebasePath);

//            // Store the url in the database for easy access.
//            if(!string.IsNullOrEmpty(url))
//            {
//                await _dataService.SaveVocabImageUrl(filenameWithoutExtension, url);
//            }

//            return url;
//        }

//        public async Task<string> GetVocabAudioLocalPath(string filenameWithoutExtension)
//        {
//            string filename = Path.Combine(filenameWithoutExtension, ".mp3");
//            return await GetFileLocalPath(filename, LocalAudioDirectory, FIREBASE_PATH_VOCAB_AUDIO);
//        }

//        public async Task<string> GetSentenceAudioLocalPath(string filenameWithoutExtension)
//        {
//            string filename = Path.Combine(filenameWithoutExtension, ".mp3");
//            return await GetFileLocalPath(filename, LocalAudioDirectory, FIREBASE_PATH_SENTENCE_AUDIO);
//        }

//        private void FirebaseAuthService_AuthChanged(string firebaseAuthToken)
//        {
//            var options = new FirebaseStorageOptions()
//            {
//                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthToken)
//            };

//            _client = new FirebaseStorage(STORAGE_BUCKET, options);
//        }

//        private async Task<string> GetFileLocalPath(string filename, string baseLocalPath, string baseRemotePath)
//        {
//            string localPath = Path.Combine(baseLocalPath, filename);
//            if(File.Exists(localPath))
//            {
//                return localPath;
//            }

//            string firebasePath = Path.Combine(baseRemotePath, filename);
//            string url = await GetDownloadUrl(firebasePath);
//            if(string.IsNullOrEmpty(url))
//            {
//                return null;
//            }
//            else
//            {
//                await DownloadFile(url, localPath);
//            }

//            return localPath;
//        }

//        private async Task<string> GetDownloadUrl(string firebasePath)
//        {
//            string downloadUrl = await _client
//                .Child(firebasePath)
//                .GetDownloadUrlAsync();

//            return downloadUrl;
//        }

//        private async Task DownloadFile(string url, string localPath)
//        {
//            using(WebClient wc = new WebClient())
//            {
//                await wc.DownloadFileTaskAsync(new Uri(url), localPath);
//            }
//        }
//    }
//}