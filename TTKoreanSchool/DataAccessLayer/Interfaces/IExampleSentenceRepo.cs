using System;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IExampleSentenceRepo
    {
        IObservable<ExampleSentence> ReadAll(string langCode);

        IObservable<ExampleSentence> Read(string sentenceId);
    }
}