using System;
using System.Collections.Generic;
using TTKoreanSchool.ViewModels;

namespace TTKoreanSchool.Services.Interfaces
{
    public interface IDialogService
    {
        void DisplayAlert(string title, string message, IEnumerable<IButtonViewModel> options);

        void DisplayActionSheet(string title, string message, IEnumerable<AlertAction> options);

        void DisplayActionSheet(string title, string message, IEnumerable<IButtonViewModel> options);

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