using System.Text.Json.Serialization;

namespace bma_license_repository.CustomModel.Middleware
{
    public class ResponseModel
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("Code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; } = string.Empty;

        [JsonPropertyName("innerException")]
        public string? InnerException { get; set; }

        [JsonPropertyName("pageNumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("rowCount")]
        public int? RowCount { get; set; }

        [JsonPropertyName("output")]
        public object? Output { get; set; }
    }
}
