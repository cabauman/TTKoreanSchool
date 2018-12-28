using System;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IHomonymItemViewModel
    {
        StringEntity Model { get; }

        string Text { get; set; }

        bool ReceivedFocus { get; set; }

        IObservable<bool> ReceivedFocusStream { get; }
    }
}
