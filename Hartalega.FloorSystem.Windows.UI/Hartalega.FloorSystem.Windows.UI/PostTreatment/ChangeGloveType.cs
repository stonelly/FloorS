using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Transactions;

namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{
    /// <summary>
    /// Module: Post Treatment
    /// Screen Name: Change Glove Type
    /// File Type: Code file
    /// </summary> 
    public partial class ChangeGloveType : FormBase
    {

        #region Class Variables
        private static string _screenName = "Post Treatment - ChangeGloveType";
        private static string _className = "ChangeGloveType";
        private static string _serialNo;
        private bool _isSave;
        private decimal _remainingGloveQty = 0;
        #endregion


        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeGloveType" /> class.
        /// </summary>
        public ChangeGloveType()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form load event - Get the current Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeGloveType_Load(object sender, EventArgs e)
        {
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            this.ActiveControl = txtSerialNo;
            txtSerialNo.SerialNo();
            btnSave.Enabled = false;
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Get the Change Glove Type based on Glove Type & bind the Change Glove Type ComboBox
        /// </summary>
        /// <param name="gloveType"></param>
        private void GetGloveType()
        {
            cmbChangeGloveType.BindComboBox(CommonBLL.GetGloveType(txtGlovetype.Text), true);
            txtChangeGloveTypeDesc.Text = String.Empty;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Checks whether Serial Number is valid and gets the Batch Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            GetBatchDetails();
        }

        /// <summary>
        /// Get the Change Glove Type Description based on Change Glove Type selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChangeGloveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string desc = CommonBLL.GetGloveDescription(cmbChangeGloveType.Text);
                if (desc != Constants.INVALID_MESSAGE)
                {
                    txtChangeGloveTypeDesc.Text = desc;
                    cmbBatchOrder.BindComboBox(CommonBLL.GetBatchOrderByGloveTypeAndLocation(WorkStationDTO.GetInstance().Location, cmbChangeGloveType.Text, txtSize.Text), true); //#Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbChangeGloveType_SelectedIndexChanged", cmbChangeGloveType.Text);
                return;
            }
        }

        /// <summary>
        ///  Validate & Save the Batch Card Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DateTime lastModified = PostTreatmentBLL.GetLastModifiedPT(Convert.ToDecimal(txtSerialNo.Text));
            PostTreatmentDTO objPT = new PostTreatmentDTO();
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbChangeGloveType, "Change Glove Type", ValidationType.Required));
            try
            {
                if (ValidateForm())
                {
                    if (_serialNo != txtSerialNo.Text)
                    {
                        GetBatchDetails();
                    }
                    validationMesssageLst = new List<ValidationMessage>();
                    validationMesssageLst.Add(new ValidationMessage(cmbChangeGloveType, "Change Glove Type", ValidationType.Required));
                    if (!string.IsNullOrEmpty(txtSerialNo.Text) && ValidateForm())
                    {
                        if (!(string.IsNullOrEmpty(cmbBatchOrder.Text)))
                        {
                            _remainingGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder.Text + "'").ToString());
                            if (_remainingGloveQty == 0)
                            {
                                GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                return;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BATCHORDER_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                            cmbBatchOrder.Focus();
                            return;
                        }
                        objPT.SerialNumber = Convert.ToDecimal(txtSerialNo.Text.Trim());
                        objPT.ChangeGloveType = Convert.ToString(cmbChangeGloveType.SelectedValue);
                        objPT.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                        objPT.OldGloveType = txtGlovetype.Text;
                        objPT.Id = Constants.ONE;
                        objPT.WorkstationNumber = WorkStationDTO.GetInstance().WorkStationId;
                        objPT.BatchOrder = cmbBatchOrder.Text; // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
                        objPT.OldBatchOrder = CommonBLL.GetBatchOrderBySerialNo(Convert.ToDecimal(txtSerialNo.Text.Trim())); // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
                        PostTreatmentBLL.SaveScanPTBatch(objPT);
                        _isSave = true;
                        bool isPostingSuccess = AXPostingBLL.PostAXDataPTChangeGloveType(Convert.ToDecimal(txtSerialNo.Text), objPT.OldGloveType); //,"New Glove"); // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product)
                        if (isPostingSuccess)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                        else
                        {
                            PostTreatmentBLL.SaveLastModifiedPT(Convert.ToDecimal(txtSerialNo.Text), lastModified, txtGlovetype.Text,
                                cmbChangeGloveType.Text, objPT.LastModifiedOn);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                        ClearForm();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                if (_isSave)
                {
                    PostTreatmentBLL.SaveLastModifiedPT(Convert.ToDecimal(txtSerialNo.Text), lastModified, txtGlovetype.Text,
                               cmbChangeGloveType.Text, objPT.LastModifiedOn);
                }
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", objPT.SerialNumber, objPT.ChangeGloveType, objPT.LastModifiedOn, objPT.OldGloveType, objPT.Id);
                return;
            }
        }

        /// <summary>
        ///  Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                ClearForm();
            }
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Get the Batch Details
        /// </summary>
        private void GetBatchDetails()
        {
            bool isPTPFGlove = false;
            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (Validator.IsValidInput(Constants.ValidationType.Integer, txtSerialNo.Text.Trim()) & txtSerialNo.Text.Length == Constants.SERIAL_LENGTH)
                {
                    try
                    {
                        //Added by Tan Wei Wah 20190312
                        if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                        {
                            GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }
                        //Ended by Tan Wei Wah 20190312

                        // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product) START
                        string ptStatus = QCPackingYieldBLL.GetPTStatus(Convert.ToInt64(txtSerialNo.Text.Trim()));
                        if (string.Compare(ptStatus, Constants.PT_INCOMPLETE) == Constants.ZERO)
                        {
                            GlobalMessageBox.Show(Messages.PT_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }
                        else if (string.Compare(ptStatus, Constants.PTQI_INCOMPLETE) == Constants.ZERO)
                        {
                            GlobalMessageBox.Show(Messages.PTQI_INCOMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            return;
                        }

                        isPTPFGlove = WasherBLL.CheckIsPTPFGlove(Convert.ToDecimal(txtSerialNo.Text.Trim())); 
                        if (isPTPFGlove) //only PTPF glove with after QCQI = pass can proceed to change glove type
                        {
                            QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToInt64(txtSerialNo.Text.Trim()));
                            bool blockDueToQIIncomplete = false;
                            string errorMessage = "";
                            if (!string.IsNullOrEmpty(qiTestResultStatus.Stage))
                            {
                                if (qiTestResultStatus.Stage == Constants.PT_QI)
                                {
                                    // PTQI without QC QAI test result
                                    errorMessage = Messages.QCQI_INCOMPLETE;
                                    blockDueToQIIncomplete = true;
                                }
                                else if (qiTestResultStatus.Stage == Constants.QC_QI && (qiTestResultStatus.QIResult.ToUpper() == Constants.FAIL || (string.IsNullOrEmpty(qiTestResultStatus.QIResult))))
                                {
                                    // QCQI fail or no QC QAI test result
                                    errorMessage = Messages.QCQI_INCOMPLETE;
                                    blockDueToQIIncomplete = true;
                                }
                            }
                            else
                            {
                                // no QI test result
                                errorMessage = Messages.QCQI_INCOMPLETE;
                                blockDueToQIIncomplete = true;
                            }
                            if (blockDueToQIIncomplete)
                            {
                                GlobalMessageBox.Show(errorMessage, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                                return;
                            }
                        }
                        // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product) END

                        string shiftName = PostTreatmentBLL.GetShiftName(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                        BatchDTO objBatch = CommonBLL.GetBatchScanInDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                        if (objBatch != null)
                        {
                            string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                            if (!string.IsNullOrEmpty(qaiStatus))
                            {
                                GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                            else
                            {
                                txtBatchNo.Text = objBatch.BatchNumber;
                                txtGlovetype.Text = objBatch.GloveType;
                                txtSize.Text = objBatch.Size;
                                txtQCType.Text = objBatch.QCType;
                                txtQCTypeDesc.Text = objBatch.QCTypeDescription;
                                _serialNo = txtSerialNo.Text;
                                if (objBatch.PTBatchWeight > Constants.ZERO)
                                {
                                    txtBatchWeight.Text = Math.Round(objBatch.PTBatchWeight, Constants.TWO).ToString("#,##0.00");
                                    txtTenPcs.Text = Math.Round(objBatch.PTTenPcsWeight, Constants.TWO).ToString("#,##0.00");
                                }
                                else
                                {
                                    txtBatchWeight.Text = Math.Round(objBatch.BatchWeight, Constants.TWO).ToString("#,##0.00");
                                    txtTenPcs.Text = Math.Round(objBatch.TenPcsWeight, Constants.TWO).ToString("#,##0.00");
                                }
                                if (!String.IsNullOrEmpty(shiftName))
                                {
                                    txtShift.Text = shiftName;
                                }
                                else
                                {
                                    txtShift.Text = objBatch.ShiftName;
                                }
                                GetGloveType();
                                btnSave.Enabled = true;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "GetBatchDetails", txtSerialNo.Text.Trim());
                        return;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                ClearForm();
            }
        }

        /// <summary>
        /// Clear the values of all the controls. 
        /// </summary>
        private void ClearForm()
        {
            txtSerialNo.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtGlovetype.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtQCType.Text = String.Empty;
            txtQCTypeDesc.Text = String.Empty;
            txtShift.Text = String.Empty;
            txtTenPcs.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            cmbChangeGloveType.SelectedIndex = Constants.MINUSONE;
            txtChangeGloveTypeDesc.Text = String.Empty;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
            CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            cmbBatchOrder.SelectedIndex = Constants.MINUSONE; // #Azrul 2021-11-18 - HTLG_HSB_002: Special Glove (Clean Room Product) START
            _isSave = false;
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
            else if (floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        #endregion

    }
}
