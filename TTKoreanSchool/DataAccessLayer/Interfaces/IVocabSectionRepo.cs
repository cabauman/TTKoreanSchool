using System;
using System.Collections.Generic;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IVocabSectionRepo
    {
        IObservable<VocabSection> ReadAll();
    }
}