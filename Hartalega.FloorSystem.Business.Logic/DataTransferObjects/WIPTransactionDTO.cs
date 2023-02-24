using System;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class WIPTransactionDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static WIPTransactionDTO()
        {

        }

        #region WIP Scan Data

        /// <summary>
        /// Row Number
        /// </summary>
        private int _RowNum;

        public int RowNum
        {
            get { return _RowNum; }
            set { _RowNum = value; }
        }

        /// <summary>
        /// Serial Number
        /// </summary>
        private string _SerialNo;

        public string SerialNo
        {
            get { return _SerialNo; }
            set { _SerialNo = value; }
        }

        /// <summary>
        /// Batch Number
        /// </summary>
        private string _BatchNo;

        public string BatchNo
        {
            get { return _BatchNo; }
            set { _BatchNo = value; }
        }

        /// <summary>
        /// Glove Type Name
        /// </summary>
        private string _GloveType;

        public string GloveType
        {
            get { return _GloveType; }
            set { _GloveType = value; }
        }

        /// <summary>
        /// Glove Type Size
        /// </summary>
        private string _GloveSize;

        public string GloveSize
        {
            get { return _GloveSize; }
            set { _GloveSize = value; }
        }

        /// <summary>
        /// Batch Weight
        /// </summary>
        private decimal? _BatchWeight;

        public decimal? BatchWeight
        {
            get { return _BatchWeight; }
            set { _BatchWeight = value; }
        }

        /// <summary>
        /// Ten Pieces   Weight
        /// </summary>
        private decimal? _TenPCsWeight;

        public decimal? TenPCsWeight
        {
            get { return _TenPCsWeight; }
            set { _TenPCsWeight = value; }
        }

        /// <summary>
        /// Total Pieces
        /// </summary>
        private int? _TotalPCs;

        public int? TotalPCs
        {
            get { return _TotalPCs; }
            set { _TotalPCs = value; }
        }

        /// <summary>
        /// WIP Scan Status ID
        /// </summary>
        private int _WIPScanStatusID;

        public int WIPScanStatusID
        {
            get { return _WIPScanStatusID; }
            set { _WIPScanStatusID = value; }
        }

        /// <summary>
        /// WIP Scan Status Name
        /// </summary>
        private string _WIPScanStatusName;

        public string WIPScanStatusName
        {
            get { return _WIPScanStatusName; }
            set { _WIPScanStatusName = value; }
        }

        #endregion

        #region WIP Void Scanned Data

        /// <summary>
        /// Total Batch
        /// </summary>
        private int _TotalBatch;

        public int TotalBatch
        {
            get { return _TotalBatch; }
            set { _TotalBatch = value; }
        }

        /// <summary>
        /// WIP Reference Number
        /// </summary>
        private string _ReferenceNumber;

        public string ReferenceNumber
        {
            get { return _ReferenceNumber; }
            set { _ReferenceNumber = value; }
        }

        /// <summary>
        /// Is Check Flag
        /// </summary>
        private bool _IsCheck = false;

        public bool IsCheck
        {
            get { return _IsCheck; }
            set { _IsCheck = value; }
        }

        #endregion
    }
}
