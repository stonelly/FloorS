using System;
using System.Collections.Generic;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// This class can be used to pass batch information from one layer to other
    /// </summary>
    public class ON2GBatchDTO
    {
        //public ON2GBatchDTO()
        //{
        //    BatchOrders = new List<DropdownDTO>();
        //}
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public string Plant { get; set; }
        public DateTime CurrentDateandTime { get; set; }
        public int UserId { get; set; }
        public int ShiftId { get; set; }
        public string LineId { get; set; }
        public string Resource { get; set; }
        public string GloveCode { get; set; }
        public string Size { get; set; }
        public string BatchOrder { get; set; }
        public int PackingSize { get; set; }
        public int InnerBox { get; set; }
        public decimal TenPcsWeight { get; set; }
        public decimal BatchWeight { get; set; }
        public int WorkStationId { get; set; }
        public string ShiftName { get; set; }
        public string GloveTypeDescription { get; set; }
        public int TotalGloveQty { get; set; }

    }
}
