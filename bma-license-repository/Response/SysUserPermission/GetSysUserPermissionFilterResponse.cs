using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysUserPermission
{
    public class GetSysUserPermissionFilterResponse : ModelMessageAlertResponse
    {
        public List<GetSysUserPermissionFilterResponseData> Data { get; set; }
    }

    public class GetSysUserPermissionFilterResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
    }
}
