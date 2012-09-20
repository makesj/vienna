using System;

namespace ViennaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var selector = new TestCaseSelector();
                selector.PrintSelection();
                var input = Console.ReadLine();
                selector.Select(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\n\ndone");
            Console.Read();
        }
    }
}
