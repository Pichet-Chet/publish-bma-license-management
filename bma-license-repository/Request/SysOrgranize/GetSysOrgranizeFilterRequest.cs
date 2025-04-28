using bma_license_repository.CustomModel.Model;
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.SysOrgranize
{
    public class GetSysOrgranizeFilterRequest : FilterModel, IValidatableObject
    {
        public string? Id { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DivisionCode { get; set; }
        public string? SectionCode { get; set; }
        public string? JobCode { get; set; }
        public string? Code { get; set; }
        public string? TypeName { get; set; }
        public bool? IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (Id != null)
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

            return results;
        }
    }
}
