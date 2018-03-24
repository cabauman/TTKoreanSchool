//using System;
//using System.Reactive.Disposables;
//using System.Reactive.Linq;
//using Firebase.Storage;
//using Foundation;
//using System.Threading;

//namespace TTKoreanSchool.iOS.Extensions
//{
//    public static class FirebaseStorageExtensions
//    {
//        public static IObservable<NSUrl> GetDownloadUrlRx(this StorageReference storageRef)
//        {
//            return Observable.Create<NSUrl>(observer =>
//            {
//                storageRef.GetDownloadUrl((url, error) =>
//                {
//                    if(error != null)
//                    {
//                        observer.OnError(new NSErrorException(error));
//                    }
//                    else
//                    {
//                        Console.WriteLine(url.RelativePath);
//                        observer.OnNext(url);
//                        observer.OnCompleted();
//                    }
//                });

//                return Disposable.Empty;
//            });
//        }

//        public static IObservable<NSUrl> WriteToFileRx(this StorageReference storageRef, string localUrl)
//        {
//            return Observable.Create<NSUrl>(observer =>
//            {
//                Console.WriteLine("ThreadId:{0}, Path:{1}", Thread.CurrentThread.ManagedThreadId, localUrl);
//                storageRef.WriteToFile(NSUrl.FromFilename(localUrl), (url, error) =>
//                {
//                    if(error != null)
//                    {
//                        observer.OnError(new NSErrorException(error));
//                    }
//                    else
//                    {
//                        Console.WriteLine("OnNext");
//                        observer.OnNext(url);
//                        observer.OnCompleted();
//                    }
//                });

//                return Disposable.Empty;
//            });
//        }
//    }
//}