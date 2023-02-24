using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Ten Pieces Weight TV Report DTO
    /// </summary>
   public class TenPcsWeightDTO
    {
        /// <summary>
        /// Line of the Batch
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Size of the Batch
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Min Range of the Line
        /// </summary>
        public decimal MinRange { get; set; }

        /// <summary>
        /// Max Range of the Line
        /// </summary>
        public decimal MaxRange { get; set; }

        /// <summary>
        /// Ten Pieces Weight of the Batch
        /// </summary>
        public decimal TenPCsWeight { get; set; }

        /// <summary>
        /// Location Area of the Line
        /// </summary>
        public string LocationAreaCode { get; set; }

    }
   public class TenPcsWeightReportDTO
   {
       /// <summary>
       /// Line of the Batch
       /// </summary>
       public string Line { get; set; }

       /// <summary>
       /// Size of the Batch
       /// </summary>
       public string Size { get; set; }

       /// <summary>
       /// Min Range of the Line
       /// </summary>
       public decimal MinRange { get; set; }

       /// <summary>
       /// Max Range of the Line
       /// </summary>
       public decimal MaxRange { get; set; }

       /// <summary>
       /// Ten Pieces Weight of the Latest Batch in the Line
       /// </summary>
       public decimal Last1Batch { get; set; }

       /// <summary>
       /// Ten Pieces Weight of the 2nd Latest Batch in the Line
       /// </summary>
       public string Last2Batch { get; set; }

       /// <summary>
       /// Ten Pieces Weight of the 3rd Latest Batch in the Line
       /// </summary>
       public string Last3Batch { get; set; }

       /// <summary>
       /// Ten Pieces Weight of the 4th Latest Batch in the Line
       /// </summary>
       public string Last4Batch { get; set; }

       /// <summary>
       /// Ten Pieces Weight of the 5th Latest Batch in the Line
       /// </summary>
       public string Last5Batch { get; set; }

   }
}
