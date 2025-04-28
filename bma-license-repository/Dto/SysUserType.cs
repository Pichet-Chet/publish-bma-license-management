using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class SysUserType
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

    public virtual ICollection<SysUser> SysUsers { get; set; } = new List<SysUser>();

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
