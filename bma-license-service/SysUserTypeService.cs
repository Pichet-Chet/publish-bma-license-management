using bma_license_repository;
using bma_license_repository.Request.SysUserType;
using bma_license_repository.Response.SysUserType;

namespace bma_license_service
{
    public interface ISysUserTypeService
    {
        Task<GetSysUserTypeFilterResponse> GetFilter(GetSysUserTypeFilterRequest param);
        public Task<int> CountAllAsync();
    }
    public class SysUserTypeService : ISysUserTypeService
    {
        private readonly ISysUserTypeRepository _repository;

        public SysUserTypeService(ISysUserTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetSysUserTypeFilterResponse> GetFilter(GetSysUserTypeFilterRequest param)
        {
            try
            {
                GetSysUserTypeFilterResponse result = new GetSysUserTypeFilterResponse();

                var findData = await _repository.GetFilter(param);
                if (findData != null)
                {
                    result.Data = new List<GetSysUserTypeFilterResponseData>();
                    foreach (var item in findData)
                    {
                        result.Data.Add(new GetSysUserTypeFilterResponseData
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
    }
}
