extern alias SplatAlias;

using System;
using System.Collections.Generic;
using ReactiveUI;
using SplatAlias::Splat;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services
{
    public class NavigationPageViewModel : Stack<IPageViewModel>, IPageViewModel
    {
        public void PagePopped()
        {
        }
    }

    public abstract class BaseNavigationService : INavigationService, IEnableLogger
    {
        public BaseNavigationService(bool rootIsNavStack = true, IViewLocator viewlocator = null)
        {
            if(rootIsNavStack)
            {
                Root = new NavigationPageViewModel();
            }

            ModalStack = new Stack<IPageViewModel>();
            ViewLocator = viewlocator ?? Locator.Current.GetService<IViewLocator>();
        }

        public IPageViewModel Root { get; }

        public Stack<IPageViewModel> ModalStack { get; }

        public IViewLocator ViewLocator { get; }

        public IPageViewModel TopModal
        {
            get
            {
                var topModal = Root;

                if(ModalStack?.Count > 0)
                {
                    topModal = ModalStack.Peek();
                }

                return topModal;
            }
        }

        public IPageViewModel TopPage
        {
            get
            {
                var topPage = TopModal;

                if(topPage is NavigationPageViewModel navPage)
                {
                    topPage = navPage.Peek();
                }

                return topPage;
            }
        }

        public void PushPage(IPageViewModel viewModel, bool resetStack = false, bool animate = true)
        {
            // C# 7 feature: https://docs.microsoft.com/en-us/dotnet/csharp/pattern-matching
            if(TopModal is NavigationPageViewModel navPage)
            {
                PushPageNative(viewModel, resetStack, animate);

                if(resetStack)
                {
                    navPage.Clear();
                }

                navPage.Push(viewModel);
                this.Log().Debug("Added page '{0}' (animate '{1}') to stack.", viewModel.GetType().Name, animate);
            }
            else
            {
                throw new InvalidOperationException("Can't push a page without a navigation stack.");
            }
        }

        public void PopPage(bool animate = true)
        {
            // C# 7 feature: https://docs.microsoft.com/en-us/dotnet/csharp/pattern-matching
            if(TopModal is NavigationPageViewModel navPage)
            {
                PopPageNative(animate);
                var removedPage = navPage.Pop();
                this.Log().Debug("Removed page '{0}' from stack.", removedPage.GetType().Name);
            }
            else
            {
                throw new InvalidOperationException("Can't pop a page without a navigation stack.");
            }
        }

        public void PresentPage(IPageViewModel viewModel, bool animate = true, Action onComplete = null, bool withNavStack = false)
        {
            PresentPageNative(viewModel, animate, onComplete, withNavStack);

            var screenToPresent = viewModel;
            if(withNavStack)
            {
                var navPage = new NavigationPageViewModel();
                navPage.Push(viewModel);
                screenToPresent = navPage;
            }

            ModalStack.Push(screenToPresent);
            this.Log().Debug("Added modal '{0}' (animate '{1}') to stack.", viewModel.GetType().Name, animate);
        }

        public void DismissPage(bool animate = true, Action onComplete = null)
        {
            if(ModalStack?.Count > 0)
            {
                DismissPageNative(animate, onComplete);
                var removedModal = ModalStack.Pop();
                this.Log().Debug("Removed modal '{0}' from stack.", removedModal.GetType().Name);
            }
            else
            {
                throw new InvalidOperationException("Can't dismiss a page because the modal stack is empty.");
            }
        }

        public void PagePopped()
        {
            // C# 7 feature: https://docs.microsoft.com/en-us/dotnet/csharp/pattern-matching
            if(TopModal is NavigationPageViewModel navPage && navPage.Count > 1)
            {
                var removedPage = navPage.Pop();
                this.Log().Debug("Removed page '{0}' from stack.", removedPage.GetType().Name);
            }
            else
            {
                if(ModalStack?.Count > 0)
                {
                    var removedModal = ModalStack.Pop();
                    this.Log().Debug("Removed modal '{0}' from stack.", removedModal.GetType().Name);
                }
            }
        }

        protected abstract void PushPageNative(IPageViewModel viewModel, bool resetStack, bool animate);

        protected abstract void PopPageNative(bool animate);

        protected abstract void PresentPageNative(IPageViewModel viewModel, bool animate, Action onComplete, bool withNavStack);

        protected abstract void DismissPageNative(bool animate, Action onComplete);

        protected TView LocatePageFor<TView>(IPageViewModel viewModel)
            where TView : class
        {
            IViewFor viewFor = ViewLocator.ResolveView(viewModel);
            TView nativeView = viewFor as TView;

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