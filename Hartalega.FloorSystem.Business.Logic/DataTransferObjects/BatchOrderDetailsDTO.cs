using System;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Batch Order Details
    /// </summary>
    public class BatchOrderDetailsDTO
    {
        public string BthOrderId { get; set; }
        public string ItemId { get; set; }
        public string Size { get; set; }
        public int ReportedQty { get; set; }
        public string Resource { get; set; }
        public DateTime BatchCarddate { get; set; }
        public string BatchNumber { get; set; }
        public string GloveType { get; set; }
        public string InnerBox { get; set; }
        public string Line { get; set; }
        public string LineId { get; set; }
        public DateTime OutputTime { get; set; }
        public string sOutputDate { get; set; }
        public string sOutputTime { get; set; }
        public string PackingSize { get; set; }
        public string TotalGloveQty { get; set; }
        public string ResourceId { get; set; }
        public string SerialNumber { get; set; }
        public int PackSize { get; set; }
        public int InBox { get; set; }
        public string TierSide { get; set; }
        public string Reprint { get; set; }

        //#Azrul 19/10/2020: Surgical Glove System Added
        public int Quantity { get; set; }
        public decimal BatchWeight { get; set; }
        public string GloveCategory { get; set; }
    }
}
