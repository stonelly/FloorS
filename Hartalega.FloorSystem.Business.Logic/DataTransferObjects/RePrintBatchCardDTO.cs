
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class RePrintBatchCardDTO
    {
        #region constructors

        public RePrintBatchCardDTO(string batchNumber, string serialNumber, string reprintDateTime, string processArea, string printDatetime, string operatorName, string reasonText, string plant, string printType)
        {
            BatchNumber = batchNumber;
            SerialNumber = serialNumber;
            ReprintDateTime = reprintDateTime;
            ProcessArea = processArea;
            PrintDatetime = printDatetime;
            OperatorName = operatorName;
            ReasonText = reasonText;
            Plant = plant;
            PrintType = printType;
        }

        #endregion

        public string BatchNumber { get; set; }
        public string SerialNumber { get; set; }
        public string PrintDatetime { get; set; }
        public string Plant { get; set; }
        public string ProcessArea { get; set; }
        public string ReprintDateTime { get; set; }
        public string OperatorName { get; set; }
        public string ReasonText { get; set; }
        public string PrintType { get; set; }
    }
}
