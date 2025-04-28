

using bma_license_repository.CustomModel.SysJobRerpairStatus;
using bma_license_repository.CustomModel.SysRepairCetegory;
using bma_license_repository.CustomModel.TransKey;

namespace bma_license_repository.CustomModel.TransJobRepair
{
    public class GetTransJobRepair
    {
        public Guid Id { get; set; }
        public Guid TransKeyId { get; set; }
        public Guid SysRepairCategoryId { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string? Remark { get; set; }
        public Guid SysJobRepairStatusId { get; set; }
        public DateTime? DateOfRequest { get; set; }
        public DateTime? DateOfStart { get; set; }
        public DateTime? DateOfFixed { get; set; }
        public string? FixedDescription { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Seq { get; set; }
        public Guid? TransEquipmentId { get; set; }
        public GetSysRepairCategory ISysRepairCategory { get; set; }
        public GetSysJobRepairStatus ISysJobRepairStatus { get; set; }
        public GetTransKey ITransKey { get; set; }
        public GetUser UserCreate { get; set; }
        public GetTransEquipmentByTransJobRepair? TransEquipment { get; set; }

        public GetTransKeyByTransJobRepair TransKey { get; set; }
    }

    public class GetUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class GetTransEquipmentByTransJobRepair
    {
        public Guid Id { get; set; }
        public string EquipmentCode { get; set; }
    }

    public class GetTransKeyByTransJobRepair
    {
        public Guid Id { get; set; }
        public GetSysOrgranizeByTransKey? SysOrgranize { get; set; }
    }

    public class GetSysOrgranizeByTransKey
    {
        public Guid? Id { get; set; }
        public string DepartmentCode { get; set; }
        public string DivisionCode { get; set; }
        public string SectionCode { get; set; }
        public string JobCode { get; set; }
    }

}
