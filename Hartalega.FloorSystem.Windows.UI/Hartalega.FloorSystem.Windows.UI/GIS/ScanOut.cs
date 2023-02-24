using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Drawing;

namespace Hartalega.FloorSystem.Windows.UI.GIS
{
    /// <summary>
    /// Module: Glove Inventory
    /// Screen Name: Scan Out
    /// File Type: Code file
    /// </summary> 
    public partial class ScanOut : FormBase
    {
        #region Class Variables
        private bool _enableSpaceBarToSave;
        private bool _isSaved;
        private static string _screenName = "Glove Inventory - ScanOut";
        private static string _className = "ScanOut";
        private string _serialNo;
        #endregion

        #region Page Load
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanOut" /> class.
        /// </summary>
        public ScanOut()
        {
            InitializeComponent();
            try
            {
                txtWarehouseLocation.Text = WorkStationDTO.GetInstance().LocationAreaCode;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanOut", string.Empty);
                return;
            }
            BindNextProcess();
        }

        /// <summary>
        /// Binds the Next Process ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanOut_Load(object sender, EventArgs e)
        {
            txtSerialNo.SerialNo();
            btnSave.Enabled = false;
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Binds the Next Process ComboBox
        /// </summary>
        private void BindNextProcess()
        {
            try
            {
                cmbNextProcess.BindComboBox(CommonBLL.GetEnumText(Constants.NEXTPROCESS), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindNextProcess", Constants.NEXTPROCESS);
                return;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        ///  Checks whether Serial Number was Scanned In and gets the Batch Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNo_Leave(object sender, EventArgs e)
        {
            GetBatchDetails();
        }

        /// <summary>
        /// Used to apply the Press Space Bar to save functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNextProcess_Leave(object sender, EventArgs e)
        {
            _enableSpaceBarToSave = true;
        }

        /// <summary>
        /// Used to apply the Press Space Bar to save functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNextProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                _enableSpaceBarToSave = false;
            }
        }

        /// <summary>
        /// Saves the Scan Out Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_serialNo != txtSerialNo.Text)
                GetBatchDetails();
            if (!string.IsNullOrEmpty(txtSerialNo.Text))
                SaveData();
        }

        /// <summary>
        /// Press Space bar to save the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' & (btnSave.Enabled) & (_enableSpaceBarToSave))
            {
                SaveData();
            }
        }

        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                ClearForm();
            }
        }

        #endregion

        #region User Methods
        private void GetBatchDetails()
        {
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

                        if (CommonBLL.ValidateBatch(Convert.ToDecimal(txtSerialNo.Text)))
                        {
                            string binNumber = GISBLL.ValidateDuplicateSerialNo(Convert.ToDecimal(txtSerialNo.Text));
                            if (!string.IsNullOrEmpty(binNumber))
                            {
                                if (GISBLL.ValidateBatchScanIn(Convert.ToDecimal(txtSerialNo.Text), WorkStationDTO.GetInstance().Location, WorkStationDTO.GetInstance().Area))
                                {
                                    BatchDTO objBatch = CommonBLL.GetBatchScanInDetails(Convert.ToDecimal(txtSerialNo.Text));
                                    if (objBatch != null)
                                    {
                                        #region Commented out by Azman
                                        //string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                                        //if (!string.IsNullOrEmpty(qaiStatus) && qaiStatus != Messages.QAI_EXPIRED)
                                        //{
                                        //   GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        //    ClearForm();
                                        //}
                                        //else
                                        //{
                                        //if (qaiStatus == Messages.QAI_EXPIRED)
                                        //    GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                        #endregion

                                        GISBatchInfo objGIS = CommonBLL.GISGetBatchInfo(Convert.ToDecimal(txtSerialNo.Text));

                                        txtBatchNo.Text = objBatch.BatchNumber;
                                        txtGloveType.Text = objBatch.GloveType;
                                        txtLine.Text = objBatch.Line;
                                        txtShift.Text = objBatch.ShiftName;
                                        txtSize.Text = objBatch.Size;

                                        //2018-09-27 GIS BugFix - Azman
                                        //txtTenPcs.Text = Math.Round(CommonBLL.GetLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)), Constants.TWO).ToString("#,##0.00");
                                        //txtBatchWeight.Text = Math.Round(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)), Constants.TWO).ToString("#,##0.00");
                                        //txtTotalPcs.Text = string.Format(Constants.NUMBER_FORMAT, Convert.ToInt32(Math.Floor(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)))));

                                        txtTenPcs.Text = string.Format(Constants.NUMBER_FORMAT, Math.Round(objGIS.TenPcsWeight, Constants.TWO).ToString("#,##0.00"));
                                        txtBatchWeight.Text = Math.Round(objGIS.BatchWeight, Constants.TWO).ToString("#,##0.00");
                                        txtTotalPcs.Text = string.Format(Constants.NUMBER_FORMAT, Convert.ToInt32(objGIS.TotalPcs));

                                        txtQCType.Text = CommonBLL.GetQCType(objBatch.QCType);
                                        _serialNo = txtSerialNo.Text;
                                        btnSave.Enabled = true;

                                        //}
                                    }
                                }
                                else
                                {                                    
                                    GlobalMessageBox.Show(Messages.BATCH_NOT_SCANIN, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.BATCH_NOT_EXISTS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
        /// Saves the Batch Details 
        /// </summary>
        private void SaveData()
        {
            GISDTO objStore = new GISDTO();
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbNextProcess, "Next Process", ValidationType.Required));
            if (ValidateForm())
            {
                GISDTO objGIS = new GISDTO();
                objGIS.SerialNumber = Convert.ToDecimal(txtSerialNo.Text.Trim());
                objGIS.LocationId = WorkStationDTO.GetInstance().LocationAreaId;
                objGIS.BinNumber = String.Empty;
                objGIS.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                objGIS.Id = Constants.ONE;
                objGIS.NextProcess = cmbNextProcess.GetItemText(cmbNextProcess.SelectedItem);
                objGIS.WorkstationId = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);
                try
                {                   
                    objStore = GISBLL.StoreScanInDetails(objGIS.SerialNumber);
                    GISBLL.SaveBatchScanInDetails(objGIS);
                    _isSaved = true;

                    #region Commented by Azman

                    //bool isPostingSuccess = AXPostingBLL.PostAXGloveInventoryScanOut(Convert.ToDecimal(objGIS.SerialNumber));
                    //if (!isPostingSuccess)
                    //{
                    //    GISBLL.UpdateGISScanOutData(objStore);
                    //    GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);                  
                    //}
                    //else
                    //{
                    //    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    //}

                    #endregion  

                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    _isSaved = false;
                }
                catch (FloorSystemException ex)
                {
                    if (_isSaved)
                    {
                        GISBLL.UpdateGISScanOutData(objStore);
                        _isSaved = false;
                    }
                    ExceptionLogging(ex, _screenName, _className, "SaveData", txtSerialNo.Text.Trim(), WorkStationDTO.GetInstance().LocationId, CommonBLL.GetCurrentDateAndTime(), cmbNextProcess.GetItemText(cmbNextProcess.SelectedItem));
                    return;
                }
            }
        }

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtSerialNo.Text = String.Empty;
            txtBatchNo.Text = String.Empty;
            txtGloveType.Text = String.Empty;
            txtLine.Text = String.Empty;
            txtShift.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtTenPcs.Text = String.Empty;
            txtBatchWeight.Text = String.Empty;
            txtTotalPcs.Text = String.Empty;
            txtQCType.Text = String.Empty;
            cmbNextProcess.SelectedIndex = Constants.MINUSONE;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
            CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
            txtWarehouseLocation.Text = WorkStationDTO.GetInstance().LocationAreaCode;
            BindNextProcess();
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
