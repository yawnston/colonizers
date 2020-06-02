using Desktop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AIController> logger;
        private readonly FileDialogService fileDialogService;

        public AIController(ILogger<AIController> logger,
            FileDialogService fileDialogService)
        {
            this.logger = logger;
            this.fileDialogService = fileDialogService;
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

        [HttpPost("addscript")]
        public async Task<IActionResult> AddSingleScript()
        {
            return Ok(await fileDialogService.AddSingleScript());
        }

        [HttpPost("addfolder")]
        public async Task<IActionResult> AddFolderScript()
        {
            return Ok(await fileDialogService.AddFolderScript());
        }

        [HttpPost("pythonexecutable")]
        public async Task<IActionResult> SetPythonExecutable()
        {
            // TODO: remember python executable between sessions?
            return Ok(new { Path = await fileDialogService.GetPythonExecutable() });
        }
    }
}
