using bma_license_repository.CustomModel.SysKeyType;
using bma_license_repository.CustomModel.SysOrgranize;
using bma_license_repository.CustomModel.TransEquipment;
namespace bma_license_repository.CustomModel.TransKey
{
    public class GetTransKey
    {
        public Guid Id { get; set; }

        public string License { get; set; } = null!;

        public Guid? SysOrgranizeId { get; set; }

        public string? Remark { get; set; }

        public bool IsActive { get; set; }

        public Guid SysKeyTypeId { get; set; }

        public int Seq { get; set; }

        public GetSysKeyType GetSysKeyType { get; set; }
        public GetSysOrgranize? GetSysOrgranize { get; set; }
        public List<GetTransEquipment>? GetTransEquipment { get; set; }
    }
}

