using System;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.iOS.Utils;
using TTKoreanSchool.iOS.Views.Cells;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("MatchGameController")]
    public class MatchGameController : BaseCollectionViewController<IMatchGamePageViewModel>
    {
        private UIBarButtonItem _btn;

        public MatchGameController()
            : base(GetLayout())
        {
        }

        public MatchGameController(UICollectionViewLayout layout)
            : base(layout)
        {
        }

        public static UICollectionViewLayout GetLayout()
        {
            var headerHeight = 0f;
            var insets = new UIEdgeInsets(2, 0, 0, 0);
            var interitemSpacing = 2f;
            var lineSpacing = 2f;
            var numRows = 4;
            var numCols = 3;
            var itemWidth = ViewUtil.GetItemWidthViaScreenWidth(numCols, interitemSpacing, insets);
            var availHeight = ViewUtil.ScreenHeightMinusStatusAndNavBar - headerHeight;
            var itemHeight = ViewUtil.GetItemHeightViaAvailableScreenHeight(
                availHeight,
                numRows,
                lineSpacing,
                insets);

            var layout = new UICollectionViewFlowLayout()
            {
                HeaderReferenceSize = new CGSize(0, headerHeight),
                ItemSize = new CGSize(itemWidth, itemHeight),
                SectionInset = insets,
                MinimumInteritemSpacing = interitemSpacing,
                MinimumLineSpacing = lineSpacing
            };

            return layout;
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

            // Perform any additional setup after loading the view
            CollectionView.BackgroundColor = UIColor.White;

            CollectionView.RegisterClassForCell(typeof(MatchGameCardCell), MatchGameCardCell.ReuseId);

            _btn = new UIBarButtonItem(UIBarButtonSystemItem.Action);
            NavigationItem.SetRightBarButtonItem(_btn, true);
            this.BindCommand(ViewModel, vm => vm.StartGame, v => v._btn);

            this.OneWayBind(ViewModel, vm => vm.StudyPoints, v => v.NavigationItem.Title);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return MatchGamePageViewModel.MAX_CARDS;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(MatchGameCardCell.ReuseId, indexPath) as MatchGameCardCell;

            var card = ViewModel.Cards[indexPath.Row];
            cell.InitViewModel(card);

            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = CollectionView.CellForItem(indexPath) as MatchGameCardCell;
            this.Log().Debug("Selected card: {0}", cell.ViewModel.Text);
            ViewModel.HandleCardSelection(cell.ViewModel);
        }
    }
}