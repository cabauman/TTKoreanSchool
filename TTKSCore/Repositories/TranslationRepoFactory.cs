using System;
using GameCtor.Repository;
using TTKSCore.Common;

namespace TTKSCore.Models
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
