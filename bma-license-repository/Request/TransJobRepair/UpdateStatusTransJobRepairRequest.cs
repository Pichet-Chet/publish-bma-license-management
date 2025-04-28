
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.TransJobRepair
{
    public class UpdateStatusTransJobRepairRequest : IValidatableObject
    {
        public string Id {  get; set; }
        public bool IsCancel { get; set; } = false;

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
            return results;
        }
    }
}
