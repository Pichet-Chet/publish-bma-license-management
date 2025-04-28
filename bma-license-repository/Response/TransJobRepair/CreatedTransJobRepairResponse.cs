
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransJobRepair
{
    public class CreatedTransJobRepairResponse : ModelMessageAlertResponse
    {
        public CreatedTransJobRepairResponseData Data { get; set; }

    }
    public class CreatedTransJobRepairResponseData
    {
        public Guid Id { get; set; }
    }
}
