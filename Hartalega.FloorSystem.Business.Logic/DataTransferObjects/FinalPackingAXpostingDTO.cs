using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FinalPackingAXpostingDTO
    {
        public DateTime Packdate { set; get; }
        public string Outerlotno { set; get; }
        public string Internallotnumber { set; get; }
        public string Ponumber { set; get; }
        public string ItemNumber { set; get; }
        public string Size { set; get; }
        public string SerialNumber { set; get; }
        public string BatchNumber { get; set; }
        public int Boxespacked { set; get; }
        public DateTime BatchCardDate { set; get; }
        public int Casespacked { set; get; }
        public int Preshipmentcases { set; get; }
        public DateTime ManufacturingDate { set; get; }
        public DateTime ExpiryDate { set; get; }
        public string orderNumber { get; set; }
        public string customerLotNumber { get; set; }

        public string CustomerReferenceNumber { get; set; }
        //For SMBP
        public int InnerBoxCapacity { get; set; }

        public string RefSerialNumber1 { set; get; }
        public string RefSize1 { set; get; }
        public int RefNumberofPieces1 { set; get; }
        public string RefBatchNumber1 { set; get; }
        public int BoxesPacked1 { set; get; }
        public string RefItemNumber1 { get; set; }

        public string RefSerialNumber2 { set; get; }
        public string RefSize2 { set; get; }
        public int RefNumberofPieces2 { set; get; }
        public string RefBatchNumber2 { set; get; }
        public int BoxesPacked2 { set; get; }
        public string RefItemNumber2 { get; set; }

        public string RefSerialNumber3 { set; get; }
        public string RefSize3 { set; get; }
        public int RefNumberofPieces3 { set; get; }
        public string RefBatchNumber3 { set; get; }
        public int BoxesPacked3 { set; get; }
        public string RefItemNumber3 { get; set; }

        public string RefSerialNumber4 { set; get; }
        public string RefSize4 { set; get; }
        public int RefNumberofPieces4 { set; get; }
        public string RefBatchNumber4 { set; get; }
        public int BoxesPacked4 { set; get; }
        public string RefItemNumber4 { get; set; }

        public string RefSerialNumber5 { set; get; }
        public string RefSize5 { set; get; }
        public int RefNumberofPieces5 { set; get; }
        public string RefBatchNumber5 { set; get; }
        public int BoxesPacked5 { set; get; }
        public string RefItemNumber5 { get; set; }

        public string oldSerialNumber { set; get; }
        public string newSerialNumber { set; get; }
        public int TotalPieces { set; get; }

        public string LocationName { set; get; }
        public string BatchOrder { get; set; }
        public string PalletId { get; set; }
        public string Resource { get; set; }
    }
}
