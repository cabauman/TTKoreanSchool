extern alias SplatAlias;

using SplatAlias::Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services
{
    public class StudyContentDataService : IStudyContentDataService, IEnableLogger
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
        private IDictionary<string, string> _vocabImageFilenameToUrlDict;
        private IDictionary<string, string> _vocabAudioFilenameToUrlDict;
        private IDictionary<string, string> _sentenceAudioFilenameToUrlDict;

        public StudyContentDataService(
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

        public IObservable<string> GetVocabAudioUrls()
        {


            _studyContentDownloadUrlRepo
                .ReadAll("");

        }

        public IObservable<IList<ExampleSentence>> GetSentences()
        {
            if(_sentences?.Count > 0)
            {
                return _sentences
                    .ToObservable()
                    .ToList();
            }

            _sentences = new List<ExampleSentence>();
            return _exampleSentenceRepo
                .ReadAll(LANG_CODE)
                .Do(
                    model =>
                    {
                        _sentences.Add(model);
                    })
                .ToList();
        }

        public IObservable<IDictionary<string, bool>> GetUserStarredTermsForStudySet(string uid, string studySetId)
        {
            return _starredTermsRepo
                .Read(uid, studySetId);
        }

        public IObservable<bool> GetUserStarredTerm(string uid, string studySetId, Term term)
        {
            return _starredTermsRepo
                .Read(uid, studySetId)
                .Select(x => x.ContainsKey(term.Id) ? x[term.Id] : false);
        }

        public IObservable<IDetailedFlashcardViewModel> GetDetailedFlashcard(Term term, string uid, string studySetId)
        {
            var sentences = GetSentences(term);
            var imageUrls = GetVocabImageUrls(term);
            var isStarred = GetUserStarredTerm(uid, studySetId, term);

            var flashcard = Observable
                .Return(term)
                .Zip(
                    sentences,
                    imageUrls,
                    isStarred,
                    (theTerm, theSentences, theImageUrls, theIsStarred) =>
                    {
                        return new DetailedFlashcardViewModel(theTerm, theSentences, theImageUrls, theIsStarred);
                    });

            return flashcard;
        }

        public IObservable<IList<IExampleSentenceViewModel>> GetSentences(Term term)
        {
            string[] sentenceIds = term.SentenceIds.Split(',');
            var sentenceVms = sentenceIds
                .ToObservable()
                .Select(sentenceId => GetSentence(sentenceId))
                .ToList();

            return sentenceVms;
        }

        private IExampleSentenceViewModel GetSentence(string sentenceId)
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

        public IObservable<IList<IDetailedFlashcardViewModel>> GetDetailedFlashcards(string uid, string studySetId, IList<Term> terms)
        {
            var vocabImageUrls = FetchVocabImageUrls();
            var sentenceAudioUrls = FetchSentenceAudioUrls();
            var sentences = FetchSentences(LANG_CODE);
            var urls = Observable.Merge(vocabImageUrls, sentenceAudioUrls, sentences);

            var result = urls
                .SelectMany(
                    _ =>
                    {
                        IEnumerable<string> missingImageFilenames = terms
                            .SelectMany(term => term.ImageIds.Split(','))
                            .Where(imageId => !_vocabImageFilenameToUrlDict.ContainsKey(imageId))
                            .Distinct();

                        IEnumerable<string> missingSentenceAudioFilenames = terms
                            .SelectMany(term => term.SentenceIds.Split(','))
                            .Where(sentenceId => !_sentenceAudioFilenameToUrlDict.ContainsKey(sentenceId))
                            .Distinct();

                        var missingVocabImageUrls = _storageService
                            .GetVocabImageDownloadUrls(missingImageFilenames.ToArray())
                            .Do(
                                entry =>
                                {
                                    _vocabImageFilenameToUrlDict[entry.Key] = entry.Value;
                                })
                            .Select(x => Unit.Default);

                        var missingSentenceAudioUrls = _storageService
                            .GetSentenceAudioDownloadUrls(missingSentenceAudioFilenames.ToArray())
                            .Catch<KeyValuePair<string, string>, Exception>(
                                ex2 =>
                                {
                                    return Observable.Return(new KeyValuePair<string, string>("", ""));
                                })
                            .Do(
                                entry =>
                                {
                                    _sentenceAudioFilenameToUrlDict[entry.Key] = entry.Value;
                                    _studyContentDownloadUrlRepo.SaveVocabImageUrl(entry.Key, entry.Value);
                                })
                            .Select(x => Unit.Default);

                        return Observable.Merge(missingVocabImageUrls, missingSentenceAudioUrls);
                    })
                .SelectMany(_ => terms.ToObservable())
                .SelectMany(term => GetDetailedFlashcard2(term))
                .ToList();

            return result;
        }

        public IObservable<Unit> FetchSentences(string langCode)
        {
            if(_sentenceIdToSentenceDict == null)
            {
                return _exampleSentenceRepo
                    .ReadAll(langCode)
                    .Select(
                        dict =>
                        {
                            _sentenceIdToSentenceDict = dict;
                            return Unit.Default;
                        });
            }

            return Observable.Return(Unit.Default);
        }

        public IObservable<Unit> FetchVocabImageUrls()
        {
            if(_vocabImageFilenameToUrlDict == null)
            {
                return _studyContentDownloadUrlRepo
                    .ReadVocabImageUrls()
                    .Select(
                        dict =>
                        {
                            _vocabImageFilenameToUrlDict = dict;
                            return Unit.Default;
                        });
            }

            return Observable.Return(Unit.Default);
        }

        public IObservable<Unit> FetchSentenceAudioUrls()
        {
            if(_sentenceAudioFilenameToUrlDict == null)
            {
                return _studyContentDownloadUrlRepo
                    .ReadSentenceAudioUrls()
                    .Select(
                        dict =>
                        {
                            _sentenceAudioFilenameToUrlDict = dict;
                            return Unit.Default;
                        });
            }

            return Observable.Return(Unit.Default);
        }

        public IObservable<IDetailedFlashcardViewModel> GetDetailedFlashcard2(Term term)
        {
            var imageUrlsObservable = GetVocabImageUrls2(term);
            var sentenceAudioUrlsObservable = GetSentenceAudioUrls();
            var sentenceVmsObservable = GetSentences(term);

            var flashcard = Observable
                .Return(term)
                .Zip(
                    imageUrlsObservable,
                    sentenceVmsObservable,
                    (theTerm, imageUrls, sentenceVms) =>
                    {
                        return new DetailedFlashcardViewModel(theTerm, sentenceVms, imageUrls);
                    });

            return flashcard;
        }

        public IObservable<IList<string>> GetVocabImageUrls2(Term term)
        {
            IList<string> imageUrls = new List<string>();
            string[] imageIds = term.ImageIds.Split(',');
            var vocabImageDownloadUrls = imageIds
                .ToObservable()
                .SelectMany(imageId => GetVocabImageUrl2(imageId))
                .ToList();

            return vocabImageDownloadUrls;
        }

        public IObservable<string> GetVocabImageUrl2(string imageId)
        {
            _vocabImageFilenameToUrlDict.TryGetValue(imageId, out string url);
            if(url != null)
            {
                return Observable.Return(url);
            }
            else
            {
                return FetchVocabImageUrlFromStorage(null, imageId);
            }
        }

        public IList<IMatchGameCardViewModel> GetMatchGameCards(string studySetId)
        {
            return _latestStudySetTerms
                .Select(
                    model =>
                    {
                        IMatchGameCardViewModel vm = new MatchGameCardViewModel(model);
                        return vm;
                    })
                .ToList();
        }

        public IList<Term> GetTerms()
        {
            return _latestStudySetTerms;
        }

        IObservable<IList<Term>> _vocabTerms;
        public IObservable<IList<Term>> GetVocabTerms(string studySetId)
        {
            if(_vocabTerms == null)
            {
                _vocabTerms = _vocabTermRepo
                    .ReadStudySet(LANG_CODE, studySetId)
                    .ToList();
            }

            return _vocabTerms;
        }

        public IObservable<IList<IMiniFlashcardViewModel>> GetMiniFlashcards(string studySetId)
        {
            _latestStudySetTerms.Clear();

            return _vocabTermRepo
                .ReadStudySet(LANG_CODE, studySetId)
                .Select(
                    model =>
                    {
                        _latestStudySetTerms.Add(model);

                        var audioFilesThatNeedToBeDownloaded = new List<string>();
                        if(model.AudioVersion > 0)
                        {
                            var audioFileExistsLocally = _storageService.AudioFileExistsLocally(model);
                            if(!audioFileExistsLocally)
                            {
                                audioFilesThatNeedToBeDownloaded.Add(model);
                            }
                        }
                        _storageService.GetVocabAudioDownloadUrls(audioFilesThatNeedToBeDownloaded.ToArray());

                        IMiniFlashcardViewModel vm = new MiniFlashcardViewModel(model);
                        return vm;
                    })
                .ToList();
        }

        public IObservable<IList<IVocabSectionViewModel>> GetVocabSections()
        {
            var sections = _vocabSectionRepo
                .ReadAll()
                .Select(x => MapSectionModelToVm(x))
                .ToList();

            return sections;
        }

        private IVocabSectionViewModel MapSectionModelToVm(VocabSection section)
        {
            var sectionChildrenVms = new List<IVocabSectionChildViewModel>();

            if(section.Subsections != null)
            {
                foreach(var subsection in section.Subsections)
                {
                    var subsectionSetVms = _vocabSubsectionRepo
                        .ReadSubsection(subsection.Key)
                        .SelectMany(x => x)
                        .Select(
                            x =>
                            {
                                IVocabSetViewModel vm = new VocabSetViewModel(x.Key, x.Value);
                                return vm;
                            })
                        .ToList();

                    IVocabSubsectionViewModel subsectionVm = new VocabSubsectionViewModel(subsection.Value, null);
                    sectionChildrenVms.Add(subsectionVm);
                }
            }

            if(section.StudySets != null)
            {
                foreach(var vocabSet in section.StudySets)
                {
                    vocabSet.Value.Id = vocabSet.Key;
                    IVocabSetViewModel vocabSetVm = new VocabSetViewModel(vocabSet.Value);
                    sectionChildrenVms.Add(vocabSetVm);
                }
            }

            return new VocabSectionViewModel(section, sectionChildrenVms);
        }

        public IObservable<string> GetVocabImageUrl(string imageId)
        {
            return _studyContentDownloadUrlRepo
                .ReadVocabImageUrl(imageId)
                .Catch<string, Exception>(ex => FetchVocabImageUrlFromStorage(ex, imageId));
        }

        public IObservable<IList<string>> GetVocabImageUrls(Term term)
        {
            IList<string> imageUrls = new List<string>();
            string[] imageIds = term.ImageIds.Split(',');
            var vocabImageDownloadUrls = imageIds
                .ToObservable()
                .SelectMany(imageId => GetVocabImageUrl(imageId))
                .ToList();

            return vocabImageDownloadUrls;
        }

        private IObservable<string> FetchVocabImageUrlFromStorage(Exception ex, string filename)
        {
            return _storageService
                .GetVocabImageDownloadUrls(filename)
                .Catch<string, Exception>(
                    ex2 =>
                    {
                        return Observable.Empty<string>();
                    })
                .Do(url => SaveUrlToDatabase(url, filename));
        }

        private void SaveUrlToDatabase(string url, string filename)
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
                        Console.WriteLine("Saved!");
                    },
                    ex =>
                    {
                        Console.WriteLine("Exception was thrown: {0}", ex.Message);
                    });
        }

        public IObservable<Unit> SaveVocabImageUrl(string imageId, string url)
        {
            throw new NotImplementedException();
        }
    }
}