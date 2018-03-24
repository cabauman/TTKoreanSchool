using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Linq;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.Models;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("VocabZoneController")]
    public class VocabZoneController : BaseTableViewController<IVocabZoneLandingPageViewModel>
    {
        private static readonly string _cellId = "Cell";

        public VocabZoneController()
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
            Title = "Vocab";
            TableView = new UITableView(View.Bounds, UITableViewStyle.Grouped);

            ViewModel.WhenAnyValue(vm => vm.LoadSections)
                .SelectMany(x => x.Execute())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x =>
                    {
                        TableView.ReloadData();
                    });

            //var abc = new ReactiveList<VocabSectionChild>();
            //this.WhenActivated(disposable =>
            //{
            //    var source = new ReactiveTableViewSource<VocabSectionChild>(
            //        TableView, abc, new NSString(""), 50f, x => x.AccessibilityIncrement());
            //    source.ElementSelected.Subscribe(x => Console.WriteLine((x as VocabSection).Id));
            //    TableView.Source = source;
            //});
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return ViewModel.Sections?[(int)section].Children.Count ?? 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return ViewModel.Sections?.Count ?? 0;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return ViewModel.Sections[(int)section].Title;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(_cellId);
            if(cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, _cellId);
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            var section = ViewModel.Sections[indexPath.Section];
            cell.TextLabel.Text = section.Children[indexPath.Row].Title;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var section = ViewModel.Sections[indexPath.Section];
            var selectedItem = section.Children[indexPath.Row];

            selectedItem.Selected();

            TableView.DeselectRow(indexPath, true);
        }
    }
}