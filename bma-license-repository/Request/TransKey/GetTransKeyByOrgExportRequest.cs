using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.TransKey
{
    public class GetTransKeyByOrgExportRequest
    {
        public string? DepartmentCode { get; set; } = null!;

        public string? DevisionCode { get; set; }

        public string? SectionCode { get; set; }

        public string? JobCode { get; set; }
        public bool IsAllColumn { get; set; } = true;
        public List<string>? ColumnName { get; set; }
    }
}
