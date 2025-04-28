using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransJobRepair
{
    public class GetTransJobRepairResponse : ModelMessageAlertResponse
    {
        public List<GetTransJobRepairResponseData> Data { get; set; }
    }

    public class GetTransJobRepairResponseData
    {
        public string recordCode { get; set; }
        public Guid Id { get; set; }
        public string DateOfRequestText { get; set; }
        public DateTime? DateOfRequest { get; set; }
        public string DateOfFixedText { get; set; }
        public DateTime? DateOfFixed { get; set; }
        public string DateOfStartText {  get; set; }
        public DateTime? DateOfStart {  get; set; }
        public string License { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId {  get; set; }
        public Guid TransKeyId { get; set; }
        public string Description { get; set; }
        public string RequestName { get; set; }
        public string Telephone { get; set; }
        public string Remark { get; set; }
        public string StatusName { get; set; }
        public string FixedDescription { get; set; }
        public int Seq {  get; set; }
        public Guid? EquipmentId { get; set; }
    }
}
