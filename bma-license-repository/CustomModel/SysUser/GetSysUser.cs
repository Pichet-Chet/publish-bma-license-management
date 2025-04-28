
namespace bma_license_repository.CustomModel.SysUser
{
    public class GetSysUser
    {
        public Guid Id { get; set; }
        public Guid SysUserTypeId { get; set; }
        public string SysUserTypeName { get; set; }
        public Guid SysUserGroupId { get; set; }
        public string SysUserGroupName { get; set; }
        public Guid? SysUserPermissionId { get; set; }
        public string SysUserPermissionName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenExpire { get; set; }
        public bool IsActive { get; set; }
        public string DepartmentCode { get; set; }
        public string DivisionCode { get; set; }
        public string SectionCode { get; set; }
        public string JobCode { get; set; }
    }
}
