using bma_license_service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bma_license_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _service;

        public TokenController(ITokenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            return Ok(await _service.GetToken());
        }

    }
}
