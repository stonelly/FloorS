using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
   public class QCTypeMasterDTO
    {
        public string QCType { get; set; }
        public string Description { get; set; }

        public int QCTypeId { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        
        public int Stopped { get; set; }

        public int QCEfficiency{ get; set; }
    }
}
