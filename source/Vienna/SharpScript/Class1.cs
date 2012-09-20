using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.SharpScript
{
    public class Temp
    {
        public static void CreateCompileAndRun(string script)
        {
            AppDomain domain = AppDomain.CreateDomain("MyDomain");

            CompilerRunner cr = (CompilerRunner)domain.CreateInstanceFromAndUnwrap("Vienna.dll", "Vienna.SharpScript.CompilerRunner");

            cr.Compile(script);

            dynamic result = cr.Activate("TestSharpScript", null);
            cr.PrintDomain();

            result.Foo();

            AppDomain.Unload(domain);
        }
    }
}
