using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using Hartalega.FloorSystem.IntegrationServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Drawing;
using System.Linq;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ChangeBatchCardForInnerV2 : FormBase
    {
        private FinalPackingDTO _objFinalPackingDTO = null;
        private string _screenname = "Change Batch Card for Inner";
        private string _uiClassName = "ChangeBatchCardForInner";
        private Boolean _isTempack = false;
        private string _operatorName = String.Empty;
        private decimal _serialNumber = Constants.ZERO;
        private string _size = string.Empty;
        private string _gloveType = string.Empty;
        private int _packSize = Constants.ZERO;
        private int _caseSize = Constants.ZERO;


        #region CONSTRUCTOR
        public ChangeBatchCardForInnerV2()
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
            //txtNewSerialNumber.SerialNo();
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// Update new serial number for InternalLotNumber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateBatchCard_TOMS())
                {
                    if (CheckPSIStatus(_objFinalPackingDTO.PSIStatus) && ValidateRequiredFields() && ValidateRemainingCases())
                    {
                        FPChangeBatchCardV2DTO objFPChangeBatchCardDTO = new FPChangeBatchCardV2DTO();
                        objFPChangeBatchCardDTO.InternalLotNumber = txtInternalLotNo.Text.Trim();
                        objFPChangeBatchCardDTO.InternalLotNumberQty = Convert.ToInt32(txtQty.Text);
                        objFPChangeBatchCardDTO.PackSize = Convert.ToInt32(txtPackSize.Text);
                        objFPChangeBatchCardDTO.ItemNumber = txtItemNumber.Text.Trim();
                        objFPChangeBatchCardDTO.PSIReworkOrderNo = cmbPSIReworkOrderNo.Text.Trim();

                        objFPChangeBatchCardDTO.NewSerialNumber1 = Convert.ToDecimal(txtFirstSerialNo.Text.Trim());
                        objFPChangeBatchCardDTO.NewSerialNumber1Qty = Convert.ToInt32(txtFirstQty.Text.Trim());
                        objFPChangeBatchCardDTO.NewSerialNumber1QtyUse = Convert.ToInt32(txtFirstSerialUseQty.Text.Trim());
                        objFPChangeBatchCardDTO.NewSerialNumber1QtyInner = Convert.ToInt32(txtFirstSerialUseQty.Text.Trim()) / _packSize;
                        objFPChangeBatchCardDTO.NewSerialNumber1QtyCase = Convert.ToInt32(txtFirstSerialUseQty.Text.Trim()) / _caseSize;

                        if (!string.IsNullOrEmpty(txtSecondSerialNo.Text))
                        {
                            objFPChangeBatchCardDTO.NewSerialNumber2 = Convert.ToDecimal(txtSecondSerialNo.Text.Trim());
                            objFPChangeBatchCardDTO.NewSerialNumber2Qty = Convert.ToInt32(txtSecondQty.Text.Trim());
                            objFPChangeBatchCardDTO.NewSerialNumber2QtyUse = Convert.ToInt32(txtSecondSerialUseQty.Text.Trim());
                            objFPChangeBatchCardDTO.NewSerialNumber2QtyInner = Convert.ToInt32(txtSecondSerialUseQty.Text.Trim()) / _packSize;
                            objFPChangeBatchCardDTO.NewSerialNumber2QtyCase = Convert.ToInt32(txtSecondSerialUseQty.Text.Trim()) / _caseSize;

                            if (!string.IsNullOrEmpty(txtThirdSerialNo.Text))
                            {
                                objFPChangeBatchCardDTO.NewSerialNumber3 = Convert.ToDecimal(txtThirdSerialNo.Text.Trim());
                                objFPChangeBatchCardDTO.NewSerialNumber3Qty = Convert.ToInt32(txtThirdQty.Text.Trim());
                                objFPChangeBatchCardDTO.NewSerialNumber3QtyUse = Convert.ToInt32(txtThirdSerialUseQty.Text.Trim());
                                objFPChangeBatchCardDTO.NewSerialNumber3QtyInner = Convert.ToInt32(txtThirdSerialUseQty.Text.Trim()) / _packSize;
                                objFPChangeBatchCardDTO.NewSerialNumber3QtyCase = Convert.ToInt32(txtThirdSerialUseQty.Text.Trim()) / _caseSize;
                            }
                        }

                        //objFPChangeBatchCardDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
                        objFPChangeBatchCardDTO.LocationId = WorkStationDTO.GetInstance().LocationId;
                        int rowsreturned = FinalPackingBLL.ChangeBatchCardForInnerV2(objFPChangeBatchCardDTO);
                        //int rowsreturned = 1;
                        if (rowsreturned > 0)
                        {
                            //AX Posting
                            //bool isPostingSuccess = false;
                            try
                            {
                                bool isPostingSuccess = AXPostingBLL.PostAXDataFinalPackingCBCI(objFPChangeBatchCardDTO, Convert.ToString(Convert.ToInt16(Constants.SubModules.ChangeBatchCardInner)));
                                //bool isPostingSuccess = AXPostingBLL.PostAXDataFinalPackingCBCI(objFPChangeBatchCardDTO.InternalLotNumber, Convert.ToString(Convert.ToInt16(Constants.SubModules.ChangeBatchCardInner)));
                                if (!isPostingSuccess)
                                {
                                    FinalPackingBLL.DeleteChangeBatchCardDataV2(objFPChangeBatchCardDTO);
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
                            }
                            catch (FloorSystemException ex)
                            {
                                FinalPackingBLL.DeleteChangeBatchCardDataV2(objFPChangeBatchCardDTO);
                                ExceptionLogging(ex, _screenname, _uiClassName, "btnSave_Click", null);
                            }
                        }

                    }
                }
            }
            catch (FloorSystemException ex)
            {
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
                        var fpDTO = FinalPackingBLL.GetInternalLotNumberDetailsWithPcs(txtInternalLotNo.Text, "usp_FP_CheckInternalLotNumberWithPcs_Get");
                        if (fpDTO.Count == 0)
                        {
                            var fpDTOList = FinalPackingBLL.GetInternalLotNumberDetailsWithPcs(txtInternalLotNo.Text, "usp_FP_InternalLotNumberWithPcs_Get");
                            if (fpDTOList.Count > 0)
                            {
                                // bind the batch order list
                                this.cmbPSIReworkOrderNo.SelectedIndexChanged -= CmbPSIReworkOrderNo_SelectedIndexChanged;
                                BindBatchOrder(cmbPSIReworkOrderNo, fpDTOList);
                                this.cmbPSIReworkOrderNo.SelectedIndexChanged += CmbPSIReworkOrderNo_SelectedIndexChanged;
                                this.cmbPSIReworkOrderNo.SelectedIndex = Constants.ZERO;
                                CmbPSIReworkOrderNo_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.NOPSIREWORKORDERNO, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                txtInternalLotNo.Text = string.Empty;
                                txtInternalLotNo.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INTERNALLOTNOISPOSTED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtInternalLotNo.Text = string.Empty;
                            txtInternalLotNo.Focus();
                        }
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
                    //txtNewSerialNumber.Text = String.Empty;
                    txtFirstSerialNo.Text = String.Empty;
                    txtSecondSerialNo.Text = String.Empty;
                    txtThirdSerialNo.Text = String.Empty;

                    txtFirstSerialNo.Enabled = false;
                    txtSecondSerialNo.Enabled = false;
                    txtThirdSerialNo.Enabled = false;

                    txtFirstSerialNo.BackColor = SystemColors.Control;
                    txtSecondSerialNo.BackColor = SystemColors.Control;
                    txtThirdSerialNo.BackColor = SystemColors.Control;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtInternalLotNumber_Leave", null);
            }

        }

        private void CmbPSIReworkOrderNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _objFinalPackingDTO = cmbPSIReworkOrderNo.SelectedItem as FinalPackingDTO;
            txtPoNumber.Text = string.IsNullOrEmpty(_objFinalPackingDTO.CustomerReferenceNumber) ? _objFinalPackingDTO.Ponumber : _objFinalPackingDTO.CustomerReferenceNumber + " | " + _objFinalPackingDTO.Ponumber;
            if (!(string.IsNullOrEmpty(txtPoNumber.Text)))
            {
                CheckPSIStatus(_objFinalPackingDTO.PSIStatus); //#AZRUL 16-10-2018 BUGS 1207: Prompt message if PSI Rework Order not started.
                txtItemNumber.Text = Convert.ToString(_objFinalPackingDTO.ItemNumber);
                txtItemName.Text = _objFinalPackingDTO.ItemName;
                txtQty.Text = _objFinalPackingDTO.TotalPcs.ToString().Trim();
                txtRemainingQty.Text = _objFinalPackingDTO.Casespacked.ToString().Trim();
                txtCases.Text = _objFinalPackingDTO.Casespacked.ToString().Trim();
                //txtPackSize.Text = (_objFinalPackingDTO.TotalPcs / _objFinalPackingDTO.Boxespacked).ToString();

                int InnerPerCases = (int)_objFinalPackingDTO.Boxespacked / (int)_objFinalPackingDTO.Casespacked;
                int PcsPerCases = (_objFinalPackingDTO.TotalPcs / _objFinalPackingDTO.Boxespacked) * InnerPerCases;

                txtPackSize.Text = PcsPerCases.ToString();
                _serialNumber = _objFinalPackingDTO.Serialnumber;
                _size = _objFinalPackingDTO.Size;
                _gloveType = _objFinalPackingDTO.GloveType;
                _caseSize = PcsPerCases;
                _packSize = _objFinalPackingDTO.TotalPcs / _objFinalPackingDTO.Boxespacked;
                txtFirstSerialNo.Enabled = true;
                txtFirstSerialNo.BackColor = SystemColors.Window;
                btnReset.Enabled = true;
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALIDPO, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                txtInternalLotNo.Text = string.Empty;
                txtInternalLotNo.Focus();
            }
        }


        /// <summary>
        ///#3.List out all Batch Order based on selected Resource
        /// <summary>
        private void BindBatchOrder(ComboBox cmbBatchOrder, List<FinalPackingDTO> fpList)
        {
            cmbBatchOrder.Enabled = true;
            cmbBatchOrder.ValueMember = null;
            cmbBatchOrder.DataSource = null;
            cmbBatchOrder.Items.Clear();
            cmbBatchOrder.DataSource = fpList;
            cmbBatchOrder.ValueMember = "PSIReworkOrderNo";
            cmbBatchOrder.DisplayMember = "PSIReworkOrderNo";
        }

        /// <summary>
        /// TO VALIDATE NEW SERIAL NUMBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /* OLD CODE
        private void txtNewSerialNo_Leave(object sender, EventArgs e)
        {

            BatchDTO objBatchDTO = new BatchDTO();
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
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
                            if (FinalPackingBLL.ValidateSerialNoByQIStatus(Convert.ToDecimal(txtNewSerialNumber.Text)) == Constants.PASS)
                            {


                                if (ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.POLYMER_TEST)
                                        && ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.POWDER_TEST)
                                        && ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.PROTEIN_TEST)
                                        && ValidateQATestResult(Convert.ToDecimal(txtNewSerialNumber.Text.Trim()), Constants.HOTBOX_TEST))
                                {

                                    objFPTempPackDTO = FinalPackingBLL.CheckIsTemPackBatch(decimal.Parse(txtNewSerialNumber.Text.Trim()));
                                    if (objFPTempPackDTO.isTempPackBatch)
                                    {
                                        batchNumber = objFPTempPackDTO.TMPPackBatch.BatchNumber;
                                        _isTempack = true;
                                        if (!CheckGloveTypeAndSize(objFPTempPackDTO.TMPPackBatch.GloveType, objFPTempPackDTO.TMPPackBatch.Size))
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
                                        if (!CheckGloveTypeAndSize(objBatchDTO.GloveType, objBatchDTO.Size))
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
                                else
                                {
                                    txtNewSerialNumber.Text = string.Empty;
                                    txtNewSerialNumber.Focus();
                                }
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
        */


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

        private Boolean ValidateBatchCard_TOMS()
        {
            bool proceedPacking = false;
            string SerialNO = string.Empty;
            List<String> serialNos = new List<string>();
            try
            {

                if (!String.IsNullOrEmpty(txtFirstSerialNo.Text.Trim()))
                {
                    serialNos.Add(txtFirstSerialNo.Text.Trim());
                }
                else if (!String.IsNullOrEmpty(txtSecondSerialNo.Text.Trim()))
                {
                    serialNos.Add(txtSecondSerialNo.Text.Trim());
                }
                else if (!String.IsNullOrEmpty(txtThirdSerialNo.Text.Trim()))
                {
                    serialNos.Add(txtThirdSerialNo.Text.Trim());
                }


                if (serialNos.Count() > 1)
                {
                    SerialNO = String.Join(",", serialNos);
                }
                else
                {
                    SerialNO = serialNos[0];
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

        #region USER METHODS

        private bool CheckGloveTypeAndSize(string gloveType, string size, TextBox txtSerial, TextBox txtBatchNo, SOLineDTO objSolIneDTO)
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
                GlobalMessageBox.Show(string.Format(Messages.TYPE_SIZE_NOTMATCH, gloveType, size, Environment.NewLine, gloveCodes, Environment.NewLine, _size), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtSerial.Text = string.Empty;
                txtBatchNo.Text = string.Empty;
                btnSave.Enabled = false;
                txtSerial.Focus();
                return false;
            }
            else if (!gloveType.Equals(_gloveType) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode1) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode2) && !gloveType.Equals(objSolIneDTO.AlternateGloveCode3))
            {
                GlobalMessageBox.Show(string.Format(Messages.TYPE_NOTMATCH, gloveType, Environment.NewLine, gloveCodes), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtSerial.Text = string.Empty;
                txtBatchNo.Text = string.Empty;
                btnSave.Enabled = false;
                txtSerial.Focus();
                return false;
            }
            else if (!size.Equals(_size))
            {
                GlobalMessageBox.Show(string.Format(Messages.SIZE_NOTMATCH, size, Environment.NewLine, _size), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                txtSerial.Text = string.Empty;
                txtBatchNo.Text = string.Empty;
                btnSave.Enabled = false;
                txtSerial.Focus();
                return false;
            }
            //txtSerial.Text = string.Empty;
            //txtBatchNo.Text = string.Empty;
            //btnSave.Enabled = false;
            //txtSerial.Focus();
            //return false;
            return true;
        }

        //#AZRUL 16-10-2018 BUGS 1207: Prompt message if PSI Rework Order not started. START
        private bool CheckPSIStatus(string PSIStatus)
        {
            if (!(PSIStatus == "StartedUp"))
            {
                GlobalMessageBox.Show(Messages.PSIREWORKORDERNOTSTARTED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                cmbPSIReworkOrderNo.Focus();
                return false;
            }
            return true;
        }
        //#AZRUL 16-10-2018 BUGS 1207: Prompt message if PSI Rework Order not started. END

        private void BindQCGroups()
        {
            List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups();
            //cmbGroupId.BindComboBox(lstDropDown, true);
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
        private Boolean ValidateSplitBatch(TextBox txtSerial, TextBox txtBatch)
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
                    txtSerial.Clear();
                    txtBatch.Clear();
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
            txtQty.Text = string.Empty;
            txtRemainingQty.Text = string.Empty;
            //txtNewBatchNumber.Text = string.Empty;
            //txtNewSerialNumber.Text = string.Empty;
            cmbPSIReworkOrderNo.SelectedIndexChanged -= CmbPSIReworkOrderNo_SelectedIndexChanged;
            cmbPSIReworkOrderNo.DataSource = null;
            cmbPSIReworkOrderNo.Items.Clear();
            txtFirstSerialNo.Text = string.Empty;
            txtFirstBatchInfo.Text = string.Empty;
            txtFirstQty.Text = string.Empty;
            txtFirstSerialUseQty.Text = string.Empty;
            txtFirstSerialUseQty.ReadOnly = true;
            txtFirstSerialUseQty.BackColor = SystemColors.Control;
            txtFirstBal.Text = string.Empty;

            txtSecondSerialNo.Text = string.Empty;
            txtSecondBatchInfo.Text = string.Empty;
            txtSecondQty.Text = string.Empty;
            txtSecondSerialUseQty.Text = string.Empty;
            txtSecondSerialUseQty.ReadOnly = true;
            txtSecondSerialUseQty.BackColor = SystemColors.Control;
            txtSecondBal.Text = string.Empty;

            txtThirdSerialNo.Text = string.Empty;
            txtThirdBatchInfo.Text = string.Empty;
            txtThirdQty.Text = string.Empty;
            txtThirdSerialUseQty.Text = string.Empty;
            txtThirdSerialUseQty.ReadOnly = true;
            txtThirdSerialUseQty.BackColor = SystemColors.Control;
            txtThirdBal.Text = string.Empty;

            txtPoNumber.Text = string.Empty;
            //cmbGroupId.SelectedIndex = Constants.MINUSONE;
            txtCases.Text = "";
            txtPackSize.Text = "";

            txtFirstSerialNo.ReadOnly = false;
            txtFirstSerialNo.Enabled = true;

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

        /// bug fix - 20200420084112194195 :
        /// <summary>
        /// VALIDATE REMAINING CASES
        /// </summary>
        private Boolean ValidateRemainingCases()
        {
            return txtRemainingQty.Text == "0";
        }

        /// <summary>
        /// VALIDATE REQUIRED FIELDS
        /// </summary>
        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtInternalLotNo, Messages.REQINTERNALLOTNUMBER, ValidationType.Required));
            //validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtFirstSerialNo, Messages.REQNEWSERIALNUMBER, ValidationType.Required));
            if (txtSecondSerialNo.Text != string.Empty)
                validationMesssageLst.Add(new ValidationMessage(txtSecondSerialNo, Messages.REQNEWSERIALNUMBER, ValidationType.Required));
            if (txtThirdSerialNo.Text != string.Empty)
                validationMesssageLst.Add(new ValidationMessage(txtThirdSerialNo, Messages.REQNEWSERIALNUMBER, ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// VALIDATE SERIAL
        /// </summary>
        /// <returns></returns> 
        private bool ValidateSerialBatch(TextBox txtSerial, TextBox txtBatch, TextBox txtQty)
        {
            bool ret = false;
            BatchDTO objBatchDTO = new BatchDTO();
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
            SOLineDTO objSolIneDTO = new SOLineDTO();
            string batchNumber = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(txtSerial.Text.Trim()) && !string.IsNullOrEmpty(txtInternalLotNo.Text.Trim()))
                {
                    if (!HourlyBatchCardBLL.IsSerialNumber(Convert.ToDecimal(txtSerial.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerial.Text = string.Empty;
                        txtSerial.Focus();
                    }
                    else if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerial.Text.Trim())))
                    {
                        GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerial.Text = string.Empty;
                        txtSerial.Focus();
                    }
                    else if (FinalPackingBLL.ValidateHotbox(txtSerial.Text.Trim()) > Constants.ZERO)
                    {
                        int i = FinalPackingBLL.ValidateHotbox(txtSerial.Text.Trim());

                        if (i == Constants.ONE)
                            GlobalMessageBox.Show(Messages.HOTBOX_FAILMSG, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        else if (i == Constants.TWO)
                            GlobalMessageBox.Show(Messages.HOTBOX_PENDINGRESULT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        else if (i == Constants.THREE)
                            GlobalMessageBox.Show(Messages.HOTBOX_RESULTEXPIRED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        else if (i == Constants.FOUR)
                            GlobalMessageBox.Show(Messages.HOTBOX_OLDBATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);

                        txtSerial.Text = string.Empty;
                        txtSerial.Focus();
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                    else if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerial.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                            !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerial.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerial.Text = string.Empty;
                        txtSerial.Focus();
                    }
                    else if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerial.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                    {
                        GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSerial.Text = string.Empty;
                        txtSerial.Focus();
                    }
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END
                    else
                    {
                        string qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerial.Text)); //validate QAI status
                        if (string.IsNullOrEmpty(qaiResults))
                        {
                            #region FP exempt on production date validation
                            FP_ExemptValidateDTO fpExemptValidateDTO = FinalPackingBLL.ValidateSerialNoByFPExemptProductionDateValidation(Convert.ToDecimal(txtSerial.Text), txtItemNumber.Text);
                            if (fpExemptValidateDTO.Result.ToUpper() == Constants.FAIL)
                            {
                                var errMsg = string.Format(Messages.FPEXEMPT_PRODUCTIONDATEVALIDATION_ERR, fpExemptValidateDTO.ProductionDateValidationDays.ToString(), fpExemptValidateDTO.ProductionDateValidationCustomer);
                                GlobalMessageBox.Show(errMsg, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtSerial.Text = string.Empty;
                                txtSerial.Focus();
                            }
                            #endregion
                            else if (FinalPackingBLL.ValidateSerialNoByQIStatus(Convert.ToDecimal(txtSerial.Text)) == Constants.PASS)
                            {
                                if (ValidateQATestResult(Convert.ToDecimal(txtSerial.Text.Trim()), Constants.POLYMER_TEST)
                                        && ValidateQATestResult(Convert.ToDecimal(txtSerial.Text.Trim()), Constants.POWDER_TEST)
                                        && ValidateQATestResult(Convert.ToDecimal(txtSerial.Text.Trim()), Constants.PROTEIN_TEST)
                                        && ValidateQATestResult(Convert.ToDecimal(txtSerial.Text.Trim()), Constants.HOTBOX_TEST))
                                {
                                    string strlocation = null;
                                    if (!FinalPackingBLL.isBatchScanOut(Convert.ToDecimal(txtSerial.Text.Trim()), out strlocation))
                                    {
                                        GlobalMessageBox.Show(String.Format(Messages.NO_SCAN_OUT_SNO_FP, strlocation), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                        txtSerial.Text = string.Empty;
                                        txtSerial.Focus();
                                    }
                                    else
                                    {
                                        objFPTempPackDTO = FinalPackingBLL.CheckIsTemPackBatch(decimal.Parse(txtSerial.Text.Trim()));
                                        objSolIneDTO = FinalPackingBLL.GetPODetailsByInternalLotNumber(txtInternalLotNo.Text.Trim());
                                        if (objFPTempPackDTO.isTempPackBatch)
                                        {
                                            batchNumber = objFPTempPackDTO.TMPPackBatch.BatchNumber;
                                            txtBatch.Text = batchNumber;
                                            _isTempack = true;
                                            if (!CheckGloveTypeAndSize(objFPTempPackDTO.TMPPackBatch.GloveType, objFPTempPackDTO.TMPPackBatch.Size, txtSerial, txtBatch, objSolIneDTO))
                                            {                                                
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            objBatchDTO = FinalPackingBLL.ValidateSerialNumber(decimal.Parse(txtSerial.Text.Trim()), _objFinalPackingDTO.Size);
                                            objBatchDTO = FinalPackingBLL.GetBatchDetailsBySNo(decimal.Parse(txtSerial.Text.Trim()));
                                            //FinalPackingBLL.CBCIQuantityValidation\
                                            //objBatchDTO = FinalPackingBLL.GetSerialTotalPcsInformation(decimal.Parse(txtSerial.Text.Trim()));
                                            batchNumber = objBatchDTO.BatchNumber;
                                            txtBatch.Text = batchNumber;
                                            _isTempack = false;
                                            if (!CheckGloveTypeAndSize(objBatchDTO.GloveType, objBatchDTO.Size, txtSerial, txtBatch, objSolIneDTO))
                                            {
                                                return false;
                                            }

                                        }

                                        if (objBatchDTO.IsFPBatchSplit || objFPTempPackDTO.TMPPackBatch.IsFPBatchSplit)
                                        {
                                            if (ValidateSplitBatch(txtSerial, txtBatch))
                                            {
                                                if (!string.IsNullOrEmpty(batchNumber))
                                                {
                                                    txtBatch.Text = batchNumber;
                                                    ret = true;
                                                }
                                                else
                                                {
                                                    GlobalMessageBox.Show(Messages.BATCH_SPLIT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                    txtSerial.Text = String.Empty;
                                                    txtSerial.Focus();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(batchNumber))
                                            {
                                                txtBatch.Text = batchNumber;
                                                ret = true;
                                            }
                                            else
                                            {
                                                GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                txtSerial.Text = String.Empty;
                                                txtBatch.Text = String.Empty;
                                                txtSerial.Focus();
                                            }
                                        }
                                    }                                    
                                }
                                //else
                                //{
                                //    txtSerial.Text = string.Empty;
                                //    txtSerial.Focus();
                                //}
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.QI_TESTRESULT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtSerial.Text = string.Empty;
                                txtSerial.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(qaiResults, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtSerial.Clear();
                            txtSerial.Focus();
                        }
                    }

                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateSerialBatch", null);
            }
            return ret;
        }

        private bool GetSerialQty(TextBox txtSerial, TextBox txtBatch, TextBox txtQty, TextBox txtUse)
        {
            bool ret = false;
            int availPcs = 0;
            //BatchDTO objBatchDTO = new BatchDTO();
            try
            {
                //objBatchDTO = FinalPackingBLL.GetSerialTotalPcsInformation(decimal.Parse(txtSerial.Text.Trim()));
                //if ((int)objBatchDTO.PackPcs < (int)objBatchDTO.TotalPcs)
                //{
                //    availPcs = (int)objBatchDTO.TotalPcs - (int)objBatchDTO.PackPcs;
                //    txtQty.Text = availPcs.ToString();
                //    ret = true;
                //}
                //#AZRUL 7/4/2019: Get latest qty if batch previously done QC scan.
                availPcs = FinalPackingBLL.GetBatchCapacity(decimal.Parse(txtSerial.Text.Trim()));
                if (availPcs > 0)
                {
                    txtQty.Text = availPcs.ToString();
                    ret = true;
                }
                else
                {
                    GlobalMessageBox.Show("Batch pcs already consumed.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    txtSerial.Text = string.Empty;
                    txtBatch.Text = string.Empty;
                    txtSerial.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "GetSerialQty", null);
            }

            return ret;
        }

        private bool ConsumeQty(TextBox RequiredQty, TextBox RemainingQty, TextBox PackSize, TextBox thisSerial, TextBox AvailQty, TextBox UseQty, TextBox nextSerial = null)
        {
            bool ret = false;
            try
            {
                
                int requiredQty = Convert.ToInt32(RequiredQty.Text);
                int remainingQty = Convert.ToInt32(RemainingQty.Text);
                int availQty = Convert.ToInt32(AvailQty.Text);
                int packSize = Convert.ToInt32(PackSize.Text);
                //int useQty = Convert.ToInt32(UseQty.Text);

                int boxPack = Convert.ToInt32(Math.Floor(Convert.ToDouble(availQty / packSize)));
                int boxQty = boxPack * packSize;
                int boxReq = remainingQty / packSize;

                //int leftoverQty = boxQty - useQty;

                if (boxPack >= boxReq)
                {
                    UseQty.Text = (boxReq * packSize).ToString().Trim();
                    RemainingQty.Text = "0";
                }
                else if (boxPack < boxReq)
                {
                    UseQty.Text = (boxPack * packSize).ToString().Trim();
                    RemainingQty.Text = (remainingQty - (boxPack * packSize)).ToString().Trim();
                }

                if (RemainingQty.Text == "0")
                {
                    btnSave.Enabled = true;
                    btnSave.Focus();
                }
                else
                {
                    if (nextSerial != null)
                    {
                        thisSerial.ReadOnly = true;
                        thisSerial.Enabled = false;
                        nextSerial.ReadOnly = false;
                        nextSerial.Enabled = true;
                        nextSerial.BackColor = SystemColors.Window;
                        nextSerial.Focus();
                    }
                    else
                    {
                        GlobalMessageBox.Show("Exceed batch limit.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        thisSerial.Focus();
                    }
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ConsumeQty", null);
            }
            return ret;
        }

        private bool ConsumeCase(TextBox RequiredCase, TextBox RemainingCase, TextBox CaseSize, TextBox thisSerial, TextBox AvailQty, TextBox UseQty, TextBox BalQty, TextBox nextSerial = null)
        {
            bool ret = false;
            try
            {

                int requiredCase = Convert.ToInt32(RequiredCase.Text);
                int remainingCase = Convert.ToInt32(RemainingCase.Text);
                int serialAvailQty = Convert.ToInt32(AvailQty.Text);
                int CaseQtySize = Convert.ToInt32(CaseSize.Text);

                //int useQty = Convert.ToInt32(UseQty.Text);

                int CasePack = Convert.ToInt32(Math.Floor(Convert.ToDouble(serialAvailQty / CaseQtySize)));

                if (CasePack > 0)
                {
                    int CaseQty = CasePack * CaseQtySize;

                    if (CasePack == remainingCase)
                    {                   
                        int QtyUse = CasePack * CaseQtySize;
                        int QtyBal = serialAvailQty - QtyUse;

                        RemainingCase.Text = "0";
                        UseQty.Text = QtyUse.ToString();
                        BalQty.Text = QtyBal.ToString();
                    }
                    else if (CasePack > remainingCase)
                    {
                        int CaseUse = CasePack - (CasePack - remainingCase);
                        int QtyUse = CaseUse * CaseQtySize;
                        int QtyBal = serialAvailQty - QtyUse;
                        int RemCase = remainingCase - CaseUse;

                        RemainingCase.Text = RemCase.ToString();
                        UseQty.Text = QtyUse.ToString();
                        BalQty.Text = QtyBal.ToString();
                    }
                    else if (CasePack < remainingCase)
                    {
                        int QtyUse = CasePack * CaseQtySize;
                        int QtyBal = serialAvailQty - QtyUse;
                        int CaseUse = remainingCase - CasePack;
                        //int RemCase = remainingCase - CaseUse;

                        RemainingCase.Text = CaseUse.ToString();
                        UseQty.Text = QtyUse.ToString();
                        BalQty.Text = QtyBal.ToString();
                    }

                    if (RemainingCase.Text == "0") 
                    {
                        btnSave.Enabled = true;
                        btnSave.Focus();
                    }
                    else
                    {
                        if (nextSerial != null)
                        {
                            thisSerial.ReadOnly = true;
                            thisSerial.Enabled = false;
                            nextSerial.ReadOnly = false;
                            nextSerial.Enabled = true;
                            nextSerial.BackColor = SystemColors.Window;
                            nextSerial.Focus();
                        }
                        else
                        {
                            GlobalMessageBox.Show("Insufficient glove quantity.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            thisSerial.Focus();
                        }
                    }
                    ret = true;
                }
                else
                {
                    string strOut = "Minimum one case for each batch card are required. This BatchCard Qty: " + serialAvailQty + " Min Qty: " + CaseQtySize;
                    GlobalMessageBox.Show(strOut, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    thisSerial.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ConsumeCase", null);
            }
            return ret;
        }
        
        private bool ValidateQty(TextBox Qty, TextBox AvailQty, string Sender, bool seekNext = false, TextBox nextSerialtb = null)
        {
            bool ret = false;
            try
            {
                int qty;
                if (int.TryParse(Qty.Text, out qty))
                {
                    int reqQty = Convert.ToInt32(txtRemainingQty.Text);
                    int availQty = Convert.ToInt32(txtFirstQty.Text);
                    if (qty > reqQty)
                    {
                        GlobalMessageBox.Show("Exceed required qty. Please use maximum qty", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        Qty.Text = "";
                        Qty.Focus();
                    }
                    else if (qty > availQty)
                    {
                        GlobalMessageBox.Show("Exceed available qty.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        Qty.Text = "";
                        Qty.Focus();
                    }
                    else
                    {
                        int useQty = reqQty - qty;
                        txtRemainingQty.Text = useQty.ToString();
                        if (useQty == 0)
                        {
                            btnSave.Enabled = true;
                            btnSave.Focus();
                            Qty.ReadOnly = true;
                            Qty.Enabled = false;
                            Qty.BackColor = SystemColors.Control;
                        }
                        else
                        {
                            Qty.ReadOnly = true;
                            Qty.Enabled = false;
                            Qty.BackColor = SystemColors.Control;
                            if (seekNext)
                            {
                                nextSerialtb.Enabled = true;
                                nextSerialtb.ReadOnly = false;
                                nextSerialtb.BackColor = SystemColors.Window;
                                nextSerialtb.Focus();
                            }
                            else
                            {
                                GlobalMessageBox.Show("Exceed batch limit.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                Qty.Focus();
                            }
                        }
                    }
                }
                else
                {
                    GlobalMessageBox.Show("Not a valid qty.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    txtFirstSerialUseQty.Text = "";
                    txtFirstSerialUseQty.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, Sender, null);
            }
            return ret;
        }
        
        private bool checkDuplicate(TextBox tb, string txtSerial)
        {
            bool ret = true;

            if ((txtFirstSerialNo.Name != tb.Name) && (txtFirstSerialNo.Text == txtSerial))
                ret = false;
            if ((txtSecondSerialNo.Name != tb.Name) && (txtSecondSerialNo.Text == txtSerial))
                ret = false;
            if ((txtThirdSerialNo.Name != tb.Name) && (txtThirdSerialNo.Text == txtSerial))
                ret = false;

            return ret;
        }
        
        private void txtFirstSerialNo_Leave(object sender, EventArgs e)
        {
            if (!txtFirstSerialNo.ReadOnly)
            {
                if (ValidateSerialBatch(txtFirstSerialNo, txtFirstBatchInfo, txtFirstQty))
                {
                    if (GetSerialQty(txtFirstSerialNo, txtFirstBatchInfo, txtFirstQty, txtFirstSerialUseQty))
                    {
                        txtFirstSerialNo.ReadOnly = true;
                        txtFirstSerialNo.BackColor = SystemColors.Control;
                        if (!string.IsNullOrEmpty(txtFirstQty.Text))
                        {
                            if (!ConsumeCase(txtQty, txtRemainingQty, txtPackSize, txtFirstSerialNo, txtFirstQty, txtFirstSerialUseQty, txtFirstBal, txtSecondSerialNo))
                            {

                                txtFirstBal.Text = string.Empty;
                                txtFirstSerialNo.Text = string.Empty;
                                txtFirstBatchInfo.Text = string.Empty;
                                txtFirstSerialUseQty.Text = string.Empty;
                                txtFirstQty.Text = string.Empty;

                                txtFirstSerialNo.ReadOnly = false;
                                txtFirstSerialNo.BackColor = Color.Empty;

                                txtFirstSerialNo.Focus();
                            }
                            //ConsumeQty(txtQty, txtRemainingQty, txtPackSize, txtFirstSerialNo, txtFirstQty, txtFirstSerialUseQty, txtSecondSerialNo);
                        }
                    }
                }
            }
        }

        private void txtSecondSerialNo_Leave(object sender, EventArgs e)
        {
            if (!txtSecondSerialNo.ReadOnly)
            {
                if (checkDuplicate(txtSecondSerialNo, txtSecondSerialNo.Text))
                {
                    if (ValidateSerialBatch(txtSecondSerialNo, txtSecondBatchInfo, txtSecondQty))
                    {
                        if (GetSerialQty(txtSecondSerialNo, txtSecondBatchInfo, txtSecondQty, txtSecondSerialUseQty))
                        {
                            txtSecondSerialNo.ReadOnly = true;
                            txtSecondSerialNo.BackColor = SystemColors.Control;
                            if (!ConsumeCase(txtQty, txtRemainingQty, txtPackSize, txtSecondSerialNo, txtSecondQty, txtSecondSerialUseQty, txtSecondBal, txtThirdSerialNo))
                            {
                                txtSecondBal.Text = string.Empty;
                                txtSecondSerialNo.Text = string.Empty;
                                txtSecondBatchInfo.Text = string.Empty;
                                txtSecondSerialUseQty.Text = string.Empty;
                                txtSecondQty.Text = string.Empty;

                                txtSecondSerialNo.ReadOnly = false;
                                txtSecondSerialNo.BackColor = Color.Empty;

                                txtSecondSerialNo.Focus();
                            }
                            //ConsumeQty(txtQty, txtRemainingQty, txtPackSize, txtSecondSerialNo, txtSecondQty, txtSecondSerialUseQty, txtThirdSerialNo);
                        }
                    }
                }
                else
                {
                    if (txtSecondSerialNo.Text != string.Empty)
                    {
                        GlobalMessageBox.Show("Duplicate serial found. Please use different Batch", Constants.AlertType.Warning, "Change Batch Card Request", GlobalMessageBoxButtons.OK);
                        txtSecondSerialNo.Text = "";
                        txtSecondSerialNo.Focus();
                    }
                }
            }
        }

        private void txtThirdSerialNo_Leave(object sender, EventArgs e)
        {
            if (!txtThirdSerialNo.ReadOnly)
            {
                if (checkDuplicate(txtThirdSerialNo, txtThirdSerialNo.Text))
                {
                    if (ValidateSerialBatch(txtThirdSerialNo, txtThirdBatchInfo, txtThirdQty))
                    {
                        if (GetSerialQty(txtThirdSerialNo, txtThirdBatchInfo, txtThirdQty, txtThirdSerialUseQty))
                        {
                            txtThirdSerialNo.ReadOnly = true;
                            txtThirdSerialNo.BackColor = SystemColors.Control;
                            ConsumeCase(txtQty, txtRemainingQty, txtPackSize, txtThirdSerialNo, txtThirdQty, txtThirdSerialUseQty, txtThirdBal);
                            //ConsumeQty(txtQty, txtRemainingQty, txtPackSize, txtThirdSerialNo, txtThirdQty, txtThirdSerialUseQty);
                        }
                    }
                }
                else
                {
                    if (txtSecondSerialNo.Text != string.Empty)
                    {
                        GlobalMessageBox.Show("Duplicate serial found. Please use different Batch", Constants.AlertType.Warning, "Change Batch Card Request", GlobalMessageBoxButtons.OK);
                        txtThirdSerialNo.Text = "";
                        txtThirdSerialNo.Focus();
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtFirstSerialNo.Text = string.Empty;
            txtFirstQty.Text = string.Empty;
            txtFirstBatchInfo.Text = string.Empty;
            txtFirstSerialUseQty.Text = string.Empty;
            txtFirstBal.Text = string.Empty;

            txtSecondSerialNo.Text = string.Empty;
            txtSecondQty.Text = string.Empty;
            txtSecondBatchInfo.Text = string.Empty;
            txtSecondSerialUseQty.Text = string.Empty;
            txtSecondSerialNo.ReadOnly = true;
            txtSecondSerialNo.BackColor = SystemColors.Control;
            txtSecondBal.Text = string.Empty;

            txtThirdSerialNo.Text = string.Empty;
            txtThirdQty.Text = string.Empty;
            txtThirdBatchInfo.Text = string.Empty;
            txtThirdSerialUseQty.Text = string.Empty;
            txtThirdSerialNo.ReadOnly = true;
            txtThirdSerialNo.BackColor = SystemColors.Control;
            txtThirdBal.Text = string.Empty;

            btnReset.Enabled = false;
            btnSave.Enabled = false;

            txtFirstSerialNo.ReadOnly = false;
            txtFirstSerialNo.Enabled = true;
            txtFirstSerialNo.BackColor = Color.Empty;
            txtFirstSerialNo.Focus();
        }
        #endregion

        #region unused
        private void txtFirstSerialUseQty_Leave(object sender, EventArgs e)
        {
            //ValidateQty(txtFirstSerialUseQty, txtFirstQty, "txtFirstSerialUseQty_Leave", true, txtSecondSerialNo);
        }

        private void txtSecondSerialUseQty_Leave(object sender, EventArgs e)
        {
            //ValidateQty(txtSecondSerialUseQty, txtSecondSerialUseQty, "txtSecondSerialUseQty_Leave", true, txtThirdSerialNo);
        }

        private void txtThirdSerialUseQty_Leave(object sender, EventArgs e)
        {
            //ValidateQty(txtThirdSerialUseQty, txtThirdQty, "txtThirdSerialUseQty_Leave", false);
        }
        #endregion
    }
}
