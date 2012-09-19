using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Vienna;

namespace ViennaConsole
{
    public static class Helper
    {
        public static Stopwatch Stopwatch = new Stopwatch();

        public static string GetExecutionPath()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return new DirectoryInfo(System.IO.Path.GetDirectoryName(location)).Parent.Parent.FullName;
        }

        public static string GetFilePath(string path)
        {
            return Path.Combine(GetExecutionPath(), path);
        }

        public static string LoadFile(string path)
        {
            var fullPath = GetFilePath(path);
            if(!File.Exists(fullPath))Logger.Error("File does not exist -> ", fullPath);
            return File.ReadAllText(fullPath);
        }

        public static void StartTimer()
        {
            Stopwatch.Restart();
        }

        public static long StopTimer()
        {
            Stopwatch.Stop();
            return Stopwatch.ElapsedMilliseconds;
        }

        public static void Loop(int maxLoops, int frequency, Action<long> action)
        {
            long delta = frequency;

            foreach (var i in Enumerable.Range(0, maxLoops + 1))
            {
                long tick = Environment.TickCount;

                action(delta);

                Thread.Sleep(frequency);

                delta = Environment.TickCount - tick;

            }
        }
    }
}
