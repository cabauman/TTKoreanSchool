extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SplatAlias::Splat;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services
{
    public class StudyContentDataService2 : IStudyContentDataService, IEnableLogger
    {
        private const string LANG_CODE = "en";

        private readonly IVocabSectionRepo _vocabSectionRepo;
        private readonly IVocabSubsectionRepo _vocabSubsectionRepo;
        private readonly IExampleSentenceRepo _exampleSentenceRepo;
        private readonly IVocabTermRepo _vocabTermRepo;
        private readonly IStarredTermsRepo _starredTermsRepo;
        private readonly IStudyContentDownloadUrlRepo _studyContentDownloadUrlRepo;

        private readonly IStudyContentStorageService _storageService;

        private IList<Term> _latestStudySetTerms = new List<Term>();
        private IDictionary<string, ExampleSentence> _sentenceIdToSentenceDict;

        public StudyContentDataService2(
            IVocabSectionRepo vocabSectionRepo,
            IVocabSubsectionRepo vocabSubsectionRepo,
            IVocabTermRepo vocabTermRepo,
            IExampleSentenceRepo exampleSentenceRepo,
            IStarredTermsRepo starredTermsRepo)
        {
            _vocabSectionRepo = vocabSectionRepo;
            _vocabSubsectionRepo = vocabSubsectionRepo;
            _vocabTermRepo = vocabTermRepo;
            _exampleSentenceRepo = exampleSentenceRepo;
            _starredTermsRepo = starredTermsRepo;
        }

        public IObservable<IList<IDetailedFlashcardViewModel>> GetDetailedFlashcards(string uid, string studySetId, IList<Term> terms)
        {
        }

        public IObservable<IDetailedFlashcardViewModel> GetDetailedFlashcard(Term term, string uid, string studySetId)
        {
            var sentences = GetSentenceVmsUsingTerm(term);
            var imageUrls = GetVocabImageUrls(term);

            var flashcard = Observable
                .Return(term)
                .Zip(
                    sentences,
                    imageUrls,
                    (theTerm, theSentences, theImageUrls) =>
                    {
                        return new DetailedFlashcardViewModel(theTerm, theSentences, theImageUrls);
                    });

            return flashcard;
        }



        private IObservable<IList<IExampleSentenceViewModel>> GetSentenceVmsUsingTerm(Term term)
        {
            string[] sentenceIds = term.SentenceIds.Split(',');
            var sentenceVms = sentenceIds
                .ToObservable()
                .Select(sentenceId => GetSentenceVm(sentenceId))
                .Where(sentenceVm => sentenceVm != null)
                .ToList();

            return sentenceVms;
        }

        private IExampleSentenceViewModel GetSentenceVm(string sentenceId)
        {
            _sentenceIdToSentenceDict.TryGetValue(sentenceId, out ExampleSentence sentence);

            IExampleSentenceViewModel sentenceVm = null;
            if(sentence == null)
            {
                this.Log().Error("Sentence {0} couldn't be found.", sentenceId);
            }
            else
            {
                sentenceVm = new ExampleSentenceViewModel(sentence);
            }

            return sentenceVm;
        }

        private IObservable<Unit> FetchSentences()
        {
            return _exampleSentenceRepo
                .ReadAll(LANG_CODE)
                .Do(dict => _sentenceIdToSentenceDict = dict)
                .Select(_ => Unit.Default);
        }



        private IObservable<IList<string>> GetVocabImageUrls(IList<Term> terms)
        {
            return terms
                .ToObservable()
                .SelectMany(term => GetVocabImageUrls(term.ImageIds))
        }

        private IObservable<IList<string>> GetVocabImageUrls(Term term)
        {
            string[] imageIds = term.ImageIds.Split(',');
            var imageUrls = imageIds
                .ToObservable()
                .SelectMany(imageId => GetImageUrl(imageId))
                .ToList();

            return imageUrls;
        }

        private IObservable<string> GetImageUrl(string imageId)
        {
            _vocabImageFilenameToUrlDict.TryGetValue(imageId, out string url);

            if(url == null)
            {
                this.Log().Debug("Image url {0} not stored in the database. Checking storage...", imageId);
                return GetVocabImageUrlFromStorage(imageId);
            }

            return Observable.Return(url);
        }

        private IObservable<string> GetVocabImageUrlFromStorage(string filename)
        {
            return _storageService
                .GetVocabImageDownloadUrl(filename)
                .Catch<string, Exception>(
                    ex =>
                    {
                        this.Log().Error("Failed to retrieve image url {0} from storage: {1}", filename, ex.Message);
                        return Observable.Empty<string>();
                    })
                .Do(url => SaveVocabImageUrlToDatabase(filename, url));
        }

        private void SaveVocabImageUrlToDatabase(string url, string filename)
        {
            if(string.IsNullOrEmpty(url))
            {
                return;
            }

            _studyContentDownloadUrlRepo
                .SaveVocabImageUrl(filename, url)
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("Saved image url {0} to the database.", filename);
                    },
                    ex =>
                    {
                        this.Log().Error("Failed to save image url {0} to the database: {1}", filename, ex.Message);
                    });
        }

        private IObservable<Unit> FetchVocabImageUrls()
        {
            if(_vocabImageFilenameToUrlDict == null)
            {
                return _studyContentDownloadUrlRepo
                    .ReadVocabImageUrls()
                    .Select(dict => _vocabImageFilenameToUrlDict = dict)
                    .Select(_ => Unit.Default);
            }

            return Observable.Return(Unit.Default);
        }





        private IObservable<Unit> DownloadSentenceAudioFiles(IList<ExampleSentence> sentences)
        {
            return sentences
                .ToObservable()
                .Where(sentence => sentence.AudioVersion > 0)
                .Select(sentence => DownloadSentenceAudioFile(sentence))
                .Merge();
        }

        private IObservable<Unit> DownloadSentenceAudioFile(ExampleSentence sentence)
        {
            if(NeedToDownloadSentenceAudioFile(sentence.Id, sentence.AudioVersion))
            {
                return GetSentenceAudioUrl(sentence.Id)
                    .SelectMany(
                        downloadUrl =>
                        {
                            return _storageService.SaveSentenceAudioFileToLocalStorage(sentence.Id, downloadUrl);
                        });
            }

            return Observable.Return(Unit.Default);
        }

        private bool NeedToDownloadSentenceAudioFile(string sentenceId, int audioVersion)
        {
            if(_storageService.FileExists(sentenceId + audioVersion + ".mp3"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private IObservable<string> GetSentenceAudioUrl(string sentenceId)
        {
            _sentenceAudioFilenameToUrlDict.TryGetValue(sentenceId, out string url);

            if(url == null)
            {
                this.Log().Debug("Sentence audio url {0} not stored in the database. Checking storage...", sentenceId);
                return GetSentenceAudioUrlFromStorage(sentenceId);
            }

            return Observable.Return(url);
        }

        private IObservable<string> GetSentenceAudioUrlFromStorage(string filename)
        {
            return _storageService
                .GetSentenceAudioDownloadUrl(filename)
                .Catch<string, Exception>(
                    ex =>
                    {
                        this.Log().Error("Failed to retrieve sentence audio url {0} from storage: {1}", filename, ex.Message);
                        return Observable.Return<string>(null);
                    })
                .Do(url => SaveSentenceAudioUrlToDatabase(filename, url));
        }

        private void SaveSentenceAudioUrlToDatabase(string url, string filename)
        {
            if(string.IsNullOrEmpty(url))
            {
                return;
            }

            _studyContentDownloadUrlRepo
                .SaveSentenceAudioUrl(filename, url)
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("Saved sentence audio url {0} to the database.", filename);
                    },
                    ex =>
                    {
                        this.Log().Error("Failed to save sentence audio url {0} to the database: {1}", filename, ex.Message);
                    });
        }





        private IObservable<Unit> DownloadAudioFilesToLocalStorage(string directory, IEnumerable<string> filenames)
        {
            var result =
                from filename in filenames.ToObservable()
                where !_storageService.FileExists(filename)
                from downloadUrl in GetFileDownloadUrl(directory, filename)
                where downloadUrl != null
                select _storageService.SaveFileToLocalStorage(directory, filename, downloadUrl);

                return result.Merge();
        }

        private IObservable<Unit> DownloadVocabAudioFiles(IList<Term> terms)
        {
            return terms
                .ToObservable()
                .Where(term => term.AudioVersion > 0)
                .Select(term => string.Format("{0}{1}.mp3", term.Romanization, term.AudioVersion))
                .Where(filename => !_storageService.FileExists(filename))
                .Select(filename => GetFileDownloadUrl(_vocabAudioFilenameToUrlDict, "vocabAudio", filename))
                .SelectMany(downloadUrl => downloadUrl)
                .Select(downloadUrl => _storageService.SaveSentenceAudioFileToLocalStorage("filename", downloadUrl))
                .Merge();
        }

        private IObservable<string> GetFileDownloadUrl(string directory, string filename)
        {
            return _studyContentDownloadUrlRepo
                .Read(directory, filename)
                .SelectMany(
                    downloadUrl =>
                    {
                        if(downloadUrl == null)
                        {
                            this.Log().Debug("Download url {0} not stored in the database. Checking storage...", filename);
                            return GetFileDownloadUrlFromStorage(directory, filename);
                        }

                        return Observable.Return(downloadUrl);
                    });
        }

        private IObservable<string> GetFileDownloadUrlFromStorage(string directory, string filename)
        {
            return _storageService
                .GetFileDownloadUrl(directory, filename)
                .Catch<string, Exception>(
                    ex =>
                    {
                        this.Log().Error("Failed to retrieve download url {0} from storage: {1}", filename, ex.Message);
                        return Observable.Return<string>(null);
                    })
                .Do(url => SaveFileDownloadUrlToDatabase(directory, filename, url));
        }

        private void SaveFileDownloadUrlToDatabase(string directory, string filename, string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                return;
            }

            _studyContentDownloadUrlRepo
                .Save(directory, filename, url)
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("Saved download url {0} to the database.", filename);
                    },
                    ex =>
                    {
                        this.Log().Error("Failed to save download url {0} to the database: {1}", filename, ex.Message);
                    });
        }
    }
}