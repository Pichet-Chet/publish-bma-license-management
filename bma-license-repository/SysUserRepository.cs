using bma_license_repository.CustomModel.SysUser;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;

namespace bma_license_repository
{
    public interface ISysUserRepository
    {
        Task<List<GetSysUser>> GetAll();
        Task<List<GetSysUser>> GetAll(bool isActive);
        Task<List<GetSysUser>> GetByUserTypeId(Guid userTypeId);
        Task<List<GetSysUser>> GetByUserGroupId(Guid userGroupId);
        Task<List<GetSysUser>> GetByUserPermissionId(Guid userPermissionId);
        Task<GetSysUser> GetById(Guid id);
        Task<GetSysUser> VerifyUser(string userName, string password);
        Task<List<GetSysUser>> GetFilter(GetSysUserFilterRequest param);
        Task InsertAsync(SysUser sysUser);
        Task InsertAsync(List<SysUser> list);
        Task UpdateAsync(SysUser sysUser);
        public Task<int> CountAllAsync();
        Task<SysUser> GetForUpdate(Guid id);
    }



    public class SysUserRepository : ISysUserRepository
    {
        private readonly DevBmaContext _context;

        private readonly IMemoryCache _cache;

        public SysUserRepository(DevBmaContext context, IMemoryCache cache)
        {
            _context = context;

            _cache = cache;
        }

        public async Task<List<GetSysUser>> GetAll()
        {
            var query = _context.SysUsers.Where(a => a.IsActive)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType)
                .OrderBy("FullName asc");

            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUser>> GetAll(bool isActive)
        {
            var query = _context.SysUsers.Where(a => a.IsActive == isActive)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType)
                .OrderBy("FullName asc");

            return await query.ConvertToListAsync();
        }

        public async Task<GetSysUser> GetById(Guid id)
        {
            var query = _context.SysUsers.Where(a => a.IsActive && a.Id == id)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType);

            return await query.ConvertToFirstOrDefaultAsync();

        }

        public async Task<List<GetSysUser>> GetByUserGroupId(Guid userGroupId)
        {

            var query = _context.SysUsers.Where(a => a.IsActive && a.SysUserGroupId == userGroupId)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType)
                .OrderBy("FullName asc");

            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUser>> GetByUserPermissionId(Guid userPermissionId)
        {
            var query = _context.SysUsers.Where(a => a.IsActive && a.SysUserPermissionId.HasValue && a.SysUserPermissionId == userPermissionId)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType)
                .OrderBy("FullName asc");

            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUser>> GetByUserTypeId(Guid userTypeId)
        {
            var query = _context.SysUsers.Where(a => a.IsActive && a.SysUserTypeId == userTypeId)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType)
                .OrderBy("FullName asc");

            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUser>> GetFilter(GetSysUserFilterRequest param)
        {
            if (param == null)
            {
                return new List<GetSysUser>();
            }
            else
            {
                var query = _context.SysUsers.AsQueryable();

                if (!string.IsNullOrEmpty(param.Id))
                {
                    query = query.Where(a => a.Id == param.Id.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.SysUserPermissionId))
                {
                    query = query.Where(a => a.SysUserPermissionId == param.SysUserPermissionId.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.SysUserGroupId))
                {
                    query = query.Where(a => a.SysUserGroupId == param.SysUserGroupId.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.SysUserTypeId))
                {
                    query = query.Where(a => a.SysUserTypeId == param.SysUserTypeId.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.TextSearch))
                {
                    query = query.Where(a => a.Username.Contains(param.TextSearch) || a.FullName.Contains(param.TextSearch));
                }

                if (param.IsActive != null)
                {
                    query = query.Where(a => a.IsActive == param.IsActive);
                }

                query = query.Where(x => x.Username.ToLower() != "system").AsQueryable();

                IQueryable<SysUser> lastQuery = null;
                lastQuery = query
                  .Include(a => a.SysUserGroup)
                  .Include(a => a.SysUserPermission)
                  .Include(a => a.SysUserType);

                var output = await query.AsNoTracking().ToListAsync();

                var cacheKey = "SysUserCount";

                _cache.Set(cacheKey, output.Count());

                return await lastQuery.ConvertToListAsync();
            }
        }

        public async Task InsertAsync(SysUser sysUser)
        {
            await _context.SysUsers.AddAsync(sysUser);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(List<SysUser> list)
        {
            await _context.SysUsers.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SysUser sysUser)
        {
            _context.SysUsers.Update(sysUser);
            await _context.SaveChangesAsync();
        }

        public async Task<GetSysUser> VerifyUser(string userName, string password)
        {
            var query = _context.SysUsers.Where(a => a.Username == userName && a.Password == password)
                .Include(a => a.SysUserGroup)
                .Include(a => a.SysUserPermission)
                .Include(a => a.SysUserType);

            return await query.ConvertToFirstOrDefaultAsync();
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

        public async Task<SysUser> GetForUpdate(Guid id)
        {
            return await _context.SysUsers.Where(a => a.Id == id).FirstOrDefaultAsync();
        }
    }

}
