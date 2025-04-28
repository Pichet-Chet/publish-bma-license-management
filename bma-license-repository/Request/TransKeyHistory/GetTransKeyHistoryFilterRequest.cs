

using bma_license_repository.CustomModel.Model;
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.TransKeyHistory
{
    public class GetTransKeyHistoryFilterRequest : FilterModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name {  get; set; }
    }
}
