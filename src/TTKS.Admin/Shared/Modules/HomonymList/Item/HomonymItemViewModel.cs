using System;
using ReactiveUI;
using TTKS.Core.Models;

namespace TTKS.Admin.Modules
{
    public class HomonymItemViewModel : ReactiveObject, IHomonymItemViewModel
    {
        private string _text;
        private bool _receivedFocus;

        public HomonymItemViewModel(StringEntity model)
        {
            Model = model;
            _text = model.Value;

            this
                .WhenAnyValue(x => x.Text)
                .Subscribe(text => Model.Value = text);

            ReceivedFocusStream = this.WhenAnyValue(x => x.ReceivedFocus);
        }

        public StringEntity Model { get; }

        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public bool ReceivedFocus
        {
            get => _receivedFocus;
            set => this.RaiseAndSetIfChanged(ref _receivedFocus, value);
        }

        public IObservable<bool> ReceivedFocusStream { get; }
    }
}
