using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PythonCLI
{
    static class ScriptLoader
    {
        public static IList<Task<string>> LoadScripts(string[] args)
        {
            var scriptReadTasks = new List<Task<string>>(4);
            for (int i = 0; i < 4; ++i)
            {
                scriptReadTasks.Add(File.ReadAllTextAsync(args[i]));
            }
            return scriptReadTasks;
        }

        public static IList<CompiledCode> CreateScripts(IList<Task<string>> tasks, ScriptEngine engine)
        {
            var strings = GetScriptsFromTasks(tasks);
            return CompileScripts(strings, engine);
        }

        private static IList<string> GetScriptsFromTasks(IList<Task<string>> tasks)
        {
            var scripts = new List<string>(tasks.Count);
            for (int i = 0; i < tasks.Count; i++)
            {
                scripts.Add(tasks[i].Result);
            }
            return scripts;
        }

        private static IList<CompiledCode> CompileScripts(IList<string> scripts, ScriptEngine engine)
        {
            var compiledScripts = new List<CompiledCode>(scripts.Count);
            for (int i = 0; i < scripts.Count; i++)
            {
                var source = engine.CreateScriptSourceFromString(scripts[i]);
                compiledScripts.Add(source.Compile());
            }
            return compiledScripts;
        }
    }
}
