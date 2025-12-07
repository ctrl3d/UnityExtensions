#if UNITY_6000_0_OR_NEWER
using System;
using UnityEngine;

namespace work.ctrl3d
{
    public static class AwaitableExtensions
    {
        public static Awaitable WaitUntil(this Func<bool> condition, int pollIntervalMs = 33)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (pollIntervalMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(pollIntervalMs), "Poll interval must be positive");

            var source = new AwaitableCompletionSource();

            if (condition())
            {
                source.SetResult();
                return source.Awaitable;
            }

            var interval = TimeSpan.FromMilliseconds(pollIntervalMs);

            async void Poll()
            {
                while (!condition())
                {
                    await Awaitable.WaitForSecondsAsync((float)interval.TotalSeconds);
                }

                source.SetResult();
            }

            Poll();
            return source.Awaitable;
        }
    }
}
#endif