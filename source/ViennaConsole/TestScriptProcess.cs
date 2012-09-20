using Vienna.Processes;
using Vienna.Scripting;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 6)]
    public class TestScriptProcess
    {
        protected ProcessManager ProcessManager = new ProcessManager();

        public void Execute()
        {
            var searchPath = Helper.GetFilePath("Scripts");
            ScriptManager.Instance.AddSearchPaths(new [] { searchPath });

            var path = Helper.GetFilePath("Scripts/PrintNumbersProcess.rb");

            var script = new ScriptProcess();
            script.RegisterScriptClass(path, "PrintNumbersProcess");

            ProcessManager.AttachProcess(script);

            Helper.Loop(25, 100, Update);
        }

        public void Update(long delta)
        {
            ProcessManager.UpdateProcesses(delta);
        }
    }
}
