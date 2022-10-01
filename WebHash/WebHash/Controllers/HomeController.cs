using WebHash.IServices;
using WebHash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebHash.Models.Enums;
using System.Threading.Tasks;
using System;

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
        public async Task<ActionResult> DecodeHash(InputHashModel inputHash)
        {
            Hash hash = new Hash()
            {
                InputValue = inputHash.InputValue,
                AttackMethod = (Enums.AttackMethod)Int32.Parse(inputHash.AttackMethod),
                HashType = (Enums.HashType)Int32.Parse(inputHash.HashType)

            };

            if (hash.InputValue != null)
            {
                _hashService.Decode(hash);
                return Ok(hash);
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
