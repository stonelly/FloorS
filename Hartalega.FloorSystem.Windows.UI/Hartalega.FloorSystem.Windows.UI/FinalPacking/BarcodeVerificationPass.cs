using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework.Common;
namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class BarcodeVerificationPass : FormBase
    {
        private string _screenname = "BarcodeValidation";
        private string _orderNumber = string.Empty;
        private string _itemname = string.Empty;
        private string _size = string.Empty;
        private string _cartonRange = string.Empty;
        private string _operatorName = String.Empty; //to capture the operator name of the split batch.
        Thread _lThread;
        HSBHWInterface hw = new HSBHWInterface();
        int _passcount = 0;
        int _failcount = 0;

        // private string _screenname = "BarcodeValidation"; //To use for DB logging.
        private string _uiClassName = "BarcodeValidation"; //To use for DB logging.
        private bool _uservalidationRequired;

        public BarcodeVerificationPass(string stationNumber, string internalNumber, string poNumber, string barcodetoValidate, int counttoValidate,
            string orderName, string itemName, string size)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(stationNumber))
                txtStationNo.Text = stationNumber;
            if (!string.IsNullOrEmpty(internalNumber))
                txtInnerLotNo.Text = internalNumber;
            if (!string.IsNullOrEmpty(poNumber))
                txtPO.Text = poNumber;
            if (!string.IsNullOrEmpty(barcodetoValidate))
                txtBarcodeToValidate.Text = barcodetoValidate;
            txtSubstring.Text = Convert.ToString(counttoValidate);
            if (!string.IsNullOrEmpty(orderName))
                _orderNumber = orderName;

            if (!string.IsNullOrEmpty(itemName))
                _itemname = itemName;
            if (!string.IsNullOrEmpty(size))
                _size = size;
            if (!string.IsNullOrEmpty(internalNumber))
                _cartonRange = FinalPackingBLL.GetLotCartonRange(internalNumber);
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                StartMountScanner();
        }

        private void StartMountScanner()
        {
            hw.ComPort = WorkStationDataConfiguration.GetInstance().ConMntScanner_Comport;
            hw.BaudRate = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().ConMntScanner_BaudRate);
            switch (WorkStationDataConfiguration.GetInstance().ConMntScanner_Parity)
            {
                case "Even":
                    hw.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "Odd":
                    hw.Parity = System.IO.Ports.Parity.Odd;
                    break;
                default:
                    hw.Parity = System.IO.Ports.Parity.None;
                    break;
            }

            switch (WorkStationDataConfiguration.GetInstance().ConMntScanner_StopBits)
            {
                case "1":
                    hw.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    hw.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    hw.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    hw.StopBits = System.IO.Ports.StopBits.None;
                    break;
            }
            hw.DataBit = Convert.ToInt32(WorkStationDataConfiguration.GetInstance().ConMntScanner_DataBits);

            hw.GS1Read();
            _lThread = new Thread(new ThreadStart(GS1Data));
            _lThread.Name = "GS1Data";
            _lThread.IsBackground = true;
            _lThread.Start();

        }

        public void GS1Data()
        {
            try
            {
                string GS1Data_Date = string.Empty;
                string barcodetoValidate = txtBarcodeToValidate.Text;
                int substringindex = Convert.ToInt32(txtSubstring.Text);
                int substringValue = substringindex - barcodetoValidate.Length;
                lblResult.Text = string.Empty;
                while (hw.GS1)
                {
                    String[] result = hw.GS1Result;

                    if (result[2] != GS1Data_Date)
                    {
                        GS1Data_Date = result[2];
                        if (result[0].Length >= substringValue + barcodetoValidate.Length - 1)
                        {
                            if (barcodetoValidate == result[0].Substring(substringValue, barcodetoValidate.Length)) //COMPARE THE BARCODE 
                            {
                                _passcount++;
                                if (txtPasscount.InvokeRequired)
                                {
                                    txtPasscount.Invoke(new MethodInvoker(delegate
                                    {
                                        txtPasscount.Text = Convert.ToString(_passcount); //result[1];
                                        lblResult.ForeColor = Color.Green;
                                        lblResult.Text = Constants.PASS.ToUpper();
                                    }));

                                }
                            }
                            else
                            {
                                if (txtfailcount.InvokeRequired)
                                {
                                    _failcount++;
                                    txtfailcount.Invoke(new MethodInvoker(delegate
                                    {
                                        txtfailcount.Text = Convert.ToString(_failcount); //result[1];
                                        lblResult.ForeColor = Color.Red;
                                        lblResult.Text = Constants.FAIL.ToUpper();
                                    }));
                                    hw.GS1Stop();
                                    FinalPackingBLL.UpdateLotVerificationResult(txtInnerLotNo.Text, false);
                                    FinalPackingBLL.LotVerificationEmailtoPlantManager(txtPO.Text, _orderNumber, _itemname, _size,
                                                                txtInnerLotNo.Text, _cartonRange, result[0]);
                                    _uservalidationRequired = true;
                                    ValidateUser();
                                }
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BARCODE_NOT_CORRECTFORMAT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception genEx)
            {
                FloorSystemException fsexp = new FloorSystemException(genEx.Message, "", genEx);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "Bar code verification", null);
            }
        }


        /// <summary>
        /// LOG TO DB, SHOW MESAGE BOX TO USER AND CLEAR THE FORM ON EXCEPTION
        /// </summary>
        /// <param name="floorException">APPLICATION EXCEPTION</param>
        /// <param name="screenName">SCREEN NAME TO BE LOGGED</param>
        /// <param name="UiClassName">CLASS NAME TO BE LOGGED</param>
        /// <param name="uiControl">CONTROL FOR WHICH THE EXCEPTION OCCURED</param>
        /// <param name="parameters"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.FINALPACKINGPRINTER)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.PRINTING_EXCEPTION + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);


        }

        private void BarcodeVerification_Closed(object sender, FormClosedEventArgs e)
        {
            hw.GS1Stop();
            hw.GS1Exit = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                StartMountScanner();
        }

        private void ValidateUser()
        {
            Login _passwordForm = new Login(Constants.Modules.FINALPACKING, _screenname);
            _passwordForm.ShowDialog();
            if (_passwordForm.Authentication != Convert.ToString(Constants.ZERO) && !string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                _operatorName = Convert.ToString(_passwordForm.Authentication);
                hw.GS1 = true;
                FinalPackingBLL.UpdateOperatorId(txtInnerLotNo.Text, _operatorName);
                _uservalidationRequired = false;
            }
        }

        private void BarcodeVerificationPass_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_uservalidationRequired)
            {
                ValidateUser();
                e.Cancel = _uservalidationRequired ? true : false;
            }
        }


    }
}
