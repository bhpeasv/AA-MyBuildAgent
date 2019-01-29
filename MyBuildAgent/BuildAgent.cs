using System;
using System.Diagnostics;
using System.Threading;

namespace MyBuildAgent
{
    class BuildAgent
    {
        public string Id { get; private set; }
        public string BuildPath { get; private set; }
        public int Time { get; private set; }

        private bool done;
        private Thread t = null;

        public BuildAgent(string id, string buildPath, int timeInSeconds)
        {
            Id = id;
            BuildPath = buildPath;
            Time = timeInSeconds;
            t = new Thread(() => Run());
            t.Start();
        }

        public void Cancel()
        {
            t.Interrupt();
            done = true;
        }

        private void Run()
        {
            done = false;
            while (!done)
            {

                Console.WriteLine($"{Id} building...");

                Process p = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        FileName = "dotnet",
                        Arguments = $" build {BuildPath}"
                    }
                };
                p.Start();
                p.WaitForExit();
                try
                {
                    Thread.Sleep(Time * 1000);
                }
                catch (ThreadInterruptedException)
                {
                    // Do nothing special - just wake up!
                }
            }

        }
    }
}
