
namespace bma_license_repository.CustomModel.TransKeyHistory
{
    public class GetTransKeyHistory
    {
        public Guid Id { get; set; }
        public Guid SysTransKeyTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ActionBy { get; set; }
        public Guid? SysOrgranizeId { get; set; }
        public string MachineType { get; set; }
        public string MachineName { get; set; }
        public string MachineNumber { get; set; }
        public string MacAddress { get; set; }
        public string Brand { get; set; }
        public string Generation { get; set; }
        public string IpAddress { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InstallLocation { get; set; }
        public string Remark { get; set; }
        public Guid TransKeyId { get; set; }
        public GetTransKeyDetail TransKeyDetail { get; set; }
        public GetSysOrgranizeDetail GetSysOrgranizeDetail { get; set; }
    }

    public class GetTransKeyDetail
    {
        public string Name { get; set; }
        public string EquipmentCode { get; set; }
        public bool IsActive { get; set; }
        public GetSysKeyTypeDetail GetSysKeyTypeDetail { get; set; }
    }

    public class GetSysKeyTypeDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GetSysOrgranizeDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DepartmentCode { get; set; }
        public string DivisionCode { get; set; }
        public string SectionCode { get; set; }
        public string JobCode { get; set; }
    }

    public class GetSysUserDetail
    {
        public Guid Id { get; set; }
        public string FullName {  get; set; }
    }
}
