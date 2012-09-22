using Vienna.Processes;

namespace Vienna.SharpScript
{
    public class SharpScriptProcess : Process
    {
        private dynamic _proxy;

        public void RegisterScriptProxy(DynamicScriptProxy proxy)
        {
            _proxy = proxy;
        }

        public override void OnInit()
        {
            base.OnInit();
            _proxy.OnInit();
        }

        public override void OnAbort()
        {
            _proxy.OnAbort();
        }

        public override void OnFail()
        {
            _proxy.OnFail();
        }

        public override void OnSuccess()
        {
            _proxy.OnSuccess();
        }

        public override void OnUpdate(long delta)
        {
            _proxy.OnUpdate(delta);
        }

        public override void Destroy()
        {
            _proxy = null;
        }

        public ScriptProxy AsProxy()
        {
            return new ScriptProxy(this);
        }
    }
}
