
using bma_license_repository.Dto;
using bma_license_repository.Helper;

namespace bma_license_repository
{
    public interface IMasterRepository
    {
        Task<List<string>> GetColumnExportAgency();
    }
    public class MasterRepository: IMasterRepository
    {
        private readonly DevBmaContext _context;
        public MasterRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetColumnExportAgency()
        {
           var result = new List<string>();
            result.Add(ColumnExportAgency.AllocationAgency);
            result.Add(ColumnExportAgency.License);
            result.Add(ColumnExportAgency.LicenseType);
            result.Add(ColumnExportAgency.CountInstall);
            result.Add(ColumnExportAgency.MachineType);
            result.Add(ColumnExportAgency.MachineNumber);
            result.Add(ColumnExportAgency.MacAddress);
            result.Add(ColumnExportAgency.Equipment);
            result.Add(ColumnExportAgency.MachineName);
            result.Add(ColumnExportAgency.Brand);
            result.Add(ColumnExportAgency.Generation);
            result.Add(ColumnExportAgency.IpAddress);
            result.Add(ColumnExportAgency.InstallLocation);
            result.Add(ColumnExportAgency.InstallDate);
            result.Add(ColumnExportAgency.Remark);

            return result;
        }

    }
}
