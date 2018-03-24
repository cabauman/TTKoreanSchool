using System;
using System.IO;
using TTKoreanSchool.Services;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.iOS.Services
{
    public class StudyContentStorageService : BaseStudyContentStorageService
    {
        public StudyContentStorageService()
        {
        }

        protected override string LocalRootDirectory
        {
            get
            {
                string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string root = Path.Combine(documents, "..", "Library");

                return root;
            }
        }
    }
}