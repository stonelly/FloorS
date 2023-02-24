using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
   public class AuditLogDTO
    {
        public string WorkstationId { get; set; }

        public string FunctionName { get; set; }

        public int AuditAction { get; set; }

        public string SourceTable { get; set; }

        public string ReferenceId { get; set; }

        public List<ColumnChange> UpdateColumns { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

    }
}
