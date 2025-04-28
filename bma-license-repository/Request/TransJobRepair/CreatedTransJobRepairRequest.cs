
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace bma_license_repository.Request.TransJobRepair
{
    public class CreatedTransJobRepairRequest : IValidatableObject
    {
        [Required(ErrorMessage = "TransKeyId is required")]
        public string TransKeyId {  get; set; }
        [Required(ErrorMessage = "CategoryId is required")]
        public string CategoryId { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string? Remark { get; set; }
        public string? EquipmentId {  get; set; }

        [Required(ErrorMessage = "CreatedBy is required")]
        public string CreatedBy {  get; set; }

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

            if (!string.IsNullOrEmpty(EquipmentId))
            {
                try
                {
                    var usBy = new Guid(EquipmentId);
                }
                catch
                {
                    results.Add(new ValidationResult("Invalid Guid format"));
                }
            }

            if (!string.IsNullOrEmpty(CreatedBy))
            {
                try
                {
                    var usBy = new Guid(CreatedBy);
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
            return results;
        }


    }
}
