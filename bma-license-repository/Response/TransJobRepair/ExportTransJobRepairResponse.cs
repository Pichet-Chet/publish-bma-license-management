
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransJobRepair
{
    public class ExportTransJobRepairResponse : ModelMessageAlertResponse
    {
        public MemoryStream Data { get; set; }
    }
}
