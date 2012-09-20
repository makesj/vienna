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
            var path = Helper.GetFilePath("Scripts/PrintNumbersProcess.rb");
            ScriptManager.Instance.ExecuteFile(path);

            var o = ScriptManager.Instance.GetInstance("PrintNumbersProcess");
            var r = ScriptManager.Instance.Invoke(o, "add_actor", "xxxx");

            Logger.Debug(r);

            ScriptManager.Instance.Invoke("some_method");

            ScriptManager.Instance.Invoke("some_other_method", 1, DateTime.Now);




        }
    }
}
