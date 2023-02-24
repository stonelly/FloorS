using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{
    public partial class PrintTestSlip : FormBase
    {

        #region Class Variables
        private bool _isRework; //For checking whether the batch comes for Rework or its the first time entry for the Batch. 
        private bool _isReprint;
        private bool _focusSerialNo = true;
        private int _cntReprint = Constants.ZERO;
        private static string _TscreenName = "Print Test Slip";
        private static string _screenName = "Post Treatment - PrintTestSlip";
        private static string _className = "PrintTestSlip";
        private static string _serialNo;
        private static string _testerName;
        private string _refNo;
        private bool _isSaved = true;
        #endregion

        #region Page Load
        /// <summary>
        ///  Initializes a new instance of the <see cref="PrintTestSlip" /> class.
        /// </summary>
        /// <param name="screenName"></param>
        public PrintTestSlip(string screenName)
        {
            InitializeComponent();
            grpBoxTestSlip.Text = screenName;
            this.Text = screenName;
            txtSerialNo.SerialNo();
            txtTesterId.OperatorId();
            btnPrint.Enabled = false;
        }

        /// <summary>
        ///  Display the Location 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintTestSlip_Load(object sender, EventArgs e)
        {
            txtReworkCount.Visible = false;
            lblReworkCount.Visible = false;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                txtWarehouseLocation.Text = WorkStationDTO.GetInstance().Location;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintTestSlip_Load", string.Empty);
                return;
            }

        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Get the Tester name based on Tester id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTesterId_Leave(object sender, EventArgs e)
        {
            GetOperatorName();
        }

        /// <summary>
        /// Get Batch Details based on Serial No
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            if (_focusSerialNo)
            {
                if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
                {
                    if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                    {
                        try
                        {
                            //Added by Tan Wei Wah 20190312
                            if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                            {
                                GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearControls();
                                return;
                            }
                            //Ended by Tan Wei Wah 20190312

                            ShowBatchDetails();
                        }
                        catch (FloorSystemException ex)
                        {
                            ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", txtSerialNo.Text);
                            return;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearControls();
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    ClearControls();
                }
            }
        }

        /// <summary>
        /// Saves & Prints the Test Slip Details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtTesterId, "Tester ID", ValidationType.Required));
                if (ValidateForm())
                {
                    if (_testerName != txtTesterId.Text)
                        GetOperatorName();
                    if (_serialNo != txtSerialNo.Text && !string.IsNullOrEmpty(txtTesterId.Text))
                        ShowBatchDetails();
                    if (!string.IsNullOrEmpty(txtTesterId.Text))
                        SaveTestSlipDetails();
                }
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnPrint_Click", string.Empty);
                return;
            }
        }

        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                ClearControls();
            }
        }

        /// <summary>
        /// Make the _focusSerialNo to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Leave(object sender, EventArgs e)
        {
            _focusSerialNo = true;
        }

        /// <summary>
        /// Make the _focusSerialNo to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Leave(object sender, EventArgs e)
        {
            _focusSerialNo = true;
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Shows the Batch details based on Serial Number
        /// </summary>
        private void ShowBatchDetails()
        {
            BatchDTO objBatch = CommonBLL.GetBatchScanInDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()));
            if (objBatch != null)
            {
                //string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                //if (!string.IsNullOrEmpty(qaiStatus))
                //{
                //    GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                //    ClearControls();
                //}
                //else
                //{
                if (GetStatusTestPrint(objBatch))
                {
                    GetStatusSerialNo(objBatch);
                    if (!_isReprint)
                    {
                        _focusSerialNo = false;
                        _serialNo = txtSerialNo.Text;
                        GetReferenceNumber();
                        if (!string.IsNullOrEmpty(_refNo))
                            txtReference.Text = _refNo;
                        txtBatchNo.Text = objBatch.BatchNumber;
                        txtGloveType.Text = objBatch.GloveType;
                        txtSize.Text = objBatch.Size;
                        btnPrint.Enabled = true;
                        btnPrint.Focus();

                    }
                }
                //}
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearControls();
            }
        }

        /// <summary>
        /// Get The Tester Name
        /// </summary>
        private void GetOperatorName()
        {
            if (!string.IsNullOrEmpty(txtTesterId.Text.Trim()))
            {
                if (SecurityModuleBLL.ValidateOperatorAccess(txtTesterId.Text, "Print Test Slip"))
                    {
                        try
                        {
                            string testerName = WasherBLL.GetOperatorName(txtTesterId.Text.Trim());
                            if (!string.IsNullOrEmpty(testerName) && testerName != Constants.INVALID_MESSAGE)
                            {
                                txtName.Text = testerName;
                                _testerName = txtTesterId.Text;
                                _focusSerialNo = true;
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.INVALID_TESTER_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearControls();
                            }
                        }
                        catch (FloorSystemException ex)
                        {
                            ExceptionLogging(ex, _screenName, _className, "GetOperatorName", txtTesterId.Text);
                            return;
                        }
                    }
                    else
                    {
                    GlobalMessageBox.Show(Messages.TESTER_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    txtTesterId.Text = String.Empty;
                    txtName.Text = String.Empty;
                    txtTesterId.Focus();
                }
            }
            else
            {
                txtName.Text = String.Empty;
            }
        }

        /// <summary>
        ///  Gets Status whether Serial No passes from Test & Record
        /// </summary>
        private void GetStatusSerialNo(BatchDTO objBatch)
        {
            TestSlipDTO objTestSlip = new TestSlipDTO();
            if (grpBoxTestSlip.Text == Constants.PRINT_PROTEIN_TEST)
                objTestSlip = PostTreatmentBLL.GetStatusProteinTest(Convert.ToDecimal(txtSerialNo.Text.Trim()));
            else if (grpBoxTestSlip.Text == Constants.PRINT_HOTBOX_TEST)
                objTestSlip = PostTreatmentBLL.GetStatusHotBoxTest(Convert.ToDecimal(txtSerialNo.Text.Trim()));
            else
                objTestSlip = PostTreatmentBLL.GetStatusPowderTest(Convert.ToDecimal(txtSerialNo.Text.Trim()));
            if (objTestSlip.Status == Constants.RECORD || String.IsNullOrEmpty(objTestSlip.Status))
            {
                if (!String.IsNullOrEmpty(objTestSlip.Status))
                {
                    _isRework = true;
                    _isReprint = false;
                    lblReworkCount.Visible = true;
                    txtReworkCount.Visible = true;
                    txtReworkCount.Text = Convert.ToString(objTestSlip.ReworkCount + Constants.ONE);
                }
                else
                {
                    _isRework = false;
                    _isReprint = false;
                    lblReworkCount.Visible = false;
                    txtReworkCount.Visible = false;
                    txtReworkCount.Text = string.Empty;
                }
            }
            else
            {
                string confirm = GlobalMessageBox.Show(Messages.SLIP_REPRINT, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                if (confirm == Constants.YES)
                {
                    _isRework = false;
                    _isReprint = true;
                    _cntReprint = objTestSlip.Reprint;
                    _cntReprint++;

                    txtBatchNo.Text = objBatch.BatchNumber;
                    txtGloveType.Text = objBatch.GloveType;
                    txtSize.Text = objBatch.Size;
                    txtReference.Text = objTestSlip.ReferenceId.ToString().PadLeft(Constants.TEN, Convert.ToChar(Convert.ToString(Constants.ZERO)));
                    lblReworkCount.Visible = false;
                    txtReworkCount.Visible = false;
                    txtReworkCount.Text = Convert.ToString(objTestSlip.ReworkCount);
                    DateTime currentDate = CommonBLL.GetCurrentDateAndTime();
                    if (grpBoxTestSlip.Text == Constants.PRINT_PROTEIN_TEST)
                        PostTreatmentBLL.UpdateProteinTestSlip(Convert.ToDecimal(txtSerialNo.Text), _cntReprint, currentDate);
                    else if (grpBoxTestSlip.Text == Constants.PRINT_HOTBOX_TEST)
                        PostTreatmentBLL.UpdateHotBoxTestSlip(Convert.ToDecimal(txtSerialNo.Text), _cntReprint, currentDate);
                    else
                        PostTreatmentBLL.UpdatePowderTestSlip(Convert.ToDecimal(txtSerialNo.Text), _cntReprint, currentDate);
                    PrintSlip(objTestSlip);
                }
                else
                {
                    _isRework = true;
                    _isReprint = false;
                    lblReworkCount.Visible = true;
                    txtReworkCount.Visible = true;
                    txtReworkCount.Text = Convert.ToString(objTestSlip.ReworkCount + Constants.ONE);
                }
            }
        }

        /// <summary>
        ///  Gets Status whether to print test slip or not.
        /// </summary>
        private bool GetStatusTestPrint(BatchDTO objBatch)
        {
            bool isValidGloveType;
            if (grpBoxTestSlip.Text == Constants.PRINT_PROTEIN_TEST)
            {
                isValidGloveType = Convert.ToBoolean(objBatch.Protein);
                if (!isValidGloveType)
                {
                    GlobalMessageBox.Show(Messages.INVALID_GLOVETYPE_PROTEIN, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearControls();
                }
            }
            else if (grpBoxTestSlip.Text == Constants.PRINT_HOTBOX_TEST)
            {
                isValidGloveType = Convert.ToBoolean(objBatch.HotBox);
                if (!isValidGloveType)
                {
                    GlobalMessageBox.Show(Messages.INVALID_GLOVETYPE_HOTBOX, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearControls();
                }
            }
            else
            {
                isValidGloveType = Convert.ToBoolean(objBatch.Powder);
                if (!isValidGloveType)
                {
                    GlobalMessageBox.Show(Messages.INVALID_GLOVETYPE_POWDER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearControls();
                }
            }
            return isValidGloveType;
        }

        /// <summary>
        ///  Creates the Reference Number
        /// </summary>
        private void GetReferenceNumber()
        {
            if (_isSaved)
            {
            if (grpBoxTestSlip.Text.Contains(Constants.PROTEIN_TEST))
            {
                txtReference.Text = Constants.PROTEIN_POWDER_REFERENCE + PostTreatmentBLL.GetReferenceNumber(Constants.PROTEIN_TEST).PadLeft(Constants.NINE, Convert.ToChar(Convert.ToString(Constants.ZERO)));
            }
            else if (grpBoxTestSlip.Text.Contains(Constants.POWDER_TEST))
            {
                txtReference.Text = Constants.PROTEIN_POWDER_REFERENCE + PostTreatmentBLL.GetReferenceNumber(Constants.POWDER_TEST).PadLeft(Constants.NINE, Convert.ToChar(Convert.ToString(Constants.ZERO)));
            }
            else if (grpBoxTestSlip.Text.Contains(Constants.HOTBOX_TEST))
            {
                txtReference.Text = Constants.HOTBOX_REFERENCE + PostTreatmentBLL.GetReferenceNumber(Constants.HOTBOX_TEST).PadLeft(Constants.NINE, Convert.ToChar(Convert.ToString(Constants.ZERO)));
            }
                _refNo = txtReference.Text;
                _isSaved = false;
        }
        }

        /// <summary>
        /// Save the Test Slip details
        /// </summary>
        private void SaveTestSlipDetails()
        {
            TestSlipDTO objTest = new TestSlipDTO();
            objTest.LocationId = WorkStationDTO.GetInstance().LocationId;
            objTest.TesterID = txtTesterId.Text;
            objTest.SerialNumber = Convert.ToDecimal(txtSerialNo.Text);
            objTest.ReferenceId = Convert.ToDecimal(txtReference.Text);
            if (_isRework)
                objTest.ReworkCount = Convert.ToInt32(txtReworkCount.Text);
            else
                objTest.ReworkCount = Constants.ZERO;
            objTest.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
            objTest.Reprint = Constants.ZERO;
            objTest.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
            if (grpBoxTestSlip.Text == Constants.PRINT_PROTEIN_TEST)
                PostTreatmentBLL.SaveProteinTestSlip(objTest);
            else if (grpBoxTestSlip.Text == Constants.PRINT_HOTBOX_TEST)
                PostTreatmentBLL.SaveHotBoxTestSlip(objTest);
            else
                PostTreatmentBLL.SavePowderTestSlip(objTest);
            if (!_isReprint)
                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            _isSaved = true;
            PrintSlip();
        }

        /// <summary>
        /// Prints the Test Slip
        /// </summary>
        /// <param name="objTestSlip"></param>
        private void PrintSlip(TestSlipDTO objTestSlip = null)
        {

            BatchDTO objBatchDTO = PostTreatmentBLL.GetUpdatedBatchWeight(Convert.ToDecimal(txtSerialNo.Text));
            PrintTestSlipDTO printData = new PrintTestSlipDTO();
            printData.BatchNumber = txtBatchNo.Text;
            printData.SerialNumber = txtSerialNo.Text;
            printData.BatchWeight = objBatchDTO.BatchWeight.ToString();
            printData.HundredPcsWeight = (objBatchDTO.TenPcsWeight * Constants.TEN).ToString();
            if (objTestSlip != null)
            {
                printData.DateTime = objTestSlip.LastModifiedOn.ToLongTimeString();
                printData.TesterId = objTestSlip.TesterID.ToString();
                printData.TesterName = objTestSlip.Name;
                printData.ReworkCount = txtReworkCount.Text;
            }
            else
            {
                printData.DateTime = usrDateControl.DateValue.ToLongTimeString();
                printData.TesterId = txtTesterId.Text;
                printData.TesterName = txtName.Text;
                if (_isRework)
                    printData.ReworkCount = txtReworkCount.Text;
                else
                    printData.ReworkCount = Constants.ZERO.ToString();
            }
            printData.Size = txtSize.Text;
            printData.GloveType = txtGloveType.Text;
            printData.ReferenceId = txtReference.Text;

            if (grpBoxTestSlip.Text == Constants.PRINT_PROTEIN_TEST)
            {
                printData.TestSlipName = Constants.PROTEIN_TEST;
            }
            else if (grpBoxTestSlip.Text == Constants.PRINT_HOTBOX_TEST)
            {
                printData.TestSlipName = Constants.HOTBOX_TEST;
                PrintTestSlipDTO printDataHotBox = PostTreatmentBLL.GetBatchDetailsForHotBox(Convert.ToDecimal(txtSerialNo.Text));
                if (printDataHotBox != null)
                {
                    printData.PTDate = printDataHotBox.PTDate;
                    printData.WasherProgram = printDataHotBox.WasherProgram;
                    printData.WasherNumber = printDataHotBox.WasherNumber;
                    printData.DryerProgram = printDataHotBox.DryerProgram;
                    printData.DryerNumber = printDataHotBox.DryerNumber;
                }
            }
            else
            {
                printData.TestSlipName = Constants.POWDER_TEST;
            }
            PostTreatmentBLL.PrintDetails(printData);
            ClearControls();
        }

        /// <summary>
        /// Clear the values of controls
        /// </summary>
        private void ClearControls()
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
            _focusSerialNo = false;
            txtTesterId.Text = String.Empty;
            txtName.Text = String.Empty;
            txtTesterId.Focus();
            txtSerialNo.Text = String.Empty;
            txtReference.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtGloveType.Text = String.Empty;
            txtReworkCount.Text = String.Empty;
            lblReworkCount.Visible = false;
            txtReworkCount.Visible = false;
            btnPrint.Enabled = false;
            _focusSerialNo = true;
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearControls();
        }
        #endregion


    }
}
