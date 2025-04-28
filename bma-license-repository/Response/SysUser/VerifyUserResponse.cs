
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysUser
{
    public class VerifyUserResponse : ModelMessageAlertResponse
    {
        public VerifyUserResponseData Data { get; set; }
    }

    public class VerifyUserResponseData
    {
        public Guid Id { get; set; }
        public string SysUserTypeName { get; set; }
        public string SysUserGroupName { get; set; }
        public string SysUserPermissionName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
        public bool IsActive { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
    }
}
