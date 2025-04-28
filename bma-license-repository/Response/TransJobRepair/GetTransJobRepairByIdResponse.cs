

using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransJobRepair
{
    public class GetTransJobRepairByIdResponse : ModelMessageAlertResponse
    {
        public GetTransJobRepairByIdResponseData Data { get; set; }
    }

    public class GetTransJobRepairByIdResponseData
    {
        public Guid Id { get; set; }
        public string DateOfRequest { get; set; }
        public string DateOfFixed { get; set; }
        public string License { get; set; }
        public Guid LicenseId { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public string RequestName { get; set; }
        public string Telephone { get; set; }
        public string Remark { get; set; }
        public string StatusName { get; set; }
        public Guid StatusId { get; set; }
        public string FixedDescription { get; set; }
        public string DateOfStart {  get; set; }

    }
}
