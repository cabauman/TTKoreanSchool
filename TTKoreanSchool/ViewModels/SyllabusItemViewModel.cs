using ReactiveUI;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.ViewModels
{
    public class SyllabusItemViewModel : BaseViewModel
    {
        private SyllabusItem _syllabusItem;

        public SyllabusItemViewModel(SyllabusItem syllabusItem)
        {
            _syllabusItem = syllabusItem;
        }

        public SyllabusItem SyllabusItem
        {
            get { return _syllabusItem; }
            set { this.RaiseAndSetIfChanged(ref _syllabusItem, value); }
        }
    }
}