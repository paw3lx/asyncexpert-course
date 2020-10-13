using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
    public class ThreadingHelpers
    {
        public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Create a thread and execute there `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `Join` to wait until created Thread finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

            var thread = new Thread(() =>
            {
                Execute(action, repeats, token, errorAction);
            });
            thread.Start();

            thread.Join();
        }

        public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)
            
            var autoEvent = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem((stateInfo) =>
            {
                Execute(action, repeats, token, errorAction);

                (stateInfo as AutoResetEvent)?.Set();
            }, autoEvent);

            autoEvent.WaitOne();
        }

        private static void Execute(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            for (int i = 0; i < repeats; i++)
            {
                if (token.IsCancellationRequested)
                {
                    errorAction?.Invoke(null);
                    break;
                }
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    errorAction?.Invoke(e);
                    break;
                }
            }
        }
    }
}
