
using bma_license_repository.Dto;

namespace bma_license_repository.CustomModel.TransKey
{
	public class GetTopOrganizationModel
	{
		
    }

    public class GetCountTop10
    {
        public List<SysOrgranizeCountDto> TopOrgranizes { get; set; }
    }

    public class SysOrgranizeCountDto
    {
        public string OrgranizeName { get; set; }
        public int TransKeyCount { get; set; }
    }
}

