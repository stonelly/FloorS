using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Linq;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System.Drawing;


namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class SurgicalPouchPrinting : FormBase
    {
        # region PRIVATE VARIABLES
        private int _nextNumber = 0;
        private string _screenname = "Surgical Pouch Printing"; //To use for DB logging.
        private string _uiClassName = "SurgicalPouchPrinting"; //To use for DB logging.
        List<SOLineDTO> lstPODetails = null;
        List<SOLineDTO> lstItemDetails = null;
        SOLineDTO objItemNumber = null;
        SOLineDTO objItemSize = null;
        private string _size = string.Empty; //To Capture the Size details.
        private string _customerSize = string.Empty; //To Capture the Customer Size details.
        private string _GloveCode = string.Empty;
        private string _AlternativeGloveCode1 = string.Empty;
        private string _AlternativeGloveCode2 = string.Empty;
        private string _AlternativeGloveCode3 = string.Empty;
        private int _manufacturingOrderbasis = 0;
        private int _expiry = 0;
        private DateTime? _receiptDateRequested = new DateTime();
        private DateTime? _shippingDateRequested = new DateTime();
        private DateTime? _ProductionDate = new DateTime();

        //Azman 2019-01-29 PODate & POReceived Date
        private DateTime? _custPODocumentDate = new DateTime();
        private DateTime? _custPORecvDate = new DateTime();
        //Azman 2019-01-29 PODate & POReceived Date
        //Pang 09/02/2020 First Carton Packing Date
        private DateTime _FirstManufacturingDate = new DateTime();

        private int _preshipmentPlan = 0; //To capture the Preshipment plan value for the selected Item.
        private int _itemcases = 0; //To capture the selected Item Cases.
        private string _poNumber = string.Empty; //To capture the selected POnumber.
        private string _itemNumber = string.Empty; //To capture the selected Item  Number.
        private int _casespacked = 0; //To capture the Cases packed per transaction.
        private int _casecapacity = 0; //To capture the outer Case Capacity.
        private int _palletcapacity = 0;//To capture the pallet Capacity.
        private int _innerBoxCapacity = 0; //To Capture the innerbox capacity.
        private string _innesetLayOut = string.Empty; //To capture the Inner Set Layout Number to print.
        private string _outersetLayOut = string.Empty; //To capture the Outer Set Layout Number to print.
        private int _BatchtotalPcs = 0; //To capture the selected Batch total pieces.

        private Boolean _isTempack = false;
        private string _operatorName = string.Empty;
        private string _customerLotNumber = string.Empty;
        private string _LineId = string.Empty;

        #endregion
        public SurgicalPouchPrinting()
        {
            InitializeComponent();
        }

        private void SurgicalPouchPrinting_Load(object sender, EventArgs e)
        {
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
            {
                txtstation.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
                txtInternalLotNumber.Text = GetInternalLotNumber();
            }
            else
                GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

            txtInnerPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
            GetActivePOList();
            BindPackingGroup();
            cmbPurchaseOrder.Focus();
            txtSerialNumber.SerialNo();
            txtSerialNumber2.SerialNo();
            this.WindowState = FormWindowState.Maximized;
        }

        private void BindPackingGroup()
        {
            List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups(Constants.PACKING_GROUP_TYPE);
            cmbGroupId.BindComboBox(lstDropDown, true);
        }


        /// <summary>
        /// GENERATE THE RUNNING INTENAL LOT NUMBER
        /// </summary>
        /// <returns></returns>
        private string GetInternalLotNumber()
        {
            string internalLotNumber = string.Empty;
            DateTime curretDate = CommonBLL.GetCurrentDateAndTimeFromServer();
            _nextNumber = FinalPackingBLL.GetWorkStationLastRunningNumber(Convert.ToString(WorkStationDataConfiguration.GetInstance().stationNumber));
            internalLotNumber = string.Format("{0}{1}{2}", curretDate.ToString(Messages.INTERNALLOTNUMBER_DATEFORMAT), Convert.ToString(WorkStationDataConfiguration.GetInstance().stationNumber).PadLeft(2, '0'), _nextNumber.ToString().PadLeft(4, '0'));
            return internalLotNumber;
        }

        /// <summary>
        /// GET ACTIVE PO LIST
        /// </summary>
        private void GetActivePOList()
        {
            try
            {
                cmbPurchaseOrder.SelectedIndexChanged -= cmbPurchaseOrder_SelectionChangeCommited;
                List<DropdownDTO> lstSurgicalPOList = FinalPackingBLL.GetSurgicalPOList().Distinct<DropdownDTO>().ToList();
                cmbPurchaseOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPurchaseOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbPurchaseOrder.BindComboBox(lstSurgicalPOList, true);
                cmbPurchaseOrder.SelectedIndexChanged += cmbPurchaseOrder_SelectionChangeCommited;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "GetActivePOList", null);
            }
        }
        private void cmbPurchaseOrder_Validated(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbPurchaseOrder.Text))
            {
                int isExists = -1;
                isExists = cmbPurchaseOrder.FindStringExact(cmbPurchaseOrder.Text.Trim());
                if (isExists == -1)
                {
                    cmbPurchaseOrder.Text = string.Empty;
                    cmbPurchaseOrder.Focus();
                }
            }
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.FINALPACKINGPRINTER)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.PRINTING_EXCEPTION + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        private void cmbPurchaseOrder_SelectionChangeCommited(object sender, EventArgs e)
        {
            lstItemDetails = new List<SOLineDTO>();
            try
            {
                lstPODetails = FinalPackingBLL.GetPurchaseOrderList(cmbPurchaseOrder.Text.FPCustomerRefernceSplit(), cmbPurchaseOrder.SelectedValue.ToString());
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbPurchaseOrder_SelectionChangeCommited", null);
            }
            cmbItemNumber.Items.Clear();
            cmbItemNumber.SelectedIndexChanged -= cmbItemNumber_SelectionChangeCommited;
            foreach (SOLineDTO soline in lstPODetails)
            {
                if (!cmbItemNumber.Items.Contains(soline.ItemNumber))
                {
                    lstItemDetails.Add(soline);
                    cmbItemNumber.Items.Add(soline.ItemNumber);
                }
            }
            cmbItemNumber.SelectedIndex = -1;
            cmbItemNumber.SelectedIndexChanged += cmbItemNumber_SelectionChangeCommited;
            txtInternalLotNumber.Text = GetInternalLotNumber();
        }

        private void cmbItemNumber_SelectionChangeCommited(object sender, EventArgs e)
        {
            try
            {
                string itemNumber = cmbItemNumber.Text;
                foreach (SOLineDTO objItemNum in lstItemDetails)
                {
                    if (objItemNum.ItemNumber == itemNumber)
                        objItemNumber = objItemNum;
                }
                List<SOLineDTO> listItemSize = lstPODetails.Where(SOLineDTO => SOLineDTO.ItemNumber == itemNumber).ToList();
                cmbItemSize.SelectedIndexChanged -= cmbItemSize_SelectedIndexChanged;
                cmbItemSize.DataSource = listItemSize;
                cmbItemSize.ValueMember = "ItemSize";
                cmbItemSize.DisplayMember = "ItemSize";
                cmbItemSize.SelectedIndex = -1;
                cmbItemSize.SelectedIndexChanged += cmbItemSize_SelectedIndexChanged;
                txtItemName.Text = objItemNumber.ItemName;


            }
            catch (Exception ex)
            {
                FloorSystemException fsexception = new FloorSystemException(ex.Message);
                ExceptionLogging(fsexception, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit", null);
            }
        }
        /// <summary>
        /// inserts soline details to purchaseorderitemtable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbItemSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbItemSize.SelectedIndex > Constants.MINUSONE)
                {
                    objItemSize = (SOLineDTO)cmbItemSize.SelectedItem;
                    _size = objItemSize.ItemSize;
                    _customerSize = objItemSize.CustomerSize;
                    _GloveCode = objItemSize.GloveCode;
                    _AlternativeGloveCode1 = objItemSize.AlternateGloveCode1;
                    _AlternativeGloveCode2 = objItemSize.AlternateGloveCode2;
                    _AlternativeGloveCode3 = objItemSize.AlternateGloveCode3;
                    _manufacturingOrderbasis = objItemSize.ManufacturingDateBasis;
                    _receiptDateRequested = objItemSize.RECEIPTDATEREQUESTED;
                    _shippingDateRequested = objItemSize.SHIPPINGDATEREQUESTED;

                    //Azman 2019-01-29 PODate & POReceived Date
                    _custPODocumentDate = objItemSize.CustPODocumentDate;
                    _custPORecvDate = objItemSize.CustPORecvDate;
                    //Azman 2019-01-29 PODate & POReceived Date
                    //Pang 09/02/2021 First Carton Packing Date
                    _FirstManufacturingDate = FinalPackingBLL.GetFirstCartonPackingDate(objItemSize.PONumber, objItemSize.ItemNumber, _size);

                    _expiry = objItemNumber.Expiry;
                    _customerLotNumber = objItemSize.CustomerLotNumber;
                    UpdatePODetails(objItemSize);
                }
            }
            catch (Exception ex)
            {
                FloorSystemException fsexception = new FloorSystemException(ex.Message);
                ExceptionLogging(fsexception, _screenname, _uiClassName, "cmbItemSize_SelectedIndexChanged", null);
            }
        }
        /// <summary>
        /// CANCEL TO CLEAR THE FORM FIELDS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                cmbPurchaseOrder.Focus();
            }
        }

        /// <summary>
        /// CLEAR THE FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            try
            {
                try
                {
                    txtBatch.Text = String.Empty;
                    txtBoxesPacked.Text = String.Empty;
                    txtBatch2.Text = String.Empty;
                    txtSerialNumber.Text = string.Empty;
                    txtSerialNumber2.Text = String.Empty;
                    cmbGroupId.SelectedIndex = Constants.MINUSONE;
                    txtItemName.Text = String.Empty;
                    cmbPurchaseOrder.Text = string.Empty;
                    cmbPurchaseOrder.DataSource = null;
                    cmbItemNumber.Items.Clear();
                    cmbItemSize.Text = string.Empty;
                    cmbItemSize.DataSource = null;
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }

                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                GetActivePOList();
                BindPackingGroup();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
        }
        /// <summary>
        /// Update Soline data to Purchaseorderitem table
        /// </summary>
        /// <param name="objPODetails"></param>
        private void UpdatePODetails(SOLineDTO objPODetails)
        {
            if (objPODetails != null)
            {
                _size = objPODetails.ItemSize;
                _customerSize = objPODetails.CustomerSize;
                objPODetails.ItemSize = _size;
                _preshipmentPlan = objPODetails.PreshipmentPlan;
                _itemcases = objPODetails.ItemCases;
                _poNumber = objPODetails.PONumber;
                _itemNumber = objPODetails.ItemNumber;
                _casecapacity = objPODetails.CaseCapacity;
                _palletcapacity = objPODetails.PalletCapacity;
                _innerBoxCapacity = objPODetails.InnerBoxCapacity;
                _innesetLayOut = objPODetails.InnersetLayout;
                _outersetLayOut = objPODetails.OuterSetLayout;
            }
            objPODetails.locationID = WorkStationDTO.GetInstance().LocationId;
            FinalPackingBLL.InsertPurchaseOrderDetails(objPODetails);//Insert Purchase Order details.
            InsertCaseNumbers();
        }
        /// <summary>
        /// INSERT OUTER CASE NUMBERS ALONG WITH PRESHIPMENT FLAG FOR THE PO ITEM
        /// </summary>
        private void InsertCaseNumbers()
        {
            try
            {
                FinalPackingBLL.InsertCaseNumbers(_itemcases, _poNumber, _itemNumber, _size, string.Empty, WorkStationDTO.GetInstance().WorkStationId, WorkStationDTO.GetInstance().LocationId, objItemSize.InventTransId, _customerSize);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "InsertCaseNumbers", null);
            }
        }
        private void txtSerialNumber_Leave(object sender, EventArgs e)
        {
            try
            {
                string[] surgicalCommonSize = _size.Split(Constants.CHARCOMMA);
                string batchNumber = string.Empty;
                if (ValidatePORequiredFields()) //Validate Required fields serial number to scan
                {
                    if (surgicalCommonSize.Count() >= Constants.TWO)
                    {
                        string strLeftsize = surgicalCommonSize[Constants.ONE];

                        string strRightSize = surgicalCommonSize[Constants.ZERO];
                        //string strLeftsize = string.Empty;
                        foreach (string commonsize in surgicalCommonSize)
                        {
                            if (commonsize.ToUpper().Contains(Constants.charLeft))
                            {
                                strLeftsize = commonsize;
                            }
                        }

                        if (!string.IsNullOrEmpty(strLeftsize))
                            if (!string.IsNullOrEmpty(txtSerialNumber.Text.Trim()))
                            {
                                if (!HourlyBatchCardBLL.IsSerialNumber(Convert.ToDecimal(txtSerialNumber.Text.Trim())))
                                {
                                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtSerialNumber.Text = string.Empty;
                                    txtSerialNumber.Focus();
                                }
                                else if (FinalPackingBLL.ValidateHotbox(txtSerialNumber.Text.Trim()) > Constants.ZERO)
                                {
                                    int i = FinalPackingBLL.ValidateHotbox(txtSerialNumber.Text.Trim());

                                    if (i == Constants.ONE)
                                        GlobalMessageBox.Show(Messages.HOTBOX_FAILMSG, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else if (i == Constants.TWO)
                                        GlobalMessageBox.Show(Messages.HOTBOX_PENDINGRESULT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else if (i == Constants.THREE)
                                        GlobalMessageBox.Show(Messages.HOTBOX_RESULTEXPIRED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else if (i == Constants.FOUR)
                                        GlobalMessageBox.Show(Messages.HOTBOX_OLDBATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);

                                    txtSerialNumber.Text = string.Empty;
                                    txtSerialNumber.Focus();
                                }
                                else if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerialNumber.Text.Trim())))
                                {
                                    GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtSerialNumber.Text = string.Empty;
                                    txtSerialNumber.Focus();
                                }
                                else
                                {
                                    batchNumber = SerilaNumberValidation(Convert.ToDecimal(txtSerialNumber.Text), strLeftsize);
                                    txtBatch.Text = batchNumber;
                                    if (batchNumber == String.Empty)
                                    {
                                        txtSerialNumber.Text = String.Empty;
                                        txtSerialNumber.Focus();
                                    }
                                }
                            }
                    }
                }
                else
                {
                    txtSerialNumber.Text = string.Empty;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtSerialNumber_Leave", null);
            }
        }
        private Boolean ValidatePORequiredFields()
        {

            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemSize, Messages.REQITEMSIZE, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            return ValidateForm();

        }
        /// <summary>
        /// VALIDATE SERIAL NUMBER ENTERED 
        /// </summary>
        private string SerilaNumberValidation(decimal serialNumber, string size)
        {
            BatchDTO objBatchDTO = new BatchDTO();
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
            string batchNumber = string.Empty;
            Boolean blnIsValid = false;
            try
            {
                string qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber); //validate QAI status
                if (string.IsNullOrEmpty(qaiResults))
                {
                    if (FinalPackingBLL.ValidateSerialNoByQIStatus(serialNumber) == Constants.PASS)
                    {
                        if (ValidateQATestResult(serialNumber, Constants.POLYMER_TEST)
                                    && ValidateQATestResult(serialNumber, Constants.POWDER_TEST)
                                    && ValidateQATestResult(serialNumber, Constants.PROTEIN_TEST)
                                    && ValidateQATestResult(serialNumber, Constants.HOTBOX_TEST))
                        {
                            string strlocation = null;
                            if (!FinalPackingBLL.isBatchScanOut(serialNumber, out strlocation))
                            {
                                GlobalMessageBox.Show(String.Format(Messages.NO_SCAN_OUT_SNO_FP, strlocation), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            }
                            else
                            {
                                objFPTempPackDTO = FinalPackingBLL.CheckIsTemPackBatch(serialNumber);//Check if scanned batch is from TempPack
                                if (objFPTempPackDTO.isTempPackBatch)
                                {
                                    objBatchDTO = FinalPackingBLL.ValidateSerialNumberGloveCode(serialNumber, _GloveCode, size, _AlternativeGloveCode1, _AlternativeGloveCode2, _AlternativeGloveCode3, _innerBoxCapacity);
                                    batchNumber = objBatchDTO.BatchNumber;
                                    _ProductionDate = objBatchDTO.BatchCarddate;
                                    _isTempack = true;
                                    _LineId = objBatchDTO.Line;
                                }
                                else
                                {
                                    objBatchDTO = FinalPackingBLL.ValidateSerialNumberGloveCode(serialNumber, _GloveCode, size, _AlternativeGloveCode1, _AlternativeGloveCode2, _AlternativeGloveCode3, _innerBoxCapacity);
                                    batchNumber = objBatchDTO.BatchNumber;
                                    _ProductionDate = objBatchDTO.BatchCarddate;
                                    _isTempack = false;
                                    _LineId = objBatchDTO.Line;
                                }

                                if (objBatchDTO.IsFPBatchSplit || objFPTempPackDTO.TMPPackBatch.IsFPBatchSplit)
                                {
                                    if (ValidateSplitBatch())
                                    {
                                        if (!string.IsNullOrEmpty(batchNumber))
                                            return batchNumber;
                                        else
                                        {
                                            GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                            return batchNumber = String.Empty;
                                        }
                                    }
                                    else
                                    {
                                        return string.Empty;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(batchNumber))
                                        return batchNumber;
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                        return batchNumber = String.Empty;
                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    blnIsValid = false;
                        //}
                    }
                    else if (FinalPackingBLL.ValidateSerialNoByQIStatus(serialNumber) == Constants.FAIL1)
                    {
                        string QCDesc = FinalPackingBLL.GetQCTypeBySerial(serialNumber);
                        GlobalMessageBox.Show("This batch is " + QCDesc + ". Please check with your superior.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        //GlobalMessageBox.Show(Messages.NEEDQCREWORK, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtSerialNumber.Text = string.Empty;
                        txtSerialNumber.Focus();
                    }
                    else
                    {
                        //GlobalMessageBox.Show(Messages.QI_TESTRESULT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        GlobalMessageBox.Show("QI not passed. Please return for QI process.", Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        blnIsValid = false;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(qaiResults, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    blnIsValid = false;
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
            return batchNumber;
        }

        /// <summary>
        /// VALIDATE SELECTED BATCH IS SPLIT BATCH OR NOT
        /// </summary>
        /// <returns></returns>
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
                    return false;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateSplitBatch", null);
                return false;
            }
        }

        private void txtSerialNumber2_Leave(object sender, EventArgs e)
        {
            try
            {
                string[] surgicalCommonSize = _size.Split(Constants.CHARCOMMA);
                string batchNumber = string.Empty;
                if (ValidatePORequiredFields()) //Validate Required fields serial number to scan
                {
                    if (surgicalCommonSize.Count() >= Constants.TWO)
                    {
                        string strRightSize = string.Empty;
                        foreach (string commonsize in surgicalCommonSize)
                        {
                            if (commonsize.ToUpper().Contains(Constants.charRight))
                            {
                                strRightSize = commonsize;
                            }
                        }
                        if (!string.IsNullOrEmpty(strRightSize))
                            if (!string.IsNullOrEmpty(txtSerialNumber2.Text.Trim()))
                            {
                                if (!HourlyBatchCardBLL.IsSerialNumber(Convert.ToDecimal(txtSerialNumber2.Text.Trim())))
                                {
                                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtSerialNumber2.Text = string.Empty;
                                    txtSerialNumber2.Focus();
                                }
                                else if (FinalPackingBLL.ValidateHotbox(txtSerialNumber2.Text.Trim()) > Constants.ZERO)
                                {
                                    int i = FinalPackingBLL.ValidateHotbox(txtSerialNumber2.Text.Trim());

                                    if (i == Constants.ONE)
                                        GlobalMessageBox.Show(Messages.HOTBOX_FAILMSG, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else if (i == Constants.TWO)
                                        GlobalMessageBox.Show(Messages.HOTBOX_PENDINGRESULT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else if (i == Constants.THREE)
                                        GlobalMessageBox.Show(Messages.HOTBOX_RESULTEXPIRED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    else if (i == Constants.FOUR)
                                        GlobalMessageBox.Show(Messages.HOTBOX_OLDBATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);

                                    txtSerialNumber2.Text = string.Empty;
                                    txtSerialNumber2.Focus();
                                }
                                else if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerialNumber2.Text.Trim())))
                                {
                                    GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtSerialNumber2.Text = string.Empty;
                                    txtSerialNumber2.Focus();
                                }
                                else
                                {
                                    batchNumber = SerilaNumberValidation(Convert.ToDecimal(txtSerialNumber2.Text), strRightSize);
                                    txtBatch2.Text = batchNumber;
                                    if (batchNumber == String.Empty)
                                    {
                                        txtSerialNumber2.Text = String.Empty;
                                        txtSerialNumber2.Focus();
                                    }
                                }

                            }
                    }
                    else
                    {
                        txtSerialNumber2.Text = string.Empty;
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtSerialNumber2_Leave", null);
            }
        }

        /// <summary>
        /// VALIDATE BOXES PACKED
        /// </summary>
        private Boolean ValidateBoxesPacked()
        {
            int POItemSizecasespacked = Constants.ZERO;
            int boxesentered = Constants.ZERO;
            Boolean blnIsValid = false;
            try
            {
                //boxes packed Null or empty validation
                if (!string.IsNullOrEmpty(txtBoxesPacked.Text))
                {
                    if (!string.IsNullOrEmpty(txtSerialNumber.Text.Trim()))
                    {
                        //Validate the boxes packed text is integer.
                        if (Validator.IsValidInput(Framework.Constants.ValidationType.Integer, txtBoxesPacked.Text))
                        {
                            if (Convert.ToInt64(txtBoxesPacked.Text) < _casecapacity)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.BOXES_LESSTHANCASECAPACITY, _casecapacity), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                txtBoxesPacked.Clear();
                                txtBoxesPacked.Focus();
                            }
                            else
                            {

                                //Round the entered boxes to case capacity and display in text box.
                                txtBoxesPacked.Text = FinalPackingBLL.RoundToCaseCapacity(txtBoxesPacked.Text, _casecapacity);
                                boxesentered = int.Parse((txtBoxesPacked.Text.Trim()));
                                //cases already packed for the given PO,ITEM and SIZE.
                                POItemSizecasespacked = FinalPackingBLL.ValidatePOSizeBoxesPackedForSurgicalPouch(_poNumber, _itemNumber, _size);
                                //validate boxes entered and boxes already packed is not greater than the requied 
                                if (!ValidateRequiredItemCases(POItemSizecasespacked, boxesentered))
                                {
                                    GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((_itemcases - POItemSizecasespacked) * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                                    txtBoxesPacked.Text = Convert.ToString((_itemcases - POItemSizecasespacked) * _casecapacity);
                                    txtBoxesPacked.Focus();
                                }
                                else if (!ValidateBatchCapacity(Convert.ToDecimal(txtSerialNumber.Text), (boxesentered)))//validate selected batch capacity
                                {
                                    GlobalMessageBox.Show(string.Format(Messages.BATCHCAPACITY, _BatchtotalPcs / (_innerBoxCapacity / Constants.TWO), (_innerBoxCapacity / Constants.TWO), Environment.NewLine), Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    txtBoxesPacked.Text = Convert.ToString(_BatchtotalPcs / (_innerBoxCapacity / Constants.TWO));
                                    txtBoxesPacked.Focus();
                                }
                                else if (!ValidateBatchCapacity(Convert.ToDecimal(txtSerialNumber2.Text), (boxesentered)))//validate selected batch capacity
                                {
                                    GlobalMessageBox.Show(string.Format(Messages.BATCHCAPACITY, _BatchtotalPcs / (_innerBoxCapacity / Constants.TWO), (_innerBoxCapacity / Constants.TWO), Environment.NewLine), Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    txtBoxesPacked.Text = Convert.ToString(_BatchtotalPcs / (_innerBoxCapacity / Constants.TWO));
                                    txtBoxesPacked.Focus();
                                }
                                else
                                { blnIsValid = true; } // return true if no validation failed
                            }
                        }
                        else
                        {
                            txtBoxesPacked.Text = string.Empty;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.REQSERIALNUMBER, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                        txtBoxesPacked.Clear();
                        txtSerialNumber.Focus();
                    }
                }

            }
            catch (FormatException ex)
            {
                FloorSystemException fsexp = new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
            return blnIsValid;
        }
        /// <summary>
        /// TO VALIDATE REQUIRED ITEM CASES
        /// </summary>
        /// <param name="POItemSizecasespacked">EXISTING PO ITEM CASES</param>
        /// <param name="boxesentered">BOXES ENTERED IN TEXTBOX</param>
        /// <returns></returns>
        private Boolean ValidateRequiredItemCases(int POItemSizecasespacked, int boxesentered)
        {
            if ((POItemSizecasespacked * _casecapacity) + (boxesentered) > (_itemcases * _casecapacity))
                return false;
            else
                return true;
        }

        /// <summary>
        /// TO VALIDATE BATCH CAPACITY
        /// </summary>
        /// <param name="serialNo">SERIAL NUMBER SCANNED</param>
        /// <param name="boxespacked">BOXES ENTERED IN TEXTBOX</param>
        /// <returns>returns true if valid</returns>
        private Boolean ValidateBatchCapacity(decimal serialNo, int boxespacked)
        {
            try
            {
                _BatchtotalPcs = FinalPackingBLL.GetBatchCapacity(serialNo);
                //compare total gloves  and total Batch Pcs
                if (boxespacked * (_innerBoxCapacity / Constants.TWO) <= _BatchtotalPcs)
                    return true;
                else
                    return false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
                return false;
            }
        }

        private void txtBoxesPacked_Leave(object sender, EventArgs e)
        {
            ValidateBoxesPacked();
        }

        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, Messages.REQSERIALNUMBER1, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNumber2, Messages.REQSERIALNUMBER2, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBoxesPacked, Messages.REQBOXESPACKED, ValidationType.Required));
            return ValidateForm();
        }
        private void txtPrint_Click(object sender, EventArgs e)
        {
            if (ValidateRequiredFields())
            {
                FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
                List<FinalPackingBatchInfoDTO> lstFPBatchInfoDTO = new List<FinalPackingBatchInfoDTO>();
                FinalPackingBatchInfoDTO objFPSerialNumberInfoDTO1 = new FinalPackingBatchInfoDTO();
                FinalPackingBatchInfoDTO objFPSerialNumberInfoDTO2 = new FinalPackingBatchInfoDTO();

                objFinalPackingDTO.locationId = WorkStationDTO.GetInstance().LocationId;
                objFinalPackingDTO.Workstationnumber = WorkStationDTO.GetInstance().WorkStationId;
                objFinalPackingDTO.Printername = WorkStationDataConfiguration.GetInstance().innerPrinter;
                objFinalPackingDTO.Packdate = CommonBLL.GetCurrentDateAndTimeFromServer();
                objFinalPackingDTO.Internallotnumber = txtInternalLotNumber.Text.Trim();
                objFinalPackingDTO.Outerlotno = txtInternalLotNumber.Text.Trim();
                objFinalPackingDTO.Ponumber = _poNumber;
                objFinalPackingDTO.ItemNumber = _itemNumber;
                objFinalPackingDTO.Size = _size;
                objFinalPackingDTO.Boxespacked = Convert.ToInt32(txtBoxesPacked.Text);
                objFinalPackingDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
                objFinalPackingDTO.Innersetlayout = _innesetLayOut;
                objFinalPackingDTO.Outersetlayout = _outersetLayOut;
                objFinalPackingDTO.palletCapacity = _palletcapacity;
                objFinalPackingDTO.isTempPack = _isTempack;
                objFinalPackingDTO.ManufacturingDate = FinalPackingBLL.GetManufacturingDate(_manufacturingOrderbasis, _shippingDateRequested, _receiptDateRequested, _ProductionDate, _custPODocumentDate, _custPORecvDate, _FirstManufacturingDate);
                objFinalPackingDTO.ExpiryDate = objFinalPackingDTO.ManufacturingDate.Value.AddMonths(_expiry);
                objFinalPackingDTO.InventTransId = objItemSize.InventTransId;

                objFPSerialNumberInfoDTO1.SerialNumber = decimal.Parse(txtSerialNumber.Text.Trim());
                objFPSerialNumberInfoDTO1.BoxesPacked = objFinalPackingDTO.Boxespacked;
                objFPSerialNumberInfoDTO1.CasesPacked = objFPSerialNumberInfoDTO1.BoxesPacked / _casecapacity;
                objFPSerialNumberInfoDTO1.TotalPcs = objFinalPackingDTO.Boxespacked * (_innerBoxCapacity / Constants.TWO);

                objFPSerialNumberInfoDTO2.SerialNumber = decimal.Parse(txtSerialNumber2.Text.Trim());
                objFPSerialNumberInfoDTO2.BoxesPacked = objFinalPackingDTO.Boxespacked;
                objFPSerialNumberInfoDTO2.CasesPacked = objFPSerialNumberInfoDTO2.BoxesPacked / _casecapacity;
                objFPSerialNumberInfoDTO2.TotalPcs = objFinalPackingDTO.Boxespacked * (_innerBoxCapacity / Constants.TWO);
                objFinalPackingDTO.stationNumber = WorkStationDataConfiguration.GetInstance().stationNumber;
                objFinalPackingDTO.Casespacked = objFinalPackingDTO.Boxespacked / _casecapacity;

                objFinalPackingDTO.FPStationNo = txtstation.Text;

                lstFPBatchInfoDTO.Add(objFPSerialNumberInfoDTO1);
                lstFPBatchInfoDTO.Add(objFPSerialNumberInfoDTO2);
                try
                {
                    int rowsreturned = FinalPackingBLL.InsertFinalPackingSurgical(objFinalPackingDTO, FinalPackingBLL.CreateXML(lstFPBatchInfoDTO));
                    if (rowsreturned > 0)
                    {
                        string specialLotNumber = FinalPackingBLL.UpdatespeciallotnumberforInner(objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.ManufacturingDate.Value, objFinalPackingDTO.ExpiryDate.Value,
                                           _LineId, _expiry, _innesetLayOut, _customerLotNumber);

                        FinalPackingBLL.UpdatespeciallotnumberforOuter(objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.ManufacturingDate.Value, objFinalPackingDTO.ExpiryDate.Value,
                             _LineId, _expiry, _outersetLayOut, _customerLotNumber, _shippingDateRequested.Value, specialLotNumber, _innesetLayOut);

                        if (!string.IsNullOrEmpty(specialLotNumber))
                        {
                            objFinalPackingDTO.Internallotnumber = specialLotNumber;
                        }
                        // Pang 09/02/2020 First Carton Packing Date
                        FinalPackingBLL.UpdateFirstCartonPackingDate(objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);

                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        try
                        {
                            FinalPackingPrint.InnerLabelPrint(objFinalPackingDTO.Internallotnumber);
                            //eventlog myadamas 20190228
                            EventLogDTO EventLog = new EventLogDTO();
                            EventLog.CreatedBy = String.Empty;
                            Constants.EventLog evtAction = Constants.EventLog.Print;
                            EventLog.EventType = Convert.ToInt32(evtAction);
                            EventLog.EventLogType = Constants.eventlogtype;

                            var screenid = CommonBLL.GetScreenIdByScreenName(_screenname);
                            CommonBLL.InsertEventLog(EventLog, _screenname, screenid.ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new FloorSystemException("Exception Occured while printing inner", Constants.FINALPACKINGPRINTER, ex, true);
                        }
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenname, _uiClassName, "txtPrint_Click", null);
                }
                ClearForm();
                txtInternalLotNumber.Text = GetInternalLotNumber();
            }
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


    }
}
