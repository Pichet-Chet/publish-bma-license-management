using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class TransJobRepair
{
    public Guid Id { get; set; }

    public Guid TransKeyId { get; set; }

    public Guid SysRepairCategoryId { get; set; }

    public string? Description { get; set; }

    public string? Name { get; set; }

    public string? Telephone { get; set; }

    public string? Remark { get; set; }

    public Guid SysJobRepairStatusId { get; set; }

    public DateTime? DateOfRequest { get; set; }

    public DateTime? DateOfStart { get; set; }

    public DateTime? DateOfFixed { get; set; }

    public string? FixedDescription { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int Seq { get; set; }

    public Guid? TransEquipmentId { get; set; }

    public virtual SysUser CreatedByNavigation { get; set; } = null!;

    public virtual SysJobRepairStatus SysJobRepairStatus { get; set; } = null!;

    public virtual SysRepairCategory SysRepairCategory { get; set; } = null!;

    public virtual TransEquipment? TransEquipment { get; set; }

    public virtual TransKey TransKey { get; set; } = null!;

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
