using bma_license_repository.CustomModel.SysConfiguration;
using bma_license_repository.CustomModel.SysJobRerpairStatus;
using bma_license_repository.CustomModel.SysKeyType;
using bma_license_repository.CustomModel.SysOrgranize;
using bma_license_repository.CustomModel.SysRepairCetegory;
using bma_license_repository.CustomModel.SysUser;
using bma_license_repository.CustomModel.SysUserGroup;
using bma_license_repository.CustomModel.SysUserPermission;
using bma_license_repository.CustomModel.SysUserType;
using bma_license_repository.CustomModel.TransEquipment;
using bma_license_repository.CustomModel.TransJobRepair;
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.CustomModel.TransKeyHistory;
using bma_license_repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace bma_license_repository.Helper
{
    public static class ConvertModel
    {
        #region | SysUser |
        public static async Task<List<GetSysUser>> ConvertToListAsync(this IQueryable<SysUser> query)
        {
            return await query.Select(a => new GetSysUser
            {
                AccessToken = a.AccessToken,
                AccessTokenExpire = a.AccessTokenExpire,
                FullName = a.FullName,
                Id = a.Id,
                IsActive = a.IsActive,
                SysUserGroupId = a.SysUserGroupId,
                SysUserGroupName = a.SysUserGroup.Name,
                SysUserPermissionId = a.SysUserPermissionId,
                SysUserPermissionName = a.SysUserPermission != null ? a.SysUserPermission.Name : string.Empty,
                SysUserTypeId = a.SysUserTypeId,
                SysUserTypeName = a.SysUserType.Name,
                UserName = a.Username,
                DepartmentCode = a.DepartmentCode,
                DivisionCode = a.DivisionCode,
                SectionCode = a.SectionCode,
                JobCode = a.JobCode,
            }).ToListAsync();
        }

        public static async Task<List<GetSysUser>> ConvertToListAsync(this IQueryable<SysUser> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetSysUser
            {
                AccessToken = a.AccessToken,
                AccessTokenExpire = a.AccessTokenExpire,
                FullName = a.FullName,
                Id = a.Id,
                IsActive = a.IsActive,
                SysUserGroupId = a.SysUserGroupId,
                SysUserGroupName = a.SysUserGroup.Name,
                SysUserPermissionId = a.SysUserPermissionId,
                SysUserPermissionName = a.SysUserPermission != null ? a.SysUserPermission.Name : string.Empty,
                SysUserTypeId = a.SysUserTypeId,
                SysUserTypeName = a.SysUserType.Name,
                UserName = a.Username,
                DepartmentCode = a.DepartmentCode,
                DivisionCode = a.DivisionCode,
                SectionCode = a.SectionCode,
                JobCode = a.JobCode,
            })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(); ;
        }

        public static async Task<GetSysUser> ConvertToFirstOrDefaultAsync(this IQueryable<SysUser> query)
        {
            return await query.Select(a => new GetSysUser
            {
                AccessToken = a.AccessToken,
                AccessTokenExpire = a.AccessTokenExpire,
                FullName = a.FullName,
                Id = a.Id,
                IsActive = a.IsActive,
                SysUserGroupId = a.SysUserGroupId,
                SysUserGroupName = a.SysUserGroup.Name,
                SysUserPermissionId = a.SysUserPermissionId,
                SysUserPermissionName = a.SysUserPermission != null ? a.SysUserPermission.Name : string.Empty,
                SysUserTypeId = a.SysUserTypeId,
                SysUserTypeName = a.SysUserType.Name,
                UserName = a.Username,
                DepartmentCode = a.DepartmentCode,
                DivisionCode = a.DivisionCode,
                SectionCode = a.SectionCode,
                JobCode = a.JobCode,
            }).FirstOrDefaultAsync();
        }
        #endregion

        #region | SysOrgranize |
        public static async Task<List<GetSysOrgranize>> ConvertToListAsync(this IQueryable<SysOrgranize> query)
        {
            return await query.Select(a => new GetSysOrgranize
            {
                Code = a.Code,
                Name = a.Name,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                DepartmentCode = a.DepartmentCode,
                DivisionCode = a.DivisionCode,
                Id = a.Id,
                IsActive = a.IsActive,
                JobCode = a.JobCode,
                SectionCode = a.SectionCode,
                Seq = a.Seq,
                TypeName = a.TypeName,
                UpdatedBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
            }).ToListAsync();
        }

        public static async Task<List<GetSysOrgranize>> ConvertToListAsync(this IQueryable<SysOrgranize> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetSysOrgranize
            {
                Code = a.Code,
                Name = a.Name,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                DepartmentCode = a.DepartmentCode,
                DivisionCode = a.DivisionCode,
                Id = a.Id,
                IsActive = a.IsActive,
                JobCode = a.JobCode,
                SectionCode = a.SectionCode,
                Seq = a.Seq,
                TypeName = a.TypeName,
                UpdatedBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public static async Task<GetSysOrgranize> ConvertToFirstOrDefaultAsync(this IQueryable<SysOrgranize> query)
        {
            return await query.Select(a => new GetSysOrgranize
            {
                Code = a.Code,
                Name = a.Name,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                DepartmentCode = a.DepartmentCode,
                DivisionCode = a.DivisionCode,
                Id = a.Id,
                IsActive = a.IsActive,
                JobCode = a.JobCode,
                SectionCode = a.SectionCode,
                Seq = a.Seq,
                TypeName = a.TypeName,
                UpdatedBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
            }).FirstOrDefaultAsync();
        }
        #endregion

        #region | SysUserGroup |
        public static async Task<List<GetSysUserGroup>> ConvertToListAsync(this IQueryable<SysUserGroup> query)
        {
            return await query.Select(a => new GetSysUserGroup
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive,
                Seq = a.Seq,
            }).ToListAsync();
        }

        public static async Task<List<GetSysUserGroup>> ConvertToListAsync(this IQueryable<SysUserGroup> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetSysUserGroup
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive,
                Seq = a.Seq,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public static async Task<GetSysUserGroup> ConvertToFirstOrDefaultAsync(this IQueryable<SysUserGroup> query)
        {
            return await query.Select(a => new GetSysUserGroup
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive,
                Seq = a.Seq,
            }).FirstOrDefaultAsync();
        }
        #endregion

        #region | SysUserPermission |
        public static async Task<List<GetSysUserPermission>> ConvertToListAsync(this IQueryable<SysUserPermission> query)
        {
            return await query
                .Select(a => new GetSysUserPermission
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsActive = a.IsActive,
                    Seq = a.Seq,
                }).ToListAsync();
        }

        public static async Task<List<GetSysUserPermission>> ConvertToListAsync(this IQueryable<SysUserPermission> query, int pageSize, int pageNumber)
        {
            return await query
                .Select(a => new GetSysUserPermission
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsActive = a.IsActive,
                    Seq = a.Seq,
                }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static async Task<GetSysUserPermission> ConvertToFirstOrDefaultAsync(this IQueryable<SysUserPermission> query)
        {
            return await query
                .Select(a => new GetSysUserPermission
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsActive = a.IsActive,
                    Seq = a.Seq,
                }).FirstOrDefaultAsync();
        }
        #endregion

        #region | SysUserType |
        public static async Task<List<GetSysUserType>> ConvertToListAsync(this IQueryable<SysUserType> query)
        {
            return await query
              .Select(a => new GetSysUserType
              {
                  Id = a.Id,
                  Name = a.Name,
                  IsActive = a.IsActive,
                  Seq = a.Seq,
              }).ToListAsync();
        }

        public static async Task<List<GetSysUserType>> ConvertToListAsync(this IQueryable<SysUserType> query, int pageSize, int pageNumber)
        {
            return await query
              .Select(a => new GetSysUserType
              {
                  Id = a.Id,
                  Name = a.Name,
                  IsActive = a.IsActive,
                  Seq = a.Seq,
              }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static async Task<GetSysUserType> ConvertToFirstOrDefaultAsync(this IQueryable<SysUserType> query)
        {
            return await query
              .Select(a => new GetSysUserType
              {
                  Id = a.Id,
                  Name = a.Name,
                  IsActive = a.IsActive,
                  Seq = a.Seq,
              }).FirstOrDefaultAsync();
        }
        #endregion

        #region | Configuration |
        public static async Task<List<GetSysConfiguration>> ConvertToListAsync(this IQueryable<SysConfiguration> query)
        {
            return await query.Select(a => new GetSysConfiguration
            {
                Id = a.Id,
                Description = a.Description,
                IsActive = a.IsActive,
                Key = a.Key,
                Value = a.Value,
            }).ToListAsync();
        }

        public static async Task<List<GetSysConfiguration>> ConvertToListAsync(this IQueryable<SysConfiguration> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetSysConfiguration
            {
                Id = a.Id,
                Description = a.Description,
                IsActive = a.IsActive,
                Key = a.Key,
                Value = a.Value,
            }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static async Task<GetSysConfiguration> ConvertToFirstOrDefaultAsync(this IQueryable<SysConfiguration> query)
        {
            return await query.Select(a => new GetSysConfiguration
            {
                Id = a.Id,
                Description = a.Description,
                IsActive = a.IsActive,
                Key = a.Key,
                Value = a.Value,
            }).FirstOrDefaultAsync();
        }
        #endregion

        #region | SysKeyType |

        public static async Task<List<GetSysKeyType>> ConvertToListAsync(this IQueryable<SysKeyType> query)
        {
            return await query.Select(a => new GetSysKeyType
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
            }).ToListAsync();
        }

        public static async Task<GetSysKeyType> ConvertToFirstOrDefaultAsync(this IQueryable<SysKeyType> query)
        {
            return await query.Select(a => new GetSysKeyType
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
            }).FirstOrDefaultAsync();
        }

        #endregion

        #region | TransKey |

        public static async Task<List<GetTransKey>> ConvertToListAsync(this IQueryable<TransKey> query)
        {
            return await query.Select(a => new GetTransKey
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Remark = a.Remark,
                License = a.License,
                SysKeyTypeId = a.SysKeyTypeId,
                SysOrgranizeId = a.SysOrgranizeId,
                GetSysKeyType = new GetSysKeyType
                {
                    Id = a.SysKeyType.Id,
                    IsActive = a.SysKeyType.IsActive,
                    Name = a.SysKeyType.Name,
                    Seq = a.SysKeyType.Seq
                },
                GetSysOrgranize = a.SysOrgranize == null ? null : new CustomModel.SysOrgranize.GetSysOrgranize
                {
                    Id = a.SysOrgranize.Id,
                    Code = a.SysOrgranize.Code,
                    DepartmentCode = a.SysOrgranize.DepartmentCode,
                    DivisionCode = a.SysOrgranize.DivisionCode,
                    IsActive = a.SysOrgranize.IsActive,
                    JobCode = a.SysOrgranize.JobCode,
                    Name = a.SysOrgranize.Name,
                    SectionCode = a.SysOrgranize.SectionCode,
                    Seq = a.SysOrgranize.Seq,
                    TypeName = a.SysOrgranize.TypeName,
                },
                GetTransEquipment = a.TransEquipments == null ? null : a.TransEquipments.Select(b => new GetTransEquipment
                {
                    Brand = b.Brand,
                    EquipmentCode = b.EquipmentCode,
                    Generation = b.Generation,
                    Id = b.Id,
                    InstallDate = b.InstallDate,
                    InstallLocation = b.InstallLocation,
                    IpAddress = b.IpAddress,
                    MacAddress = b.MacAddress,
                    MachineName = b.MachineName,
                    MachineNumber = b.MachineName,
                    MachineType = b.MachineType,
                    Remark = b.Remark,
                    TransKeyId = b.TransKeyId
                }).ToList()
            }).ToListAsync();
        }

        public static async Task<List<GetTransKey>> ConvertToListAsync(this IQueryable<TransKey> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetTransKey
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Remark = a.Remark,
                License = a.License,
                SysKeyTypeId = a.SysKeyTypeId,
                SysOrgranizeId = a.SysOrgranizeId,
                GetSysKeyType = new GetSysKeyType
                {
                    Id = a.SysKeyType.Id,
                    IsActive = a.SysKeyType.IsActive,
                    Name = a.SysKeyType.Name,
                    Seq = a.SysKeyType.Seq
                },
                GetSysOrgranize = a.SysOrgranize == null ? null : new CustomModel.SysOrgranize.GetSysOrgranize
                {
                    Id = a.SysOrgranize.Id,
                    Code = a.SysOrgranize.Code,
                    DepartmentCode = a.SysOrgranize.DepartmentCode,
                    DivisionCode = a.SysOrgranize.DivisionCode,
                    IsActive = a.SysOrgranize.IsActive,
                    JobCode = a.SysOrgranize.JobCode,
                    Name = a.SysOrgranize.Name,
                    SectionCode = a.SysOrgranize.SectionCode,
                    Seq = a.SysOrgranize.Seq,
                    TypeName = a.SysOrgranize.TypeName,
                },
                GetTransEquipment = a.TransEquipments == null ? null : a.TransEquipments.Select(b => new GetTransEquipment
                {
                    Brand = b.Brand,
                    EquipmentCode = b.EquipmentCode,
                    Generation = b.Generation,
                    Id = b.Id,
                    InstallDate = b.InstallDate,
                    InstallLocation = b.InstallLocation,
                    IpAddress = b.IpAddress,
                    MacAddress = b.MacAddress,
                    MachineName = b.MachineName,
                    MachineNumber = b.MachineName,
                    MachineType = b.MachineType,
                    Remark = b.Remark,
                    TransKeyId = b.TransKeyId
                }).ToList()
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public static async Task<GetTransKey> ConvertToFirstOrDefaultAsync(this IQueryable<TransKey> query)
        {
            return await query.Select(a => new GetTransKey
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Remark = a.Remark,
                License = a.License,
                SysKeyTypeId = a.SysKeyTypeId,
                SysOrgranizeId = a.SysOrgranizeId,
                GetSysKeyType = new GetSysKeyType
                {
                    Id = a.SysKeyType.Id,
                    IsActive = a.SysKeyType.IsActive,
                    Name = a.SysKeyType.Name,
                    Seq = a.SysKeyType.Seq
                },
                GetSysOrgranize = a.SysOrgranize == null ? null : new CustomModel.SysOrgranize.GetSysOrgranize
                {
                    Id = a.SysOrgranize.Id,
                    Code = a.SysOrgranize.Code,
                    DepartmentCode = a.SysOrgranize.DepartmentCode,
                    DivisionCode = a.SysOrgranize.DivisionCode,
                    IsActive = a.SysOrgranize.IsActive,
                    JobCode = a.SysOrgranize.JobCode,
                    Name = a.SysOrgranize.Name,
                    SectionCode = a.SysOrgranize.SectionCode,
                    Seq = a.SysOrgranize.Seq,
                    TypeName = a.SysOrgranize.TypeName,
                },
                GetTransEquipment = a.TransEquipments == null ? null : a.TransEquipments.Select(b => new GetTransEquipment
                {
                    Brand = b.Brand,
                    EquipmentCode = b.EquipmentCode,
                    Generation = b.Generation,
                    Id = b.Id,
                    InstallDate = b.InstallDate,
                    InstallLocation = b.InstallLocation,
                    IpAddress = b.IpAddress,
                    MacAddress = b.MacAddress,
                    MachineName = b.MachineName,
                    MachineNumber = b.MachineName,
                    MachineType = b.MachineType,
                    Remark = b.Remark,
                    TransKeyId = b.TransKeyId
                }).ToList()
            }).FirstOrDefaultAsync();
        }

        #endregion

        #region | SysRepairCategory |

        public static async Task<List<GetSysRepairCategory>> ConvertToListAsync(this IQueryable<SysRepairCategory> query)
        {
            return await query.Select(a => new GetSysRepairCategory
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
            }).ToListAsync();
        }

        public static async Task<GetSysRepairCategory> ConvertToFirstOrDefaultAsync(this IQueryable<SysRepairCategory> query)
        {
            return await query.Select(a => new GetSysRepairCategory
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
            }).FirstOrDefaultAsync();
        }

        #endregion

        #region | JobRepairStatus |
        public static async Task<List<GetSysJobRepairStatus>> ConvertToListAsync(this IQueryable<SysJobRepairStatus> query)
        {
            return await query.Select(a => new GetSysJobRepairStatus
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
            }).ToListAsync();
        }

        public static async Task<GetSysJobRepairStatus> ConvertToFirstOrDefaultAsync(this IQueryable<SysJobRepairStatus> query)
        {
            return await query.Select(a => new GetSysJobRepairStatus
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
            }).FirstOrDefaultAsync();
        }
        #endregion

        #region | TransJobRepair |
        public static async Task<List<GetTransJobRepair>> ConvertToListAsync(this IQueryable<TransJobRepair> query)
        {
            return await query.Select(a => new GetTransJobRepair
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                DateOfFixed = a.DateOfFixed,
                DateOfRequest = a.DateOfRequest,
                DateOfStart = a.DateOfStart,
                Description = a.Description,
                FixedDescription = a.FixedDescription,
                Remark = a.Remark,
                SysJobRepairStatusId = a.SysJobRepairStatusId,
                SysRepairCategoryId = a.SysRepairCategoryId,
                Telephone = a.Telephone,
                TransKeyId = a.TransKeyId,
                UpdateBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
                ISysJobRepairStatus = new GetSysJobRepairStatus
                {
                    Id = a.SysJobRepairStatus.Id,
                    IsActive = a.SysJobRepairStatus.IsActive,
                    Name = a.SysJobRepairStatus.Name,
                    Seq = a.SysJobRepairStatus.Seq,
                },
                ISysRepairCategory = new GetSysRepairCategory
                {
                    Id = a.SysRepairCategory.Id,
                    IsActive = a.SysRepairCategory.IsActive,
                    Name = a.SysRepairCategory.Name,
                    Seq = a.SysRepairCategory.Seq,
                },
                ITransKey = new GetTransKey
                {
                    Id = a.Id,
                    IsActive = a.IsActive,
                    License = a.TransKey.License,
                    Seq = a.Seq,
                    Remark = a.Remark,
                    SysKeyTypeId = a.TransKey.SysKeyTypeId,
                    SysOrgranizeId = a.TransKey.SysOrgranizeId,
                },
                UserCreate = new GetUser
                {
                    FullName = a.CreatedByNavigation.FullName,
                    Id = a.CreatedByNavigation.Id,
                    UserName = a.CreatedByNavigation.Username
                },
                TransEquipment = a.TransEquipment != null ? new GetTransEquipmentByTransJobRepair
                {
                    Id = a.TransEquipment.Id,
                    EquipmentCode = a.TransEquipment.EquipmentCode
                } : null,
                TransEquipmentId = a.TransEquipmentId,
                TransKey = new GetTransKeyByTransJobRepair
                {
                    Id = a.TransKey.Id,
                    SysOrgranize = a.TransKey.SysOrgranize != null ? new GetSysOrgranizeByTransKey
                    {
                        DepartmentCode = a.TransKey.SysOrgranize.DepartmentCode,
                        DivisionCode = a.TransKey.SysOrgranize.DivisionCode,
                        Id = a.TransKey.SysOrgranizeId,
                        JobCode = a.TransKey.SysOrgranize.JobCode,
                        SectionCode = a.TransKey.SysOrgranize.SectionCode,
                    } : null,
                }
            }).ToListAsync();
        }

        public static async Task<List<GetTransJobRepair>> ConvertToListAsync(this IQueryable<TransJobRepair> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetTransJobRepair
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                DateOfFixed = a.DateOfFixed,
                DateOfRequest = a.DateOfRequest,
                DateOfStart = a.DateOfStart,
                Description = a.Description,
                FixedDescription = a.FixedDescription,
                Remark = a.Remark,
                SysJobRepairStatusId = a.SysJobRepairStatusId,
                SysRepairCategoryId = a.SysRepairCategoryId,
                Telephone = a.Telephone,
                TransKeyId = a.TransKeyId,
                UpdateBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
                ISysJobRepairStatus = new GetSysJobRepairStatus
                {
                    Id = a.SysJobRepairStatus.Id,
                    IsActive = a.SysJobRepairStatus.IsActive,
                    Name = a.SysJobRepairStatus.Name,
                    Seq = a.SysJobRepairStatus.Seq,
                },
                ISysRepairCategory = new GetSysRepairCategory
                {
                    Id = a.SysRepairCategory.Id,
                    IsActive = a.SysRepairCategory.IsActive,
                    Name = a.SysRepairCategory.Name,
                    Seq = a.SysRepairCategory.Seq,
                },
                ITransKey = new GetTransKey
                {
                    Id = a.Id,
                    IsActive = a.IsActive,
                    License = a.TransKey.License,
                    Seq = a.Seq,
                    Remark = a.Remark,
                    SysKeyTypeId = a.TransKey.SysKeyTypeId,
                    SysOrgranizeId = a.TransKey.SysOrgranizeId,
                },
                UserCreate = new GetUser
                {
                    FullName = a.CreatedByNavigation.FullName,
                    Id = a.CreatedByNavigation.Id,
                    UserName = a.CreatedByNavigation.Username
                },
                TransEquipment = a.TransEquipment != null ? new GetTransEquipmentByTransJobRepair
                {
                    Id = a.TransEquipment.Id,
                    EquipmentCode = a.TransEquipment.EquipmentCode
                } : null,
                TransKey = new GetTransKeyByTransJobRepair
                {
                    Id = a.TransKey.Id,
                    SysOrgranize = a.TransKey.SysOrgranize != null ? new GetSysOrgranizeByTransKey
                    {
                        DepartmentCode = a.TransKey.SysOrgranize.DepartmentCode,
                        DivisionCode = a.TransKey.SysOrgranize.DivisionCode,
                        Id = a.TransKey.SysOrgranizeId,
                        JobCode = a.TransKey.SysOrgranize.JobCode,
                        SectionCode = a.TransKey.SysOrgranize.SectionCode,
                    } : null,
                }
            }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static async Task<GetTransJobRepair> ConvertToFirstOrDefaultAsync(this IQueryable<TransJobRepair> query)
        {
            return await query.Select(a => new GetTransJobRepair
            {
                Id = a.Id,
                IsActive = a.IsActive,
                Name = a.Name,
                Seq = a.Seq,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                DateOfFixed = a.DateOfFixed,
                DateOfRequest = a.DateOfRequest,
                DateOfStart = a.DateOfStart,
                Description = a.Description,
                FixedDescription = a.FixedDescription,
                Remark = a.Remark,
                SysJobRepairStatusId = a.SysJobRepairStatusId,
                SysRepairCategoryId = a.SysRepairCategoryId,
                Telephone = a.Telephone,
                TransKeyId = a.TransKeyId,
                UpdateBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
                ISysJobRepairStatus = new GetSysJobRepairStatus
                {
                    Id = a.SysJobRepairStatus.Id,
                    IsActive = a.SysJobRepairStatus.IsActive,
                    Name = a.SysJobRepairStatus.Name,
                    Seq = a.SysJobRepairStatus.Seq,
                },
                ISysRepairCategory = new GetSysRepairCategory
                {
                    Id = a.SysRepairCategory.Id,
                    IsActive = a.SysRepairCategory.IsActive,
                    Name = a.SysRepairCategory.Name,
                    Seq = a.SysRepairCategory.Seq,
                },
                ITransKey = new GetTransKey
                {
                    Id = a.Id,
                    IsActive = a.IsActive,
                    License = a.TransKey.License,
                    Seq = a.Seq,
                    Remark = a.Remark,
                    SysKeyTypeId = a.TransKey.SysKeyTypeId,
                    SysOrgranizeId = a.TransKey.SysOrgranizeId,
                },
                UserCreate = new GetUser
                {
                    FullName = a.CreatedByNavigation.FullName,
                    Id = a.CreatedByNavigation.Id,
                    UserName = a.CreatedByNavigation.Username
                },
                TransEquipment = a.TransEquipment != null ? new GetTransEquipmentByTransJobRepair
                {
                    Id = a.TransEquipment.Id,
                    EquipmentCode = a.TransEquipment.EquipmentCode
                } : null,
                TransKey = new GetTransKeyByTransJobRepair
                {
                    Id = a.TransKey.Id,
                    SysOrgranize = a.TransKey.SysOrgranize != null ? new GetSysOrgranizeByTransKey
                    {
                        DepartmentCode = a.TransKey.SysOrgranize.DepartmentCode,
                        DivisionCode = a.TransKey.SysOrgranize.DivisionCode,
                        Id = a.TransKey.SysOrgranizeId,
                        JobCode = a.TransKey.SysOrgranize.JobCode,
                        SectionCode = a.TransKey.SysOrgranize.SectionCode,
                    } : null,
                }
            }).FirstOrDefaultAsync();
        }
        #endregion

        #region | TransKeyHistory |

        public static async Task<List<GetTransKeyHistory>> ConvertToListAsync(this IQueryable<TransKeyHistory> query)
        {
            return await query.Select(a => new GetTransKeyHistory
            {
                ActionBy = a.ActionBy,
                EndDate = a.EndDate,
                Id = a.Id,
                Remark = a.Remark,
                StartDate = a.StartDate,
                SysOrgranizeId = a.SysOrgranizeId,
                TransKeyId = a.TransKeyId,
                TransKeyDetail = new GetTransKeyDetail
                {
                    IsActive = a.TransKey.IsActive,
                    GetSysKeyTypeDetail = new GetSysKeyTypeDetail
                    {
                        Id = a.TransKey.SysKeyType.Id,
                        Name = a.TransKey.SysKeyType.Name
                    },
                },
                GetSysOrgranizeDetail = new GetSysOrgranizeDetail
                {
                    DepartmentCode = a.SysOrgranize.DepartmentCode,
                    DivisionCode = a.SysOrgranize.DivisionCode,
                    Id = a.SysOrgranizeId,
                    JobCode = a.SysOrgranize.JobCode,
                    Name = a.SysOrgranize.Name,
                    SectionCode = a.SysOrgranize.SectionCode,
                }
            }).ToListAsync();
        }

        public static async Task<List<GetTransKeyHistory>> ConvertToListAsync(this IQueryable<TransKeyHistory> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetTransKeyHistory
            {
                ActionBy = a.ActionBy,
                EndDate = a.EndDate,
                Id = a.Id,
                Remark = a.Remark,
                StartDate = a.StartDate,
                SysOrgranizeId = a.SysOrgranizeId,
                TransKeyId = a.TransKeyId,
                TransKeyDetail = new GetTransKeyDetail
                {
                    IsActive = a.TransKey.IsActive,
                    GetSysKeyTypeDetail = new GetSysKeyTypeDetail
                    {
                        Id = a.TransKey.SysKeyType.Id,
                        Name = a.TransKey.SysKeyType.Name
                    }
                },
                GetSysOrgranizeDetail = new GetSysOrgranizeDetail
                {
                    DepartmentCode = a.SysOrgranize.DepartmentCode,
                    DivisionCode = a.SysOrgranize.DivisionCode,
                    Id = a.SysOrgranizeId,
                    JobCode = a.SysOrgranize.JobCode,
                    Name = a.SysOrgranize.Name,
                    SectionCode = a.SysOrgranize.SectionCode,
                }
            }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static async Task<GetTransKeyHistory> ConvertToFirstOrDefaultAsync(this IQueryable<TransKeyHistory> query)
        {
            return await query.Select(a => new GetTransKeyHistory
            {
                ActionBy = a.ActionBy,
                EndDate = a.EndDate,
                Id = a.Id,
                Remark = a.Remark,
                StartDate = a.StartDate,
                SysOrgranizeId = a.SysOrgranizeId,
                TransKeyId = a.TransKeyId,
                TransKeyDetail = new GetTransKeyDetail
                {
                    IsActive = a.TransKey.IsActive,
                    GetSysKeyTypeDetail = new GetSysKeyTypeDetail
                    {
                        Id = a.TransKey.SysKeyType.Id,
                        Name = a.TransKey.SysKeyType.Name
                    }
                },
                GetSysOrgranizeDetail = new GetSysOrgranizeDetail
                {
                    DepartmentCode = a.SysOrgranize.DepartmentCode,
                    DivisionCode = a.SysOrgranize.DivisionCode,
                    Id = a.SysOrgranizeId,
                    JobCode = a.SysOrgranize.JobCode,
                    Name = a.SysOrgranize.Name,
                    SectionCode = a.SysOrgranize.SectionCode,
                }
            }).FirstOrDefaultAsync();
        }

        #endregion

        #region | TransEquipment |

        public static async Task<List<GetTransEquipment>> ConvertToListAsync(this IQueryable<TransEquipment> query)
        {
            return await query.Select(a => new GetTransEquipment
            {
                Brand = a.Brand,
                EquipmentCode = a.EquipmentCode,
                Generation = a.Generation,
                Id = a.Id,
                InstallDate = a.InstallDate,
                InstallLocation = a.InstallLocation,
                IpAddress = a.IpAddress,
                MacAddress = a.MacAddress,
                MachineName = a.MachineName,
                MachineNumber = a.MachineName,
                MachineType = a.MachineType,
                Remark = a.Remark,
                TransKeyId = a.TransKeyId
            }).ToListAsync();
        }

        public static async Task<List<GetTransEquipment>> ConvertToListAsync(this IQueryable<TransEquipment> query, int pageSize, int pageNumber)
        {
            return await query.Select(a => new GetTransEquipment
            {
                Brand = a.Brand,
                EquipmentCode = a.EquipmentCode,
                Generation = a.Generation,
                Id = a.Id,
                InstallDate = a.InstallDate,
                InstallLocation = a.InstallLocation,
                IpAddress = a.IpAddress,
                MacAddress = a.MacAddress,
                MachineName = a.MachineName,
                MachineNumber = a.MachineName,
                MachineType = a.MachineType,
                Remark = a.Remark,
                TransKeyId = a.TransKeyId
            }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static async Task<GetTransEquipment> ConvertToFirstOrDefaultAsync(this IQueryable<TransEquipment> query)
        {
            return await query.Select(a => new GetTransEquipment
            {
                Brand = a.Brand,
                EquipmentCode = a.EquipmentCode,
                Generation = a.Generation,
                Id = a.Id,
                InstallDate = a.InstallDate,
                InstallLocation = a.InstallLocation,
                IpAddress = a.IpAddress,
                MacAddress = a.MacAddress,
                MachineName = a.MachineName,
                MachineNumber = a.MachineName,
                MachineType = a.MachineType,
                Remark = a.Remark,
                TransKeyId = a.TransKeyId
            }).FirstOrDefaultAsync();
        }

        #endregion
    }
}
