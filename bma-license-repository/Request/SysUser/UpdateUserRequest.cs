

using System.ComponentModel.DataAnnotations;
using bma_license_repository.CustomModel.Model;

namespace bma_license_repository.Request.SysUser
{
    public class UpdateUserRequest : IValidatableObject
    {
        [ValidGuid(ErrorMessage = "Invalid Guid format")]
        public string Id { get; set; }

        public bool IsActive {  get; set; }

        [ValidGuid(ErrorMessage = "Invalid Guid format")]

        public string UpdatedBy { get; set; }

        [ValidGuid(ErrorMessage = "Invalid Guid format")]
        public string SysUserGroupId { get; set; }

        public string? SysUserPermissionId { get; set; }

        public string? Password { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        public string? DepartmentCode { get; set; }
        public string? DivisionCode { get; set; }
        public string? SectionCode { get; set; }
        public string? JobCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

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

            return results;
        }
    }
}
