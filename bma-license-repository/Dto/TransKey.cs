using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class TransKey
{
    public Guid Id { get; set; }

    /// <summary>
    /// ชื่อ License
    /// </summary>
    public string License { get; set; } = null!;

    /// <summary>
    /// รหัสหน่วยงาน
    /// </summary>
    public Guid? SysOrgranizeId { get; set; }

    public string? Remark { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    /// <summary>
    /// ประเภท License
    /// </summary>
    public Guid SysKeyTypeId { get; set; }

    public virtual SysUser CreatedByNavigation { get; set; } = null!;

    public virtual SysKeyType SysKeyType { get; set; } = null!;

    public virtual SysOrgranize? SysOrgranize { get; set; }

    public virtual ICollection<TransEquipment> TransEquipments { get; set; } = new List<TransEquipment>();

    public virtual ICollection<TransJobRepair> TransJobRepairs { get; set; } = new List<TransJobRepair>();

    public virtual ICollection<TransKeyHistory> TransKeyHistories { get; set; } = new List<TransKeyHistory>();

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
