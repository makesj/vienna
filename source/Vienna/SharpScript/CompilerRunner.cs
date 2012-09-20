using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Vienna.SharpScript
{
    public class CompilerRunner : MarshalByRefObject
    {
        private Assembly assembly = null;

        public void PrintDomain()
        {
            Console.WriteLine("Object is executing in AppDomain \"{0}\"",
                AppDomain.CurrentDomain.FriendlyName);
        }

        public bool Compile(string code)
        {
            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.ReferencedAssemblies.Add("system.dll");
            parameters.ReferencedAssemblies.Add("system.core.dll");
            parameters.ReferencedAssemblies.Add("Vienna.dll");

            var result = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (result.Errors.HasErrors)
            {
                var sb = new StringBuilder().AppendLine("Errors detected in script:");
                foreach (CompilerError error in result.Errors)
                {
                    if (error.IsWarning) continue;
                    sb.Append(error.Line)
                        .Append(" - ")
                        .Append(error.ErrorText)
                        .AppendLine();
                }
                Logger.Error(sb.ToString());

                throw new Exception("Script compilation failed.");
            }
            this.assembly = result.CompiledAssembly;

            return this.assembly != null;
        }

        public object Run(string typeName, string methodName, object[] args)
        {
            var type = this.assembly.GetType(typeName);

            return type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, assembly, args);
        }

        public ScriptProxy Activate(string typeName, object[] args)
        {
            var type = this.assembly.GetType(typeName);

            if (type == null)
            {
                throw new Exception("Unknown script type " + typeName);
            }

            var x = Activator.CreateInstance(type, args);
            return new ScriptProxy(x);
        }

        public void Inject(ScriptProxy proxy)
        {
            throw new NotImplementedException();
        }
    }
}
