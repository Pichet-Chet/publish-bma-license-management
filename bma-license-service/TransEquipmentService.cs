
using bma_license_repository;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransEquipment;
using bma_license_repository.Request.TransKey;
using bma_license_repository.Response.TransEquipment;
using bma_license_repository.Response.TransKey;
using bma_license_service.Helper;

namespace bma_license_service
{
    public interface ITransEquipmentService
    {
        Task<CreatedTransEquipmentResponse> Create(CreatedTransEquipmentRequest param);
        Task<UpdateEquipmentResponse> Update(UpdateEquipmentRequest param);
    }

    public class TransEquipmentService : ITransEquipmentService
    {
        private readonly ITransEquipmentRepository _repository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;
        private readonly DateTime _datetime;
        private readonly ITransKeyRepository _keyRepository;

        public TransEquipmentService(ISysMessageConfigurationService messageConfigurationService,
                ITransEquipmentRepository transEquipmentRepository,
                ITransKeyRepository transKeyRepository)
        {
            _messageConfigurationService = messageConfigurationService;
            _repository = transEquipmentRepository;
            _datetime = DateTime.Now;
            _keyRepository = transKeyRepository;
        }

        public async Task<CreatedTransEquipmentResponse> Create(CreatedTransEquipmentRequest param)
        {
            try
            {
                CreatedTransEquipmentResponse result = new CreatedTransEquipmentResponse();

                var findTransKey = await _keyRepository.GetById(param.TransKeyId.ToGuid());
                if (!ValidateService.Validate(findTransKey))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }


                TransEquipment transEquipment = new TransEquipment
                {
                    Brand = param.Brand,
                    CreatedBy = param.CreatedBy.ToGuid(),
                    CreatedDate = _datetime,
                    EquipmentCode = param.EquipmentCode,
                    Generation = param.Generation,
                    Id = HelperService.GenerateGuid(),
                    InstallDate = param.InstallDate,
                    InstallLocation = param.InstallLocation,
                    IpAddress = param.IpAddress,
                    IsActive = true,
                    MacAddress = param.MacAddress,
                    MachineName = param.MachineName,
                    MachineType = param.MachineType,
                    MachineNumber = param.MachineNumber,
                    Remark = param.Remark,
                    TransKeyId = param.TransKeyId.ToGuid(),
                    UpdatedBy = param.CreatedBy.ToGuid(),
                    UpdatedDate = _datetime,
                };

                await _repository.InsertAsync(transEquipment);
                result.Data = new CreatedTransEquipmentResponseData { Id = transEquipment.Id };

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UpdateEquipmentResponse> Update(UpdateEquipmentRequest param)
        {
            try
            {
                UpdateEquipmentResponse result = new UpdateEquipmentResponse();

                var data = await _repository.GetForUpdate(param.Id.ToGuid());
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var findTransKey = await _keyRepository.GetById(param.TransKeyId.ToGuid());
                if (!ValidateService.Validate(findTransKey))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                data.Brand = param.Brand;
                data.MachineNumber = param.MachineNumber;
                data.EquipmentCode = param.EquipmentCode;
                data.Generation = param.Generation;
                data.InstallDate = param.InstallDate;
                data.InstallLocation = param.InstallLocation;
                data.IpAddress = param.IpAddress;
                data.MacAddress = param.MacAddress;
                data.MachineName = param.MachineName;
                data.MachineType = param.MachineType;
                data.Remark = param.Remark;
                data.TransKeyId = param.TransKeyId.ToGuid();
                data.UpdatedBy = param.UpdateBy.ToGuid();
                data.UpdatedDate = _datetime;

                await _repository.UpdateAsync();

                result.Data = new UpdateEquipmentResponseData { Id = param.Id.ToGuid() };

                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
