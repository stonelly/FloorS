using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Post Treatment DTO
    /// </summary>
   public class PostTreatmentDTO
    {
        /// <summary>
        /// Serial Number generated for the batch
        /// </summary>
        public decimal SerialNumber { get; set; }
        /// <summary>
        /// Shift when batch will be scanned in
        /// </summary>
        public int Shift { get; set; }
        /// <summary>
        ///Rework Reason for the Batch
        /// </summary>
        public string ReworkReasonId { get; set; }
        /// <summary>
        ///Rework Process for the Batch
        /// </summary>
        public string ReworkProcess { get; set; }
        /// <summary>
        ///Rework Count for the Batch
        /// </summary>
        public int ReworkCount { get; set; }
        /// <summary>
        ///Current Location of the user
        /// </summary>
        public int LocationId { get; set; }
        /// <summary>
        ///Last Modified Date of the Batch
        /// </summary>
        public DateTime LastModifiedOn { get; set; }
        /// <summary>
        ///Ten Pieces Weight of the Batch
        /// </summary>
        public Decimal TenPcsWeight { get; set; }
        /// <summary>
        ///Total Batch Weight
        /// </summary>
        public Decimal BatchWeight { get; set; }
        /// <summary>
        ///New Glove Type of the Batch
        /// </summary>
        public string ChangeGloveType { get; set; }
        /// <summary>
        ///Old Glove Type of the Batch
        /// </summary>
        public string OldGloveType { get; set; }
        /// <summary>
        ///Workstation Number of the user
        /// </summary>
        public string WorkstationNumber { get; set; }
        /// <summary>
        /// Used for checking whether we are updating the existing row or inserting a new row
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id of the Dryer Number
        /// </summary>
        public int DryerId { get; set; }

        /// <summary>
        /// Authorized for
        /// </summary>
        public int AuthorizedFor { get; set; }

        /// <summary>
        /// #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
        /// </summary>
        public string BatchOrder { get; set; }

        public string OldBatchOrder { get; set; }
    }
}
