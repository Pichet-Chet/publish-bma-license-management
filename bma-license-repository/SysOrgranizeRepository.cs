using bma_license_repository.CustomModel.SysOrgranize;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.SysOrgranize;
using bma_license_repository.Response.SysOrgranize;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace bma_license_repository
{
    public interface ISysOrgranizeRepository
    {
        Task<List<GetSysOrgranize>> GetAll();
        Task<List<GetSysOrgranize>> GetAll(bool isActive);
        Task<GetSysOrgranize> GetByName(string name);
        Task<GetSysOrgranize> GetById(Guid id);
        Task<List<GetSysOrgranize>> GetById(List<Guid> listId);
        Task<List<GetSysOrgranize>> GetByType(string typeName);


        Task<List<GetSysOrgranize>> GetFilter(GetSysOrgranizeFilterRequest param);
        Task<List<GetSysOrgranize>> GetDepartment();
        Task<GetSysOrgranize> GetDepartment(string code);
        Task<List<GetSysOrgranize>> GetDivision();
        Task<GetSysOrgranize> GetDivision(string departmentCode, string divisionCode);


        Task<List<GetSysOrgranize>> GetDivision(string departmentCode);
        Task<List<GetSysOrgranize>> GetSection();
        Task<GetSysOrgranize> GetSection(string departmentCode, string divisionCode, string sectionCode);
        Task<List<GetSysOrgranize>> GetSection(string departmentCode, string divisionCode);
        Task<List<GetSysOrgranize>> GetJob();


        Task<GetSysOrgranize> GetJob(string departmentCode, string divisionCode, string sectionCode, string jobCode);
        Task<List<GetSysOrgranize>> GetJob(string departmentCode, string divisionCode, string sectionCode);
        Task<GetSysOrgranize> GetOrgranize(string departmentCode, string divisionCode, string sectionCode, string jobCode);

        Task InsertAsync(SysOrgranize sysOrgranize);

        public Task<int> CountAllAsync();

    }
    public class SysOrgranizeRepository : ISysOrgranizeRepository
    {
        private readonly DevBmaContext _context;

        private readonly IMemoryCache _cache;


        public SysOrgranizeRepository(DevBmaContext context, IMemoryCache cache)
        {
            _context = context;

            _cache = cache;
        }

        public async Task<List<GetSysOrgranize>> GetAll()
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive)
                .Include(a => a.TransKeys)
                .ThenInclude(a => a.TransEquipments).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysOrgranize>> GetAll(bool isActive)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive == isActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysOrgranize> GetById(Guid id)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive && a.Id == id);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetSysOrgranize> GetByName(string name)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive && a.Name == name);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysOrgranize>> GetByType(string typeName)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive && a.TypeName == typeName).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysOrgranize>> GetDepartment()
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Department && a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysOrgranize> GetDepartment(string code)
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Department && a.DepartmentCode == code && a.IsActive);

            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysOrgranize>> GetDivision()
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Division && a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysOrgranize> GetDivision(string departmentCode, string divisionCode)
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Division
                                                        && a.DivisionCode == divisionCode
                                                        && a.DepartmentCode == departmentCode
                                                        && a.IsActive);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysOrgranize>> GetDivision(string departmentCode)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive && a.DepartmentCode == departmentCode && a.TypeName == OrgranizeType.Division).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<List<GetSysOrgranize>> GetFilter(GetSysOrgranizeFilterRequest param)
        {
            List<GetSysOrgranize> result = new List<GetSysOrgranize>();

            if (param == null)
            {
                return new List<GetSysOrgranize>();
            }
            else
            {

                var query = _context.SysOrgranizes
                    .Include(a => a.TransKeys)
                    .ThenInclude(b => b.SysKeyType)
                    .Include(a => a.TransKeys)
                    .ThenInclude(b => b.TransEquipments)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(param.TextSearch))
                {
                    query = query.Where(a =>
                    a.Name.ToLower().Contains(param.TextSearch.ToLower()) ||
                    a.DepartmentCode.ToLower().Contains(param.TextSearch) ||
                    a.DivisionCode.ToLower().Contains(param.TextSearch) ||
                    a.SectionCode.ToLower().Contains(param.TextSearch) ||
                    a.JobCode.ToLower().Contains(param.TextSearch) ||
                    a.Code.ToLower().Contains(param.TextSearch) ||
                    a.TypeName.ToLower().Contains(param.TextSearch) ||
                    a.Name.ToLower().Contains(param.TextSearch));
                }

                if (!string.IsNullOrEmpty(param.Id))
                {
                    query = query.Where(a => a.Id == param.Id.ToGuid());
                }

                if (!string.IsNullOrEmpty(param.DepartmentCode))
                {
                    query = query.Where(a => a.DepartmentCode == param.DepartmentCode);
                }

                if (!string.IsNullOrEmpty(param.DivisionCode))
                {
                    query = query.Where(a => a.DivisionCode == param.DivisionCode);
                }

                if (!string.IsNullOrEmpty(param.SectionCode))
                {
                    query = query.Where(a => a.SectionCode == param.SectionCode);
                }

                if (!string.IsNullOrEmpty(param.JobCode))
                {
                    query = query.Where(a => a.JobCode == param.JobCode);
                }

                if (!string.IsNullOrEmpty(param.TypeName))
                {
                    query = query.Where(a => a.TypeName == param.TypeName);
                }

                if (!string.IsNullOrEmpty(param.Code))
                {
                    query = query.Where(a => a.Code == param.Code);
                }

                if (param.IsActive != null)
                {
                    query = query.Where(x => x.IsActive == param.IsActive);
                }

                var output = await query.AsNoTracking().ToListAsync();

                var cacheKey = "SysOrgranizeCount";

                _cache.Set(cacheKey, output.Count());

                result = output
                    .Select(a => new GetSysOrgranize
                    {
                        Code = a.Code,
                        CreatedBy = a.CreatedBy,
                        CreatedDate = a.CreatedDate,
                        DepartmentCode = a.DepartmentCode,
                        DivisionCode = a.DivisionCode,
                        Id = a.Id,
                        IsActive = a.IsActive,
                        JobCode = a.JobCode,
                        Name = a.Name,
                        SectionCode = a.SectionCode,
                        Seq = a.Seq,
                        TypeName = a.TypeName,
                        UpdatedBy = a.UpdatedBy,
                        UpdatedDate = a.UpdatedDate,
                        GetTransKey = a.TransKeys == null ? null : a.TransKeys.Select(b => new CustomModel.TransKey.GetTransKey
                        {
                            Id = b.Id,
                            IsActive = b.IsActive,
                            Remark = b.Remark,
                            SysKeyTypeId = b.SysKeyTypeId,
                            SysOrgranizeId = b.SysOrgranizeId,
                            License = b.License,
                            GetSysKeyType = b.SysKeyType != null ? new CustomModel.SysKeyType.GetSysKeyType
                            {
                                Id = b.SysKeyType.Id,
                                Name = b.SysKeyType.Name,
                                IsActive = b.SysKeyType.IsActive,
                                Seq = b.SysKeyType.Seq
                            } : null,
                            GetTransEquipment = b.TransEquipments == null ? null : b.TransEquipments.Select(c => new CustomModel.TransEquipment.GetTransEquipment
                            {
                                Id = c.Id,
                            }).ToList()
                        }).ToList()
                    }).ToList();


            }

            return result;
        }

        public async Task<List<GetSysOrgranize>> GetJob()
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Job && a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysOrgranize> GetJob(string departmentCode, string divisionCode, string sectionCode, string jobCode)
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Job
                                                        && a.JobCode == jobCode
                                                        && a.IsActive
                                                        && a.SectionCode == sectionCode
                                                        && a.DivisionCode == divisionCode
                                                        && a.DepartmentCode == departmentCode);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysOrgranize>> GetJob(string departmentCode, string divisionCode, string sectionCode)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive && a.DepartmentCode == departmentCode
                                                        && a.DivisionCode == divisionCode
                                                        && a.TypeName == OrgranizeType.Job
                                                        && a.SectionCode == sectionCode).OrderBy("Seq asc");

            return await query.ConvertToListAsync();
        }

        public async Task<GetSysOrgranize> GetOrgranize(string departmentCode, string divisionCode, string sectionCode, string jobCode)
        {
            return await _context.SysOrgranizes.Where(a => a.DepartmentCode == departmentCode
                                                     && a.DivisionCode == divisionCode
                                                     && a.SectionCode == sectionCode
                                                     && a.JobCode == jobCode
                                                     && a.IsActive).ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysOrgranize>> GetSection()
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Section && a.IsActive).OrderBy("Seq asc");
            return await query.ConvertToListAsync();
        }

        public async Task<GetSysOrgranize> GetSection(string departmentCode, string divisionCode, string sectionCode)
        {
            var query = _context.SysOrgranizes.Where(a => a.TypeName == OrgranizeType.Section
                                                        && a.SectionCode == sectionCode
                                                        && a.DivisionCode == divisionCode
                                                        && a.DepartmentCode == departmentCode
                                                        && a.IsActive);
            return await query.ConvertToFirstOrDefaultAsync();
        }

        public async Task<List<GetSysOrgranize>> GetSection(string departmentCode, string divisionCode)
        {
            var query = _context.SysOrgranizes.Where(a => a.IsActive && a.DepartmentCode == departmentCode
                                                        && a.DivisionCode == divisionCode && a.TypeName == OrgranizeType.Section).OrderBy("Seq asc");

            return await query.ConvertToListAsync();
        }

        public async Task<int> CountAllAsync()
        {
            // Use a caching library like MemoryCache, Redis, etc.
            var cacheKey = "SysOrgranizeCount";
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

        public async Task<List<GetSysOrgranize>> GetById(List<Guid> listId)
        {
            return await _context.SysOrgranizes.Where(a => listId.Contains(a.Id)).ConvertToListAsync();
        }

        public async Task InsertAsync(SysOrgranize sysOrgranize)
        {
            await _context.SysOrgranizes.AddAsync(sysOrgranize);
            await _context.SaveChangesAsync();
        }


        private async Task<List<SysOrgranize>> SortingColumn(List<SysOrgranize> list, string sortName, string sortType)
        {
            var propertyInfo = typeof(SysOrgranize).GetProperty(sortName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

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
