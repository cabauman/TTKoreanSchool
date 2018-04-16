using TTKoreanSchool.Models;

namespace TTKoreanSchool.ViewModels
{
    public interface IExampleSentenceViewModel
    {
        string Id { get; }

        string Ko { get; set; }

        string Romanization { get; set; }

        string Translation { get; set; }
    }

    public class ExampleSentenceViewModel : IExampleSentenceViewModel
    {
        public ExampleSentenceViewModel()
        {
        }

        public ExampleSentenceViewModel(ExampleSentence sentence)
        {
        }

        public string Ko { get; set; }

        public string Romanization { get; set; }

        public string Translation { get; set; }

        public void PlayAudio()
        {
        }
    }
}