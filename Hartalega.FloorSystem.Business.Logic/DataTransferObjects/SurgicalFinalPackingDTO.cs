using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class SurgicalFinalPackingDTO
    {
        public string PoNumber { set; get; }
        public string ItemNumber { set; get; }
        public string ItemName { get; set; }

        public string ItemSize { get; set; }
        public int GroupId { get; set; }
        public int BoxesPacked { get; set; }
        public decimal SerialNumberLeft { get; set; }
        public decimal SerialNumberRight { get; set; }
        public string BatchNumberLeft { get; set; }
        public string BatchNumberRight { get; set; }
        public int PalletCapacity { get; set; }
        public int CaseCapacity { get; set; }
        public string palletid { get; set; }
        public int BarcodeVerificationRequired { set; get; }
        public string OrderNumber { set; get; }
        public string CustomerReferenceNumber { get; set; }
        public string CustomerLotNumber { get; set; }
        public string CustomerSize { get; set; }
        public decimal NettWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public string InnerSetLayout { get; set; }
        public string OuterSetLayout { get; set; }
        public string OuterLotNumber { get; set; }
        public string QCGroupName { get; set; }

        public SurgicalFinalPackingDTO()
        {
            BatchOrders = new List<DropdownDTO>();
        }
        /// <summary>
        /// cater multiple Batch Orders generated for the batch(Glove & FG)
        /// </summary>
        public List<DropdownDTO> BatchOrders { get; set; }
    }
}
