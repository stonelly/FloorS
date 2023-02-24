using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct DOCINFO
{
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pDocName;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pOutputFile;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pDataType;
}

namespace Hartalega.FloorSystem.Framework
{
    public class DirectPrinter
    {
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long ClosePrinter(IntPtr hPrinter);

        /// <summary>
        /// This method is used to print data through printer
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="PCL5Commands"></param>
        /// <param name="printerName"></param>
        public static void SendToPrinter(string jobName, string PCL5Commands, string printerName)
        {
            System.IntPtr lhPrinter = new System.IntPtr();

            DOCINFO di = new DOCINFO();
            int pcWritten = 0;
            di.pDocName = jobName;
            di.pDataType = "RAW";
            StartDocPrinter(lhPrinter, 1, ref di);
            StartPagePrinter(lhPrinter);
            WritePrinter(lhPrinter, PCL5Commands, PCL5Commands.Length, ref pcWritten);
            EndPagePrinter(lhPrinter);
            EndDocPrinter(lhPrinter);
            ClosePrinter(lhPrinter);
        }
    }
}
