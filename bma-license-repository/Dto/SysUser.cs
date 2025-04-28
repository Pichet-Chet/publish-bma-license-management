using System;
using System.Collections.Generic;

namespace bma_license_repository.Dto;

public partial class SysUser
{
    public Guid Id { get; set; }

    public Guid SysUserTypeId { get; set; }

    public Guid SysUserGroupId { get; set; }

    public Guid? SysUserPermissionId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? AccessToken { get; set; }

    public string? AccessTokenExpire { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int Seq { get; set; }

    public string? DepartmentCode { get; set; }

    public string? DivisionCode { get; set; }

    public string? SectionCode { get; set; }

    public string? JobCode { get; set; }

    public virtual ICollection<SysConfiguration> SysConfigurationCreatedByNavigations { get; set; } = new List<SysConfiguration>();

    public virtual ICollection<SysConfiguration> SysConfigurationUpdatedByNavigations { get; set; } = new List<SysConfiguration>();

    public virtual ICollection<SysJobRepairStatus> SysJobRepairStatusCreatedByNavigations { get; set; } = new List<SysJobRepairStatus>();

    public virtual ICollection<SysJobRepairStatus> SysJobRepairStatusUpdatedByNavigations { get; set; } = new List<SysJobRepairStatus>();

    public virtual ICollection<SysKeyType> SysKeyTypeCreatedByNavigations { get; set; } = new List<SysKeyType>();

    public virtual ICollection<SysKeyType> SysKeyTypeUpdatedByNavigations { get; set; } = new List<SysKeyType>();

    public virtual ICollection<SysMessageConfiguration> SysMessageConfigurationCreatedByNavigations { get; set; } = new List<SysMessageConfiguration>();

    public virtual ICollection<SysMessageConfiguration> SysMessageConfigurationUpdatedByNavigations { get; set; } = new List<SysMessageConfiguration>();

    public virtual ICollection<SysOrgranize> SysOrgranizeCreatedByNavigations { get; set; } = new List<SysOrgranize>();

    public virtual ICollection<SysOrgranize> SysOrgranizeUpdatedByNavigations { get; set; } = new List<SysOrgranize>();

    public virtual ICollection<SysRepairCategory> SysRepairCategoryCreatedByNavigations { get; set; } = new List<SysRepairCategory>();

    public virtual ICollection<SysRepairCategory> SysRepairCategoryUpdatedByNavigations { get; set; } = new List<SysRepairCategory>();

    public virtual SysUserGroup SysUserGroup { get; set; } = null!;

    public virtual ICollection<SysUserGroup> SysUserGroupCreatedByNavigations { get; set; } = new List<SysUserGroup>();

    public virtual ICollection<SysUserGroup> SysUserGroupUpdatedByNavigations { get; set; } = new List<SysUserGroup>();

    public virtual SysUserPermission? SysUserPermission { get; set; }

    public virtual ICollection<SysUserPermission> SysUserPermissionCreatedByNavigations { get; set; } = new List<SysUserPermission>();

    public virtual ICollection<SysUserPermission> SysUserPermissionUpdatedByNavigations { get; set; } = new List<SysUserPermission>();

    public virtual SysUserType SysUserType { get; set; } = null!;

    public virtual ICollection<SysUserType> SysUserTypeCreatedByNavigations { get; set; } = new List<SysUserType>();

    public virtual ICollection<SysUserType> SysUserTypeUpdatedByNavigations { get; set; } = new List<SysUserType>();

    public virtual ICollection<TransEquipment> TransEquipmentCreatedByNavigations { get; set; } = new List<TransEquipment>();

    public virtual ICollection<TransEquipment> TransEquipmentUpdatedByNavigations { get; set; } = new List<TransEquipment>();

    public virtual ICollection<TransJobRepair> TransJobRepairCreatedByNavigations { get; set; } = new List<TransJobRepair>();

    public virtual ICollection<TransJobRepair> TransJobRepairUpdatedByNavigations { get; set; } = new List<TransJobRepair>();

    public virtual ICollection<TransKey> TransKeyCreatedByNavigations { get; set; } = new List<TransKey>();

    public virtual ICollection<TransKeyHistory> TransKeyHistories { get; set; } = new List<TransKeyHistory>();

    public virtual ICollection<TransKey> TransKeyUpdatedByNavigations { get; set; } = new List<TransKey>();
}
