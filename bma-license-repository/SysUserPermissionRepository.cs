using bma_license_repository.CustomModel.SysUserPermission;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysUserPermission;
using System.Linq.Dynamic.Core;

namespace bma_license_repository
{
    public interface ISysUserPermissionRepository
    {
        Task<List<GetSysUserPermission>> GetAll();
        Task<List<GetSysUserPermission>> GetAll(bool isActive);
        Task<List<GetSysUserPermission>> GetFilter(GetSysUserPermissionFilterRequest param);
        Task<GetSysUserPermission> GetById(Guid id);
        Task<GetSysUserPermission> GetByName(string name);
    }

    public class SysUserPermissionRepository : ISysUserPermissionRepository
    {
        private readonly DevBmaContext _context;

        public SysUserPermissionRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<GetSysUserPermission>> GetAll()
        {
            var query = _context.SysUserPermissions.Where(a => a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUserPermission>> GetAll(bool isActive)
        {
            var query = _context.SysUserPermissions.Where(a => a.IsActive == isActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysUserPermission> GetById(Guid id)
        {
           var query =_context.SysUserPermissions.Where(a => a.IsActive && a.Id == id);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetSysUserPermission> GetByName(string name)
        {
            var query = _context.SysUserPermissions.Where(a => a.IsActive && a.Name.Contains(name));
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysUserPermission>> GetFilter(GetSysUserPermissionFilterRequest param)
        {
            if (param == null)
            {
                return new List<GetSysUserPermission>();
            }
            else
            {
                var query = _context.SysUserPermissions.Where(a => a.IsActive).AsQueryable();
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

                IQueryable<SysUserPermission> lastQuery = null;
                lastQuery = query.OrderBy(sortOrder);
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
    }
}
