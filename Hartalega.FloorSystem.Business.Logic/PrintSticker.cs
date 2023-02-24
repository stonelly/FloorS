using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
    /// <summary>
    /// This class is used to print Reject and 2nd Grade Sticker
    /// </summary>
    public class PrintSticker
    {
        /// <summary>
        ///  Print Production Reject Sticker label From Reject Glove screen in Tumbling. 
        /// </summary>
        /// <param name="ModifiedOn">Date when Reject Sticker is printed</param>
        /// <param name="SerialNumber">Serial Number of Rejected Gloves </param>
        /// <param name="BatchNumber">Batch Number of Rejected Gloves</param>
        /// <param name="BatchWeight">Batch Weight of Batch</param>
        /// <param name="GloveDesc">GloveDesc of Batch</param>
        /// <param name="Barcode">Barcode of Serial Number</param>

        /// <returns>returns Boolean</returns>   
        public static Boolean ProductionRejectStickerPrint(DateTime dateTime, string SerialNumber, string batchNumber, decimal BatchWeight, string GloveDesc, Bitmap Barcode)
        {
            //Implementation goes here
            return true;
        }

        /// <summary>
        ///  Print Second Grade Sticker label From Second Grade in Tumbling. 
        /// </summary>
        /// <param name="ModifiedOn">Date when Reject Sticker is printed</param>
        /// <param name="SerialNumber">Serial Number of Rejected Gloves </param>
        /// <param name="BatchNumber">Batch Number of Rejected Gloves</param>
        /// <param name="BatchWeight">Batch Weight of Batch</param>
        /// <param name="TenPcsWeight">TenPcs Weight of Batch</param>
        /// <param name="GloveDesc">GloveDesc of Batch</param>
        /// <param name="Barcode">Barcode of Serial Number</param>

        /// <returns>returns Boolean</returns>   
        public static Boolean SecondGradeStickerPrint(DateTime dateTime, string SerialNumber, string batchNumber, decimal BatchWeight, string GloveDesc, decimal tenPcsWeight, Bitmap Barcode)
        {
            //Implementation goes here
            return true;
        }
    }
}
