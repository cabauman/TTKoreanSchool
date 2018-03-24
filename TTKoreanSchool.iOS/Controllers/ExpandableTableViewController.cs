using System;
using System.Drawing;
using System.Reactive.Linq;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("ExpandableTableViewController")]
    public abstract class ExpandableTableViewController<TViewModel> : BaseTableViewController<TViewModel>
        where TViewModel : class, IPageViewModel
    {
        protected static readonly string ParentCellId = "ParentCell";
        protected static readonly string ChildCellId = "ChildCell";

        private int _expandedIndex = -1;

        public ExpandableTableViewController()
        {
        }

        protected abstract int ItemCount { get; }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public sealed override nint RowsInSection(UITableView tableView, nint section)
        {
            int numExpandedCells = (_expandedIndex > -1) ? 1 : 0;

            return ItemCount + numExpandedCells;
        }

        public sealed override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public sealed override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if(IsChild(indexPath))
            {
                return GetChildCell(tableView, indexPath.Row - 1);
            }
            else
            {
                int parentIdx = indexPath.Row;
                if(_expandedIndex > -1 && indexPath.Row > _expandedIndex)
                {
                    parentIdx -= 1;
                }

                return GetParentCell(tableView, parentIdx);
            }
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

            if(_expandedIndex == indexPath.Row)
            {
                GetCell(tableView, indexPath).BackgroundColor = UIColor.White;
                CollapseSubItemsAtIndex(tableView, _expandedIndex);
                _expandedIndex = -1;
            }
            else
            {
                var shouldCollapse = _expandedIndex > -1;
                if(shouldCollapse)
                {
                    CollapseSubItemsAtIndex(tableView, _expandedIndex);
                }

                _expandedIndex = (shouldCollapse && indexPath.Row > _expandedIndex) ? indexPath.Row - 1 : indexPath.Row;
                ExpandItemAtIndex(tableView, _expandedIndex);

                GetCell(tableView, indexPath).BackgroundColor = UIColor.FromRGB(245, 245, 245);
            }

            tableView.EndUpdates();

            tableView.DeselectRow(indexPath, true);
        }

        protected abstract UITableViewCell GetParentCell(UITableView tableView, int index);

        protected abstract UITableViewCell GetChildCell(UITableView tableView, int index);

        private bool IsChild(NSIndexPath indexPath)
        {
            return _expandedIndex > -1 &&
                indexPath.Row > _expandedIndex &&
                indexPath.Row <= _expandedIndex + 1;
        }

        private void CollapseSubItemsAtIndex(UITableView tableView, int index)
        {
            tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
        }

        private void ExpandItemAtIndex(UITableView tableView, int index)
        {
            tableView.InsertRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
        }
    }
}