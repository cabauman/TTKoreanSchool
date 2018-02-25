using System;
using System.IO;
using AVFoundation;
using Firebase.Storage;
using Foundation;
using Plugin.Connectivity;
using Splat;
using TTKoreanSchool.Services.Interfaces;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading;

namespace TTKoreanSchool.iOS.Services
{
    public class AudioService : IAudioService
    {
        private readonly IFirebaseStorageService _storageService;
        private readonly ISpeechService _speechService;
        private readonly LocalStorageService _localStorageService;

        private AVAudioPlayer _audioPlayer;

        public AudioService()
        {
            _storageService = Locator.Current.GetService<IFirebaseStorageService>();
            _speechService = Locator.Current.GetService<ISpeechService>();
            _localStorageService = Locator.Current.GetService<LocalStorageService>();
        }

        public void Play(string filename, string text)
        {
            var localUrl = _localStorageService.GetVocabAudioFileUrl(filename);

            var root = Storage.DefaultInstance.GetRootReference();

            if(File.Exists(localUrl))
            {
                PlayClip(localUrl);
            }
            else
            {
                if(CrossConnectivity.Current.IsConnected)
                {
                    DownloadAndPlay(filename, localUrl, text);
                }
                else
                {
                    _speechService.Speak(text);
                }
            }
        }

        public void DownloadAndPlay(string filename, string localUrl, string text)
        {
            _storageService.DownloadVocabAudio(filename, localUrl)
                .Subscribe(
                    url =>
                    {
                        PlayClip(localUrl);
                    },
                    error =>
                    {
                        _speechService.Speak(text);
                    });
        }

        public void PlayClip(string filePath)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.Stop();
                _audioPlayer.Dispose();
            }

            NSUrl url = new NSUrl(filePath);
            _audioPlayer = new AVAudioPlayer(url, "mp3", out NSError error);

            Observable.FromEventPattern<AVStatusEventArgs>(
                x => _audioPlayer.FinishedPlaying += x,
                x => _audioPlayer.FinishedPlaying -= x);

            _audioPlayer.Volume = 1f;
            _audioPlayer.NumberOfLoops = 0;
            _audioPlayer.Play();
        }

        public IObservable<Unit> Play2(string name)
        {
            return Observable
                .Create<Unit>(
                    observer =>
                    {
                        var disposables = new CompositeDisposable();
                        var url = NSBundle.MainBundle.GetUrlForResource(name, "mp3", "Audio");
                        var audioPlayer = AVAudioPlayer.FromUrl(url);
                        var finishedPlaying = Observable
                            .FromEventPattern<AVStatusEventArgs>(x => audioPlayer.FinishedPlaying += x, x => audioPlayer.FinishedPlaying -= x)
                            .FirstAsync()
                            .Select(_ => Unit.Default)
                            .Publish();

                        finishedPlaying
                            .Subscribe(observer)
                            .DisposeWith(disposables);

                        finishedPlaying
                            .ObserveOn(new SynchronizationContextScheduler(SynchronizationContext.Current)) //mainScheduler
                            .Subscribe(_ => audioPlayer.Dispose())
                            .DisposeWith(disposables);

                        finishedPlaying
                            .Connect()
                            .DisposeWith(disposables);

                        audioPlayer.Play();

                        return disposables;
                    });
        }
    }
}