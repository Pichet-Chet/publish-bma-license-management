
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetCountAllocateResponse : ModelMessageAlertResponse
    {
        public GetCountAllocateResponseData Data { get; set; }
    }

    public class GetCountAllocateResponseData
    {
        public string? AllKeyStr { get; set; }
        public int AllKeyCount { get; set; }
        public decimal AllKeyPercentage { get; set; }

        public string? ReserveKeyStr { get; set; }
        public int ReserveKeyCount { get; set; }
        public decimal ReserveKeyPercentage { get; set; }
    }
}
