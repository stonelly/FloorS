using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FP_ExemptValidateDTO
    {
        public string Result { get; set; }
        public int ProductionDateValidationDays { get; set; }
        public string ProductionDateValidationCustomer { get; set; }
    }
}
