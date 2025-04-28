
using System.ComponentModel.DataAnnotations;
using bma_license_repository.CustomModel.Model;

namespace bma_license_repository.Request.TransKey
{
    public class MoveToNewRequest
    {
        [ValidGuid(ErrorMessage = "Invalid Guid Format")]
        public string TransKeyId { get; set; }
        [Required(ErrorMessage = "DepartmentCode is required")]
        public string DepartmentCode { get; set; }
        [Required(ErrorMessage = "DivisionCode is required")]
        public string DivisionCode { get; set; }
        [Required(ErrorMessage = "SectionCode is required")]
        public string SectionCode { get; set; }
        [Required(ErrorMessage = "JobCode is required")]
        public string JobCode { get; set; }
        [ValidGuid(ErrorMessage ="Invalid Guid Format")]
        public string ActionBy {  get; set; }
    }
}
