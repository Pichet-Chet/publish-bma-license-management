using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysOrgranize
{
    public class GetSysOrgranizeDivisionResponse : ModelMessageAlertResponse
    {
        public List<GetSysOrgranizeDivisionResponseData> Data { get; set; }
    }
    public class GetSysOrgranizeDivisionResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
