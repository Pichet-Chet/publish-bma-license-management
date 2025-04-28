using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysUserGroup
{
    public class GetSysUserGroupFilterResponse : ModelMessageAlertResponse
    {
        public List<GetUserGroupFilterResponseData> Data { get; set; }
    }

    public class GetUserGroupFilterResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
    }
}
