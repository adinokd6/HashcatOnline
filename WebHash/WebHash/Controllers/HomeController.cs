using WebHash.IServices;
using WebHash.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebHash.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using WebHash.Models.ViewModels.Analyse;

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
            var files = _fileService.GetFiles();
            MainWindowViewModel vm = new MainWindowViewModel()
            {
                CrackHash = new CrackHashViewModel()
                {
                    Files = files
                },
                SendFile = new SendFileViewModel()
            };
            return View(vm);
        }


        [HttpPost]
        public IActionResult CrackHash(CrackHashViewModel hash)
        {
            if(hash != null && ModelState.IsValid)
            {
                _hashService.Decode(hash);
                hash.Files = _fileService.GetFiles();

            }
            else
            {
                if(hash == null)
                {
                    hash = new CrackHashViewModel();
                    
                }
                hash.Files = _fileService.GetFiles();


            }

            var model = new MainWindowViewModel()
            {
                CrackHash = hash,
                SendFile = new SendFileViewModel()
            };

            return View("Index",model);

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

            return PartialView("../Home/Partials/_SendFile", file);
        }

        [HttpGet]
        public JsonResult GetHashes(Guid fileId)
        {
            var hashes = _fileService.GetHashesFromFile(fileId);
            return Json(new SelectList(hashes, "Id", "Hash"));
        }

        [HttpGet]
        public IActionResult Analyse()
        {
            var model = _hashService.GetAnalyseData();
            return View("../Analyse/Index", model);
        }


    }
}
