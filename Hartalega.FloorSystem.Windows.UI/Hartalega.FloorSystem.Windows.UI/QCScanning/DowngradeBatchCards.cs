using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Collections.Generic;
using Hartalega.FloorSystem.IntegrationServices;
using System.Drawing;


namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    /// <summary>
    /// Module: QC Scanning
    /// Screen Name: DowngradeBatchcard
    /// File Type: Code file
    /// </summary>
    public partial class DowngradeBatchCards : FormBase
    {
        #region Member Variables
        private string _screenName = "QC Scanning - DowngradeBatchCards";
        private string _className = "DowngradeBatchCards";
        private Int64 _totalPcs;
        private static bool _issaved;
        private string _downgradeType;
        #endregion
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public DowngradeBatchCards()
        {
            InitializeComponent();
            _issaved = false;
        }

        #region User Methods


        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtSerialNo.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtGloveType.Text = string.Empty;
            txtSize.Text = string.Empty;
            txtQCType.Text = string.Empty;
            txtBatchDate.Text = string.Empty;
            txtBatchTime.Text = string.Empty;
            txtQCTypeDescription.Text = string.Empty;
            txtTenPcs.Text = string.Empty;
            txtBatchWeight.Text = string.Empty;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
            btnCancel.Enabled = true;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
                return;
            }
            BindDowngradeType();
            cmbDowngradeType.SelectedIndex = Constants.MINUSONE;
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
            else if (floorException.subSystem == Constants.SERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.SERVICE_UNAVAILABLE, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);

            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Functionality to be performed on save of data
        /// </summary>
        private void SaveData()
        {
            int rowsReturned = 0;
            DowngradeBatchCardDTO objDowngradeDTO = new DowngradeBatchCardDTO();
            try
            {
                objDowngradeDTO.SerialNumber = txtSerialNo.Text.Trim();
                objDowngradeDTO.LastModifiedBy = string.Empty;
                objDowngradeDTO.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                objDowngradeDTO.DowngradeType = _downgradeType;
                objDowngradeDTO.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;

                rowsReturned = QCScanningBLL.SaveDowngradeBatchCard(objDowngradeDTO);


                if (rowsReturned > 0)
                {
                    if (objDowngradeDTO.DowngradeType != Constants.DOWNGRADE_REJECT)
                    {
                        // AX Posting
                        if (!CommonBLL.ValidateAXPosting(Convert.ToDecimal(objDowngradeDTO.SerialNumber), CreateInvMovJournalFunctionidentifier.DBC2G.ToString()))
                        {
                            //bool isPostingSuccess = AXPostingBLL.PostAXDataQCScanDowngradeBatchCard(Convert.ToDecimal(objDowngradeDTO.SerialNumber));
                            bool isPostingSuccess = true; // D365 FS don't post for downgradedbatch card
                            if (!isPostingSuccess)
                            {
                                QCScanningBLL.DeleteDowngradeBatchCardData(Convert.ToDecimal(objDowngradeDTO.SerialNumber));
                                _issaved = false;
                            }
                            else
                            {
                                _issaved = true;
                            }
                        }
                        else
                        {
                            _issaved = true;
                        }
                    }
                    else
                    {
                        _issaved = true;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }

            catch (FloorSystemException ex)
            {
                QCScanningBLL.DeleteDowngradeBatchCardData(Convert.ToDecimal(objDowngradeDTO.SerialNumber));
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        /// <summary>
        /// Method to identify status of Save
        /// </summary>
        private void ShowStatus(IAsyncResult res)
        {
            if (_issaved)
            {
                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                ClearForm();
            }
        }

        #endregion

        #region Fill Sources
        /// <summary>
        /// Binds the Downgrade type ComboBox
        /// </summary>
        private void BindDowngradeType()
        {
            try
            {
                cmbDowngradeType.BindComboBox(CommonBLL.GetEnumText(Constants.DOWNGRADE_TYPE), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindDowngradeType", null);
                return;
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbDowngradeType.Leave -= new System.EventHandler(cmbDowngradeType_Leave);

            if (_issaved)
            {
                ClearForm();
            }

            if (GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
            }
            cmbDowngradeType.Leave += new System.EventHandler(cmbDowngradeType_Leave);
        }

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender">Downgrade Batch card Page</param>
        /// <param name="e">On load event argument</param>
        private void DowngradeBatchCards_Load(object sender, EventArgs e)
        {
            txtSerialNo.SerialNo();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "DowngradeBatchCards_Load", null);
                return;
            }
        }

        /// <summary>
        /// Serial No text box leave event
        /// </summary>
        /// <param name="sender">Serial No text box</param>
        /// <param name="e">On leave event argument</param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            string qaiStatus = string.Empty;
            bool isBatchDowngraded = false;

            if (string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                ValidateForm();
                ClearForm();
            }
            else
            {
                Int64 serialNumber = Int64.Parse(txtSerialNo.Text.Trim());
                try
                {
                    //Fill Data Sources
                    BindDowngradeType();

                    BatchDTO resultDTO = QCPackingYieldBLL.GetBatchScanInDetails(serialNumber);
                    if (resultDTO != null)
                    {
                        isBatchDowngraded = Convert.ToBoolean(QCScanningBLL.IsBatchDowngraded(serialNumber));

                        if (!isBatchDowngraded)
                        {
                            qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber);
                            if (!string.IsNullOrEmpty(qaiStatus))
                            {
                                GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                            else
                            {
                                txtBatchNo.Text = resultDTO.BatchNumber;
                                txtGloveType.Text = resultDTO.GloveType;
                                txtSize.Text = resultDTO.Size;
                                txtQCType.Text = resultDTO.QCType;
                                txtBatchDate.Text = resultDTO.ShortDate;
                                txtBatchTime.Text = resultDTO.ShortTime;
                                txtQCTypeDescription.Text = resultDTO.QCTypeDescription;
                                txtTenPcs.Text = string.Format(Constants.QC_DECIMAL_FORMAT, Math.Round(resultDTO.TenPcsWeight, 2));
                                txtBatchWeight.Text = string.Format(Constants.QC_DECIMAL_FORMAT, Math.Round(resultDTO.BatchWeight, 2));
                                _totalPcs = resultDTO.TotalPcs;
                                btnSave.Enabled = true;
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BATCH_ALREADY_DOWNGRADED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", null);
                    return;
                }
            }
        }

        /// <summary>
        /// Save button Click event
        /// </summary>
        /// <param name="sender">Save button</param>
        /// <param name="e">On click event argument</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            cmbDowngradeType.Leave -= new System.EventHandler(cmbDowngradeType_Leave);

            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbDowngradeType, "Downgrade Type", ValidationType.Required));
            bool isValid = ValidateForm();

            if (isValid)
            {
                if (GlobalMessageBox.Show(Messages.DOWNGRADE_CONFIRMATION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    _downgradeType = Convert.ToString(cmbDowngradeType.SelectedValue);
                    MethodInvoker mI = delegate
            {
                SaveData();
            };
                    mI.BeginInvoke(ShowStatus, null);
                }
                else
                {
                    ClearForm();
                }
            }
            cmbDowngradeType.Leave += new System.EventHandler(cmbDowngradeType_Leave);
        }

        /// <summary>
        /// Downgrade type combo box leave event 
        /// </summary>
        /// <param name="sender">Downgrade type combo box leave event</param>
        /// <param name="e">On change event argument</param>
        private void cmbDowngradeType_Leave(object sender, EventArgs e)
        {
            if (!btnCancel.Focused)
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(cmbDowngradeType, "Downgrade Type", ValidationType.Required));
                ValidateForm();
            }
        }


        #endregion
    }
}
