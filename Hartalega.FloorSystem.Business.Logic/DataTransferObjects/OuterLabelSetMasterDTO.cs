using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class OuterLabelSetMasterDTO
    {
        public string Description { get; set; }
        public string OuterLabelSetNo { get; set; }
        public int OuterLabelSetId { get; set; }

        public bool IsDeleted { get; set; }

        public bool GCLabel { get; set; }
         
        public Constants.ActionLog ActionType { get; set; }

        public int Status { get; set; }

        public bool CustomDate { get; set; }

        public bool MandatoryCustLotId { get; set; }

    }
}
