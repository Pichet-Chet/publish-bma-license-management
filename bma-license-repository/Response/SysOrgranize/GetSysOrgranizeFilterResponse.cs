using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysOrgranize
{
    public class GetSysOrgranizeFilterResponse : ModelMessageAlertResponse
    {
        public List<GetSysOrgranizeFilterResponseData> Data { get; set; }

        public int Rows { get; set; }
    }

    public class GetSysOrgranizeFilterResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
        public string TypeName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string Code { get; set; }
        public int Allocate { get; set; } //จัดสรร
        public int InstallPermission { get; set; } //สิทธิ์ติดตั้ง
        public int Install { get; set; } //ติดตั้งเครื่อง
        public int Remain { get; set; } // คงเหลือ
        public decimal InstallPercentage { get; set; } // % ติดตั้ง
        public string PermissionType { get; set; }
    }
}
