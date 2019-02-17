using System;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public interface IHomonymItemViewModel
    {
        StringEntity Model { get; }

        string Text { get; set; }

        bool ReceivedFocus { get; set; }

        IObservable<bool> ReceivedFocusStream { get; }
    }
}
