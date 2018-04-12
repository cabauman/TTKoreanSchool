using System;
using System.Collections.Generic;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IStarredTermsRepo
    {
        IObservable<IDictionary<string, bool>> ReadAll(string uid);

        IObservable<IDictionary<string, bool>> Read(string uid, string studySetId);
    }
}