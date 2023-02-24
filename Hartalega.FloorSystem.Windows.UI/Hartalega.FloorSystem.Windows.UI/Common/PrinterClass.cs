using System;
using System.Collections.Generic;
using System.Management;

namespace Hartalega.FloorSystem.Windows.UI.Common
{
    /// <summary>
    /// Printer Details
    /// </summary>
    public class PrinterClass
    {
        #region Private Variables
        private static List<PrinterProperties> PrinterList { get; set; }
        #endregion

        #region Public Variables
        /// <summary>
        /// Is Printer connected
        /// </summary>
        public static bool IsPrinterExists { get; set; }
        #endregion

        #region Private Methods
        private static void Getprinters()
        {
            PrinterList = new List<PrinterProperties>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            foreach (ManagementObject printer in searcher.Get())
            {
                PrinterList.Add(new PrinterProperties() { PrinterName = printer["Name"].ToString().ToLower(), Default = PrintProps(printer, "Default"), Status = PrintProps(printer, "PrinterStatus"), Local = PrintProps(printer, "Local"), IsConnected = PrintProps(printer, "Availability") });
                string printerName = printer["Name"].ToString().ToLower();

                #region "Printer Other Properties"
                #endregion
            }
        }
        
        static string PrintProps(ManagementObject o, string prop)
        {
            return Convert.ToString(o[prop]);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// To check default Printer added or not
        /// </summary>
        /// <returns></returns>
        public static void DefeaultPrinterConnected()
        {
            Getprinters();
            foreach (PrinterProperties pp in PrinterList)
            {
                if (Convert.ToBoolean(pp.Default))
                {
                    IsPrinterExists = true;
                    return;
                }
            }
        }
        #endregion
    }
       
    public class PrinterProperties
    {
        public string PrinterName { get; set; }
        public string Default { get; set; }
        public string Status { get; set; }
        public string Local { get; set; }
        public string IsConnected { get; set; }
    }
}
