using ReactiveUI;

namespace TongTongAdmin.Modules
{
    public interface IHangulItemViewModel
    {
        string Ko { get; set; }

        string Romanization { get; set; }

        string Char { get; set; }

        bool IsVowel { get; set; }

        bool IsComposite { get; set; }

        string SimilarSound { get; set; }

        //IDictionary<string, string> SoundToSampleWordsDict { get; set; }
    }
}
