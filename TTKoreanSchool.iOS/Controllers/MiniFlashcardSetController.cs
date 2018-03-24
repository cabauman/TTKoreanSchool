using System;
using System.Drawing;
using System.Reactive.Disposables;
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
    [Register("MiniFlashcardsController")]
    public class MiniFlashcardSetController : ExpandableTableViewController<IMiniFlashcardsPageViewModel>
    {
        private UIBarButtonItem _displayStudyActivitiesBtn;

        public MiniFlashcardSetController()
        {
        }

        protected override int ItemCount
        {
            get { return ViewModel.Terms?.Count ?? 0; }
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
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), ParentCellId);
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), ChildCellId);

            _displayStudyActivitiesBtn = new UIBarButtonItem(UIBarButtonSystemItem.Action);
            NavigationItem.SetRightBarButtonItem(_displayStudyActivitiesBtn, true);

            ViewModel.WhenAnyValue(vm => vm.LoadVocabTerms)
                .SelectMany(x => x.Execute())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x =>
                    {
                        TableView.ReloadData();
                    },
                    x =>
                    {
                        Console.WriteLine(x.Message);
                    },
                    () =>
                    {
                        Console.WriteLine("Complete");
                    })
                .DisposeWith(SubscriptionDisposables);

            this.WhenActivated(
                disposables =>
                {
                    this.BindCommand(
                        this.ViewModel,
                        x => x.DisplayStudyActivities,
                        x => x._displayStudyActivitiesBtn)
                            .DisposeWith(disposables);
                });
        }

        protected override UITableViewCell GetParentCell(UITableView tableView, int index)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(ParentCellId);
            StyleCell(cell);
            cell.TextLabel.Text = ViewModel.Terms[index].Ko;

            return cell;
        }

        protected override UITableViewCell GetChildCell(UITableView tableView, int index)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(ChildCellId);
            StyleCell(cell);
            cell.TextLabel.Text = ViewModel.Terms[index].Translation;

            return cell;
        }

        private void StyleCell(UITableViewCell cell)
        {
            cell.Layer.ShadowColor = UIColor.Black.CGColor;
            cell.Layer.ShadowOffset = new CGSize(0f, 1f);
            cell.Layer.ShadowOpacity = 0.6f;
            cell.Layer.ShadowRadius = 1f;
            cell.Layer.MasksToBounds = false;
            cell.ClipsToBounds = false;
            var shadowFrame = new CGRect(0f, 0f, cell.Frame.Width, cell.Frame.Height);
            var shadowPath = UIBezierPath.FromRect(shadowFrame).CGPath;
            cell.Layer.ShadowPath = shadowPath;
        }
    }
}