using System;
using System.Collections.Generic;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IVocabSubsectionRepo
    {
        IObservable<IDictionary<string, string>> ReadSubsection(string subsectionId);
    }
}