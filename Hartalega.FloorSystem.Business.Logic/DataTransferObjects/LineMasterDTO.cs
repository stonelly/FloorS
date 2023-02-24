using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class LineMasterDTO : ICloneable
    {
      // public int RecIndex { get; set; }
       public string LineNumber { get; set; }
       public int LocationId { get; set; }
       public bool IsDeleted { get; set; }
       public Constants.ActionLog ActionType { get; set; }
       public object Clone()
       {
           ProductionDefectMasterDTO objqaidto = (ProductionDefectMasterDTO)this.MemberwiseClone();
           return objqaidto;
       }
    }
}
