using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FinalPackingDTO
    {
        public int locationId { set; get; }
        public string Workstationnumber { set; get; }
        public string Printername { set; get; }
        public DateTime Packdate { set; get; }
        public string Outerlotno { set; get; }
        public string Internallotnumber { set; get; }
        public string Ponumber { set; get; }
        public string ItemNumber { set; get; }
        public string Size { set; get; }
        public string GloveType { set; get; }
        public int GroupId { set; get; }
        public string QCGroupName { set; get; }
        public decimal Serialnumber { set; get; }
        public string listSecondGradeSerialNumber { set; get; }
        public int Boxespacked { set; get; }
        public string Palletid { set; get; }
        public int Casespacked { set; get; }
        public string PreshipmentPLTId { set; get; }
        public int Preshipmentcases { set; get; }
        public int OperatorId { set; get; }
        public string Innersetlayout { set; get; }
        public string Outersetlayout { set; get; }
        public string ItemName { set; get; }
        public string BatchNumber { set; get; }
        public int CaseNumber { set; get; }
        public int palletCapacity { set; get; }
        public int TotalPcs { set; get; }
        public Boolean isTempPack { set; get; }
        public DateTime? ManufacturingDate { set; get;}
        public DateTime? ExpiryDate { set; get; }
        public string orderNumber { get; set; }
        public string customerLotNumber { get; set;}
        public string stationNumber { get; set; }
        public string BarcodetoValidate { get; set; }
        public int CounttoValidate { get; set; }
        public int Barcodevalidationrequired { get; set;}
        public string CustomerReferenceNumber { get; set; }
        public string InventTransId { get; set;}
        public string PSIReworkOrderNo { get; set; }
        public string FGBatchOrderNo { get; set; }
        public string Resource { get; set; }
        public string PSIStatus { get; set; } //#AZRUL 16-10-2018 BUGS 1207: Prompt message if PSI Rework Order not started.
        public string FPStationNo { set; get; } //#Azrul 08/10/2020 Surgical Packing Plan: Merged from HSBPackage03

        public int caseCapacity { set; get; }
        public int innerBoxCapacity { set; get; }
    }
}
