using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using Microsoft.Reactive.Testing;
using NSubstitute;
using ReactiveUI;
using System.Reactive;
using TongTongAdmin.Modules;
using TTKSCore.Models;
using Xunit;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace TTKoreanSchool.Tests
{
    public class AudiobookListViewModelTest
    {
        private TestSchedulers _schedulerProvider;

        public AudiobookListViewModelTest()
        {
            _schedulerProvider = new TestSchedulers();
        }

        [Fact]
        public void Should_LoadRepoItemsIntoSourceList_IfAnyExist()
        {
            // Arrange
            IEnumerable<Audiobook> audiobooks = new[] { new Audiobook(), new Audiobook(), new Audiobook() };
            var expected = audiobooks.Count();
            var viewStackService = Substitute.For<IViewStackService>();
            var repo = Substitute.For<IRepository<Audiobook>>();
            var sut = new AudiobookListViewModel(viewStackService, repo, _schedulerProvider.Dispatcher);

            var onNext = Notification.CreateOnNext(audiobooks);
            var obs = _schedulerProvider.Dispatcher.CreateColdObservable(new Recorded<Notification<IEnumerable<Audiobook>>>(0, onNext));
            repo.GetItems(true).Returns(obs);

            // Act
            sut.LoadItems.Execute().Subscribe();
            //_schedulerProvider.Dispatcher.Start();
            _schedulerProvider.Dispatcher.AdvanceBy(2);

            // Assert
            sut.AudiobookItems.Should().HaveCount(expected);
        }
    }
}
