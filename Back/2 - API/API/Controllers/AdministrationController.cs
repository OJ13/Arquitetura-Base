using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : BaseController
    {
        private readonly ILogger _logger;
        public AdministrationController(
            ILogger<AdministrationController> logger
            ) :base(logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("adm")]
        [Authorize]
        public async Task<IActionResult> Teste()
        {
            return null;
        }
    }
}
