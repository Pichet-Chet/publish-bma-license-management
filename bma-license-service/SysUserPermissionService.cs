using bma_license_repository;
using bma_license_repository.Request.SysUserPermission;
using bma_license_repository.Response.SysUserPermission;

namespace bma_license_service
{
    public interface ISysUserPermissionService
    {
        Task<GetSysUserPermissionFilterResponse> GetFilter(GetSysUserPermissionFilterRequest param);
    }
    public class SysUserPermissionService : ISysUserPermissionService
    {
        private readonly ISysUserPermissionRepository _repository;

        public SysUserPermissionService(ISysUserPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSysUserPermissionFilterResponse> GetFilter(GetSysUserPermissionFilterRequest param)
        {
            try
            {
                GetSysUserPermissionFilterResponse result = new GetSysUserPermissionFilterResponse();

                var findData = await _repository.GetFilter(param);
                if (findData != null)
                {
                    result.Data = new List<GetSysUserPermissionFilterResponseData>();
                    foreach (var item in findData)
                    {
                        result.Data.Add(new GetSysUserPermissionFilterResponseData
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
