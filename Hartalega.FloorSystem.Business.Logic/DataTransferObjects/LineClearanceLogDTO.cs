using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class LineClearanceLogDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static LineClearanceLogDTO()
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
        /// Screen Name
        /// </summary>
        private string _ScreenName;

        public string ScreenName
        {
            get { return _ScreenName; }
            set { _ScreenName = value; }
        }

        /// <summary>
        /// Authorised By
        /// </summary>
        private string _AuthorisedBy;

        public string AuthorisedBy
        {
            get { return _AuthorisedBy; }
            set { _AuthorisedBy = value; }
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
        /// WorkStation ID
        /// </summary>
        private int _WorkStationID;

        public int WorkStationID
        {
            get { return _WorkStationID; }
            set { _WorkStationID = value; }
        }
    }
}
