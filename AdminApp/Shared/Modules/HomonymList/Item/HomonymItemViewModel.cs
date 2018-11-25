using System;
using ReactiveUI;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class HomonymItemViewModel : ReactiveObject, IHomonymItemViewModel
    {
        public HomonymItemViewModel(StringEntity model)
        {
            Model = model;
        }

        public StringEntity Model { get; }

        public string Value { get; set; }
    }
}
