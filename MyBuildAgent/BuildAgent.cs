using System;
using System.Diagnostics;
using System.Threading;

namespace MyBuildAgent
{
    class BuildAgent
    {
        private static readonly object buildLock = new object();

        public string Id { get; private set; }
        public string BuildPath { get; private set; }
        public int Time { get; private set; }
        public bool IsCanceled { get; private set; }

        private Process buildProcess;
        private Thread t = null;

        public BuildAgent(string id, string buildPath, int timeInSeconds)
        {
            Id = id;
            BuildPath = buildPath;
            Time = timeInSeconds;
            buildProcess = ConfigureBuildProcess(BuildPath);

            t = new Thread(() => Run());
            t.Start();
        }

        public void Cancel()
        {
            IsCanceled = true;
            if (t.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
                t.Interrupt();
            }
        }

        private void Run()
        {
            IsCanceled = false;
            while (!IsCanceled)
            {
                int exitCode = DoBuild();

                if (!IsCanceled)
                {
                    TimedDelay(Time);
                }
            }
        }

        private Process ConfigureBuildProcess(string BuildPath)
        {
            return new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    FileName = "dotnet",
                    Arguments = $" build {BuildPath}"
                }
            };
        }

        private int DoBuild()
        {
            lock (buildLock)
            {
                Console.WriteLine($"{Id} building...");
                buildProcess.Start();
                buildProcess.WaitForExit();
                return buildProcess.ExitCode;
            }
        }

        private void TimedDelay(int seconds)
        {
            try
            {
                Thread.Sleep(seconds * 1000);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine($"{Id} Got Interrupted!");
                // Do nothing special - just wake up!
            }
        }
    }
}
