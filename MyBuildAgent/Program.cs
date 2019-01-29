using System;

namespace MyBuildAgent
{
    class Program
    {
        const string BuildPath = @"C:\Users\bhp\source\repos\AA\Week5\MyBuildAgent";

        static void Main(string[] args)
        {
            BuildAgent agent1 = new BuildAgent("Agent1", BuildPath, 5);
            BuildAgent agent2 = new BuildAgent("Agent2", BuildPath, 8);

            Console.WriteLine("Press any key to stop Agent1");
            Console.ReadKey();

            agent1.Cancel();

            Console.WriteLine("Press any key to stop Agent2");
            Console.ReadKey();
        
            agent2.Cancel();
        }
    }
}
