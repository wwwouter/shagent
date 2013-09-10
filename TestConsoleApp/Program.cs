using System;
using System.Configuration;
using System.Threading;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting test console app");

            var millisecondsTimeout = int.Parse(ConfigurationManager.AppSettings["SleepTimeInMs"]);

            for (int i = 0; i < millisecondsTimeout/1000; i++)
            {
                Console.WriteLine("progress after " + i + " seconds");

                Thread.Sleep(1000);
            }

            Console.WriteLine("App Done");
        }
    }
}
