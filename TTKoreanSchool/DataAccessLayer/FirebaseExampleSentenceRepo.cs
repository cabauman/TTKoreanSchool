using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using TTKoreanSchool.DataAccessLayer.Interfaces;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer
{
    public class FirebaseExampleSentenceRepo : FirebaseRepo<ExampleSentence>, IExampleSentenceRepo
    {
        private readonly ChildQuery _sentencesRef;
        private readonly ChildQuery _translationsRef;

        public FirebaseExampleSentenceRepo(FirebaseClient client)
        {
            _sentencesRef = client.Child("sentences/examples");
            _translationsRef = client.Child("sentences/translations");
        }

        public IObservable<ExampleSentence> ReadAll(string langCode)
        {
            ChildQuery translationsQuery = _translationsRef
                .Child(langCode);

            var translations = ReadAllBasicType<string>(translationsQuery);

            return ReadAll(_sentencesRef)
                .Zip(
                    translations,
                    (sentence, translation) =>
                    {
                        sentence.Translation = translation;
                        return sentence;
                    });
        }

        public IObservable<ExampleSentence> Read(string sentenceId)
        {
            ChildQuery childQuery = _sentencesRef
                .Child(sentenceId);

            return Read(childQuery, sentenceId);
        }
    }
}