extern alias SplatAlias;

using ReactiveUI;
using SplatAlias::System.Drawing;
using TTKoreanSchool.Models;
using TTKoreanSchool.Utils;
using TTKoreanSchool.ViewModels.Enums;

namespace TTKoreanSchool.ViewModels
{
    public interface IMatchGameCardViewModel
    {
        string Id { get; set; }

        string Text { get; set; }

        bool Hidden { get; set; }

        Color Color { get; set; }

        Color BorderColor { get; set; }

        MatchGameCardState State { get; set; }
    }

    public class MatchGameCardViewModel : BaseViewModel, IMatchGameCardViewModel
    {
        private string _text;
        private bool _hidden;
        private Color _color;
        private Color _borderColor;
        private MatchGameCardState _state;

        public MatchGameCardViewModel()
        {
        }

        public MatchGameCardViewModel(Term flashcard)
        {
        }

        public string Id { get; set; }

        public string Text
        {
            get { return _text; }
            set { this.RaiseAndSetIfChanged(ref _text, value); }
        }

        public bool Hidden
        {
            get { return _hidden; }
            set { this.RaiseAndSetIfChanged(ref _hidden, value); }
        }

        public Color Color
        {
            get { return _color; }
            set { this.RaiseAndSetIfChanged(ref _color, value); }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { this.RaiseAndSetIfChanged(ref _borderColor, value); }
        }

        public MatchGameCardState State
        {
            get
            {
                return _state;
            }

            set
            {
                switch(value)
                {
                    case MatchGameCardState.Inactive:
                        Hidden = true;
                        break;
                    case MatchGameCardState.Match:
                        Color = ColorPalette.Green200;
                        BorderColor = ColorPalette.SilverMultGreen200;
                        break;
                    case MatchGameCardState.Mismatch:
                        Color = ColorPalette.Red200;
                        BorderColor = ColorPalette.SilverMultRed200;
                        break;
                    case MatchGameCardState.Normal:
                        Hidden = false;
                        Color = Color.White;
                        BorderColor = Color.LightGray;
                        break;
                    case MatchGameCardState.Selected:
                        Color = ColorPalette.Blue200;
                        BorderColor = ColorPalette.SilverMultBlue200;
                        break;
                }

                _state = value;
            }
        }
    }
}