using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace LpricharCodeHour.Utils
{
    public static class TaskUtils
    {
        /// <summary>
        /// Disables CS4014. Adapted from http://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FireAndForget(this Task task) { }

        public static Task WhenCanceled(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }
}