using WebHash.IServices;
using WebHash.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebHash.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebHash.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashService _hashService;
        private readonly IFileService _fileService;
        private readonly ILoggerService _loggerService;

        public HomeController(ILogger<HomeController> logger, IHashService hashService, IFileService fileService, ILoggerService loggerService)
        {
            _logger = logger;
            _hashService = hashService;
            _fileService = fileService;
            _loggerService = loggerService;
        }

        public IActionResult Index()
        {
            var files = _fileService.GetFiles();
            var vm = GetNewMainViewModel();
            return View(vm);
        }


        [HttpPost]
        public IActionResult CrackHash(CrackHashViewModel hash)
        {
            if (hash != null && ModelState.IsValid)
            {
                _hashService.Decode(hash);
                hash.Files = _fileService.GetFiles();

            }
            else
            {
                if (hash == null)
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

            return View("Index", model);

        }

        [HttpPost]
        public async Task<IActionResult> SendFile(SendFileViewModel file)
        {

            if (ModelState.IsValid)
            {
                var result = await _fileService.ImportFile(file.UploadedFile, file.FileName);

                if (!result)
                {
                    ModelState.AddModelError("FileError", "There is some error with your file. Please check it before next upload.");
                }
            }

            var vm = GetNewMainViewModel();

            vm.SendFile = file;

            return View("Index", vm);
        }

        [HttpPost]
        public IActionResult DeleteFile(DeleteFileViewModel vm)
        {
            if (vm.Id != Guid.Empty)
            {
                if (_fileService.IsFileExists(vm.Id))
                {
                    _fileService.DeleteFile(vm.Id);
                    return Json(new { isDelete = true, message = "File successfully deleted." });
                }
                else
                {
                    ModelState.AddModelError("FileDoesntExist", "File you have choosen doesnt exists.");
                    Json(new { isDeleted = false });
                }

            }
            ModelState.AddModelError("FileDoesntExist", "Error occured. Try again later.");
            return Json(new { message = "Error occured. Try again later", isDelete = false });
        }

        [HttpGet]
        public JsonResult GetHashes(Guid fileId)
        {
            var hashes = _fileService.GetHashesFromFile(fileId); //TODO: Tylko import plikow ktore nie sa empty. Wewnatrz serwisu zrobic
            if (hashes == null)
            {
                ModelState.AddModelError("EmptyFileList", "File list is empty. Upload some files to database.");
                return Json(new { isEmpty = true });
            }
            return Json(new { hashes = new SelectList(hashes, "Id", "Hash"), isEmpty = false });
        }

        [HttpGet]
        public IActionResult Analyse()
        {
            var model = _hashService.GetAnalyseData();
            return View("../Analyse/Index", model);
        }

        [HttpGet]
        public IActionResult ViewLogs()
        {
            var model = new LogsViewModel();

            model.Logs = _loggerService.GetLogsInformation();
            return View("../Logs/Index", model);
        }

        private MainWindowViewModel GetNewMainViewModel()
        {
            var files = _fileService.GetFiles();
            var allFiles = _fileService.GetAllFiles();
            MainWindowViewModel vm = new MainWindowViewModel()
            {
                CrackHash = new CrackHashViewModel()
                {
                    Files = files
                },
                SendFile = new SendFileViewModel(),
                DeleteFile = new DeleteFileViewModel()
                {
                    Files = allFiles
                },
            };

            return vm;
        }


    }
}
