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

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ScanTMPPackInventory : FormBase
    {
        # region PRIVATE VARIABLES
        private string _screenname = "Scan TMP Pack Inventory";
        private string _uiClassName = "Scan TMPPack Inventory";
        private string _moduleId;
        private string _UpdatedPcs;        
        # endregion

        #region CONSTRUCTOR
        public ScanTMPPackInventory()
        {
            InitializeComponent();
        }
        # endregion
        /// <summary>
        /// Validate QA Test Result
        /// </summary>
        /// <param name="serialNo">Serial Number scanned</param>
        /// <param name="testNameDifferent QA Test result"></param>
        /// <returns></returns>
        public static Boolean ValidateQATestResult(decimal serialNo, string testName)
        {
            string testResult = string.Empty;
            string area = string.Empty;
            testResult = FinalPackingBLL.ValidateQATestResult(serialNo, testName);
            if (testName == Constants.POLYMER_TEST)
            {
                area = Constants.PT;
            }
            else
            {
                area = Constants.QA;
            }
            switch (testResult)
            {
                case Constants.FP_NORECORD:
                    return true;
                case Constants.FP_RESULTPASS:
                    return true;
                case Constants.FP_NORESULTCAPTURED:
                    GlobalMessageBox.Show(string.Format(Messages.QA_TESTRESULT, testName, area), Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    return false;
                case Constants.FP_RESULTFAIL:
                    GlobalMessageBox.Show(string.Format(Messages.QA_TESTRESULT, testName, area), Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    return false;
                default:
                    return true;
            }
        }
        /// <summary>
        /// SERIALNUMBER VALIDATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNumber_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSerialNumber.Text))
                {
                    if (FinalPackingBLL.ValidateFPTempPack(txtSerialNumber.Text) != Constants.ONE)
                    {                  
                    
                         string qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNumber.Text)); //validate QAI status
                         if (string.IsNullOrEmpty(qaiResults))
                         {
                             if (FinalPackingBLL.ValidateSerialNoByQIStatus(Convert.ToDecimal(txtSerialNumber.Text)) == Constants.PASS)
                             {
                                // Comment: disable due to FP station need to proceed before QA testing result out/ready
                                //if (ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.POLYMER_TEST)
                                //    && ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.POWDER_TEST)
                                //    && ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.PROTEIN_TEST)
                                //    && ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.HOTBOX_TEST))
                                // {

                                     BatchDTO objBatchDTO = FinalPackingBLL.BatchCardDetailsbySerialNumber(Convert.ToDecimal(txtSerialNumber.Text));
                                     if (!string.IsNullOrEmpty(objBatchDTO.BatchNumber))
                                     {
                                         txtBatchNumber.Text = objBatchDTO.BatchNumber;
                                         txtGloveType.Text = objBatchDTO.GloveType;
                                         txtSize.Text = objBatchDTO.Size;
                                         txtBatchWeight.Text = string.Format(Constants.NUMBER_FORMAT, objBatchDTO.BatchWeight);
                                         txtTenPcs.Text = Convert.ToString(objBatchDTO.TenPcsWeight);
                                         txtQcType.Text = Convert.ToString(objBatchDTO.QCType);
                                         txtQCDescription.Text = objBatchDTO.QCTypeDescription;
                                         txtTenPcs.Text = Convert.ToString(objBatchDTO.TenPcsWeight);
                                         txtPNPcs.Text = string.Format(Constants.NUMBER_FORMAT, objBatchDTO.TotalPcs);
                                         txtTMPPackPcs.Text = string.Format(Constants.NUMBER_FORMAT, objBatchDTO.UpdatedPcs);
                                         _UpdatedPcs = Convert.ToString(objBatchDTO.UpdatedPcs);
                                         btnSave.Enabled = true;
                                     }
                                     else
                                     {
                                         GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                         txtSerialNumber.Text = string.Empty;
                                         txtSerialNumber.Focus();
                                         ClearForm();
                                     }
                                 //}
                                 //else
                                 //{
                                 //    txtSerialNumber.Text = string.Empty;
                                 //    txtSerialNumber.Focus();                                     
                                 //}
                             }
                             else
                             {
                                 GlobalMessageBox.Show(Messages.QI_TESTRESULT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                 txtSerialNumber.Text = string.Empty;
                                 txtSerialNumber.Focus();
                             }
                         }
                         else
                         {                             
                             GlobalMessageBox.Show(qaiResults, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);                                                                       
                             txtSerialNumber.Clear();
                             txtSerialNumber.Focus();
                             ClearForm();
                         }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.BCH_TMPI, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);                                                                       
                        txtSerialNumber.Text = string.Empty;
                        txtSerialNumber.Focus();
                        ClearForm();
                    }
                }
                else
                {
                    ClearForm();
                    
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtSerialNumber_Leave");
            }
        }
        /// <summary>
        /// FORM LOAD TO POPULATE DEFAULT VALUES
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanTMPPackInventory_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
            {
                txtStationNo.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
            }
            else
                GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED,Constants.AlertType.Information,Messages.INFORMATION,GlobalMessageBoxButtons.OK);
            txtPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;            
            _moduleId = CommonBLL.GetModuleId(Constants.FINALPACKING);
            txtSerialNumber.SerialNo();
            LoadTMPPackReasons();
            this.WindowState = FormWindowState.Maximized;
            
        }
        /// <summary>
        /// POPULATE REASONS DROPDOWN
        /// </summary>
        private void LoadTMPPackReasons()
        {
            try
            {
                List<DropdownDTO> lstReason = CommonBLL.GetReasons(Constants.FP_TMP_REASONTYPE, _moduleId); 
                cmbTMPPackReason.DataSource = lstReason;
                cmbTMPPackReason.DisplayMember = "DisplayField";
                cmbTMPPackReason.ValueMember = "IDField";
                cmbTMPPackReason.SelectedIndex = -1;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "LoadTMPPackReasons");
            }
        }
        /// <summary>
        /// SAVE EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRequiredFields())
                {
                    if (Convert.ToInt32(txtTMPPackPcs.Text.Replace(",", "")) > Constants.ZERO)
                    {
                        FPTempPackDTO objFPTEmpPackDTO = new FPTempPackDTO();
                        objFPTEmpPackDTO.SerialNumber = Convert.ToDecimal(txtSerialNumber.Text);
                        objFPTEmpPackDTO.TempPackReason = cmbTMPPackReason.Text;
                        objFPTEmpPackDTO.TempPackPcs = Convert.ToInt32(txtTMPPackPcs.Text.Replace(",", ""));
                        objFPTEmpPackDTO.WorkStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                        objFPTEmpPackDTO.PrinterName = WorkStationDataConfiguration.GetInstance().innerPrinter;
                        objFPTEmpPackDTO.LocationID = WorkStationDTO.GetInstance().LocationId;
                        int rowsreturned = FinalPackingBLL.TMPPackSave(objFPTEmpPackDTO);
                        if (rowsreturned > 0)
                        {
                            //Decoupled for Industrial PC Scan
                            //bool isPostingSuccess = AXPostingBLL.PostAXDataFinalPacking(objFPTEmpPackDTO.SerialNumber.ToString(), Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanTMPPackInventory)));
                            bool isPostingSuccess = true;
                            if (!isPostingSuccess)
                            {
                                FinalPackingBLL.DeleteTempPackData(objFPTEmpPackDTO.SerialNumber.ToString());
                                GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                            txtSerialNumber.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.TMPBATCHPCS, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtSerialNumber.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnSave_Click", null);
            }

        }

        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, Messages.REQSERIALNUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbTMPPackReason, Messages.REQTMPPACKREASON, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtTMPPackPcs, Messages.REQTMPPCS, ValidationType.Required));            
            return ValidateForm();
        }

        
        /// <summary>
        /// NUMERIC VALIDATION FOR TMPPACKPCS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmpPackPcs_Leave(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtTMPPackPcs.Text))
            {
                if (!Validator.IsValidInput(Framework.Constants.ValidationType.Integer, txtTMPPackPcs.Text))
                {
                    txtTMPPackPcs.Text = string.Empty;
                }
            }
        }       
        /// <summary>
        /// CLEAR FORM CONTROLS AND SET FOCUS TO FIRST EDITABLE FIELD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)                       
            {
                ClearForm();
                txtSerialNumber.Focus();
            }
        }
        /// <summary>
        /// CLEAR FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            txtSerialNumber.Text = string.Empty;
            txtBatchNumber.Text = string.Empty;
            txtGloveType.Text = string.Empty;
            txtSize.Text = string.Empty;
            txtBatchWeight.Text = string.Empty;
            txtTenPcs.Text = string.Empty;
            txtQcType.Text = string.Empty;
            txtTenPcs.Text = string.Empty;
            txtPNPcs.Text = string.Empty;
            txtQCDescription.Text = String.Empty;
            if (cmbTMPPackReason.DataSource != null)
                cmbTMPPackReason.SelectedIndex = Constants.MINUSONE;
            txtTMPPackPcs.Text = string.Empty;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
            LoadTMPPackReasons();
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
            ClearForm();
        }   
        

    }
}
