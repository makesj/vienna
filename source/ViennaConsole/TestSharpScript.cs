using System;
using Vienna;
using Vienna.SharpScript;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 100)]
    public class TestSharpScript
    {
        public void Execute()
        {
            Logger.Debug("Creating script domain");
            var script = Helper.LoadFile("Scripts/MySharpScript.cscript");

            AppDomain domain = AppDomain.CreateDomain("ScriptDomain");

            var cr = (CompilerRunner)domain.CreateInstanceFromAndUnwrap("Vienna.dll", "Vienna.SharpScript.CompilerRunner");
            cr.PrintDomain();

            cr.Compile(script);

            Logger.Debug("\nCreating proxy to ScriptDomain");

            var script1 = cr.Activate("MySharpScript", null);
            dynamic d = new DynamicScriptProxy(script1);

            d.Foo();
            var y = d.MyValue;

            Logger.Debug("MySharpScript.MyValue = " + y);

            d.MyValue = "1234";

            y = d.MyValue;

            Logger.Debug("MySharpScript.MyValue = " + y);

            Logger.Debug("Script proxy domain = " + d.ProxyDomain);

            Logger.Debug("\nCreating proxy to Host process");

            var hostProxy = new ScriptProxy(this);


            var script2 = cr.Activate("MySharpScript", null);
            script2.InvokeMethod("Bar", new[] { hostProxy });


            AppDomain.Unload(domain);

            Logger.Debug("\nNext line should throw:");

            script2.InvokeMethod("Bar", new[] { hostProxy });

        }

        public void Foo()
        {
            Console.WriteLine("Hello! from host process " + AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
