using System;
using GameCtor.Repository;
using TTKS.Core.Common;

namespace TTKS.Data.Repositories
{
    public class TranslationRepoFactory
    {
        private readonly RepoRegistrar _repoRegistrar;

        public TranslationRepoFactory(RepoRegistrar repoRegistrar)
        {
            _repoRegistrar = repoRegistrar;
        }

        public IRepository<Translation> Create(TranslationType translationType, string langCode)
        {
            return _repoRegistrar.GetTranslationRepo(translationType, langCode);
        }
    }
}
