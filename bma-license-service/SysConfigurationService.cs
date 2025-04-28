
using bma_license_repository;
using bma_license_repository.Request.SysConfiguration;
using bma_license_repository.Response.SysConfiguration;

namespace bma_license_service
{
    public interface ISysConfigurationService
    {
        Task<GetSysConfigurationFilterResponse> GetFilter(GeSysConfigurationtFilterRequest request);
    }

    public class SysConfigurationService : ISysConfigurationService
    {
        private readonly ISysConfigurationRepository _repository;

        public SysConfigurationService(ISysConfigurationRepository sysConfigurationRepository)
        {
            _repository = sysConfigurationRepository;
        }

        public async Task<GetSysConfigurationFilterResponse> GetFilter(GeSysConfigurationtFilterRequest request)
        {
            try
            {
                GetSysConfigurationFilterResponse result = new GetSysConfigurationFilterResponse();
                var data = await _repository.GetFilter(request);
                if (data != null && data.Any())
                {
                    result.Data = new List<GetFilterResponseData>();
                    foreach (var item in data)
                    {
                        result.Data.Add(new GetFilterResponseData
                        {
                            Description = item.Description,
                            Id = item.Id,
                            IsActive = item.IsActive,
                            Key = item.Key,
                            Value = item.Value
                        });
                    }
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
