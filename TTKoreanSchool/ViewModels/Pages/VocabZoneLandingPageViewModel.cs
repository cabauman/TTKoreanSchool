extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabZoneLandingPageViewModel : IPageViewModel
    {
        IList<IVocabSectionViewModel> Sections { get; }

        ReactiveCommand<Unit, IList<IVocabSectionViewModel>> LoadSections { get; }
    }

    public class VocabZoneLandingPageViewModel : BasePageViewModel, IVocabZoneLandingPageViewModel
    {
        private ObservableAsPropertyHelper<IList<IVocabSectionViewModel>> _sections;

        public VocabZoneLandingPageViewModel()
        {
            LoadSections = ReactiveCommand.CreateFromObservable(() => GetSections());
            LoadSections.ToProperty(this, x => x.Sections, out _sections);
            LoadSections.ThrownExceptions.Subscribe(
                ex =>
                {
                    throw new Exception(ex.ToString());
                });
        }

        public ReactiveCommand<Unit, IList<IVocabSectionViewModel>> LoadSections { get; set; }

        public IList<IVocabSectionViewModel> Sections
        {
            get { return _sections.Value; }
        }

        public IObservable<IList<IVocabSectionViewModel>> GetSections()
        {
            var studyContentDataService = Locator.Current.GetService<IStudyContentDataService>();
            return studyContentDataService.GetVocabSections();
        }
    }
}