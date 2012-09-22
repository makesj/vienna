using System;
using Vienna.Processes;

namespace Vienna.Scripting
{
    public class ScriptProcess : Process
    {
        protected int Frequency { get; set; }
        protected int Time { get; set; }
        protected dynamic ScriptObject { get; set; }
        
        public void RegisterScriptClass(string path, string className)
        {
            try
            {
                ScriptManager.Instance.ExecuteFile(path);
                ScriptObject = ScriptManager.Instance.GetInstance(className, this);
            }
            catch (Exception ex)
            {
                Logger.Debug("Could not create script process -> {0}", ex.Message);
                Fail();
            }          
        }

        public void AttachChildFromScript(dynamic child)
        {

        }

        public override void OnInit()
        {
            base.OnInit();
            ScriptObject.on_init();
        }

        public override void OnAbort()
        {
            ScriptObject.on_abort();
        }

        public override void  OnFail()
        {
            ScriptObject.on_fail();
        }

        public override void OnSuccess()
        {
            ScriptObject.on_success();
        }

        public override void OnUpdate(long delta)
        {
            ScriptObject.on_update(delta);
        }
    }
}
