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
using CoreAnimation;

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

                    //var size = cell.Frame.Size;
                    //cell.ClipsToBounds = true;
                    //var layer = new CALayer();
                    //layer.BackgroundColor = UIColor.LightGray.CGColor;
                    //layer.Position = new CGPoint(size.Width / 2f, -size.Height / 2f + 0.5f);
                    //layer.Bounds = new CGRect(0f, 0f, size.Width, size.Height);
                    //layer.ShadowColor = UIColor.DarkGray.CGColor;
                    //layer.ShadowOffset = new CGSize(0.5f, 0.5f);
                    //layer.ShadowOpacity = 0.8f;
                    //layer.ShadowRadius = 5.0f;
                    //cell.Layer.AddSublayer(layer);

                    cell.Layer.ShadowColor = UIColor.Black.CGColor;
                    cell.Layer.ShadowOffset = new CGSize(0f, 1f);
                    cell.Layer.ShadowOpacity = 0.6f;
                    cell.Layer.ShadowRadius = 1f;
                    cell.Layer.MasksToBounds = false;
                    cell.ClipsToBounds = false;
                    var shadowFrame = new CGRect(0f, 0f, cell.Frame.Width, cell.Frame.Height);
                    var shadowPath = UIBezierPath.FromRect(shadowFrame).CGPath;
                    cell.Layer.ShadowPath = shadowPath;

                    //var layer = new CALayer();
                    //layer.Frame = new CGRect(0, cell.Frame.Size.Height + 2f, cell.Frame.Size.Width, 2f);
                    //layer.BackgroundColor = UIColor.White.CGColor;
                    //layer.ShadowColor = UIColor.Black.CGColor;
                    //layer.ShadowOffset = new CGSize(0f, 0f);
                    //layer.ShadowRadius = 10f;
                    //layer.ShadowOpacity = 0.7f;
                    //cell.Layer.InsertSublayer(layer, 0);

                    //UIImageView innerShadowView = new UIImageView(cell.Frame);
                    //innerShadowView.ContentMode = UIViewContentMode.ScaleToFill;
                    //innerShadowView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                    //cell.AddSubview(innerShadowView);
                    //innerShadowView.Layer.MasksToBounds = true;
                    //innerShadowView.Layer.BorderColor = UIColor.LightGray.CGColor;
                    //innerShadowView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
                    //innerShadowView.Layer.BorderWidth = 1f;
                    //innerShadowView.Layer.ShadowOffset = new CGSize(0f, 0f);
                    //innerShadowView.Layer.ShadowOpacity = 0.5f;
                    //// this is the inner shadow thickness
                    //innerShadowView.Layer.ShadowRadius = 1.2f;
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
                GetCell(tableView, indexPath).BackgroundColor = UIColor.White;
                CollapseSubItemsAtIndex(tableView, _currentExpandedIndex);
                _currentExpandedIndex = -1;
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

                GetCell(tableView, indexPath).BackgroundColor = UIColor.FromRGB(245, 245, 245);
            }

            tableView.EndUpdates();

            tableView.DeselectRow(indexPath, true);
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