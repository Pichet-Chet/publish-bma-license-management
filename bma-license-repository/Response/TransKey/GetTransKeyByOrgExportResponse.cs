using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.TransKey
{
    public class GetTransKeyByOrgExportResponse : ModelMessageAlertResponse
    {
        public MemoryStream Data { get; set; }
    }
}
