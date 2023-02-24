using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
   public class EventLogDTO
    { 
        public int EventType { get; set; }
       public string EventLogType { get; set; }
          
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string EventLogData { get; set; }
         
    }
}
