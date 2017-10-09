using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services
{
    public class NavigationScreenViewModel : Stack<IScreenViewModel>, IScreenViewModel
    {
    }

    public abstract class NavigationServiceBase : INavigationService, IEnableLogger
    {
        public NavigationServiceBase(bool rootIsNavStack, IViewLocator viewlocator = null)
        {
            if(rootIsNavStack)
            {
                Root = new NavigationScreenViewModel();
            }

            ModalStack = new Stack<IScreenViewModel>();
            ViewLocator = viewlocator ?? Locator.Current.GetService<IViewLocator>();
        }

        public IScreenViewModel Root { get; }

        public Stack<IScreenViewModel> ModalStack { get; }

        public IViewLocator ViewLocator { get; }

        public IScreenView CurrentScreen { get; protected set; }

        public IScreenViewModel TopMostPage
        {
            get
            {
                var topPage = Root;

                if(ModalStack?.Count > 0)
                {
                    topPage = ModalStack.Peek();
                }

                return topPage;
            }
        }

        public void PushScreen(IScreenViewModel viewModel, bool resetStack = false, bool animate = true)
        {
            var navScreen = TopMostPage as NavigationScreenViewModel;
            if(navScreen != null)
            {
                PushScreenNative(viewModel, resetStack, animate);

                if(resetStack)
                {
                    navScreen.Clear();
                }

                navScreen.Push(viewModel);
                this.Log().Debug("Added page '{0}' (animate '{1}') to stack.", viewModel.GetType().Name, animate);
            }
            else
            {
                throw new InvalidOperationException("Can't push a page without a navigation stack.");
            }
        }

        public void PopScreen(bool animate = true)
        {
            var navScreen = TopMostPage as NavigationScreenViewModel;
            if(navScreen != null)
            {
                PopScreenNative(animate);
                var removedPage = navScreen.Pop();
                this.Log().Debug("Removed page '{0}' from stack.", removedPage.GetType().Name);
            }
            else
            {
                throw new InvalidOperationException("Can't pop a page without a navigation stack.");
            }
        }

        public void PresentScreen(IScreenViewModel viewModel, bool animate = true, Action onComplete = null, bool withNavStack = false)
        {
            PresentScreenNative(viewModel, animate, onComplete, withNavStack);

            var screenToPresent = viewModel;
            if(withNavStack)
            {
                var navScreen = new NavigationScreenViewModel();
                navScreen.Push(viewModel);
                screenToPresent = navScreen;
            }

            ModalStack.Push(screenToPresent);
            this.Log().Debug("Added modal '{0}' (animate '{1}') to stack.", viewModel.GetType().Name, animate);
        }

        public void DismissScreen(bool animate = true, Action onComplete = null)
        {
            if(ModalStack?.Count > 0)
            {
                DismissScreenNative(animate, onComplete);
                var removedModal = ModalStack.Pop();
                this.Log().Debug("Removed modal '{0}' from stack.", removedModal.GetType().Name);
            }
        }

        public void ScreenPopped()
        {
            CurrentScreen
                .PagePopped
                .Do(
                    poppedViewModel =>
                    {
                        var navScreen = TopMostPage as NavigationScreenViewModel;
                        if(navScreen != null)
                        {
                            var removedScreen = navScreen.Pop();
                            this.Log().Debug("Removed page '{0}' from stack.", removedScreen.GetType().Name);
                        }
                        else
                        {
                            if(ModalStack?.Count > 0)
                            {
                                var removedModal = ModalStack.Pop();
                                this.Log().Debug("Removed modal '{0}' from stack.", removedModal.GetType().Name);
                            }
                        }
                    })
                .Subscribe();
        }

        protected abstract void PushScreenNative(IScreenViewModel viewModel, bool resetStack, bool animate);

        protected abstract void PopScreenNative(bool animate);

        protected abstract void PresentScreenNative(IScreenViewModel viewModel, bool animate, Action onComplete, bool withNavStack);

        protected abstract void DismissScreenNative(bool animate, Action onComplete);

        protected TView LocatePageFor<TView>(IScreenViewModel viewModel)
            where TView : class
        {
            var viewFor = ViewLocator.ResolveView(viewModel);
            var nativeView = viewFor as TView;

            if(viewFor == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}'. Be sure Splat has an appropriate registration.");
            }

            if(nativeView == null)
            {
                throw new InvalidOperationException($"Resolved view '{viewFor.GetType().FullName}' for type '{viewModel.GetType().FullName}' is not a '{typeof(TView).FullName}'.");
            }

            viewFor.ViewModel = viewModel;

            return nativeView;
        }
    }
}
