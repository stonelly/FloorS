using System;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Made To Stock
    /// </summary>
    public class MadeToStockDTO
    {
        public string AlternateGloveCode1 { get; set; }

        public string AlternateGloveCode2 { get; set; }

        public string AlternateGloveCode3 { get; set; }

        public int SpecialInnerCode { get; set; }

        public string SpecialInnerCharacter { get; set; }

        public string InnerDateFormat { get; set; }

        public string OuterDateFormat { get; set; }

        public string PrintingSize { get; set; }

        public decimal GrossWeight { get; set; }

        public decimal NetWeight { get; set; }

        public int InnerboxinCaseNo { get; set; }

        public string HartalegaCommonSize { get; set; }

        public int GlovesInnerboxNo { get; set; }

        public string InnerProductCode { get; set; }

        public string OuterProductCode { get; set; }

        public string Reference1 { get; set; }

        public string Reference2 { get; set; }

        public string DOTCustomerLotID { get; set; }

        public string MadeToStockStatus { get; set; }

        public int LotVerification { get; set; }

        public int PreShipmentPlan { get; set; }

        public string InnerLabelSet { get; set; }

        public string OuterLabelSetNo { get; set; }

        public int PalletCapacity { get; set; }

        public int ManufacturingDateOn { get; set; }

        public int Expiry { get; set; }

        public int GCLabel { get; set; }

        public string WarehouseId { get; set; }
    }
}
