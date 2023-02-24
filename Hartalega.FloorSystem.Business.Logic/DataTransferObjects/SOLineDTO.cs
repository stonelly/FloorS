using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class SOLineDTO
    {
        public string InventTransId { set; get;}
        public string PONumber { set; get; }
        public string BrandName { set; get; }
        public string CustomerSpecification { set; get; }
        public string CustomerRefernceNumber { set; get; }
        public int BarcodeVerificationRequired { set; get; }
        public string GloveCode { set; get;}
        public int PreshipmentPlan { set; get; }
        public string ItemNumber { set; get; }
        public string ItemNumberDisplay { set; get; }
        public string ItemSize { set; get; }
        public int ItemCases { set; get; }
        public string InnersetLayout { set; get; }
        public string OuterSetLayout { set; get; }
        public string InnerLabelSetDateFormat { set; get; } // yiksiu SEP 2017: Label Set Optimization
        public string OuterLabelSetDateFormat { set; get; } // yiksiu SEP 2017: Label Set Optimization
        public string CustomerSize { set; get; }
        public string CustomerSizeDesc { set; get; }
        public decimal GrossWeight { set; get; }
        public decimal NettWeight { set; get; }
        public int CaseCapacity { set; get; }
        public int PalletCapacity { set; get; }
        public int InnerBoxCapacity { set; get; }
        public string CustomerLotNumber { set; get; }        
        public string ItemName { set; get; }
        public int ManufacturingOrderBasis { set; get; }
        public int POStatus { set; get; }
        public int locationID { get; set; }
        public int SpecialInnerCode {get; set;}
        public string SpecialInnerCodeCharacter { get; set;}
        public int Expiry { get; set; }
        public int ManufacturingDateBasis { get; set; }
        public string InnerProductCode { get; set; }
        public string OuterProductCode { get; set;}
        
        public string BrandBarcode { get; set; }
        public int GCLabelPrintingRequired { get; set; }
        public string AlternateGloveCode1{get; set;}
        public string  AlternateGloveCode2 {get; set;}
        public string AlternateGloveCode3 { get; set; }
        public string CustomerName { get; set;}
        public string packingdate { get; set; }
        public string productiondate { get; set; }
        public DateTime SHIPPINGDATEREQUESTED { get; set; }
        public DateTime RECEIPTDATEREQUESTED { get; set; }
        public int ItemType { get; set; }
        public string Preshipmentcases { get; set; }
        public string OrderNumber { get; set; }
        public string Reference1 { get; set; }
        //public string Reference2 { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string Reference2 { get; set; }

        public string CompanyCode { get; set; }
        public string BatchOrder { get; set; }
        public string ProdStatus { get; set; }

        //KahHeng 28Jan2019 added get-set method for PODate and POReceivedDate
        public DateTime CustPODocumentDate { get; set; }
        public DateTime CustPORecvDate { get; set; }
        //KahHeng End
        //Pang YS - FP VISION
        public string BARCODE { get; set; }
        public string BARCODEOUTERBOX { get; set; }

        public string VSReceiptFilePath { get; set; }


        public DateTime ShippingDateETD { get; set; }
        public DateTime ManufacturingDateETD { get; set; }
    }
}
