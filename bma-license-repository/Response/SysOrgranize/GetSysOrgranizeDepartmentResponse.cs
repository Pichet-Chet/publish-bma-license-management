using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysOrgranize
{
    public class GetSysOrgranizeDepartmentResponse : ModelMessageAlertResponse
    {
        public List<GetSysOrgranizeDepartmentResponseData> Data { get; set; }
    }
    public class GetSysOrgranizeDepartmentResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code {  get; set; }
    }
}
