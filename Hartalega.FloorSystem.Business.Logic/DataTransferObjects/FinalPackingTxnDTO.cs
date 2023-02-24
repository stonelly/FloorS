using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class FinalPackingTxnDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static FinalPackingTxnDTO()
        {

        }

        /// <summary>
        /// PO Number
        /// </summary>
        private string _PONumber;

        public string PONumber
        {
            get { return _PONumber; }
            set { _PONumber = value; }
        }

        /// <summary>
        /// Size
        /// </summary>
        private string _Size;

        public string Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        /// Item Number
        /// </summary>
        private string _ItemNumber;

        public string ItemNumber
        {
            get { return _ItemNumber; }
            set { _ItemNumber = value; }
        }

        /// <summary>
        /// Lot Number
        /// </summary>
        private string _LotNumber;

        public string LotNumber
        {
            get { return _LotNumber; }
            set { _LotNumber = value; }
        }

        /// <summary>
        /// Lot Number Scanned
        /// </summary>
        private string _LotNumberScanned;

        public string LotNumberScanned
        {
            get { return _LotNumberScanned; }
            set { _LotNumberScanned = value; }
        }

        /// <summary>
        /// Created Date Time
        /// </summary>
        private DateTime _CreatedDateTime;

        public DateTime CreatedDateTime
        {
            get { return _CreatedDateTime; }
            set { _CreatedDateTime = value; }
        }

        /// <summary>
        /// Scanner
        /// </summary>
        private int _Scanner;

        public int Scanner
        {
            get { return _Scanner; }
            set { _Scanner = value; }
        }

        /// <summary>
        /// Is Mismatch
        /// </summary>
        private bool _IsMismatch;

        public bool IsMismatch
        {
            get { return _IsMismatch; }
            set { _IsMismatch = value; }
        }

        /// <summary>
        /// IsEmail
        /// </summary>
        private bool _IsEmail;

        public bool IsEmail
        {
            get { return _IsEmail; }
            set { _IsEmail = value; }
        }

        /// <summary>
        /// Carton No
        /// </summary>
        private int _CartonNo;

        public int CartonNo
        {
            get { return _CartonNo; }
            set { _CartonNo = value; }
        }

        /// <summary>
        /// Scanner Misscanned Count
        /// </summary>
        private int _ScnMisscannedCount;

        public int ScnMisscannedCount
        {
            get { return _ScnMisscannedCount; }
            set { _ScnMisscannedCount = value; }
        }
    }
}
