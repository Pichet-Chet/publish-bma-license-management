
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransEquipment
{
    public class CreatedTransEquipmentResponse : ModelMessageAlertResponse
    {
        public CreatedTransEquipmentResponseData Data { get; set; }
    }

    public class CreatedTransEquipmentResponseData
    {
        public Guid Id { get; set; }
    }
}
