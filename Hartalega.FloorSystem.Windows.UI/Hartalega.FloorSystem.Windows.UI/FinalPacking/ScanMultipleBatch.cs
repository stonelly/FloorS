using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.IntegrationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Transactions;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ScanMultipleBatch : FormBase
    {
        # region PRIVATE VARIABLES

        private int _preshipmentPlan = 0; //To capture the Preshipment plan value for the selected Item.
        private int _itemcases = 0; //To capture the selected Item Cases.
        private string _itemNumber = string.Empty; //To capture the selected Item  Number.
        private int _casespacked = 0; //To capture the Cases packed per transaction.
        private int _casecapacity = 0; //To capture the outer Case Capacity.
        private int _palletcapacity = 0;//To capture the pallet Capacity.
        private int _innerBoxCapacity = 0; //To Capture the innerbox capacity.
        private string _innesetLayOut = string.Empty; //To capture the Inner Set Layout Number to print.
        private string _outersetLayOut = string.Empty; //To capture the Outer Set Layout Number to print.
        private int _BatchtotalPcs = 0; //To capture the selected Batch total pieces.
        private string _batchNo = string.Empty;
        private int _totalBoxesPacked = 0;
        private int _nextNumber = 0;
        private string _purchaseorderFormNum = string.Empty;

        private string _poNumber = string.Empty; //To capture the selected POnumber.
        private string _preshipmentRandomNumbers = string.Empty; //To capture the preshipment Random numbers generated for the selected Po Iem.
        private string _childpalletID = string.Empty; //To capture the palletID from new pallet window when pallet capacity has reached.        
        private string _size = string.Empty; //To Capture the Size details.
        private string _CustomerSize = string.Empty; //To Capture the Size details.
        private string _operatorName = String.Empty; //to capture the operator name of the split batch.
        private string _screenname = "Scan Multiple Batch Packing"; //To use for DB logging.
        private string _uiClassName = "Scan Multiple Batch Packing"; //To use for DB logging.

        private string _GloveCode = string.Empty;
        private string _AlternativeGloveCode1 = string.Empty;
        private string _AlternativeGloveCode2 = string.Empty;
        private string _AlternativeGloveCode3 = string.Empty;
        private int _manufacturingOrderbasis = 0;
        private int _expiry = 0;
        private int _GCLabelPrintingRequired = Constants.ZERO;
        private string _customerLotNumber = string.Empty;
        private int _BarcodeVerificationRequired = Constants.ZERO;

        private DateTime? _receiptDateRequested = new DateTime();
        private DateTime? _shippingDateRequested = new DateTime();
        private DateTime? _ProductionDate = new DateTime();

        //Azman 2019-01-29 PODate & POReceived Date
        private DateTime? _custPODocumentDate = new DateTime();
        //Azman 2019-01-29 PODate & POReceived Date
        //Pang 09/02/2020 First Carton Packing Date
        private DateTime _FirstManufacturingDate = new DateTime();

        private Boolean _isTempack = false; //To check is the selected batch is from Temppack inventory. 
        private string _LineId = string.Empty;
        private Boolean _PSIRequired = false;
        private Boolean _isAssociated = false;
        DataTable dtPODetails = new DataTable(); // 

        //List<SOLineDTO> lstItemNumber = null;
        List<SOLineDTO> lstPODetails = null;
        List<SOLineDTO> lstItemDetails = null;
        SOLineDTO objItemNumber = null;
        SOLineDTO objItemSize = null;

        List<FinalPackingBatchInfoDTO> _lstFinalPackingInfo = new List<FinalPackingBatchInfoDTO>();
        List<FinalPackingBatchInfoDTO> _lt = new List<FinalPackingBatchInfoDTO>();

        private Dictionary<string, string> _dictSerialNumber = new Dictionary<string, string>();

        //KahHeng 26APr2019 add flag to identify 1st pallet is continued from previous same pallet
        private Boolean _isContinuedPallet = false;
        private Boolean _isContinuedPreshipmentPallet = false;
        private DialogResult result;
        //Kahheng end
        #endregion


        #region CONSTRUCTOR
        public ScanMultipleBatch()
        {
            InitializeComponent();
        }
        # endregion

        #region FORM LOAD
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanMultipleBatch_Load(object sender, EventArgs e)
        {
            try
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
                txtInternalLotNumber.Text = GetInternalLotNumber();
                GetActivePOList();
                BindPackingGroup();
                cmbPurchaseOrder.Focus();
                txtSerialNumber.SerialNo();
                txtBoxesPacked.BoxesPacked();
                this.WindowState = FormWindowState.Maximized;
                cmbPreShipPltId.Enabled = false;

                //#MH 26/06/2018 Hide Group Id info
                lblGroupId.Visible = false;
                cmbGroupId.Visible = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ScanMultipleBatch_Load", null);
            }
        }

        private void BindPackingGroup()
        {
            List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups(Constants.PACKING_GROUP_TYPE);
            cmbGroupId.BindComboBox(lstDropDown, true);
        }
        #endregion

        #region EVENTS

        void cmbItemSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbItemSize.SelectedIndex > Constants.MINUSONE)
                {
                    objItemSize = (SOLineDTO)cmbItemSize.SelectedItem;
                    _size = objItemSize.ItemSize;
                    _CustomerSize = objItemSize.CustomerSize;
                    _GloveCode = objItemSize.GloveCode;
                    _AlternativeGloveCode1 = objItemSize.AlternateGloveCode1;
                    _AlternativeGloveCode2 = objItemSize.AlternateGloveCode2;
                    _AlternativeGloveCode3 = objItemSize.AlternateGloveCode3;
                    _manufacturingOrderbasis = objItemSize.ManufacturingDateBasis;
                    _receiptDateRequested = objItemSize.RECEIPTDATEREQUESTED;
                    _shippingDateRequested = objItemSize.SHIPPINGDATEREQUESTED;

                    //Azman 2019-01-29 PODate & POReceived Date
                    _custPODocumentDate = objItemSize.CustPODocumentDate;
                    //Azman 2019-01-29 PODate & POReceived Date
                    //Pang 09/02/2021 First Carton Packing Date
                    _FirstManufacturingDate = FinalPackingBLL.GetFirstCartonPackingDate(objItemSize.PONumber, objItemSize.ItemNumber, _size);

                    _expiry = objItemNumber.Expiry;
                    _GCLabelPrintingRequired = objItemSize.GCLabelPrintingRequired;
                    _customerLotNumber = objItemSize.CustomerLotNumber;
                    _BarcodeVerificationRequired = objItemSize.BarcodeVerificationRequired;
                    _purchaseorderFormNum = objItemSize.OrderNumber;
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
        /// Update PO Details with SOline data
        /// </summary>
        /// <param name="objPODetails"></param>
        private void UpdatePODetails(SOLineDTO objPODetails)
        {
            try
            {
                if (objPODetails != null)
                {
                    _size = objPODetails.ItemSize;
                    _CustomerSize = objPODetails.CustomerSize;
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
                int preshipmentQty = FinalPackingBLL.GetPreshipmentCaseQty(_itemcases, _preshipmentPlan); //Get preshipment qunatity from Preshiment sampling plan.
                _preshipmentRandomNumbers = GenerateRandomNumbers(preshipmentQty, _itemcases); //Generate Random numbers
                if (!string.IsNullOrEmpty(_preshipmentRandomNumbers))
                    objPODetails.Preshipmentcases = _preshipmentRandomNumbers;
                objPODetails.locationID = WorkStationDTO.GetInstance().LocationId;
                FinalPackingBLL.InsertPurchaseOrderDetails(objPODetails);//Insert Purchase Order details.
                InsertCaseNumbers(); //Insert the Item CaseNumbers for the selected Item.              

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "UpdatePODetails", null);
            }
        }


        /// <summary>
        /// IDENTIFY ASSOCIATED PALLET ID AND POPULATE PALLET LIST
        /// </summary>
        private Boolean PopulateAssociatedPalletID(bool isfromPrinting = false)
        {
            string associatedPalletId = string.Empty;
            string associatedPreshipmentPLTID = string.Empty;
            try
            {
                int caseNo = 0;
                if (!String.IsNullOrEmpty(txtTotalBoxesPacked.Text))
                    caseNo = Convert.ToInt32(txtTotalBoxesPacked.Text);

                _PSIRequired = ValidatePreshipementPalletCase(_poNumber, _itemNumber, caseNo / _casecapacity, _size);

                if (isfromPrinting)
                {
                    if (FinalPackingBLL.validatePalletPOItemSize(_poNumber, _itemNumber, _size, cmbPalletId.Text) == Constants.ZERO)
                        return false;

                    if (FinalPackingBLL.validatePreshipmentPalletPO(_poNumber, cmbPreShipPltId.Text) == Constants.ZERO)
                        return false;
                }
                if (isfromPrinting == false && _isAssociated == false)
                {
                    associatedPalletId = GetItemSizePalletId(_poNumber, _itemNumber, _size);
                    associatedPreshipmentPLTID = GetItemPreshipmentPalletId(_poNumber, _itemNumber);
                    if (!string.IsNullOrEmpty(associatedPalletId))
                    {
                        if (GlobalMessageBox.Show(string.Format(Messages.ASSOCIATEDPALLETID, associatedPalletId), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                        {
                            associatedPalletId = string.Empty;
                        }
                    }
                    if (!_PSIRequired)
                    {
                        associatedPreshipmentPLTID = string.Empty;

                    }
                    if (!string.IsNullOrEmpty(associatedPreshipmentPLTID))
                    {
                        if (GlobalMessageBox.Show(string.Format(Messages.ASSOCIATEDPRESHIPMENTPALLETID, associatedPreshipmentPLTID), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                        {
                            associatedPreshipmentPLTID = string.Empty;
                        }
                    }
                    if (string.IsNullOrEmpty(associatedPalletId))
                    {
                        PopulatePallet(associatedPalletId);
                    }
                    else
                    {
                        cmbPalletId.Items.Clear();
                        if (!string.IsNullOrEmpty(associatedPalletId))
                        {
                            cmbPalletId.Items.Insert(0, associatedPalletId);
                            cmbPalletId.SelectedIndex = 0;
                            //KahHeng 26Apr2019 Flag to indicate the 1st preshippalletID is continued from before pallet
                            _isContinuedPreshipmentPallet = true;
                            //KahHeng 26Apr2019 End 
                        }
                    }
                    if (string.IsNullOrEmpty(associatedPreshipmentPLTID))
                    {
                        PopulatePreshipmentPLTID(associatedPreshipmentPLTID);
                    }
                    else
                    {
                        cmbPreShipPltId.Items.Clear();
                        if (!string.IsNullOrEmpty(associatedPreshipmentPLTID))
                        {
                            cmbPreShipPltId.Enabled = true;
                            cmbPreShipPltId.Items.Insert(0, associatedPreshipmentPLTID);
                            cmbPreShipPltId.SelectedIndex = 0;
                            //KahHeng 26Apr2019 Flag to indicate the 1st preshippalletID is continued from before pallet
                            _isContinuedPreshipmentPallet = true;
                            //KahHeng 26Apr2019 End 
                        }
                    }
                    _isAssociated = true;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "PopulateAssociatedPalletID", null);
            }
            return true;
        }

        /// <summary>
        /// POPULATE PALLET AND PRESHIPMENT PALLET ID LIST AND ASSOCIATED ID'S
        /// </summary>
        /// <param name="associatedPalletId"></param>
        /// <param name="associatedpreShipPLTID"></param>
        private void PopulatePallet(string associatedPalletId = "", string associatedpreShipPLTID = "")
        {
            try
            {
                List<DropdownDTO> lstPallet = FinalPackingBLL.GetPalletIdList();
                cmbPalletId.Items.Clear();
                cmbPalletId.AutoCompleteCustomSource.Clear();

                cmbPalletId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPalletId.AutoCompleteSource = AutoCompleteSource.CustomSource;

                if (!string.IsNullOrEmpty(associatedPalletId))
                {
                    cmbPalletId.Items.Add(associatedPalletId);
                    cmbPalletId.AutoCompleteCustomSource.Add(associatedPalletId);
                    cmbPalletId.SelectedIndex = 0;
                }
                else
                {
                    cmbPalletId.Items.Clear();
                    cmbPalletId.Text = String.Empty;
                }


                if (lstPallet != null)
                    foreach (DropdownDTO plt in lstPallet)
                    {
                        cmbPalletId.Items.Add(plt.DisplayField);
                        cmbPalletId.AutoCompleteCustomSource.Add(plt.DisplayField);
                    }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
        }
        private void PopulatePreshipmentPLTID(string associatedpreShipPLTID = "")
        {
            try
            {
                List<DropdownDTO> lstPrePallet = FinalPackingBLL.GetPreshipmentPalletIdList();
                cmbPreShipPltId.Items.Clear();

                if (cmbPreShipPltId.AutoCompleteCustomSource != null)
                    cmbPreShipPltId.AutoCompleteCustomSource.Clear();

                cmbPreShipPltId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPreShipPltId.AutoCompleteSource = AutoCompleteSource.CustomSource;

                if (_PSIRequired)
                {
                    cmbPreShipPltId.Enabled = true;
                    string newAssociatedPreShipPLTID = string.Empty;

                    if (String.IsNullOrEmpty(associatedpreShipPLTID))
                        newAssociatedPreShipPLTID = GetItemPreshipmentPalletId(_poNumber, _itemNumber);

                    if (!string.IsNullOrEmpty(newAssociatedPreShipPLTID) && !associatedpreShipPLTID.ToUpper().Equals(newAssociatedPreShipPLTID.ToUpper()))
                    {
                        if (!string.IsNullOrEmpty(newAssociatedPreShipPLTID))
                            associatedpreShipPLTID = newAssociatedPreShipPLTID;

                        if (GlobalMessageBox.Show(string.Format(Messages.ASSOCIATEDPRESHIPMENTPALLETID, associatedpreShipPLTID), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                        {
                            associatedpreShipPLTID = string.Empty;
                        }
                    }
                }
                else
                {
                    cmbPreShipPltId.Enabled = false;
                    associatedpreShipPLTID = string.Empty;
                    cmbPreShipPltId.Text = String.Empty;
                }

                if (!string.IsNullOrEmpty(associatedpreShipPLTID))
                {
                    cmbPreShipPltId.Items.Add(associatedpreShipPLTID);
                    cmbPreShipPltId.AutoCompleteCustomSource.Add(associatedpreShipPLTID);
                    cmbPreShipPltId.SelectedIndex = 0;
                }
                else
                {
                    cmbPreShipPltId.Items.Clear();
                    cmbPreShipPltId.Text = String.Empty;
                }

                if (lstPrePallet != null && _PSIRequired)
                    foreach (DropdownDTO plt in lstPrePallet)
                    {
                        cmbPreShipPltId.Items.Add(plt.DisplayField);
                        cmbPreShipPltId.AutoCompleteCustomSource.Add(plt.DisplayField);
                    }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "PopulatePreshipmentPLTID", null);
            }
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
                if (boxespacked * _innerBoxCapacity <= _BatchtotalPcs)
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

        private void txtBoxesPacked_Leave(object sender, EventArgs e)
        {
            ValidateIndividualBoxesPacked(txtBoxesPacked.Text);
            //if (ValidateIndividualBoxesPacked(txtBoxesPacked.Text))
            //{
            //    if (!_isAssociated)
            //    {
            //        PopulateAssociatedPalletID(); //Populate Associated Pallet and Preshipment Pallet.
            //        _isAssociated = true;
            //    }
            //}
        }

        private void txtTotalBoxesPacked_Change(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtTotalBoxesPacked.Text))
            {
                PopulateAssociatedPalletID(); //Populate Associated Pallet and Preshipment Pallet.

                if (_isAssociated)
                {
                    string associatedPreshipmentPLTID = string.Empty;
                    associatedPreshipmentPLTID = Convert.ToString(cmbPreShipPltId.SelectedItem);
                    PopulatePreshipmentPLTID(associatedPreshipmentPLTID);
                }
            }
        }

        /// <summary>
        /// SERIAL NUMBER VALIDATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerialNumber_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSerialNumber.Text))
            {
                String[] CustRefPO = cmbPurchaseOrder.Text.ToString().Split('|'); //#AZRUL 29/01/2019: SO Field Validation
                #region FP exempt on production date validation
                FP_ExemptValidateDTO fpExemptValidateDTO = FinalPackingBLL.ValidateSerialNoByFPExemptProductionDateValidation(Convert.ToDecimal(txtSerialNumber.Text.Trim()), _itemNumber);
                if (fpExemptValidateDTO.Result.ToUpper() == Constants.FAIL)
                {
                    var errMsg = string.Format(Messages.FPEXEMPT_PRODUCTIONDATEVALIDATION_ERR, fpExemptValidateDTO.ProductionDateValidationDays.ToString(), fpExemptValidateDTO.ProductionDateValidationCustomer);
                    GlobalMessageBox.Show(errMsg, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    txtSerialNumber.Text = string.Empty;
                    txtSerialNumber.Focus();
                    return;
                }
                #endregion
                //#AZRUL 09/01/2019: Block batch card to proceed if not complete QCQI. START
                else if (!(FinalPackingBLL.ValidateSerialNoByQIStatus(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == Constants.FAIL1)) //#AZRUL 14/02/2019: Bypass if SN is exists in FP Exemption
                {
                    if (FinalPackingBLL.ValidateSerialNoByFPExempt(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == Constants.FAIL) //#AZRUL 27/07/2022: 2nd check if got fp exempt, bypass QI checking
                    {
                        QITestResultStatusDto qiTestResultStatus = QAIBLL.Get_PT_Or_QC_QITestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()));
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
                        if (blockDueToQIIncomplete)
                        {
                            GlobalMessageBox.Show(errorMessage, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtSerialNumber.Text = string.Empty;
                            txtSerialNumber.Focus();
                            return;
                        }
                    }
                }
                //#AZRUL 09/01/2019 END
                if (!HourlyBatchCardBLL.IsSerialNumber(Convert.ToDecimal(txtSerialNumber.Text.Trim())))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtSerialNumber.Text = string.Empty;
                    txtSerialNumber.Focus();
                }
                //#AZRUL 29/01/2019: SO Field Validation. START
                else if (FinalPackingBLL.ValidateFGLabel(cmbItemNumber.Text, CustRefPO[1].ToString().Trim(), txtSerialNumber.Text.Trim()) > Constants.ZERO)
                {
                    int r = FinalPackingBLL.ValidateFGLabel(cmbItemNumber.Text, CustRefPO[1].ToString().Trim(), txtSerialNumber.Text.Trim());
                    if (r == Constants.ONE)
                    {
                        GlobalMessageBox.Show(Messages.VALIDATELABEL_CETD, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                    else if (r == Constants.TWO)
                    {
                        GlobalMessageBox.Show(Messages.VALIDATELABEL_CUSTLOTID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                    else if (r == Constants.THREE)
                    {
                        GlobalMessageBox.Show(Messages.VALIDATELABEL_CETDCUSTLOTID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                    else if (r == Constants.FOUR)
                    {
                        GlobalMessageBox.Show(Messages.VALIDATELABEL_NOTCONFIGURED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                    else if (r == Constants.FIVE)
                    {
                        GlobalMessageBox.Show(Messages.VALIDATELABEL_PODATE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }

                    ClearForm();
                }
                //#AZRUL 29/01/2019: END
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
                //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. START
                else if (CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNumber.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPI) &&
                        !CommonBLL.ValidateAXPosting((Convert.ToDecimal(txtSerialNumber.Text.Trim())), CreateInvTransJournalFunctionidentifier.STPO))
                {
                    GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtSerialNumber.Text = string.Empty;
                    txtSerialNumber.Focus();
                }
                else if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == CreateInvTransJournalFunctionidentifier.STPI)
                {
                    GlobalMessageBox.Show(Messages.TEMPPACK_NOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtSerialNumber.Text = string.Empty;
                    txtSerialNumber.Focus();
                }
                else
                {
                    //#AZRUL 29-10-2018 BUGS-1218: TOMS: STPI without STPO cannot proceed to do Final Packing in Floor System. END
                    if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == ReworkOrderFunctionidentifier.RWKCR)
                    {
                        //#AZRUL 27/07/2022: 2nd check if got fp exempt, bypass RWKCR checking.
                        if (FinalPackingBLL.ValidateSerialNoByFPExempt(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == Constants.FAIL)
                        {
                            GlobalMessageBox.Show(Messages.PT_QC_NOT_COMPLETE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtSerialNumber.Text = string.Empty;
                            txtSerialNumber.Focus();
                            return;
                        }
                    }

                    SerilaNumberValidation(Convert.ToDecimal(txtSerialNumber.Text));
                }
            }
            else
            {
                cmbFGBOList.Text = String.Empty;
            }

        }

        /// <summary>
        /// CANCEL TO CLEAR THE FORM FIELDS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                cmbPurchaseOrder.Focus();
            }
        }
        #endregion

        #region USER METHODS

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
                    txtSerialNumber.Clear();
                    _batchNo = string.Empty; //txtBatch.Clear();
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
        /// VALIDATE SERIAL NUMBER ENTERED 
        /// </summary>
        private Boolean SerilaNumberValidation(decimal serialNumber, Boolean isfromPrint = false)
        {
            var objBatchDTO = new BatchDTO();
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
            string batchNumber = string.Empty;
            Boolean blnIsValid = false;
            try
            {
                if (ValidatePORequiredFields()) //Validate Required fields serial number to scan
                {
                    if (!_dictSerialNumber.ContainsKey(txtSerialNumber.Text))
                    {
                        string qaiResults = CommonBLL.ValidateSerialNoByQAIStatus(serialNumber); //validate QAI status
                        if (string.IsNullOrEmpty(qaiResults))
                        {
                            #region FP exempt on production date validation
                            FP_ExemptValidateDTO fpExemptValidateDTO = FinalPackingBLL.ValidateSerialNoByFPExemptProductionDateValidation(serialNumber, _itemNumber);
                            if (fpExemptValidateDTO.Result.ToUpper() == Constants.FAIL)
                            {
                                var errMsg = string.Format(Messages.FPEXEMPT_PRODUCTIONDATEVALIDATION_ERR, fpExemptValidateDTO.ProductionDateValidationDays.ToString(), fpExemptValidateDTO.ProductionDateValidationCustomer);
                                GlobalMessageBox.Show(errMsg, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtSerialNumber.Text = string.Empty;
                                txtSerialNumber.Focus();
                            }
                            #endregion
                            else if (FinalPackingBLL.ValidateSerialNoByQIStatus(serialNumber) == Constants.PASS)
                            {

                                // Comment: disable due to FP station need to proceed before QA testing result out/ready
                                //if (ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.POLYMER_TEST)
                                //&& ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.POWDER_TEST)
                                //&& ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.PROTEIN_TEST)
                                //&& ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.HOTBOX_TEST))
                                //{
                                string strlocation = null;
                                if (!FinalPackingBLL.isBatchScanOut(Convert.ToDecimal(txtSerialNumber.Text.Trim()), out strlocation))
                                {
                                    GlobalMessageBox.Show(String.Format(Messages.NO_SCAN_OUT_SNO_FP, strlocation), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtSerialNumber.Text = string.Empty;
                                    txtSerialNumber.Focus();
                                }
                                else
                                {
                                    objFPTempPackDTO = FinalPackingBLL.CheckIsTemPackBatch(Convert.ToDecimal(txtSerialNumber.Text.Trim()));//Check if scanned batch is from TempPack
                                    if (objFPTempPackDTO.isTempPackBatch)
                                    {
                                        batchNumber = objFPTempPackDTO.TMPPackBatch.BatchNumber;
                                        _isTempack = true;
                                    }
                                    else
                                    {
                                        objBatchDTO = FinalPackingBLL.GetBatchBySerialNoAndSalesOrder(Convert.ToDecimal(txtSerialNumber.Text.Trim()), _GloveCode, _size, _AlternativeGloveCode1, _AlternativeGloveCode2, _AlternativeGloveCode3, _innerBoxCapacity, cmbPurchaseOrder.Text.FPCustomerRefernceSplit(), cmbItemNumber.Text);
                                        batchNumber = objBatchDTO.BatchNumber;
                                        //Max He,20/9/2018,Bind Active FG Batch(Production) Order list
                                        cmbFGBOList.BindComboBox(objBatchDTO.BatchOrders, false);
                                        _ProductionDate = objBatchDTO.BatchCarddate;
                                        _LineId = objBatchDTO.Line;
                                        _isTempack = false;
                                    }

                                    if ((objBatchDTO.IsFPBatchSplit || objFPTempPackDTO.TMPPackBatch.IsFPBatchSplit) && isfromPrint == true)
                                    {
                                        if (ValidateSplitBatch())
                                        {
                                            if (!string.IsNullOrEmpty(batchNumber))
                                            {
                                                _batchNo = batchNumber; //txtBatch.Text = batchNumber;
                                            }
                                            else
                                            {
                                                GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                txtSerialNumber.Focus();
                                                txtSerialNumber.Focus();
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(batchNumber))
                                        {
                                            _batchNo = batchNumber; //txtBatch.Text = batchNumber;
                                        }
                                        else
                                        {
                                            GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                            txtSerialNumber.Focus();
                                            txtSerialNumber.Text = string.Empty;
                                        }
                                    }
                                    blnIsValid = true;
                                }
                                //}
                                //else
                                //{
                                //    txtSerialNumber.Text = string.Empty;
                                //    txtSerialNumber.Focus();
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
                                txtSerialNumber.Text = string.Empty;
                                txtSerialNumber.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(qaiResults, Constants.AlertType.Information, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            txtSerialNumber.Focus();
                            txtSerialNumber.Text = string.Empty;
                            blnIsValid = false;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.SERIALNO_ALREADY_SCANNEDIN, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtSerialNumber.Clear();
                        txtBoxesPacked.Clear();
                        txtSerialNumber.Focus();
                    }
                }
                else
                {
                    txtSerialNumber.Clear();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
            return blnIsValid;
        }

        private Boolean ValidateIndividualBoxesPacked(string individualBoxes)
        {
            int POItemSizecasespacked = Constants.ZERO;
            int boxesentered = Constants.ZERO;
            int totalBoxesEntered = Constants.ZERO;
            Boolean blnIsValid = false;
            try
            {
                //boxes packed Null or empty validation
                if (!string.IsNullOrEmpty(individualBoxes))
                {
                    if (!string.IsNullOrEmpty(txtSerialNumber.Text.Trim()))
                    {
                        boxesentered = int.Parse((txtBoxesPacked.Text.Trim()));
                        if (!string.IsNullOrEmpty(txtTotalBoxesPacked.Text.Trim()))
                        {
                            totalBoxesEntered = int.Parse(txtTotalBoxesPacked.Text.Trim());
                        }

                        if (boxesentered > Constants.ZERO)
                        {
                            POItemSizecasespacked = FinalPackingBLL.ValidatePOSizeBoxesPacked(_poNumber, _itemNumber, _size);
                            //validate boxes entered and boxes already packed is not greater than the requied 
                            if (!ValidateRequiredItemCases(POItemSizecasespacked, boxesentered + totalBoxesEntered))
                            {
                                GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString(((_itemcases - POItemSizecasespacked) * _casecapacity) - totalBoxesEntered)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);

                                txtBoxesPacked.Text = Convert.ToString(((_itemcases - POItemSizecasespacked) * _casecapacity) - totalBoxesEntered);
                                txtBoxesPacked.Focus();
                            }
                            //validate boxes entered and boxes already packed is not greater than the requied 
                            else if (!ValidateBatchCapacity(Convert.ToDecimal(txtSerialNumber.Text), (boxesentered)))//validate selected batch capacity
                            {
                                GlobalMessageBox.Show(string.Format(Messages.BATCHCAPACITY, _BatchtotalPcs / _innerBoxCapacity, _innerBoxCapacity, Environment.NewLine), Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                txtBoxesPacked.Text = Convert.ToString(_BatchtotalPcs / _innerBoxCapacity);
                                txtBoxesPacked.Focus();
                            }
                            //#AZRUL 30-10-2018 BUGS-1227: InnerBox must lesser than CaseCapacity. START
                            else if (boxesentered >= _casecapacity)
                            {
                                GlobalMessageBox.Show(Messages.INNERBOXES_IS_MAX + _casecapacity.ToString(), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                txtBoxesPacked.Focus();
                            }
                            //#AZRUL 30-10-2018 BUGS-1227: InnerBox cannot more than CaseCapacity. END
                            else
                            {
                                blnIsValid = true;
                            } // return true if no validation failed
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BOXES_PACKED_COUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtBoxesPacked.Clear();
                            txtBoxesPacked.Focus();
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
                ExceptionLogging(fsexp, _screenname, _uiClassName, "ValidateIndividualBoxesPacked", null);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateIndividualBoxesPacked", null);
            }
            return blnIsValid;
        }

        private Boolean ValidateTotalBoxesPacked(string totalBoxesPacked)
        {
            int POItemSizecasespacked = Constants.ZERO;
            int boxesentered = Constants.ZERO;
            Boolean blnIsValid = false;
            try
            {
                //boxes packed Null or empty validation
                if (!string.IsNullOrEmpty(totalBoxesPacked))
                {
                    //Validate the boxes packed text is integer.
                    if (Validator.IsValidInput(Framework.Constants.ValidationType.Integer, totalBoxesPacked))
                    {
                        //Round the entered boxes to case capacity and display in text box.
                        boxesentered = int.Parse(totalBoxesPacked);
                        //cases already packed for the given PO,ITEM and SIZE.
                        POItemSizecasespacked = FinalPackingBLL.ValidatePOSizeBoxesPacked(_poNumber, _itemNumber, _size);
                        //validate boxes entered and boxes already packed is not greater than the requied 
                        if (!ValidateRequiredItemCases(POItemSizecasespacked, boxesentered))
                        {
                            GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((_itemcases - POItemSizecasespacked) * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                        }
                        //#AZRUL 30-10-2018 BUGS-1227: TotalInnerbox % CaseCapacity must be 0. START
                        if (!(int.Parse(txtTotalBoxesPacked.Text.Trim()) % _casecapacity == 0))
                        {
                            GlobalMessageBox.Show(Messages.TOTAL_INNERBOXES_MOD_GOT_BALANCE + _casecapacity.ToString(), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        }
                        //#AZRUL 30-10-2018 BUGS-1227: TotalInnerbox % CaseCapacity must be 0. START
                        else
                        {
                            blnIsValid = true;
                        } // return true if no validation failed

                    }
                    else
                    {
                        txtBoxesPacked.Text = string.Empty;
                    }
                }

            }
            catch (FormatException ex)
            {
                FloorSystemException fsexp = new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "ValidateTotalBoxesPacked", null);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateTotalBoxesPacked", null);
            }
            return blnIsValid;
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
            internalLotNumber = string.Format("{0}{1}{2}", curretDate.ToString(Messages.INTERNALLOTNUMBER_DATEFORMAT),
                Convert.ToString(WorkStationDataConfiguration.GetInstance().stationNumber).PadLeft(2, '0'), _nextNumber.ToString().PadLeft(4, '0'));
            return internalLotNumber;
        }

        /// <summary>
        /// GET ACTIVE PO LIST
        /// </summary>
        private void GetActivePOList()
        {
            try
            {
                List<DropdownDTO> lstFirstGradeActivePOList = FinalPackingBLL.GetFirstGradePOList().Distinct<DropdownDTO>().ToList();
                cmbPurchaseOrder.SelectedIndexChanged -= cmbPurchaseOrder_SelectedIndexChanged;
                cmbPurchaseOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPurchaseOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbPurchaseOrder.BindComboBox(lstFirstGradeActivePOList, true);
                cmbPurchaseOrder.SelectedIndexChanged += cmbPurchaseOrder_SelectedIndexChanged;

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

        void cmbPurchaseOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPurchaseOrder.SelectedIndex > Constants.MINUSONE)
                {
                    var selectDto = cmbPurchaseOrder.SelectedItem as DropdownDTO;
                    if (selectDto.CustomField == "StartedUp")
                    {
                        lstItemDetails = new List<SOLineDTO>();
                        lstPODetails = FinalPackingBLL.GetPurchaseOrderList(cmbPurchaseOrder.Text.FPCustomerRefernceSplit(), cmbPurchaseOrder.SelectedValue.ToString());
                        cmbItemNumber.Items.Clear();
                        cmbItemNumber.SelectedIndexChanged -= cmbItemNumber_SelectedIndexChanged;
                        foreach (SOLineDTO soline in lstPODetails)
                        {
                            if (!cmbItemNumber.Items.Contains(soline.ItemNumber))
                            {
                                lstItemDetails.Add(soline);
                                cmbItemNumber.Items.Add(soline.ItemNumber);
                            }
                            //txtBO.Text = soline.BatchOrder;
                        }
                        cmbItemNumber.SelectedIndex = -1;
                        cmbItemNumber.SelectedIndexChanged += cmbItemNumber_SelectedIndexChanged;
                        ClearFormPOChange();
                    }
                    else
                    {
                        GlobalMessageBox.Show("Production status is not started for the selected PO!", Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                        cmbPurchaseOrder.SelectedIndex = -1;
                        cmbFGBOList.Text = string.Empty;
                        this.ActiveControl = cmbPurchaseOrder;
                    }
                }
                txtInternalLotNumber.Text = GetInternalLotNumber();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbPurchaseOrder_SelectedIndexChanged", null);
            }
        }

        void cmbItemNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbItemNumber.SelectedIndex > Constants.MINUSONE)
                {
                    string itemNumber = cmbItemNumber.Text;
                    foreach (SOLineDTO objItemNum in lstItemDetails)
                    {
                        if (objItemNum.ItemNumber == itemNumber)
                            objItemNumber = objItemNum;
                    }
                    List<SOLineDTO> listItemSize = lstPODetails.Where(SOLineDTO => SOLineDTO.ItemNumber == itemNumber).ToList();
                    listItemSize = listItemSize.GroupBy(c => c.ItemSize).Select(g => g.First()).ToList(); //20/06/2018 #AZ: Distinct Item Size
                    cmbItemSize.SelectedIndexChanged -= cmbItemSize_SelectedIndexChanged;
                    cmbItemSize.DataSource = listItemSize;
                    cmbItemSize.ValueMember = "ItemSize";
                    cmbItemSize.DisplayMember = "ItemSize";
                    cmbItemSize.SelectedIndex = -1;
                    cmbItemSize.SelectedIndexChanged += cmbItemSize_SelectedIndexChanged;

                    //List<SOLineDTO> listItemSize = lstPODetails.Where(SOLineDTO => SOLineDTO.ItemNumber == objItemNumber.ItemNumber).ToList();
                    txtItemName.Text = objItemNumber.ItemName;
                }

            }
            catch (Exception ex)
            {
                FloorSystemException fsexception = new FloorSystemException(ex.Message);
                ExceptionLogging(fsexception, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit", null);
            }
        }

        /// <summary>
        /// INSERT OUTER CASE NUMBERS ALONG WITH PRESHIPMENT FLAG FOR THE PO ITEM
        /// </summary>
        private void InsertCaseNumbers()
        {
            try
            {
                FinalPackingBLL.InsertCaseNumbers(_itemcases, _poNumber, _itemNumber, _size, _preshipmentRandomNumbers, WorkStationDTO.GetInstance().WorkStationId, WorkStationDTO.GetInstance().LocationId, objItemSize.InventTransId, _CustomerSize);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "InsertCaseNumbers", null);
            }
        }

        /// <summary>
        /// RETURN ASSOCIATED PALLET AND PRESHIPMENT PALLETID
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return Pallet and Preshipment PalletId</returns>
        private String GetItemSizePalletId(string poNumber, string ItemNumber, string size)
        {
            try
            {
                return FinalPackingBLL.GetItemSizePalletId(poNumber, ItemNumber, size, WorkStationDTO.GetInstance().LocationId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "GetItemSizePalletId", null);
                return null;
            }
        }
        private String GetItemPreshipmentPalletId(string poNumber, string itemNumber)
        {
            try
            {
                return FinalPackingBLL.GetItemPreshipementPalletId(poNumber, itemNumber, WorkStationDTO.GetInstance().LocationId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "GetItemPreshipmentPalletId", null);
                return null;
            }
        }

        private Boolean ValidatePreshipementPalletCase(string poNumber, string itemNumber, int caseNumber, string size)
        {
            try
            {
                return FinalPackingBLL.ValidatePreshipementPalletCase(poNumber, itemNumber, caseNumber, size) > 0;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "GetItemPreshipmentPalletId", null);
                return false;
            }
        }

        /// <summary>
        /// GENERATE RANDOM PRESHIPMENT CASE NUMBER
        /// </summary>
        /// <param name="preshipmentQty">No of Preshipmetn casenumbers required</param>
        /// <param name="cty">Total Case Quantity</param>
        /// <returns>returns , separated Preshipment case numbers</returns>
        private string GenerateRandomNumbers(int preshipmentQty, int cty)
        {
            string strresult = string.Empty;
            try
            {
                Random rand = new Random();
                Int32 curValue = 0;
                int preshipmentQtyStart = 0;
                int preshipmentQtyEnd = 0;
                // Calculate rounded interval amount - always rounding down to avoid out of range 
                if (preshipmentQty == 0) //#MH 06/08/2018
                    preshipmentQty = 1;
                int preshipmentQtyInterval = Decimal.ToInt32((decimal)cty / preshipmentQty);

                List<Int32> result = new List<Int32>();
                for (Int32 presqtycount = Constants.ZERO; presqtycount < preshipmentQty; presqtycount++)
                {
                    // Start will be 1 and Last interval will always be the cty amount
                    preshipmentQtyStart = preshipmentQtyEnd + 1;
                    preshipmentQtyEnd = ((presqtycount + 1) == preshipmentQty) ? cty : preshipmentQtyInterval * (presqtycount + 1);
                    curValue = rand.Next(preshipmentQtyStart, preshipmentQtyEnd + 1);

                    result.Add(curValue);
                    strresult += curValue + Constants.COMMA;
                }
                return strresult.TrimEnd(Constants.CHARCOMMA);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(ex.Message);
            }
        }

        /// <summary>
        /// REQUIRED FIELDS TILL PO, ITEM, Size
        /// </summary>
        /// <returns>returns true if valid</returns>
        private Boolean ValidatePORequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// REQUIRED FIELD VALIDATION
        /// </summary>
        /// <returns>returns true if valid</returns>
        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            //#MH 26/06/2018 Hide Group Id info
            //validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            if (gvScanMultiBatchInfo.Rows.Count == 0)
                validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, Messages.MBT, Hartalega.FloorSystem.Framework.Common.ValidationType.Custom));
            validationMesssageLst.Add(new ValidationMessage(cmbPalletId, Messages.REQPALLETREQUIRED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));

            if (cmbPreShipPltId.Enabled)
                validationMesssageLst.Add(new ValidationMessage(cmbPreShipPltId, Messages.REQPRESHIPMENTPALLETREQUIRED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));

            return ValidateForm();
        }

        private Boolean ValidateMultipleBatchAdd()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, Messages.REQSERIALNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBoxesPacked, Messages.REQBOXESPACKED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            //#MH 26/06/2018 Hide Group Id info
            //validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            return ValidateForm();
        }

        public string CreateXML(Object BatchInfo)
        {
            XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
            // Initializes a new instance of the XmlDocument class.          
            XmlSerializer xmlSerializer = new XmlSerializer(BatchInfo.GetType());
            // Creates a stream whose backing store is memory. 
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, BatchInfo);
                xmlStream.Position = 0;
                //Loads the XML document from the specified string.
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        /// <summary>
        /// saves the transaction details for the scanned Batch
        /// </summary>
        /// <param name="objFinalPackingDTO">Final Packing DTO object</param>
        private int InsertFinalPacking(FinalPackingDTO objFinalPackingDTO, List<FinalPackingBatchInfoDTO> objFinalPackingBatchInfoDTO)
        {
            int rowsreturned = Constants.ZERO;
            int totalCasespacked = Constants.ZERO;
            int BatchUpdate = Constants.ZERO;
            string batchInfo = string.Empty;
            try
            {
                batchInfo = CreateXML(objFinalPackingBatchInfoDTO);
                //capture the already placed case count on the selected pallet
                int selectedPalletCasesPlaced = FinalPackingBLL.ValidatePalletCapacity(objFinalPackingDTO.Palletid, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                if (selectedPalletCasesPlaced < _palletcapacity)
                {
                    rowsreturned = FinalPackingBLL.InsertMultiScanFinalPacking(objFinalPackingDTO, batchInfo);
                    if (rowsreturned > 0)
                    {
                        totalCasespacked = FinalPackingBLL.GetPONumberItemCasesPacked(_poNumber, _itemNumber, _size);
                        if (totalCasespacked == _itemcases)
                        {
                            int rowsupdated = FinalPackingBLL.UpdatePONumberItemstatus(_poNumber, _itemNumber, _size);
                            if (rowsupdated > 0)
                            {
                                BatchUpdate = FinalPackingBLL.UpdateMultiScanBatchCapacity(batchInfo);
                            }
                            //added by KahHeng (09Jan019)
                            //Used for update the Preshipment PalletID "isOccupied" flag to 1 in PalletMaster table
                            FinalPackingBLL.UpdatePreshipmentPalletIDFlag(objFinalPackingDTO.PreshipmentPLTId, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.locationId);
                            //end edit
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "InsertFinalPacking", null);
            }
            return rowsreturned;
        }

        private static bool PostAXData(string internallotnumber)
        {
            bool isPostingSuccess = false;
            try
            {
                //Decoupled for Industrial PC Scan
                isPostingSuccess = true;
                //isPostingSuccess = AXPostingBLL.PostAXDataFinalPacking(internallotnumber, Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanMultipleBatchPacking)));
            }
            catch (Exception ex)
            {
                isPostingSuccess = false;
                //FinalPackingBLL.RollBackPrintInnerOuterTransaction(internallotnumber);
                throw new FloorSystemException(ex.Message, Constants.AXSERVICEERROR, ex);
            }
            if (isPostingSuccess == false)
            {
                //FinalPackingBLL.RollBackPrintInnerOuterTransaction(internallotnumber);
                GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
            else
            {
                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }

            return isPostingSuccess;
        }

        private void PrintInnerandOuter(string internalLotNumber, Boolean isGCLabel = false)
        {
            FinalPackingPrint.LabelPrint(internalLotNumber, isGCLabel);
            //InnerSetReturn objInnerSetReturn = FinalPackingPrint.ReturnOuterSet(internalLotNumber, _outersetLayOut);
            //if (objInnerSetReturn != null)
            //{
            //    FinalPackingBLL.UpdateSpecialInternalLotNumber(internalLotNumber, objInnerSetReturn);
            //}
        }

        /// <summary>
        /// CLEAR THE FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            cmbPurchaseOrder.SelectedIndex = Constants.MINUSONE;
            cmbItemNumber.Items.Clear();
            cmbFGBOList.Text = string.Empty;
            cmbItemSize.DataSource = null;
            cmbItemNumber.Text = string.Empty;
            _isAssociated = false;
            _isTempack = false;
            ClearFormPOChange();
            EnableDisabledField(true); // #Azrul 13/07/2018: Merged from Live AX6
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
            GetActivePOList();
            BindPackingGroup();
            btnPrint.Enabled = true;
        }
        /// <summary>
        /// CLEAR FORM FOR PO CHNAGE
        /// </summary>
        private void ClearFormPOChange()
        {
            txtItemName.Text = string.Empty;

            txtSerialNumber.Text = string.Empty;
            _dictSerialNumber.Clear();
            _batchNo = string.Empty; //txtBatch.Text = string.Empty;
            txtBoxesPacked.Text = string.Empty;
            txtTotalBoxesPacked.Text = string.Empty;
            _totalBoxesPacked = 0;
            cmbPalletId.Items.Clear();
            cmbPalletId.Text = string.Empty;
            cmbPreShipPltId.DataSource = null;
            cmbPreShipPltId.Text = string.Empty;
            cmbGroupId.SelectedIndex = -1;
            _lstFinalPackingInfo = new List<FinalPackingBatchInfoDTO>();
            this.gvScanMultiBatchInfo.DataSource = null;
            this.gvScanMultiBatchInfo.Rows.Clear();
        }

        /// <summary>
        /// LOG TO DB, SHOW MESAGE BOX TO USER AND CLEAR THE FORM ON EXCEPTION
        /// </summary>
        /// <param name="floorException">APPLICATION EXCEPTION</param>
        /// <param name="screenName">SCREEN NAME TO BE LOGGED</param>
        /// <param name="UiClassName">CLASS NAME TO BE LOGGED</param>
        /// <param name="uiControl">CONTROL FOR WHICH THE EXCEPTION OCCURED</param>
        /// <param name="parameters"></param>
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

        private DateTime? GetManufacturingDate(int manufacturingOrderBasis, DateTime? SHIPPINGDATEREQUESTED, DateTime? RECEIPTDATEREQUESTED, DateTime? batchCardProductionDate, DateTime? PODATE, DateTime FirstManufacturingDate)
        {
            DateTime? manufacturingdate = new DateTime();

            switch (manufacturingOrderBasis)
            {
                case Constants.ZERO:
                    manufacturingdate = CommonBLL.GetCurrentDateAndTimeFromServer(); 
                    break;
                case Constants.ONE:
                    manufacturingdate = batchCardProductionDate;
                    break;
                case Constants.TWO:
                    manufacturingdate = RECEIPTDATEREQUESTED;
                    break;
                case Constants.THREE:
                    manufacturingdate = SHIPPINGDATEREQUESTED;
                    break;
                case Constants.FOUR:
                    manufacturingdate = PODATE;
                    break;
                //Pang 09/02/2020 First Carton Packing Date
                case Constants.FIVE:
                    manufacturingdate = FirstManufacturingDate; //First Carton Packing Date
                    break;
            }
            return manufacturingdate;
        }

        #endregion

        /// <summary>
        /// #MH 26/06/2018 use TransactionScope replace rollback function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_BarcodeVerificationRequired == Constants.ZERO || _BarcodeVerificationRequired == Constants.THREE)
            {
                ProceedPrint();
            }
            else
            {
                if (WorkStationDataConfiguration.GetInstance()._skipBarcodeValidation == true && _BarcodeVerificationRequired == Constants.ONE)
                {
                    ProceedPrint();
                }
                else if (PHLotVerification.CheckTowerLightScanner(_BarcodeVerificationRequired == Constants.TWO ? true : false))
                {
                    ProceedPrint();
                }
            }
        }

        private void ProceedPrint()
        {
            //decimal serialNumber = Constants.ZERO;
            string internallotno = string.Empty;
            string _palletid = string.Empty;
            string _preshipmentPltID = string.Empty;
            int boxespacked = Constants.ZERO;
            int _preshipmentcases = Constants.ZERO;
            int selectedPalletCasesPlaced = Constants.ZERO;
            int selectedPreshipPalletCasesPlaced = Constants.ZERO;
            int rowsaffected = Constants.ZERO;
            //KahHeng 25Apr2019
            //String for concatinated preshipmentPalletID
            string _preshipmentPltIDConcated = string.Empty;

            FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
            OuterLabelDTO objOuterLabelDTO = new OuterLabelDTO();

            bool isPass = false;
            bool isComplete = false;
            string authorisedBy = string.Empty;
            LineClearanceLogDTO objLineClearanceLog = new LineClearanceLogDTO();
            //Get previous Line Clearance info for current screen.
            objLineClearanceLog = GetLineClearanceLogObject(WorkStationDTO.GetInstance().WorkStationId);

            try
            {
                btnPrint.Enabled = false;
                if (ValidateRequiredFields())
                {
                    if (ValidateBatchCard_TOMS())
                    {
                        if (ValidateTotalBoxesPacked(txtTotalBoxesPacked.Text))  // Validate Boxes entered on Print 
                        {
                            if (PopulateAssociatedPalletID(true))
                            {
                                // Check any changes on PONumber, Size, ItemNumber and Pallet ID from previous Print Info
                                if (ChangesOnRelatedField(objLineClearanceLog))
                                {
                                    //Prompt Authorization to verify Line Clearance
                                    LineClearanceVerification LCV = new LineClearanceVerification();
                                    LCV.IsPeha = false;
                                    LCV.StartPosition = FormStartPosition.Manual;
                                    LCV.Location = new Point(txtSerialNumber.Location.X + 330, groupBox1.Location.Y + 303);
                                    LCV.ShowDialog();

                                    // Proceed when Verified successfully 
                                    if (LCV.IsVerified)
                                    {
                                        isPass = true;
                                        authorisedBy = LCV.EmployeeID;
                                    }
                                }
                                else
                                {
                                    // proceed when no changes on PONumber, Size, ItemNumber and Pallet ID compare to previous record.
                                    isPass = true;
                                }

                                if (isPass)
                                {

                                    // Avoid duplicate internallotnumber at workstation level
                                    int lastRunningNo = 0;
                                    if (FinalPackingBLL.CheckDuplicateInternalLotNumber(txtInternalLotNumber.Text,
                                            WorkStationDataConfiguration.GetInstance().stationNumber,
                                            out lastRunningNo))
                                    {
                                        txtInternalLotNumber.Text =
                                            GetInternalLotNumber(); // Get latest/updated internal lot number 
                                    }

                                    boxespacked = int.Parse(txtTotalBoxesPacked.Text);
                                    internallotno = txtInternalLotNumber.Text;
                                    _palletid = Convert.ToString(cmbPalletId.SelectedItem);
                                    _preshipmentPltID = Convert.ToString(cmbPreShipPltId.SelectedItem);
                                    _casespacked = Convert.ToInt32(txtTotalBoxesPacked.Text) / _casecapacity;
                                    selectedPalletCasesPlaced = FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size);
                                    objFinalPackingDTO.locationId = WorkStationDTO.GetInstance().LocationId;
                                    objFinalPackingDTO.Workstationnumber = WorkStationDTO.GetInstance().WorkStationId;
                                    objFinalPackingDTO.Printername = WorkStationDataConfiguration.GetInstance().innerPrinter;
                                    objFinalPackingDTO.Packdate = CommonBLL.GetCurrentDateAndTimeFromServer();
                                    objFinalPackingDTO.Internallotnumber = internallotno;
                                    objFinalPackingDTO.Outerlotno = internallotno;
                                    objFinalPackingDTO.Ponumber = _poNumber;
                                    objFinalPackingDTO.ItemNumber = _itemNumber;
                                    objFinalPackingDTO.Size = _size;
                                    objFinalPackingDTO.Boxespacked = boxespacked;
                                    objFinalPackingDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
                                    objFinalPackingDTO.Innersetlayout = _innesetLayOut;
                                    objFinalPackingDTO.Outersetlayout = _outersetLayOut;
                                    objFinalPackingDTO.palletCapacity = _palletcapacity;
                                    objFinalPackingDTO.FPStationNo = txtstation.Text; //Added by Azman 2019-08-22 FPDetailsReport
                                    objFinalPackingDTO.ManufacturingDate = GetManufacturingDate(_manufacturingOrderbasis, _shippingDateRequested, _receiptDateRequested, _ProductionDate, _custPODocumentDate, _FirstManufacturingDate);
                                    objFinalPackingDTO.ExpiryDate = objFinalPackingDTO.ManufacturingDate.Value.AddMonths(_expiry);
                                    objFinalPackingDTO.stationNumber = WorkStationDataConfiguration.GetInstance().stationNumber;
                                    objFinalPackingDTO.Palletid = _palletid;
                                    objFinalPackingDTO.PreshipmentPLTId = _preshipmentPltID;
                                    objFinalPackingDTO.Casespacked = _casespacked;
                                    objFinalPackingDTO.InventTransId = objItemSize.InventTransId;
                                    objFinalPackingDTO.FGBatchOrderNo = cmbFGBOList.Text;
                                    objFinalPackingDTO.Resource = FinalPackingBLL.GetResourceFromBatchOrder(objFinalPackingDTO.FGBatchOrderNo);

                                    rowsaffected = InsertFinalPacking(objFinalPackingDTO, _lstFinalPackingInfo);
                                    int palletCasesCount = FinalPackingBLL.GetPalletIdCount(objFinalPackingDTO.Internallotnumber);
                                    int preshipmentPalletCasesCount = FinalPackingBLL.GetPreshipmentPalletIdCount(objFinalPackingDTO.Internallotnumber);
                                    selectedPalletCasesPlaced = FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size);
                                    selectedPreshipPalletCasesPlaced = FinalPackingBLL.ValidatePreshipmentPalletCapacity(_preshipmentPltID, _poNumber);

                                    //map Line Clearance Log object
                                    objLineClearanceLog = new LineClearanceLogDTO
                                    {
                                        PONumber = objFinalPackingDTO.Ponumber,
                                        Size = objFinalPackingDTO.Size,
                                        ItemNumber = objFinalPackingDTO.ItemNumber,
                                        PalletID = objFinalPackingDTO.Palletid,
                                        ScreenName = _screenname,
                                        AuthorisedBy = authorisedBy,
                                        CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer(),
                                        WorkStationID = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId)
                                    };
                                    //bool isFPPostingSuccess = true;

                                    if (rowsaffected > Constants.ZERO)
                                    {
                                        //try
                                        //{

                                        while (palletCasesCount > Constants.ZERO)
                                        {
                                            if (palletCasesCount > _palletcapacity - selectedPalletCasesPlaced)
                                            {

                                                //_palletid = string.Empty;
                                                if ((_palletid == string.Empty) || (_palletid == "") || (_palletid.Length < 7))
                                                {
                                                    throw new Exception("Pallet Id is empty");
                                                }
                                                FinalPackingBLL.InsertPalletId(objFinalPackingDTO.Internallotnumber, _palletid, _palletcapacity - selectedPalletCasesPlaced,
                                                    _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size, WorkStationDTO.GetInstance().LocationId);
                                                palletCasesCount = palletCasesCount - (_palletcapacity - selectedPalletCasesPlaced);
                                                //edit by Cheah (17Aug2017)
                                                //GenerateEwareNaviFile(_palletid, objFinalPackingDTO);
                                                InsertDataToEWNTable(_palletid, false, objFinalPackingDTO);
                                                //end edit (17Aug2017)
                                                PalletSelection _PalletSelectionForm = new PalletSelection();
                                                _PalletSelectionForm.SelectedPalletID = Convert.ToString(_palletid);
                                                _PalletSelectionForm.isPreshipment = false;
                                                _PalletSelectionForm.ShowDialog();
                                                if (!string.IsNullOrEmpty(_PalletSelectionForm.childPalletId))
                                                {
                                                    _palletid = objFinalPackingDTO.Palletid = Convert.ToString(_PalletSelectionForm.childPalletId);
                                                    selectedPalletCasesPlaced = Constants.ZERO;
                                                }
                                            }
                                            else
                                            {
                                                //_palletid = string.Empty;
                                                if ((_palletid == string.Empty) || (_palletid == "") || (_palletid.Length < 7))
                                                {
                                                    throw new Exception("Pallet Id is empty");
                                                }
                                                FinalPackingBLL.InsertPalletId(objFinalPackingDTO.Internallotnumber, _palletid, palletCasesCount, _palletcapacity,
                                                    objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size, WorkStationDTO.GetInstance().LocationId);
                                                if (palletCasesCount == _palletcapacity - selectedPalletCasesPlaced)
                                                {
                                                    //edit by Cheah (17Aug2017)
                                                    //GenerateEwareNaviFile(_palletid, objFinalPackingDTO);
                                                    InsertDataToEWNTable(_palletid, false, objFinalPackingDTO);
                                                    //end edit (17Aug2017)
                                                }
                                                palletCasesCount = palletCasesCount - _palletcapacity;
                                            }
                                        }
                                        //Preshipment Pallet ID
                                        while (preshipmentPalletCasesCount > Constants.ZERO)
                                        {
                                            if (preshipmentPalletCasesCount > _palletcapacity - selectedPreshipPalletCasesPlaced)
                                            {
                                                //KahHeng (26Apr2019)
                                                //To store the multiple preshipment PalletID
                                                _preshipmentPltIDConcated = _preshipmentPltIDConcated + _preshipmentPltID + "!!!";
                                                //End Edit (26Apr2019)

                                                FinalPackingBLL.InsertPresPalletId(objFinalPackingDTO.Internallotnumber, _preshipmentPltID,
                                                    _palletcapacity - selectedPreshipPalletCasesPlaced, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, WorkStationDTO.GetInstance().LocationId);
                                                preshipmentPalletCasesCount = preshipmentPalletCasesCount - (_palletcapacity - selectedPreshipPalletCasesPlaced);
                                                //edit by Cheah (17Aug2017)
                                                //GenerateEwareNaviFileForPreshipment(_preshipmentPltID, objFinalPackingDTO);
                                                InsertDataToEWNTable(_preshipmentPltID, true, objFinalPackingDTO);
                                                //end edit (17Aug2017)
                                                PalletSelection _PalletSelectionForm = new PalletSelection();
                                                _PalletSelectionForm.SelectedPalletID = Convert.ToString(_preshipmentPltID);
                                                _PalletSelectionForm.isPreshipment = true;
                                                _PalletSelectionForm.ShowDialog();
                                                if (!string.IsNullOrEmpty(_PalletSelectionForm.childPalletId))
                                                {
                                                    _preshipmentPltID = objFinalPackingDTO.PreshipmentPLTId = Convert.ToString(_PalletSelectionForm.childPalletId);
                                                    preshipmentPalletCasesCount = Constants.ZERO;
                                                }
                                            }
                                            else
                                            {
                                                FinalPackingBLL.InsertPresPalletId(objFinalPackingDTO.Internallotnumber, _preshipmentPltID,
                                                    preshipmentPalletCasesCount, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, WorkStationDTO.GetInstance().LocationId);
                                                if (preshipmentPalletCasesCount == _palletcapacity - selectedPreshipPalletCasesPlaced)
                                                {
                                                    //edit by Cheah (17Aug2017)
                                                    //GenerateEwareNaviFileForPreshipment(_preshipmentPltID, objFinalPackingDTO);
                                                    InsertDataToEWNTable(_preshipmentPltID, true, objFinalPackingDTO);
                                                    //end edit (17Aug2017)
                                                }
                                                preshipmentPalletCasesCount = preshipmentPalletCasesCount - _palletcapacity;
                                            }
                                        }
                                        // Update SpecialInternalLotnumber, Mfg Date and Exp Date
                                        string specialLotNumber = FinalPackingBLL.UpdatespeciallotnumberforInner(objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.ManufacturingDate.Value, objFinalPackingDTO.ExpiryDate.Value,
                                            _LineId, _expiry, _innesetLayOut, _customerLotNumber);

                                        FinalPackingBLL.UpdatespeciallotnumberforOuter(objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.ManufacturingDate.Value, objFinalPackingDTO.ExpiryDate.Value,
                                             _LineId, _expiry, _outersetLayOut, _customerLotNumber, _shippingDateRequested.Value, specialLotNumber, _innesetLayOut);

                                        if (!string.IsNullOrEmpty(specialLotNumber))
                                        {
                                            objFinalPackingDTO.Internallotnumber = specialLotNumber;
                                        }

                                        // 20220808: Azrul: FP Exempt with RWKCR, need to post RWKDEL
                                        List<BatchDTO> objBatchDTO = FinalPackingBLL.GetSerialNumberbyInternalLotNumber(objFinalPackingDTO.Internallotnumber);
                                        foreach (var batch in objBatchDTO)
                                        {
                                            // FP Exempt with RWKCR, need to post RWKDEL
                                            if (QAIBLL.GetLastServiceName(Convert.ToDecimal(batch.SerialNumber.Trim())) == ReworkOrderFunctionidentifier.RWKCR &&
                                                FinalPackingBLL.ValidateSerialNoByFPExempt(Convert.ToDecimal(batch.SerialNumber.Trim())) == Constants.PASS)
                                            {
                                                AXPostingBLL.PostAXDataDeleteReworkOrderPTQIFailPT(batch.SerialNumber.Trim());
                                            }
                                        }

                                        //Call Printer Interface methods

                                        // Decoupled for Industrial PC Scan
                                        //AXPostingBLL.PostAXDataFinalPacking(objFinalPackingDTO.Internallotnumber, Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanMultipleBatchPacking)));

                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    isFPPostingSuccess = false;
                                        //    GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                        //    txtInternalLotNumber.Text = GetInternalLotNumber();
                                        //    FinalPackingBLL.RollBackPrintInnerOuterTransaction(objFinalPackingDTO.Internallotnumber);
                                        //    FloorSystemException fsexp = new FloorSystemException(ex.Message, "", ex);
                                        //    CommonBLL.LogExceptionToDB(fsexp, _screenname, _uiClassName, "Print_Click", null);
                                        //}

                                        //if (isFPPostingSuccess)
                                        //{
                                        // No need check AX posting status for D365
                                        //bool isPostingSucess = PostAXData(objFinalPackingDTO.Internallotnumber);
                                        //if (isPostingSucess)
                                        //{
                                        //Call Printer Interface methods
                                        objOuterLabelDTO = FinalPackingBLL.PrintInnerandOuter(objFinalPackingDTO.Internallotnumber, _GCLabelPrintingRequired == Constants.ONE ? true : false, _BarcodeVerificationRequired == Constants.ONE ? true : false);
                                        if (_BarcodeVerificationRequired == Constants.ONE)
                                        {
                                            if (!string.IsNullOrEmpty(objOuterLabelDTO.barcodeToValidate))
                                            {
                                                new BarcodeVerificationPass(WorkStationDataConfiguration.GetInstance().stationNumber, objFinalPackingDTO.Internallotnumber,
                                                           objFinalPackingDTO.Ponumber, objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, _purchaseorderFormNum, txtItemName.Text, _size).ShowDialog();
                                            }
                                            else
                                            {
                                                GlobalMessageBox.Show(Messages.BARCODE_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                            }
                                        }
                                        // Pang 09/02/2020 First Carton Packing Date
                                        FinalPackingBLL.UpdateFirstCartonPackingDate(objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);

                                        //    //Update POStatus
                                        int POStatusRow = FinalPackingBLL.UpdatePOStatus(objFinalPackingDTO.Ponumber);

                                        if (FinalPackingBLL.GetPreshipmentCasesQuantitytoPackforPO(objFinalPackingDTO.Ponumber) == Constants.ZERO)
                                        {
                                            if (!FinalPackingBLL.isPreshipmentQAEmailSent(objFinalPackingDTO.Ponumber))
                                                FinalPackingBLL.SendPreshipmentQAEmail(objFinalPackingDTO.Ponumber, _purchaseorderFormNum, objFinalPackingDTO.ItemNumber,
                                                    txtItemName.Text, objFinalPackingDTO.Size, objFinalPackingDTO.Internallotnumber, cmbPreShipPltId.Text);
                                        }
                                        //}
                                    }

                                    //Insert LineClearanceLog
                                    FinalPackingBLL.InsertLineClearanceLog(objLineClearanceLog);

                                    EventLogDTO EventLog = new EventLogDTO();
                                    EventLog.CreatedBy = String.Empty;
                                    Constants.EventLog evtAction = Constants.EventLog.Print;
                                    EventLog.EventType = Convert.ToInt32(evtAction);
                                    EventLog.EventLogType = Constants.eventlogtype;

                                    var screenid = CommonBLL.GetScreenIdByScreenName(_screenname);
                                    CommonBLL.InsertEventLog(EventLog, _screenname, screenid.ToString());


                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    isComplete = true;
                                    txtInternalLotNumber.Text = GetInternalLotNumber();
                                    ClearForm();
                                    cmbPurchaseOrder.Focus();
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.FP_PALLETALREADYUSED, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "Print_Click", null);
            }
            catch (Exception genEx)
            {
                FloorSystemException fsexp = new FloorSystemException(genEx.Message, "", genEx);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "Print_Click", null);
            }


            if (isPass && isComplete)
            {
                #region lot number verification
                if (_BarcodeVerificationRequired == Constants.ONE && WorkStationDataConfiguration.GetInstance()._skipBarcodeValidation == false)
                {
                    if (!string.IsNullOrEmpty(objOuterLabelDTO.barcodeToValidate))
                    {
                        new BarcodeVerificationPass(WorkStationDataConfiguration.GetInstance().stationNumber, objFinalPackingDTO.Internallotnumber,
                                   objFinalPackingDTO.Ponumber, objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, _purchaseorderFormNum, txtItemName.Text, _size).ShowDialog();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.BARCODE_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                else if (_BarcodeVerificationRequired == Constants.TWO && WorkStationDataConfiguration.GetInstance().LHardwareIntegration.GetValueOrDefault())
                {
                    PHLotVerification ph = new PHLotVerification
                    {
                        PONumber = objFinalPackingDTO.Ponumber,
                        ItemNumber = objFinalPackingDTO.ItemNumber,
                        ItemSize = objFinalPackingDTO.Size,
                        internalLotNumber = objFinalPackingDTO.Internallotnumber,
                        caseCapacity = Convert.ToInt32(_casecapacity),
                        TotalInnerbox = Convert.ToInt32(txtTotalBoxesPacked.Text),
                        PalletID = objFinalPackingDTO.Palletid
                    };
                    ph.ShowDialog();
                }
                #endregion

                txtInternalLotNumber.Text = GetInternalLotNumber();
                ClearForm();
                cmbPurchaseOrder.Focus();
            }

            btnPrint.Enabled = true;
        }

        //add by Cheah (17Aug2017)
        private void InsertDataToEWNTable(string _palletid, bool isPreship, FinalPackingDTO objFinalPackingDTO)
        {
            if (isPreship)
            {
                if (FinalPackingBLL.ValidatePreshipmentPalletCapacity(_palletid, _poNumber) == _palletcapacity)
                    //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                    FinalPackingBLL.InsertEWareNaviData(_palletid, isPreship, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size, _CustomerSize);
            }
            else
            {
                if (FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size) == _palletcapacity)
                    //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                    FinalPackingBLL.InsertEWareNaviData(objFinalPackingDTO.Palletid, isPreship, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size, _CustomerSize);
            }
        }
        //end add(17Aug2017)

        //commented out by Cheah (17Aug2017)
        //private void GenerateEwareNaviFile(string _palletid, FinalPackingDTO objFinalPackingDTO)
        //{
        //    if (FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size) == _palletcapacity)
        //        FinalPackingBLL.GeneratePalletFile(objFinalPackingDTO.Palletid, false, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size);
        //}

        //private void GenerateEwareNaviFileForPreshipment(string _palletid, FinalPackingDTO objFinalPackingDTO)
        //{
        //    if (FinalPackingBLL.ValidatePreshipmentPalletCapacity(_palletid, _poNumber) == _palletcapacity)
        //        FinalPackingBLL.GeneratePalletFile(_palletid, true, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size);
        //}
        // end comment(17Aug2017)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMultiJob_Click(object sender, EventArgs e)
        {
            //int selectedPalletCasesPlaced = 0;
            //string _palletid = string.Empty;
            FinalPackingBatchInfoDTO objFinalPackingBatchInfoDTO = new FinalPackingBatchInfoDTO();

            try
            {
                if (!(gvScanMultiBatchInfo.Rows.Count == 5))
                {
                    if (ValidateMultipleBatchAdd())
                    {
                        if (ValidateIndividualBoxesPacked(txtBoxesPacked.Text))  // Validate Boxes entered on Print
                        {
                            if (SerilaNumberValidation(Convert.ToDecimal(txtSerialNumber.Text), true)) // Validate SerialNumber scanned on print
                            {
                                _dictSerialNumber.Add(txtSerialNumber.Text, txtSerialNumber.Text);
                                //_palletid = Convert.ToString(cmbPalletId.SelectedItem);
                                //selectedPalletCasesPlaced = FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size);

                                objFinalPackingBatchInfoDTO.SerialNumber = Convert.ToDecimal(txtSerialNumber.Text);
                                objFinalPackingBatchInfoDTO.BatchNumber = _batchNo;
                                objFinalPackingBatchInfoDTO.BoxesPacked = Convert.ToInt32(txtBoxesPacked.Text);
                                objFinalPackingBatchInfoDTO.TotalPcs = objFinalPackingBatchInfoDTO.BoxesPacked * _innerBoxCapacity;
                                _totalBoxesPacked += Convert.ToInt32(txtBoxesPacked.Text);
                                txtTotalBoxesPacked.Text = string.Format(Constants.NUMBER_FORMAT, _totalBoxesPacked);
                                objFinalPackingBatchInfoDTO.CasesPacked = Convert.ToInt32(txtBoxesPacked.Text) / _casecapacity;
                                objFinalPackingBatchInfoDTO.TotalPcs = Convert.ToInt32(txtBoxesPacked.Text) * _innerBoxCapacity;
                                objFinalPackingBatchInfoDTO.IsTempPack = _isTempack;
                                _lstFinalPackingInfo.Add(objFinalPackingBatchInfoDTO);
                                BindScanMultiBatchInfoGrid();

                                txtSerialNumber.Text = string.Empty;
                                txtBoxesPacked.Text = string.Empty;
                                _batchNo = string.Empty;

                                // #Azrul 13/07/2018: Merged from Live AX6 Start
                                if (gvScanMultiBatchInfo.Rows.Count > 0)
                                {
                                    EnableDisabledField(false);
                                }
                                // #Azrul 13/07/2018: Merged from Live AX6 End
                            }
                        }
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.SCAN_MULTIPLE_BATCH_TABLE, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnAddMultiJob_Click", null);
            }
            catch (Exception genEx)
            {
                FloorSystemException fsexp = new FloorSystemException(genEx.Message, "", genEx);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "btnAddMultiJob_Click", null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSource"></param>
        private void BindScanMultiBatchInfoGrid()
        {
            this.gvScanMultiBatchInfo.DataSource = null;
            this.gvScanMultiBatchInfo.Rows.Clear();

            gvScanMultiBatchInfo.AutoGenerateColumns = false;
            gvScanMultiBatchInfo.AutoSize = false;

            DataTable BindScanMultiBatchInfoDT = new DataTable();
            BindScanMultiBatchInfoDT.Columns.Add("SerialNumber", typeof(System.String));
            BindScanMultiBatchInfoDT.Columns.Add("BatchNumber", typeof(System.String));
            BindScanMultiBatchInfoDT.Columns.Add("BoxesPacked", typeof(System.String));

            var query = from row in _lstFinalPackingInfo select row;
            DataRow BindScanMultiBatchInfoRow = null;
            foreach (var row in query)
            {
                BindScanMultiBatchInfoRow = BindScanMultiBatchInfoDT.NewRow();
                BindScanMultiBatchInfoRow["SerialNumber"] = row.SerialNumber;
                BindScanMultiBatchInfoRow["BatchNumber"] = row.BatchNumber;
                BindScanMultiBatchInfoRow["BoxesPacked"] = string.Format(Constants.NUMBER_FORMAT, row.BoxesPacked);
                BindScanMultiBatchInfoDT.Rows.Add(BindScanMultiBatchInfoRow);
            }
            gvScanMultiBatchInfo.DataSource = BindScanMultiBatchInfoDT;

        }

        //Vinoden March 2020
        private Boolean ValidateBatchCard_TOMS()
        {
            List<string> reg_SerialNo = new List<string>();
            List<string> scanout_SerialNo = new List<string>();
            List<string> datagridLst = new List<string>();
            string param_SerialNO = string.Empty;
            bool proceedPacking = false;
            try
            {
                if (gvScanMultiBatchInfo.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gvScanMultiBatchInfo.Rows)
                    {
                        datagridLst.Add(row.Cells[0].Value.ToString());
                    }
                }

                if (datagridLst.Count() > 1)
                {
                    param_SerialNO = String.Join(",", datagridLst);
                }
                else
                {
                    param_SerialNO = datagridLst[0];
                }

                List<FP_TOMSValidateDTO> lstResult = new List<FP_TOMSValidateDTO>();

                lstResult = FinalPackingBLL.ValidateBatch_TOMS(param_SerialNO);

                if (lstResult != null && lstResult.Count() > 0)
                {

                    foreach (var item in lstResult)
                    {
                        if (item.Status.Equals(1) || item.Status.Equals(2))
                        {
                            reg_SerialNo.Add(item.SerialNo);
                            FloorSystemException floorException = new FloorSystemException("FP_TomsValidation", "FinalPacking", new Exception(String.Format("Registration Removed in TOMS for {0}", item.SerialNo)), true);
                            CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ScanBatchInnerOuter", param_SerialNO);
                        }
                        else if (item.Status.Equals(3))
                        {
                            scanout_SerialNo.Add(item.SerialNo);
                            FloorSystemException floorException = new FloorSystemException("FP_TomsValidation", "FinalPacking", new Exception(String.Format("Batch ({0}) has not been scanned out from TOMS", item.SerialNo)), true);
                            CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ScanBatchInnerOuter", param_SerialNO);
                        }
                    }
                    if (lstResult.Any(x => datagridLst.Any(y => y.Equals(x.SerialNo)) && (x.Status.Equals(1) || x.Status.Equals(2))))
                    {
                        GlobalMessageBox.Show(String.Format("The Batch Card with serial no {0} has been registered in TOMS. Please remove from TOMS before proceed to Final Packing",
                        String.Join(",", reg_SerialNo)), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                    else if (lstResult.Any(x => datagridLst.Any(y => y.Equals(x.SerialNo)) && (x.Status.Equals(3))))
                    {
                        GlobalMessageBox.Show(String.Format("The Batch Card with serial no {0} has been scan in/inventorized in TOMS. Please scan out from TOMS before proceed to Final Packing",
                        String.Join(",", scanout_SerialNo)), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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
                CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ScanMultiBatch", param_SerialNO);
                proceedPacking = true;
            }
            return proceedPacking;
        }

        /// <summary>
        /// Clear if invalid data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPalletId_Leave(object sender, EventArgs e)
        {
            if (!cmbPalletId.Items.Contains(cmbPalletId.Text.ToUpper()) && cmbPalletId.Text != String.Empty)
            {
                cmbPalletId.Text = String.Empty;
                cmbPalletId.Focus();
            }

        }

        /// <summary>
        /// Clear if Invalid data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPreShipPltId_Leave(object sender, EventArgs e)
        {
            if (!cmbPreShipPltId.Items.Contains(cmbPreShipPltId.Text.ToUpper()) && cmbPreShipPltId.Text != String.Empty)
            {
                cmbPreShipPltId.Text = String.Empty;
                cmbPreShipPltId.Focus();
            }
        }

        private LineClearanceLogDTO GetLineClearanceLogObject(string workStationID)
        {
            LineClearanceLogDTO obj = new LineClearanceLogDTO();
            obj = FinalPackingBLL.GetLineClearanceLog(workStationID);
            return obj;
        }

        /// <summary>
        /// REQUIRED FIELD VALIDATION
        /// </summary>
        /// <returns>returns true if valid</returns>
        private Boolean ChangesOnRelatedField(LineClearanceLogDTO objLineClearanceLogDTO)
        {
            if (objLineClearanceLogDTO.PONumber != null && _poNumber != objLineClearanceLogDTO.PONumber
                || objLineClearanceLogDTO.Size != null && _size != objLineClearanceLogDTO.Size
                || objLineClearanceLogDTO.ItemNumber != null && _itemNumber != objLineClearanceLogDTO.ItemNumber
                || objLineClearanceLogDTO.PalletID != null && Convert.ToString(cmbPalletId.SelectedItem) != objLineClearanceLogDTO.PalletID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // #Azrul 13/07/2018: Merged from Live AX6 Start
        private void EnableDisabledField(Boolean flag)
        {
            cmbPurchaseOrder.Enabled = flag;
            cmbGroupId.Enabled = flag;
            cmbItemNumber.Enabled = flag;
            cmbItemSize.Enabled = flag;

        }
        // #Azrul 13/07/2018: Merged from Live AX6 End
    }
}
