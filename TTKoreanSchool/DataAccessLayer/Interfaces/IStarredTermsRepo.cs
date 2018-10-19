using System;
using System.Collections.Generic;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface IStarredTermsRepo
    {
        // A collection of dictionaries. One for each study set with starred terms.
        IObservable<IDictionary<string, bool>> Read(string uid);
    }
}