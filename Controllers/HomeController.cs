using System.Diagnostics;
using IronOcr;
using Microsoft.AspNetCore.Mvc;
using ProjetoOCR.Models;

namespace ProjetoOCR.Controllers
{
    public static class FormFileExtensions
    {
        public static byte[] GetBytes(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            _ = formFile.CopyToAsync(memoryStream);
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
            byte[] file = FormFileExtensions.GetBytes(anexo);
            IronTesseract Ocr = new()
            {
                Language = OcrLanguage.PortugueseBest
            };
            using (OcrInput input = new(file))
            {
                //input.AddPdf("example.pdf", "password");
                OcrResult Result = Ocr.Read(input);
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