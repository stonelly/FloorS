using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class LotVerificationLogDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static LotVerificationLogDTO()
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
        /// Pallet ID
        /// </summary>
        private string _PalletID;

        public string PalletID
        {
            get { return _PalletID; }
            set { _PalletID = value; }
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

    }
}
