using bma_license_repository.CustomModel;
using bma_license_repository.Response.MessageAlert;

namespace bma_license_service
{
    public interface ISysMessageConfigurationService
    {
        Task<MessageAlertResponse> SetMessageAlert(string code);
    }
    public class SysMessageConfigurationService : ISysMessageConfigurationService
    {
        private readonly ISysMessageConfigurationRepository _repository;

        public SysMessageConfigurationService(ISysMessageConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<MessageAlertResponse> SetMessageAlert(string code)
        {
            try
            {
                var data = await _repository.GetByCode(code);
                if (data == null)
                {
                    throw new Exception("Message Not Set");
                }
                else
                {
                    return new MessageAlertResponse
                    {
                        EN = data.MessageEN,
                        TH = data.MessageTh,
                    };
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
