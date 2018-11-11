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
        public SentenceListViewModel(
            IViewStackService viewStackService = null,
            IRepository<ExampleSentence> sentenceRepository = null)
                : base(viewStackService)
        {
            SentenceRepository = Locator.Current.GetService<IRepository<ExampleSentence>>();
        }

        public override string Title => "Sentences";

        public ReactiveCommand<Unit, Unit> AddItem { get; }

        public ReactiveCommand<Unit, Unit> DeleteItem { get; }

        public ReactiveCommand<Unit, Unit> UpdateItem { get; }

        public IRepository<ExampleSentence> SentenceRepository { get; }

        public ObservableCollection<ISentenceItemViewModel> SentenceItems { get; }
    }
}
