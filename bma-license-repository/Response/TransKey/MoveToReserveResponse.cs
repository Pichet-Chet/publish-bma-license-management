
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class MoveToReserveResponse : ModelMessageAlertResponse
    {
        public MoveToReserveResponseData Data { get; set; }
    }

    public class MoveToReserveResponseData
    {
        public Guid Id { get; set; }
    }
}
