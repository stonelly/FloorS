#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    public partial class EditOnlineBatchCardInfo : QAIBase
    {

        #region  private Variables

        private static string _screenName = Constants.EDIT_ONLINE_BATCH_CARD_INFO;
        private static string _className = "EditOnlineBatchCardInfo";
        private int _qAIExpiryDays = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
        #endregion

        #region Public Variables
        public Constants.QAIPageTransition _QAITranistion { get; set; }

        #endregion

        #region Load Form

        public EditOnlineBatchCardInfo()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "EditOnlineBatchCardInfo", null);
            }
            this.Text = Constants.EDIT_ONLINE_BATCH_CARD_INFO;
            groupBox1.Text = Constants.EDIT_ONLINE_BATCH_CARD_INFO;
            btnSave.Enabled = false;
            BindPackingSize();
            txtQaiInspectorId.OperatorId();
            txtSerialNo.SerialNo();
            txtInnerBox.InnerBoxCount();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// txtQaiInspectorId Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQaiInspectorId_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            string operatorName = string.Empty;
            if (!string.IsNullOrEmpty(txtQaiInspectorId.Text.Trim()))
            {
                try
                {
                    operatorName = CommonBLL.GetOperatorNameQAI((txtQaiInspectorId.Text).Trim());
                    if (!string.IsNullOrEmpty(operatorName))
                    {
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtQaiInspectorId.Text.Trim(), Constants.SCAN_INNER_TENPCS))
                        {
                            ISPageValid = true;
                            txtQaiInspectorName.Text = operatorName;
                            txtQaiInspectorName.Enabled = false;
                            //commented by MYAdamas 20171127 due to shift +tab not working
                            // txtSerialNo.Focus();
                        }
                        else
                        {
                            txtQaiInspectorId.Text = string.Empty;
                            GlobalMessageBox.Show(Messages.QAI_SCREEN_ACCESS_MSG, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                            txtQaiInspectorName.Text = string.Empty;
                            txtQaiInspectorId.Focus();
                        }
                    }
                    else
                    {
                        txtQaiInspectorId.Text = string.Empty;
                        GlobalMessageBox.Show(Messages.QAI_SCREEN_ACCESS_MSG, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        txtQaiInspectorName.Text = string.Empty;
                        txtQaiInspectorId.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtQaiInspectorId_Leave", Convert.ToInt16(txtQaiInspectorId.Text));
                    return;
                }
            }
            else
            {
                txtQaiInspectorName.Text = string.Empty;
                GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID_QAIINSPECTOR, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                txtQaiInspectorId.Text = string.Empty;
                txtQaiInspectorId.Focus();
            }
        }

        /// <summary>
        /// txtSerialNo Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            btnSave.Enabled = false;
            txtBatchNo.Text = string.Empty;
            lblGlovecode.Text = string.Empty;
            txtQcType.Text = string.Empty;
            txtQcTypeDescription.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtInnerBox.Text = string.Empty;
            txt10pcweight.Text = string.Empty;
            cmbPackingSize.SelectedIndex = -1;
            txtWTSamplingSize.Text = string.Empty;
            txtVTSamplingSize.Text = string.Empty;
            txtHBQty.Text = string.Empty;

            if (!string.IsNullOrEmpty(txtSerialNo.Text))
            {
                //Added by Tan Wei Wah 20190312
                if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                {
                    GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    return;
                }
                //Ended by Tan Wei Wah 20190312

                QAIDTO qaiBatach = null;
                //qaiBatach = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                //commented by MYAdamas to get batch details by serial number and screen name QAIScanInnerTenPcs/EditOnlineBatchCard to populate the current inner box and packing size
                qaiBatach = CommonBLL.GetBatchNumberBySerialNumberEditOnlineBatchCard(Convert.ToDecimal(txtSerialNo.Text.Trim()), Constants.QAIScreens.QAIScanInnerTenPcs.ToString(), Constants.QAIScreens.EditOnlineBatchCardInfo.ToString());
                
                if (qaiBatach != null && string.IsNullOrEmpty(qaiBatach.BatchNo))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    txtSerialNo.Text = string.Empty;
                    txtSerialNo.Focus();
                    return;
                }
                else
                {
                    if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerialNo.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                    }
                    else if (!(QAIBLL.GetPTStatusForPWT(Convert.ToInt64(txtSerialNo.Text.Trim()))))
                    {
                        GlobalMessageBox.Show(Messages.QAI_PWT_PT_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (!qaiBatach.QAIDate.HasValue)
                    {
                        GlobalMessageBox.Show(Messages.QAI_NOT_COMPLETED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                    else if (CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(qaiBatach.SerialNo)) == Messages.QAI_EXPIRED)
                    {
                        GlobalMessageBox.Show(Messages.QAI_EXPIRED, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        txtSerialNo.Text = string.Empty;
                        txtSerialNo.Focus();
                        return;
                    }
                
                    else
                    {
                        ISPageValid = true;
                        txtBatchNo.Text = qaiBatach.BatchNo;
                        lblGlovecode.Text = qaiBatach.GloveType;
                        GloveType = qaiBatach.GloveType;
                        txtQcType.Text = qaiBatach.QCType;
                        txtQcTypeDescription.Text = QAIBLL.GetQCtypeValue(CommonBLL.GetQCType(), qaiBatach.QCType);
                        txtInnerBox.Text = qaiBatach.InnerBoxes.ToString();
                        txt10pcweight.Text = qaiBatach.TenPcsWeight.ToString();
                        txtWTSamplingSize.Text = qaiBatach.WTSamplingSize;
                        txtVTSamplingSize.Text = qaiBatach.VTSamplingSize;
                        txtHBQty.Text = qaiBatach.HBSamplingSize;
                        cmbPackingSize.SelectedValue = qaiBatach.PackingSize;

                        btnSave.Enabled = true;
                         
                    }
                }
            }
            else
            {
                txtSerialNo.Text = string.Empty;
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                txtSerialNo.Focus();
            }
        }

        /// <summary>
        /// Clear all the editable fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string result = GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                ClearForm();
            }
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnCancel_Click", null);
            }
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Bind Packing Size
        /// </summary>
        private void BindPackingSize()
        {
            try
            {
                cmbPackingSize.BindComboBox(CommonBLL.GetPackingSize(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQcType", null);
                return;
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
            else if (floorException.subSystem == Constants.DAL)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                if (uiControl == "getTenPcsWeight")
                    GlobalMessageBox.Show("(" + result + ") - " + Constants.TENPCS_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        private void QAINavigation(Constants.QAIPageTransition qaitranistion)
        {
            if (qaitranistion == Constants.QAIPageTransition.FormClose)
            {
                this.Close();
            }

            if (qaitranistion == Constants.QAIPageTransition.FormEscape)
            {
                ClearForm();
            }
        }

        private void ClearForm()
        {
            this.txtSerialNo.Leave -= new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave -= new EventHandler(txtQaiInspectorId_Leave);
            BindPackingSize();
            
            txtQaiInspectorId.Text = string.Empty;
            txtQaiInspectorName.Text = string.Empty;
            lblGlovecode.Text = string.Empty;
            txtQcType.Text = string.Empty;
            txtQcTypeDescription.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtInnerBox.Text = string.Empty;
            txt10pcweight.Text = string.Empty;
            cmbPackingSize.SelectedIndex = -1;
            txtWTSamplingSize.Text = string.Empty;
            txtVTSamplingSize.Text = string.Empty;
            txtHBQty.Text = string.Empty;
            txtQaiInspectorId.Focus();
            btnSave.Enabled = false;
            txt10pcweight.Text = string.Empty;

            this.txtSerialNo.Leave += new EventHandler(txtSerialNo_Leave);
            this.txtQaiInspectorId.Leave += new EventHandler(txtQaiInspectorId_Leave);
        }

        #endregion
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtInnerBox, "Inner Box:", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPackingSize, "Packing Size:", ValidationType.Required));
            if (ValidateForm())
            {
                if (QAIBLL.QAIEditBatchCardInfoSave(txtSerialNo.Text.Trim(), txtQaiInspectorId.Text.Trim(), Convert.ToInt32(txtInnerBox.Text), Convert.ToInt32(cmbPackingSize.SelectedValue), WorkStationDTO.GetInstance().WorkStationId, Constants.QAIScreens.QAIScanInnerTenPcs.ToString(), Constants.QAIScreens.EditOnlineBatchCardInfo.ToString()) > 0)
                {
                    //event log myadamas 20190227
                    EventLogDTO EventLog = new EventLogDTO();

                    EventLog.CreatedBy = Convert.ToString(txtQaiInspectorId.Text.Trim());
                    Constants.EventLog audAction = Constants.EventLog.Save;
                    EventLog.EventType = Convert.ToInt32(audAction);
                    EventLog.EventLogType = Constants.eventlogtype;

                    var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                    CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_QAI, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
        }
    }
}
