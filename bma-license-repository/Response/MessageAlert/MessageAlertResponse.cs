
namespace bma_license_repository.Response.MessageAlert
{
    public class MessageAlertResponse
    {
        public MessageAlertResponse()
        {
            TH = string.Empty;
            EN = string.Empty;
        }

        public string? TH { get; set; }
        public string? EN { get; set; }
    }

    public class ModelMessageAlertResponse
    {
        public MessageAlertResponse MessageAlert { get; set; }
    }
}
