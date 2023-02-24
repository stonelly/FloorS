using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class BatchTypeMasterDTO
    {
        public string BatchType { get; set; }
        public int BatchTypeId { get; set; }

        public string Description { get; set; }
        public Constants.ActionLog ActionType { get; set; }

        public bool IsDeleted { get; set; }
    }
}
