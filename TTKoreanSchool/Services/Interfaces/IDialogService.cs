using System;
using System.Collections.Generic;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IDialogService
    {
        void DisplayAlert(string title, string message, string accept, string cancel);

        void DisplayActionSheet(string title, string message, IEnumerable<AlertAction> options);

        IObservable<T> DisplayActionSheet<T>(string title, string message, IEnumerable<T> options);
    }

    public struct AlertAction
    {
        public AlertAction(string title, Action actionToTake)
        {
            Title = title;
            ActionToTake = actionToTake;
        }

        public string Title { get; }

        public Action ActionToTake { get; }
    }
}