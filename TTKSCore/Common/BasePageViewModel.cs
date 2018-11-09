using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;

namespace TTKSCore.Common
{
    public abstract class BasePageViewModel : ReactiveObject, IPageViewModel
    {
        public BasePageViewModel(IViewStackService viewStackService)
        {
            ViewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();
        }

        public abstract string Title { get; }

        public IViewStackService ViewStackService { get; }
    }
}
