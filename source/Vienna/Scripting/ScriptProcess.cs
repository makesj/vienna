using Vienna.Processes;

namespace Vienna.Scripting
{
    public class ScriptProcess : Process
    {
        protected int Frequency { get; set; }
        protected int Time { get; set; }
        protected string InitFunction { get; set; }
        protected string UpdateFunction { get; set; }
        protected string SuccessFunction { get; set; }
        protected string FailFunction { get; set; }
        protected string AbortFunction { get; set; }
        protected dynamic ScriptObject { get; set; }
        
        public void RegisterScriptClass()
        {
        }

        //alias functions for when working in script
        public bool is_alive() { return IsAlive; }
        public bool is_dead() { return IsDead; }
        public bool is_paused() { return IsPaused; }

        // This wrapper function is needed so we can translate a script 
        //object to a regular CLR object.
        public void attach_child(dynamic child)
        {

        }

        public override void OnAbort()
        {
            
        }
    }
}
