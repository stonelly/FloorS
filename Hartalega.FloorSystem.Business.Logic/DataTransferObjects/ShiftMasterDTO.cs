using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
   public class ShiftMasterDTO
    {
        public string Name { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }

        public string GroupType { get; set; }


        public int GroupTypeId { get; set; }
        public int ShiftId { get; set; }

        public Constants.ActionLog ActionType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
