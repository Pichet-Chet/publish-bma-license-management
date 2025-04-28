using bma_license_repository.Response.MessageAlert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bma_license_repository.Response.Master
{
    public class UseDropDownResponse : ModelMessageAlertResponse
    {
        public List<UseDropDownResponseData> Data {  get; set; }
    }

    public class UseDropDownResponseData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value {  get; set; }
    }
}
