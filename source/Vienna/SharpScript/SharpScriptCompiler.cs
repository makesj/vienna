using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Vienna.SharpScript
{
    public class SharpScriptCompiler
    {
        private readonly CSharpCodeProvider _codeProvider;
        private readonly CompilerParameters _parameters;
        private readonly StringBuilder _imports;

        public SharpScriptCompiler()
        {
            _codeProvider = new CSharpCodeProvider();

            _parameters = new CompilerParameters();
            _parameters.TempFiles.KeepFiles = false;
            _parameters.GenerateInMemory = true;
            _parameters.GenerateExecutable = false;
            _parameters.ReferencedAssemblies.Add("system.dll");
            _parameters.ReferencedAssemblies.Add("system.core.dll");
            _parameters.ReferencedAssemblies.Add("Vienna.dll");
            _parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");

            _imports = new StringBuilder();
            _imports.Append("using System;");  
            _imports.Append("using System.Text;"); 
            _imports.Append("using System.Collections.Generic;"); 
            _imports.Append("using System.Linq;");
            _imports.Append("using Vienna.SharpScript;"); 
        }

        public Assembly Compile(string[] scripts)
        {
            //add default using statements
            var processedScripts = scripts.Select(script => _imports + script).ToArray();

            var result = _codeProvider.CompileAssemblyFromSource(_parameters, processedScripts);

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
                sb.AppendLine(error.FileName)
                    .Append(error.Line)
                    .Append(" - ")
                    .Append(error.ErrorText)
                    .AppendLine(error.FileName);
            }
            Logger.Error(sb.ToString());
        }
    }
}
