using bma_license_repository.CustomModel.SysMessageConfiguration;
using bma_license_repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace bma_license_repository.CustomModel
{

    public interface ISysMessageConfigurationRepository
    {
        Task<GetMessageConfiguration> GetByCode(string code);
    }

    public class SysMessageConfigurationRepository : ISysMessageConfigurationRepository
    {
        private readonly DevBmaContext _context;

        public SysMessageConfigurationRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<GetMessageConfiguration> GetByCode(string code)
        {
            return await _context.SysMessageConfigurations.Where(a => a.IsActive && a.Code == code)
                .Select(a => new GetMessageConfiguration
                {
                    Code = a.Code,
                    MessageEN = a.MessageEn,
                    MessageTh = a.MessageTh,
                }).FirstOrDefaultAsync();
        }
    }
}
