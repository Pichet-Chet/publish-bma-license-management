using bma_license_repository;
using bma_license_repository.Helper;
using bma_license_repository.Response.SysKeyType;
using bma_license_service.Helper;

namespace bma_license_service
{
    public interface ISysKeyTypeService
    {
        Task<GetSysKeyTypeResponse> GetSysKeyType();
    }
    public class SysKeyTypeService : ISysKeyTypeService
    {
        private readonly ISysKeyTypeRepository _repository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;

        public SysKeyTypeService(ISysKeyTypeRepository repository, ISysMessageConfigurationService sysMessageConfigurationService)
        {
            _repository = repository;
            _messageConfigurationService = sysMessageConfigurationService;
        }

        public async Task<GetSysKeyTypeResponse> GetSysKeyType()
        {
            try
            {
                GetSysKeyTypeResponse result = new GetSysKeyTypeResponse();
                var data = await _repository.GetAll(true);
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                result.Data = new List<GetSysKeyTypeResponseData>();
                foreach (var item in data)
                {
                    result.Data.Add(new GetSysKeyTypeResponseData
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Name = item.Name,
                    });
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}
