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
using GameCtor.FirebaseStorage.DotNet;
using System.Threading.Tasks;
using Xamarin.Forms;
using Splat;

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
            var storageService = Substitute.For<IFirebaseStorageService>();
            var sut = new AudiobookListViewModel(viewStackService, repo, storageService, _schedulerProvider.Dispatcher);

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

        [Fact]
        public void Should_LeakMemory_WhenNotDisposed()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            var vm = new AudiobookListViewModel(Substitute.For<IViewStackService>(), Substitute.For<IRepository<Audiobook>>());
            var leakMonitor = new LeakMonitor<AudiobookListPage>(new AudiobookListPage() { ViewModel = vm });

            leakMonitor.IsItemAlive().Should().BeTrue();
        }

        [Fact]
        public void Should_NotLeakMemory_WhenDisposed()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            var vm = new AudiobookListViewModel(Substitute.For<IViewStackService>(), Substitute.For<IRepository<Audiobook>>());
            var leakMonitor = new LeakMonitor<AudiobookListPage>(new AudiobookListPage() { ViewModel = vm });
            //(_weakReference.Target as GrammarListPage).Sub.Dispose();
            vm = null;

            leakMonitor.IsItemAlive().Should().BeFalse();
        }

        [Fact]
        public async Task Push()
        {
            Xamarin.Forms.Mocks.MockForms.Init();

            var locator = new ModernDependencyResolver();
            locator.InitializeSplat();
            locator.InitializeReactiveUI();

            using (locator.WithResolver())
            {
                var root = new ContentPage();
                var page = new AudiobookListPage();
                await root.Navigation.PushAsync(page);
                root.Navigation.NavigationStack.Last().Should().Be(page);
            }
        }
    }
}
