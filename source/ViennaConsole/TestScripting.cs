using System;
using Vienna;
using Vienna.Scripting;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 5)]
    public class TestScripting
    {
        public void Execute()
        {
            var path = Helper.GetFilePath("Scripts/TestScript.rb");

            Helper.StartTimer();

            ScriptManager.Instance.ExecuteFile(path);

            var ms1 = Helper.StopTimer();

            Logger.Debug("script executed in {0}ms\n", ms1);

            Helper.StartTimer();

            var o = ScriptManager.Instance.GetInstance("TestScript");

            var ms2 = Helper.StopTimer();

            Logger.Debug("Instance created in {0}ms\n", ms2);

            Helper.StartTimer();

            var r = ScriptManager.Instance.Invoke(o, "instance_method", DateTime.Now);

            var ms3 = Helper.StopTimer();

            Console.WriteLine("class method invoked {0}ms\n", ms3);

            Logger.Debug("instance_method return value = {0}", r);
            
            ScriptManager.Instance.Invoke("some_global_scope_method", 37728);

        }
    }
}
