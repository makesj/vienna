using Vienna.Processes;
using Vienna.SharpScript;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 103)]
    public class TestSharpScriptProcess
    {
        protected SharpScriptContext ScriptContext = new SharpScriptContext();
        protected ProcessManager ProcessManager = new ProcessManager();
        const string Global = "Global";

        public void Execute()
        {
            var scripts = Helper.LoadFilesByExtension("Scripts", "*.cscript");
            ScriptContext.Compile(Global, scripts);

            //create a 2-way channel between the processes
            var process = new SharpScriptProcess();
            var proxy = ScriptContext.Activate(Global, "PrintNumbersProcess", new[] { process.AsProxy() });
            process.RegisterScriptProxy(proxy);

            ProcessManager.AttachProcess(process);

            Helper.Loop(25, 100, Update);
        }

        public void Update(long delta)
        {
            ProcessManager.UpdateProcesses(delta);
        }
    }
}
