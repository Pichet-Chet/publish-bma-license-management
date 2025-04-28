
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class RemoveEquipmentResponse : ModelMessageAlertResponse
    {
        public RemoveEquipmentResponseData Data { get; set; }
    }

    public class RemoveEquipmentResponseData
    {
        public Guid Id { get; set; }
    }
}
