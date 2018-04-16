extern alias SplatAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplatAlias::Splat;
using TTKoreanSchool.Models;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.ViewModels
{
    public interface IDetailedFlashcardViewModel
    {
        string Ko { get; }

        string Romanization { get; }

        string Translation { get; }

        bool IsStarred { get; set; }

        IReadOnlyList<IExampleSentenceViewModel> Sentences { get; }
    }

    public class DetailedFlashcardViewModel : BaseViewModel, IDetailedFlashcardViewModel
    {
        public DetailedFlashcardViewModel(Term term)
        {
        }

        public DetailedFlashcardViewModel(Term term, IList<IExampleSentenceViewModel> sentences, IList<string> imageUrls = null, bool isStarred = false)
        {
            Ko = term.Ko;
            Romanization = term.Romanization;
            Translation = term.Translation;
            Sentences = sentences;

            if(term.ImageIds == null || term.ImageIds.Length == 0)
            {
                return;
            }

            //var storage = Locator.Current.GetService<IStorageService>();
            //storage.GetDownloadUrlForVocabImage(term.ImageIds[0])
            //    .Subscribe(
            //        imageUrl =>
            //        {
            //        },
            //        error =>
            //        {
            //            this.Log().Error(error.Message);
            //        });
        }

        public string Ko { get; }

        public string Romanization { get; }

        public string Translation { get; }

        public bool IsStarred { get; set; }

        public IReadOnlyList<IExampleSentenceViewModel> Sentences { get; }
    }
}