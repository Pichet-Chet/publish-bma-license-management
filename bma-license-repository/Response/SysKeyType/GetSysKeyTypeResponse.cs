
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysKeyType
{
    public class GetSysKeyTypeResponse : ModelMessageAlertResponse
    {
        public List<GetSysKeyTypeResponseData> Data { get; set; }
    }

    public class GetSysKeyTypeResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
