using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using TTKoreanSchool.Services.Interfaces;

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

        public void DisplayActionSheet(string title, string message, params AlertAction[] options)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);
            foreach(var option in options)
            {
                alert.AddAction(
                    UIAlertAction.Create(
                        option.Title,
                        UIAlertActionStyle.Default,
                        action => { option.ActionToTake?.Invoke(); }));
            }

            alert.AddAction(
                UIAlertAction.Create(
                    "Cancel",
                    UIAlertActionStyle.Cancel,
                    null));

            Present(alert);
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
    }
}