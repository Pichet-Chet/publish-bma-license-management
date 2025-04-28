

using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKeyHistory
{
    public class GetTransKeyHistoryFilterResponse : ModelMessageAlertResponse
    {
        public List<GetTransKeyHistoryFilterResponseData> Data { get; set; }
    }

    public class GetTransKeyHistoryFilterResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Guid ActionBy { get; set; }
        public string ActionName { get; set; }
        public string KeyType { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }

    }
}
