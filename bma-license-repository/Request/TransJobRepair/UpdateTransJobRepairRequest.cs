using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.TransJobRepair
{
    public class UpdateTransJobRepairRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id {  get; set; }
        [Required(ErrorMessage = "TransKeyId is required")]
        public string TransKeyId { get; set; }
        [Required(ErrorMessage = "CategoryId is required")]
        public string CategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string RequestName { get; set; }
        public string Telephone { get; set; }
        public string Remark { get; set; } = string.Empty;

        [Required(ErrorMessage = "UpdateBy is required")]
        public string UpdateBy { get; set; }

        public DateTime? DateOfRequest { get; set; }
        public DateTime? DateOfStart { get; set; }
        public DateTime? DateOfFixed { get; set; }
        public string? FixedDescription {  get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

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

            if (!string.IsNullOrEmpty(UpdateBy))
            {
                try
                {
                    var usBy = new Guid(UpdateBy);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            if (!string.IsNullOrEmpty(TransKeyId))
            {
                try
                {
                    var usBy = new Guid(TransKeyId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

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
