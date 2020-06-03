using System.IO;
using System.Reflection;

namespace Desktop.Services
{
    /// <summary>
    /// Service responsible for remembering the selected Python executable path
    /// between sessions. It writes the selected path into a file in the game's
    /// installation directory.
    /// </summary>
    public sealed class PythonExecutableService
    {
        private const string PythonPathFileName = "PythonExecutablePath.txt";
        private readonly string PythonPathFullPath;
        private readonly object fileLock = new object();

        public PythonExecutableService()
        {
            string gameFilesFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PythonPathFullPath = Path.Combine(gameFilesFolder, PythonPathFileName);
        }

        /// <summary>
        /// Return the path currently configured for the Python executable, or null otherwise.
        /// </summary>
        public string GetPath()
        {
            lock (fileLock)
            {
                if (!File.Exists(PythonPathFullPath))
                {
                    return null;
                }
                return File.ReadAllText(PythonPathFullPath);
            }
        }

        /// <summary>
        /// Sets the path for the Python executable.
        /// </summary>
        public void SetPath(string path)
        {
            lock (fileLock)
            {
                File.WriteAllText(PythonPathFullPath, path);
            }
        }
    }
}
