using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKoreanSchool.Common;

namespace TTKoreanSchool.Modules
{
    public class HomeViewModel : BasePageViewModel, IHomeViewModel
    {
        public HomeViewModel(
            AppBootstrapper appBootstrapper,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            //NavigateToHangulSection = ReactiveCommand.CreateFromObservable(
            //    () => ViewStackService.PushPage(new StoreInfoViewModel());
        }

        public override string Title => "Home";

        public ReactiveCommand NavigateToHangulSection { get; }

        public ReactiveCommand NavigateToVocabSection { get; }

        public ReactiveCommand NavigateToGrammarSection { get; }

        public ReactiveCommand NavigateToConjugatorSection { get; }
    }
}
