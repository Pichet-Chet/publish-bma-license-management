using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysOrgranize
{
    public class GetSysOrgranizeSectionResponse : ModelMessageAlertResponse
    {
        public List<GetSysOrgranizeSectionResponseData> Data {  get; set; }
    }

    public class GetSysOrgranizeSectionResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
