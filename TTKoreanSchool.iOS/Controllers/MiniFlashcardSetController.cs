using System;
using System.Drawing;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.ViewModels;
using UIKit;
using System.Reactive.Linq;
using CoreGraphics;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("MiniFlashcardsController")]
    public class MiniFlashcardSetController : BaseTableViewController<IMiniFlashcardSetViewModel>
    {
        private static readonly string _parentCellId = "ParentCell";
        private static readonly string _childCellId = "ChildCell";

        private int _currentExpandedIndex = -1;

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
            int numExpandedCells = (_currentExpandedIndex > -1) ? 1 : 0;

            return ViewModel.Terms?.Count + numExpandedCells ?? 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cellId = IsChild(indexPath) ? _childCellId : _parentCellId;
            UITableViewCell cell = tableView.DequeueReusableCell(cellId);

            if(cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellId);
                if(cellId == _childCellId)
                {
                    cell.BackgroundColor = UIColor.FromRGB(245, 245, 245);
                    cell.Layer.CornerRadius = 3;
                    cell.Layer.ShadowColor = UIColor.LightGray.CGColor;
                    cell.Layer.ShadowOffset = new CGSize(0, 1);
                    cell.Layer.ShadowOpacity = 1;
                    cell.Layer.ShadowRadius = 2;
                    cell.SeparatorInset = UIEdgeInsets.Zero;
                }
            }

            if(cellId == _parentCellId)
            {
                int parentIdx = (_currentExpandedIndex > -1 && indexPath.Row > _currentExpandedIndex) ? indexPath.Row - 1 : indexPath.Row;
                cell.TextLabel.Text = ViewModel.Terms[parentIdx].Ko;
            }
            else
            {
                cell.TextLabel.Text = ViewModel.Terms[indexPath.Row - 1].Translation;
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if(IsChild(indexPath))
            {
                // Handle selection of child cell
                tableView.DeselectRow(indexPath, true);
                return;
            }

            tableView.BeginUpdates();

            if(_currentExpandedIndex == indexPath.Row)
            {
                CollapseSubItemsAtIndex(tableView, _currentExpandedIndex);
                _currentExpandedIndex = -1;
                tableView.DeselectRow(indexPath, true);
            }
            else
            {
                var shouldCollapse = _currentExpandedIndex > -1;
                if(shouldCollapse)
                {
                    CollapseSubItemsAtIndex(tableView, _currentExpandedIndex);
                }

                _currentExpandedIndex = (shouldCollapse && indexPath.Row > _currentExpandedIndex) ? indexPath.Row - 1 : indexPath.Row;
                ExpandItemAtIndex(tableView, _currentExpandedIndex);
            }

            tableView.EndUpdates();
        }

        private bool IsChild(NSIndexPath indexPath)
        {
            return _currentExpandedIndex > -1 &&
                indexPath.Row > _currentExpandedIndex &&
                indexPath.Row <= _currentExpandedIndex + 1;
        }

        private void CollapseSubItemsAtIndex(UITableView tableView, int index)
        {
            tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
        }

        private void ExpandItemAtIndex(UITableView tableView, int index)
        {
            int insertPos = index + 1;
            tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
        }
    }
}