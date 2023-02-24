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

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    //test
    /// <summary>
    /// Module: QC Scanning
    /// Screen Name: Defective Glove (Platform)
    /// File Type: Code file
    /// </summary>
    public partial class DefectiveGlovePlatform : FormBase
    {
        #region Class Variables
        private int _qcGroupId;
        private string _size;
        private bool _isPlatform = true;
        private string _moduleId;
        private decimal _tenPcsWeight;
        private string _subModuleId;
        private bool _clearBatchWeight;
        private static string _screenName = "QC Scanning - DefectiveGlovePlatform";
        private static string _className = "DefectiveGlovePlatform";
        private string _serialNo;

        private string _logScreenName;
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="DefectiveGlovePlatform" /> class.
        ///// </summary>
        public DefectiveGlovePlatform(string screenName)
        {
            InitializeComponent();
            this.Text = screenName;
            grpBoxScreen.Text = screenName;
            try
            {
                if (screenName == Constants.DEFECTIVE_GLOVE_SMALLSCALE)
                {
                    lblBatch.Text = "Batch (g)";
                    lblPcsCount.Visible = true;
                    txtPcsCount.Visible = true;
                    _isPlatform = false;
                    _logScreenName = Constants.QC_DEFECTIVE_GLOVE_SMALLSCALE;
                    _subModuleId = CommonBLL.GetSubModuleId(Constants.QC_DEFECTIVE_GLOVE_SMALLSCALE);
                    txtPcsCount.MaxLength = Convert.ToString(FloorSystemConfiguration.GetInstance().PcsCountSmallScale).Length;
                }
                else
                {
                    _logScreenName = Constants.QC_DEFECTIVE_GLOVE_PLATFORM;
                    _subModuleId = CommonBLL.GetSubModuleId(Constants.QC_DEFECTIVE_GLOVE_PLATFORM);
                }
                _moduleId = CommonBLL.GetModuleId(Constants.QC_SCANNING_SYSTEM);
                BindReasonsAndDefectType();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "DefectiveGlovePlatform", string.Empty);
                return;
            }
        }

        /// <summary>
        /// Form load event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefectiveGlovePlatform_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            txtSerialNo.SerialNo();
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                txtBatchWeight.ReadOnly = true;
                txtBatchWeight.TabStop = false;
            }
            else
            {
                txtBatchWeight.ReadOnly = false;
                txtBatchWeight.TabStop = true;
            }
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Get the Rework Reason & Defect Type data & Bind the Rework Reason & Defect Type ComboBox  
        /// </summary>
        private void BindReasonsAndDefectType()
        {
            //cmbReason.BindComboBox(CommonBLL.GetReasons(Constants.QC_DEFECTIVE_GLOVE_PLATFORM, _moduleId), true);
            cmbDefectType.BindComboBox(QCScanningBLL.GetDefectiveGloveType(), true);
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
            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH && CommonBLL.ValidateBatch(Convert.ToDecimal(txtSerialNo.Text)))
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
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
        /// Get Batch Weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// /// //***TBC - Made editable for only Testing Purposes - To Comment
        private void cmbReason_Leave(object sender, EventArgs e)
        {
            if (!_clearBatchWeight)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {

                    if (_isPlatform)
                    {
                        GetBatchWeight();
                        btnSave.Focus();
                    }
                    else
                    {
                        GetTenPcsWeight();
                        txtPcsCount.Focus();
                    }
                }
                else
                {
                    txtBatchWeight.Focus();
                }

                ////try
                ////{

                //}
                //catch (FloorSystemException ex)
                //{
                //    ExceptionLogging(ex, _screenName, _className, "cmbReason_Leave", string.Empty);
                //    return;
                //}
            }
        }

        /// <summary>
        /// Validate & Save the Batch Card Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbDefectType, "Defect Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbReason, "Reason", ValidationType.Required));
            if (!_isPlatform)
            {
                validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, "Batch (g)", ValidationType.Required));
               // validationMesssageLst.Add(new ValidationMessage(txtPcsCount, "Pcs Count", ValidationType.Required));
            }
            else
                validationMesssageLst.Add(new ValidationMessage(txtBatchWeight, "Batch(Kg)", ValidationType.Required));
            try
            {
                if (ValidateForm())
                {
                    if (_serialNo != txtSerialNo.Text)
                    {
                        if (CommonBLL.ValidateBatch(Convert.ToDecimal(txtSerialNo.Text)))
                            ShowBatchDetails();
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                        }
                    }
                    if (!string.IsNullOrEmpty(txtSerialNo.Text))
                    {
                        if (_isPlatform)
                            SaveBatchCard();
                        else
                        {
                            if (!string.IsNullOrEmpty(txtPcsCount.Text))
                            {
                                if (Convert.ToInt32(txtPcsCount.Text) <= Convert.ToInt32(FloorSystemConfiguration.GetInstance().PcsCountSmallScale))
                                {
                                    SaveBatchCard();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(string.Format(Messages.PCSCOUNT_RANGE, FloorSystemConfiguration.GetInstance().PcsCountSmallScale), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtPcsCount.Text = string.Empty;
                                    txtPcsCount.Focus();
                                }
                            }
                            else
                            {
                                SaveBatchCard();
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", string.Empty);
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

        /// <summary>
        /// User should not be able to enter characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPcsCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Clear the values of all the controls. 
        /// </summary>
        private void ClearForm()
        {
            txtSerialNo.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtGloveType.Text = String.Empty;
            txtQCGroup.Text = String.Empty;
            cmbDefectType.SelectedIndex = Constants.MINUSONE;
            cmbReason.SelectedIndex = Constants.MINUSONE;
            txtBatchWeight.Text = String.Empty;
            txtPcsCount.Text = String.Empty;
            _clearBatchWeight = true;
            // txtSerialNo.Focus();
            btnSave.Enabled = false;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            BindReasonsAndDefectType();
            txtSerialNo.Focus();
        }

        /// <summary>
        /// Show the Batch details based on Serial Number
        /// </summary>
        private void ShowBatchDetails()
        {
            BatchDTO objBatch = QCScanningBLL.GetBatchDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()), Constants.ONE);
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
                    txtGloveType.Text = objBatch.GloveType;
                    txtQCGroup.Text = objBatch.QCGroupName;
                    _size = objBatch.Size;
                    _tenPcsWeight = objBatch.TenPcsWeight;
                    _qcGroupId = objBatch.QCGroupId;
                    _serialNo = txtSerialNo.Text;
                    btnSave.Enabled = true;
                    _clearBatchWeight = false;
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_QC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearForm();
            }
        }


        private void txtBatchWeight_Leave(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text != String.Empty)
            {
                //if (_isPlatform)
                //    ValidateBatchWeight();
                //else
                //{
                //    ValidateTenPcsWeight();
                //}

                //Modified by Lakshman 17 / 1 / 2018 for Negative input validation    
                decimal decim;
                if (Decimal.TryParse(txtBatchWeight.Text, out decim) && decim > 0)
                {
                    ValidateBatchWeight();
                    txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                }
                else
                {
                    if (!_isPlatform)
                    {
                        GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH_GRAMS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }                        
                    txtBatchWeight.Text = String.Empty;
                    txtBatchWeight.Focus();
                }
                
            }
            //if (!_isPlatform) 
            //    txtPcsCount.Focus();

        }

        private void txtBatchWeight_Enter(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text == String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtBatchWeight.ReadOnly = true;
                    if (_isPlatform)
                        txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                    else
                        txtBatchWeight.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                    txtBatchWeight_Leave(sender, e);
                }
                else
                {
                    txtBatchWeight.ReadOnly = false;
                    txtBatchWeight.Focus();
                }
            }
        }


        /// <summary>
        /// To get Batch Weight
        /// </summary>
        /// <returns></returns>
        /// //***TBC - Made editable for only Testing Purposes - To Comment
        private void GetBatchWeight()
        {
            try
            {
                txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                ValidateBatchWeight();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBatchWeight", null);
                return;
            }
        }


        /// <summary>
        /// To validate Batch Weight
        /// </summary>
        /// <returns></returns>
        private void ValidateBatchWeight()
        {
            try
            {
                WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
                FloorSystemConfiguration Floordata = FloorSystemConfiguration.GetInstance();
                if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(Floordata.MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(Floordata.MaxBatchWeight)))
                {
                    GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
                //***TBC - Made editable for only Testing Purposes - To Comment
                //Start
                txtPcsCount.Focus();
                //End
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateBatchWeight", string.Empty);
                return;
            }
        }


        /// <summary>
        /// To get 10 PCs weight by calling the method in the Integration class through BLL
        /// </summary>
        /// <returns></returns>
        private void GetTenPcsWeight()
        {
            try
            {
                txtBatchWeight.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                ValidateTenPcsWeight();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBatchWeight", null);
                return;
            }
        }

        /// <summary>
        /// Validate 10 Pcs Weight based on Glove Type and Size by getting base values from AX
        /// </summary>
        private void ValidateTenPcsWeight()
        {
            try
            {
                TenPcsDTO weight = new TenPcsDTO();
                weight = CommonBLL.GetMinMaxTenPcsWeight(txtGloveType.Text, _size);
                WorkStationDataConfiguration data = WorkStationDataConfiguration.GetInstance();
                if (Convert.ToBoolean(data.LWeight) && weight.MinWeight != null)
                {
                    if (Convert.ToBoolean(data.LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(weight.MinWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(weight.MaxWeight)))
                    {
                        GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                txtBatchWeight.Text = txtBatchWeight.Text.ToString();
                //***TBC - Made editable for only Testing Purposes - To Comment
                //Start
                txtPcsCount.Focus();
                //End
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ValidateTenPcsWeight", string.Empty);
                return;
            }
        }

        /// <summary>
        /// Saves the Scan Batch Card Details
        /// </summary>
        private void SaveBatchCard()
        {
            if (WorkStationDataConfiguration.GetInstance().smallScalingSystem != Constants.ZERO.ToString() && !_isPlatform && Convert.ToDouble(txtBatchWeight.Text) != Constants.ZERO)
            {
                GetTenPcsWeight();
            }
            string pcsCount = txtPcsCount.Text;
            if (String.IsNullOrEmpty(pcsCount))
                pcsCount = Constants.ZERO.ToString();
            if (string.IsNullOrEmpty(txtBatchWeight.Text))
            {
                GetBatchWeight();
                /// //***TBC - Made editable for only Testing Purposes - To Comment below line
                //ValidateBatchWeight();
            }
            if (string.IsNullOrEmpty(txtBatchWeight.Text) || Convert.ToDecimal(pcsCount) == Constants.ZERO || _isPlatform || Convert.ToDecimal(txtBatchWeight.Text) == Constants.ZERO)
            {
                if ((Convert.ToDecimal(pcsCount) > Constants.ZERO || Convert.ToDecimal(txtBatchWeight.Text) > Constants.ZERO) || _isPlatform)
                {
                    if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                    {
                        if (_isPlatform)
                            GetBatchWeight();
                        else
                            GetTenPcsWeight();
                    }


                    QCYieldandPackingDTO objQCYPDTO = new QCYieldandPackingDTO();
                    objQCYPDTO.SerialNumber = txtSerialNo.Text.Trim();
                    objQCYPDTO.GloveType = txtGloveType.Text;
                    objQCYPDTO.QCGroupId = _qcGroupId;
                    objQCYPDTO.DefectTypeId = Convert.ToString(cmbDefectType.SelectedValue);
                    objQCYPDTO.ReworkReasonId = cmbReason.SelectedValue.ToString();
                    if (_isPlatform)
                        objQCYPDTO.BatchWeight = Convert.ToString(Convert.ToDecimal(txtBatchWeight.Text));
                    else
                    {
                        if (Convert.ToDecimal(txtBatchWeight.Text) == Constants.ZERO && Convert.ToDecimal(pcsCount) > Constants.ZERO)
                        {
                            objQCYPDTO.BatchWeightGrm = Math.Round(Convert.ToDecimal(pcsCount) * _tenPcsWeight / Constants.TEN, Constants.ZERO).ToString();
                        }
                        else
                        {
                            objQCYPDTO.BatchWeightGrm = Convert.ToString(Convert.ToDecimal(txtBatchWeight.Text));
                        }
                    }
                    objQCYPDTO.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    if (!_isPlatform && Convert.ToDecimal(pcsCount) > Constants.ZERO)
                    {
                        objQCYPDTO.PiecesCount = Convert.ToDecimal(txtPcsCount.Text);
                    }
                    else
                    {
                        if (_isPlatform)
                        {
                            objQCYPDTO.PiecesCount = Math.Round(Convert.ToDecimal(txtBatchWeight.Text) * Constants.TEN * Constants.THOUSAND / _tenPcsWeight, Constants.ZERO);
                        }
                        else
                        {
                            objQCYPDTO.PiecesCount = Math.Round(Convert.ToDecimal(txtBatchWeight.Text) * Constants.TEN / _tenPcsWeight, Constants.ZERO);
                        }
                    }
                    objQCYPDTO.ModuleName = _moduleId;
                    objQCYPDTO.SubModuleName = _subModuleId;
                    objQCYPDTO.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                    QCScanningBLL.SaveDefectiveGlovePlatform(objQCYPDTO);
                    //event log myadamas 20190227
                    EventLogDTO EventLog = new EventLogDTO();

                    EventLog.CreatedBy = String.Empty;
                    Constants.EventLog audAction = Constants.EventLog.Save;
                    EventLog.EventType = Convert.ToInt32(audAction);
                    EventLog.EventLogType = Constants.eventlogtype;

                    //var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                    CommonBLL.InsertEventLog(EventLog, _logScreenName, _subModuleId);

                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
                else
                {
                    GlobalMessageBox.Show(Constants.BATCH_PCSCOUNT_INVALID, Constants.AlertType.Error, Messages.ERROR_TITLE, GlobalMessageBoxButtons.OK);
                    txtPcsCount.Text = string.Empty;
                    txtPcsCount.Focus();
                }
            }
            else
            {
                GlobalMessageBox.Show(Constants.BATCH_PCSCOUNT_ENTERED, Constants.AlertType.Error, Messages.ERROR_TITLE, GlobalMessageBoxButtons.OK);
                txtPcsCount.Text = string.Empty;
                txtPcsCount.Focus();
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
            else if (uiControl == "getBatchWeight")
                GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        #endregion

        private void cmbDefectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbReason.BindComboBox(QCScanningBLL.GetDefectiveReasons(cmbDefectType.Text), true);
            cmbReason.DropDownHeight = (cmbReason.ItemHeight * 10) + 2;
        }

        private void txtPcsCount_Validated(object sender, EventArgs e)
        {
            btnSave.Focus();
        }
    }
}
