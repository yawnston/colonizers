using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Desktop.Services
{
    public class FileDialogService
    {
        private readonly ILogger<FileDialogService> logger;

        public FileDialogService(ILogger<FileDialogService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Open a file dialog and let the user copy a single script file
        /// into the game AI folder.
        /// </summary>
        /// <returns>true if a script was copied, false otherwise</returns>
        public async Task<bool> AddSingleScript()
        {
            if (HybridSupport.IsElectronActive)
            {
                string file = await OpenFileDialog(OpenDialogProperty.openFile);

                if (file != null)
                {
                    string scriptFolderPath = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");
                    string newFilename = Path.Combine(scriptFolderPath, Path.GetFileName(file));
                    File.Copy(file, newFilename, true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Open a file dialog and let the user copy a folder-based AI script
        /// into the game AI folder.
        /// </summary>
        /// <returns>true if the folder was copied, false otherwise</returns>
        public async Task<bool> AddFolderScript()
        {
            if (HybridSupport.IsElectronActive)
            {
                string fromDirectory = await OpenFileDialog(OpenDialogProperty.openDirectory);

                if (fromDirectory != null)
                {
                    string scriptFolderPath = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");
                    string toDirectory = Path.Combine(scriptFolderPath, Path.GetFileName(fromDirectory));
                    CopyDirectory(fromDirectory, toDirectory);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Opens a file dialog to let the user pick a Python executable.
        /// </summary>
        /// <returns>Path to the selected Python executable, or null if it was not selected.</returns>
        public async Task<string> GetPythonExecutable()
        {
            if (HybridSupport.IsElectronActive)
            {
                return await OpenFileDialog(OpenDialogProperty.openFile);
            }

            return null;
        }

        private async Task<string> OpenFileDialog(OpenDialogProperty openDialogProperty)
        {
            BrowserWindow mainWindow = Electron.WindowManager.BrowserWindows.First();
            OpenDialogOptions options = new OpenDialogOptions
            {
                Properties = new OpenDialogProperty[] {
                        openDialogProperty
                    }
            };

            string[] files = await Electron.Dialog.ShowOpenDialogAsync(mainWindow, options);

            if (files.Length > 0)
            {
                return files[0];
            }
            return null;
        }

        /// <summary>
        /// Recursively copy a directory (with overwriting of existing files).
        /// </summary>
        private void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo target = new DirectoryInfo(targetDirectory);
            CopyDirectoryInternal(source, target);

            void CopyDirectoryInternal(DirectoryInfo source, DirectoryInfo target)
            {
                Directory.CreateDirectory(target.FullName);

                // Copy each file into the new directory
                foreach (FileInfo fileInfo in source.GetFiles())
                {
                    logger.LogInformation("Copying {0}\\{1}", target.FullName, fileInfo.Name);
                    fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
                }

                // Copy each subdirectory using recursion
                foreach (DirectoryInfo subdirectoryInfo in source.GetDirectories())
                {
                    DirectoryInfo newSubdirectoryInfo = target.CreateSubdirectory(subdirectoryInfo.Name);
                    CopyDirectoryInternal(subdirectoryInfo, newSubdirectoryInfo);
                }
            }
        }
    }
}
