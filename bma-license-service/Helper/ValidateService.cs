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
using bma_license_repository.Dto;

namespace bma_license_service.Helper
{
    public static class ValidateService
    {
        public static bool Validate(GetSysUserType sysUserType)
        {
            if (sysUserType == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysUserGroup sysUserGroup)
        {
            if (sysUserGroup == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysUserPermission sysUserPermission)
        {
            if (sysUserPermission == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysOrgranize sysOrgranize)
        {
            if (sysOrgranize == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysUser sysUser)
        {
            if (sysUser == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysKeyType sysKeyType)
        {
            if (sysKeyType == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(List<GetSysKeyType> sysKeyType)
        {
            if (sysKeyType == null || !sysKeyType.Any())
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysRepairCategory sysRepairCategory)
        {
            if (sysRepairCategory == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(List<GetSysRepairCategory> sysRepairCategory)
        {
            if (sysRepairCategory == null || !sysRepairCategory.Any())
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetSysJobRepairStatus sysJobRepairStatus)
        {
            if (sysJobRepairStatus == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(List<GetSysJobRepairStatus> sysJobRepairStatus)
        {
            if (sysJobRepairStatus == null || !sysJobRepairStatus.Any())
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetTransKey transKey)
        {
            if (transKey == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(List<GetTransKey> transKey)
        {
            if (transKey == null || !transKey.Any())
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetTransJobRepair transJobRepair)
        {
            if (transJobRepair == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(SysUser sysUser)
        {
            if (sysUser == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(List<TransKeyHistory> transKeyHistories)
        {
            if (transKeyHistories == null || !transKeyHistories.Any())
            {
                return false;
            }
            return true;
        }

        public static bool Validate(List<GetTransEquipment> transEquipment)
        {
            if (transEquipment == null || !transEquipment.Any())
            {
                return false;
            }
            return true;
        }

        public static bool Validate(GetTransEquipment transEquipment)
        {
            if (transEquipment == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(TransEquipment transEquipment)
        {
            if (transEquipment == null)
            {
                return false;
            }
            return true;
        }

        public static bool Validate(TransJobRepair transJobRepair)
        {
            if (transJobRepair == null)
            {
                return false;
            }
            return true;
        }
    }
}
