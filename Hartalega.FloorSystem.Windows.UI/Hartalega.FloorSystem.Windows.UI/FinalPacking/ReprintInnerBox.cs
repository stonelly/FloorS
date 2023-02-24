using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ReprintInnerBox : FormBase
    {
        # region PRIVATE variables
        private string _screenname = "Re-print Inner Box";
        private string _uiClassName = "ReprintInnerBox";
        string _operatorName = String.Empty;
        FinalPackingDTO objFinalPackingDTO = null;
        private int _BarcodeVerificationRequired = Constants.ZERO;
        private string _orderNumber = string.Empty;
        private string barcodeToValidate = string.Empty;
        private int countToValidate = Constants.ZERO;
        #endregion
        #region CONSTRUCTOR
        public ReprintInnerBox()
        {
            InitializeComponent();
        }
        #endregion
        #region EVENTS

        /// <summary>
        /// RETRIEVE INTERNALOTNUMBER DETAILS AND AUTOPOPULATE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInternalLot_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInnerLotNo.Text))
                {
                    if(FinalPackingBLL.validateInnerLotNumber(txtInnerLotNo.Text,Constants.FIVE))
                    { 
                    objFinalPackingDTO = FinalPackingBLL.GetInternalLotNumberDetails(txtInnerLotNo.Text);
                    txtPoNumber.Text = string.IsNullOrEmpty(objFinalPackingDTO.CustomerReferenceNumber) ? objFinalPackingDTO.Ponumber : objFinalPackingDTO.CustomerReferenceNumber + " | " + objFinalPackingDTO.Ponumber;    
                    txtItemNo.Text = objFinalPackingDTO.ItemNumber;
                    txtItemName.Text = objFinalPackingDTO.ItemName;
                    txtSerialNo.Text = Convert.ToString(objFinalPackingDTO.Serialnumber);
                    txtBatchNo.Text = objFinalPackingDTO.BatchNumber;
                    //txtGroupId.Text = objFinalPackingDTO.QCGroupName;
                    txtBoxesPacked.Text = GetString(objFinalPackingDTO.Boxespacked);
                    txtPalletId.Text = objFinalPackingDTO.Palletid;
                    txtPreshipPltId.Text = objFinalPackingDTO.PreshipmentPLTId;
                    _BarcodeVerificationRequired = objFinalPackingDTO.Barcodevalidationrequired;
                    barcodeToValidate = objFinalPackingDTO.BarcodetoValidate;
                    countToValidate = objFinalPackingDTO.CounttoValidate;
                    _orderNumber = objFinalPackingDTO.orderNumber;
                    btnPrint.Enabled = true;
                    btnPrint.Focus();
                    }
                    else
                    {                      
                        GlobalMessageBox.Show(Messages.INVALIDINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtInnerLotNo.Focus();
                    }
                }
                else
                {
                  ClearForm();                   
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtInternalLot_Leave", null);
            }
        }
       
        /// <summary>
        /// SAVE THE REPRINT INFORMAION AND PRINT THE INNER BOX LABEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_BarcodeVerificationRequired == Constants.ZERO || _BarcodeVerificationRequired == Constants.THREE)
            {
                ProceedPrint();
            }
            else
            {
                if (WorkStationDataConfiguration.GetInstance()._skipBarcodeValidation == true && _BarcodeVerificationRequired == Constants.ONE)
                {
                    ProceedPrint();
                }
                else if (PHLotVerification.CheckTowerLightScanner(_BarcodeVerificationRequired == Constants.TWO ? true : false))
                {
                    ProceedPrint();
                }
            }
        }

        private void ProceedPrint()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInnerLotNo.Text))
                {
                    FPReprintInner objFPReprintInner = new FPReprintInner();
                    objFPReprintInner.InternalLotNumber = txtInnerLotNo.Text.Trim();
                    objFPReprintInner.LocationId = WorkStationDTO.GetInstance().LocationId;
                    objFPReprintInner.PrinterName = txtInnerLabelPrinter.Text.Trim();
                    objFPReprintInner.WorkStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                    objFPReprintInner.AuthhorizedBy = _operatorName;
                    int rowsReturned = FinalPackingBLL.InsertFPReprintInner(objFPReprintInner);
                    if (rowsReturned > Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        FinalPackingPrint.InnerLabelPrint(objFPReprintInner.InternalLotNumber);
                        
                            if (_BarcodeVerificationRequired == Constants.ONE && WorkStationDataConfiguration.GetInstance()._skipBarcodeValidation == false)
                            {
                                if (!string.IsNullOrEmpty(barcodeToValidate))
                                {
                                    new BarcodeVerificationPass(WorkStationDataConfiguration.GetInstance().stationNumber, txtInnerLotNo.Text.Trim(), objFinalPackingDTO.Ponumber,
                                        barcodeToValidate, countToValidate, _orderNumber, objFinalPackingDTO.ItemName, objFinalPackingDTO.Size).ShowDialog();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.BARCODE_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                }
                            }
                            else if (_BarcodeVerificationRequired == Constants.TWO && WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                            {
                                PHLotVerification ph = new PHLotVerification
                                {
                                    PONumber = objFinalPackingDTO.Ponumber,
                                    ItemNumber = objFinalPackingDTO.ItemNumber,
                                    ItemSize = objFinalPackingDTO.Size,
                                    internalLotNumber = objFPReprintInner.InternalLotNumber,
                                    caseCapacity = objFinalPackingDTO.caseCapacity,
                                    TotalInnerbox = Convert.ToInt32(txtBoxesPacked.Text),
                                    PalletID = objFinalPackingDTO.Palletid
                                };
                                ph.ShowDialog();
                            }
                        Close(); // To close window after reprint success
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                    ClearForm();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.REQINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);                    
                }
                ClearForm();
                txtInnerLotNo.Focus();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnPrint_Click", null);
            }
        }

        /// <summary>
        /// CLEAR THE FORM FIELDS ON CONFIRMATION FROM USER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)                                
            {
                ClearForm();
                txtInnerLotNo.Focus();
            }
        }
      
        /// <summary>
        /// TO VALIDATE OPERATORID AND DISPLAY FORM LOAD  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReprintInnerBox_Load(object sender, EventArgs e)
        {
            this.Hide();
            Login _passwordForm = new Login(Constants.Modules.FINALPACKING, _screenname);
            _passwordForm.ShowDialog();

            if (_passwordForm.Authentication != Convert.ToString(Constants.ZERO) && !string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                _operatorName = _passwordForm.Authentication;
                txtLocation.Text = WorkStationDTO.GetInstance().Location;
                if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
                {
                    txtStationNo.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
                }
                else
                    GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED,Constants.AlertType.Information,Messages.INFORMATION,GlobalMessageBoxButtons.OK);

                txtInnerLabelPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
                this.WindowState = FormWindowState.Maximized;
                this.Show();
               
            }
            else
            {                
                this.Close();
            }
          
            }
        #endregion

        #region  USER METHODS
        /// <summary>
        /// TO CLEAR FORM FIELDS
        /// </summary>
        private void ClearForm()
        {
            txtInnerLotNo.Text = string.Empty;
            txtPoNumber.Text = string.Empty;
            txtItemNo.Text = string.Empty;
            txtItemName.Text = string.Empty;
            //txtGroupId.Text = string.Empty;
            txtBoxesPacked.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtPalletId.Text = string.Empty;
            txtPreshipPltId.Text = string.Empty;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }

        }
        /// <summary>
        /// TO RETURN STRING
        /// </summary>
        /// <param name="number">INT VALUE CONVERTED TO STRING TO BE DISPLAYED IN CONTROLS</param>
        /// <returns></returns>
        public string GetString(int number)
        {
            return Convert.ToString(number);
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
            else             
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        #endregion        
    }
}
