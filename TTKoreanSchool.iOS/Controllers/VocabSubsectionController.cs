﻿using System;
using System.Drawing;
using System.Reactive.Linq;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using TTKoreanSchool.Extensions;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Controllers
{
    [Register("VocabSubsectionController")]
    public class VocabSubsectionController : ReactiveViewController<IVocabSubsectionViewModel>
    {
        private static readonly string _cellId = "Cell";

        public VocabSubsectionController()
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
            Title = "Vocab Subsection";

            //ViewModel.WhenAnyValue(vm => vm.VocabSets)
            //    .Where(vocabSets => vocabSets != null)
            //    .Subscribe(
            //        vocabSets =>
            //        {
            //            TableView.ReloadData();
            //        })
            //    .DisposeWith(SubscriptionDisposables);
        }

        //public override nint RowsInSection(UITableView tableView, nint section)
        //{
        //    return ViewModel.VocabSets?.Count ?? 0;
        //}

        //public override nint NumberOfSections(UITableView tableView)
        //{
        //    return 1;
        //}

        //public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        //{
        //    var cell = tableView.DequeueReusableCell(_cellId);
        //    if(cell == null)
        //    {
        //        cell = new UITableViewCell(UITableViewCellStyle.Subtitle, _cellId);
        //        cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
        //    }

        //    cell.TextLabel.Text = ViewModel.VocabSets[indexPath.Row].Title;

        //    return cell;
        //}

        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    var vocabSet = ViewModel.VocabSets[indexPath.Row];

        //    //ViewModel.ItemSelected(vocabSet);

        //    TableView.DeselectRow(indexPath, true);
        //}
    }
}