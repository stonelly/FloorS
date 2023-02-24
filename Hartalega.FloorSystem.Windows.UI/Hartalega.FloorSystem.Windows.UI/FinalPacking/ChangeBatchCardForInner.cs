using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ChangeBatchCardForInner : FormBase
    {
        private FinalPackingDTO _objFinalPackingDTO = null;
        private string _screenname = "Change Batch Card for Inner";
        private string _uiClassName = "ChangeBatchCardForInner";
        private Boolean _isTempack = false;
        private string _operatorName = String.Empty;
        private decimal _serialNumber = Constants.ZERO;
        private string _size = string.Empty;
        private string _gloveType = string.Empty;

        #region CONSTRUCTOR
        public ChangeBatchCardForInner()
        {
            InitializeComponent();
        }
        #endregion

        #region EVENTS
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeBatchCardForInner_Load(object sender, EventArgs e)
        {
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
            {
                txtStationNo.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
            }
            else
                GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

            txtPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
            BindQCGroups();
            txtNewSerialNumber.SerialNo();
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// Update new serial number for InternalLotNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            FPChangeBatchCardDTO objFPChangeBatchCardDTO = new FPChangeBatchCardDTO();
            try
            {
                if (ValidateRequiredFields())
                {
                    //Vinoden : FP Validation against TOMS
                    string[] serialNo = new string[] { txtNewSerialNumber.Text.Trim() };
                    if (ValidateBatchCard_TOMS(serialNo))
                    {
                        int NewBatchtotalPcs = FinalPackingBLL.GetBatchCapacity(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()));
                        if (NewBatchtotalPcs == 0)
                        {
                            GlobalMessageBox.Show(Messages.NEW_BATCH_PACKING_QTY_ZERO, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtNewSerialNumber.Focus();
                        }
                        else if (_objFinalPackingDTO.Boxespacked * _objFinalPackingDTO.innerBoxCapacity > NewBatchtotalPcs)
                        {
                            GlobalMessageBox.Show(Messages.NEW_BATCH_PACKING_QTY_LESS, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtNewSerialNumber.Focus();
                        }
                        else
                        {
                            objFPChangeBatchCardDTO.InternalLotNumber = txtInternalLotNo.Text.Trim();
                            objFPChangeBatchCardDTO.NewSerialNumber = Convert.ToDecimal(txtNewSerialNumber.Text.Trim());
                            objFPChangeBatchCardDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
                            objFPChangeBatchCardDTO.LocationId = WorkStationDTO.GetInstance().LocationId;
                            int rowsreturned = FinalPackingBLL.ChangeBatchCardForInner(objFPChangeBatchCardDTO);
                            if (rowsreturned > 0)
                            {
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                ClearForm();
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                FinalPackingBLL.DeleteChangeBatchCardData(objFPChangeBatchCardDTO.InternalLotNumber);
                ExceptionLogging(ex, _screenname, _uiClassName, "btnSave_Click", null);
            }
        }

        /// <summary>
        /// INTERNALLOTNUMBER LEAVE EVENT TO VALIDATE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInternalLotNumber_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInternalLotNo.Text))
                {
                    if (FinalPackingBLL.validateInnerLotNumberforCBCI(txtInternalLotNo.Text, Constants.FIVE))
                    {
                        _objFinalPackingDTO = FinalPackingBLL.GetInternalLotNumberDetails(txtInternalLotNo.Text);
                        txtPoNumber.Text = string.IsNullOrEmpty(_objFinalPackingDTO.CustomerReferenceNumber) ? _objFinalPackingDTO.Ponumber : _objFinalPackingDTO.CustomerReferenceNumber + " | " + _objFinalPackingDTO.Ponumber;
                        txtItemNumber.Text = Convert.ToString(_objFinalPackingDTO.ItemNumber);
                        txtItemName.Text = _objFinalPackingDTO.ItemName;
                        _serialNumber = _objFinalPackingDTO.Serialnumber;
                        _size = _objFinalPackingDTO.Size;
                        _gloveType = _objFinalPackingDTO.GloveType;
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALIDINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtInternalLotNo.Text = string.Empty;
                        txtInternalLotNo.Focus();
                    }
                }
                else
                {
                    ClearForm();
                    txtNewSerialNumber.Text = String.Empty;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtInternalLotNumber_Leave", null);
            }

        }

        /// <summary>
        /// TO VALIDATE NEW SERIAL NUMBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNewSerialNo_Leave(object sender, EventArgs e)
        {

            BatchDTO objBatchDTO = new BatchDTO();
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
            SOLineDTO objSolIneDTO = new SOLineDTO();
            string batchNumber = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(txtNewSerialNumber.Text.Trim()) && !string.IsNullOrEmpty(txtInternalLotNo.Text.Trim()))
                {
                    if (!HourlyBatchCardBLL.IsSerialNumber(Convert.ToDecimal(txtNewSerialNumber.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtNewSerialNumber.Text = string.Empty;
                        txtNewSerialNumber.Focus();
                    }
                    else if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtNewSerialNumber.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtNewSerialNumber.Text = string.Empty;
                        txtNewSerialNumber.Focus();
                    }
                    else
                    {                       
                        string qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtNewSerialNumber.Text)); //validate QAI status
                        if (string.IsNullOrEmpty(qaiResults))
                        {
                            #region FP exempt on production date validation
                            FP_ExemptValidateDTO fpExemptValidateDTO = FinalPackingBLL.ValidateSerialNoByFPExemptProductionDateValidation(Convert.ToDecimal(txtNewSerialNumber.Text), txtItemNumber.Text);
                            if (fpExemptValidateDTO.Result.ToUpper() == Constants.FAIL)
                            {
                                var errMsg = string.Format(Messages.FPEXEMPT_PRODUCTIONDATEVALIDATION_ERR, fpExemptValidateDTO.ProductionDateValidationDays.ToString(), fpExemptValidateDTO.ProductionDateValidationCustomer);
                                GlobalMessageBox.Show(errMsg, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtNewBatchNumber.Text = string.Empty;
                                txtNewBatchNumber.Focus();
                            }
                            #endregion
                            else if (FinalPackingBLL.ValidateHotbox(txtNewSerialNumber.Text.Trim()) > Constants.ZERO)
                            {
                                int i = FinalPackingBLL.ValidateHotbox(txtNewSerialNumber.Text.Trim());

                                if (i == Constants.ONE)
                                    GlobalMessageBox.Show(Messages.HOTBOX_FAILMSG, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                else if (i == Constants.TWO)
                                    GlobalMessageBox.Show(Messages.HOTBOX_PENDINGRESULT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                else if (i == Constants.THREE)
                                    GlobalMessageBox.Show(Messages.HOTBOX_RESULTEXPIRED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                else if (i == Constants.FOUR)
                                    GlobalMessageBox.Show(Messages.HOTBOX_OLDBATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);

                                txtNewSerialNumber.Text = string.Empty;
                                txtNewSerialNumber.Focus();
                            }
                            else if (FinalPackingBLL.ValidateSerialNoByQIStatus(Convert.ToDecimal(txtNewSerialNumber.Text)) == Constants.PASS)
                            {

                                // Comment: disable due to FP station need to proceed before QA testing result out/ready
                                //if (ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.POLYMER_TEST)
                                //        && ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.POWDER_TEST)
                                //        && ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.PROTEIN_TEST)
                                //        && ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.HOTBOX_TEST))
                                //{
                                string strlocation = null;
                                if (!FinalPackingBLL.isBatchScanOut(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), out strlocation))
                                {
                                    GlobalMessageBox.Show(String.Format(Messages.NO_SCAN_OUT_SNO_FP, strlocation), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtNewSerialNumber.Text = string.Empty;
                                    txtNewSerialNumber.Focus();
                                }
                                else
                                {
                                    objFPTempPackDTO = FinalPackingBLL.CheckIsTemPackBatch(decimal.Parse(txtNewSerialNumber.Text.Trim()));
                                    objSolIneDTO = FinalPackingBLL.GetPODetailsByInternalLotNumber(txtInternalLotNo.Text.Trim());
                                    if (objFPTempPackDTO.isTempPackBatch)
                                    {
                                        batchNumber = objFPTempPackDTO.TMPPackBatch.BatchNumber;
                                        _isTempack = true;
                                        if (!CheckGloveTypeAndSize(objFPTempPackDTO.TMPPackBatch.GloveType, objFPTempPackDTO.TMPPackBatch.Size, objSolIneDTO))
                                        {
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        //objBatchDTO = FinalPackingBLL.ValidateSerialNumber(int.Parse(txtNewSerialNumber.Text.Trim()), _objFinalPackingDTO.Size);
                                        objBatchDTO = FinalPackingBLL.GetBatchDetailsBySNo(decimal.Parse(txtNewSerialNumber.Text.Trim()));
                                        batchNumber = objBatchDTO.BatchNumber;
                                        _isTempack = false;
                                        if (!CheckGloveTypeAndSize(objBatchDTO.GloveType, objBatchDTO.Size, objSolIneDTO))
                                        {
                                            return;
                                        }

                                    }
                                    if (objBatchDTO.IsFPBatchSplit || objFPTempPackDTO.TMPPackBatch.IsFPBatchSplit)
                                    {
                                        if (ValidateSplitBatch())
                                        {
                                            if (!string.IsNullOrEmpty(batchNumber))
                                            {
                                                txtNewBatchNumber.Text = batchNumber;
                                                btnSave.Enabled = true;
                                            }
                                            else
                                            {
                                                GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                txtNewSerialNumber.Text = String.Empty;
                                                txtNewSerialNumber.Focus();
                                                btnSave.Enabled = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(batchNumber))
                                        {
                                            txtNewBatchNumber.Text = batchNumber;
                                            btnSave.Enabled = true;
                                            btnSave.Focus();
                                        }
                                        else
                                        {
                                            GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                            txtNewSerialNumber.Text = String.Empty;
                                            txtNewBatchNumber.Text = String.Empty;
                                            txtNewSerialNumber.Focus();
                                        }
                                    }
                                }
                                //}
                                //else
                                //{
                                //    txtNewSerialNumber.Text = string.Empty;
                                //    txtNewSerialNumber.Focus();
                                //}
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.QI_TESTRESULT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtNewSerialNumber.Text = string.Empty;
                                txtNewSerialNumber.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(qaiResults, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtNewSerialNumber.Clear();
                            txtNewSerialNumber.Focus();
                        }
                    }

                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtNewSerialNo_Leave", null);
            }
        }



        /// <summary>
        /// TO CLEAR FORM ON CONFIRMATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                txtInternalLotNo.Focus();
            }
        }
        #endregion

        #region USER METHODS


        private bool CheckGloveTypeAndSize(string gloveType, string size, SOLineDTO objSolIneDTO)
        {
            string gloveCodes = _gloveType;

            if (!string.IsNullOrEmpty(objSolIneDTO.AlternateGloveCode1))
            {
                gloveCodes += ", " + objSolIneDTO.AlternateGloveCode1;
            }
            if (!string.IsNullOrEmpty(objSolIneDTO.AlternateGloveCode2))
            {
                gloveCodes += ", " + objSolIneDTO.AlternateGloveCode2;
            }
            if (!string.IsNullOrEmpty(objSolIneDTO.AlternateGloveCode3))
            {
                gloveCodes += ", " + objSolIneDTO.AlternateGloveCode3;
            }

            if ((!gloveType.Equals(_gloveType) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode1) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode2) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode3))
                && !size.Equals(_size))
            {
                GlobalMessageBox.Show(string.Format(Messages.TYPE_SIZE_NOTMATCH,gloveType,size,Environment.NewLine, gloveCodes, Environment.NewLine,_size), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtNewSerialNumber.Text = string.Empty;
                txtNewBatchNumber.Text = string.Empty;
                btnSave.Enabled = false;
                txtNewSerialNumber.Focus();
                return false;
            }
            else if (!gloveType.Equals(_gloveType) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode1) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode2) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode3))
            {
                GlobalMessageBox.Show(string.Format(Messages.TYPE_NOTMATCH,gloveType,Environment.NewLine, gloveCodes), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtNewSerialNumber.Text = string.Empty;
                txtNewBatchNumber.Text = string.Empty;
                btnSave.Enabled = false;
                txtNewSerialNumber.Focus();
                return false;
            }
            else if (!size.Equals(_size))
            {
                GlobalMessageBox.Show(string.Format(Messages.SIZE_NOTMATCH, size, Environment.NewLine, _size), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtNewSerialNumber.Text = string.Empty;
                txtNewBatchNumber.Text = string.Empty;
                btnSave.Enabled = false;
                txtNewSerialNumber.Focus();
                return false;
            }

            return true;
        }

        private void BindQCGroups()
        {
            List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups();
            cmbGroupId.BindComboBox(lstDropDown, true);
        }

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
        /// TO VALIDATE SPLIT BATCH 
        /// </summary>
        /// <returns>returns true for successful authentication</returns>
        private Boolean ValidateSplitBatch()
        {
            Login _passwordForm = new Login(Constants.Modules.FINALPACKING, _screenname);
            _passwordForm.ShowDialog();
            try
            {
                if (_passwordForm.Authentication != Convert.ToString(Constants.ZERO) && !string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
                {
                    _operatorName = Convert.ToString(_passwordForm.Authentication);
                    return true;
                }
                else
                {
                    txtNewSerialNumber.Clear();
                    txtNewBatchNumber.Clear();
                    return false;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateSplitBatch", null);
                return false;
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
        /// <summary>
        /// CLEAR THE FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            // bug fix - 20200420084112194195 : btnSave button enabled after saved record
            btnSave.Enabled = false;

            txtInternalLotNo.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtItemNumber.Text = string.Empty;
            txtNewBatchNumber.Text = string.Empty;
            txtNewSerialNumber.Text = string.Empty;
            txtPoNumber.Text = string.Empty;
            cmbGroupId.SelectedIndex = Constants.MINUSONE;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
            BindQCGroups();

        }
        /// <summary>
        /// VALIDATE REQUIRED FIELDS
        /// </summary>
        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtInternalLotNo, Messages.REQINTERNALLOTNUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtNewSerialNumber, Messages.REQNEWSERIALNUMBER, ValidationType.Required));
            return ValidateForm();
        }

        private Boolean ValidateBatchCard_TOMS(string[] arr_serialNo)
        {
            bool proceedPacking = false;
            string SerialNO = string.Empty;
            try
            {

                if (arr_serialNo.Count() > 1)
                {
                    SerialNO = String.Join(",", arr_serialNo);
                }
                else
                {
                    SerialNO = arr_serialNo[0];
                }

                List<FP_TOMSValidateDTO> lstResult = new List<FP_TOMSValidateDTO>();

                lstResult = FinalPackingBLL.ValidateBatch_TOMS(SerialNO);

                if (lstResult != null && lstResult.Count() > 0)
                {
                    foreach (var item in lstResult)
                    {
                        if (item.Status.Equals(1) || item.Status.Equals(2))
                        {
                            FloorSystemException floorException = new FloorSystemException("FP_TomsValidation", "FinalPacking", new Exception(String.Format("Registration Removed in TOMS for {0}", item.SerialNo)), true);
                            CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ChangeBatchCardForInner", SerialNO);
                        }
                        else if (item.Status.Equals(3))
                        {
                            FloorSystemException floorException = new FloorSystemException("FP_TomsValidation", "FinalPacking", new Exception(String.Format("Batch ({0}) has not been scanned out from TOMS", item.SerialNo)), true);
                            CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ChangeBatchCardForInner", SerialNO);
                        }
                    }

                    if (lstResult.Any(x => x.SerialNo.Equals(SerialNO) && (x.Status.Equals(1) || x.Status.Equals(2))))
                    {
                        GlobalMessageBox.Show("The Batch Card has been Registered in TOMS. Please remove from TOMS before proceed to Final Packing", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                    else if (lstResult.Any(x => x.SerialNo.Equals(SerialNO) && (x.Status.Equals(3))))
                    {
                        GlobalMessageBox.Show("The Batch Card has been scan in/inventorized in TOMS. Please scan out from TOMS before proceed to Final Packing", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    proceedPacking = true;
                }
            }
            catch (Exception ex)
            {
                FloorSystemException floorException = new FloorSystemException("FP_TomsValidation", "FinalPacking", new Exception(ex.ToString()), true);
                CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ScanBatchInnerOuter", SerialNO);
                proceedPacking = true;
            }
            return proceedPacking;
        }

        #endregion
    }
}
