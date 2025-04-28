using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.MigrateData
{
    public class MigrateDataResponse
    {
        public MigrateDataResponse()
        {
            MessageAlert = new MessageAlertResponse();
            Data = new List<MigrateDataResponseData>();
            Logging = new List<Logging>();
        }

        public MessageAlertResponse MessageAlert { get; set; }

        public List<MigrateDataResponseData> Data { get; set; }

        public List<Logging> Logging { get; set; }
    }

    public class MigrateDataResponseData
    {
        public Guid Id { get; set; }
    }


    public class Logging
    {
        public int Row { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }
    }

}

