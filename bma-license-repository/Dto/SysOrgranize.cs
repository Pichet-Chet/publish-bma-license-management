using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class SysOrgranize
{
    public Guid Id { get; set; }

    /// <summary>
    /// ชื่อหน่วยงาน
    /// </summary>
    public string Name { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public string DivisionCode { get; set; } = null!;

    public string SectionCode { get; set; } = null!;

    public string JobCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    /// <summary>
    /// DEPARTMENT , DIVISION , SECTION , JOB
    /// </summary>
    public string TypeName { get; set; } = null!;

    public string Code { get; set; } = null!;

    public int Seq { get; set; }

    public string? InstallatioLocation { get; set; }

    public virtual SysUser CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<TransKeyHistory> TransKeyHistories { get; set; } = new List<TransKeyHistory>();

    public virtual ICollection<TransKey> TransKeys { get; set; } = new List<TransKey>();

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
