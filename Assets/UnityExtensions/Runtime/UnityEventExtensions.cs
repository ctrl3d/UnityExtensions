using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace work.ctrl3d
{
    public static class UnityEventExtensions
    {
#if UNITY_6000_0_OR_NEWER
        public static Awaitable<T> AsAwaitable<T>(this UnityEvent<T> unityEvent)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));

            var acs = new AwaitableCompletionSource<T>();
            UnityAction<T> handler = null;
            handler = value =>
            {
                unityEvent.RemoveListener(handler);
                acs.SetResult(value);
            };

            unityEvent.AddListener(handler);
            return acs.Awaitable;
        }

        public static Awaitable AsAwaitable(this UnityEvent unityEvent)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));

            var acs = new AwaitableCompletionSource();
            UnityAction handler = null;
            handler = () =>
            {
                unityEvent.RemoveListener(handler);
                acs.SetResult();
            };

            unityEvent.AddListener(handler);
            return acs.Awaitable;
        }
#endif

        public static Task<T> AsTask<T>(this UnityEvent<T> unityEvent)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));

            var tcs = new TaskCompletionSource<T>();
            UnityAction<T> handler = null;
            handler = value =>
            {
                unityEvent.RemoveListener(handler);
                tcs.TrySetResult(value);
            };

            unityEvent.AddListener(handler);
            return tcs.Task;
        }

        public static Task AsTask(this UnityEvent unityEvent)
        {
            if (unityEvent == null) throw new ArgumentNullException(nameof(unityEvent));

            var tcs = new TaskCompletionSource<bool>();
            UnityAction handler = null;
            handler = () =>
            {
                unityEvent.RemoveListener(handler);
                tcs.TrySetResult(true);
            };

            unityEvent.AddListener(handler);
            return tcs.Task;
        }
    }
}