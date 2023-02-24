using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Production Line Activity DTO
    /// </summary>
    public class ProductionLineActivityDTO
    {
        public int ProductionLineActivityId { get; set; }
        public string Line { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string ActivityType { get; set; }
        public string ActivityDetails { get; set; }
    }

    public class ProductionLineActivityExcel
    {
        public string Line { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string ActivityType { get; set; }
        public string ActivityDetails { get; set; }
    }
}
