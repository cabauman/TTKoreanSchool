using System;
using System.Collections.Generic;
using GameCtor.RxNavigation;
using ReactiveUI;
using TTKSCore.Common;

namespace TTKoreanSchool.Modules
{
    public class HomeViewModel : BasePageViewModel, IHomeViewModel
    {
        public HomeViewModel(
            AppBootstrapper appBootstrapper,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            NavigateToHangulSection = ReactiveCommand.CreateFromObservable(
                () => ViewStackService.PushPage(new HangulHomeViewModel()));

            NavigateToVocabSection = ReactiveCommand.CreateFromObservable(
                () => ViewStackService.PushPage(new VocabSetListViewModel()));

            NavigateToGrammarSection = ReactiveCommand.CreateFromObservable(
                () => ViewStackService.PushPage(new GrammarListViewModel()));

            NavigateToConjugatorSection = ReactiveCommand.CreateFromObservable(
                () => ViewStackService.PushPage(new ConjugatorViewModel()));
        }

        public override string Title => "Home";

        public ReactiveCommand NavigateToHangulSection { get; }

        public ReactiveCommand NavigateToVocabSection { get; }

        public ReactiveCommand NavigateToGrammarSection { get; }

        public ReactiveCommand NavigateToConjugatorSection { get; }
    }
}
