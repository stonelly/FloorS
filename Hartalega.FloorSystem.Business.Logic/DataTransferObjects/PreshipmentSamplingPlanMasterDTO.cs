using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
   public class PreshipmentSamplingPlanMasterDTO
    {
        public int PreshipmentSamplingPlanMasterID { get; set; }
        public string PreshipmentSamplingPlan { get; set; }

        public string PreshipmentDescription { get; set; }
        public bool IsDeleted { get; set; }

        public Constants.ActionLog ActionType { get; set; }
    }
}
