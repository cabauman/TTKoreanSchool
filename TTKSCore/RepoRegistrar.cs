﻿using System;
using System.IO;
using Firebase.Database;
using Firebase.Database.Offline;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseDatabase.DotNet;
using GameCtor.Repository;
using Splat;
using TTKSCore.Models;

namespace TTKSCore
{
    public class RepoRegistrar
    {
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
        private const string NODE_VOCAB = "vocab";
        private const string NODE_VOCAB_IMAGES = "vocabImages";
        private const string NODE_VOCAB_AUDIO = "vocabAudio";
        private const string NODE_ADMIN_VARS = "adminVars";

        private static readonly string PATHFMT_USER = Path.Combine(NODE_USERS, "{0}");
        private static readonly string PATH_ADMIN = NODE_ADMIN;
        private static readonly string PATH_AUDIOBOOKS = Path.Combine(NODE_V2, NODE_AUTH_READABLE, NODE_AUDIOBOOKS);
        private static readonly string PATH_SENTENCES = Path.Combine(NODE_AUTH_READABLE, NODE_SENTENCES);
        private static readonly string PATH_GRAMMAR = Path.Combine(NODE_AUTH_READABLE, NODE_GRAMMAR);
        private static readonly string PATH_VOCAB = Path.Combine(NODE_AUTH_READABLE, NODE_VOCAB);
        private static readonly string PATH_VOCAB_IMAGES = Path.Combine(NODE_AUTH_READABLE, NODE_VOCAB_IMAGES);
        //private static readonly string PATH_VOCAB_AUDIO_URLS = Path.Combine(NODE_AUTH_READABLE, NODE_VOCAB_AUDIO);

        private FirebaseClient _firebaseClient;

        public RepoRegistrar(IFirebaseAuthService firebaseAuthService, IMutableDependencyResolver dependencyResolver)
        {
            _firebaseClient = CreateFirebaseClient(firebaseAuthService);

            dependencyResolver.Register(() => AudiobookRepo, typeof(IRepository<Audiobook>));
            dependencyResolver.Register(() => SentenceRepo, typeof(IRepository<ExampleSentence>));
            dependencyResolver.Register(() => GrammarRepo, typeof(IRepository<GrammarPrinciple>));
            dependencyResolver.Register(() => VocabTermRepo, typeof(IRepository<VocabTerm>));
            dependencyResolver.Register(() => VocabImageRepo, typeof(IRepository<VocabImage>));
        }

        private IRepository<Audiobook> AudiobookRepo
        {
            get => new FirebaseOfflineRepo<Audiobook>(_firebaseClient, PATH_AUDIOBOOKS);
        }

        private IRepository<ExampleSentence> SentenceRepo
        {
            get => new FirebaseOfflineRepo<ExampleSentence>(_firebaseClient, PATH_SENTENCES);
        }

        private IRepository<GrammarPrinciple> GrammarRepo
        {
            get => new FirebaseOfflineRepo<GrammarPrinciple>(_firebaseClient, PATH_GRAMMAR);
        }

        private IRepository<VocabTerm> VocabTermRepo
        {
            get => new FirebaseOfflineRepo<VocabTerm>(_firebaseClient, PATH_VOCAB);
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
