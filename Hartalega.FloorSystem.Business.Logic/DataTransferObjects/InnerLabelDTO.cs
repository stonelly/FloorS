using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class InnerLabelDTO
    {
        public Boolean isPrintSuccess { get; set; }
        public string customerinternallotnumber { get; set; }
        public Boolean isLabelSetDoesnotExists { get; set; }
    }
}
