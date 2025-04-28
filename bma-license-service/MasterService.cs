

using bma_license_repository;
using bma_license_repository.Helper;
using bma_license_repository.Response.Master;
using bma_license_service.Helper;

namespace bma_license_service
{
    public interface IMasterService
    {
        Task<GetColumnExportAgencyResponse> GetColumnAgency();
        Task<UseDropDownResponse> GetSysRepairCategory();
    }
    public class MasterService : IMasterService
    {
        private readonly IMasterRepository _repository;
        private readonly ISysRepairCategoryRepository _sysRepairCategoryRepository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;
        public MasterService(IMasterRepository repository, ISysRepairCategoryRepository sysRepairCategoryRepository, ISysMessageConfigurationService messageConfigurationService)
        {
            _repository = repository;
            _sysRepairCategoryRepository = sysRepairCategoryRepository;
            _messageConfigurationService = messageConfigurationService;
        }

        public async Task<GetColumnExportAgencyResponse> GetColumnAgency()
        {
            try
            {
                GetColumnExportAgencyResponse result = new GetColumnExportAgencyResponse();
                result.Data = new List<GetColumnExportAgencyResponseData>();
                var data = await _repository.GetColumnExportAgency();

                foreach (var row in data)
                {
                    result.Data.Add(new GetColumnExportAgencyResponseData { Column = row });
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UseDropDownResponse> GetSysRepairCategory()
        {
            try
            {
                UseDropDownResponse result = new UseDropDownResponse();

                var data = await _sysRepairCategoryRepository.GetAll(true);
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                result.Data = new List<UseDropDownResponseData>();
                foreach (var item in data)
                {
                    result.Data.Add(new UseDropDownResponseData
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Value = item.Id.ToString()
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
