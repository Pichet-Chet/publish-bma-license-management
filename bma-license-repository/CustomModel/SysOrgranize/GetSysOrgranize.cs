using bma_license_repository.CustomModel.TransKey;
namespace bma_license_repository.CustomModel.SysOrgranize
{
    public class GetSysOrgranize
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string DepartmentCode { get; set; } = null!;

        public string DivisionCode { get; set; } = null!;

        public string SectionCode { get; set; } = null!;

        public string JobCode { get; set; } = null!;

        public bool? IsActive { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string TypeName { get; set; } = null!;

        public string? Code { get; set; }

        public int Seq { get; set; }
        public List<GetTransKey> GetTransKey { get; set; }
    }
}

