using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class DefectiveGloveMasterDTO
    {
        public int DefectiveGloveId { get; set; }
        public string DefectiveGloveType { get; set; }

        public string DefectiveGloveReason { get; set; }

        public bool TVConfigurable { get; set; }

        public bool IsDeleted { get; set; }
        public Constants.ActionLog ActionType { get; set; }
    }
}
