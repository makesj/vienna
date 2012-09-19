using Vienna.Processes;

namespace Vienna.Scripting
{
    public class ScriptProcess : Process
    {
        protected int Frequency { get; set; }
        protected int Time { get; set; }

        protected dynamic InitFunction { get; set; }
        protected dynamic UpdateFunction { get; set; }
        protected dynamic SuccessFunction { get; set; }
        protected dynamic FailFunction { get; set; }
        protected dynamic AbortFunction { get; set; }
        protected dynamic Self { get; set; }
        
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
