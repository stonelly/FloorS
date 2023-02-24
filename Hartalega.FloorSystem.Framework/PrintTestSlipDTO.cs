using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace Hartalega.FloorSystem.Framework
{
    /// <summary>
    /// This class can be used for printing the Test Slip
    /// </summary>
    public class PrintTestSlipDTO
    {
        /// <summary>
        /// Test Slip Generation Date Time
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// Rework Count of the batch
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        ///  Serial Number of the batch
        /// </summary>      
        public string SerialNumber { get; set; }

        /// <summary>
        ///  Batch Weight of the batch
        /// </summary>      
        public string BatchWeight { get; set; }

        /// <summary>
        ///  Hundred Pieces Weight of the batch
        /// </summary>      
        public string HundredPcsWeight { get; set; }

        /// <summary>
        /// Reference Id of the Batch
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Tester Name
        /// </summary>
        public string TesterName { get; set; }

        /// <summary>
        /// Tester Id
        /// </summary>
        public string TesterId { get; set; }

        /// <summary>
        /// Size of the Batch
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Glove type of the Batch
        /// </summary>
        public string GloveType { get; set; }

        /// <summary>
        /// Name of the Test Slip
        /// </summary>
        public string TestSlipName { get; set; }

        /// <summary>
        /// Rework Count
        /// </summary>
        public string ReworkCount { get; set; }

        /// <summary>
        /// PT Date Time
        /// </summary>
        public string PTDate { get; set; }

        /// <summary>
        /// Washer Program of the Batch
        /// </summary>
        public string WasherProgram { get; set; }

        /// <summary>
        /// Dryer Program of the Batch
        /// </summary>
        public string DryerProgram { get; set; }

        /// <summary>
        /// Washer Number of the Batch
        /// </summary>
        public string WasherNumber { get; set; }

        /// <summary>
        /// Dryer Number of the Batch
        /// </summary>
        public string DryerNumber { get; set; }

        /// <summary>
        /// Image of barcode generated
        /// </summary>
        public Metafile Bmp { get; set; }

        /// <summary>
        /// Image of Reference Id generated
        /// </summary>
        public Metafile BmpReference { get; set; }

    }
}
