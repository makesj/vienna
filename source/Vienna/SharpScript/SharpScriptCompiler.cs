using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Vienna.SharpScript
{
    public class SharpScriptCompiler
    {
        private readonly CSharpCodeProvider _codeProvider;
        private readonly CompilerParameters _parameters;

        public SharpScriptCompiler()
        {
            _codeProvider = new CSharpCodeProvider();
            _parameters = new CompilerParameters();
            _parameters.GenerateInMemory = true;
            _parameters.GenerateExecutable = false;
            _parameters.ReferencedAssemblies.Add("system.dll");
            _parameters.ReferencedAssemblies.Add("system.core.dll");
            _parameters.ReferencedAssemblies.Add("Vienna.dll");

        }

        public Assembly Compile(string script)
        {
            var result = _codeProvider.CompileAssemblyFromSource(_parameters, script);

            if (result.Errors.HasWarnings)
                LogWarnings(result);          

            if (result.Errors.HasErrors)
            {
                LogErrors(result);
                throw new Exception("Script compilation failed. See log for details.");
            }

            return result.CompiledAssembly;
        }

        private void LogWarnings(CompilerResults result)
        {
            foreach (CompilerError error in result.Errors)
            {
                if (!error.IsWarning) continue;
                Logger.Warn("{0} - {1}", error.Line, error.ErrorText);
            }
        }

        private void LogErrors(CompilerResults result)
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
        }
    }
}
