using System.Reflection;
using Vienna.SharpScript;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 100)]
    public class TestSharpScript
    {
        public void Execute()
        {
            //// Setup interop between this assembly and scripts
            //SharpScriptManager.Instance.AddReference(Assembly.GetExecutingAssembly());

            var script = Helper.LoadFile("Scripts/TestSharpScript.cscript");
            //SharpScriptManager.Instance.Compile(script);

            //SharpScriptManager.Instance.GetInstance("TestSharpScript");

            Temp.CreateCompileAndRun(script);
        }
    }
}
