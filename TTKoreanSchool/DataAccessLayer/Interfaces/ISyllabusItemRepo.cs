using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using TTKoreanSchool.Models;

namespace TTKoreanSchool.DataAccessLayer.Interfaces
{
    public interface ISyllabusItemRepo
    {
        IObservable<SyllabusItem> ReadAll(string courseId);

        IObservable<Unit> Add(SyllabusItem syllabusItem, string courseId);

        IObservable<Unit> Update(SyllabusItem syllabusItem, string syllabusItemId);

        IObservable<Unit> Delete(string courseId, string syllabusItemId);

        IObservable<SyllabusItem> Observe(string courseId);
    }
}