
using System.ComponentModel.DataAnnotations;
using bma_license_repository.CustomModel.Model;

namespace bma_license_repository.Request.TransKey
{
    public class GetTransKeyFilterExportRequest : FilterModel, IValidatableObject
    {
        public string? Id { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DivisionCode { get; set; }
        public string? SectionCode { get; set; }
        public string? JobCode { get; set; }
        public bool IsAllColumn { get; set; } = true;
        public List<int>? ColumnIndex { get; set; }

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
