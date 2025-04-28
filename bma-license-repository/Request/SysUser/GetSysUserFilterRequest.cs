using bma_license_repository.CustomModel.Model;
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.SysUser
{
    public class GetSysUserFilterRequest : FilterModel, IValidatableObject
    {
        public string? Id { get; set; }
        public string? SysUserTypeId { get; set; }
        public string? SysUserGroupId { get; set; }
        public string? SysUserPermissionId { get; set; }
        public string? SysOrgranizeId { get; set; }
        public bool? IsActive { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(Id))
            {
                try
                {
                    var usBy = new Guid(Id);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            if (!string.IsNullOrEmpty(SysUserTypeId))
            {
                try
                {
                    var usBy = new Guid(SysUserTypeId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            if (!string.IsNullOrEmpty(SysUserGroupId))
            {
                try
                {
                    var usBy = new Guid(SysUserGroupId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            if (!string.IsNullOrEmpty(SysUserPermissionId))
            {
                try
                {
                    var usBy = new Guid(SysUserPermissionId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            if (!string.IsNullOrEmpty(SysOrgranizeId))
            {
                try
                {
                    var usBy = new Guid(SysOrgranizeId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            return results;
        }
    }
}
