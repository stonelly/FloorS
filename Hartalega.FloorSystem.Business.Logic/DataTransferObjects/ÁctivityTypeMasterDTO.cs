using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class ÁctivityTypeMasterDTO
    {
        public string ActivityType { get; set; }

        public bool IsDeleted { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        public int ActivityTypeId { get; set; }
    }
}
