using bma_license_repository;
using bma_license_repository.CustomModel.TransEquipment;
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransKey;
using bma_license_repository.Response.Master;
using bma_license_repository.Response.TransKey;
using bma_license_service.Helper;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace bma_license_service
{
    public interface ITransKeyService
    {
        Task<GetCountByOrganizeResponse> GetCountByOrg(GetTransKeyFilterRequest param);
        Task<GetTransKeyFilterResponse> GetFilter(GetTransKeyFilterRequest param);
        Task<UseDropDownResponse> GetTransStatus();
        Task<GetCountDashboardResponse> GetCountDashboard();
        Task<MoveToReserveResponse> MoveToReserve(MoveToReserveRequest param);
        Task<MoveToNewResponse> MoveToNew(MoveToNewRequest param);
        Task<RemoveEquipmentResponse> RemoveEquipment(string transKeyId);

        Task<GetTransKeyFilterResponse> GetByName(string name);
        public Task<int> CountAllAsync(string cacheName);
        Task<GetTransKeyByOrgExportResponse> GetTransKeyByOrgExport(GetTransKeyByOrgExportRequest param);
        Task<GetCountTop10Response> GetCountTop10();
        Task<GetCountAllocateResponse> GetAllowCate();
        Task<GetAllocateOverviewResponse> GetAllocateOverview();
        Task<ImportDataKeyResponse> Import(IFormFile file, string createdBy);
    }


    public class TransKeyService : ITransKeyService
    {
        private readonly ITransKeyRepository _repository;
        private readonly ISysOrgranizeRepository _sysOrgranizeRepository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;
        private readonly IMasterRepository _masterRepository;
        private readonly ITransEquipmentRepository _transEquipmentRepository;
        private readonly ITransKeyHistoryResitory _transKeyHistoryResitory;
        private readonly DateTime _datetime;
        private readonly ISysKeyTypeRepository _sysKeyTypeRepository;
        public TransKeyService(ITransKeyRepository repository,
            ISysOrgranizeRepository sysOrgranizeRepository,
            ISysMessageConfigurationService sysMessageConfigurationService,
            IMasterRepository masterRepository,
            ITransEquipmentRepository transEquipment,
            ITransKeyHistoryResitory transKeyHistoryResitory,
            ISysKeyTypeRepository sysKeyTypeRepository)
        {
            _repository = repository;
            _sysOrgranizeRepository = sysOrgranizeRepository;
            _messageConfigurationService = sysMessageConfigurationService;
            _masterRepository = masterRepository;
            _transEquipmentRepository = transEquipment;
            _transKeyHistoryResitory = transKeyHistoryResitory;
            _datetime = DateTime.Now;
            _sysKeyTypeRepository = sysKeyTypeRepository;
        }


        public async Task<GetCountByOrganizeResponse> GetCountByOrg(GetTransKeyFilterRequest param)
        {
            GetCountByOrganizeResponse result = new GetCountByOrganizeResponse();

            GetCountByOrganizeResponseData data = new GetCountByOrganizeResponseData();

            try
            {
                var query = await _repository.GetFilter(param);

                if (query != null && query.Count > 0)
                {
                    data.CountLicense = query.Count();
                }
                else
                {
                    data.CountLicense = 0;
                }

                result.Data = data;
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<GetCountDashboardResponse> GetCountDashboard()
        {
            try
            {
                GetCountDashboardResponse result = new GetCountDashboardResponse();

                var list = await _repository.GetAll(true);
                var countAll = list.Count;
                var countUse = list.Where(a => a.GetTransEquipment != null && a.GetTransEquipment.Any()).Count();
                var countNotUse = list.Where(a => a.GetTransEquipment == null || !a.GetTransEquipment.Any()).Count();
                var countNotAllocate = list.Where(a => !a.SysOrgranizeId.HasValue).Count();
                var countAllocate = list.Where(a => a.SysOrgranizeId.HasValue).Count();

                result.Data = new GetCountDashboardResponseData
                {
                    CountAll = countAll,
                    CountUse = countUse,
                    CountNotUse = countNotUse,
                    AllPercentage = 100,
                    UsePercnetage = countUse != 0 ? Math.Truncate(((countUse.ToDecimal() / countAll.ToDecimal()) * 100) * 1_000_000) / 1_000_000 : 0,
                    NotUsePercnetage = countNotUse != 0 ? Math.Truncate(((countNotUse.ToDecimal() / countAll.ToDecimal()) * 100) * 1_000_000) / 1_000_000 : 0,
                    CountAllocate = countAllocate,
                    AllocatePercentage = countAllocate != 0 ? Math.Truncate(((countAllocate.ToDecimal() / countAll.ToDecimal()) * 100) * 1_000_000) / 1_000_000 : 0,
                    CountNotAllocate = countNotAllocate,
                    NotAllocatePercentage = countNotAllocate != 0 ? Math.Truncate(((countNotAllocate.ToDecimal() / countAll.ToDecimal()) * 100) * 1_000_000) / 1_000_000 : 0,
                };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetTransKeyFilterResponse> GetFilter(GetTransKeyFilterRequest param)
        {
            try
            {
                GetTransKeyFilterResponse result = new GetTransKeyFilterResponse();

                var data = await _repository.GetFilter(param);

                if (data != null && data.Any())
                {
                    var tempData = new List<GetTransKeyFilterResponseData>();
                    var sysOrgranize = await _sysOrgranizeRepository.GetAll(true);

                    foreach (var transKey in data)
                    {
                        var department = sysOrgranize.GetOrgranize(OrgranizeType.Department, transKey.GetSysOrgranize?.DepartmentCode);
                        var division = sysOrgranize.GetOrgranize(OrgranizeType.Division, transKey.GetSysOrgranize?.DepartmentCode, transKey.GetSysOrgranize?.DivisionCode);
                        var section = sysOrgranize.GetOrgranize(OrgranizeType.Section, transKey.GetSysOrgranize?.DepartmentCode, transKey.GetSysOrgranize?.DivisionCode, transKey.GetSysOrgranize?.SectionCode);
                        var job = sysOrgranize.GetOrgranize(OrgranizeType.Section, transKey.GetSysOrgranize?.DepartmentCode, transKey.GetSysOrgranize?.DivisionCode, transKey.GetSysOrgranize?.SectionCode, transKey.GetSysOrgranize?.JobCode);

                        var count = transKey.GetTransEquipment != null ? transKey.GetTransEquipment.Count : 0;
                        if (transKey.GetTransEquipment != null && transKey.GetTransEquipment.Any())
                        {
                            foreach (var equip in transKey.GetTransEquipment)
                            {
                                tempData.Add(new GetTransKeyFilterResponseData
                                {
                                    RecordCode = transKey.Id.ToString().Split('-')[0].ToUpper(),
                                    DepartmentCode = transKey.GetSysOrgranize?.DepartmentCode,
                                    DepartmentName = department?.Name,
                                    DivisionCode = transKey.GetSysOrgranize?.DivisionCode,
                                    DivisionName = division?.Name,
                                    Id = transKey.Id,
                                    CountInstall = $"{count} / 5",
                                    JobCode = transKey.GetSysOrgranize?.JobCode,
                                    JobName = job?.Name,
                                    License = transKey.License,
                                    Remark = transKey.Remark,
                                    SectionCode = transKey.GetSysOrgranize?.SectionCode,
                                    SectionName = section?.Name,
                                    LicenseType = transKey.GetSysKeyType.Name,
                                    Equipment = equip.EquipmentCode,
                                    InstallLocation = equip.InstallLocation,
                                    Brand = equip.Brand,
                                    EquipmentId = equip.Id,
                                    EquipmentRemark = equip.Remark,
                                    Generation = equip.Generation,
                                    InstallDate = equip.InstallDate.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                                    IpAddress = equip.IpAddress,
                                    MacAddress = equip.MacAddress,
                                    MachineName = equip.MachineName,
                                    MachineNumber = equip.MachineNumber,
                                    MachineType = equip.MachineType,
                                });
                            }
                        }
                        else
                        {
                            tempData.Add(new GetTransKeyFilterResponseData
                            {
                                RecordCode = transKey.Id.ToString().Split('-')[0].ToUpper(),
                                DepartmentCode = transKey.GetSysOrgranize?.DepartmentCode,
                                DepartmentName = department?.Name,
                                DivisionCode = transKey.GetSysOrgranize?.DivisionCode,
                                DivisionName = division?.Name,
                                Id = transKey.Id,
                                CountInstall = $"{count} / 5",
                                JobCode = transKey.GetSysOrgranize?.JobCode,
                                JobName = job?.Name,
                                License = transKey.License,
                                Remark = transKey.Remark,
                                SectionCode = transKey.GetSysOrgranize?.SectionCode,
                                SectionName = section?.Name,
                                LicenseType = transKey.GetSysKeyType.Name,
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(param.SortName) && !string.IsNullOrEmpty(param.SortType))
                    {
                        tempData = await SortingColumn(tempData, param.SortName, param.SortType);
                    }
                    result.Data = tempData.ManageData(param.IsAll, param.PageSize, param.PageNumber);
                }
                else
                {
                    result.Data = new List<GetTransKeyFilterResponseData>();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UseDropDownResponse> GetTransStatus()
        {
            try
            {
                UseDropDownResponse resul = new UseDropDownResponse();
                resul.Data = new List<UseDropDownResponseData>();

                resul.Data.Add(new UseDropDownResponseData { Id = Guid.Empty, Name = TransKeyStatus.TextAll, Value = TransKeyStatus.All });
                resul.Data.Add(new UseDropDownResponseData { Id = Guid.Empty, Name = TransKeyStatus.TextUse, Value = TransKeyStatus.Use });
                resul.Data.Add(new UseDropDownResponseData { Id = Guid.Empty, Name = TransKeyStatus.TextNotUse, Value = TransKeyStatus.NotUse });

                return resul;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MoveToNewResponse> MoveToNew(MoveToNewRequest param)
        {
            try
            {
                MoveToNewResponse result = new MoveToNewResponse();

                var data = await _repository.GetById(param.TransKeyId.ToGuid());
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                if (await _transEquipmentRepository.CheckUseEquipment(data.Id))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeCannotUpdate);
                    return result;
                }

                var findNewOrg = await _sysOrgranizeRepository.GetOrgranize(param.DepartmentCode, param.DivisionCode, param.SectionCode, param.JobCode);
                if (!ValidateService.Validate(findNewOrg))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var transKey = await _repository.GetForUpdate(data.Id);
                transKey.SysOrgranizeId = findNewOrg.Id;

                await _repository.UpdateAsync(transKey);

                TransKeyHistory transKeyHistory = new TransKeyHistory
                {
                    ActionBy = param.ActionBy.ToGuid(),
                    Id = HelperService.GenerateGuid(),
                    StartDate = _datetime,
                    SysOrgranizeId = findNewOrg.Id,
                    TransKeyId = data.Id,
                };

                await _transKeyHistoryResitory.InsertAsync(transKeyHistory);

                result.Data = new MoveToNewResponseData { Id = new Guid() };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MoveToReserveResponse> MoveToReserve(MoveToReserveRequest param)
        {
            try
            {
                MoveToReserveResponse result = new MoveToReserveResponse();

                var data = await _repository.GetById(param.TransKeyId.ToGuid());
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                if (await _transEquipmentRepository.CheckUseEquipment(data.Id))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeCannotUpdate);
                    return result;
                }

                var transKey = await _repository.GetForUpdate(data.Id);
                transKey.SysOrgranizeId = null;

                if (transKey.TransKeyHistories != null && transKey.TransKeyHistories.Any())
                {
                    foreach (var item in transKey.TransKeyHistories)
                    {
                        if (!item.EndDate.HasValue)
                        {
                            item.EndDate = _datetime;
                        }
                    }
                }

                await _repository.UpdateAsync();





                result.Data = new MoveToReserveResponseData { Id = new Guid() };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<RemoveEquipmentResponse> RemoveEquipment(string transKeyId)
        {
            try
            {
                RemoveEquipmentResponse result = new RemoveEquipmentResponse();

                var data = await _transEquipmentRepository.GetByTransKeyId(transKeyId.ToGuid());
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var listData = await _transEquipmentRepository.GetForUpdate(data.Select(a => a.Id).ToList());
                foreach (var item in listData)
                {
                    item.IsActive = false;
                    item.UpdatedDate = _datetime;
                }

                await _transEquipmentRepository.UpdateAsync();

                result.Data = new RemoveEquipmentResponseData { Id = transKeyId.ToGuid() };

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetTransKeyFilterResponse> GetByName(string name)
        {
            try
            {
                GetTransKeyFilterResponse result = new GetTransKeyFilterResponse();

                var data = await _repository.GetByName(name);
                if (data != null)
                {
                    result.Data = new List<GetTransKeyFilterResponseData>();
                    var sysOrgranize = await _sysOrgranizeRepository.GetAll(true);
                    var departmentList = sysOrgranize.Where(a => a.TypeName.ToUpper() == OrgranizeType.Department);
                    var divisionList = sysOrgranize.Where(a => a.TypeName.ToUpper() == OrgranizeType.Division);
                    var sectionList = sysOrgranize.Where(a => a.TypeName.ToUpper() == OrgranizeType.Section);
                    var jobList = sysOrgranize.Where(a => a.TypeName.ToUpper() == OrgranizeType.Job);

                    foreach (var transKey in data)
                    {
                        var department = departmentList.FirstOrDefault(a => a.DepartmentCode == transKey.GetSysOrgranize?.DepartmentCode);
                        var division = divisionList.FirstOrDefault(a => a.DepartmentCode == transKey.GetSysOrgranize?.DepartmentCode && a.DivisionCode == transKey.GetSysOrgranize?.DivisionCode);
                        var section = sectionList.FirstOrDefault(a => a.DepartmentCode == transKey.GetSysOrgranize?.DepartmentCode && a.DivisionCode == transKey.GetSysOrgranize?.DivisionCode && a.SectionCode == transKey.GetSysOrgranize?.SectionCode);
                        var job = jobList.FirstOrDefault(a => a.DepartmentCode == transKey.GetSysOrgranize?.DepartmentCode && a.DivisionCode == transKey.GetSysOrgranize?.DivisionCode && a.SectionCode == transKey.GetSysOrgranize?.SectionCode && a.JobCode == transKey.GetSysOrgranize?.JobCode);

                        var address = sysOrgranize.FirstOrDefault(a => a.DepartmentCode == transKey.GetSysOrgranize?.DepartmentCode
                                                            && a.DivisionCode == transKey.GetSysOrgranize?.DivisionCode
                                                            && a.SectionCode == transKey.GetSysOrgranize?.SectionCode
                                                            && a.JobCode == transKey.GetSysOrgranize?.JobCode);
                        var count = data.Where(a => a.GetTransEquipment != null).Count();

                        result.Data.Add(new GetTransKeyFilterResponseData
                        {
                            RecordCode = transKey.Id.ToString().Split('-')[0].ToUpper(),
                            DepartmentCode = transKey.GetSysOrgranize?.DepartmentCode,
                            DepartmentName = department?.Name,
                            DivisionCode = transKey.GetSysOrgranize?.DivisionCode,
                            DivisionName = division?.Name,
                            Id = transKey.Id,
                            CountInstall = $"{count} / 5",
                            JobCode = transKey.GetSysOrgranize?.JobCode,
                            JobName = job?.Name,
                            License = transKey.License,
                            Remark = transKey.Remark,
                            SectionCode = transKey.GetSysOrgranize?.SectionCode,
                            SectionName = section?.Name,
                            InstallLocation = address?.Name,
                            LicenseType = transKey.GetSysKeyType.Name,
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

        public async Task<int> CountAllAsync(string cacheName)
        {
            int result = 0;
            try
            {
                result = await _repository.CountAllAsync(cacheName);
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<GetTransKeyByOrgExportResponse> GetTransKeyByOrgExport(GetTransKeyByOrgExportRequest param)
        {
            try
            {
                GetTransKeyByOrgExportResponse result = new GetTransKeyByOrgExportResponse();
                var request = new GetTransKeyFilterRequest
                {
                    DepartmentCode = param.DepartmentCode,
                    DivisionCode = param.DevisionCode,
                    JobCode = param.JobCode,
                    SectionCode = param.SectionCode,
                    IsAll = true,
                };
                var data = await _repository.GetFilter(request);
                if (data != null && data.Count > 0)
                {
                    var sysOrgranize = await _sysOrgranizeRepository.GetAll(true);

                    List<dynamic> listData = new List<dynamic>();
                    List<string> listColumn = await GetHeaderAgency(param.IsAllColumn, param.ColumnName);

                    int countData = await _repository.GetCountAll();

                    int indexRow = 1;

                    foreach (var transKey in data)
                    {
                        var department = sysOrgranize.GetOrgranize(OrgranizeType.Department, transKey.GetSysOrgranize?.DepartmentCode);
                        var division = sysOrgranize.GetOrgranize(OrgranizeType.Division, transKey.GetSysOrgranize?.DepartmentCode, transKey.GetSysOrgranize?.DivisionCode);
                        var section = sysOrgranize.GetOrgranize(OrgranizeType.Section, transKey.GetSysOrgranize?.DepartmentCode, transKey.GetSysOrgranize?.DivisionCode, transKey.GetSysOrgranize?.SectionCode);
                        var job = sysOrgranize.GetOrgranize(OrgranizeType.Job, transKey.GetSysOrgranize?.DepartmentCode, transKey.GetSysOrgranize?.DivisionCode, transKey.GetSysOrgranize?.SectionCode, transKey.GetSysOrgranize?.JobCode);

                        var address = sysOrgranize.FirstOrDefault(a => a.DepartmentCode == transKey.GetSysOrgranize?.DepartmentCode
                                                            && a.DivisionCode == transKey.GetSysOrgranize?.DivisionCode
                                                            && a.SectionCode == transKey.GetSysOrgranize?.SectionCode
                                                            && a.JobCode == transKey.GetSysOrgranize?.JobCode);
                        var count = transKey.GetTransEquipment != null ? transKey.GetTransEquipment.Count : 0;

                        var dynamicProperties = new Dictionary<string, object>();
                        if (transKey.GetTransEquipment != null && transKey.GetTransEquipment.Any())
                        {
                            foreach (var equipment in transKey.GetTransEquipment)
                            {
                                dynamicProperties = await SetDynamicExport(transKey, equipment, param.IsAllColumn, listColumn, indexRow, count);
                                listData.Add(dynamicProperties);
                                indexRow++;
                            }
                        }
                        else
                        {
                            dynamicProperties = await SetDynamicExport(transKey, null, param.IsAllColumn, listColumn, indexRow, count);
                            listData.Add(dynamicProperties);
                            indexRow++;
                        }
                    }

                    result.Data = HelperService.GenerateExcelFile(
                            JsonConvert.SerializeObject(listData),
                            listColumn,
                            data.Count,
                            sysOrgranize.GetOrgranize(OrgranizeType.Department, param.DepartmentCode)?.Name,
                            sysOrgranize.GetOrgranize(OrgranizeType.Division, param.DepartmentCode, param.DevisionCode)?.Name,
                            sysOrgranize.GetOrgranize(OrgranizeType.Section, param.DepartmentCode, param.DevisionCode, param.SectionCode)?.Name,
                            sysOrgranize.GetOrgranize(OrgranizeType.Job, param.DepartmentCode, param.DevisionCode, param.SectionCode, param.JobCode)?.Name);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        private async Task<List<string>> GetHeaderAgency(bool isAll, List<string>? listColumnIndex)
        {
            List<string> result = new List<string>();
            result.Add("ลำดับ");
            if (isAll)
            {
                result.AddRange(await _masterRepository.GetColumnExportAgency());
            }
            else
            {
                if (listColumnIndex == null || !listColumnIndex.Any()) { return result; }
                foreach (var index in listColumnIndex)
                {
                    switch (index)
                    {
                        case ColumnExportAgency.AllocationAgency:
                            result.Add(ColumnExportAgency.AllocationAgency);
                            break;
                        case ColumnExportAgency.License:
                            result.Add(ColumnExportAgency.License);
                            break;
                        case ColumnExportAgency.LicenseType:
                            result.Add(ColumnExportAgency.LicenseType);
                            break;
                        case ColumnExportAgency.CountInstall:
                            result.Add(ColumnExportAgency.CountInstall);
                            break;
                        case ColumnExportAgency.MachineType:
                            result.Add(ColumnExportAgency.MachineType);
                            break;
                        case ColumnExportAgency.MachineNumber:
                            result.Add(ColumnExportAgency.MachineNumber);
                            break;
                        case ColumnExportAgency.MacAddress:
                            result.Add(ColumnExportAgency.MacAddress);
                            break;
                        case ColumnExportAgency.Equipment:
                            result.Add(ColumnExportAgency.Equipment);
                            break;
                        case ColumnExportAgency.MachineName:
                            result.Add(ColumnExportAgency.MachineName);
                            break;
                        case ColumnExportAgency.Brand:
                            result.Add(ColumnExportAgency.Brand);
                            break;
                        case ColumnExportAgency.Generation:
                            result.Add(ColumnExportAgency.Generation);
                            break;
                        case ColumnExportAgency.IpAddress:
                            result.Add(ColumnExportAgency.IpAddress);
                            break;
                        case ColumnExportAgency.InstallLocation:
                            result.Add(ColumnExportAgency.InstallLocation);
                            break;
                        case ColumnExportAgency.InstallDate:
                            result.Add(ColumnExportAgency.InstallDate);
                            break;
                        case ColumnExportAgency.Remark:
                            result.Add(ColumnExportAgency.Remark);
                            break;
                    }
                }
            }
            return result;
        }

        private async Task<Dictionary<string, object>> SetDynamicExport(GetTransKey transKey, GetTransEquipment transEquipment, bool isAllColumn, List<string> listColumn, int indexRow, int count)
        {
            var dynamicProperties = new Dictionary<string, object>();

            if (isAllColumn)
            {
                dynamicProperties["Index"] = indexRow;
                dynamicProperties[$"{ColumnExportAgency.AllocationAgency}"] = $"[{transKey.GetSysOrgranize?.DepartmentCode +
                                                                                    transKey.GetSysOrgranize?.DivisionCode +
                                                                                    transKey.GetSysOrgranize?.SectionCode +
                                                                                    transKey.GetSysOrgranize?.JobCode}] {transKey.GetSysOrgranize?.Name}";
                dynamicProperties[$"{ColumnExportAgency.License}"] = transKey.License;
                dynamicProperties[$"{ColumnExportAgency.LicenseType}"] = transKey.GetSysKeyType.Name;
                dynamicProperties[$"{ColumnExportAgency.CountInstall}"] = count;
                dynamicProperties[$"{ColumnExportAgency.MachineType}"] = transEquipment != null ? transEquipment.MachineType : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.MachineNumber}"] = transEquipment != null ? transEquipment.MachineNumber : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.MacAddress}"] = transEquipment != null ? transEquipment.MacAddress : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.Equipment}"] = transEquipment != null ? transEquipment.EquipmentCode : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.MachineName}"] = transEquipment != null ? transEquipment.MachineName : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.Brand}"] = transEquipment != null ? transEquipment.Brand : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.Generation}"] = transEquipment != null ? transEquipment.Generation : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.IpAddress}"] = transEquipment != null ? transEquipment.IpAddress : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.InstallLocation}"] = transEquipment != null ? transEquipment.InstallLocation : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.InstallDate}"] = transEquipment != null ? transEquipment.InstallDate.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss) : string.Empty;
                dynamicProperties[$"{ColumnExportAgency.Remark}"] = transKey.Remark;
            }
            else
            {
                if (listColumn != null && listColumn.Any())
                {
                    dynamicProperties["Index"] = indexRow;
                    foreach (var index in listColumn)
                    {
                        switch (index)
                        {
                            case ColumnExportAgency.AllocationAgency:
                                dynamicProperties[$"{ColumnExportAgency.AllocationAgency}"] = $"[{transKey.GetSysOrgranize?.DepartmentCode +
                                                                                    transKey.GetSysOrgranize?.DivisionCode +
                                                                                    transKey.GetSysOrgranize?.SectionCode +
                                                                                    transKey.GetSysOrgranize?.JobCode}] {transKey.GetSysOrgranize?.Name}";
                                break;
                            case ColumnExportAgency.License:
                                dynamicProperties[$"{ColumnExportAgency.License}"] = transKey.License;
                                break;
                            case ColumnExportAgency.LicenseType:
                                dynamicProperties[$"{ColumnExportAgency.LicenseType}"] = transKey.GetSysKeyType.Name;
                                break;
                            case ColumnExportAgency.CountInstall:
                                dynamicProperties[$"{ColumnExportAgency.CountInstall}"] = count;
                                break;
                            case ColumnExportAgency.MachineType:
                                dynamicProperties[$"{ColumnExportAgency.MachineType}"] = transEquipment != null ? transEquipment.MachineType : string.Empty;
                                break;
                            case ColumnExportAgency.MachineNumber:
                                dynamicProperties[$"{ColumnExportAgency.MachineNumber}"] = transEquipment != null ? transEquipment.MachineNumber : string.Empty;
                                break;
                            case ColumnExportAgency.MacAddress:
                                dynamicProperties[$"{ColumnExportAgency.MacAddress}"] = transEquipment != null ? transEquipment.MacAddress : string.Empty;
                                break;
                            case ColumnExportAgency.Equipment:
                                dynamicProperties[$"{ColumnExportAgency.Equipment}"] = transEquipment != null ? transEquipment.EquipmentCode : string.Empty;
                                break;
                            case ColumnExportAgency.MachineName:
                                dynamicProperties[$"{ColumnExportAgency.MachineName}"] = transEquipment != null ? transEquipment.MachineName : string.Empty;
                                break;
                            case ColumnExportAgency.Brand:
                                dynamicProperties[$"{ColumnExportAgency.Brand}"] = transEquipment != null ? transEquipment.Brand : string.Empty;
                                break;
                            case ColumnExportAgency.Generation:
                                dynamicProperties[$"{ColumnExportAgency.Generation}"] = transEquipment != null ? transEquipment.Generation : string.Empty;
                                break;
                            case ColumnExportAgency.IpAddress:
                                dynamicProperties[$"{ColumnExportAgency.IpAddress}"] = transEquipment != null ? transEquipment.IpAddress : string.Empty;
                                break;
                            case ColumnExportAgency.InstallLocation:
                                dynamicProperties[$"{ColumnExportAgency.InstallLocation}"] = transEquipment != null ? transEquipment.InstallLocation : string.Empty;
                                break;
                            case ColumnExportAgency.InstallDate:
                                dynamicProperties[$"{ColumnExportAgency.InstallDate}"] = transEquipment != null ? transEquipment.InstallDate.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss) : string.Empty;
                                break;
                            case ColumnExportAgency.Remark:
                                dynamicProperties[$"{ColumnExportAgency.Remark}"] = transKey.Remark;
                                break;
                        }
                    }
                }
            }

            return dynamicProperties;
        }

        public async Task<GetCountTop10Response> GetCountTop10()
        {
            try
            {
                GetCountTop10Response result = new GetCountTop10Response();
                var data = await _repository.GetTop10();
                if (data != null && data.TopOrgranizes != null && data.TopOrgranizes.Any())
                {
                    result.Data = new List<GetCountTop10ResponseData>();
                    foreach (var item in data.TopOrgranizes)
                    {
                        result.Data.Add(new GetCountTop10ResponseData
                        {
                            CountOrgnize = item.TransKeyCount,
                            OrgranizeName = item.OrgranizeName,
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

        public async Task<GetCountAllocateResponse> GetAllowCate()
        {
            try
            {
                GetCountAllocateResponse result = new GetCountAllocateResponse();

                var list = await _repository.GetAll(true);
                var countAll = list.Count;
                //var countNotUse = list.Where(a => a.GetTransEquipment == null || !a.GetTransEquipment.Any()).Count();

                var countAllocate = list.Where(a => a.SysOrgranizeId.HasValue).Count();
                var countNotAllocate = list.Where(a => !a.SysOrgranizeId.HasValue).Count();

                result.Data = new GetCountAllocateResponseData
                {
                    AllKeyCount = countAll,
                    AllKeyPercentage = countAllocate != 0 ? Math.Truncate(((countAllocate.ToDecimal() / countAll.ToDecimal()) * 100) * 1_000_000) / 1_000_000 : 0,
                    AllKeyStr = "จัดสรร",

                    ReserveKeyCount = countNotAllocate,
                    ReserveKeyPercentage = countNotAllocate != 0 ? Math.Truncate(((countNotAllocate.ToDecimal() / countAll.ToDecimal()) * 100) * 1_000_000) / 1_000_000 : 0,
                    ReserveKeyStr = "ยังไม่ได้จัดสรร"

                };

                return result;

            }
            catch
            {
                throw;
            }
        }

        public async Task<GetAllocateOverviewResponse> GetAllocateOverview()
        {
            try
            {
                GetAllocateOverviewResponse result = new GetAllocateOverviewResponse();

                result.Data = await _repository.GetAllocateOverview();

                return result;

            }
            catch
            {
                throw;
            }
        }

        public async Task<ImportDataKeyResponse> Import(IFormFile file, string createdBy)
        {
            try
            {
                ImportDataKeyResponse result = new ImportDataKeyResponse();

                if (file == null)
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var fileExtension = Path.GetExtension(file.FileName);
                if (!fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeExcelOnly);
                    return result;
                }

                string path = Directory.GetCurrentDirectory() + "/" + $"upload/import/{_datetime.ToDateTimeString(FormatDatetimeString.FormatyyyyMMddHHmmss)}";
                string pathUpload = file.UploadFile(path);
                if (!string.IsNullOrEmpty(pathUpload))
                {
                    int skip = 7;
                    var dtData = pathUpload.ReadToExcel(skip);
                    if (dtData == null)
                    {
                        result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeFileImportInvalid);
                        return result;
                    }
                    else if (dtData.Columns.Count != 16)
                    {
                        result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeFileImportInvalid);
                        return result;
                    }

                    for (int i = skip - 1; i < dtData.Rows.Count; i++)
                    {
                        var sysOrgranizeFullName = dtData.Rows[i][1].ToString();
                        var license = dtData.Rows[i][2].ToString();
                        var sysKeyTypeName = dtData.Rows[i][3].ToString();
                        var machineType = dtData.Rows[i][5].ToString();
                        var machineNumber = dtData.Rows[i][6].ToString();
                        var macAddress = dtData.Rows[i][7].ToString();
                        var equipmentCode = dtData.Rows[i][8].ToString();
                        var machineName = dtData.Rows[i][9].ToString();
                        var brand = dtData.Rows[i][10].ToString();
                        var generation = dtData.Rows[i][11].ToString();
                        var ipAddress = dtData.Rows[i][12].ToString();
                        var installLocation = dtData.Rows[i][13].ToString();
                        var installDate = dtData.Rows[i][14].ToString();
                        var remark = dtData.Rows[i][15].ToString();

                        var (departmentCode, divisionCode, sectionCode, jobCode) = sysOrgranizeFullName.GetCodeFromDataImport(true);
                        if (string.IsNullOrEmpty(departmentCode) || string.IsNullOrEmpty(divisionCode) || string.IsNullOrEmpty(sectionCode) || string.IsNullOrEmpty(jobCode))
                        {
                            result.MessageAlert = new bma_license_repository.Response.MessageAlert.MessageAlertResponse
                            {
                                TH = $"ข้อมูลในบรรทัดที่ {i + 1} ไม่สมบูรณ์"
                            };
                            return result;
                        }

                        var sysOrgranize = await _sysOrgranizeRepository.GetOrgranize(departmentCode, divisionCode, sectionCode, jobCode);
                        if (!ValidateService.Validate(sysOrgranize))
                        {
                            result.MessageAlert = new bma_license_repository.Response.MessageAlert.MessageAlertResponse
                            {
                                TH = $"ข้อมูลในบรรทัดที่ {i + 1} ไม่ถูกต้อง"
                            };
                            return result;
                        }

                        var sysKeyType = await _sysKeyTypeRepository.GetByName(sysKeyTypeName);
                        if (!ValidateService.Validate(sysKeyType))
                        {
                            result.MessageAlert = new bma_license_repository.Response.MessageAlert.MessageAlertResponse
                            {
                                TH = $"ข้อมูลในบรรทัดที่ {i + 1} ไม่สมบูรณ์"
                            };
                            return result;
                        }
                        var transKey = await UpsertTransKey(license, createdBy, sysKeyType.Id, sysOrgranize.Id);

                        if (!string.IsNullOrEmpty(equipmentCode))
                        {
                            var equipmentResult = await ProcessEquipment(transKey, equipmentCode, brand, generation, installDate, installLocation, ipAddress, macAddress, machineName, machineNumber, machineType, remark, createdBy);
                            if (!string.IsNullOrEmpty(equipmentResult))
                            {
                                result.MessageAlert = new bma_license_repository.Response.MessageAlert.MessageAlertResponse
                                {
                                    TH = $"ข้อมูลบรรทัดที่ {i + 1}: {equipmentResult}"
                                };
                                return result;
                            }
                        }
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        private async Task<TransKey> UpsertTransKey(string license, string createdBy, Guid sysKeyTypeId, Guid sysOrgranizeId)
        {
            var transKey = await _repository.GetForUpdate(license);

            if (transKey != null)
            {
                // อัปเดตข้อมูลที่มีอยู่
                transKey.UpdatedBy = createdBy.ToGuid();
                transKey.UpdatedDate = _datetime;
                transKey.IsActive = true;
                transKey.License = license;
                transKey.Remark = null;
                transKey.SysKeyTypeId = sysKeyTypeId;
                transKey.SysOrgranizeId = sysOrgranizeId;

                await _repository.UpdateAsync(transKey);
            }
            else
            {
                // สร้าง TransKey ใหม่
                transKey = new TransKey
                {
                    CreatedBy = createdBy.ToGuid(),
                    CreatedDate = _datetime,
                    Id = HelperService.GenerateGuid(),
                    IsActive = true,
                    License = license,
                    Remark = null,
                    SysKeyTypeId = sysKeyTypeId,
                    SysOrgranizeId = sysOrgranizeId,
                    UpdatedBy = createdBy.ToGuid(),
                    UpdatedDate = _datetime,
                };

                await _repository.InsertAsync(transKey);
            }

            return transKey;
        }

        private async Task<string> ProcessEquipment(TransKey transKey, string equipmentCode, string brand, string generation, string installDate, string installLocation, string ipAddress, string macAddress, string machineName, string machineNumber, string machineType, string remark, string createdBy)
        {
            // ดึงข้อมูล TransEquipment ที่เกี่ยวข้องกับ TransKey
            var listEquipmentId = transKey.TransEquipments?.Select(a => a.Id).ToList() ?? new List<Guid>();
            var listTransEquipment = await _transEquipmentRepository.GetForUpdate(listEquipmentId);
            var findTransEquipment = listTransEquipment.FirstOrDefault(e => e.EquipmentCode == equipmentCode);

            if (findTransEquipment != null)
            {
                // อัปเดตข้อมูล TransEquipment ที่มีอยู่
                findTransEquipment.Brand = brand;
                findTransEquipment.EquipmentCode = equipmentCode;
                findTransEquipment.Generation = generation;
                findTransEquipment.InstallDate = installDate.ToDateTimeNull();
                findTransEquipment.InstallLocation = installLocation;
                findTransEquipment.IpAddress = ipAddress;
                findTransEquipment.MacAddress = macAddress;
                findTransEquipment.MachineName = machineName;
                findTransEquipment.MachineNumber = machineNumber;
                findTransEquipment.MachineType = machineType;
                findTransEquipment.Remark = remark;
                findTransEquipment.UpdatedBy = createdBy.ToGuid();
                findTransEquipment.UpdatedDate = _datetime;

                await _transEquipmentRepository.UpdateAsync(findTransEquipment);
            }
            else
            {
                // ตรวจสอบข้อจำกัด (สูงสุด 5 รายการ)
                if (listTransEquipment.Count >= 5)
                {
                    return "รหัสครุภัณฑ์เกินกำหนด (สูงสุด 5 รายการ)";
                }

                // สร้าง TransEquipment ใหม่
                var transEquipment = new TransEquipment
                {
                    Brand = brand,
                    CreatedBy = createdBy.ToGuid(),
                    CreatedDate = _datetime,
                    EquipmentCode = equipmentCode,
                    Generation = generation,
                    Id = HelperService.GenerateGuid(),
                    InstallDate = installDate.ToDateTimeNull(),
                    InstallLocation = installLocation,
                    IpAddress = ipAddress,
                    IsActive = true,
                    MacAddress = macAddress,
                    MachineName = machineName,
                    MachineNumber = machineNumber,
                    MachineType = machineType,
                    Remark = remark,
                    TransKeyId = transKey.Id,
                    UpdatedBy = createdBy.ToGuid(),
                    UpdatedDate = _datetime,
                };

                await _transEquipmentRepository.InsertAsync(transEquipment);
            }

            return string.Empty;
        }

        private async Task<List<GetTransKeyFilterResponseData>> SortingColumn(List<GetTransKeyFilterResponseData> list, string sortName, string sortType)
        {
            var propertyInfo = typeof(GetTransKeyFilterResponseData).GetProperty(sortName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

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
