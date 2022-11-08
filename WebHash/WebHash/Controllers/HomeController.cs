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
        private readonly IHashService _hashService;
        private readonly IAnalyseService _analyseService;
        private readonly IFileService _fileService;
        private readonly ILoggerService _loggerService;

        public HomeController(IHashService hashService, IFileService fileService, ILoggerService loggerService, IAnalyseService analyseService)
        {
            _hashService = hashService;
            _fileService = fileService;
            _loggerService = loggerService;
            _analyseService = analyseService;
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
            try
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

                var model = GetNewMainViewModel();
                model.CrackHash = hash;

                return View("Index", model);
            }
            catch
            {
                TempData["Error"] = true;
                return RedirectToAction("Index");
            }


        }

        [HttpPost]
        public async Task<IActionResult> SendFile(SendFileViewModel file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileService.ImportFile(file.UploadedFile, file.FileName);

                    if (!result)
                    {
                        ModelState.AddModelError("FileError", "There is some error with your file. Please check it before next upload.");
                    }
                }
            }
            catch
            {
                TempData["Error"] = true;
                return RedirectToAction("Index");
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
            var hashes = _fileService.GetHashesFromFile(fileId);
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
            var model = _analyseService.GetAnalyseData();
            return View("../Analyse/Index", model);
        }

        [HttpGet]
        public IActionResult ViewLogs()
        {
            var model = new LogsViewModel();

            model.Logs = _loggerService.GetLogsInformation();
            return View("../Logs/Index", model);
        }

        [HttpGet]
        public IActionResult FileInfo(FileInfoViewModel viewModel)
        {
            if (viewModel.Id == Guid.Empty && viewModel.FilesForSelect == null && viewModel.FileDetails == null)
            {
                var model = new FileInfoViewModel();
                model.FilesForSelect = _analyseService.GetAllFiles();
                return View("../FileInfo/Index", model);
            }
            else
            {
                var fileDetails = _analyseService.GetDetailsForFile(viewModel.Id);
                viewModel.FilesForSelect = _analyseService.GetAllFiles();
                if (fileDetails != null)
                {
                    viewModel.FileDetails = fileDetails;
                    return View("../FileInfo/Index", viewModel);
                }
                ModelState.AddModelError("FileError", "File not found. Try again later.");
                return View("../FileInfo/Index", viewModel);
            }


            
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
