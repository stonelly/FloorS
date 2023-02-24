using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class PHLotVerification : Form
    {
        public PHLotVerification()
        {
            InitializeComponent();
            caseCapacity = 0;
            TotalInnerbox = 0;
            PONumber = string.Empty;
            ItemNumber = string.Empty;
            ItemSize = string.Empty;
            internalLotNumber = string.Empty;
            lotNumber = string.Empty;
            CloseBtn.Enabled = false;
        }

        #region PH Lot Verification Variables
        private HSBHWInterface scanner = new HSBHWInterface();
        private HSBHWInterface towerLight = new HSBHWInterface();

        private Thread _lThread;
        private List<int> _cartonRange = new List<int>();

        private List<FinalPackingTxnDTO> fpTxnList = new List<FinalPackingTxnDTO>();

        public int caseCapacity = Constants.ZERO;
        public int TotalInnerbox = Constants.ZERO;
        public string PONumber = string.Empty;
        public string ItemNumber = string.Empty;
        public string ItemSize = string.Empty;
        public string internalLotNumber = string.Empty;
        public string PalletID = string.Empty;
        private string lotNumber = string.Empty;

        private static string _screenname = "Print Inner and Outer Box Label"; //To use for DB logging.
        private static string _uiClassName = "Print Inner and Outer Box Label"; //To use for DB logging.

        #endregion

        #region PH Lot Verification
        /// <summary>
        /// Insert Line Clearance Log
        /// </summary>
        /// <param name="objLineClearanceLogDTO">Line Clearance DTO object</param>
        /// <returns>Return rows affected</returns>
        public void StartScan()
        {
            try
            {
                #region Towerlight
                towerLight.ComPort = WorkStationDataConfiguration.GetInstance().TowerLight_ComPort;
                towerLight.BaudRate = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().TowerLight_BaudRate);
                string parity = WorkStationDataConfiguration.GetInstance().TowerLight_Parity;
                switch (parity)
                {

                    case "Even":
                        towerLight.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "Odd":
                        towerLight.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    default:
                        towerLight.Parity = System.IO.Ports.Parity.None;
                        break;
                }
                towerLight.DataBit = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().TowerLight_DataBit);
                string stopBit = WorkStationDataConfiguration.GetInstance().TowerLight_StopBits;
                switch (stopBit)
                {

                    case "1":
                        towerLight.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "1.5":
                        towerLight.StopBits = System.IO.Ports.StopBits.OnePointFive;
                        break;
                    case "2":
                        towerLight.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        towerLight.StopBits = System.IO.Ports.StopBits.None;
                        break;
                }

                towerLight.Timeout = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().TowerLight_Timeout);
                #endregion

                scanner.ComPort = WorkStationDataConfiguration.GetInstance().ConMntScanner_Comport;
                scanner.BaudRate = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().ConMntScanner_BaudRate);
                switch (WorkStationDataConfiguration.GetInstance().ConMntScanner_Parity)
                {
                    case "Even":
                        scanner.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "Odd":
                        scanner.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    default:
                        scanner.Parity = System.IO.Ports.Parity.None;
                        break;
                }

                switch (WorkStationDataConfiguration.GetInstance().ConMntScanner_StopBits)
                {
                    case "1":
                        scanner.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "1.5":
                        scanner.StopBits = System.IO.Ports.StopBits.OnePointFive;
                        break;
                    case "2":
                        scanner.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        scanner.StopBits = System.IO.Ports.StopBits.None;
                        break;
                }
                scanner.DataBit = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().ConMntScanner_DataBits);
                scanner.GS1Read();
                if (!scanner.HasError)
                {
                    _lThread = new Thread(new ThreadStart(GS1Data));
                    _lThread.Name = "GS1Data";
                    _lThread.IsBackground = true;
                    _lThread.Start();
                    lblStatus.Text = "Scanning Start.";
                    towerLight.Towerlight("G");
                    if(towerLight.HasError)
                    {
                        FloorSystemException fsexp = new FloorSystemException(towerLight.ErrorMessage, "", new Exception(towerLight.ErrorMessage));
                        ExceptionLogging(fsexp, "Peha Lot Verification Screen", "HSBHWInterface", "Towerlight", null);
                        this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
                        this.Close();
                    }
                }
                else
                {
                    FloorSystemException fsexp = new FloorSystemException(scanner.ErrorMessage, "", new Exception(scanner.ErrorMessage));
                    ExceptionLogging(fsexp, "Peha Lot Verification Screen", "HSBHWInterface", "GS1Thread", null);
                    this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                FloorSystemException fsexp = new FloorSystemException(ex.Message, "", new Exception(ex.Message));
                ExceptionLogging(fsexp, "Peha Lot Verification Screen", "PHLotVerification", "StartScan", null);
                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
                this.Close();
            }
        }

        public void GS1Data()
        {
            try
            {
                string GS1Data_Date = "";
                string cGs1ScanData = "";
                string cScannerID = "";
                string cMurgeData = "";
                int nScaner1 = 0;
                int Sc2Cnt = 0;
                int Sc3Cnt = 0;
                Int64 nPrScantime1 = 0;
                Int64 nPrScantime2 = 0;
                Int64 nPrScantime3 = 0;
                int TotalCtn = 0;
                //get total ctn
                TotalCtn = Convert.ToInt32(Math.Ceiling(TotalInnerbox / (Convert.ToDecimal(caseCapacity))));
                if (tbScanData.InvokeRequired)
                {
                    tbScanData.Invoke(new MethodInvoker(delegate { tbScanData.Text = ""; }));
                }

                while (scanner.GS1)
                {
                    if (lblCartonNo.InvokeRequired)
                    {
                        lblCartonNo.Invoke(new MethodInvoker(delegate { lblCartonNo.Text = (Sc3Cnt + 1).ToString("D" + 5); }));
                    }
                        
                    Int64 nScantime = Convert.ToInt64(DateTime.Now.ToString("yyMMddhhmmss"));
                    String[] s = scanner.GS1Result;
                    if (s[2] != GS1Data_Date)
                    {
                        GS1Data_Date = s[2];
                        Console.WriteLine(GS1Data_Date);
                        cGs1ScanData = s[0].Substring(9);
                        cScannerID = s[0].Substring(2, 2);
                        switch (cScannerID)
                        {
                            case "01": //Inner Box barcode for scanner 1
                                       
                                if (cGs1ScanData.Length >= 35)
                                {
                                    cMurgeData = cGs1ScanData.Substring(2, 14) + cGs1ScanData.Substring(24, 11);
                                    nScaner1 = nScaner1 + 1;

                                    //get current carton
                                    int curCtn = Convert.ToInt32(Math.Ceiling(nScaner1 / (Convert.ToDecimal(caseCapacity))));

                                    //add record for current carton scanner 1 into list
                                    if (fpTxnList.Count() == 0 || !fpTxnList.Any(x => x.CartonNo == GetCaseNumber(curCtn) && x.Scanner == 1))
                                    {
                                        fpTxnList.Add(new FinalPackingTxnDTO
                                        {
                                            PONumber = this.PONumber,
                                            Size = this.ItemSize,
                                            ItemNumber = this.ItemNumber,
                                            CartonNo = GetCaseNumber(curCtn),
                                            Scanner = 1,
                                            LotNumber = cMurgeData,
                                            LotNumberScanned = cMurgeData,
                                            IsMismatch = false,
                                            IsEmail = false,
                                            CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                            ScnMisscannedCount = 0
                                        });

                                        if (lblSc1Status.InvokeRequired)
                                        {
                                            lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.Text = "Scanned"; }));
                                            lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.ForeColor = System.Drawing.Color.Green; }));
                                        }
                                    }

                                    //lot number set for the first time successfully scanned only
                                    if (String.IsNullOrEmpty(lotNumber))
                                    {
                                        lotNumber = cMurgeData;
                                        
                                        //update lot number into the record with current carton and scanner 1 in the list
                                        fpTxnList.Where(x => x.CartonNo == GetCaseNumber(curCtn) && x.Scanner == 1).ToList()
                                                    .ForEach(y =>
                                                    {
                                                        y.LotNumber = lotNumber;
                                                        y.LotNumberScanned = cMurgeData;
                                                    });
                                    }

                                    //Check lot number only when get the first lot number
                                    if (!String.IsNullOrEmpty(lotNumber))
                                    {
                                        if(lotNumber != cMurgeData)
                                        {
                                            towerLight.Towerlight("R");
                                            //label show MisMatch
                                            if (lblSc1Status.InvokeRequired)
                                            {
                                                lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.Text = "Mismatch"; }));
                                                lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.ForeColor = System.Drawing.Color.Red; }));
                                            }
                                               
                                            //update the record with current carton and scanner 1 to mismatch
                                            fpTxnList.Where(x => x.CartonNo == GetCaseNumber(curCtn) && x.Scanner == 1).ToList()
                                                    .ForEach(y => 
                                                    {
                                                        y.LotNumberScanned = cMurgeData;
                                                        y.IsMismatch = true;
                                                        y.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                                                    });
                                        }
                                        else
                                        {
                                            if (lblSc1Status.InvokeRequired)
                                            {
                                                lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.Text = "Scanned"; }));
                                                lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.ForeColor = System.Drawing.Color.Green; }));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cGs1ScanData.Substring(0, 5) == "ERROR") //Miss scan inner box
                                    {
                                        cMurgeData = "MisScanned";
                                        nScaner1 = nScaner1 + 1;

                                        int curCtn = Convert.ToInt32(Math.Ceiling(nScaner1 / (Convert.ToDecimal(caseCapacity))));

                                        //add a new record for scanner 1 and current carton with misscan info when miss scan for the first scan
                                        if (fpTxnList.Count() == 0 || !fpTxnList.Any(x => x.CartonNo == GetCaseNumber(curCtn) && x.Scanner == 1))
                                        {
                                            fpTxnList.Add(new FinalPackingTxnDTO
                                            {
                                                PONumber = this.PONumber,
                                                Size = this.ItemSize,
                                                ItemNumber = this.ItemNumber,
                                                CartonNo = GetCaseNumber(curCtn),
                                                Scanner = 1,
                                                LotNumber = cMurgeData,
                                                LotNumberScanned = cMurgeData,
                                                IsMismatch = false,
                                                IsEmail = false,
                                                CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                                ScnMisscannedCount = 1
                                            });
                                        }

                                        //accumulate miscanned counter
                                        if (nScaner1 > 1)
                                        {
                                            fpTxnList.Where(x => x.CartonNo == GetCaseNumber(curCtn) && x.Scanner == 1).ToList()
                                                .ForEach(y =>
                                                {
                                                    y.LotNumberScanned = "MisScanned";
                                                    y.IsMismatch = false;
                                                    y.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                                                    y.ScnMisscannedCount++;
                                                });
                                        }
                                        //Sc1 label show miss scan
                                        if (lblSc1Status.InvokeRequired)
                                        {
                                            lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.Text = "Miss Scanned"; }));
                                            lblSc1Status.Invoke(new MethodInvoker(delegate { lblSc1Status.ForeColor = System.Drawing.Color.Red; }));
                                        }
                                    }
                                }
                                nPrScantime1 = Convert.ToInt64(DateTime.Now.ToString("yyMMddhhmmss"));
                            
                        break;

                            case "02": //Outer case barcode for scanner 2
                                if ((nScantime - nPrScantime2) > 3) //To verify is the scan is double scan
                                {
                                    if (cGs1ScanData.Length > 35)
                                    {
                                        Sc2Cnt++;
                                        cMurgeData = cGs1ScanData.Substring(2, 14) + cGs1ScanData.Substring(28, 11);

                                        //add new scanner 2 record with current carton
                                        if (!fpTxnList.Any(x => x.CartonNo == GetCaseNumber(Sc2Cnt) && x.Scanner == 2))
                                        {
                                            fpTxnList.Add(new FinalPackingTxnDTO
                                            {
                                                PONumber = this.PONumber,
                                                Size = this.ItemSize,
                                                ItemNumber = this.ItemNumber,
                                                CartonNo = GetCaseNumber(Sc2Cnt),
                                                Scanner = 2,
                                                LotNumber = lotNumber,
                                                LotNumberScanned = cMurgeData,
                                                IsMismatch = false,
                                                IsEmail = false,
                                                CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                                ScnMisscannedCount = 0
                                            });
                                        }

                                        if (lblSc2Status.InvokeRequired)
                                        {
                                            lblSc2Status.Invoke(new MethodInvoker(delegate { lblSc2Status.Text = "Scanned"; }));
                                            lblSc2Status.Invoke(new MethodInvoker(delegate { lblSc2Status.ForeColor = System.Drawing.Color.Green; }));
                                        }

                                        if (!String.IsNullOrEmpty(lotNumber) && lotNumber != cMurgeData)
                                        {
                                            towerLight.Towerlight("R");
                                            //mismatch
                                            if (lblSc2Status.InvokeRequired)
                                            {
                                                lblSc2Status.Invoke(new MethodInvoker(delegate { lblSc2Status.Text = "Mismatch"; }));
                                                lblSc2Status.Invoke(new MethodInvoker(delegate { lblSc2Status.ForeColor = System.Drawing.Color.Red; }));
                                            }

                                            //update scanner 2 record with current carton to mismatch 
                                            fpTxnList.Where(x => x.CartonNo == GetCaseNumber(Sc2Cnt) && x.Scanner == 2).ToList()
                                                    .ForEach(y =>
                                                    {
                                                        y.LotNumberScanned = cMurgeData;
                                                        y.IsMismatch = true;
                                                        y.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                                                    });
                                        }

                                    }
                                    else
                                    {
                                        if (cGs1ScanData.Substring(0, 5) == "ERROR") //Miss scan inner box
                                        {
                                            cMurgeData = "MisScanned";
                                            Sc2Cnt++;
                                            //Sc2 label show miss 
                                            if (lblSc2Status.InvokeRequired)
                                            {
                                                lblSc2Status.Invoke(new MethodInvoker(delegate { lblSc2Status.Text = "Miss Scanned"; }));
                                                lblSc2Status.Invoke(new MethodInvoker(delegate { lblSc2Status.ForeColor = System.Drawing.Color.Red; }));
                                            }

                                            //add scanner 2 record with current carton
                                            if (!fpTxnList.Any(x => x.CartonNo == GetCaseNumber(Sc2Cnt) && x.Scanner == 2))
                                            {
                                                fpTxnList.Add(new FinalPackingTxnDTO
                                                {
                                                    PONumber = this.PONumber,
                                                    Size = this.ItemSize,
                                                    ItemNumber = this.ItemNumber,
                                                    CartonNo = GetCaseNumber(Sc2Cnt),
                                                    Scanner = 2,
                                                    LotNumber = lotNumber,
                                                    LotNumberScanned = "MisScanned",
                                                    IsMismatch = false,
                                                    IsEmail = false,
                                                    CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                                    ScnMisscannedCount = 1
                                                });
                                            }
                                        }
                                    }
                                    nPrScantime2 = Convert.ToInt64(DateTime.Now.ToString("yyMMddhhmmss"));
                                }
                                break;

                            case "03": //Outer case barcode for scanner 3
                                if ((nScantime - nPrScantime3) > 3) //To verify is the scan is double scan
                                {
                                    if (cGs1ScanData.Length > 35)
                                    {
                                        Sc3Cnt++;
                                        cMurgeData = cGs1ScanData.Substring(2, 14) + cGs1ScanData.Substring(28, 11);

                                        //add new scanner 3 record with current carton
                                        if (!fpTxnList.Any(x => x.CartonNo == GetCaseNumber(Sc3Cnt) && x.Scanner == 3))
                                        {
                                            fpTxnList.Add(new FinalPackingTxnDTO
                                            {
                                                PONumber = this.PONumber,
                                                Size = this.ItemSize,
                                                ItemNumber = this.ItemNumber,
                                                CartonNo = GetCaseNumber(Sc3Cnt),
                                                Scanner = 3,
                                                LotNumber = lotNumber,
                                                LotNumberScanned = cMurgeData,
                                                IsMismatch = false,
                                                IsEmail = false,
                                                CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                                ScnMisscannedCount = 0
                                            });
                                        }

                                        if (lblSc3Status.InvokeRequired)
                                        {   
                                            lblSc3Status.Invoke(new MethodInvoker(delegate { lblSc3Status.Text = "Scanned"; }));
                                            lblSc3Status.Invoke(new MethodInvoker(delegate { lblSc3Status.ForeColor = System.Drawing.Color.Green; }));
                                        }

                                        if (lotNumber != cMurgeData)
                                        {
                                            towerLight.Towerlight("R");
                                            //mismatch
                                            if (lblSc3Status.InvokeRequired)
                                            {
                                                lblSc3Status.Invoke(new MethodInvoker(delegate { lblSc3Status.Text = "Mismatch"; }));
                                                lblSc3Status.Invoke(new MethodInvoker(delegate { lblSc3Status.ForeColor = System.Drawing.Color.Red; }));
                                            }

                                            //add scanner 3 record with current carton 
                                            fpTxnList.Where(x => x.CartonNo == GetCaseNumber(Sc3Cnt) && x.Scanner == 3).ToList()
                                                    .ForEach(y =>
                                                    {
                                                        y.LotNumberScanned = cMurgeData;
                                                        y.IsMismatch = true;
                                                        y.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                                                    });
                                        }
                                        
                                        EmailAlert(GetCaseNumber(Sc3Cnt));
                                    }
                                    else
                                    {
                                        if (cGs1ScanData.Substring(0, 5) == "ERROR") //Miss scan inner box
                                        {
                                            cMurgeData = "MisScanned";
                                            Sc3Cnt++;
                                            //Sc3 label show miss scan
                                            if (lblSc3Status.InvokeRequired)
                                            {
                                                lblSc3Status.Invoke(new MethodInvoker(delegate { lblSc3Status.Text = "Miss Scanned"; }));
                                                lblSc3Status.Invoke(new MethodInvoker(delegate { lblSc3Status.ForeColor = System.Drawing.Color.Red; }));
                                            }

                                            //update scanner 3 record with current carton to misscanned 
                                            if (!fpTxnList.Any(x => x.CartonNo == GetCaseNumber(Sc3Cnt) && x.Scanner == 3))
                                            {
                                                fpTxnList.Add(new FinalPackingTxnDTO
                                                {
                                                    PONumber = this.PONumber,
                                                    Size = this.ItemSize,
                                                    ItemNumber = this.ItemNumber,
                                                    CartonNo = GetCaseNumber(Sc3Cnt),
                                                    Scanner = 3,
                                                    LotNumber = lotNumber,
                                                    LotNumberScanned = "MisScanned",
                                                    IsMismatch = false,
                                                    IsEmail = false,
                                                    CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                                    ScnMisscannedCount = 1
                                                });
                                            }

                                            EmailAlert(GetCaseNumber(Sc3Cnt));
                                        }
                                    }

                                    nPrScantime3 = Convert.ToInt64(DateTime.Now.ToString("yyMMddhhmmss"));
                                }
                                break;

                            default:
                                MessageBox.Show("Wrong Scanner ID.");
                                break;
                        }

                        if (tbScanData.InvokeRequired)
                        {
                            tbScanData.Invoke(new MethodInvoker(delegate { tbScanData.AppendText(cGs1ScanData + "\r\n"); }));
                        }

                        if(Sc3Cnt == TotalCtn)
                        {
                            scanner.GS1 = false;
                            CloseBtn.Enabled = true;
                            lblStatus.Text = "Completed";
                            if (fpTxnList.Any())
                            {
                                FinalPackingBLL.InsertFinalPackingTxn(fpTxnList);
                            }
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception e3)
            {
                FloorSystemException fsexp = new FloorSystemException(e3.Message, "", new Exception(e3.Message));
                ExceptionLogging(fsexp, "Peha Lot Verification Screen", "PHLotVerification", "GS1Data", null);
                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
                this.Close();
            }
        }

        //Generate Email when mismatch occur
        public void EmailAlert(int ctn)
        {
            try
            {
                bool scanner1 = false, scanner2 = false, scanner3 = false;
                string sc1Result = string.Empty, sc2Result = string.Empty, sc3Result = string.Empty;

                //Check any mismatch in scanner 1
                if(fpTxnList.Any(x => x.CartonNo == ctn && x.Scanner == Constants.ONE && x.IsMismatch == true))
                {
                    scanner1 = true;
                    sc1Result = fpTxnList.Where(x => x.CartonNo == ctn && x.Scanner == Constants.ONE).FirstOrDefault().LotNumberScanned;
                }

                //Check any mismatch in scanner 2
                if (fpTxnList.Any(x => x.CartonNo == ctn && x.Scanner == Constants.TWO && x.IsMismatch == true))
                {
                    scanner2 = true;
                    sc2Result = fpTxnList.Where(x => x.CartonNo == ctn && x.Scanner == Constants.TWO).FirstOrDefault().LotNumberScanned;
                }

                //Check any mismatch in scanner 3
                if (fpTxnList.Any(x => x.CartonNo == ctn && x.Scanner == Constants.THREE && x.IsMismatch == true))
                {
                    scanner3 = true;
                    sc3Result = fpTxnList.Where(x => x.CartonNo == ctn && x.Scanner == Constants.THREE).FirstOrDefault().LotNumberScanned;
                }
                
                //If any of the scanner have mismatch, send email to plant manager
                if (scanner1 || scanner2 || scanner3)
                {
                    if (FinalPackingBLL.PehaLotVerificationEmailtoPlantManager(this.PONumber, this.ItemNumber, this.ItemSize, this.internalLotNumber,
                                                                    GetCaseNumber(ctn), this.lotNumber, sc1Result, sc2Result, sc3Result, scanner1, scanner2, scanner3))
                    {
                        //update IsEmail to true for all the scanner record for current carton when successfully sent email
                        fpTxnList.Where(x => x.CartonNo == ctn && x.IsMismatch == true).ToList()
                                                    .ForEach(y =>
                                                    {
                                                        y.IsEmail = true;
                                                    });
                    }
                }
            }
            catch (Exception e3)
            {
                FloorSystemException fsexp = new FloorSystemException(e3.Message, "", new Exception(e3.Message));
                ExceptionLogging(fsexp, "Peha Lot Verification Screen", "PHLotVerification", "EmailAlert", null);
                this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
                this.Close();
            }

        }

        //Get current carton number
        private int GetCaseNumber(int ctn)
        {
            int caseNumber = 0;
            if (ctn > 0)
            {
                if(_cartonRange.Count > 0 && ctn <= _cartonRange.Count)
                {
                    caseNumber = _cartonRange[ctn - 1];
                }
                else
                {
                    caseNumber = _cartonRange[_cartonRange.Count - 1];
                }
            }
            return caseNumber;
        }
        #endregion

        private void PHLotVerification_Load(object sender, EventArgs e)
        {
            //Page load get carton range based on internal lot number
            if (!string.IsNullOrEmpty(this.internalLotNumber))
                _cartonRange = FinalPackingBLL.GetLotCartonRangeList(this.internalLotNumber);

            StartScan();
        }

        private void btResetTl_Click(object sender, EventArgs e)
        {
            bool isPass = false;
            string authorisedBy = string.Empty;
            //Prompt Authorization 
            LineClearanceVerification LCV = new LineClearanceVerification();
            LCV.IsPeha = true;
            LCV.ShowDialog();

            // Proceed when Verified successfully 
            if (LCV.IsVerified)
            {
                isPass = true;
                authorisedBy = LCV.EmployeeID;
            }

            // if verified passed, then allow reset tower light
            if(isPass)
            {
                towerLight.Towerlight("G");

                LineClearanceLogDTO objLineClearanceLog = new LineClearanceLogDTO();
                objLineClearanceLog.PONumber = this.PONumber;
                objLineClearanceLog.Size = this.ItemSize;
                objLineClearanceLog.ItemNumber = this.ItemNumber;
                objLineClearanceLog.PalletID = this.PalletID;
                objLineClearanceLog.ScreenName = "Reset Tower Light";
                objLineClearanceLog.AuthorisedBy = authorisedBy;
                objLineClearanceLog.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();

                //Insert LineClearanceLog
                FinalPackingBLL.InsertLineClearanceLog(objLineClearanceLog);
            }

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            towerLight.Towerlight("#");
            scanner.GS1Exit = true;
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
            this.Close();
        }

        /// <summary>
        /// LOG TO DB, SHOW MESAGE BOX TO USER AND CLEAR THE FORM ON EXCEPTION
        /// </summary>
        /// <param name="floorException">APPLICATION EXCEPTION</param>
        /// <param name="screenName">SCREEN NAME TO BE LOGGED</param>
        /// <param name="UiClassName">CLASS NAME TO BE LOGGED</param>
        /// <param name="uiControl">CONTROL FOR WHICH THE EXCEPTION OCCURED</param>
        /// <param name="parameters"></param>
        private static void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.FP_TOWERLIGHT)
                GlobalMessageBox.Show(Messages.TOWERLIGHT_COMMUNICATION_ERROR, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.FP_SCANNER)
                GlobalMessageBox.Show(Messages.SCANNER_COMMUNICATION_ERROR, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.FP_TOWERLIGHT_SCANNER)
                GlobalMessageBox.Show(Messages.TOWERLIGHT_SCANNER_COMMUNICATION_ERROR, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        #region tower light and scanner
        private static bool chkTowerLightStatus()
        {
            bool isAvailable = true;
            string comPort = WorkStationDataConfiguration.GetInstance().TowerLight_ComPort;
            int BaudRate = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().TowerLight_BaudRate);
            Parity parity;
            switch (WorkStationDataConfiguration.GetInstance().TowerLight_Parity)
            {
                case "Even":
                    parity = System.IO.Ports.Parity.Even;
                    break;
                case "Odd":
                    parity = System.IO.Ports.Parity.Odd;
                    break;
                default:
                    parity = System.IO.Ports.Parity.None;
                    break;
            }
            int dataBit = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().TowerLight_DataBit);
            StopBits stopBits;
            switch (WorkStationDataConfiguration.GetInstance().TowerLight_StopBits)
            {
                case "1":
                    stopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    stopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    stopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    stopBits = System.IO.Ports.StopBits.None;
                    break;
            }

            int timeout = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().TowerLight_Timeout);
            try
            {
                using (SerialPort Twlightport = new SerialPort(comPort, BaudRate, parity,dataBit, stopBits))
                {
                    Twlightport.ReadTimeout = timeout;
                    if (Twlightport.IsOpen == true) Twlightport.Close();
                    Twlightport.Open();
                    Twlightport.Close();
                }
            }
            catch (Exception ex)
            {
                isAvailable = false;
            }

            return isAvailable;
        }

        private static bool chkScannerStatus()
        {
            bool isAvailable = true;
            string comPort = WorkStationDataConfiguration.GetInstance().ConMntScanner_Comport;
            int BaudRate = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().ConMntScanner_BaudRate);
            Parity parity;
            switch (WorkStationDataConfiguration.GetInstance().ConMntScanner_Parity)
            {
                case "Even":
                    parity = System.IO.Ports.Parity.Even;
                    break;
                case "Odd":
                    parity = System.IO.Ports.Parity.Odd;
                    break;
                default:
                    parity = System.IO.Ports.Parity.None;
                    break;
            }
            int dataBit = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().ConMntScanner_DataBits);
            StopBits stopBits;
            switch (WorkStationDataConfiguration.GetInstance().ConMntScanner_StopBits)
            {
                case "1":
                    stopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    stopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    stopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    stopBits = System.IO.Ports.StopBits.None;
                    break;
            }

            try
            {
                using (SerialPort scanner = new SerialPort(comPort, BaudRate, parity, dataBit, stopBits))
                {
                    if (scanner.IsOpen == true) scanner.Close();
                    scanner.Open();
                    scanner.Close();
                }
            }
            catch (Exception ex)
            {
                isAvailable = false;
            }

            return isAvailable;
        }

        public static bool CheckTowerLightScanner(bool isPeha = false)
        {
            bool checkStatus = true; 
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration.GetValueOrDefault())
            {                
                bool isScannerAvailable = chkScannerStatus();

                FloorSystemException exTemp;

                if(isPeha)
                {
                    bool isTwlAvailable = chkTowerLightStatus();
                    if (isTwlAvailable && isScannerAvailable)
                    {
                        return checkStatus;
                    }
                    else if (!isTwlAvailable && !isScannerAvailable)
                    {
                        exTemp = new FloorSystemException(Messages.TOWERLIGHT_SCANNER_COMMUNICATION_ERROR, Constants.FP_TOWERLIGHT_SCANNER, new Exception(), true);
                        ExceptionLogging(exTemp, _screenname, _uiClassName, "Print_Click", null);
                    }
                    else
                    {
                        if (!isTwlAvailable)
                        {
                            exTemp = new FloorSystemException(Messages.TOWERLIGHT_COMMUNICATION_ERROR, Constants.FP_TOWERLIGHT, new Exception(), true);
                        }
                        else
                        {
                            exTemp = new FloorSystemException(Messages.SCANNER_COMMUNICATION_ERROR, Constants.FP_SCANNER, new Exception(), true);
                        }
                        ExceptionLogging(exTemp, _screenname, _uiClassName, "Print_Click", null);
                    }

                    checkStatus = (isScannerAvailable && isTwlAvailable);
                }
                else
                {
                    if(!isScannerAvailable)
                    {
                        exTemp = new FloorSystemException(Messages.SCANNER_COMMUNICATION_ERROR, Constants.FP_SCANNER, new Exception(), true);
                        ExceptionLogging(exTemp, _screenname, _uiClassName, "Print_Click", null);
                    }
                    checkStatus = isScannerAvailable;
                }                
            }
            else
            {
                checkStatus = false;
                GlobalMessageBox.Show(Messages.PEHA_SARAYA_HARDWARE_INTEGRATION_FLAG_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }

            return checkStatus;
        }
        #endregion

        private void PHLotVerification_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
