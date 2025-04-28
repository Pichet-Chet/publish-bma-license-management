using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetTransKeyFilterExportResponse : ModelMessageAlertResponse
    {
        public MemoryStream Data { get; set; }
    }
}
