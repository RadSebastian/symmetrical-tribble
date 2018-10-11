using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using ThreadState = System.Threading.ThreadState;
using Timer = System.Timers.Timer;

namespace Suggeritore_Cisco.Logic
{
    internal static class MemoryThread
    {
        /// <summary>
        ///     Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        /// <summary>
        ///     Starts the Thread
        /// </summary>
        public static void Start()
        {
            T = new Thread(() =>
            {
                var t = new Timer
                {
                    Interval = 2000D //Every 2 seconds force GC collection.
                };

                t.Elapsed += (s, a) =>
                {
                    GC.Collect(GC.MaxGeneration);
                    GC.WaitForPendingFinalizers();

                    if (Environment.Is64BitProcess)
                        SetProcessWorkingSetSize64(Process.GetCurrentProcess().Handle, 
                            (UIntPtr)0xFFFFFFFFFFFFFFFF, (UIntPtr)0xFFFFFFFFFFFFFFFF);
                    else
                        SetProcessWorkingSetSize32(Process.GetCurrentProcess().Handle, 
                            (UIntPtr)0xFFFFFFFF, (UIntPtr)0xFFFFFFFF);
                };
                t.Enabled = true;
            });
            T.Start();
        }

        /// <summary>
        /// Stop the Thread.
        /// </summary>
        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }


        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetProcessWorkingSetSize32(IntPtr pProcess,
            UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize",
            SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetProcessWorkingSetSize64(IntPtr pProcess,
            UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);

    }
}
