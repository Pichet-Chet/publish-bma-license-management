using bma_license_repository.CustomModel.SysUserType;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysUserType;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;

namespace bma_license_repository
{
    public interface ISysUserTypeRepository
    {
        Task<List<GetSysUserType>> GetAll();
        Task<List<GetSysUserType>> GetAll(bool isActive);
        Task<List<GetSysUserType>> GetFilter(GetSysUserTypeFilterRequest param);
        Task<GetSysUserType> GetById(Guid id);
        Task<GetSysUserType> GetByName(string name);

        public Task<int> CountAllAsync();
    }


    public class SysUserTypeRepository : ISysUserTypeRepository
    {
        private readonly DevBmaContext _context;
        private readonly IMemoryCache _cache;
        public SysUserTypeRepository(DevBmaContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<GetSysUserType>> GetAll()
        {
            var query = _context.SysUserTypes.Where(a => a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUserType>> GetAll(bool isActive)
        {
            var query = _context.SysUserTypes.Where(a => a.IsActive == isActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysUserType> GetById(Guid id)
        {
            var query = _context.SysUserTypes.Where(a => a.IsActive && a.Id == id);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetSysUserType> GetByName(string name)
        {
            var query = _context.SysUserTypes.Where(a => a.IsActive && a.Name.Contains(name));
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysUserType>> GetFilter(GetSysUserTypeFilterRequest param)
        {
            if (param == null)
            {
                return new List<GetSysUserType>();
            }
            else
            {
                var query = _context.SysUserTypes.Where(a => a.IsActive).AsQueryable();
                if (!string.IsNullOrEmpty(param.Id))
                {
                    query = query.Where(a => a.Id == param.Id.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.TextSearch))
                {
                    query = query.Where(a => a.Name.Contains(param.TextSearch));
                }

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

                IQueryable<SysUserType> lastQuery = null;
                lastQuery = query.OrderBy(sortOrder);

                var output = await query.AsNoTracking().ToListAsync();

                var cacheKey = "SysUserTypeCount";

                _cache.Set(cacheKey, output.Count());

                if (param.IsAll)
                {
                    return await lastQuery.ConvertToListAsync();
                }
                else
                {
                    return await lastQuery.ConvertToListAsync(param.PageSize, param.PageNumber);
                }
            }
        }

        public async Task<int> CountAllAsync()
        {
            // Use a caching library like MemoryCache, Redis, etc.
            var cacheKey = "SysUserCount";
            if (!_cache.TryGetValue(cacheKey, out int count))
            {
                count = await _context.SysOrgranizes.CountAsync();

                // Set cache options and expiration as needed
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, count, cacheOptions);
            }

            return count;
        }
    }
}
