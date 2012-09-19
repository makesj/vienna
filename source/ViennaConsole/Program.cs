using System;

namespace ViennaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //new TestActors().Execute();
                new TestScripting().Execute();
                //new TestResources().Execute();
                //new TestEvents().Execute();
                //new TestProcess().Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\ndone");
            Console.Read();
        }
    }
}
