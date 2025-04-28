using bma_license_repository.CustomModel.Middleware;
using bma_license_repository.Request.SysOrgranize;
using bma_license_service;
using bma_license_service.Helper.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bma_license_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysOrgranizeController : ControllerBase
    {
        private readonly ISysOrgranizeService _service;

        public SysOrgranizeController(ISysOrgranizeService serive)
        {
            _service = serive;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilter([FromQuery] GetSysOrgranizeFilterRequest param)
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

        [HttpPost("export-excel")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilterExport([FromBody] GetSysOrgranizeFilterExportRequest param)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var result = await _service.GetFilterExport(param);
                if (result.Data == null)
                {
                    resp.Status = ConstantsResponse.StatusError;
                    resp.Code = ConstantsResponse.HttpCode200;
                    resp.Message = ConstantsResponse.HttpCode200Message;
                    resp.Output = result;
                }
                else
                {
                    var setFilename = $"Orgranize :{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";

                    return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{setFilename}.xlsx");
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

        [HttpGet("ddl-department")]
        public async Task<IActionResult> GetDepartment()
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.GetDepaerment();

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

        [HttpGet("ddl-division/{departmentCode}")]
        public async Task<IActionResult> GetDivision(string departmentCode)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.GetDivision(departmentCode);

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

        [HttpGet("ddl-section/{departmentCode}/{divisionCode}")]
        public async Task<IActionResult> GetSection(string departmentCode, string divisionCode)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.GetSection(departmentCode, divisionCode);

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

        [HttpGet("ddl-job/{departmentCode}/{divisionCode}/{sectionCode}")]
        public async Task<IActionResult> GetJob(string departmentCode, string divisionCode, string sectionCode)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var Outbound = await _service.GetJob(departmentCode, divisionCode, sectionCode);

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
