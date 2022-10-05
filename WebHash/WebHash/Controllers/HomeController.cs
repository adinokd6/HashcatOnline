using WebHash.IServices;
using WebHash.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebHash.Interfaces;
using System.Threading.Tasks;

namespace WebHash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashService _hashService;
        private readonly IFileService _fileService;

        public HomeController(ILogger<HomeController> logger, IHashService hashService, IFileService fileService)
        {
            _logger = logger;
            _hashService = hashService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            CrackHashViewModel vm = new CrackHashViewModel();
            return View(vm);
        }

        [HttpPost]
        public IActionResult CrackToFile(CrackHashViewModel hash)
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrackHash(CrackHashViewModel hash)
        {
            _hashService.Decode(hash);

            return PartialView("Partials/HashForm",hash);

        }

        [HttpGet]
        public IActionResult SendFile()
        {
            var vm = new SendFileViewModel();
            return View("../Hack/SendFile",vm);
        }

        [HttpPost]
        public async Task<IActionResult> SendFile(SendFileViewModel file)
        {

            if (ModelState.IsValid)
            {
                var result = await _fileService.ImportFile(file.UploadedFile, file.FileName);
                
                if(!result)
                {
                    ModelState.AddModelError("Error: ", " there is some error with your file. Please check it before next upload.");
                }
            }

            return View(file);
        }


    }
}
