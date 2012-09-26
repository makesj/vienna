using System;
using Vienna;
using Vienna.SharpScript;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 101)]
    public class TestSharpScript
    {
        const string Global = "ScriptMain";

        public void Execute()
        {
            Logger.Debug("Creating script context {0} and compiling script.", Global);

            var script = Helper.LoadFile("Scripts/MySharpScript.cscript");
            var script2 = Helper.LoadFile("Scripts/AnotherScriptClass.cscript");

            var manager = new SharpScriptContext();
            manager.Compile(Global, new[]{script,script2});

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

        public void HostFoo(StringMap args)
        {
            Logger.Debug("HostFoo() called from script, args={0}", args);
        }

        public dynamic Foo()
        {
            Print(Bar, Bar);
            return null;
        }

        public dynamic Bar()
        {
            return "Hello";
        }

        public void Print(Func<dynamic> x, Func<dynamic> y)
        {
            Console.WriteLine(x().ToString());
            Console.WriteLine(x());
        }
    }
}
