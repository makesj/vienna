using System;
using Vienna;
using Vienna.SharpScript;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 101)]
    public class TestSharpScript2
    {
        public void Execute()
        {
            const string Global = "ScriptMain";

            Logger.Debug("Creating script context {0} and compiling script.", Global);

            var script = Helper.LoadFile("Scripts/MySharpScript.cscript");
            var manager = new SharpScriptContextManager();
            manager.Compile(Global, script);

            Logger.Debug("\nActivating new class of type MySharpScript");

            dynamic s1 = manager.Activate(Global, "MySharpScript", null);
            Logger.Debug("MySharpScript context = {0}", s1.ProxyContext);
            s1.Foo();
            Logger.Debug(s1.MyValue);
            s1.MyValue = "123";
            Logger.Debug(s1.MyValue);

            Logger.Debug("\nSending host process objects to script");

            var hostProxy = new ScriptProxy(this);
            s1.Bar(hostProxy);

            Logger.Debug("\nNext call should throw script error...");

            try
            {
                s1.ThrowsError();
            }
            catch
            {
            }

            manager.Dispose();

            Logger.Debug("\nNext call should throw...");

            var x = s1.ProxyContext;
        }

        public void HostFoo()
        {
            Logger.Debug("HostFoo() called from script");
        }
    }
}
