

using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysOrgranize
{
    public class GetSysOrgranizeJobResponse : ModelMessageAlertResponse
    {
        public List<GetSysOrgranizeJobResponseData> Data { get; set; }
    }

    public class GetSysOrgranizeJobResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
