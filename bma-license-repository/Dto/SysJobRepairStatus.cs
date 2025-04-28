using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class SysJobRepairStatus
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int Seq { get; set; }

    public virtual SysUser CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<TransJobRepair> TransJobRepairs { get; set; } = new List<TransJobRepair>();

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
