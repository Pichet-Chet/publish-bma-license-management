
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKeyHistory
{
    public class CreateTransKeyHistoryResponse : ModelMessageAlertResponse
    {
        public CreateTransKeyHistoryResponseData Data { get; set; }
    }

    public class CreateTransKeyHistoryResponseData
    {
        public Guid Id { get; set; }
    }
}
