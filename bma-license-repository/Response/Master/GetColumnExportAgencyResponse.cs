

using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.Master
{
    public class GetColumnExportAgencyResponse : ModelMessageAlertResponse
    {
        public List<GetColumnExportAgencyResponseData> Data { get; set; }
    }

    public class GetColumnExportAgencyResponseData
    {
        public string Column { get; set; }
    }
}
