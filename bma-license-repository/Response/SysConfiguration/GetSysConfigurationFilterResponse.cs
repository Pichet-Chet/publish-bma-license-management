using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysConfiguration
{
    public class GetSysConfigurationFilterResponse : ModelMessageAlertResponse
    {
        public List<GetFilterResponseData> Data { get; set; }

    }

    public class GetFilterResponseData
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
