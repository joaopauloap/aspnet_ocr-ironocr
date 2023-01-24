using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using ProjetoOCR.Models;
using System.Diagnostics;
using System;
using static System.Net.Mime.MediaTypeNames;
using IronOcr;

namespace ProjetoOCR.Controllers
{
    public static class FormFileExtensions
    {
        public static byte[] GetBytes(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

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

        [HttpPost]
        public IActionResult CarregarAnexo(IFormFile anexo)
        {
            var file = FormFileExtensions.GetBytes(anexo);
            var Ocr = new IronTesseract();
            Ocr.Language = OcrLanguage.PortugueseBest;
            using (var input = new OcrInput(file))
            {
                //input.AddPdf("example.pdf", "password");
                var Result = Ocr.Read(input);
                Console.WriteLine(Result.Text);
                Console.WriteLine($"{Result.Pages.Count()} Pages");
                TempData["text"] = Result.Text;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}