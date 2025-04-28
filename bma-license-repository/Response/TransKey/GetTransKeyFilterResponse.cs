using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetTransKeyFilterResponse : ModelMessageAlertResponse
    {
        public List<GetTransKeyFilterResponseData> Data { get; set; }
    }

    public class GetTransKeyFilterResponseData
    {
        public string RecordCode { get; set; }
        public Guid Id { get; set; }
        public string License { get; set; }
        public string LicenseType { get; set; }
        public string CountInstall { get; set; }
        public string InstallLocation { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string Remark { get; set; }
        public string Equipment { get; set; }
        public string MachineName { get; set; }
        public string MachineNumber { get; set; }
        public string MachineType { get; set; }
        public string MacAddress { get; set; }
        public string Brand { get; set; }
        public string Generation { get; set; }
        public string IpAddress {  get; set; }
        public string InstallDate { get;set; }
        public string EquipmentRemark {  get; set; }
        public Guid EquipmentId { get; set; }
    }

}
