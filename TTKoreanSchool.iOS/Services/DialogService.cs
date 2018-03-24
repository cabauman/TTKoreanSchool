using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TTKoreanSchool.Services.Interfaces;
using TTKoreanSchool.ViewModels;
using UIKit;

namespace TTKoreanSchool.iOS.Services
{
    public class DialogService : IDialogService
    {
        public void DisplayAlert(string title, string message, string accept, string cancel)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, action => { }));

            Present(alert);
        }

        public void DisplayActionSheet(string title, string message, IEnumerable<IButtonViewModel> options) //AlertAction
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
            foreach(var option in options)
            {
                alert.AddAction(
                    UIAlertAction.Create(
                        option.Title,
                        UIAlertActionStyle.Default,
                        action => { option.Command.Execute().Subscribe(); }));
                        //action => { option.ActionToTake?.Invoke(); }));
            }

            alert.AddAction(
                UIAlertAction.Create(
                    "Cancel",
                    UIAlertActionStyle.Cancel,
                    null));

            Present(alert);
        }

        public IObservable<T> DisplayActionSheet<T>(string title, string message, IEnumerable<T> options)
        {
            return Observable.Create<T>(observer =>
            {
                var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
                foreach(var option in options)
                {
                    var alertAction = UIAlertAction.Create(
                        "",
                        UIAlertActionStyle.Default,
                        action
                            =>
                            {
                                observer.OnNext(option);
                                observer.OnCompleted();
                            });

                    alert.AddAction(alertAction);
                }

                var cancelAction = UIAlertAction.Create(
                    "Cancel",
                    UIAlertActionStyle.Cancel,
                    action
                        =>
                        {
                            observer.OnCompleted();
                        });

                alert.AddAction(cancelAction);

                Present(alert);

                return Disposable.Empty;
            });
        }

        public void DisplayActionSheet(string title, string message, Action<int> onSelect, params string[] options)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
            for(int i = 0; i < options.Length; ++i)
            {
                alert.AddAction(
                    UIAlertAction.Create(
                        options[i],
                        UIAlertActionStyle.Default,
                        action => { int idx = i; onSelect?.Invoke(idx); }));
            }

            Present(alert);
        }

        private void Present(UIAlertController alert)
        {
            // TODO: Add a utility method to get the top-most controller.
            var window = UIApplication.SharedApplication.KeyWindow;
            var controller = window.RootViewController;
            controller.PresentViewController(alert, true, null);
        }

        public void DisplayActionSheet(string title, string message, IEnumerable<AlertAction> options)
        {
            throw new NotImplementedException();
        }
    }
}