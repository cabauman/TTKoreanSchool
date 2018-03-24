extern alias SplatAlias;

using SplatAlias::Splat;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public abstract class BasePageViewModel : BaseViewModel, IPageViewModel
    {
        public void PagePopped()
        {
            var navService = Locator.Current.GetService<INavigationService>();
            navService.PagePopped();
        }
    }
}