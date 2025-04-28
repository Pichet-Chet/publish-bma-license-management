using bma_license_repository.CustomModel.Middleware;
using bma_license_repository.Request.TransKey;
using bma_license_repository.Request.TransKeyHistory;
using bma_license_service;
using bma_license_service.Helper.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bma_license_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransKeyHistoryController : ControllerBase
    {
        private readonly ITransKeyHistoryService _service;

        public TransKeyHistoryController(ITransKeyHistoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetFilter([FromQuery] GetTransKeyHistoryFilterRequest param)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.GetFilter(param);

                if (Outbound.Data != null)
                {
                    resp.Status = ConstantsResponse.StatusSuccess;
                    resp.Code = ConstantsResponse.HttpCode200;
                    resp.Message = ConstantsResponse.HttpCode200Message;
                    resp.PageSize = param.PageSize;
                    resp.PageNumber = param.PageNumber;
                    resp.RowCount = await _service.CountAllAsync();
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatedTransKeyHistoryRequest param)
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
    }
}
