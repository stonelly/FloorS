using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class SurgicalPouchPrintingV2DTO
    {

    }


    public class SurgicalPackingPlanDTO
    {
        public Guid SurgicalPackingPlanId { get; set; }
        public string SPPSerialNo { get; set; }
        public string PONumber { get; set; }
        public string ItemNumber { get; set; }
        public string ItemBrand { get; set; }
        public string ItemSize { get; set; }
        public string InternalLotNo { get; set; }
        public int TotalCases { get; set; }
        public int PlannedCases { get; set; }
        public int PlanStatus { get; set; }
        public string GloveCode { get; set; }
        public string GloveSize { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public int CaseCapacity { get; set; }
        public int InnerBoxCapacity { get; set; }
        public int SamplingPcsQty { get; set; }
        public int RequiredPcsQty { get; set; }
        public int StartCarton { get; set; }
        public int EndCarton { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int WorkstationId { get; set; }

        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class SurgicalPackingSNDetailsDTO
    {
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public int ReservedQty { get; set; }
        public string GloveSide { get; set; }
    }
}
