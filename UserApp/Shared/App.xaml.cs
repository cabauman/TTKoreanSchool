using System;
using Microsoft.AppCenter;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ReactiveUI;
using System.Threading.Tasks;
using TTKoreanSchool.Common;
using Microsoft.AppCenter.Distribute;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TTKoreanSchool
{
    public partial class App : ReactiveApplication<AppBootstrapper>
    {
        public App()
        {
            LiveReload.Init();

            InitializeComponent();

            ViewModel = new AppBootstrapper();
            Task.Run(async () => { await ViewModel.NavigateToFirstPage(); }).Wait();
            this.OneWayBind(ViewModel, vm => vm.MainView, v => v.MainPage, selector: ResolvePage);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start(
                "ios={Your App Secret};android={Your App Secret};uwp={Your App Secret}",
                typeof(Distribute));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private Page ResolvePage(object viewModel)
        {
            var viewFor = ViewLocator.Current.ResolveView(viewModel);
            var page = viewFor as Page;
            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{viewFor.GetType().FullName}' for type '{viewModel.GetType().FullName}', is not a Page.");
            }

            viewFor.ViewModel = viewModel;

            return page;
        }
    }
}
