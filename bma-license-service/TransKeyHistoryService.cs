
using bma_license_repository;
using bma_license_repository.CustomModel.TransKeyHistory;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransKeyHistory;
using bma_license_repository.Response.TransJobRepair;
using bma_license_repository.Response.TransKeyHistory;
using bma_license_service.Helper;
using System.Reflection;

namespace bma_license_service
{
    public interface ITransKeyHistoryService
    {
        Task<GetTransKeyHistoryFilterResponse> GetFilter(GetTransKeyHistoryFilterRequest param);
        Task<CreatedTransJobRepairResponse> Create(CreatedTransKeyHistoryRequest param);
        public Task<int> CountAllAsync();
    }

    public class TransKeyHistoryService : ITransKeyHistoryService
    {
        private readonly ITransKeyHistoryResitory _repository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;
        private readonly ISysOrgranizeRepository _sysOrgranizeRepository;

        public TransKeyHistoryService(ITransKeyHistoryResitory repository, ISysOrgranizeRepository sysOrgranizeRepository, ISysMessageConfigurationService sysMessageConfigurationService)
        {
            _repository = repository;
            _sysOrgranizeRepository = sysOrgranizeRepository;
            _messageConfigurationService = sysMessageConfigurationService;
        }

        public async Task<CreatedTransJobRepairResponse> Create(CreatedTransKeyHistoryRequest param)
        {
            try
            {
                CreatedTransJobRepairResponse result = new CreatedTransJobRepairResponse();

                TransKeyHistory transKeyHistory = new TransKeyHistory
                {
                    ActionBy = param.ActionBy.ToGuid(),
                    EndDate = param.EndDate,
                    Id = HelperService.GenerateGuid(),
                    Remark = param.Remark,
                    StartDate = param.StartDate,
                    SysOrgranizeId = param.SysOrgranizeId.ToGuid(),
                    TransKeyId = param.TransKeyId.ToGuid(),
                };

                await _repository.InsertAsync(transKeyHistory);

                result.Data = new CreatedTransJobRepairResponseData
                {
                    Id = transKeyHistory.Id
                };

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetTransKeyHistoryFilterResponse> GetFilter(GetTransKeyHistoryFilterRequest param)
        {
            try
            {
                GetTransKeyHistoryFilterResponse result = new GetTransKeyHistoryFilterResponse();

                var data = await _repository.GetFilter(param);
                if (data != null && data.Any())
                {
                    var tempData = new List<GetTransKeyHistoryFilterResponseData>();

                    var sysOrgranize = await _sysOrgranizeRepository.GetAll(true);

                    foreach (var item in data)
                    {
                        var department = sysOrgranize.GetOrgranize(OrgranizeType.Department, item.GetSysOrgranizeDetail?.DepartmentCode);
                        var division = sysOrgranize.GetOrgranize(OrgranizeType.Division, item.GetSysOrgranizeDetail?.DepartmentCode, item.GetSysOrgranizeDetail?.DivisionCode);
                        var section = sysOrgranize.GetOrgranize(OrgranizeType.Section, item.GetSysOrgranizeDetail?.DepartmentCode, item.GetSysOrgranizeDetail?.DivisionCode, item.GetSysOrgranizeDetail?.SectionCode);
                        var job = sysOrgranize.GetOrgranize(OrgranizeType.Job, item.GetSysOrgranizeDetail?.DepartmentCode, item.GetSysOrgranizeDetail?.DivisionCode, item.GetSysOrgranizeDetail?.SectionCode, item.GetSysOrgranizeDetail?.JobCode);

                        tempData.Add(new GetTransKeyHistoryFilterResponseData
                        {
                            ActionBy = item.ActionBy,
                            Name = item.TransKeyDetail.Name,
                            DepartmentCode = item.GetSysOrgranizeDetail.DepartmentCode,
                            DepartmentName = department?.Name,
                            DivisionCode = item.GetSysOrgranizeDetail.DivisionCode,
                            DivisionName = division?.Name,
                            EndDate = item.EndDate.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                            Id = item.Id,
                            JobCode = item.GetSysOrgranizeDetail.JobCode,
                            JobName = job?.Name,
                            KeyType = item.TransKeyDetail.GetSysKeyTypeDetail.Name,
                            SectionCode = item.GetSysOrgranizeDetail.SectionCode,
                            SectionName = section?.Name,
                            StartDate = item.StartDate.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss)
                        });
                    }

                    if (!string.IsNullOrEmpty(param.SortName) && !string.IsNullOrEmpty(param.SortType))
                    {
                        tempData = await SortingColumn(tempData, param.SortName, param.SortType);
                    }
                    result.Data = tempData.ManageData(param.IsAll, param.PageSize, param.PageNumber);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> CountAllAsync()
        {
            int result = 0;
            try
            {
                result = await _repository.CountAllAsync();
            }
            catch
            {
                throw;
            }

            return result;
        }

        private async Task<List<GetTransKeyHistoryFilterResponseData>> SortingColumn(List<GetTransKeyHistoryFilterResponseData> list, string sortName, string sortType)
        {
            var propertyInfo = typeof(GetTransKeyHistoryFilterResponseData).GetProperty(sortName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                return list;
            }

            if (sortType.Contains(SortType.DESC, StringComparison.OrdinalIgnoreCase))
            {
                return list.OrderByDescending(x => propertyInfo.GetValue(x)).ToList();
            }
            else
            {
                return list.OrderBy(x => propertyInfo.GetValue(x)).ToList();
            }
        }
    }
}
