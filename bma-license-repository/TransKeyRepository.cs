using System.Reflection.Emit;
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Request.TransKey;
using bma_license_repository.Response.TransKey;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace bma_license_repository
{
    public interface ITransKeyRepository
    {
        Task<List<GetTransKey>> GetAll();
        Task<List<GetTransKey>> GetAll(bool isActive);
        Task<List<GetTransKey>> GetBySysOrgranizeId(Guid sysOrgranizeId);
        Task<List<GetTransKey>> GetBySysKeyTypeId(Guid sysKeyTypeId);
        Task<GetCountTop10> GetTop10();
        Task<GetAllocateOverview> GetAllocateOverview();
        Task<GetTransKey> GetById(Guid id);
        Task<List<GetTransKey>> GetFilter(GetTransKeyFilterRequest param);
        Task<List<GetTransKey>> GetByName(string name);
        Task UpdateAsync();
        Task UpdateAsync(TransKey transKey);
        Task UpdateAsync(List<TransKey> list);
        Task<List<TransKey>> GetForUpdate(List<Guid> listId);
        Task<TransKey> GetForUpdate(Guid id);
        Task<TransKey> GetForUpdate(string name);
        public Task<int> CountAllAsync(string cacheName);
        Task InsertAsync(TransKey transKey);
        Task<int> GetCountAll();
    }

    public class TransKeyRepository : ITransKeyRepository
    {
        private readonly DevBmaContext _context;
        private readonly IMemoryCache _cache;
        private readonly ISysOrgranizeRepository _sysOrgranizeRepository;

        public TransKeyRepository(DevBmaContext context, IMemoryCache cache, ISysOrgranizeRepository sysOrgranizeRepository)
        {
            _context = context;
            _cache = cache;
            _sysOrgranizeRepository = sysOrgranizeRepository;
        }

        public async Task<List<GetTransKey>> GetAll()
        {
            return await _context.TransKeys.Include(a => a.SysOrgranize).Include(a => a.SysKeyType).Include(a => a.TransEquipments).ConvertToListAsync();
        }

        public async Task<List<GetTransKey>> GetAll(bool isActive)
        {
            return await _context.TransKeys.Where(a => a.IsActive == isActive).Include(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.TransEquipments).ConvertToListAsync();
        }

        public async Task<GetTransKey> GetById(Guid id)
        {
            return await _context.TransKeys.Where(a => a.Id == id).Include(a => a.SysOrgranize).Include(a => a.SysKeyType).Include(a => a.TransEquipments).ConvertToFirstOrDefaultAsync();
        }

        public async Task<GetCountTop10> GetTop10()
        {
            var topOrgranizeData = await _context.TransKeys
            .Where(a => a.SysOrgranizeId.HasValue)
            .GroupBy(a => a.SysOrgranize)
            .Select(group => new
            {
                SysOrgranize = group.Key.Name,
                TransKeyCount = group.Count()
            })
            .OrderByDescending(x => x.TransKeyCount)
            .Take(10)
            .ToListAsync();

            var result = new GetCountTop10
            {
                TopOrgranizes = topOrgranizeData.Select(x => new SysOrgranizeCountDto
                {
                    OrgranizeName = x.SysOrgranize,
                    TransKeyCount = x.TransKeyCount
                }).ToList()
            };
            return result;
        }


        public async Task<GetAllocateOverview> GetAllocateOverview()
        {
            GetAllocateOverview output = new GetAllocateOverview();


            var GetSysOrganizeAll = await _context.SysOrgranizes.AsNoTracking().ToListAsync();
            var GetTransKeysAll = await _context.TransKeys.Include(a => a.TransEquipments).AsNoTracking().ToListAsync();


            var result = GetSysOrganizeAll
            .Where(o => o.IsActive) // กรองเฉพาะหน่วยงานที่ Active
            .GroupJoin(
                GetTransKeysAll.Where(t => t.TransEquipments != null && t.IsActive), // กรอง TransKeys ที่ Active และ EquipmentCode ไม่ว่าง
                org => org.Id,
                tran => tran.SysOrgranizeId,
                (org, trans) => new { Orgranize = org, TransKeys = trans }
            )
            .SelectMany(group => group.TransKeys, (group, tran) => new
            {
                Orgranize = group.Orgranize,
                EquipmentCount = tran.TransEquipments != null ? tran.TransEquipments.Count : 0,
                TypeName = group.Orgranize.TypeName
            })
            .GroupBy(o => new { o.TypeName, o.Orgranize.DepartmentCode, o.Orgranize.DivisionCode, o.Orgranize.SectionCode, o.Orgranize.JobCode, o.EquipmentCount })
            .Select(g => new
            {
                DepartmentName = g.Key.DepartmentCode,
                DivisionName = g.Key.DivisionCode,
                SectionName = g.Key.SectionCode,
                JobName = g.Key.JobCode,
                EquipmentCount = g.Key.EquipmentCount
            })
            .ToList();


            //var GetSysOrganizeAll = await _context.SysOrgranizes.ToListAsync();

            output = new GetAllocateOverview
            {
                getDepartments = result
        .GroupBy(x => x.DepartmentName)
        .Select(departmentGroup => new GetDepartment
        {
            name = GetOrganizeName(OrgranizeType.Department, departmentGroup.Key + "000000"),
            value = departmentGroup.Sum(d => d.EquipmentCount),
            path = GetOrganizeName(OrgranizeType.Department, departmentGroup.Key + "000000"),
            getDivisions = departmentGroup
                .GroupBy(d => d.DivisionName)
                .Select(divisionGroup => new GetDivision
                {
                    name = GetOrganizeName(OrgranizeType.Division, departmentGroup.Key + divisionGroup.Key + "0000"),
                    value = divisionGroup.Sum(d => d.EquipmentCount),
                    path = BuildPath(
                        departmentGroup.Key + "000000",
                        departmentGroup.Key + divisionGroup.Key + "0000"
                    ),
                    getSections = divisionGroup
                        .GroupBy(d => d.SectionName)
                        .Select(sectionGroup => new GetSection
                        {
                            name = GetOrganizeName(OrgranizeType.Section, departmentGroup.Key + divisionGroup.Key + sectionGroup.Key + "00"),
                            value = sectionGroup.Sum(d => d.EquipmentCount),
                            path = BuildPath(
                                departmentGroup.Key + "000000",
                                departmentGroup.Key + divisionGroup.Key + "0000",
                                departmentGroup.Key + divisionGroup.Key + sectionGroup.Key + "00"
                            ),
                            getJobs = sectionGroup
                                .GroupBy(d => d.JobName)
                                .Select(jobGroup => new GetJob
                                {
                                    name = GetOrganizeName(OrgranizeType.Job, departmentGroup.Key + divisionGroup.Key + sectionGroup.Key + jobGroup.Key),
                                    value = jobGroup.Sum(d => d.EquipmentCount),
                                    path = BuildPath(
                                        departmentGroup.Key + "000000",
                                        departmentGroup.Key + divisionGroup.Key + "0000",
                                        departmentGroup.Key + divisionGroup.Key + sectionGroup.Key + "00",
                                        departmentGroup.Key + divisionGroup.Key + sectionGroup.Key + jobGroup.Key
                                    )
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList()
        })
        .ToList()
            };

            string GetOrganizeName(string type, string code)
            {
                string output = "-";

                output = GetSysOrganizeAll.FirstOrDefault(x => x.TypeName == type && x.Code == code) == null ? "-" : GetSysOrganizeAll.FirstOrDefault(x => x.TypeName == type && x.Code == code).Name;

                return output;
            }

            string BuildPath(params string[] codes)
            {
                return string.Join("/", codes.Select(code => GetOrganizeName(OrgranizeType.Department, code)));
            }

            return output;
        }


        public async Task<List<GetTransKey>> GetByName(string name)
        {
            var query = await _context.TransKeys.Include(a => a.SysOrgranize).Include(a => a.SysKeyType).Include(a => a.TransEquipments).ConvertToListAsync();

            var output = query.ToList();

            var cacheKey = "GetByNameCount";

            _cache.Set(cacheKey, output.Count());

            return output;
        }

        public async Task<List<GetTransKey>> GetBySysKeyTypeId(Guid sysKeyTypeId)
        {
            return await _context.TransKeys.Where(a => a.SysKeyTypeId == sysKeyTypeId).Include(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.TransEquipments).ConvertToListAsync();
        }

        public async Task<List<GetTransKey>> GetBySysOrgranizeId(Guid sysOrgranizeId)
        {
            return await _context.TransKeys.Where(a => a.SysOrgranizeId == sysOrgranizeId).Include(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.TransEquipments).ConvertToListAsync();
        }

        public async Task<List<GetTransKey>> GetFilter(GetTransKeyFilterRequest param)
        {
            var query = _context.TransKeys.Where(a => a.IsActive).Include(a => a.SysKeyType).Include(a => a.SysOrgranize).Include(a => a.TransEquipments).AsQueryable();
            if (!string.IsNullOrEmpty(param.DepartmentCode))
            {
                query = query.Where(a => a.SysOrgranize != null && a.SysOrgranize.DepartmentCode == param.DepartmentCode);
            }

            if (!string.IsNullOrEmpty(param.DivisionCode))
            {
                query = query.Where(a => a.SysOrgranize != null && a.SysOrgranize.DivisionCode == param.DivisionCode);
            }

            if (!string.IsNullOrEmpty(param.SectionCode))
            {
                query = query.Where(a => a.SysOrgranize != null && a.SysOrgranize.SectionCode == param.SectionCode);
            }

            if (!string.IsNullOrEmpty(param.JobCode))
            {
                query = query.Where(a => a.SysOrgranize != null && a.SysOrgranize.JobCode == param.JobCode);
            }

            if (!string.IsNullOrEmpty(param.TypeKeyId))
            {
                query = query.Where(a => a.SysKeyType != null && a.SysKeyType.Id == param.TypeKeyId.ToGuid());
            }

            if (!string.IsNullOrEmpty(param.Status))
            {
                if (param.Status == TransKeyStatus.Use)
                {
                    query = query.Where(a => a.TransEquipments != null && a.TransEquipments.Any());
                }
                else if (param.Status == TransKeyStatus.NotUse)
                {
                    query = query.Where(a => a.TransEquipments == null || !a.TransEquipments.Any());
                }
            }

            if (!string.IsNullOrEmpty(param.TextSearch))
            {
                query = query.Where(a => a.SysKeyType.Name.Contains(param.TextSearch)
                                    || (a.TransEquipments != null && a.TransEquipments.Any() && a.TransEquipments.Any(b => b.IsActive && (b.EquipmentCode.Contains(param.TextSearch)
                                                                                                                        || b.InstallLocation.Contains(param.TextSearch))
                                                                                                                        ))
                                    || a.License.Contains(param.TextSearch));
            }

            var output = await query.CountAsync();

            var cacheKey = "TransKeyCount";

            _cache.Set(cacheKey, output);

            return await query.ConvertToListAsync();
        }

        public async Task<List<TransKey>> GetForUpdate(List<Guid> listId)
        {
            return await _context.TransKeys.Where(a => listId.Contains(a.Id)).Include(a => a.TransKeyHistories).ToListAsync();
        }

        public async Task<TransKey> GetForUpdate(Guid id)
        {
            return await _context.TransKeys.Where(a => a.Id == id).Include(a => a.TransKeyHistories).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TransKey transKey)
        {
            _context.TransKeys.Update(transKey);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<TransKey> list)
        {
            _context.TransKeys.UpdateRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAllAsync(string cacheName)
        {
            // Use a caching library like MemoryCache, Redis, etc.
            var cacheKey = cacheName;
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

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TransKey> GetForUpdate(string name)
        {
            return await _context.TransKeys.Where(a => a.License.Contains(name)).Include(a => a.TransEquipments).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TransKey transKey)
        {
            await _context.TransKeys.AddAsync(transKey);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAll()
        {
            return await _context.TransKeys.Where(a => a.IsActive).CountAsync();
        }
    }
}
