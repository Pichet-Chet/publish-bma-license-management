
using System.ComponentModel.DataAnnotations;
using bma_license_repository.CustomModel.Model;

namespace bma_license_repository.Request.TransKey
{
    public class UpdateEquipmentRequest
    {
        [ValidGuid(ErrorMessage = "Invalid Guid Format")]
        public string Id { get; set; }
        [ValidGuid(ErrorMessage = "Invalid Guid Format")]
        public string TransKeyId { get; set; }
        [Required(ErrorMessage = "EquipmentCode is required")]
        public string? EquipmentCode { get; set; }
        public string? MachineType { get; set; }
        public string? MachineNumber { get; set; }
        public string? MachineName { get; set; }
        public string? MacAddress { get; set; }
        public string? Brand { get; set; }
        public string? Generation { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? InstallDate { get; set; }
        public string? InstallLocation { get; set; }
        public string? Remark { get; set; }
        [ValidGuid(ErrorMessage = "Invalid Guid Format")]
        public string UpdateBy { get; set; }

    }
}
