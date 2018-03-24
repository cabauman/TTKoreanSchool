using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services
{
    public class StudyContentDataService : IStudyContentDataService
    {
        private const string LANG_CODE = "en";

        private readonly IVocabSectionRepo _vocabSectionRepo;
        private readonly IVocabSubsectionRepo _vocabSubsectionRepo;
        private readonly IExampleSentenceRepo _exampleSentenceRepo;
        private readonly IVocabTermRepo _vocabTermRepo;

        public StudyContentDataService(
            IVocabSectionRepo vocabSectionRepo,
            IVocabSubsectionRepo vocabSubsectionRepo,
            IVocabTermRepo vocabTermRepo,
            IExampleSentenceRepo exampleSentenceRepo)
        {
            _vocabSectionRepo = vocabSectionRepo;
            _vocabSubsectionRepo = vocabSubsectionRepo;
            _vocabTermRepo = vocabTermRepo;
            _exampleSentenceRepo = exampleSentenceRepo;
        }

        public IObservable<IList<IExampleSentenceViewModel>> GetSentences()
        {
            return _exampleSentenceRepo
                .ReadAll(LANG_CODE)
                .Select(
                    model =>
                    {
                        IExampleSentenceViewModel vm = new ExampleSentenceViewModel(model);
                        return vm;
                    })
                .ToList();
        }

        public IObservable<IList<IDetailedFlashcardViewModel>> GetDetailedFlashcards(string studySetId)
        {
            return _vocabTermRepo
                .ReadStudySet(LANG_CODE, studySetId)
                .Select(
                    model =>
                    {
                        IDetailedFlashcardViewModel vm = new DetailedFlashcardViewModel(model);
                        return vm;
                    })
                .ToList();
        }

        public IObservable<IList<IMiniFlashcardViewModel>> GetMiniFlashcards(string studySetId)
        {
            return _vocabTermRepo
                .ReadStudySet(LANG_CODE, studySetId)
                .Select(
                    model =>
                    {
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
            throw new NotImplementedException();
        }

        public IObservable<Unit> SaveVocabImageUrl(string imageId, string url)
        {
            throw new NotImplementedException();
        }
    }
}