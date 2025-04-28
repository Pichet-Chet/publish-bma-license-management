using bma_license_repository;
using bma_license_repository.Request.SysUserGroup;
using bma_license_repository.Response.SysUserGroup;

namespace bma_license_service
{
    public interface ISysUserGroupService
    {
        Task<GetSysUserGroupFilterResponse> GetFilter(GetSysUserGroupFilterRequest param);
    }

    public class SysUserGroupService : ISysUserGroupService
    {
        private readonly ISysUserGroupRepository _repository;

        public SysUserGroupService(ISysUserGroupRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSysUserGroupFilterResponse> GetFilter(GetSysUserGroupFilterRequest param)
        {
            try
            {
                GetSysUserGroupFilterResponse result = new GetSysUserGroupFilterResponse();

                var findData = await _repository.GetFilter(param);
                if (findData != null)
                {
                    result.Data = new List<GetUserGroupFilterResponseData>();
                    foreach (var item in findData)
                    {
                        result.Data.Add(new GetUserGroupFilterResponseData
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Seq = item.Seq,
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
