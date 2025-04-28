
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class ImportDataKeyResponse : ModelMessageAlertResponse
    {
        public ImportDataKeyResponseData Data { get; set; }
    }

    public class ImportDataKeyResponseData
    {
        public string ErrorMessage { get; set; }
    }
}
