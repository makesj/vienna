using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
            return new DirectoryInfo(System.IO.Path.GetDirectoryName(location)).FullName;
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

    public class TestCaseAttribute : Attribute
    {
        public int CaseNumber { get; set; }
    }

    public class TestCaseSelector
    {
        protected List<Type> TestCases;

        public TestCaseSelector()
        {
            var map = Assembly.GetExecutingAssembly().GetTypesWithAttribute<TestCaseAttribute>();
            var myList = map.ToList();
            myList.Sort((firstPair, nextPair) => firstPair.Value.CaseNumber.CompareTo(nextPair.Value.CaseNumber));
            TestCases = myList.Select(x => x.Key).ToList();
        }

        public void PrintSelection()
        {
            Console.WriteLine("Choose Wisely:\n");
            for (var i = 0; i < TestCases.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, TestCases[i].Name);
            }
            if (TestCases.Count == 0)
            {
                Console.WriteLine("Nothing to see here.");
            }
            Console.Write("\nSelection: ");
        }

        public void Select(string input)
        {
            int number;
            int.TryParse(input, NumberStyles.Integer, null, out number);

            if (number == 0 || number > TestCases.Count)
            {
                Console.WriteLine("You chose... poorly");
                return;
            }

            Console.Clear();

            var type = TestCases[number - 1];
            dynamic obj = Activator.CreateInstance(type);
            obj.Execute();
        }
    }

}
