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

        public object Activate(string typeName, object[] args)
        {
            //var type = this.assembly.GetType(typeName);
            //dynamic instance = Activator.CreateInstance(type, args);
            var x = AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap(assembly.FullName, typeName);
            return x;
        }

    }
}
