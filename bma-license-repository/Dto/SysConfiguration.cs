using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class SysConfiguration
{
    public Guid Id { get; set; }

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string? Group { get; set; }

    public int Seq { get; set; }

    public virtual SysUser CreatedByNavigation { get; set; } = null!;

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
