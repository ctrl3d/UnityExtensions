using System;
using System.Threading.Tasks;

namespace work.ctrl3d
{
    public static class TaskExtensions
    {
        // Default poll interval of 33 ms ~ one frame at 30 FPS (1000 ms ~ 30 ! 33.33 ms)
        public static async Task<bool> WaitUntil(this Func<bool> condition, int timeoutMs = -1, int pollIntervalMs = 33)
        {
            if(condition == null) throw new ArgumentNullException(nameof(condition));
            if(pollIntervalMs <= 0) throw new ArgumentOutOfRangeException(nameof(pollIntervalMs), "Poll interval must be greater than zero.");
            
            var waitTask = RunWaitLoop(condition, pollIntervalMs);

            if (timeoutMs < 0)
            {
                await waitTask;
                return true;
            }

            var timeoutTask = Task.Delay(timeoutMs);
            var finished = await Task.WhenAny(waitTask, timeoutTask);
            return finished == waitTask;
        }

        static async Task RunWaitLoop(Func<bool> condition, int pollIntervalMs)
        {
            while (!condition())
                await Task.Delay(pollIntervalMs).ConfigureAwait(false);
        }
    }
}