using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Vienna.SharpScript
{
    public class SharpScriptContext
    {
        public string Name { get; protected set; }
        protected CSharpCodeProvider CodeProvider { get; set; }
        protected CompilerParameters CompilerParameters { get; set; }
        protected AppDomain Context { get; set; }
        
        public SharpScriptContext(string name)
        {
            Name = name;
            CodeProvider = new CSharpCodeProvider();
            CompilerParameters = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };
            CompilerParameters.ReferencedAssemblies.Add("system.dll");
            Context = AppDomain.CreateDomain(name);
        }

        public void Unload()
        {
            AppDomain.Unload(Context);
        }

        public void AddReference(Assembly assembly)
        {
            CompilerParameters.ReferencedAssemblies.Add(assembly.Location);
        }

        public void AddReference(Assembly[] assembly)
        {
            foreach (var a in assembly)
            {
                AddReference(a);
            }
        }

        public void Compile(string script)
        {
            var result = CodeProvider.CompileAssemblyFromSource(CompilerParameters, script);

            if(result.Errors.HasWarnings)
            {
                foreach (CompilerError error in result.Errors)
                {
                    if (!error.IsWarning) continue;
                    Logger.Warn("{0} - {1}", error.Line, error.ErrorText);
                }
            }

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

            //Context.Load();
        }

        public void GetInstance(string type, object[] args)
        {
            //Context.CreateInstance();
        }
    }
}