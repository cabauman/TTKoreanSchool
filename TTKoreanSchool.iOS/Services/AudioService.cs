using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using AVFoundation;
using Foundation;
using TTKoreanSchool.Services.Interfaces;

namespace TTKoreanSchool.iOS.Services
{
    public class AudioService : IAudioService
    {
        private AVAudioPlayer _audioPlayer;

        public void Play(string localUrl)
        {
            if(_audioPlayer != null)
            {
                _audioPlayer.Stop();
                _audioPlayer.Dispose();
            }

            NSUrl url = new NSUrl(localUrl);
            _audioPlayer = new AVAudioPlayer(url, "mp3", out NSError error);

            Observable.FromEventPattern<AVStatusEventArgs>(
                x => _audioPlayer.FinishedPlaying += x,
                x => _audioPlayer.FinishedPlaying -= x);

            _audioPlayer.Volume = 1f;
            _audioPlayer.NumberOfLoops = 0;
            _audioPlayer.Play();
        }

        public IObservable<Unit> Play2(string filename)
        {
            return Observable
                .Create<Unit>(
                    observer =>
                    {
                        var disposables = new CompositeDisposable();
                        var url = NSBundle.MainBundle.GetUrlForResource(filename, "mp3", "Audio");
                        var audioPlayer = AVAudioPlayer.FromUrl(url);
                        var finishedPlaying = Observable
                            .FromEventPattern<AVStatusEventArgs>(
                                x => audioPlayer.FinishedPlaying += x,
                                x => audioPlayer.FinishedPlaying -= x)
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