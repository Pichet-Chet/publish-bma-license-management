using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransJobRepair
{
    public class UpdateTransJobRepairResponse : ModelMessageAlertResponse
    {
        public UpdateTransJobRepairResponseData Data { get; set; }
    }

    public class UpdateTransJobRepairResponseData
    {
        public Guid Id { get; set; }
    }
}
