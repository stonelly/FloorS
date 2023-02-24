using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class LocationMasterDTO
    {
        public string LocationName { get; set; }
        public string LocationAreaCode { get; set; }

        public int LocationId { get; set; }
        public bool IsDeleted { get; set; }
        public Constants.ActionLog ActionType { get; set; }
    }
}
