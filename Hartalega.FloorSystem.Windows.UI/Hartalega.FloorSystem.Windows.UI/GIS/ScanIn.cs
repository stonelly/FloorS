using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Collections.Generic;
using System.Drawing;

namespace Hartalega.FloorSystem.Windows.UI.GIS
{
    /// <summary>
    /// Module: Glove Inventory
    /// Screen Name: Scan In
    /// File Type: Code file
    /// </summary> 
    public partial class ScanIn : FormBase
    {

        #region Class Variables
        private string _prevBinNumber;
        private bool _isSaved;
        private bool _cancelClick;
        private static string _screenName = "Glove Inventory - ScanIn";
        private static string _className = "ScanIn";
        private string _serialNo;
        private string _binNumber;
        private bool _pressSave;

        private static string _logScreenName = "Scan In";
        #endregion

        #region Page Load
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanIn" /> class.
        /// </summary>
        public ScanIn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the Location Area Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanIn_Load(object sender, EventArgs e)
        {
            txtSerialNo.SerialNo();
            btnSave.Enabled = false;
            try
            {
                txtWarehouseLocation.Text = WorkStationDTO.GetInstance().LocationAreaCode;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ScanIn", string.Empty);
                return;
            }
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
            if (!_pressSave)
                GetBatchDetails();
        }

        /// <summary>
        /// Checks whether Bin Number is Valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBin_Leave(object sender, EventArgs e)
        {
            if (!_cancelClick)
            {
                GetBinData();
            }
        }

        /// <summary>
        /// Saves the Scan In Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_serialNo != txtSerialNo.Text)
                GetBatchDetails();
            if (string.IsNullOrEmpty(txtBin.Text) && !string.IsNullOrEmpty(txtSerialNo.Text))
            {
                validationMesssageLst = new List<ValidationMessage>();
                validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(txtBin, "Bin", ValidationType.Required));
                _pressSave = true;
                ValidateForm();
                _pressSave = false;
            }
            if (_binNumber != txtBin.Text)
                GetBinData();
            if (!string.IsNullOrEmpty(txtSerialNo.Text) && !string.IsNullOrEmpty(txtBin.Text))
                SaveData();
        }

        /// <summary>
        /// Press Space bar to save the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' & (btnSave.Enabled))
            {
                SaveData();
            }
        }

        /// <summary>
        ///  Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                _cancelClick = true;
                ClearForm();
            }
        }

        #endregion

        #region User Methods
        /// <summary>
        /// Saves the Batch Details 
        /// </summary>
        private void SaveData()
        {
            GISDTO objStore = new GISDTO();
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial No", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBin, "Bin", ValidationType.Required));
            if (ValidateForm())
            {
                GISDTO objGIS = new GISDTO();
                objGIS.SerialNumber = Convert.ToDecimal(txtSerialNo.Text.Trim());
                objGIS.LocationId = WorkStationDTO.GetInstance().LocationAreaId;
                objGIS.BinNumber = txtBin.Text.Trim();
                objGIS.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                objGIS.Id = Constants.ZERO;
                objGIS.NextProcess = String.Empty;
                objGIS.WorkstationId = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);
                try
                {

                    objStore = GISBLL.StoreScanInDetails(objGIS.SerialNumber);
                    GISBLL.SaveBatchScanInDetails(objGIS);
                    _isSaved = true;
                    //bool isPostingSuccess = AXPostingBLL.PostAXGloveInventoryScanIn(Convert.ToDecimal(objGIS.SerialNumber));
                    //if (!isPostingSuccess)
                    //{
                    //    if (objStore == null)
                    //    {
                    //        GISBLL.DeleteGISScanInData(Convert.ToDecimal(objGIS.SerialNumber));
                    //    }
                    //    else
                    //    {
                    //        objStore.Id = Constants.ZERO;
                    //        GISBLL.SaveBatchScanInDetails(objStore);
                    //    }
                    //    GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    //}
                    //else
                    //{
                    //    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    //    //MessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY);
                    //}
                    //adamas event log 20190228
                    EventLogDTO EventLog = new EventLogDTO();
                    EventLog.CreatedBy = String.Empty;
                    Constants.EventLog evtAction = Constants.EventLog.Save;
                    EventLog.EventType = Convert.ToInt32(evtAction);
                    EventLog.EventLogType = Constants.eventlogtype;

                    var screenid = CommonBLL.GetScreenIdByScreenName(_logScreenName);
                    CommonBLL.InsertEventLog(EventLog, _logScreenName, screenid.ToString());

                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    _isSaved = false;
                }
                catch (FloorSystemException ex)
                {
                    if (_isSaved)
                    {
                        if (objStore == null)
                        {
                            GISBLL.DeleteGISScanInData(Convert.ToDecimal(objGIS.SerialNumber));
                        }
                        else
                        {
                            objStore.Id = Constants.ZERO;
                            GISBLL.SaveBatchScanInDetails(objStore);
                        }
                        _isSaved = false;
                    }
                    ExceptionLogging(ex, _screenName, _className, "SaveData", txtSerialNo.Text.Trim(), WorkStationDTO.GetInstance().LocationId, txtBin.Text.Trim(), usrDateControl.DateValue);
                    return;
                }
            }
        }

        /// <summary>
        /// Clear the values of all the controls. 
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
            txtBin.Text = String.Empty;
            txtSerialNo.Focus();
            btnSave.Enabled = false;
            CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
            txtWarehouseLocation.Text = WorkStationDTO.GetInstance().LocationAreaCode;
        }

        /// <summary>
        /// Get BatchDetails
        /// </summary>
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

                        BatchDTO objBatch = CommonBLL.GetBatchScanInDetails(Convert.ToDecimal(txtSerialNo.Text.Trim()));

                        if ((objBatch != null) && (objBatch.QCType.Length > 0))
                        {
                            GISBatchInfo objGIS = CommonBLL.GISGetBatchInfo(Convert.ToDecimal(txtSerialNo.Text.Trim()));

                            if (objGIS.TotalPcs > Constants.ZERO)
                            {
                                
                                #region Removed by Azman 
                                //string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                                //if (!string.IsNullOrEmpty(qaiStatus))
                                //{
                                //GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                //ClearForm();
                                //}
                                //else
                                //{
                                //if (CommonBLL.ValidateAXPostingForArea(Convert.ToDecimal(txtSerialNo.Text), WorkStationDTO.GetInstance().Area))
                                //{
                                #endregion

                                txtBatchNo.Text = objBatch.BatchNumber;
                                txtGloveType.Text = objBatch.GloveType;
                                txtLine.Text = objBatch.Line;
                                txtShift.Text = objBatch.ShiftName;
                                txtSize.Text = objBatch.Size;

                                //2018-09-27 GIS BugFix - Azman
                                //txtTenPcs.Text = string.Format(Constants.NUMBER_FORMAT, Math.Round(CommonBLL.GetLatestTenPcsWeightV2(Convert.ToDecimal(txtSerialNo.Text)), Constants.TWO).ToString("#,##0.00"));
                                //txtBatchWeight.Text = Math.Round(CommonBLL.GetLatestBatchWeightV2(Convert.ToDecimal(txtSerialNo.Text)), Constants.TWO).ToString("#,##0.00");
                                //txtTotalPcs.Text = string.Format(Constants.NUMBER_FORMAT, Convert.ToInt32(Math.Floor(PostTreatmentBLL.GetPTLatestBatchWeight(Convert.ToDecimal(txtSerialNo.Text)) * Constants.THOUSAND * Constants.TEN / PostTreatmentBLL.GetPTLatestTenPcsWeight(Convert.ToDecimal(txtSerialNo.Text)))));

                                txtTenPcs.Text = string.Format(Constants.NUMBER_FORMAT, Math.Round(objGIS.TenPcsWeight, Constants.TWO).ToString("#,##0.00"));
                                txtBatchWeight.Text = Math.Round(objGIS.BatchWeight, Constants.TWO).ToString("#,##0.00");
                                txtTotalPcs.Text = string.Format(Constants.NUMBER_FORMAT, Convert.ToInt32(objGIS.TotalPcs));

                                txtQCType.Text = CommonBLL.GetQCType(objBatch.QCType);
                                _prevBinNumber = GISBLL.ValidateDuplicateSerialNo(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                                _cancelClick = false;
                                _serialNo = txtSerialNo.Text;
                                if (!string.IsNullOrEmpty(_prevBinNumber))
                                {
                                    #region Removed by Azman 
                                    //string confirm = GlobalMessageBox.Show(string.Format(Messages.BIN_DUPLICATE_CONFIRM, _prevBinNumber), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                                    //if (confirm == Constants.NO)
                                    //{
                                    //    ClearForm();
                                    //    _serialNo = string.Empty;
                                    //}
                                    //else
                                    //{
                                    //    txtBin.Text = string.Empty;
                                    //}
                                    #endregion
                                    GlobalMessageBox.Show(string.Format(Messages.BIN_DUPLICATE_CONFIRM, _prevBinNumber), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                    _serialNo = string.Empty;
                                }
                                #region Removed by Azman 
                                //ClearForm();
                                /*
                            }
                            else
                            {
                                if (WorkStationDTO.GetInstance().Area == Constants.QC)
                                    GlobalMessageBox.Show(string.Format(Messages.INVALID_SERIAL_NUMBER_LOCATION, WorkStationDTO.GetInstance().Area), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                else if (WorkStationDTO.GetInstance().Area == Constants.PT)
                                {

                                    GlobalMessageBox.Show(string.Format(Messages.INVALID_SERIAL_NUMBER_PT_LOCATION, WorkStationDTO.GetInstance().Area), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                }
                                else
                                {
                                    if (PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.PWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.QWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.OWT || PostTreatmentBLL.GetBatchType(Convert.ToDecimal(txtSerialNo.Text)) == Constants.PSW)
                                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_PN_WATERTIGHT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else
                                        GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_PN_LOCATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                }



                            }
                            */
                                //}
                                #endregion

                            }
                            else
                            {
                                GlobalMessageBox.Show("Batch card have fully consumed. Please contact MIS.", Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
        /// Validates the Bin
        /// </summary>
        private void GetBinData()
        {
            if (!string.IsNullOrEmpty(txtBin.Text.Trim()))
            {
                try
                {
                    if (!GISBLL.ValidateBin(txtBin.Text.Trim(), WorkStationDTO.GetInstance().LocationAreaId))
                    {
                        GlobalMessageBox.Show(Messages.BIN_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtBin.Text = string.Empty;
                        txtBin.Focus();
                        btnSave.Enabled = false;
                        _binNumber = string.Empty;
                    }
                    else if (_prevBinNumber == txtBin.Text.Trim())
                    {
                        GlobalMessageBox.Show(Messages.BIN_DUPLICATE, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtBin.Text = string.Empty;
                        txtBin.Focus();
                        btnSave.Enabled = false;
                        _binNumber = txtBin.Text;
                    }
                    else
                    {
                        _binNumber = txtBin.Text;
                        if (!btnSave.Enabled)
                        {
                            btnSave.Enabled = true;
                            btnSave.Focus();
                        }
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "GetBinData", txtBin.Text.Trim());
                    return;
                }
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
            else if (floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);

            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        #endregion





    }
}
