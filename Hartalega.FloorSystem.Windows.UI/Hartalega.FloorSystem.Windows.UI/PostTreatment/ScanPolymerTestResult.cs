using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;

namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{
    public partial class ScanPolymerTestResult : FormBase
    {

        #region Class Variables
        private static string _TscreenName = "Scan Polymer Test Results";
        private static string _screenName = "Post Treatment - ScanPolymerTestResult";
        private static string _className = "ScanPolymerTestResult";
        private string _serialNo;
        #endregion

        #region Page Load
        /// <summary>
        ///  Initializes a new instance of the <see cref="ScanPolymerTestResult" /> class.
        /// </summary>
        /// <param name="screenName"></param>
        public ScanPolymerTestResult()
        {
            InitializeComponent();
            try
            {
                txtLocation.Text = WorkStationDTO.GetInstance().Location;
                cmbResult.BindComboBox(CommonBLL.GetEnumText(Constants.RESULT), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanPolymerTestResult", string.Empty);
                return;
            }
        }

        /// <summary>
        ///  Display the Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanPolymerTestResult_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            txtSerialNo.SerialNo();
            txtTesterId.OperatorId();
            this.ActiveControl = txtTesterId;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Validate Tester Id & Get the Tester name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTesterId_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTesterId.Text.Trim()))
            {
                if (SecurityModuleBLL.ValidateOperatorAccess(txtTesterId.Text, _TscreenName))
                {                    
                    try
                    {
                        GetTesterName();
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtTesterId_Leave", txtTesterId.Text);
                        return;
                    }                   
                }
                else
                {
                    GlobalMessageBox.Show(Messages.TESTER_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    ClearTesterId();
                }
            }
            else
            {
                txtName.Text = String.Empty;
            }
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
                    try
                    {
                        ShowBatchDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()));
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
                    ClearControls(true);
                    ClearTesterId();
                }
            }
        }

        /// <summary>
        /// Get the Serial Number based on Reference Id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReference_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtReference.Text.Trim()))
            {
                if (txtReference.Text.Length == Constants.TEN)
                {
                    try
                    {
                        GetSerialNumber();
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtReference_Leave", txtReference.Text);
                        return;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_REFERENCE_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearControls(true);
                    ClearTesterId();
                }
            }
        }

        /// <summary>
        /// Saves the Polymer Test Result Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            TestSlipDTO objTest = new TestSlipDTO();
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtTesterId, "Tester ID", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtReference, "Reference", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbResult, "Result", ValidationType.Required));
            try
            {
                if (ValidateForm())
                {
                    GetTesterName();
                    if (!string.IsNullOrEmpty(txtTesterId.Text))
                    {
                        if (_serialNo != txtSerialNo.Text)
                        {
                            ShowBatchDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()));

                        }
                        if (!string.IsNullOrEmpty(txtSerialNo.Text))
                        {
                            validationMesssageLst = new List<ValidationMessage>();
                            validationMesssageLst.Add(new ValidationMessage(cmbResult, "Result", ValidationType.Required));
                            if (ValidateForm())
                            {
                                objTest.LocationId = WorkStationDTO.GetInstance().LocationId;
                                objTest.SerialNumber = Convert.ToDecimal(txtSerialNo.Text);
                                objTest.TesterID = txtTesterId.Text;
                                objTest.ReferenceId = Convert.ToDecimal(txtReference.Text);
                                objTest.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                                if (cmbResult.Text == Constants.PASS)
                                {
                                    objTest.Result = true;
                                }
                                else
                                {
                                    objTest.Result = false;
                                }
                                PostTreatmentBLL.SavePolymerTestResult(objTest);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearControls(true);
                                ClearTesterId();
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", objTest.LocationId, objTest.SerialNumber, objTest.TesterID, objTest.ReferenceId, objTest.LastModifiedOn, objTest.Result);
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
                ClearControls(true);
                ClearTesterId();
            }
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Shows the Batch details based on Serial Number
        /// </summary>
        private void ShowBatchDetails(decimal serialNo)
        {
            BatchDTO objBatch = CommonBLL.GetBatchScanInDetails(serialNo);
            if (objBatch != null)
            {
                string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNo);
                if (!string.IsNullOrEmpty(qaiStatus))
                {
                    GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearControls(true);
                    ClearTesterId();
                }
                else
                {
                    if (GetReferenceNumber())
                    {
                        txtBatchNo.Text = objBatch.BatchNumber;
                        txtGloveType.Text = objBatch.GloveType;
                        txtSize.Text = objBatch.Size;
                        btnSave.Enabled = true;
                        _serialNo = txtSerialNo.Text;
                        cmbResult.SelectedIndex = Constants.MINUSONE;
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearControls(true);
                        ClearTesterId();
                    }
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearControls(true);
                ClearTesterId();
            }
        }

        /// <summary>
        ///  Gets the Reference Number based on Serial No
        /// </summary>
        private bool GetReferenceNumber()
        {
            bool serialNoExists = false;
            string refNo = PostTreatmentBLL.GetReferenceNumber(Convert.ToDecimal(txtSerialNo.Text));
            if (!string.IsNullOrEmpty(refNo))
            {
                txtReference.Text = refNo.PadLeft(Constants.TEN, Convert.ToChar(Convert.ToString(Constants.ZERO)));
                serialNoExists = true;
            }
            return serialNoExists;
        }

        /// <summary>
        ///  Gets the Reference Number based on Serial No
        /// </summary>
        private void GetSerialNumber()
        {
            string serialNo = PostTreatmentBLL.GetSerialNumber(Convert.ToDecimal(txtReference.Text));
            if (!string.IsNullOrEmpty(serialNo))
            {
                string refNo = PostTreatmentBLL.GetReferenceNumber(Convert.ToDecimal(serialNo));
                if (!string.IsNullOrEmpty(refNo))
                {
                    txtSerialNo.Text = serialNo;
                    ShowBatchDetails(Convert.ToDecimal(txtSerialNo.Text));
                }
                else
                {
                    GlobalMessageBox.Show(Messages.DUPLICATE_REFERENCE_NUMBER_ALERT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearControls(true);
                    ClearTesterId();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_REFERENCE_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearControls(true);
                ClearTesterId();
            }

            //string serialNo = PostTreatmentBLL.GetSerialNumber(Convert.ToDecimal(txtReference.Text));
            //if (!string.IsNullOrEmpty(serialNo))
            //{
            //    txtSerialNo.Text = serialNo;
            //    ShowBatchDetails(Convert.ToDecimal(txtSerialNo.Text));
            //}
            //else
            //{
            //    GlobalMessageBox.Show(Messages.INVALID_REFERENCE_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK); 
            //    ClearControls(true);
            //    ClearTesterId();
            //}
        }

        /// <summary>
        /// Clear the Tester Id & Tester Name and set focus on Tester Id.
        /// </summary>
        private void ClearTesterId()
        {
            txtTesterId.Text = String.Empty;
            txtName.Text = String.Empty;
            txtTesterId.Focus();
        }

        /// <summary>
        /// Clear the values of controls
        /// </summary>
        private void ClearControls(bool clear)
        {
            txtSerialNo.Text = String.Empty;
            txtReference.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtGloveType.Text = String.Empty;
            if (clear)
                cmbResult.SelectedIndex = Constants.MINUSONE;
            btnSave.Enabled = false;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }

        /// <summary>
        /// Get the Tester Name
        /// </summary>
        private void GetTesterName()
        {
            string testerName = WasherBLL.GetOperatorName(txtTesterId.Text.Trim());
            if (!string.IsNullOrEmpty(testerName) && testerName != Constants.INVALID_MESSAGE)
            {
                txtName.Text = testerName;
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_TESTER_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearTesterId();
            }
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
            ClearControls(true);
            ClearTesterId();
        }
        #endregion

    }
}
