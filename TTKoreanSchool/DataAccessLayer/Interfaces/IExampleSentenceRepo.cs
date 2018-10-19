using System;
using System.Collections.Generic;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IExampleSentenceRepo
    {
        IObservable<IDictionary<string, ExampleSentence>> ReadAll(string langCode);

        IObservable<ExampleSentence> Read(string sentenceId);
    }
}