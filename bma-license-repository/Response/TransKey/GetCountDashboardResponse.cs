
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetCountDashboardResponse : ModelMessageAlertResponse
    {
        public GetCountDashboardResponseData Data { get; set; }

    }

    public class GetCountDashboardResponseData
    {
        public GetCountDashboardResponseData()
        {
            CountUse = 0;
            UsePercnetage = 0;
            CountNotUse = 0;
            NotUsePercnetage = 0;
            CountAll = 0;
            AllPercentage = 0;

        }

        public int CountUse { get; set; }
        public decimal UsePercnetage { get; set; }
        public int CountNotUse { get; set; }
        public decimal NotUsePercnetage { get; set; }
        public int CountAll { get; set; }
        public decimal AllPercentage { get; set; }
        public int CountAllocate { get; set; }
        public decimal AllocatePercentage { get; set; }
        public int CountNotAllocate { get; set; }
        public decimal NotAllocatePercentage { get; set; }
    }

    public class GetAllocateOverview
    {
        public GetAllocateOverview()
        {
            getDepartments = new List<GetDepartment>();
        }

        public List<GetDepartment> getDepartments { get; set; }
    }


    public class GetDepartment
    {
        public GetDepartment()
        {
            getDivisions = new List<GetDivision>();
        }

        public int value { get; set; }

        public string? name { get; set; }

        public string? path { get; set; }

        public List<GetDivision> getDivisions { get; set; }
    }

    public class GetDivision
    {
        public GetDivision()
        {
            getSections = new List<GetSection>();
        }

        public int value { get; set; }

        public string? name { get; set; }

        public string? path { get; set; }

        public List<GetSection> getSections { get; set; }
    }

    public class GetSection
    {
        public GetSection()
        {
            getJobs = new List<GetJob>();
        }
        public int value { get; set; }

        public string? name { get; set; }

        public string? path { get; set; }

        public List<GetJob> getJobs { get; set; }
    }

    public class GetJob
    {
        public int value { get; set; }

        public string? name { get; set; }

        public string? path { get; set; }
    }

}
