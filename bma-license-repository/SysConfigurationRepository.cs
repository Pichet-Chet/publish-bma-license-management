using bma_license_repository.CustomModel.SysConfiguration;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysConfiguration;
using System.Linq.Dynamic.Core;

namespace bma_license_repository
{
    public interface ISysConfigurationRepository
    {
        Task<List<GetSysConfiguration>> GetFilter(GeSysConfigurationtFilterRequest param);
        Task<List<GetSysConfiguration>> GetAll();
        Task<List<GetSysConfiguration>> GetAll(bool isActive);
        Task<GetSysConfiguration> GetById(Guid id);
        Task<GetSysConfiguration> GetByKey(string key);
    }



    public class SysConfigurationRepository : ISysConfigurationRepository
    {
        private readonly DevBmaContext _context;

        public SysConfigurationRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<GetSysConfiguration>> GetAll()
        {
            return await _context.SysConfigurations.ConvertToListAsync();
        }

        public async Task<List<GetSysConfiguration>> GetAll(bool isActive)
        {
            return await _context.SysConfigurations.Where(a => a.IsActive == isActive).ConvertToListAsync();
        }

        public async Task<GetSysConfiguration> GetById(Guid id)
        {
            return await _context.SysConfigurations.Where(a => a.Id == id).ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetSysConfiguration> GetByKey(string key)
        {
            return await _context.SysConfigurations.Where(a => a.Key == key).ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysConfiguration>> GetFilter(GeSysConfigurationtFilterRequest param)
        {
            if (param == null)
            {
                return new List<GetSysConfiguration>();
            }
            else
            {
                string sortOrder = string.Empty;

                if (!string.IsNullOrEmpty(param.SortName))
                {
                    sortOrder = param.SortName;
                }
                else
                {
                    sortOrder = "CreatedDate";
                }

                if (!string.IsNullOrEmpty(param.SortType))
                {
                    sortOrder += $" {param.SortType}";
                }
                else
                {
                    sortOrder += " desc";
                }

                var query = _context.SysConfigurations.Where(a => a.IsActive).AsQueryable();
                if (!string.IsNullOrEmpty(param.Id))
                {
                    query = query.Where(a => a.Id == param.Id.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.TextSearch))
                {
                    query = query.Where(a => a.Key.Contains(param.TextSearch) || a.Value.Contains(param.TextSearch) || a.Description.Contains(param.TextSearch));
                }

                if (param.IsAll)
                {
                    return await query.OrderBy(sortOrder).ConvertToListAsync();
                }
                else
                {
                    return await query.OrderBy(sortOrder).ConvertToListAsync(param.PageSize, param.PageNumber);
                }
            }
        }
    }
}
