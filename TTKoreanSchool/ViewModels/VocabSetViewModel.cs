extern alias SplatAlias;

using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IVocabSetViewModel : IVocabSectionChildViewModel
    {
    }

    public class VocabSetViewModel : BaseViewModel, IVocabSetViewModel
    {
        private VocabSectionChild _model;
        private INavigationService _navService;

        public VocabSetViewModel(
            VocabSectionChild model,
            INavigationService navService = null)
        {
            _model = model;
            _navService = navService ?? Locator.Current.GetService<INavigationService>();
        }

        public VocabSetViewModel(string id, string title)
        {
            _model = new VocabSectionChild
            {
                Id = id,
                Title = title,
                IconId = null
            };
        }

        public string Title
        {
            get { return _model.Title; }
        }

        public IReadOnlyList<Term> Terms { get; }

        public void Selected()
        {
            var page = new MiniFlashcardsPageViewModel(_model.Id);
            _navService.PushPage(page);
        }
    }
}