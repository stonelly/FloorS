using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class ChangeLogDTO : ICloneable
    {
        public string TableName { get; set; }
        public string UserId { get; set; }
        public List<ColumnChange> UpdateColumns { get; set; }

        public string WorkstationId { get; set; }
        public object Clone()
        {
            ChangeLogDTO objqaidto = (ChangeLogDTO)this.MemberwiseClone();
            return objqaidto;
        }
    }

    public class ColumnChange : ICloneable
   {
       public string OldValue { get; set; }
       public string NewValue { get; set; }
       public string ColumnName { get; set; }

       public object Clone()
       {
           ColumnChange objqaidto = (ColumnChange)this.MemberwiseClone();
           return objqaidto;
       }
   }


    public class EventLogDetail : ICloneable
    {
        public string Source { get; set; }
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }

        public object Clone()
        {
            ColumnChange objqaidto = (ColumnChange)this.MemberwiseClone();
            return objqaidto;
        }
    }
}
