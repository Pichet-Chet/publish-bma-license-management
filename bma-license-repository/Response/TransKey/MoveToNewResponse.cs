
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class MoveToNewResponse : ModelMessageAlertResponse
    {
        public MoveToNewResponseData Data { get; set; }
    }

    public class MoveToNewResponseData
    {
        public Guid Id { get; set; }
    }
}
