using System;
using System.Drawing;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.ViewModels;
using UIKit;
using System.Reactive.Linq;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("MiniFlashcardsController")]
    public class MiniFlashcardSetController : BaseTableViewController<IMiniFlashcardSetViewModel>
    {
        private static readonly string _cellId = "Cell";

        public MiniFlashcardSetController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            Title = "Mini Flashcards";

            ViewModel.WhenAnyValue(vm => vm.Terms)
                .Where(terms => terms != null)
                .Subscribe(
                    terms =>
                    {
                        TableView.ReloadData();
                    })
                .DisposeWith(SubscriptionDisposables);
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return ViewModel.Terms?.Count ?? 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(_cellId);
            if(cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, _cellId);
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            cell.TextLabel.Text = ViewModel.Terms[indexPath.Row].Ko;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //var term = ViewModel.Terms[indexPath.Row];
            //ViewModel.ItemSelected(term);

            TableView.DeselectRow(indexPath, true);
        }
    }
}