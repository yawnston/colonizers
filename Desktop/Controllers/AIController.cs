using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public AIController(ILogger<AIController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("scripts")]
        public IActionResult GetExistingScripts()
        {
            string scriptFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");
            var aiFiles = Directory.GetFiles(scriptFolderPath, "*Intelligence.py")
                .Select(Path.GetFileNameWithoutExtension);
            aiFiles = Enumerable.Repeat("Human Player", 1).Concat(aiFiles);

            return Ok(aiFiles);
        }
    }
}
