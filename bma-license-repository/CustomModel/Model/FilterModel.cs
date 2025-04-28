

namespace bma_license_repository.CustomModel.Model
{
    public class FilterModel
    {
        public string? TextSearch { get; set; }

        public bool IsAll { get; set; } = false;

        const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int PageSizeDefault = 10;

        public string? SortName { get; set; }

        public string? SortType { get; set; }

        public int PageSize
        {
            get
            {
                return PageSizeDefault;
            }
            set
            {
                PageSizeDefault = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}
