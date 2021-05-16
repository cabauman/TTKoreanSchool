using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TTKS.Presentation.Modules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchGamePage : ReactiveContentPage<IMatchGameViewModel>
    {
		public MatchGamePage()
		{
			InitializeComponent();
		}
	}
}