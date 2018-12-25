﻿using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Firebase.Storage;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseAuth.DotNet;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.RxNavigation;
using GameCtor.RxNavigation.XamForms;
using GameCtor.XamarinAuth;
using LocalStorage.XamarinEssentials;
using ReactiveUI;
using Splat;
using TongTongAdmin.Modules;
using TongTongAdmin.Services;
using TTKSCore;
using TTKSCore.Config;

namespace TongTongAdmin
{
    public class AppBootstrapper : ReactiveObject
    {
        private object _mainView;

        public AppBootstrapper()
        {
            RegisterServices();
            RegisterViews();
            RegisterViewModels();
        }

        public IViewShell NavigationShell { get; private set; }

        public IFirebaseAuthService FirebaseAuthService { get; private set; }

        public object MainView
        {
            get => _mainView;
            set => this.RaiseAndSetIfChanged(ref _mainView, value);
        }

        public async Task NavigateToFirstPage()
        {
            MainView = new MasterDetailViewModel(this);
            return;
            bool isAuthenticated = await FirebaseAuthService.IsAuthenticated;
            if (isAuthenticated)
            {
                MainView = new MasterDetailViewModel(this);
            }
            else
            {
                MainView = new SignInViewModel(this);
            }
        }

        private void RegisterServices()
        {

            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            NavigationShell = new ViewShell(RxApp.TaskpoolScheduler, RxApp.MainThreadScheduler, ViewLocator.Current);
            var viewStackService = new ViewStackService(NavigationShell);
            Locator.CurrentMutable.RegisterConstant(viewStackService, typeof(IViewStackService));
            Locator.CurrentMutable.Register(() => new AuthService(), typeof(IAuthService));
            FirebaseAuthService = new FirebaseAuthService(ApiKeys.FIREBASE, new LocalStorageService());
            Locator.CurrentMutable.RegisterConstant(FirebaseAuthService, typeof(IFirebaseAuthService));
            Locator.CurrentMutable.RegisterConstant(new FirebaseStorageService(new FirebaseStorage("tt-korean-academy.appspot.com")), typeof(IFirebaseStorageService));
            Locator.CurrentMutable.RegisterConstant(new StudyContentService(), typeof(StudyContentService));
            new RepoRegistrar(FirebaseAuthService, Locator.CurrentMutable);
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new MasterDetailPage(NavigationShell), typeof(IViewFor<MasterDetailViewModel>));
            Locator.CurrentMutable.Register(() => new AudiobookListPage(), typeof(IViewFor<IAudiobookListViewModel>));
            Locator.CurrentMutable.Register(() => new AudiobookItemCell(), typeof(IViewFor<IAudiobookItemViewModel>));
            Locator.CurrentMutable.Register(() => new AudiobookPage(), typeof(IViewFor<IAudiobookViewModel>));
            Locator.CurrentMutable.Register(() => new VocabListPage(), typeof(IViewFor<IVocabListViewModel>));
            Locator.CurrentMutable.Register(() => new VocabItemCell(), typeof(IViewFor<IVocabItemViewModel>));
            Locator.CurrentMutable.Register(() => new VocabTermPage(), typeof(IViewFor<IVocabTermViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarListPage(), typeof(IViewFor<IGrammarListViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarItemCell(), typeof(IViewFor<IGrammarItemViewModel>));
            Locator.CurrentMutable.Register(() => new GrammarPrinciplePage(), typeof(IViewFor<IGrammarPrincipleViewModel>));
            Locator.CurrentMutable.Register(() => new SentenceListPage(), typeof(IViewFor<ISentenceListViewModel>));
            Locator.CurrentMutable.Register(() => new SentenceItemCell(), typeof(IViewFor<ISentenceItemViewModel>));
            Locator.CurrentMutable.Register(() => new SentencePage(), typeof(IViewFor<ISentenceViewModel>));
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new AudiobookListViewModel(), typeof(IPageViewModel), typeof(AudiobookListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new VocabListViewModel(), typeof(IPageViewModel), typeof(VocabListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new GrammarListViewModel(), typeof(IPageViewModel), typeof(GrammarListViewModel).FullName);
            Locator.CurrentMutable.Register(() => new SentenceListViewModel(), typeof(IPageViewModel), typeof(SentenceListViewModel).FullName);
        }
    }
}
