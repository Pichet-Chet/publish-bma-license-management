using bma_license_repository.CustomModel.Middleware;
using bma_license_repository.Request.SysConfiguration;
using bma_license_service;
using bma_license_service.Helper.Constants;
using Microsoft.AspNetCore.Mvc;

namespace bma_license_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysConfigurationController : ControllerBase
    {
        private readonly ISysConfigurationService _service;

        public SysConfigurationController(ISysConfigurationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] GeSysConfigurationtFilterRequest request)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.GetFilter(request);

                if (Outbound.Data != null)
                {
                    resp.Status = ConstantsResponse.StatusSuccess;
                    resp.Code = ConstantsResponse.HttpCode200;
                    resp.Message = ConstantsResponse.HttpCode200Message;

                    resp.Output = Outbound;
                }
                else
                {
                    resp.Status = ConstantsResponse.StatusError;
                    resp.Code = ConstantsResponse.HttpCode200;
                    resp.Message = ConstantsResponse.HttpCode204Message;
                    resp.Output = Outbound;
                }
            }
            catch (Exception ex)
            {
                resp.Status = ConstantsResponse.StatusError;
                resp.Code = ConstantsResponse.HttpCode500;
                resp.Message = ex.InnerException == null ? ex.Message : ex.InnerException.ToString();
            }

            return StatusCode(resp.Code, ApiConfiguration.GetResponseController(resp));
        }
    }
}
