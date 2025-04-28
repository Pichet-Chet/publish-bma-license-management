
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysUserType
{
    public class GetSysUserTypeFilterResponse : ModelMessageAlertResponse
    {
        public List<GetSysUserTypeFilterResponseData> Data { get; set; }
    }

    public class GetSysUserTypeFilterResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
    }
}
