using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class BinMasterDTO
    {
        public string BinNumber { get; set; }
 
        public bool IsDeleted { get; set; }
  
        public bool IsAvailable { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        public int LocationId { get; set; }

        public int AreaId { get; set; }
    }
}
