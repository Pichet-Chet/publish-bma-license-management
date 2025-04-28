
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetCountTop10Response : ModelMessageAlertResponse
    {
        public List<GetCountTop10ResponseData> Data { get; set; }
    }

    public class GetCountTop10ResponseData
    {
        public string OrgranizeName { get; set; }
        public int CountOrgnize { get; set; }
    }
}
