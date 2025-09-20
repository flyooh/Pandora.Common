using System;
using System.Diagnostics;
using System.Threading;

namespace Pandora.Common
{
    /// <summary>
    /// Class Utilities.
    /// </summary>
    public static class RetryExecutor
    {
        /// <summary>
        /// Attempts an action until timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="function">The action.</param>
        /// <returns>True or false.</returns>
        public static bool TryUntilSuccessOrTimeout(TimeSpan timeout, TimeSpan interval, Func<bool> function)
        {
            Ensure.ArgumentIsNotNull(function, nameof(function));

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (true)
            {
                if (function())
                {
                    return true;
                }

                TimeSpan elapsed = stopwatch.Elapsed;
                if (elapsed > timeout)
                {
                    break;
                }

                if (interval > TimeSpan.Zero)
                {
                    var remainingTime = timeout - stopwatch.Elapsed;
                    var waitTime = TimeSpan.Compare(interval, remainingTime) < 0 ? interval : remainingTime;

                    if (waitTime > TimeSpan.Zero)
                    {
                        Thread.Sleep(waitTime);
                    }

                    Thread.Sleep(interval);
                }
            }

            return false;
        }

        /// <summary>
        /// Tries an action with several attempts.
        /// </summary>
        /// <param name="maxAttempts">The number of attempt.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="function">The action.</param>
        /// <returns>True or false.</returns>
        public static bool TryUntilSuccessOrMaxAttempts(int maxAttempts, TimeSpan interval, Func<bool> function)
        {
            Ensure.ArgumentIsNotNull(function, nameof(function));

            for (int attempt = 0; attempt < maxAttempts; ++attempt)
            {
                if (function())
                {
                    return true;
                }

                if (attempt == maxAttempts - 1)
                {
                    break;
                }

                if (interval > TimeSpan.Zero)
                {
                    Thread.Sleep(interval);
                }
            }

            return false;
        }
    }
}
