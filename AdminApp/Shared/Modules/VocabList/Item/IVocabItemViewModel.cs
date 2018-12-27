using System;
using System.Collections.Generic;
using TTKSCore.Common;
using TTKSCore.Models;

namespace TongTongAdmin.Modules
{
    public interface IVocabItemViewModel
    {
        VocabTerm Model { get; }

        Translation EnTranslation { get; }

        //IObservable<bool> ModifiedStream { get; }

        //IObservable<bool> ModifiedEnStream { get; }

        string Ko { get; set; }

        string En { get; set; }

        string HomonymSpecifier { get; set; }

        WordClass WordClass { get; set; }

        Transitivity Transitivity { get; set; }

        string HonorificForm { get; set; }

        string PassiveForm { get; set; }

        string AdverbForm { get; set; }

        string Notes { get; set; }

        bool Modified { get; }

        void UpdateModel();

        void UpdateEnTranslation();
    }
}
