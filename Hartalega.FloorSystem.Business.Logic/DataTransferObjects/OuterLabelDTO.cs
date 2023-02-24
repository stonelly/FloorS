using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class OuterLabelDTO
    {
        public Boolean isPrintSuccess { get; set; }
        public Boolean isLabelSetDoesnotExists { get; set; }
        public string barcodeToValidate { get; set; }
        public int countToValidate { get; set; }
        public string specialInternalLotNumber { get; set; }
        public DateTime manufacturingdate { get; set; }

        // pang FP vision
        public string outerBarcode_cGs1 { get; set; }

    }
}
