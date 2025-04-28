
using bma_license_repository.CustomModel.Model;
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.TransJobRepair
{
    public class GetTransJobRepairFilterRequest : FilterModel, IValidatableObject
    {
        public string? Id { get; set; }
        public DateTime? DateOfRequest { get; set; }
        public DateTime? DateOfFixed { get; set; }
        public string? CategoryId { get; set; }
        public string? StatusId { get; set; }

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


            if (!string.IsNullOrEmpty(StatusId))
            {
                try
                {
                    var usBy = new Guid(StatusId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }


            if (!string.IsNullOrEmpty(CategoryId))
            {
                try
                {
                    var usBy = new Guid(CategoryId);
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
