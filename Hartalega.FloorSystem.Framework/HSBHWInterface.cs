using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Hartalega.FloorSystem.Framework
{
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
    #region unused param
    /*
    public struct Param
    {
        string comPort;
        int readSec;
        int baudRate;
        int dataBit;
        Parity parity;
        StopBits stopBits;
        int basket;
        string labelPrint;
        string lptPort;

        public Param(string _comPort = "", int _baudRate = 9600, int _readSec = 3, int _dataBit = 8, Parity _parity = Parity.None, StopBits _stopBits = StopBits.One, int _basket = 0, string _lblPrint = "", string _lptPort = "")
        {
            this.comPort = _comPort;
            this.baudRate = _baudRate;
            this.readSec = _readSec;
            this.dataBit = _dataBit;
            this.parity = _parity;
            this.stopBits = _stopBits;
            this.basket = _basket;
            this.labelPrint = _lblPrint;
            this.lptPort = _lptPort;
        }

        public string ComPort
        {
            get { return this.comPort; }
            set { this.comPort = value; }
        }

        public int ReadSec
        {
            get { return readSec; }
            set { readSec = value; }
        }

        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        public int Basket
        {
            get { return basket; }
            set { basket = value; }
        }

        public int DataBit
        {
            get { return dataBit; }
            set { dataBit = value; }
        }

        public Parity Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        public StopBits StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }

        public string LabelString
        {
            get { return labelPrint; }
            set { labelPrint = value; }
        }

        public string LptPort
        {
            get { return lptPort; }
            set { lptPort = value; }
        }

    }
     */
    #endregion
    public partial class HSBHWInterface
    {
        #region DLLImport
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
        CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall)]
        public static extern long WritePrinter(IntPtr hPrinter, byte[] data, Int32 buf, ref int pcWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long ClosePrinter(IntPtr hPrinter);
        #endregion

        string comPort;
        int readSec = 3;
        int baudRate = 9600;
        int dataBit = 8;
        Parity parity = Parity.None;
        StopBits stopBits = StopBits.One;
        double basket = 0;
        string[] labelPrint;
        string printerName = "X2000";
        int timeout = 10000;
        string bufferCache = string.Empty;
        Thread thread1;
        System.Windows.Forms.Timer _timer;
        string errorMessage = string.Empty;
        bool hasError = false;

        #region GS1 Setting
        bool GS1Loop = false;
        bool ExitBarcodeValidation = false;
        // GS1Result array -> Result, Count, Last read Time
        // Example {"1234567890", "10", "2014-05-20 11:43:52.123"}
        String[] GS1Val = new String[3] { "", "0", "" };
        Dictionary<int, string> GS1Dict = new Dictionary<int, string>();
        SerialPort gs1Port;
        string GS1LeftOver;
        #endregion

        /*
         * Private Method for HSBHWInterface Class
         * 
         */

        #region Constructor

        public HSBHWInterface(string _comPort = "", int _baudRate = 9600, int _readSec = 3, int _dataBit = 8, Parity _parity = Parity.None, StopBits _stopBits = StopBits.One, double _basket = 0, string[] _lblPrint = null, string _printerName = "X2000", int _timeout = 10000)
        {
            this.comPort = _comPort;
            this.baudRate = _baudRate;
            this.readSec = _readSec;
            this.dataBit = _dataBit;
            this.parity = _parity;
            this.stopBits = _stopBits;
            this.basket = _basket;
            this.labelPrint = _lblPrint;
            this.printerName = _printerName;
            this.timeout = _timeout;
        }

        public HSBHWInterface(string _comPort, int _baudRate, int _readSec, int _dataBit, Parity _parity, StopBits _stopBits, double _basket)
        {
            this.comPort = _comPort;
            this.baudRate = _baudRate;
            this.readSec = _readSec;
            this.dataBit = _dataBit;
            this.parity = _parity;
            this.stopBits = _stopBits;
            this.basket = _basket;
        }

        public HSBHWInterface(string _comPort, int _baudRate, int _readSec, int _dataBit, Parity _parity, StopBits _stopBits, int _timeout)
        {
            this.comPort = _comPort;
            this.baudRate = _baudRate;
            this.readSec = _readSec;
            this.dataBit = _dataBit;
            this.parity = _parity;
            this.stopBits = _stopBits;
            this.timeout = _timeout;
        }

        public HSBHWInterface(string _comPort)
        {
            this.comPort = _comPort;
        }

        public HSBHWInterface(string _comPort, int _timeout)
        {
            this.comPort = _comPort;
            this.timeout = _timeout;
        }

        #endregion

        #region GetSet
        public string ComPort
        {
            get { return this.comPort; }
            set { this.comPort = value; }
        }

        public int ReadSec
        {
            get { return readSec; }
            set { readSec = value; }
        }

        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        public double Basket
        {
            get { return basket; }
            set { basket = value; }
        }

        public int DataBit
        {
            get { return dataBit; }
            set { dataBit = value; }
        }

        public Parity Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        public StopBits StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }

        public string[] LabelString
        {
            get { return labelPrint; }
            set { labelPrint = value; }
        }

        public string PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
        private string BufferCache
        {
            get { return bufferCache; }
            set { bufferCache = value; }
        }

        public Boolean GS1
        {
            get { return GS1Loop; }
            set { GS1Loop = value; }
        }

        public Boolean GS1Exit
        {
            get { return ExitBarcodeValidation; }
            set { ExitBarcodeValidation = value; }
        }

        public String[] GS1Result
        {
            get { return GS1Val; }
        }

        public Dictionary<int, string> GS1History
        {
            get { return GS1Dict; }
        }
        public String ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public Boolean HasError
        {
            get { return hasError; }
            set { hasError = value; }
        }


        #endregion

        #region Small Scale : Ohaus
        public double SmallScaleOhaus()
        {

            double _returnVal = 0;
            string cdata = string.Empty, cdata1 = string.Empty, cReader = string.Empty;

            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    port1.ReadTimeout = Timeout;
                    bool lstop = true;
                    int nStable = 0;
                    if (port1.IsOpen == true) port1.Close();
                    port1.Open();
                    port1.Write(Convert.ToString(Character(6)));
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        cdata = port1.ReadLine();
                        if (cdata == null)
                        {
                            break;
                        }

                        if (cdata1 == cdata)
                        {
                            if (nStable < ReadSec)
                            {
                                nStable++;
                                cdata1 = cdata;
                            }
                            else if (nStable == ReadSec)
                            {
                                cReader = cdata.Substring(0, cdata.LastIndexOf("g"));
                                lstop = false;
                                break;
                            }
                        }
                        else
                        {
                            nStable = 0;
                            cdata1 = cdata;
                        }

                    } while (lstop);
                    port1.Close();
                    _returnVal = Convert.ToDouble(cdata);
                }
            }
            catch
            {
                _returnVal = 0;
            }

            return _returnVal;
        }
        #endregion

        #region Small Scale: Shimadzu
        public double SmallScaleShimadzu()
        {
            double _returnVal = 0;
            string cdata = string.Empty, cdata1 = string.Empty, cReader = string.Empty;

            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    bool lstop = true;
                    int nStable = 0;
                    cdata1 = null;
                    cReader = null;
                    int nData = 0;
                    port1.ReadTimeout = Timeout;
                    if (port1.IsOpen == true) port1.Close();
                    port1.Open();
                    port1.Write("D03" + Character(13));
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        cdata += port1.ReadExisting();
                        if (cdata == null)
                        {
                            break;
                        }
                        else
                        {
                            nData = cdata.LastIndexOf("d");
                            if (nData >= 10)
                            {
                                cdata = cdata.Substring((nData - 9), 9);
                                if (cdata.Substring(0, 1) != "-")
                                {
                                    if (cdata1 != null && cdata1.Substring(0, 8) == cdata.Substring(0, 8))
                                    {

                                        if (nStable < ReadSec)
                                        {
                                            nStable++;
                                            cdata1 = cdata;

                                        }
                                        else if (nStable == ReadSec)
                                        {
                                            cReader = cdata;
                                            lstop = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        nStable = 0;
                                        cdata1 = cdata;
                                    }
                                }
                                else
                                {
                                    nStable = 0;
                                    cdata1 = cdata;
                                }
                            }
                        }
                    } while (lstop);
                    port1.Close();
                    _returnVal = Convert.ToDouble(cdata);
                }
            }
            catch
            {
                throw;
            }
            return _returnVal;
        }
        #endregion
        #region Small Scale: Shimadzu TXB622L
        public double SmallScaleShimadzuTXB622L()
        {
            double _returnVal = 0;
            string cdata = string.Empty, cdata1 = string.Empty, cReader = string.Empty;

            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    bool lstop = true;
                    int nStable = 0;
                    cdata1 = null;
                    cReader = null;
                    int nData = 0;
                    port1.ReadTimeout = Timeout;
                    if (port1.IsOpen == true) port1.Close();
                    port1.Open();
                    port1.Write("D03" + Character(13));
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        cdata += port1.ReadExisting();
                        if (cdata == null)
                        {
                            break;
                        }
                        else
                        {
                            nData = cdata.LastIndexOf("g");
                            if (nData >= 9)
                            {
                                cdata = cdata.Substring((nData - 8), 8);
                                if (cdata.Substring(0, 1) != "-")
                                {
                                    if (cdata1 != null && cdata1.Substring(0, 7) == cdata.Substring(0, 7))
                                    {

                                        if (nStable < ReadSec)
                                        {
                                            nStable++;
                                            cdata1 = cdata;

                                        }
                                        else if (nStable == ReadSec)
                                        {
                                            cReader = cdata;
                                            lstop = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        nStable = 0;
                                        cdata1 = cdata;
                                    }
                                }
                                else
                                {
                                    nStable = 0;
                                    cdata1 = cdata;
                                }
                            }
                        }
                    } while (lstop);
                    port1.Close();
                    _returnVal = Convert.ToDouble(cdata);
                }
            }
            catch
            {
                throw;
            }
            return _returnVal;
        }
        #endregion

        #region Check COM Setting
        public Boolean chkCOM()
        {
            bool ret = true;
            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    port1.Open();
                    port1.Close();
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
        #endregion

        public Boolean chkImajePrinterResponse()
        {
            bool ret = true;
            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    String sCommand;
                    String sPrinterReturn;
                    port1.ReadTimeout = Timeout;
                    if (port1.IsOpen == true) { port1.Close(); }
                    sCommand = String.Concat((char)5);
                    port1.DtrEnable = true;
                    port1.Open();
                    port1.Write(sCommand);
                    Thread.Sleep(500);
                    sPrinterReturn = port1.ReadExisting();
                    byte[] bHex = Encoding.ASCII.GetBytes(sPrinterReturn);
                    char a = (char)sPrinterReturn.ToCharArray()[0];
                    int nRet1 = (int)a;
                    port1.Close();
                    if (nRet1 == 6)
                    {
                        ret = true;   
                    }
                    else
                    {
                        ret = false;
                    }                    
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
        public Boolean chkArgoxPrinterResponse()
        {
            bool ret = true;
            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    String sCommand;
                    String sPrinterReturn;
                    //sCommand = String.Concat((char)1, "A'");
                    sCommand = String.Concat((char)2, "v'");
                    port1.ReadTimeout = 10000;
                    port1.Open();
                    port1.WriteLine(sCommand);
                    Thread.Sleep(300);
                    sPrinterReturn = port1.ReadExisting();
                    port1.Close();
                    sPrinterReturn = sPrinterReturn.Trim();
                    if (sPrinterReturn.Length > 5)
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }                    
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }

        #region Platform Scale: DI-10
        public double PlatformScaleDI10()
        {
            double _returnVal = 0;
            //Param parm = new Param();
            string cdata = string.Empty, cdata1 = string.Empty, cReader = string.Empty;
            try
            {
                using (SerialPort port2 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    port2.ReadTimeout = Timeout;
                    bool lstop = true;
                    int nStable = 0;
                    cdata = null;
                    cdata1 = null;
                    cReader = null;
                    //cBWeight         = null;
                    int nPos = 0;
                    if (port2.IsOpen == true) port2.Close();
                    port2.Open();
                    port2.Write("Chr(5)");
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        cdata = port2.ReadLine();
                        if (cdata == null)
                        {
                            break;
                        }
                        else
                        {
                            if (cdata.Length == 29)
                            {
                                nPos = cdata.IndexOf(Character(13));
                                cdata = cdata.Substring(nPos + 2, 8);
                            }
                        }
                        if (cdata1 == cdata)
                        {

                            if (nStable < 3)
                            {
                                nStable++;
                                cdata1 = cdata;

                            }
                            else if (nStable == 3)
                            {
                                cReader = cdata;
                                lstop = false;
                                break;
                            }
                        }
                        else
                            nStable = 0;
                        cdata1 = cdata;
                    } while (lstop);
                    port2.Close();
                    _returnVal = Convert.ToDouble(cdata);
                }
            }
            catch
            {
                throw;
            }

            if (Basket > 0)
            {
                _returnVal = _returnVal - Basket;
            }

            return _returnVal;
        }
        #endregion

        #region Platform Scale: DI-28
        public double PlatformScaleDI28()
        {
            double _returnVal = 0;
            string cdata = string.Empty, cdata1 = string.Empty, cReader = string.Empty;
            try
            {
                using (SerialPort port2 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {

                    bool lstop = true;
                    int nStable = 0;
                    cdata = null;
                    cdata1 = null;
                    cReader = null;
                    int nData = 0;
                    if (port2.IsOpen == true) port2.Close();
                    port2.Open();
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        port2.Write(Convert.ToString(Character(6)));
                        cdata = port2.ReadLine();
                        if (cdata == null)
                        {
                            break;
                        }
                        else
                        {
                            if (cdata.Length == 20)
                            {
                                nData = cdata.IndexOf(Character(13));
                                cdata = Convert.ToDecimal(cdata.Substring(nData + 2, 7)).ToString();
                            }
                        }
                        if (cdata1 == cdata)
                        {

                            if (nStable < ReadSec)
                            {
                                nStable++;
                                cdata1 = cdata;

                            }
                            else if (nStable == ReadSec)
                            {
                                cReader = cdata;
                                lstop = false;
                                break;
                            }
                        }
                        else
                        {
                            nStable = 0;
                            cdata1 = cdata;
                        }

                    } while (lstop);
                    port2.Close();
                    _returnVal = Convert.ToDouble(cdata);
                }
            }
            catch
            {
                throw;
            }
            if (Basket > 0)
            {
                _returnVal = _returnVal - Basket;
            }

            return _returnVal;
        }
        #endregion DI-28

        #region Platform Scale: Yamato
        public double PlatformScaleYamato()
        {
            double _returnVal = 0;
            //Param parm = new Param();
            string cdata = string.Empty, cdata1 = string.Empty, cReader = string.Empty;
            try
            {
                using (SerialPort port2 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    port2.ReadTimeout = Timeout;
                    bool lstop = true;
                    int nStable = 0;
                    cdata = null;
                    cdata1 = null;
                    cReader = null;
                    //cBWeight         = null;
                    int nPos = 0;
                    if (port2.IsOpen == true) port2.Close();
                    port2.Open();
                    port2.Write("Chr(5)");
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        cdata = port2.ReadLine();
                        if (cdata == null)
                        {
                            break;
                        }
                        else
                        {
                            if (cdata.Length == 12)
                            {
                                nPos = cdata.IndexOf(Character(13));
                                cdata = Convert.ToDecimal(cdata.Substring(0, 8)).ToString();
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (cdata1 == cdata)
                        {

                            if (nStable < ReadSec)
                            {
                                nStable++;
                                cdata1 = cdata;

                            }
                            else if (nStable == ReadSec)
                            {
                                cReader = cdata;
                                lstop = false;
                                break;
                            }
                        }
                        else
                            nStable = 0;
                        cdata1 = cdata;
                    } while (lstop);
                    port2.Close();
                    _returnVal = Convert.ToDouble(cdata);
                }
            }
            catch
            {
                throw;
            }

            if (Basket > 0)
            {
                _returnVal = _returnVal - Basket;
            }

            return _returnVal;
        }
        #endregion

        #region Label Printer: X2000 LPT
        public Boolean X2000LPT()
        {
            bool ret = false;
            try
            {

                System.IntPtr lhPrinter = new System.IntPtr();

                //String[] s = { "f300",  "L", "H06", "PK", "420000000200035 Test", "400000000200050Stock    : Test", "400000000200060Date     : Test", "400000000200070Supplier : Test", "431000002200100 Test", "E" };
                String[] s = LabelString;
                string ns2 = null;

                for (int i = 0; i < s.Length; i++)
                {
                    if (i < 2)
                    {
                        ns2 += Character(2) + s[i] + Character(13);
                    }
                    else
                    {
                        ns2 += s[i] + Character(13);
                    }
                }

                string PCL5Commands = ns2;

                DOCINFO di = new DOCINFO();
                int pcWritten = 0;
                di.pDocName = string.Empty;
                di.pDataType = "RAW";
                byte[] bData = Encoding.GetEncoding("iso-8859-1").GetBytes(PCL5Commands);
                OpenPrinter(printerName, ref lhPrinter, 0);
                StartDocPrinter(lhPrinter, 1, ref di);
                StartPagePrinter(lhPrinter);
                WritePrinter(lhPrinter, bData, bData.Length, ref pcWritten);
                EndPagePrinter(lhPrinter);
                EndDocPrinter(lhPrinter);
                ClosePrinter(lhPrinter);

                ret = true;
            }
            catch
            {
                throw;
            }
            return ret;
        }
        #endregion

        #region Label Printer: X2000 COM
        public Boolean X2000COM()
        {
            bool ret = false;
            string[] ns = null;
            string ns2 = null;
            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    port1.ReadTimeout = 10000;
                    if (port1.IsOpen == true) { port1.Close(); }
                    port1.Open();
                    ns = LabelString;
                    for (int i = 0; i < ns.Length; i++)
                    {
                        ns2 = string.Empty;
                        if (i < 2)
                        {
                            ns2 = Character(2) + ns[i] + Character(13);
                        }
                        else
                        {
                            ns2 = ns[i] + Character(13);
                        }
                        port1.Write(ns2);
                    }
                }
                ret = true;
            }
            catch
            {
                throw;
            }
            return ret;
        }
        #endregion

        #region VideoJet
        public Boolean VideoJet()
        {
            bool ret = false;
            string cmd = string.Empty;
            String[] s = new String[LabelString.Length];
            string cmdFlush = string.Empty + Character(27) + Character(0) + Character(0) + Character(13);

            for (int i = 0; i < LabelString.Length; i++)
            {
                if (i < 4)
                {
                    s[i] = LabelString[i];
                }
            }

            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    bool chkReturn = false;
                    int chkMax = 0;
                    while (chkReturn == false)
                    {
                        chkMax++;
                        port1.ReadTimeout = Timeout;
                        port1.Encoding = Encoding.GetEncoding("iso-8859-1");
                        port1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        port1.Open();
                        port1.Write(cmdFlush);
                        Thread.Sleep(100);

                        //cmd = (char)Convert.ToChar( + (char)Convert.ToChar(129) + (char)Convert.ToChar(11);

                        if (s.Length <= 3)
                        {
                            cmd = "" + Character(27) + Character(129) + Character(11);
                            cmd += "" + Character(27) + Character(129) + Character(1) + VideoJetStringBuildier(s) + Character(13);
                        }
                        else
                        {
                            cmd = "" + Character(27) + Character(4) + Character(11);
                            cmd += "" + Character(27) + Character(4) + Character(22) + VideoJetStringBuildier(s) + Character(13);
                        }
                        port1.Write(cmd);
                        Thread.Sleep(400);
                        if (!(String.IsNullOrEmpty (BufferCache)))
                        {
                            chkReturn = true;
                        }
                        if (chkMax > 2)
                        {
                            chkReturn = true;
                            ret = false;
                        }
                        port1.Close();
                    }
                }
                ret = true;
            }
            catch
            {
                throw;
            }
            return ret;
        }
        #endregion

        #region Imaje: String
        public Boolean ImajeString(int _jet, bool _bld1, int _font1, string _message1, bool _bld2, int _font2, string _message2)
        {
            bool ret = false;

            if (String.IsNullOrEmpty(_message1)) { _message1 = " "; }
            if (String.IsNullOrEmpty(_message2)) { _message2 = " "; }

            if ((_jet != 0) && (_jet < 5))
            {
                try
                {
                    using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                    {
                        bool chkReturn = false;
                        int chkMax = 0;
                        string stdCommand = string.Empty;

                        port1.DtrEnable = true;
                        port1.ReadTimeout = Timeout;
                        port1.Encoding = Encoding.GetEncoding("iso-8859-1");
                        port1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        port1.Open();
                        while (chkReturn == false)
                        {
                            chkMax++;

                            stdCommand = "" + Character(_jet);
                            stdCommand += "" + Character(10) + Character(getBolderazation(_bld1)) + Character(_font1) + _message1;
                            stdCommand += "" + Character(10) + Character(getBolderazation(_bld2)) + Character(_font2) + _message2;
                            stdCommand += "" + Character(13);

                            port1.Write(buildImajeMessage(stdCommand));
                            Thread.Sleep(1500); // wait response!
                            if (BufferCache == Convert.ToString(Character(6)))
                            {
                                ret = true;
                                chkReturn = true;
                            }
                            if (chkMax > 2)
                            {
                                chkReturn = true;
                                ret = false;
                            }
                            port1.DtrEnable = false;
                        }
                        port1.Close();
                    }
                }
                catch
                {
                    throw;
                }
            }
            return ret;
        }
        #endregion

        #region Imaje: Barcode
        public Boolean ImajeBarcode(int _jet, bool _bld, int _font, string _message)
        {
            bool ret = false;
            if ((_jet != 0) && (_jet < 5))
            {
                try
                {
                    using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                    {
                        bool chkReturn = false;
                        int chkMax = 0;
                        string stdCommand = string.Empty;

                        port1.DtrEnable = true;
                        port1.ReadTimeout = Timeout;
                        port1.Encoding = Encoding.GetEncoding("iso-8859-1");
                        port1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        port1.Open();

                        while (chkReturn == false)
                        {
                            chkMax++;

                            stdCommand = "" + Character(_jet);
                            stdCommand += "" + Character(10) + Character(getBolderazation(_bld)) + Character(_font);
                            stdCommand += "" + Character(31) + Character(81) + Character(9) + Character(17) + Character(137) + Character(134);
                            stdCommand += _message;
                            stdCommand += "" + Character(31) + Character(13);
                            port1.Write(buildImajeMessage(stdCommand));
                            Thread.Sleep(1500);
                            if (BufferCache == Convert.ToString(Character(6)))
                            {
                                chkReturn = true;
                                ret = true;
                            }
                            if (chkMax > 2)
                            {
                                chkReturn = true;
                                ret = false;
                            }
                        }
                        port1.DtrEnable = false;
                        port1.Close();
                    }
                }
                catch
                {
                    throw;
                }
            }
            return ret;
        }
        #endregion

        #region ImajeNew: String & Barcode

        public Boolean ImajeNew(string _message1, int nPara)
        {
            bool ret = false;

            if (String.IsNullOrEmpty(_message1)) { _message1 = " "; }

            try
            {
                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    bool chkReturn = false;
                    int chkMax = 0;
                    string stdCommand = string.Empty;

                    port1.DtrEnable = true;
                    port1.ReadTimeout = Timeout;
                    port1.Encoding = Encoding.GetEncoding("iso-8859-1");
                    port1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    port1.Open();
                    while (chkReturn == false)
                    {
                        chkMax++;

                        stdCommand = _message1;

                        port1.Write(buildImajeMessage(stdCommand, nPara));
                        Thread.Sleep(1500); // wait response!
                        if (BufferCache == Convert.ToString(Character(6)))
                        {
                            ret = true;
                            chkReturn = true;
                        }
                        if (chkMax > 2)
                        {
                            chkReturn = true;
                            ret = false;
                        }
                        port1.DtrEnable = false;
                    }
                    port1.Close();
                }
            }
            catch
            {
                throw;
            }

            return ret;
        }

        #endregion


        #region GS1
        public void GS1Read()
        {
            // passive reading for GS1 barcode reader. Will spawn thread and create timer to check thread status
            try
            {
                thread1 = new Thread(new ThreadStart(GS1Thread));
                thread1.Name = "GS1Thread";
                thread1.IsBackground = true;
                GS1Loop = true;
                if (!thread1.IsAlive)
                {
                    thread1.Start();
                }
                _timer = new System.Windows.Forms.Timer();
                _timer.Interval = 1000;
                _timer.Tick += new EventHandler(GS1CheckThread);
                _timer.Enabled = true;
            }
            catch
            {
                throw;
            }
        }

        public void GS1Stop()
        {
            GS1 = false;
        }
        #endregion

        #region Imaje: Misc Command
        public Boolean ImajeClearBuffer(int _jet)
        {
            bool ret = false;
            bool chkJet = false;
            try
            {
                if ((_jet != 0) && (_jet < 4))
                {
                    if (ImajeClrBuff(_jet) == true)
                    {
                        ret = true;
                    }
                }
                else
                {
                    for (int i = 1; i < 5; i++)
                    {
                        chkJet = ImajeClrBuff(i);
                        Thread.Sleep(2000);
                        if (chkJet == false)
                        {
                            i = 5;
                            ret = false;
                        }
                    }
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        private Boolean ImajeClrBuff(int _jet)
        {
            bool ret = false;
            try
            {
                string stdCommand = string.Empty;
                stdCommand = "" + Character(_jet);
                stdCommand += "" + Character(10) + Character(getBolderazation(true)) + Character(52) + " ";
                stdCommand += "" + Character(10) + Character(getBolderazation(true)) + Character(53) + " ";
                stdCommand += "" + Character(13);

                using (SerialPort port1 = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    bool chkReturn = false;
                    int chkMax = 0;

                    port1.DtrEnable = true;
                    port1.ReadTimeout = Timeout;
                    port1.Encoding = Encoding.GetEncoding("iso-8859-1");
                    port1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    port1.Open();
                    while (chkReturn == false)
                    {
                        chkMax++;

                        port1.Write(buildImajeMessage(stdCommand));
                        Thread.Sleep(1500);
                        if (BufferCache == Convert.ToString(Character(6)))
                        {
                            chkReturn = true;
                            ret = true;
                        }
                        if (chkMax > 2)
                        {
                            chkReturn = true;
                            ret = false;
                        }
                    }
                    port1.DtrEnable = false;
                    port1.Close();
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /*
         * Private Method for HSBHWInterface Class
         * 
         */

        #region Private Method

        public static Char Character(int i) //return ascii value
        {
            try
            {
                return Convert.ToChar(i);
            }
            catch
            {
                throw;
            }
        }

        private static string VideoJetStringBuildier(String[] s) //build string for VideoJet printer
        {
            int nLen = 0;
            string str = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length > nLen)
                {
                    nLen = s[i].Length;
                }
            }

            for (int i = 0; i < nLen; i++)
            {
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j].Length <= nLen)
                    {
                        if (i < s[j].Length)
                        {
                            str += s[j].Substring(i, 1);
                        }
                        else
                        {
                            str += " ";
                        }
                    }
                }
            }
            return str;
        }

        private Boolean ImajeJet(int jetNum, string command)
        {

            return true;
        }

        public int getBolderazation(bool _bool) //get bolderazation value for Imaje printer
        {
            if (_bool)
                return 2;
            else
                return 1;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) //buffer handler
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            BufferCache = indata;
        }

        private void DataReceivedHandlerGS1(object sender, SerialDataReceivedEventArgs e) //buffer handler
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string tmp = string.Empty;
                bool isFinish = false;
                string indata = sp.ReadExisting();
                BufferCache = GS1LeftOver + indata;
                int i = 0;
                while (i < BufferCache.Length)
                {
                    if (BufferCache.Substring(i, 1) == "\r")
                    {
                        GS1LeftOver = BufferCache.Substring(i + 1);
                        isFinish = true;
                    }
                    else
                    {
                        tmp += BufferCache.Substring(i, 1);
                    }
                    i++;
                }

                if (isFinish)
                {
                    GS1Val[0] = tmp;
                    GS1Val[1] = (Int32.Parse(GS1Val[1]) + 1).ToString();
                    GS1Val[2] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    int v = GS1Dict.Count + 1;
                    GS1Dict.Add(v, tmp);
                }
                else
                {
                    GS1LeftOver = tmp;
                }
            }
            catch
            {
                throw;
            }
        }
        private void GS1Thread() // GS1 Main thread
        {
            try
            {
                using (gs1Port = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    gs1Port.ReadTimeout = 500;
                    gs1Port.Encoding = Encoding.GetEncoding("iso-8859-1");
                    gs1Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandlerGS1);
                    gs1Port.Open();
                    while (GS1)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                GS1 = false;
                HasError = true;
                //throw;
            }
        }

        private void GS1CheckThread(object sender, EventArgs e)
        {
            if (GS1) //check thread is alive or not, if not, trying to start it back
            {
                if (!thread1.IsAlive)
                {
                    thread1 = null;
                    thread1 = new Thread(new ThreadStart(GS1Thread));
                    thread1.IsBackground = true;
                    thread1.Name = "GS1Thread";
                    thread1.Start();
                }
            }
            else // loop stop, stop thread.
            {
                if (ExitBarcodeValidation)
                {
                    _timer.Stop();
                    _timer = null;
                }
            }
        }

        private string buildImajeMessage(string _msg, int nParam = 10) //build a string to be sent to Imaje printer
        {
            try
            {
                string nPara = "" + Character(nParam);                          // Identifier
                string aData1 = _msg.Length.ToString("X4");
                string bLeft = aData1.Substring(0, 2);                      // Left Byte
                string bRight = aData1.Substring(2, 2);                     // Right Byte
                string chkSum = checkSum(nPara + Character(Convert.ToInt32(bLeft, 16)) + Character(Convert.ToInt32(bRight, 16)) + _msg);    // chksum
                return "" + nPara + Character(Convert.ToInt32(bLeft, 16)) + Character(Convert.ToInt32(bRight, 16)) + _msg + Character(Convert.ToInt32(chkSum, 16));
            }
            catch
            {
                throw;
            }
        }

        #region TowerLight RYG
        public void Towerlight(string cColor)
        {
            try
            {
                using (SerialPort Twlightport = new SerialPort(ComPort, BaudRate, Parity, DataBit, StopBits))
                {
                    Twlightport.ReadTimeout = Timeout;
                    if (Twlightport.IsOpen == true) Twlightport.Close();
                    Twlightport.Open();
                    Twlightport.Write(cColor);
                    Twlightport.Close();
                }
            }
            catch (Exception e3)
            {
                ErrorMessage = e3.Message;
                HasError = true;
                //throw;
            }
        }
        #endregion

        private string checkSum(string _msg) // provide checkbyte for Imaje printer
        {
            try
            {
                decimal nAsc = Convert.ToChar(_msg.Substring(0, 1));
                int xOR = 0;
                for (int i = 1; i < _msg.Length; i++)
                {
                    decimal num = Convert.ToChar(_msg.Substring(i, 1));
                    if (i == 1)
                    {
                        xOR = (int)nAsc ^ (int)num;
                    }
                    else
                    {
                        xOR = (int)xOR ^ (int)num;
                    }
                }
                return xOR.ToString("X2");
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
