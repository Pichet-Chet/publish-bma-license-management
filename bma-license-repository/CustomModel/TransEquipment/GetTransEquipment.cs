using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bma_license_repository.CustomModel.TransEquipment
{
    public class GetTransEquipment
    {
        public Guid Id { get; set; }
        public Guid TransKeyId { get; set; }
        public string EquipmentCode {  get; set; }
        public string MachineType {  get; set; }
        public string MachineName { get; set; }
        public string MachineNumber { get; set; }
        public string MacAddress { get; set; }
        public string Brand { get; set; }
        public string Generation { get; set; }
        public string IpAddress { get; set; }
        public DateTime? InstallDate { get; set; }
        public string InstallLocation { get; set; }
        public string Remark { get; set; }

    }
}
