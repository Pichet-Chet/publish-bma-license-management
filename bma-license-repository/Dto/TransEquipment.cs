using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class TransEquipment
{
    public Guid Id { get; set; }

    public string? Brand { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? EquipmentCode { get; set; }

    public string? Generation { get; set; }

    public DateTime? InstallDate { get; set; }

    public string? InstallLocation { get; set; }

    public string? IpAddress { get; set; }

    public bool IsActive { get; set; }

    public string? MacAddress { get; set; }

    public string? MachineName { get; set; }

    public string? MachineNumber { get; set; }

    public string? MachineType { get; set; }

    public string? Remark { get; set; }

    public Guid TransKeyId { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual SysUser CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<TransJobRepair> TransJobRepairs { get; set; } = new List<TransJobRepair>();

    public virtual TransKey TransKey { get; set; } = null!;

    public virtual SysUser UpdatedByNavigation { get; set; } = null!;
}
