﻿using System;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;

namespace TTKS.Core
{
    public static class ReactiveUIExtensions
    {
        /// <summary>
        /// Turns *view model* activation into an IObservable<bool> stream.
        /// </summary>
        /// <remarks>
        // Credit: Kent Boogaart
        // https://github.com/kentcb/YouIandReactiveUI
        /// </remarks>
        public static IObservable<bool> GetIsActivated(this IActivatableViewModel @this) =>
            Observable
                .Merge(
                    @this.Activator.Activated.Select(_ => true),
                    @this.Activator.Deactivated.Select(_ => false))
                .Replay(1)
                .RefCount();

        /// <summary>
        /// Turns *view* activation into an IObservable<bool> stream.
        /// </summary>
        /// <remarks>
        // Credit: Kent Boogaart
        // https://github.com/kentcb/YouIandReactiveUI
        /// </remarks>
        public static IObservable<bool> GetIsActivated(this IActivatableView @this)
        {
            var activationForViewFetcher = Locator.Current.GetService<IActivationForViewFetcher>();
            return activationForViewFetcher
                .GetActivationForView(@this)
                .Replay(1)
                .RefCount();
        }
    }
}
