
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class UpdateEquipmentResponse : ModelMessageAlertResponse
    {
        public UpdateEquipmentResponseData Data { get; set; }
    }

    public class UpdateEquipmentResponseData
    {
        public Guid Id { get; set; }
    }
}
