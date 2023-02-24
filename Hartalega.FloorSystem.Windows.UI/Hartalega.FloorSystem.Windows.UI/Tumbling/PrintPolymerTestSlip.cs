using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    public partial class PrintPolymerTestSlip : FormBase
    {
        #region Class Variables
        private bool _isRework; //For checking whether the batch comes for Rework or its the first time entry for the Batch. 
        private bool _isReprint;
        private bool _isReferenced = false;
        private string _oldReferenceNumber = String.Empty;
        private int _cntReprint = 0;
        private static string _screenName = "Tumbling - PrintPolymerTestSlip";
        private static string _className = "PrintPolymerTestSlip";
        #endregion

        #region Load Page
        /// <summary>
        ///  Initializes a new instance of the <see cref="PrintPolymerTestSlip" /> class.
        /// </summary>
        public PrintPolymerTestSlip()
        {
            InitializeComponent();
            txtSerialNo.SerialNo();
            txtTesterId.OperatorId();
            btnPrint.Enabled = false;
        }
        #endregion

        #region Load Form
        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPolymerTestSlip_Load(object sender, EventArgs e)
        {
            txtReworkCount.Visible = false;
            lblReworkCount.Visible = false;
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
            string operatorId = txtTesterId.Text.Trim();
            if (operatorId != String.Empty)
            {
                try
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(operatorId, _screenName))
                    {
                        txtName.Text = CommonBLL.GetOperatorNameQAI(operatorId);
                        if (txtName.Text == String.Empty)
                        {
                            GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtName.Text = String.Empty;
                            //lblOperatorName.Text = String.Empty;
                            txtTesterId.Focus();
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
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtTesterId_Leave", txtTesterId.Text);
                    return;
                }
            }
            else
                txtName.Text = String.Empty;
        }

        /// <summary>
        /// Get Batch Details based on Serial No
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                {
                    ShowBatchDetails();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Exclamation, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    SetFocusOnSerialNo();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Constants.AlertType.Exclamation, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                SetFocusOnSerialNo();
            }
        }

        /// <summary>
        /// Saves & Prints the Test Slip Details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtTesterId, Constants.OPERATORID, ValidationType.Required));
            if (ValidateForm())
            {
                SaveTestSlipDetails();
            }
        }

        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                SetFocusOnSerialNo();
            }
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Shows the Batch details based on Serial Number
        /// </summary>
        private void ShowBatchDetails()
        {
            try
            {
                bool IsExist = TumblingBLL.CheckSerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                if (IsExist)
                {
                    string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                    if (!string.IsNullOrEmpty(qaiStatus))
                    {
                        GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        SetFocusOnSerialNo();
                    }
                    else
                    {
                        BatchDTO objBatch = TumblingBLL.GetBatchScanInDetailsforPolymer(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                        if (objBatch != null)
                        {
                            GetStatusSerialNo(objBatch);
                            if (!_isReprint)
                            {
                                if (_isReferenced == false)
                                    GetReferenceNumber();
                                else
                                    txtReference.Text = _oldReferenceNumber;

                                txtBatchNo.Text = objBatch.BatchNumber;
                                txtGloveType.Text = objBatch.GloveType;
                                txtSize.Text = objBatch.Size;
                                btnPrint.Enabled = true;
                                txtTesterId.Text = String.Empty;
                                txtName.Text = String.Empty;
                                txtTesterId.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.POLYMERTEST_NOT_REQUIRED, Constants.AlertType.Exclamation, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            SetFocusOnSerialNo();
                        }
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Exclamation, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    SetFocusOnSerialNo();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ShowBatchDetails", txtSerialNo.Text);
            }
        }

        /// <summary>
        ///  Gets Status whether Serial No passes from Test & Record
        /// </summary>
        private void GetStatusSerialNo(BatchDTO objBatch)
        {
            TestSlipDTO objTestSlip = new TestSlipDTO();
            objTestSlip = TumblingBLL.GetStatusPolymerTest(Convert.ToDecimal(txtSerialNo.Text.Trim()));
            if (objTestSlip != null)
            {
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
                    }
                }
                else
                {
                    if (GlobalMessageBox.Show(Messages.SLIP_REPRINT, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        _isRework = false;
                        _isReprint = true;
                        _cntReprint = objTestSlip.Reprint;
                        _cntReprint++;

                        txtBatchNo.Text = objBatch.BatchNumber;
                        txtGloveType.Text = objBatch.GloveType;
                        txtSize.Text = objBatch.Size;
                        txtReference.Text = objTestSlip.ReferenceId.ToString().PadLeft(10, '0');
                        lblReworkCount.Visible = false;
                        txtReworkCount.Visible = false;
                        txtTesterId.Text = objTestSlip.TesterID;
                        txtName.Text = objTestSlip.Name;
                        txtReworkCount.Text = Convert.ToString(objTestSlip.ReworkCount);
                        TumblingBLL.UpdatePolymerTestSlip(Convert.ToDecimal(txtSerialNo.Text), _cntReprint, CommonBLL.GetCurrentDateAndTime());
                        PrintSlip(objTestSlip);
                        SetFocusOnSerialNo();
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
            else
            {
                _isRework = false;
                _isReprint = false;
            }

        }

        /// <summary>
        ///  Creates the Reference Number
        /// </summary>
        private void GetReferenceNumber()
        {
            try
            {
                txtReference.Text = PostTreatmentBLL.GetReferenceNumber(Constants.POLYMER_TEST).PadLeft(10, '0');
                _isReferenced = true;
                _oldReferenceNumber = txtReference.Text;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetReferenceNumber");
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
            try
            {
                TumblingBLL.SavePolymerTestSlip(objTest);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "SaveTestSlipDetails");
            }
            if (!_isReprint)
                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            _isReferenced = false;
            PrintSlip();
            SetFocusOnSerialNo();
        }

        /// <summary>
        /// Prints the Test Slip
        /// </summary>
        /// <param name="objTestSlip"></param>
        private void PrintSlip(TestSlipDTO objTestSlip = null)
        {
            try
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
                    printData.DateTime = CommonBLL.GetCurrentDateAndTime().ToLongTimeString();
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
                printData.TestSlipName = Constants.POLYMER_TEST;
                PostTreatmentBLL.PrintDetails(printData);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintSlip");
            }
            SetFocusOnSerialNo();
        }

        /// <summary>
        /// Clear the values of all the controls and set focus on Serial No.
        /// </summary>
        private void SetFocusOnSerialNo()
        {
            ClearControls();
             txtSerialNo.Focus();
        }

        /// <summary>
        /// Clear the values of controls
        /// </summary>
        private void ClearControls()
        {
            txtSerialNo.Text = String.Empty;
            txtReference.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtGloveType.Text = String.Empty;
            txtReworkCount.Text = String.Empty;
            txtTesterId.Text = String.Empty;
            txtName.Text = String.Empty;
            lblReworkCount.Visible = false;
            txtReworkCount.Visible = false;
            btnPrint.Enabled = false;
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
            else if (floorException.subSystem == Constants.DAL)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.INTEGRATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearControls();
        }
        #endregion
    }
}


