using Desktop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Desktop.Controllers
{
    /// <summary>
    /// Controller responsible for adding AIs to the game
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly FileDialogService fileDialogService;
        private readonly IPythonExecutableService pythonExecutableService;

        public AIController(FileDialogService fileDialogService,
            IPythonExecutableService pythonExecutableService)
        {
            this.fileDialogService = fileDialogService;
            this.pythonExecutableService = pythonExecutableService;
        }

        [HttpGet("scripts")]
        public IActionResult GetExistingScripts()
        {
            string scriptFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");

            // Script files
            IEnumerable<string> aiFiles = Directory.GetFiles(scriptFolderPath, "*Intelligence.py")
                .Select(Path.GetFileNameWithoutExtension);

            // Human player
            aiFiles = Enumerable.Repeat("Human Player", 1).Concat(aiFiles);

            // Directories
            IEnumerable<string> aiDirs = Directory.GetDirectories(scriptFolderPath, "*Intelligence")
                .Where(aiDir => System.IO.File.Exists(Path.Combine(aiDir, "main.py")))
                .Select(Path.GetFileName);

            return Ok(aiFiles.Concat(aiDirs));
        }

        /// <summary>
        /// Add an AI script through a file select dialog.
        /// </summary>
        [HttpPost("addscript")]
        public async Task<IActionResult> AddSingleScript()
        {
            return Ok(await fileDialogService.AddSingleScript());
        }

        /// <summary>
        /// Add an AI folder through a file select dialog.
        /// </summary>
        [HttpPost("addfolder")]
        public async Task<IActionResult> AddFolderScript()
        {
            return Ok(await fileDialogService.AddFolderScript());
        }

        /// <summary>
        /// Returns the currently configured path to the Python executable.
        /// </summary>
        [HttpGet("pythonexecutable")]
        public IActionResult GetPythonExecutable()
        {
            return Ok(new { Path = pythonExecutableService.GetPath() });
        }

        /// <summary>
        /// Opens a dialog where the user can specify a Python executable to use.
        /// If they cancel the dialog, the previous path remains.
        /// </summary>
        /// <returns>Path to the executable</returns>
        [HttpPost("pythonexecutable")]
        public async Task<IActionResult> SetPythonExecutable()
        {
            string previousExecutable = pythonExecutableService.GetPath();
            string selectedExecutable = await fileDialogService.GetPythonExecutableSelection();
            if (selectedExecutable != null)
            {
                pythonExecutableService.SetPath(selectedExecutable);
                return Ok(new { Path = selectedExecutable });
            }
            return Ok(new { Path = previousExecutable });
        }
    }
}
