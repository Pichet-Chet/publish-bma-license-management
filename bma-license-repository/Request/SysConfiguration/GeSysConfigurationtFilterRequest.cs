using bma_license_repository.CustomModel.Model;
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.SysConfiguration
{
    public class GeSysConfigurationtFilterRequest : FilterModel, IValidatableObject
    {
        public string? Id { get; set; }

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
