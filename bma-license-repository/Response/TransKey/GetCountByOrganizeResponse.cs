
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetCountByOrganizeResponse : ModelMessageAlertResponse
    {
        public GetCountByOrganizeResponseData Data { get; set; }
    }

    public class GetCountByOrganizeResponseData
    {
        public int CountLicense { get; set; }
    }
}
