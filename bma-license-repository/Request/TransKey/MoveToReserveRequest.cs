
using System.ComponentModel.DataAnnotations;
using bma_license_repository.CustomModel.Model;

namespace bma_license_repository.Request.TransKey
{
    public class MoveToReserveRequest
    {
        [ValidGuid(ErrorMessage = "Invalid Guid Format")]
        public string TransKeyId { get; set; }
    }
}
