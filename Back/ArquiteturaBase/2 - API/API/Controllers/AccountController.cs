using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILogger _logger;
        public AccountController(
            ILogger<AccountController> logger
            ):base(logger)
        {
            _logger = logger;
        }
    }
}