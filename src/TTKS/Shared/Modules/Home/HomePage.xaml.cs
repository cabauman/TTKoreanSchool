using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TTKS.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ReactiveContentPage<IHomeViewModel>
    {
		public HomePage()
		{
			InitializeComponent();

            this
                .BindCommand(ViewModel, vm => vm.NavigateToHangulSection, v => v.HangulButton);
            this
                .BindCommand(ViewModel, vm => vm.NavigateToVocabSection, v => v.VocabButton);
            this
                .BindCommand(ViewModel, vm => vm.NavigateToGrammarSection, v => v.GrammarButton);
            this
                .BindCommand(ViewModel, vm => vm.NavigateToConjugatorSection, v => v.ConjugatorButton);
        }
	}
}