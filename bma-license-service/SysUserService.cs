using bma_license_repository;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysUser;
using bma_license_repository.Response.SysOrgranize;
using bma_license_repository.Response.SysUser;
using bma_license_service.Helper;
using System.Reflection;
using static bma_license_repository.CustomModel.AppSetting.AppSettingConfig;

namespace bma_license_service
{
    public interface ISysUserService
    {
        Task<GetSysUserFilterResponse> GetFilter(GetSysUserFilterRequest param);
        Task<CreateUserResponse> Create(CreateUserRequest param);
        Task<UpdateUserResponse> Update(UpdateUserRequest param);
        Task<VerifyUserResponse> VerifyUser(VerifyUserRequest param);

        public Task<int> CountAllAsync();
    }

    public class SysUserService : ISysUserService
    {
        private readonly ISysUserRepository _repository;
        private readonly JWTConfig _jwtConfig;
        private readonly DateTime _datetime;
        private readonly ISysUserTypeRepository _userTypeRepository;
        private readonly ISysMessageConfigurationService _messageConfigurationService;
        private readonly ISysUserGroupRepository _userGroupRepository;
        private readonly ISysUserPermissionRepository _userPermissionRepository;
        private readonly ISysOrgranizeRepository _orgranizeRepository;

        public SysUserService(ISysUserRepository repository,
            JWTConfig jWTConfig,
            ISysUserTypeRepository userTypeRepository,
            ISysMessageConfigurationService messageConfigurationService,
            ISysUserGroupRepository sysUserGroupRepository,
            ISysUserPermissionRepository sysUserPermissionRepository,
            ISysOrgranizeRepository orgranizeRepository)
        {
            _repository = repository;
            _jwtConfig = jWTConfig;
            _datetime = DateTime.Now;
            _userTypeRepository = userTypeRepository;
            _messageConfigurationService = messageConfigurationService;
            _userGroupRepository = sysUserGroupRepository;
            _userPermissionRepository = sysUserPermissionRepository;
            _orgranizeRepository = orgranizeRepository;
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest param)
        {
            try
            {
                CreateUserResponse result = new CreateUserResponse();
                var findUserGroup = await _userGroupRepository.GetById(param.SysUserGroupId.ToGuid());
                if (!ValidateService.Validate(await _userTypeRepository.GetById(param.SysUserTypeId.ToGuid()))
                    || !ValidateService.Validate(findUserGroup)
                    || (!string.IsNullOrEmpty(param.SysUserPermissionId) && !ValidateService.Validate(await _userPermissionRepository.GetById(param.SysUserPermissionId.ToGuid())))
                    )
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }


                DateTime tokenExpired = _datetime.AddDays(int.Parse(_jwtConfig.TokenValidityInDays));

                var id = HelperService.GenerateGuid();
                SysUser data = new SysUser
                {
                    AccessToken = HelperService.GenerateToken(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, tokenExpired, id, findUserGroup.Name),
                    AccessTokenExpire = tokenExpired.ToDateTimeString(FormatDatetimeString.FormatyyyyMMdd_HHmmss),
                    CreatedBy = param.CreatedBy.ToGuid(),
                    CreatedDate = _datetime,
                    FullName = param.FullName,
                    Id = id,
                    IsActive = true,
                    Password = HelperService.HashPassword(param.Password),
                    SysUserGroupId = param.SysUserGroupId.ToGuid(),
                    SysUserPermissionId = !string.IsNullOrEmpty(param.SysUserPermissionId) ? param.SysUserPermissionId.ToGuid() : null,
                    SysUserTypeId = param.SysUserTypeId.ToGuid(),
                    UpdatedBy = param.CreatedBy.ToGuid(),
                    UpdatedDate = _datetime,
                    Username = param.UserName,
                    DepartmentCode = param.DepartmentCode,
                    DivisionCode = param.DivisionCode,
                    JobCode = param.JobCode,
                    SectionCode = param.SectionCode,
                };

                await _repository.InsertAsync(data);

                result.Data = new CreateUserResponseData { Id = id };
                result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeSaveSuccess);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetSysUserFilterResponse> GetFilter(GetSysUserFilterRequest param)
        {
            try
            {
                GetSysUserFilterResponse result = new GetSysUserFilterResponse();

                var data = await _repository.GetFilter(param);

                if (data != null && data.Any())
                {
                    var tempData = new List<GetFilterResponseData>();

                    var department = await _orgranizeRepository.GetDepartment();
                    var division = await _orgranizeRepository.GetDivision();
                    var section = await _orgranizeRepository.GetSection();
                    var job = await _orgranizeRepository.GetJob();

                    foreach (var filter in data)
                    {
                        var findDepartment = department.FirstOrDefault(a => a.DepartmentCode == filter.DepartmentCode);
                        var findDivision = division.FirstOrDefault(a => a.DepartmentCode == filter.DepartmentCode && a.DivisionCode == filter.DivisionCode);
                        var findSection = section.FirstOrDefault(a => a.DepartmentCode == filter.DepartmentCode && a.DivisionCode == filter.DivisionCode && a.SectionCode == filter.SectionCode);
                        var findJob = job.FirstOrDefault(a => a.DepartmentCode == filter.DepartmentCode && a.DivisionCode == filter.DivisionCode && a.SectionCode == filter.SectionCode && a.JobCode == filter.JobCode);

                        tempData.Add(new GetFilterResponseData
                        {
                            AccessToken = filter.AccessToken,
                            FullName = filter.FullName,
                            Id = filter.Id,
                            IsActive = filter.IsActive,
                            SysUserTypeId = filter.SysUserTypeId,
                            SysUserTypeName = filter.SysUserTypeName,
                            SysUserPermissionId = filter.SysUserPermissionId,
                            SysUserPermissionName = filter.SysUserPermissionName,
                            SysUserGroupId = filter.SysUserGroupId,
                            SysUserGroupName = filter.SysUserGroupName,
                            UserName = filter.UserName,
                            DepartmentCode = filter.DepartmentCode,
                            DepartmentName = findDepartment != null ? findDepartment.Name : string.Empty,
                            DivisionCode = filter.DivisionCode,
                            DivisionName = findDivision != null ? findDivision.Name : string.Empty,
                            JobCode = filter.JobCode,
                            JobName = findJob != null ? findJob.Name : string.Empty,
                            SectionCode = filter.SectionCode,
                            SectionName = findSection != null ? findSection.Name : string.Empty,
                        });
                    }

                    if(!string.IsNullOrEmpty(param.SortName) && !string.IsNullOrEmpty(param.SortType))
                    {
                        tempData = await SortingColumn(tempData, param.SortName, param.SortType);
                    }
                   
                    result.Data = tempData.ManageData(param.IsAll, param.PageSize, param.PageNumber);
                }
                else
                {
                    data = new List<bma_license_repository.CustomModel.SysUser.GetSysUser>();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<UpdateUserResponse> Update(UpdateUserRequest param)
        {
            try
            {
                UpdateUserResponse result = new UpdateUserResponse();

                var findUser = await _repository.GetForUpdate(param.Id.ToGuid());
                if (!ValidateService.Validate(findUser))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }


                findUser.IsActive = param.IsActive;
                findUser.UpdatedBy = param.UpdatedBy.ToGuid();
                findUser.SysUserGroupId = param.SysUserGroupId.ToGuid();
                findUser.SysUserPermissionId = param.SysUserPermissionId.ToGuid();
                findUser.FullName = param.FullName;
                findUser.DepartmentCode = param.DepartmentCode;
                findUser.DivisionCode = param.DivisionCode;
                findUser.SectionCode = param.SectionCode;
                findUser.JobCode = param.JobCode;

                findUser.Password = string.IsNullOrEmpty(param.Password) ? findUser.Password : HelperService.HashPassword(param.Password);

                await _repository.UpdateAsync(findUser);

                result.Data = new UpdateUserResponseData { Id = findUser.Id };
                result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeSaveSuccess);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<VerifyUserResponse> VerifyUser(VerifyUserRequest param)
        {
            try
            {
                VerifyUserResponse result = new VerifyUserResponse();
                var password = HelperService.HashPassword(param.Password);

                var findUser = await _repository.VerifyUser(param.UserName, password);
                if (!ValidateService.Validate(findUser))
                {
                    result.MessageAlert = await _messageConfigurationService.SetMessageAlert(ConfigMessage.CodeDataNotFound);
                    return result;
                }

                var user = await _repository.GetForUpdate(findUser.Id);

                DateTime tokenExpired = _datetime.AddDays(int.Parse(_jwtConfig.TokenValidityInDays));
                user.AccessToken = HelperService.GenerateToken(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, tokenExpired, findUser.Id, findUser.SysUserGroupName);
                user.AccessTokenExpire = tokenExpired.ToDateTimeString(FormatDatetimeString.FormatyyyyMMdd_HHmmss);

                await _repository.UpdateAsync(user);

                var findDepartment = await _orgranizeRepository.GetDepartment(findUser.DepartmentCode);
                var findDivision = await _orgranizeRepository.GetDivision(findUser.DepartmentCode, findUser.DivisionCode);
                var findSection = await _orgranizeRepository.GetSection(findUser.DepartmentCode, findUser.DivisionCode, findUser.SectionCode);
                var findJob = await _orgranizeRepository.GetJob(findUser.DepartmentCode, findUser.DivisionCode, findUser.SectionCode, findUser.JobCode);

                result.Data = new VerifyUserResponseData
                {
                    AccessToken = findUser.AccessToken,
                    DepartmentCode = findUser.DepartmentCode,
                    DepartmentName = findDepartment != null ? findDepartment.Name : string.Empty,
                    DivisionCode = findUser.DivisionCode,
                    DivisionName = findDivision != null ? findDivision.Name : string.Empty,
                    FullName = findUser.FullName,
                    Id = findUser.Id,
                    IsActive = findUser.IsActive,
                    JobCode = findUser.JobCode,
                    JobName = findJob != null ? findJob.Name : string.Empty,
                    SectionCode = findUser.SectionCode,
                    SectionName = findSection != null ? findSection.Name : string.Empty,
                    SysUserGroupName = findUser.SysUserGroupName,
                    SysUserPermissionName = findUser.SysUserPermissionName,
                    SysUserTypeName = findUser.SysUserTypeName,
                    UserName = findUser.UserName,
                };

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

        private async Task<List<GetFilterResponseData>> SortingColumn(List<GetFilterResponseData> list, string sortName, string sortType)
        {
            var propertyInfo = typeof(GetFilterResponseData).GetProperty(sortName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

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
