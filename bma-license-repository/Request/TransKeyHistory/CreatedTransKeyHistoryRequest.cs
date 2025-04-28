
using bma_license_repository.CustomModel.Model;
using bma_license_repository.Dto;
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.TransKeyHistory
{
    public class CreatedTransKeyHistoryRequest : IValidatableObject
    {
        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [ValidGuid(ErrorMessage = "Invalid Guid Format")]
        public string ActionBy { get; set; }
        [Required(ErrorMessage = "SysOrgranizeId is required")]
        public string? SysOrgranizeId { get; set; }
        public string? MachineType { get; set; }
        public string? MachineNumber { get; set; }
        public string? MacAddress { get; set; }
        public string? MachineName { get; set; }
        public string? Brand { get; set; }
        public string? Generation { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? InstallDate { get; set; }
        public string? InstallLocation { get; set; }
        public string? Remark { get; set; }
        [Required(ErrorMessage = "TransKeyId is required")]
        public string TransKeyId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

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
