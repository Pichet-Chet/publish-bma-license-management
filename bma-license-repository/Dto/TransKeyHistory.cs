using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class TransKeyHistory
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid SysOrgranizeId { get; set; }

    public string? Remark { get; set; }

    public Guid TransKeyId { get; set; }

    public Guid ActionBy { get; set; }

    public virtual SysUser ActionByNavigation { get; set; } = null!;

    public virtual SysOrgranize SysOrgranize { get; set; } = null!;

    public virtual TransKey TransKey { get; set; } = null!;
}
