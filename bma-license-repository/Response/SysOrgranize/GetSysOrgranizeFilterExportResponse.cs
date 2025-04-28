
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysOrgranize
{
    public class GetSysOrgranizeFilterExportResponse : ModelMessageAlertResponse
    {
        public MemoryStream Data { get; set; }
    }
}
