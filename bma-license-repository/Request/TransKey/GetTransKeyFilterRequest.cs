using System.ComponentModel.DataAnnotations;
using bma_license_repository.CustomModel.Model;

namespace bma_license_repository.Request.TransKey
{
    public class GetTransKeyFilterRequest : FilterModel, IValidatableObject
    {
        public string? DepartmentCode { get; set; }
        public string? DivisionCode { get; set; }
        public string? SectionCode { get; set; }
        public string? JobCode { get; set; }
        public string? Status { get; set; }
        public string? TypeKeyId { get; set; }
        public string? Permission { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (!string.IsNullOrEmpty(TypeKeyId))
            {
                try
                {
                    var usBy = new Guid(TypeKeyId);
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
