using Vienna.Processes;

namespace ViennaConsole
{
    public class TestProcess
    {
        protected ProcessManager Manager = new ProcessManager();

        public void Execute()
        {
            var delay = new DelayProcess(1000);
            Manager.AttachProcess(delay);

            Helper.Loop(50, 100, MainLoop);
        }

        private void MainLoop(long delta)
        {
            Manager.UpdateProcesses(delta);
        }
    }
}
