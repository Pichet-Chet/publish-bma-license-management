
namespace bma_license_repository.Helper
{
    public class ConstantsMessage
    {
        public static class RoleConfig
        {
            public static string RoleAdmin = "admin";
        }
    }

    public static class ConfigMessage
    {
        public static string CodeDataNotFound = "DATA_NOT_FOUND";
        public static string CodeSaveSuccess = "SAVE_SUCCESS";
        public static string CodeCannotUpdate = "CAN_NOT_UPDATE";
        public static string CodeExcelOnly = "EXCEL_ONLY";
        public static string CodeFileImportInvalid = "FILE_IMPORT_INVALID";
    }

    public static class FormatDatetimeString
    {
        public static string FormatyyyyMMdd_HHmmss = "yyyyMMdd HHmmss";
        public static string FormatyyyySlashMMSlashdd_HHmmss = "yyyy/MM/dd HH:mm:ss";
        public static string FormatyyyyMMddHHmmss = "yyyyMMddHHmmss";
    }

    public static class OrgranizeType
    {
        public const string Department = "DEPARTMENT";
        public const string Division = "DIVISION";
        public const string Job = "JOB";
        public const string Section = "SECTION";
        public const string Empty = "00";
    }

    public static class TransKeyStatus
    {
        public const string Use = "U";
        public const string NotUse = "NU";
        public const string All = "A";
        public const string TextUse = "กำลังใช้งาน";
        public const string TextNotUse = "ไม่ได้ใช้งาน";
        public const string TextAll = "ทั้งหมด";
    }

    public static class ColumnExportAgency
    {
        public const string AllocationAgency = "หน่วยงานที่จัดสรร";
        public const string License = "คีย์ลิขสิทธิ์";
        public const string LicenseType = "ประเภทลิขสิทธิ์";
        public const string CountInstall = "จำนวนติดตั้ง";
        public const string MachineType = "ประเภทเครื่อง";
        public const string MachineNumber = "หมายเลขเครื่อง";
        public const string MacAddress = "MAC Address";
        public const string Equipment = "รหัสครุภัณฑ์";
        public const string MachineName = "ชื่อเครื่อง";
        public const string Brand = "ยี่ห้อ";
        public const string Generation = "รุ่น";
        public const string IpAddress = "IP Address";
        public const string InstallLocation = "สถานที่ติดตั้ง";
        public const string InstallDate = "วันที่ติดตั้ง";
        public const string Remark = "หมายเหตุ";
    }

    public static class ExportType
    {
        public const string Agency = "agency";
    }

    public static class PermissionTranskeyPage
    {
        public const string Admin = "a";
    }

    public static class SortType
    {
        public static string ASC = "asc";
        public static string DESC = "desc";
    }

    public static class ColumnExportJobRepair
    {
        public const string Account = "บัญชีใช้งาน";
        public const string EquipmentCode = "หมายเลขครุภัณฑ์";
        public const string Department = "หน่วยงาน";
        public const string RequestName = "เจ้าหน้าที่หน่วยที่แจ้ง";
        public const string Telephone = "เบอร์ติดต่อ";
        public const string Problam = "ปัญหา";
        public const string Detail = "รายละเอียด";
        public const string FixDetail = "วิธีแก้แก้ไข";
        public const string CreateDate = "วันที่และเวลา";
        public const string Status = "ผลการแก้ไข";
    }

}
