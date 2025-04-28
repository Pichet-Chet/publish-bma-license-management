using bma_license_repository.CustomModel.Middleware;
using bma_license_repository.Request.TransEquipment;
using bma_license_repository.Request.TransKey;
using bma_license_service;
using bma_license_service.Helper.Constants;
using Microsoft.AspNetCore.Mvc;

namespace bma_license_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransEquipmentController : ControllerBase
    {
        private readonly ITransEquipmentService _service;

        public TransEquipmentController(ITransEquipmentService service)
        {
            _service = service;
        }

        [HttpPost("add-equipment")]
        public async Task<IActionResult> Create([FromBody] CreatedTransEquipmentRequest param)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.Create(param);

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

        [HttpPost("update-equipment")]
        public async Task<IActionResult> Update([FromBody] UpdateEquipmentRequest param)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.Update(param);

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
