using System;

namespace Vienna
{
    public class Logger
    {
        public static void Debug(object obj)
        {
            Console.WriteLine(obj);
        }

        public static void Debug(string text)
        {
            Console.WriteLine("debug - {0}", text);
        }

        public static void Debug(string text, params object[] args)
        {
            Console.WriteLine("debug - {0}", string.Format(text, args));
        }

        public static void Error(string text)
        {
            Console.WriteLine("error - {0}", text);
        }

        public static void Error(string text, params object[] args)
        {
            Console.WriteLine("error - {0}", string.Format(text, args));
        }

        public static void Warn(string text)
        {
            Console.WriteLine("warn - {0}", text);
        }

        public static void Warn(string text, params object[] args)
        {
            Console.WriteLine("warn - {0}", string.Format(text, args));
        }
    }
}
