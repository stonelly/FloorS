using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hartalega.BarcodeScannerIntegrator.D365Integration
{
    public class BarcodeDeviceContract
    {
        
        public List<string> BarcodeList { get; set; }

        public string ErrorMessage { get; set; }

        public string DataAreaId { get; set; }

        public string WorkStationName { get; set; }

        public ProcessingStatus ProcessingStatus { get; set; }

        public string JournalId { get; set; }

        public string PickingRouteID { get; set; }
    }
}
