using bma_license_repository.CustomModel.SysUserGroup;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysUserGroup;
using System.Linq.Dynamic.Core;

namespace bma_license_repository
{
    public interface ISysUserGroupRepository
    {
        Task<List<GetSysUserGroup>> GetAll();
        Task<List<GetSysUserGroup>> GetAll(bool isActive);
        Task<GetSysUserGroup> GetById(Guid id);
        Task<List<GetSysUserGroup>> GetByName(string name);
        Task<List<GetSysUserGroup>> GetFilter(GetSysUserGroupFilterRequest param);
    }


    public class SysUserGroupRepository : ISysUserGroupRepository
    {
        private readonly DevBmaContext _context;

        public SysUserGroupRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<GetSysUserGroup>> GetAll()
        {
            var query = _context.SysUserGroups.Where(a => a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUserGroup>> GetAll(bool isActive)
        {
            var query = _context.SysUserGroups.Where(a => a.IsActive == isActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysUserGroup> GetById(Guid id)
        {
            var query = _context.SysUserGroups.Where(a => a.IsActive && a.Id == id);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysUserGroup>> GetByName(string name)
        {
            var query = _context.SysUserGroups.Where(a => a.IsActive && a.Name.Contains(name)).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysUserGroup>> GetFilter(GetSysUserGroupFilterRequest param)
        {
            if (param == null)
            {
                return new List<GetSysUserGroup>();
            }
            else
            {
                var query = _context.SysUserGroups.Where(a => a.IsActive).AsQueryable();
                if (!string.IsNullOrEmpty(param.Id))
                {
                    query = query.Where(a => a.Id == param.Id.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.TextSearch))
                {
                    query = query.Where(a => a.Name.Contains(param.TextSearch));
                }

                return await query.ConvertToListAsync();
            }


        }
    }
}
