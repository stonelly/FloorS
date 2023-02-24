using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class InnerLabelSetMasterDTO
    {
        public string Description { get; set; }
        public string InnerLabelSetNo { get; set; }
        public int InnerLabelSetId { get; set; }

        public bool IsDeleted { get; set; }

        public bool SpecialCode { get; set; }

        public string SpecialCodeText { get; set; }
        public Constants.ActionLog ActionType { get; set; }

        public bool CustomDate { get; set; }

        public int Status { get; set; }

        public bool MandatoryCustLotId { get; set; }
    }
}
