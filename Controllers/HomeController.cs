using InkBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InkBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public record CompileRequest(string Code);
        public IActionResult Compile([FromBody] CompileRequest req)
        {
            var status = new List<string>();
            var inkle = new Ink.Compiler(req.Code, new Ink.Compiler.Options()
            {
                errorHandler = (message, type) =>
                 {
                     status.Add(Enum.GetName<Ink.ErrorType>(type) + ":" + message);
                 }
            });
            var story = inkle.Compile();
            return Json(new { Status = String.Join("\n",status.ToArray()),Story= story.ToJson()});
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
