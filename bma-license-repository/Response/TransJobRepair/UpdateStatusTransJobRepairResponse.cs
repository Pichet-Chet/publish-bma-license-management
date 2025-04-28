
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransJobRepair
{
    public class UpdateStatusTransJobRepairResponse : ModelMessageAlertResponse
    {
        public UpdateStatusTransJobRepairResponseData Data { get; set; }
    }

    public class UpdateStatusTransJobRepairResponseData
    {
        public Guid Id { get; set; }
    }
}
