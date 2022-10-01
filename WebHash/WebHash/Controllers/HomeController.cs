using WebHash.IServices;
using WebHash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace WebHash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashService _hashService;
        private readonly ICsvService _csvService;

        public HomeController(ILogger<HomeController> logger, IHashService hashService, ICsvService csvService)
        {
            _logger = logger;
            _hashService = hashService;
            _csvService = csvService;
        }

        public IActionResult Index(Hash hash)
        {
            _hashService.Decode(hash);
            return View(hash);
        }

        [HttpGet]
        public IEnumerable<string> ReadCsv(string fileName)
        {
            return _csvService.ImportCsvFile(fileName);
        }


    }
}
