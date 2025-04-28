using bma_license_repository;
using bma_license_repository.CustomModel.TransEquipment;
using bma_license_repository.CustomModel.TransJobRepair;
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransJobRepair;
using bma_license_repository.Response.TransJobRepair;
using bma_license_service.Helper;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Reflection;

namespace bma_license_service
{
    public interface ITransJobRepirService
    {
        Task<GetTransJobRepairResponse> GetFilter(GetTransJobRepairFilterRequest param);
        Task<CreatedTransJobRepairResponse> Create(CreatedTransJobRepairRequest param);
        Task<UpdateTransJobRepairResponse> Update(UpdateTransJobRepairRequest param);
        Task<GetTransJobRepairByIdResponse> GetById(string id);
        Task<UpdateStatusTransJobRepairResponse> UpdateStatus(UpdateStatusTransJobRepairRequest param);
        public Task<int> CountAllAsync();
        Task<ExportTransJobRepairResponse> Export(string month, string year);
    }

    public class TransJobRepirService : ITransJobRepirService
    {
        private readonly ITransJobRepairRepository _repository;
        private readonly DateTime _datetime;
        private readonly ISysJobRepairStatusRepository _statusRepository;
        private readonly ISysRepairCategoryRepository _categoryRepository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;
        private readonly ITransKeyRepository _keyRepository;
        private readonly ISysOrgranizeRepository _orgranizeRepository;

        public TransJobRepirService(ITransJobRepairRepository repository,
            ISysJobRepairStatusRepository sysJobRepairStatusRepository,
            ISysRepairCategoryRepository sysRepairCategoryRepository,
            ISysMessageConfigurationService sysMessageConfigurationService,
            ITransKeyRepository transKeyRepository,
            ISysOrgranizeRepository sysOrgranizeRepository)
        {
            _repository = repository;
            _datetime = DateTime.Now;
            _statusRepository = sysJobRepairStatusRepository;
            _categoryRepository = sysRepairCategoryRepository;
            _messageConfigurationService = sysMessageConfigurationService;
            _keyRepository = transKeyRepository;
            _orgranizeRepository = sysOrgranizeRepository;
        }

        public async Task<CreatedTransJobRepairResponse> Create(CreatedTransJobRepairRequest param)
        {
            try
            {
                CreatedTransJobRepairResponse result = new CreatedTransJobRepairResponse();

                var findStatus = await _statusRepository.GetBySeq(1);
                if (!ValidateService.Validate(findStatus))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }
                var findCategory = await _categoryRepository.GetById(param.CategoryId.ToGuid());
                if (!ValidateService.Validate(findCategory))
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

                TransJobRepair transjob = new TransJobRepair
                {
                    CreatedBy = param.CreatedBy.ToGuid(),
                    CreatedDate = _datetime,
                    DateOfRequest = _datetime,
                    Description = param.Description,
                    Id = HelperService.GenerateGuid(),
                    IsActive = true,
                    Name = param.Name,
                    Remark = param.Remark,
                    SysJobRepairStatusId = findStatus.Id,
                    SysRepairCategoryId = findCategory.Id,
                    Telephone = param.Telephone,
                    TransKeyId = findTransKey.Id,
                    UpdatedBy = param.CreatedBy.ToGuid(),
                    UpdatedDate = _datetime,
                    TransEquipmentId = param.EquipmentId.ToGuid(),
                };

                await _repository.InsertAsync(transjob);
                result.Data = new CreatedTransJobRepairResponseData { Id = transjob.Id };

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetTransJobRepairByIdResponse> GetById(string id)
        {
            try
            {
                GetTransJobRepairByIdResponse result = new GetTransJobRepairByIdResponse();

                var data = await _repository.GetById(id.ToGuid());
                if (ValidateService.Validate(data))
                {
                    result.Data = new GetTransJobRepairByIdResponseData
                    {
                        CategoryName = data.ISysRepairCategory.Name,
                        DateOfFixed = data.DateOfFixed.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                        DateOfRequest = data.DateOfRequest.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                        Description = data.Description,
                        FixedDescription = data.FixedDescription,
                        Id = data.Id,
                        License = data.ITransKey.License,
                        Remark = data.Remark,
                        RequestName = data.Name,
                        StatusName = data.ISysJobRepairStatus.Name,
                        Telephone = data.Telephone,
                        CategoryId = data.SysRepairCategoryId,
                        DateOfStart = data.DateOfStart.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                        LicenseId = data.TransKeyId,
                        StatusId = data.SysJobRepairStatusId,
                    };
                }
                else
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetTransJobRepairResponse> GetFilter(GetTransJobRepairFilterRequest param)
        {
            try
            {
                GetTransJobRepairResponse result = new GetTransJobRepairResponse();

                var data = await _repository.GetFilter(param);
                if (data.Any())
                {
                    var tempData = new List<GetTransJobRepairResponseData>();
                    foreach (var item in data)
                    {

                        tempData.Add(new GetTransJobRepairResponseData
                        {
                            recordCode = item.Id.ToString().Split('-')[0].ToUpper(),
                            CategoryName = item.ISysRepairCategory.Name,
                            CategoryId = item.SysRepairCategoryId,
                            DateOfFixedText = item.DateOfFixed.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                            DateOfFixed = item.DateOfFixed,
                            DateOfRequestText = item.DateOfRequest.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                            DateOfRequest = item.DateOfRequest,
                            DateOfStart = item.DateOfStart,
                            DateOfStartText = item.DateOfStart.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss),
                            Description = item.Description,
                            FixedDescription = item.FixedDescription,
                            Id = item.Id,
                            License = item.ITransKey.License,
                            Remark = item.Remark,
                            RequestName = item.Name,
                            StatusName = item.ISysJobRepairStatus.Name,
                            Telephone = item.Telephone,
                            TransKeyId = item.TransKeyId,
                            EquipmentId = item.TransEquipmentId,
                            Seq = item.Seq,
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

        public async Task<UpdateTransJobRepairResponse> Update(UpdateTransJobRepairRequest param)
        {
            try
            {
                UpdateTransJobRepairResponse result = new UpdateTransJobRepairResponse();
                var findCategory = await _categoryRepository.GetById(param.CategoryId.ToGuid());
                if (!ValidateService.Validate(findCategory))
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

                var data = await _repository.GetForUpdate(param.Id.ToGuid());
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                data.TransKeyId = findTransKey.Id;
                data.SysRepairCategoryId = findCategory.Id;
                data.Description = param.Description;
                data.Name = param.RequestName;
                data.Telephone = param.Telephone;
                data.Remark = param.Remark;
                data.DateOfRequest = param.DateOfRequest;
                data.DateOfStart = param.DateOfStart;
                data.DateOfFixed = param.DateOfFixed;
                data.FixedDescription = param.FixedDescription;

                data.UpdatedBy = param.UpdateBy.ToGuid();
                data.UpdatedDate = _datetime;

                await _repository.UpdateAsync();
                result.Data = new UpdateTransJobRepairResponseData { Id = data.Id };

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UpdateStatusTransJobRepairResponse> UpdateStatus(UpdateStatusTransJobRepairRequest param)
        {
            try
            {
                UpdateStatusTransJobRepairResponse result = new UpdateStatusTransJobRepairResponse();

                var data = await _repository.GetForUpdate(param.Id.ToGuid());
                if (!ValidateService.Validate(data))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var getStatus = await _statusRepository.GetAll();
                if (!ValidateService.Validate(getStatus))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var findStatus = getStatus.FirstOrDefault(a => a.Id == data.SysJobRepairStatusId);
                if (param.IsCancel)
                {
                    data.SysJobRepairStatusId = getStatus.FirstOrDefault(a => a.Seq == 4).Id;
                }
                else
                {
                    data.SysJobRepairStatusId = findStatus.Seq switch
                    {
                        1 => getStatus.FirstOrDefault(a => a.Seq == 2).Id,
                        2 => getStatus.FirstOrDefault(a => a.Seq == 3).Id,
                        _ => Guid.Empty
                    };

                    if (data.SysJobRepairStatusId == Guid.Empty)
                    {
                        result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                        return result;
                    }
                }

                await _repository.UpdateAsync();
                result.Data = new UpdateStatusTransJobRepairResponseData { Id = data.Id };
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

        private async Task<List<GetTransJobRepairResponseData>> SortingColumn(List<GetTransJobRepairResponseData> list, string sortName, string sortType)
        {
            var propertyInfo = typeof(GetTransJobRepairResponseData).GetProperty(sortName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

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

        public async Task<ExportTransJobRepairResponse> Export(string month, string year)
        {
            try
            {
                ExportTransJobRepairResponse result = new ExportTransJobRepairResponse();
                var data = await _repository.GetByMonthYear(month.ToInt(), year.ToInt());
                if (data != null)
                {
                    var listColumn = await GetColumnExportJobRepair();
                    var listSysOrgranize = await _orgranizeRepository.GetAll(true);
                    List<dynamic> list = new List<dynamic>();
                    int index = 1;
                    foreach (var item in data)
                    {
                        var findOrgranize = listSysOrgranize.Where(a => a.DepartmentCode == item.TransKey.SysOrgranize?.DepartmentCode
                                                                    && a.DivisionCode == item.TransKey.SysOrgranize?.DivisionCode
                                                                    && a.SectionCode == item.TransKey.SysOrgranize?.SectionCode
                                                                    && a.JobCode == item.TransKey.SysOrgranize?.JobCode).FirstOrDefault();

                        list.Add(await SetDynamicExport(item, index, findOrgranize?.Name));
                        index++;
                    }
                    var lastDayInMonth = DateTime.DaysInMonth(year.ToInt(), month.ToInt());
                    var thaiYear = (year.ToInt() + 543).ToString();
                    result.Data = GenerateExcelFile(JsonConvert.SerializeObject(list), listColumn, 1, lastDayInMonth, month.ToInt().GetMonthNameInThai(), thaiYear);
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        private async Task<List<string>> GetColumnExportJobRepair()
        {
            
            var result = new List<string>();
            result.Add("ลำดับ");
            result.Add(ColumnExportJobRepair.Account);
            result.Add(ColumnExportJobRepair.EquipmentCode);
            result.Add(ColumnExportJobRepair.Department);
            result.Add(ColumnExportJobRepair.RequestName);
            result.Add(ColumnExportJobRepair.Telephone);
            result.Add(ColumnExportJobRepair.Problam);
            result.Add(ColumnExportJobRepair.Detail);
            result.Add(ColumnExportJobRepair.FixDetail);
            result.Add(ColumnExportJobRepair.CreateDate);
            result.Add(ColumnExportJobRepair.Status);

            return result;
        }

        private async Task<Dictionary<string, object>> SetDynamicExport(GetTransJobRepair transJobRepair, int indexRow, string orgranizeName)
        {
            var dynamicProperties = new Dictionary<string, object>();

            dynamicProperties["Index"] = indexRow;
            dynamicProperties[$"{ColumnExportJobRepair.Account}"] = $"{transJobRepair.UserCreate.UserName}";
            dynamicProperties[$"{ColumnExportJobRepair.EquipmentCode}"] = $"{transJobRepair.TransEquipment?.EquipmentCode}";
            dynamicProperties[$"{ColumnExportJobRepair.Department}"] = $"{orgranizeName}";
            dynamicProperties[$"{ColumnExportJobRepair.RequestName}"] = $"{transJobRepair.Name}";
            dynamicProperties[$"{ColumnExportJobRepair.Telephone}"] = $"{transJobRepair.Telephone}";
            dynamicProperties[$"{ColumnExportJobRepair.Problam}"] = $"{transJobRepair.ISysRepairCategory.Name}";
            dynamicProperties[$"{ColumnExportJobRepair.Detail}"] = $"{transJobRepair.Description}";
            dynamicProperties[$"{ColumnExportJobRepair.FixDetail}"] = $"{transJobRepair.FixedDescription}";
            dynamicProperties[$"{ColumnExportJobRepair.CreateDate}"] = $"{transJobRepair.CreatedDate.ToDateTimeString(FormatDatetimeString.FormatyyyySlashMMSlashdd_HHmmss)}";
            dynamicProperties[$"{ColumnExportJobRepair.Status}"] = $"{transJobRepair.ISysJobRepairStatus.Name}";

            return dynamicProperties;
        }

        private static MemoryStream GenerateExcelFile(string dataJson, List<string> listColumn, int firstDay, int lastDay, string monthName, string year)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Sheet1");

                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Style.Font.FontSize = 16;
                worksheet.Style.Font.FontName = "TH SarabunPSK";

                int startrow = 1;
                int startcolumn = 1;
                int rowHeight = 20;
                #region | Set Header  |

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"รายงานการแก้ไขปัญหาการใช้งานโปรแกรมจัดการสำนักงาน";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow++;

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"ระหว่างวันที่ {firstDay} - {lastDay} {monthName} {year}";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow++;

                foreach (var item in listColumn)
                {
                    worksheet.Row(startrow).Height = rowHeight;

                    worksheet.Cell(startrow, startcolumn).Value = item;

                    worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                    worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                    startcolumn++;
                }
                startrow++;
                #endregion

                #region | Set Item |
                var listData = JsonConvert.DeserializeObject<List<dynamic>>(dataJson);

                foreach (var item in listData)
                {
                    startcolumn = 1;
                    var dict = item.ToObject<Dictionary<string, object>>();

                    foreach (var keyValue in dict)
                    {
                        if (keyValue.Value == null)
                        {
                            worksheet.Cell(startrow, startcolumn++).Value = "";
                        }
                        else
                        {
                            worksheet.Cell(startrow, startcolumn++).Value = keyValue.Value;
                        }
                    }

                    startrow++;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

                #endregion

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0; // Reset stream position to the beginning

                return stream;
            }
        }
    }
}
