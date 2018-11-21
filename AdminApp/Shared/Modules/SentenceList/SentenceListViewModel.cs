using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public class SentenceListViewModel : BasePageViewModel, ISentenceListViewModel
    {
        private ISentenceItemViewModel _selectedItem;

        public SentenceListViewModel(
            IViewStackService viewStackService = null,
            IRepository<ExampleSentence> sentenceRepo = null)
                : base(viewStackService)
        {
            SentenceRepo = sentenceRepo ?? Locator.Current.GetService<IRepository<ExampleSentence>>();
        }

        public override string Title => "Sentences";

        public ReactiveCommand<Unit, Unit> LoadItems { get; }

        public ReactiveCommand<Unit, Unit> CreateItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> SaveItem { get; }

        public Interaction<string, bool> ConfirmDelete { get; }

        public IRepository<ExampleSentence> SentenceRepo { get; }

        public ObservableCollection<ISentenceItemViewModel> Items { get; }

        public ISentenceItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }
    }
}
