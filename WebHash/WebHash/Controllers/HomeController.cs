using WebHash.IServices;
using WebHash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebHash.Models.Enums;
using System.Threading.Tasks;

namespace WebHash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashService _hashService;

        public HomeController(ILogger<HomeController> logger, IHashService hashService)
        {
            _logger = logger;
            _hashService = hashService;
        }

        [HttpPost]
        [Route("Decode")]
        public async Task<ActionResult> DecodeHash(InputHash inputHash)
        {
            Hash hash = new Hash();

            if (hash.InputValue != null)
            {
                _hashService.Decode(hash);
                //return Json(new { decoded = hash.OutputValue.Item1, errorMessage = hash.OutputValue.Item2 });
                return Ok();
            }
            return Ok("dupa");

        }

        [HttpGet]
        public async Task<ActionResult> Test()
        {
            return Ok("elo");
        }


    }
}
