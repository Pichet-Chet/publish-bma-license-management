
using System.Dynamic;
using System.Reflection;
using bma_license_repository;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysOrgranize;
using bma_license_repository.Response.SysOrgranize;
using bma_license_repository.Response.TransKey;
using bma_license_service.Helper;
using Newtonsoft.Json;

namespace bma_license_service
{
    public interface ISysOrgranizeService
    {
        Task<GetSysOrgranizeFilterResponse> GetFilter(GetSysOrgranizeFilterRequest param);
        Task<GetSysOrgranizeDepartmentResponse> GetDepaerment();
        Task<GetSysOrgranizeDivisionResponse> GetDivision(string departmentCode);
        Task<GetSysOrgranizeSectionResponse> GetSection(string departmentCode, string divisionCode);
        Task<GetSysOrgranizeJobResponse> GetJob(string departmentCode, string divisionCode, string sectionCode);
        Task<GetSysOrgranizeFilterExportResponse> GetFilterExport(GetSysOrgranizeFilterExportRequest param);

        public Task<int> CountAllAsync();
    }

    public class SysOrgranizeService : ISysOrgranizeService
    {
        private readonly ISysOrgranizeRepository _repository;
        private readonly ITransKeyRepository _transKeyRepository;
        private readonly IServiceProvider _serviceProvider;

        public SysOrgranizeService(ISysOrgranizeRepository repository, ITransKeyRepository transKeyRepository, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _transKeyRepository = transKeyRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<GetSysOrgranizeDepartmentResponse> GetDepaerment()
        {
            try
            {
                GetSysOrgranizeDepartmentResponse result = new GetSysOrgranizeDepartmentResponse();

                var data = await _repository.GetDepartment();

                if (data != null)
                {
                    result.Data = new List<GetSysOrgranizeDepartmentResponseData>();

                    foreach (var item in data)
                    {
                        result.Data.Add(new GetSysOrgranizeDepartmentResponseData
                        {
                            Code = item.DepartmentCode,
                            Id = item.Id,
                            Name = item.Name,
                        });
                    }

                    result.Data = data
                        .OrderBy(x => x.Name)
                        .GroupBy(item => item.DepartmentCode)
                        .Select(group => new GetSysOrgranizeDepartmentResponseData
                        {
                            Code = group.Key,
                            Id = group.First().Id,
                            Name = group.First().Name,
                        })
                        .ToList();

                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetSysOrgranizeDivisionResponse> GetDivision(string departmentCode)
        {
            try
            {
                GetSysOrgranizeDivisionResponse result = new GetSysOrgranizeDivisionResponse();

                var data = await _repository.GetDivision(departmentCode);
                if (data != null)
                {
                    result.Data = new List<GetSysOrgranizeDivisionResponseData>();
                    foreach (var item in data)
                    {
                        result.Data.Add(new GetSysOrgranizeDivisionResponseData
                        {
                            Code = item.DivisionCode,
                            Id = item.Id,
                            Name = item.Name,
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

        public async Task<GetSysOrgranizeFilterResponse> GetFilter(GetSysOrgranizeFilterRequest param)
        {
            try
            {
                GetSysOrgranizeFilterResponse result = new GetSysOrgranizeFilterResponse();

                var data = await _repository.GetFilter(param);

                if (data != null)
                {
                    var tempData = new List<GetSysOrgranizeFilterResponseData>();
                    var sysOrgranize = await _repository.GetAll();

                    foreach (var item in data)
                    {
                        int Allocate = 0;
                        int InstallPermission = 0;
                        int Install = 0;
                        int Remain = 0;
                        decimal InstallPercentage = 0;
                        string PermissionType = string.Empty;

                        if (item.GetTransKey != null && item.GetTransKey.Any())
                        {
                            var groupPermissionType = item.GetTransKey.GroupBy(a => a.GetSysKeyType.Name).Select(group => group.Key).ToList();

                            Allocate = item.GetTransKey.Count;
                            InstallPermission = item.GetTransKey.Where(a => a.GetTransEquipment != null && a.GetTransEquipment.Any()).Count();
                            Install = item.GetTransKey.Where(a => a.GetTransEquipment != null && a.GetTransEquipment.Any()).Count();
                            PermissionType = string.Join(",", groupPermissionType);
                            Remain = item.GetTransKey.Where(a => a.GetTransEquipment == null || !a.GetTransEquipment.Any()).Count();
                            InstallPercentage = InstallPermission == 0 ? 0 : ((InstallPermission.ToDecimal() / Allocate.ToDecimal()) * 100).ToDecimal();
                        }

                        var findDepartment = sysOrgranize.GetOrgranize(OrgranizeType.Department, item.DepartmentCode);
                        var findDivision = sysOrgranize.GetOrgranize(OrgranizeType.Division, item.DepartmentCode, item.DivisionCode);
                        var findSection = sysOrgranize.GetOrgranize(OrgranizeType.Section, item.DepartmentCode, item.DivisionCode, item.SectionCode);
                        var findJob = sysOrgranize.GetOrgranize(OrgranizeType.Job, item.DepartmentCode, item.DivisionCode, item.SectionCode, item.JobCode);

                        tempData.Add(new GetSysOrgranizeFilterResponseData
                        {
                            Code = item.Code,
                            DepartmentCode = item.DepartmentCode,
                            DepartmentName = findDepartment != null ? findDepartment.Name : string.Empty,
                            DivisionCode = item.DivisionCode,
                            DivisionName = findDivision != null ? findDivision.Name : string.Empty,
                            Id = item.Id,
                            JobCode = item.JobCode,
                            JobName = findJob != null ? findJob.Name : string.Empty,
                            Name = item.Name,
                            SectionCode = item.SectionCode,
                            SectionName = findSection != null ? findSection.Name : string.Empty,
                            Seq = item.Seq,
                            TypeName = item.TypeName,
                            Allocate = Allocate,
                            Install = Install,
                            InstallPercentage = InstallPercentage,
                            InstallPermission = InstallPermission,
                            Remain = Remain,
                            PermissionType = PermissionType
                        });
                    }

                    if (!string.IsNullOrEmpty(param.SortName) && !string.IsNullOrEmpty(param.SortType))
                    {
                        tempData = await SortingColumn(tempData, param.SortName, param.SortType);
                    }

                    tempData = tempData.ManageData(param.IsAll, param.PageSize, param.PageNumber);


                    result.Data = tempData;

                    //result.Data = tempData.ManageData(param.IsAll, param.PageSize, param.PageNumber);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetSysOrgranizeFilterExportResponse> GetFilterExport(GetSysOrgranizeFilterExportRequest param)
        {
            try
            {
                GetSysOrgranizeFilterExportResponse result = new GetSysOrgranizeFilterExportResponse();

                var filter = new GetSysOrgranizeFilterRequest
                {
                    Code = param.Code,
                    DepartmentCode = param.DepartmentCode,
                    DivisionCode = param.DevisionCode,
                    Id = param.Id,
                    IsAll = true,
                    JobCode = param.JobCode,
                    SectionCode = param.SectionCode,
                    TextSearch = param.TextSearch,
                    TypeName = param.TypeName,
                };

                var data = await _repository.GetFilter(filter);

                if (data != null && data.Count > 0)
                {
                    List<GetSysOrgranizeFilterResponseData> list = new List<GetSysOrgranizeFilterResponseData>();
                    List<dynamic> listData = new List<dynamic>();
                    dynamic dynamicEntity = new ExpandoObject();
                    var dynamicProperties = (IDictionary<string, object>)dynamicEntity;

                    var department = await _repository.GetDepartment();
                    var division = await _repository.GetDivision();
                    var section = await _repository.GetSection();
                    var job = await _repository.GetJob();

                    List<string> listHeader = await GetHeader(param.IsAllColumn, param.ColumnIndex);

                    await Parallel.ForEachAsync(data, async (item, cancellationToken) =>
                     {
                         int Allocate = 0;
                         int InstallPermission = 0;
                         int Install = 0;
                         int Remain = 0;
                         decimal InstallPercentage = 0;
                         string PermissionType = string.Empty;
                         var dynamicProperties = new Dictionary<string, object>();

                         if (item.GetTransKey != null && item.GetTransKey.Any())
                         {
                             var groupName = item.GetTransKey.GroupBy(x => x.License).Select(group => group.Key).ToList();
                             var groupPermissionType = item.GetTransKey.GroupBy(a => a.GetSysKeyType.Name).Select(group => group.Key).ToList();
                             Allocate = item.GetTransKey.Count;
                             InstallPermission = groupName.Count();
                             Install = item.GetTransKey.Where(a => a.GetTransEquipment != null).Count();
                             PermissionType = string.Join(",", groupPermissionType);
                             Remain = item.GetTransKey.Where(a => a.GetTransEquipment == null).Count();
                             InstallPercentage = ((Remain.ToDecimal() / InstallPermission.ToDecimal()) * 100).ToDecimal();
                         }

                         var findDepartment = department.FirstOrDefault(a => a.DepartmentCode == item.DepartmentCode);
                         var findDivision = division.FirstOrDefault(a => a.DepartmentCode == item.DepartmentCode && a.DivisionCode == item.DivisionCode);
                         var findSection = section.FirstOrDefault(a => a.DepartmentCode == item.DepartmentCode && a.DivisionCode == item.DivisionCode && a.SectionCode == item.SectionCode);
                         var findJob = job.FirstOrDefault(a => a.DepartmentCode == item.DepartmentCode && a.DivisionCode == item.DivisionCode && a.SectionCode == item.SectionCode && a.JobCode == item.JobCode);

                         if (param.IsAllColumn)
                         {
                             dynamicProperties["Code"] = item.Code;
                             dynamicProperties["Name"] = item.Name;
                             dynamicProperties["DepartmentName"] = findDepartment?.Name ?? string.Empty;
                             dynamicProperties["DivisionName"] = findDivision?.Name ?? string.Empty;
                             dynamicProperties["SectionName"] = findSection?.Name ?? string.Empty;
                             dynamicProperties["JobName"] = findJob?.Name ?? string.Empty;
                             dynamicProperties["Allocate"] = Allocate;
                             dynamicProperties["InstallPermission"] = InstallPermission;
                             dynamicProperties["Install"] = Install;
                             dynamicProperties["Remain"] = Remain;
                             dynamicProperties["InstallPercentage"] = InstallPercentage;
                             dynamicProperties["PermissionType"] = PermissionType;
                         }
                         else
                         {
                             if (param.ColumnIndex != null && param.ColumnIndex.Any())
                             {
                                 foreach (var index in param.ColumnIndex)
                                 {
                                     switch (index)
                                     {
                                         case 1:
                                             dynamicProperties["Code"] = item.Code;
                                             break;
                                         case 2:
                                             dynamicProperties["Name"] = item.Name;
                                             break;
                                         case 3:
                                             dynamicProperties["DepartmentName"] = findDepartment?.Name ?? string.Empty;
                                             break;
                                         case 4:
                                             dynamicProperties["DivisionName"] = findDivision?.Name ?? string.Empty;
                                             break;
                                         case 5:
                                             dynamicProperties["SectionName"] = findSection?.Name ?? string.Empty;
                                             break;
                                         case 6:
                                             dynamicProperties["JobName"] = findJob?.Name ?? string.Empty;
                                             break;
                                         case 7:
                                             dynamicProperties["Allocate"] = Allocate;
                                             break;
                                         case 8:
                                             dynamicProperties["InstallPermission"] = InstallPermission;
                                             break;
                                         case 9:
                                             dynamicProperties["Install"] = Install;
                                             break;
                                         case 10:
                                             dynamicProperties["Remain"] = Remain;
                                             break;
                                         case 11:
                                             dynamicProperties["InstallPercentage"] = InstallPercentage;
                                             break;
                                         case 12:
                                             dynamicProperties["PermissionType"] = PermissionType;
                                             break;
                                     }
                                 }
                             }
                         }

                         lock (listData) // ใช้ lock เพื่อป้องกันปัญหาจากการเขียนข้อมูลพร้อมกัน
                         {
                             listData.Add(dynamicProperties);
                         }
                     });


                    result.Data = HelperService.GenerateExcelFile(JsonConvert.SerializeObject(listData), listHeader);
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetSysOrgranizeJobResponse> GetJob(string departmentCode, string divisionCode, string sectionCode)
        {
            try
            {
                GetSysOrgranizeJobResponse result = new GetSysOrgranizeJobResponse();
                var data = await _repository.GetJob(departmentCode, divisionCode, sectionCode);
                if (data != null)
                {
                    result.Data = new List<GetSysOrgranizeJobResponseData>();
                    foreach (var item in data)
                    {
                        result.Data.Add(new GetSysOrgranizeJobResponseData
                        {
                            Code = item.JobCode,
                            Id = item.Id,
                            Name = item.Name,
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

        public async Task<GetSysOrgranizeSectionResponse> GetSection(string departmentCode, string divisionCode)
        {
            try
            {
                GetSysOrgranizeSectionResponse result = new GetSysOrgranizeSectionResponse();
                var data = await _repository.GetSection(departmentCode, divisionCode);
                if (data != null)
                {
                    result.Data = new List<GetSysOrgranizeSectionResponseData>();
                    foreach (var item in data)
                    {
                        result.Data.Add(new GetSysOrgranizeSectionResponseData
                        {
                            Code = item.SectionCode,
                            Id = item.Id,
                            Name = item.Name,
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

        private async Task<List<string>> GetHeader(bool isAll, List<int> listColumnIndex)
        {
            List<string> result = new List<string>();
            if (isAll)
            {
                result.Add("รหัสหน่วยงาน");
                result.Add("ชื่อหน่วยงาน");
                result.Add("Department");
                result.Add("Division");
                result.Add("Section");
                result.Add("Job");
                result.Add("จัดสรร");
                result.Add("ติดตั้ง (สิทธิ์)");
                result.Add("ติดตั้ง (เครื่อง)");
                result.Add("คงเหลือ (สิทธิ์)");
                result.Add("%ติดตั้ง (สิทธิ์)");
                result.Add("ประเภทสิทธิ์");
            }
            else
            {
                foreach (var index in listColumnIndex)
                {
                    switch (index)
                    {
                        case 1:
                            result.Add("รหัสหน่วยงาน");
                            break;
                        case 2:
                            result.Add("ชื่อหน่วยงาน");
                            break;
                        case 3:
                            result.Add("Department");
                            break;
                        case 4:
                            result.Add("Division");
                            break;
                        case 5:
                            result.Add("Section");
                            break;
                        case 6:
                            result.Add("Job");
                            break;
                        case 7:
                            result.Add("จัดสรร");
                            break;
                        case 8:
                            result.Add("ติดตั้ง (สิทธิ์)");
                            break;
                        case 9:
                            result.Add("ติดตั้ง (เครื่อง)");
                            break;
                        case 10:
                            result.Add("คงเหลือ (สิทธิ์)");
                            break;
                        case 11:
                            result.Add("% ติดตั้ง (สิทธิ์)");
                            break;
                        case 12:
                            result.Add("ประเภทสิทธิ์");
                            break;
                    }
                }
            }
            return result;
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

        private async Task<List<GetSysOrgranizeFilterResponseData>> SortingColumn(List<GetSysOrgranizeFilterResponseData> list, string sortName, string sortType)
        {
            var propertyInfo = typeof(GetSysOrgranizeFilterResponseData).GetProperty(sortName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

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
