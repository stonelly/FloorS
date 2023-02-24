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
    public partial class RePrint2ndGradeInner : FormBase
    {
        # region PRIVATE variables
        private string _screenname = "Print 2ndGrade Inner Outer";
        private string _uiClassName = "RePrint2ndGradeInner";
        string _operatorName = String.Empty;
        FinalPackingDTO objFinalPackingDTO = null;
        private int _GCLabelPrintingRequired = Constants.ZERO;        
        private int _BarcodeVerificationRequired = Constants.ZERO;

        #endregion
        public RePrint2ndGradeInner()
        {
            InitializeComponent();
        }

        private void txtInnerLotNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInnerLotNo.Text))
                {
                    if (FinalPackingBLL.validateInnerLotNumber(txtInnerLotNo.Text.Trim(),Constants.SIX))
                    {                        
                        objFinalPackingDTO = FinalPackingBLL.GetSecondGradeInternalLotNumberDetails(txtInnerLotNo.Text);
                        txtPoNumber.Text = objFinalPackingDTO.Ponumber;
                        txtPoNumber.Text = string.IsNullOrEmpty(objFinalPackingDTO.CustomerReferenceNumber) ? objFinalPackingDTO.Ponumber : objFinalPackingDTO.CustomerReferenceNumber + " | " + objFinalPackingDTO.Ponumber;    
                        txtItemNo.Text = objFinalPackingDTO.ItemNumber;
                        txtItemName.Text = string.Format("{0}{1}{2}", objFinalPackingDTO.ItemName, Constants.UNDERSCORE, objFinalPackingDTO.Size);


                        txtGroupId.Text = objFinalPackingDTO.QCGroupName;
                        txtBoxesPacked.Text = GetString(objFinalPackingDTO.Boxespacked);
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
            txtGroupId.Text = string.Empty;
            txtBoxesPacked.Text = string.Empty;
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

        private void btnPrint_Click(object sender, EventArgs e)
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
                        //Printing Code
                        FinalPackingPrint.InnerLabelPrint(objFPReprintInner.InternalLotNumber, Convert.ToString(Constants.SIX));
                        //if (_BarcodeVerificationRequired == Constants.ONE)
                        //{
                        //    if (!string.IsNullOrEmpty(barcodeToValidate))
                        //    {
                        //        // FinalPackingBLL.UpdateBarcodeToValidate(barcodeToValidate, countToValidate, objFinalPackingDTO.Internallotnumber);
                        //        new BarcodeVerificationPass(WorkStationDataConfiguration.GetInstance().stationNumber, objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.Ponumber, barcodetoValidate: barcodeToValidate, counttoValidate: countToValidate).ShowDialog();
                        //    }
                        //    else
                        //    {
                        //        GlobalMessageBox.Show(Messages.BARCODE_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        //    }
                        //}
                        this.Close();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.FPERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);                       
                       
                    }
                    ClearForm();
                }
                else
                {                   
                    GlobalMessageBox.Show(Messages.REQINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);                       
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnPrint_Click", null);
            }
        }

        private void RePrint2ndGradeInner_Load(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)                                  
            {
                ClearForm();
                txtInnerLotNo.Focus();
            }
        }
    }
}
