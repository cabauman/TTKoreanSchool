using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Database.Offline;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseDatabase.DotNet;
using GameCtor.Repository;
using Splat;
using TTKS.Core.Common;
using TTKS.Core.Models;
using TTKS.Core.Repositories;

namespace TTKS.Core
{
    public class RepoRegistrar
    {
        private static readonly HashSet<string> SUPPORTED_LANG_CODES = new HashSet<string>{ "en" };

        private const string NODE_V2 = "v2";
        private const string NODE_ADMIN = "admin";
        private const string NODE_USERS = "users";
        private const string NODE_USER_OWNED = "userOwned";
        private const string NODE_USER_READABLE = "userReadable";
        private const string NODE_USER_WRITABLE = "userWritable";
        private const string NODE_AUTH_READABLE = "authReadable";

        private const string NODE_AUDIOBOOKS = "audiobooks";
        private const string NODE_SENTENCES = "sentences";
        private const string NODE_GRAMMAR = "grammar";
        private const string NODE_HOMONYMS = "homonyms";
        private const string NODE_VOCAB = "vocab";
        private const string NODE_VOCAB_IMAGES = "vocabImages";
        private const string NODE_VOCAB_AUDIO = "vocabAudio";
        private const string NODE_TRANSLATIONS = "translations";
        private const string NODE_EN = "en";
        private const string NODE_ADMIN_VARS = "adminVars";

        private static readonly string PATHFMT_USER = Path.Combine(NODE_USERS, "{0}");
        private static readonly string PATH_ADMIN = NODE_ADMIN;
        private static readonly string PATH_AUDIOBOOKS = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_AUDIOBOOKS);
        private static readonly string PATH_SENTENCES = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_SENTENCES);
        private static readonly string PATH_GRAMMAR = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_GRAMMAR);
        private static readonly string PATH_HOMONYMS = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_HOMONYMS);
        private static readonly string PATH_VOCAB = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_VOCAB);
        private static readonly string PATH_VOCAB_IMAGES = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_VOCAB_IMAGES);
        private static readonly string PATH_TRANSLATIONS = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_TRANSLATIONS);
        //private static readonly string PATH_VOCAB_AUDIO_URLS = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_VOCAB_AUDIO);

        private FirebaseClient _firebaseClient;

        public RepoRegistrar(IFirebaseAuthService firebaseAuthService, IMutableDependencyResolver dependencyResolver)
        {
            _firebaseClient = CreateFirebaseClient(firebaseAuthService);

            dependencyResolver.Register(() => AudiobookRepo, typeof(IRepository<Audiobook>));
            dependencyResolver.Register(() => VocabImageRepo, typeof(IRepository<VocabImage>));
            dependencyResolver.Register(() => new TranslationRepoFactory(this), typeof(TranslationRepoFactory));

            dependencyResolver.RegisterLazySingleton(() => SentenceRepo, typeof(SentenceRepository));
            dependencyResolver.RegisterLazySingleton(() => GrammarRepo, typeof(GrammarPrincipleRepository));
            dependencyResolver.RegisterLazySingleton(() => HomonymRepo, typeof(HomonymRepository));
            dependencyResolver.RegisterLazySingleton(() => VocabTermRepo, typeof(VocabTermRepository));
        }

        public IRepository<Translation> GetTranslationRepo(TranslationType translationType, string langCode)
        {
            string translationNode = null;
            switch (translationType)
            {
                case TranslationType.Grammar:
                    translationNode = NODE_GRAMMAR;
                    break;
                case TranslationType.Sentence:
                    translationNode = NODE_SENTENCES;
                    break;
                case TranslationType.Vocab:
                    translationNode = NODE_VOCAB;
                    break;
            }

            if (!SUPPORTED_LANG_CODES.Contains(langCode))
            {
                langCode = NODE_EN;
            }

            string key = string.Concat(translationNode, "-", langCode);
            string path = Path.Combine(PATH_TRANSLATIONS, translationNode, langCode);

            return new FirebaseOfflineRepo<Translation>(_firebaseClient, path, key);
        }

        private IRepository<Audiobook> AudiobookRepo
        {
            get => new FirebaseOfflineRepo<Audiobook>(_firebaseClient, PATH_AUDIOBOOKS);
        }

        private SentenceRepository SentenceRepo
        {
            get => new SentenceRepository(_firebaseClient, PATH_SENTENCES);
        }

        private GrammarPrincipleRepository GrammarRepo
        {
            get => new GrammarPrincipleRepository(_firebaseClient, PATH_GRAMMAR);
        }

        private HomonymRepository HomonymRepo
        {
            get => new HomonymRepository(_firebaseClient, PATH_HOMONYMS, NODE_HOMONYMS);
        }

        private VocabTermRepository VocabTermRepo
        {
            get => new VocabTermRepository(_firebaseClient, PATH_VOCAB);
        }

        private IRepository<VocabImage> VocabImageRepo
        {
            get => new FirebaseOfflineRepo<VocabImage>(_firebaseClient, PATH_VOCAB_IMAGES);
        }

        private FirebaseClient CreateFirebaseClient(IFirebaseAuthService firebaseAuthService)
        {
            const string BaseUrl = "https://tt-korean-academy.firebaseio.com/";

            FirebaseOptions options = new FirebaseOptions()
            {
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
                //AuthTokenAsyncFactory = async () => await firebaseAuthService.GetIdTokenAsync(),
                JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore
                }
            };

            return new FirebaseClient(BaseUrl, options);
        }
    }
}
