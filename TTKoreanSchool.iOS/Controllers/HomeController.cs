﻿using System;
using System.Drawing;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using Splat;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.iOS.Utils;
using TTKoreanSchool.iOS.Views.Cells;
using TTKoreanSchool.iOS.Views.Headers;
using TTKoreanSchool.ViewModels;
using UIKit;
using TTKoreanSchool.Utils;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("HomeController")]
    public class HomeController : BaseCollectionViewController<IHomeViewModel>
    {
        public HomeController()
            : base()
        {
        }

        public HomeController(UICollectionViewLayout layout)
            : base(layout)
        {
        }

        public static UICollectionViewLayout GetLayout()
        {
            var headerHeight = 200f;
            var insets = new UIEdgeInsets(2, 0, 0, 0);
            var interitemSpacing = 2f;
            var lineSpacing = 2f;
            var numRows = 3;
            var numCols = 2;
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
            CollectionView.BackgroundColor = ColorPalette.Amber.ToNative();

            CollectionView.RegisterNibForCell(AppSectionCell.Nib, AppSectionCell.Key);
            CollectionView.RegisterClassForSupplementaryView(
                typeof(LearningProgressHeader),
                UICollectionElementKindSection.Header,
                LearningProgressHeader.Key);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ViewModel.AppSections.Length;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(AppSectionCell.Key, indexPath) as AppSectionCell;

            var section = ViewModel.AppSections[indexPath.Row];
            cell.Image = UIImage.FromBundle(section.ImageName);
            cell.Title = section.Title;

            return cell;
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            var header = collectionView.DequeueReusableSupplementaryView(elementKind, LearningProgressHeader.Key, indexPath) as LearningProgressHeader;

            return header;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            this.Log().Debug("Section selected");
            ViewModel.AppSections[indexPath.Row].GoToSection
                .Execute()
                .Subscribe()
                .DisposeWith(SubscriptionDisposables);
        }
    }
}