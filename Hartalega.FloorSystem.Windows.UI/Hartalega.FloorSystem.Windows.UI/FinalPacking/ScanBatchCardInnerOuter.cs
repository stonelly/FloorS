using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.IntegrationServices;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ScanBatchCardInnerOuter : FormBase
    {
        # region PRIVATE VARIABLES
        private int _nextNumber = Constants.ZERO;
        private int _preshipmentPlan = Constants.ZERO; //To capture the Preshipment plan value for the selected Item.
        private int _itemcases = Constants.ZERO; //To capture the selected Item Cases.
        private string _itemNumber = string.Empty; //To capture the selected Item  Number.
        private int _casespacked = Constants.ZERO; //To capture the Cases packed per transaction.
        private int _casecapacity = Constants.ZERO; //To capture the outer Case Capacity.
        private int _palletcapacity = Constants.ZERO;//To capture the pallet Capacity.
        private int _innerBoxCapacity = Constants.ZERO; //To Capture the innerbox capacity.
        private string _innesetLayOut = string.Empty; //To capture the Inner Set Layout Number to print.
        private string _outersetLayOut = string.Empty; //To capture the Outer Set Layout Number to print.
        private int _BatchtotalPcs = Constants.ZERO; //To capture the selected Batch total pieces.
        private string MailBody = string.Empty; // #Azrul 13/07/2018: Merged from Live AX6

        private string _poNumber = string.Empty; //To capture the selected POnumber.
        private string _purchaseorderFormNum = string.Empty;
        private string _preshipmentRandomNumbers = string.Empty; //To capture the preshipment Random numbers generated for the selected Po Iem.
        private string _childpalletID = string.Empty; //To capture the palletID from new pallet window when pallet capacity has reached.        
        private string _size = string.Empty; //To Capture the Size details.
        private string _customerSize = string.Empty; //To Capture the Customer Size details.
        private string _Customer = string.Empty; // TODO: test barcode - Customer
        private string _ProductName = string.Empty; // TODO: test barcode - Product Name
        private string _productCode = string.Empty; // Inner Barcode
        private string _productCodeOuter = string.Empty; // Outer Barcode
        private string _VSFilePath = string.Empty; // Vision inner sharepath
        private bool _VisionSystemDown = false;
        private bool _VisionRecipeLoaded = false;
        private string _operatorName = String.Empty; //to capture the operator name of the split batch.
        private string _poReferenceNumber = String.Empty; //to capture PO reference number
        private string _screenname = "Print Inner and Outer Box Label"; //To use for DB logging.
        private string _uiClassName = "Print Inner and Outer Box Label"; //To use for DB logging.

        private string _GloveCode = string.Empty;
        private string _AlternativeGloveCode1 = string.Empty;
        private string _AlternativeGloveCode2 = string.Empty;
        private string _AlternativeGloveCode3 = string.Empty;
        private int _manufacturingOrderbasis = Constants.ZERO;
        private int _expiry = Constants.ZERO;
        private int _GCLabelPrintingRequired = Constants.ZERO;
        private string _customerLotNumber = string.Empty;
        private int _BarcodeVerificationRequired = Constants.ZERO;
        private DateTime? _receiptDateRequested = new DateTime();
        private DateTime? _shippingDateRequested = new DateTime();
        private DateTime? _ProductionDate = new DateTime();

        //Azman 2019-01-29 PODate & POReceived Date
        private DateTime? _custPODocumentDate = new DateTime();
        private DateTime? _custPORecvDate = new DateTime();
        //Azman 2019-01-29 PODate & POReceived Date
        //Pang 09/02/2020 First Carton Packing Date
        private DateTime _FirstManufacturingDate = new DateTime();

        private Boolean _isTempack = false; //To check is the selected batch is from Temppack inventory.  
        private string _LineId = string.Empty;
        private Boolean _PSIRequired = false;
        private Boolean _PSIChecked = false;
        private Boolean _isAssociated = false;
        DataTable dtPODetails = new DataTable(); // 

        //List<SOLineDTO> lstItemNumber = null;
        List<SOLineDTO> lstPODetails = null;
        List<SOLineDTO> lstItemDetails = null;
        List<SOLineDTO> listItemSize = null;
        SOLineDTO objItemNumber = null;
        SOLineDTO objItemSize = null;

        //KahHeng 26APr2019 add flag to identify 1st pallet is continued from previous same pallet
        private Boolean _isContinuedPallet = false;
        private Boolean _isContinuedPreshipmentPallet = false;
        private string _strMethodParameter = "";
        private DialogResult result;
        //Kahheng end
        #endregion

        #region CONSTRUCTOR
        public ScanBatchCardInnerOuter()
        {
            InitializeComponent();
        }
        # endregion

        #region FORM LOAD
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="e"></param>
        private void ScanBatchCardInnerOuter_Load(object sender, EventArgs e)
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
                txtBoxesPacked.BoxesPacked();
                GetActivePOList();
                //BindPackingGroup();
                cmbPurchaseOrder.Focus();
                txtSerialNumber.SerialNo();
                this.WindowState = FormWindowState.Maximized;
                cmbPreShipPltId.Enabled = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ScanBatchCardInnerOuter_Load", null);
            }
        }
        //private void BindPackingGroup()
        //{
        //    List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups(Constants.PACKING_GROUP_TYPE);
        //    cmbGroupId.BindComboBox(lstDropDown, true);
        //}
        #endregion

        #region EVENTS
        /// <summary>
        /// Insert Soline Data in Floor System
        /// </summary>
        private void UpdateItemSizedetails()
        {
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

            _expiry = objItemSize.Expiry;
            _GCLabelPrintingRequired = objItemSize.GCLabelPrintingRequired;
            _customerLotNumber = objItemSize.CustomerLotNumber;
            _BarcodeVerificationRequired = objItemSize.BarcodeVerificationRequired;
            _purchaseorderFormNum = objItemSize.OrderNumber;

            _Customer = objItemSize.CustomerName;
            _ProductName = objItemSize.ItemName;
            _productCode = objItemSize.BARCODE;
            _productCodeOuter = objItemSize.BARCODEOUTERBOX;
            _VSFilePath = objItemSize.VSReceiptFilePath;

            UpdatePODetails(objItemSize);
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
                    _customerSize = objPODetails.CustomerSize;
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
            bool listAllAssociatedPallet = false;
            _PSIRequired = ValidatePreshipementPalletCase(_poNumber, _itemNumber, Convert.ToInt32(txtBoxesPacked.Text) / _casecapacity, _size);
            _PSIChecked = false;

            try
            {
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
                            listAllAssociatedPallet = true;
                        }
                    }
                    if (!_PSIRequired)
                    {
                        associatedPreshipmentPLTID = string.Empty;

                    }
                    if (string.IsNullOrEmpty(associatedPalletId))
                    {
                        if (!listAllAssociatedPallet)
                        {
                            PopulatePallet(associatedPalletId);
                        }
                        else
                        {
                            PopulatePallet(associatedPalletId, listAllAssociatedPallet, _poNumber, _itemNumber, _size);
                        }
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

                    PopulatePReshipmentPLTID(associatedPreshipmentPLTID);
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
        /// PRINT CLICK EVENT TO SAVE AND PRINT THE TRANSACTION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Print_Click(object sender, EventArgs e)
        {
            #region FP Vision Test Code
            //VisionLotVerification hlTest = new VisionLotVerification(new ValidateVision
            //{
            //    FGCode = "FG-HLYD-0015",
            //    Customer = "Halyard Customer",
            //    ProductName = "BASICS BLUE PF NITRILE",
            //    Size = "S",
            //    LotNumber = "SJ013915X_0288",
            //    ExpiryDate = Convert.ToDateTime("2022-06-23"),
            //    ProductCode = "(01)20680651507062"
            //},
            //new ValidateVision
            //{
            //    FGCode = "FG-HLYD-0015",
            //    Customer = "Halyard Customer",
            //    ProductName = "BASICS BLUE PF NITRILE",
            //    Size = "S",
            //    LotNumber = "SJ013915X_0288",
            //    ExpiryDate = Convert.ToDateTime("2022-06-23"),
            //    ProductCode = "(01)20680651507063"
            //}
            //, "SO123", "TEST 999/20", 10, 10);
            //hlTest.ShowDialog();
            //return;

            #endregion

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
            decimal serialNumber = Constants.ZERO;
            string internallotno = string.Empty;
            string _palletid = string.Empty;
            string _palletidConcated = string.Empty;
            string _preshipmentPltID = string.Empty;
            int boxespacked = Constants.ZERO;
            int selectedPalletCasesPlaced = Constants.ZERO;
            int selectedPreshipPalletCasesPlaced = Constants.ZERO;
            int rowsaffected = Constants.ZERO;
            //KahHeng 25Apr2019
            //String for concatinated preshipmentPalletID
            string _preshipmentPltIDConcated = string.Empty;

            FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
            OuterLabelDTO objOuterLabelDTO = new OuterLabelDTO();
            FinalPackingBatchInfoDTO objFinalPackingBatchInfoDTO = new FinalPackingBatchInfoDTO();

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
                {   //#MK 23/03/2019 Validate Inner Box against FGBatchOrder
                    if (ValidateInnerBoxes() && ValidateBoxesPacked())  // Validate Boxes entered on Print
                    {
                        // FPVIsion call API down
                        if (_VisionSystemDown)
                        {
                            MessageBox.Show(Messages.FPVISION_VSAPI_DOWN, "Vision System Connect Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //Vinoden : FP Validation against TOMS
                        string[] serialNo = new string[] { txtSerialNumber.Text.Trim() };
                        if (ValidateBatchCard_TOMS(serialNo))
                        {
                            if (SerilaNumberValidation(Convert.ToDecimal(txtSerialNumber.Text), true)) // Validate SerialNumber scanned on print
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
                                        LCV.Location = new Point(txtstation.Location.X + 190, tableLayoutPanel1.Location.Y);
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
                                        int lastRunningNo = 0;
                                        if (FinalPackingBLL.CheckDuplicateInternalLotNumber(txtInternalLotNumber.Text,
                                                WorkStationDataConfiguration.GetInstance().stationNumber,
                                                out lastRunningNo))
                                        {
                                            txtInternalLotNumber.Text =
                                                GetInternalLotNumber(); // Get latest/updated internal lot number 
                                        }

                                        boxespacked = int.Parse(txtBoxesPacked.Text);
                                        serialNumber = Convert.ToDecimal(txtSerialNumber.Text);
                                        internallotno = txtInternalLotNumber.Text;
                                        _palletid = Convert.ToString(cmbPalletId.SelectedItem);
                                        _preshipmentPltID = Convert.ToString(cmbPreShipPltId.SelectedItem);
                                        _casespacked = Convert.ToInt32(txtBoxesPacked.Text) / _casecapacity;
                                        objFinalPackingDTO.TotalPcs = objFinalPackingDTO.Casespacked * _casecapacity * _innerBoxCapacity;
                                        GetFinalPackingObject(serialNumber, internallotno, boxespacked, objFinalPackingDTO);
                                        objFinalPackingDTO.Palletid = _palletid;
                                        objFinalPackingDTO.Casespacked = _casespacked;
                                        objFinalPackingDTO.PreshipmentPLTId = _preshipmentPltID;
                                        objFinalPackingBatchInfoDTO.SerialNumber = serialNumber;
                                        objFinalPackingBatchInfoDTO.BoxesPacked = boxespacked;
                                        objFinalPackingBatchInfoDTO.CasesPacked = _casespacked;
                                        objFinalPackingDTO.InventTransId = objItemSize.InventTransId;
                                        objFinalPackingDTO.FPStationNo = txtstation.Text; //added by Azman 22/08/2019
                                        objFinalPackingDTO.FGBatchOrderNo = cmbFGBOList.Text;
                                        objFinalPackingDTO.Resource = FinalPackingBLL.GetResourceFromBatchOrder(objFinalPackingDTO.FGBatchOrderNo);
                                        rowsaffected = InsertFinalPacking(objFinalPackingDTO, objFinalPackingBatchInfoDTO);
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

                                        bool isFPPostingSuccess = true;

                                        if (rowsaffected > Constants.ZERO)
                                        {
                                            try
                                            {
                                                while (palletCasesCount > Constants.ZERO)
                                                {
                                                    if (palletCasesCount > _palletcapacity - selectedPalletCasesPlaced)
                                                    {
                                                        //_palletid = string.Empty;
                                                        if ((_palletid == string.Empty) || (_palletid == "") || (_palletid.Length < 7))
                                                        {
                                                            throw new Exception("Pallet Id is empty");
                                                        }

                                                        _palletidConcated = _palletidConcated + _palletid + "!!!";

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
                                                        if (_PalletSelectionForm.IsCancel)
                                                        {
                                                            txtInternalLotNumber.Text = GetInternalLotNumber();
                                                            FinalPackingBLL.RollBackPrintInnerOuterTransaction(objFinalPackingDTO.Internallotnumber, _palletidConcated, _preshipmentPltIDConcated,
                                                                                                            objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size,
                                                                                                            _isContinuedPallet, _isContinuedPreshipmentPallet, Environment.UserName, "Cancel pallet selection");
                                                            FinalPackingBLL.RollBackEWNCaseQty(objFinalPackingDTO.Ponumber, objFinalPackingDTO.Palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                                                            ClearForm();
                                                            cmbPurchaseOrder.Focus();
                                                            return;
                                                        }
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

                                                        _palletidConcated = _palletidConcated + _palletid;

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
                                                        if (_PalletSelectionForm.IsCancel)
                                                        {
                                                            txtInternalLotNumber.Text = GetInternalLotNumber();
                                                            FinalPackingBLL.RollBackPrintInnerOuterTransaction(objFinalPackingDTO.Internallotnumber, _palletidConcated, _preshipmentPltIDConcated,
                                                                                                            objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size,
                                                                                                            _isContinuedPallet, _isContinuedPreshipmentPallet, Environment.UserName, "Cancel pallet selection");
                                                            FinalPackingBLL.RollBackEWNCaseQty(objFinalPackingDTO.Ponumber, objFinalPackingDTO.Palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                                                            ClearForm();
                                                            cmbPurchaseOrder.Focus();
                                                            return;
                                                        }
                                                        if (!string.IsNullOrEmpty(_PalletSelectionForm.childPalletId))
                                                        {
                                                            _preshipmentPltID = objFinalPackingDTO.PreshipmentPLTId = Convert.ToString(_PalletSelectionForm.childPalletId);
                                                            selectedPreshipPalletCasesPlaced = Constants.ZERO;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //KahHeng (26Apr2019)
                                                        //To store the multiple preshipment PalletID
                                                        _preshipmentPltIDConcated = _preshipmentPltIDConcated + _preshipmentPltID + "!!!";
                                                        //End Edit (26Apr2019)
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

                                                //// TODO: remove it
                                                //GlobalMessageBox.Show("Simualte time out", Constants.AlertType.Error, "Let's disconnect network now. Once done, click ok button", GlobalMessageBoxButtons.OK);

                                                // Update SpecialInternalLotnumber, Mfg Date and Exp Date
                                                string specialLotNumber = FinalPackingBLL.UpdatespeciallotnumberforInner(objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.ManufacturingDate.Value, objFinalPackingDTO.ExpiryDate.Value,
                                                    _LineId, _expiry, _innesetLayOut, _customerLotNumber);

                                                FinalPackingBLL.UpdatespeciallotnumberforOuter(objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.ManufacturingDate.Value, objFinalPackingDTO.ExpiryDate.Value,
                                                     _LineId, _expiry, _outersetLayOut, _customerLotNumber, _shippingDateRequested.Value, specialLotNumber, _innesetLayOut);

                                                //KahHeng (26Apr2019)
                                                //To store the multiple preshipment PalletID
                                                if (preshipmentPalletCasesCount == 0) { _preshipmentPltIDConcated = _preshipmentPltID; }
                                                //End Edit (26Apr2019)
                                                Console.WriteLine("This is concated" + _preshipmentPltIDConcated);


                                                if (!string.IsNullOrEmpty(specialLotNumber))
                                                {
                                                    objFinalPackingDTO.Internallotnumber = specialLotNumber;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                isFPPostingSuccess = false;
                                                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                                //// TODO: remove it
                                                //GlobalMessageBox.Show("Simualte time out", Constants.AlertType.Error, "Let's connect network now. Once done, click ok button", GlobalMessageBoxButtons.OK);
                                                txtInternalLotNumber.Text = GetInternalLotNumber();
                                                //KahHeng 26Apr2019 Added new parameters to the function
                                                FinalPackingBLL.RollBackPrintInnerOuterTransaction(objFinalPackingDTO.Internallotnumber, _palletidConcated, _preshipmentPltIDConcated,
                                                                                                           objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size,
                                                                                                           _isContinuedPallet, _isContinuedPreshipmentPallet, Environment.UserName, "Exception error");
                                                _isContinuedPallet = false;
                                                _isContinuedPreshipmentPallet = false;
                                                _strMethodParameter = objFinalPackingDTO.Internallotnumber + ", " + _palletidConcated + ", " + _preshipmentPltIDConcated + ", " +
                                                                        objFinalPackingDTO.Ponumber + ", " + objFinalPackingDTO.ItemNumber + ", " + objFinalPackingDTO.Size;
                                                //End Edit 26Apr2019
                                                FinalPackingBLL.RollBackEWNCaseQty(objFinalPackingDTO.Ponumber, objFinalPackingDTO.Palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                                                FloorSystemException fsexp = new FloorSystemException(ex.Message, "", ex);
                                                //KahHeng 26Apr2019
                                                //CommonBLL.LogExceptionToDB(fsexp, _screenname, _uiClassName, "Print_Click", null);
                                                CommonBLL.LogExceptionToDB(fsexp, _screenname, _uiClassName, "Print_Click", _strMethodParameter);
                                                MessageBox.Show(Messages.INNEROUTERROLLBACKERROR, "Print Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                                //End Edit 26Apr2019
                                            }

                                            if (isFPPostingSuccess)
                                            {
                                                // No need check AX posting status for D365
                                                //bool isPostingSucess = PostAXData(objFinalPackingDTO.Internallotnumber);
                                                //if (isPostingSucess)
                                                //{                                            

                                                // fp exempt with RWKCR, need to post RWKDEL
                                                if (QAIBLL.GetLastServiceName(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == ReworkOrderFunctionidentifier.RWKCR &&
                                                    FinalPackingBLL.ValidateSerialNoByFPExempt(Convert.ToDecimal(txtSerialNumber.Text.Trim())) == Constants.PASS)
                                                {
                                                    AXPostingBLL.PostAXDataDeleteReworkOrderPTQIFailPT(txtSerialNumber.Text.Trim());
                                                }

                                                //Call Printer Interface methods
                                                objOuterLabelDTO = FinalPackingBLL.PrintInnerandOuter(objFinalPackingDTO.Internallotnumber, _GCLabelPrintingRequired == Constants.ONE ? true : false, _BarcodeVerificationRequired == Constants.ONE ? true : false);

                                                // Pang 09/02/2020 First Carton Packing Date
                                                FinalPackingBLL.UpdateFirstCartonPackingDate(objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);

                                                int POStatusRow = FinalPackingBLL.UpdatePOStatus(objFinalPackingDTO.Ponumber);

                                                if (FinalPackingBLL.GetPreshipmentCasesQuantitytoPackforPO(objFinalPackingDTO.Ponumber) == Constants.ZERO)
                                                {
                                                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                                                    // 2018-02-13 Azman START
                                                    // Disable roll back if preshipment email fail
                                                    if (!FinalPackingBLL.isPreshipmentQAEmailSent(objFinalPackingDTO.Ponumber))
                                                    {
                                                        try
                                                        {
                                                            // create MailContent in case of any error appear
                                                            List<SOLineDTO> lstPurchaseorderitem = FinalPackingBLL.GetPOPreshipmentcases(objFinalPackingDTO.Ponumber);
                                                            MailBody = FinalPackingBLL.GetPreshipmentQAEmailBodyContent(objFinalPackingDTO.Ponumber, _purchaseorderFormNum, cmbPreShipPltId.Text, txtItemName.Text, WorkStationDTO.GetInstance().Location, lstPurchaseorderitem);

                                                            FinalPackingBLL.SendPreshipmentQAEmail(objFinalPackingDTO.Ponumber, _purchaseorderFormNum, objFinalPackingDTO.ItemNumber,
                                                                txtItemName.Text, objFinalPackingDTO.Size, objFinalPackingDTO.Internallotnumber, cmbPreShipPltId.Text);
                                                        }
                                                        catch (FloorSystemException ex)
                                                        {
                                                            ExceptionLoggingWithoutPopup(ex, _screenname, _uiClassName, "Print_Click", MailBody);
                                                        }
                                                        catch (Exception genEx)
                                                        {
                                                            FloorSystemException fsexp = new FloorSystemException(genEx.Message, "", genEx);
                                                            ExceptionLoggingWithoutPopup(fsexp, _screenname, _uiClassName, "Print_Click", MailBody);
                                                        }
                                                    }
                                                    // 2018-02-13 Azman END
                                                    // #Azrul 13/07/2018: Merged from Live AX6 End
                                                }

                                                // Insert LineClearanceLog
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

                                                //KahHeng 26Apr2019 to reset the variable back to false value
                                                _isContinuedPallet = false;
                                                _isContinuedPreshipmentPallet = false;
                                                // End KahHeng 26Apr2019
                                            }
                                            if (isFPPostingSuccess == true)
                                            {
                                                if (_BarcodeVerificationRequired == Constants.ONE)
                                                {
                                                    FinalPackingBLL.UpdateBarcodeToValidate(objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, objFinalPackingDTO.Internallotnumber);
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
                                                else if (_BarcodeVerificationRequired == Constants.THREE && WorkStationDataConfiguration.GetInstance()._FPVisionValidation == true && _VisionRecipeLoaded)
                                                {
                                                    // FP vision: handle outer barcode generated on the during outer label print: cGs1
                                                    var _dynamicproductCodeOuter = _productCodeOuter;
                                                    if (string.IsNullOrEmpty(_productCodeOuter))
                                                        _dynamicproductCodeOuter = objOuterLabelDTO.outerBarcode_cGs1;


                                                    //VisionLotVerification hl = new VisionLotVerification(new ValidateVision
                                                    //{
                                                    //    FGCode = objFinalPackingDTO.ItemNumber,
                                                    //    Customer = _Customer,
                                                    //    ProductName = _ProductName,
                                                    //    Size = objFinalPackingDTO.Size,
                                                    //    LotNumber = objFinalPackingDTO.Internallotnumber,
                                                    //    ExpiryDate = objFinalPackingDTO.ExpiryDate ?? DateTime.MinValue,
                                                    //    ProductCode = _productCode
                                                    //},
                                                    //new ValidateVision
                                                    //{
                                                    //    FGCode = objFinalPackingDTO.ItemNumber,
                                                    //    Customer = _Customer,
                                                    //    ProductName = _ProductName,
                                                    //    Size = objFinalPackingDTO.Size,
                                                    //    LotNumber = objFinalPackingDTO.Internallotnumber,
                                                    //    ExpiryDate = objFinalPackingDTO.ExpiryDate ?? DateTime.MinValue,
                                                    //    ProductCode = _dynamicproductCodeOuter
                                                    //},
                                                    //_poNumber, _poReferenceNumber, int.Parse((txtBoxesPacked.Text.Trim())), _casespacked);
                                                    //hl.ShowDialog();
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                                txtInternalLotNumber.Text = GetInternalLotNumber();
                                ClearForm();
                                cmbPurchaseOrder.Focus();
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
                FinalPackingBLL.RollBackEWNCaseQty(objFinalPackingDTO.Ponumber, objFinalPackingDTO.Palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                //KahHeng 26Apr2019
                //Added new parameters to the function
                //FinalPackingBLL.RollBackPrintInnerOuterTransaction(objFinalPackingDTO.Internallotnumber);
                FinalPackingBLL.RollBackPrintInnerOuterTransaction(objFinalPackingDTO.Internallotnumber, _palletidConcated, _preshipmentPltIDConcated,
                                                                                                   objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size,
                                                                                                   _isContinuedPallet, _isContinuedPreshipmentPallet, Environment.UserName, "Exception error");
                _isContinuedPallet = false;
                _isContinuedPreshipmentPallet = false;
                _strMethodParameter = objFinalPackingDTO.Internallotnumber + ", " + _palletidConcated + ", " + _preshipmentPltIDConcated + ", " +
                                      objFinalPackingDTO.Ponumber + ", " + objFinalPackingDTO.ItemNumber + ", " + objFinalPackingDTO.Size;
                //ExceptionLogging(ex, _screenname, _uiClassName, "Print_Click", null);
                ExceptionLogging(ex, _screenname, _uiClassName, "Print_Click", _strMethodParameter);
                //MessageBox.Show("Inner and Outer Label print failed. Please redo the process. ", "Print Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(Messages.INNEROUTERROLLBACKERROR, "Print Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //End Edit 26Apr2019
            }
            catch (Exception genEx)
            {
                FloorSystemException fsexp = new FloorSystemException(genEx.Message, "", genEx);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "Print_Click", null);
            }

            if (isPass && isComplete)
            {
                #region lotnumber validation
                if (_BarcodeVerificationRequired == Constants.ONE && WorkStationDataConfiguration.GetInstance()._skipBarcodeValidation == false)
                {
                    FinalPackingBLL.UpdateBarcodeToValidate(objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, objFinalPackingDTO.Internallotnumber);
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
                        TotalInnerbox = Convert.ToInt32(txtBoxesPacked.Text),
                        PalletID = objFinalPackingDTO.Palletid
                    };
                    ph.ShowDialog();
                }
                #endregion
                ClearForm();
                cmbPurchaseOrder.Focus();
            }

            btnPrint.Enabled = true;
        }

        private void GetFinalPackingObject(decimal serialNumber, string internallotno, int boxespacked, FinalPackingDTO objFinalPackingDTO)
        {
            objFinalPackingDTO.locationId = WorkStationDTO.GetInstance().LocationId;
            objFinalPackingDTO.Workstationnumber = WorkStationDTO.GetInstance().WorkStationId;
            objFinalPackingDTO.stationNumber = WorkStationDataConfiguration.GetInstance().stationNumber;
            objFinalPackingDTO.Printername = WorkStationDataConfiguration.GetInstance().innerPrinter;
            objFinalPackingDTO.Packdate = CommonBLL.GetCurrentDateAndTimeFromServer();
            objFinalPackingDTO.Internallotnumber = internallotno;
            objFinalPackingDTO.Outerlotno = internallotno;
            objFinalPackingDTO.Ponumber = _poNumber;
            objFinalPackingDTO.ItemNumber = _itemNumber;
            objFinalPackingDTO.Size = _size;
            objFinalPackingDTO.Serialnumber = serialNumber;
            objFinalPackingDTO.Boxespacked = boxespacked;
            //objFinalPackingDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
            objFinalPackingDTO.Innersetlayout = _innesetLayOut;
            objFinalPackingDTO.Outersetlayout = _outersetLayOut;
            objFinalPackingDTO.palletCapacity = _palletcapacity;
            objFinalPackingDTO.TotalPcs = boxespacked * _innerBoxCapacity;
            objFinalPackingDTO.isTempPack = _isTempack;
            objFinalPackingDTO.ManufacturingDate = FinalPackingBLL.GetManufacturingDate(_manufacturingOrderbasis, _shippingDateRequested, _receiptDateRequested, _ProductionDate, _custPODocumentDate, _custPORecvDate, _FirstManufacturingDate);
            objFinalPackingDTO.ExpiryDate = objFinalPackingDTO.ManufacturingDate.Value.AddMonths(_expiry);
        }

        //add by Cheah (17Aug2017)
        private void InsertDataToEWNTable(string _palletid, bool isPreship, FinalPackingDTO objFinalPackingDTO)
        {
            if (isPreship)
            {
                if (FinalPackingBLL.ValidatePreshipmentPalletCapacity(_palletid, _poNumber) == _palletcapacity)
                    //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                    FinalPackingBLL.InsertEWareNaviData(_palletid, isPreship, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size, _customerSize);
            }
            else
            {
                if (FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size) == _palletcapacity)
                    //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                    FinalPackingBLL.InsertEWareNaviData(objFinalPackingDTO.Palletid, isPreship, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size, _customerSize);
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

        /// <summary>
        /// BOXES PACKED LEAVE EVENT TO ROUND TO CASE QUANTITY
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBoxespacked_Leave(object sender, EventArgs e)
        {
            if (ValidateInnerBoxes() && ValidateBoxesPacked())
            {
                PopulateAssociatedPalletID(); //Populate Associated Pallet and Preshipment Pallet.

                if (_isAssociated && !_PSIChecked)
                {
                    string associatedPreshipmentPLTID = string.Empty;
                    associatedPreshipmentPLTID = Convert.ToString(cmbPreShipPltId.SelectedItem);
                    PopulatePReshipmentPLTID(associatedPreshipmentPLTID);
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
            Boolean isValid = false;
            _VisionSystemDown = false;

            if (!string.IsNullOrEmpty(txtSerialNumber.Text))
            {
                String[] CustRefPO = cmbPurchaseOrder.Text.ToString().Split('|'); //#AZRUL 29/01/2019: SO Field Validation.
                _poReferenceNumber = CustRefPO[0].ToString().Trim();
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
                    GlobalMessageBox.Show(Messages.INVALID_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
                    else
                    {
                        //#AZRUL 04/03/2019: SO Field Validation temp error msg, to check live sp
                        GlobalMessageBox.Show("Validate FG Label fail, error code:" + r.ToString());
                    }

                    ClearForm();
                }
                //#AZRUL 29/01/2019 END
                else if (FinalPackingBLL.isSecondGradeBatch(Convert.ToDecimal(txtSerialNumber.Text.Trim())))
                {
                    GlobalMessageBox.Show(Messages.SECONDGRADE_SNO_FP, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
                //#AZRUL 29-10-2018 END
                else
                {
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

                    string batchcardsize = FinalPackingBLL.GetBatchCardSize(Convert.ToDecimal(txtSerialNumber.Text.Trim()));
                    foreach (SOLineDTO itemsize in listItemSize)
                    {
                        if (itemsize.ItemSize == batchcardsize)
                        {
                            isValid = true;
                            objItemSize = itemsize;
                            UpdateItemSizedetails();
                        }
                    }
                    if (isValid)
                        SerilaNumberValidation(Convert.ToDecimal(txtSerialNumber.Text.Trim()));
                    else
                    {
                        StringBuilder strPOItemSize = new StringBuilder();
                        foreach (SOLineDTO itemsize in listItemSize)
                        {
                            strPOItemSize.Append(itemsize.ItemSize);
                            strPOItemSize.Append(Constants.COMMA);
                        }
                        GlobalMessageBox.Show(string.Format(Messages.SELCORRECT_PO, batchcardsize, strPOItemSize.ToString().TrimEnd(Constants.CHARCOMMA)), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        txtSerialNumber.Text = string.Empty;
                        txtSerialNumber.Focus();
                    }
                }
            }
            else
            {
                txtBatch.Text = String.Empty;
                cmbFGBOList.DataSource = null;
                cmbFGBOList.Items.Clear();
            }


            //If isValid and VisionFlag on
            //if (isValid && WorkStationDataConfiguration.GetInstance()._FPVisionValidation == true && _BarcodeVerificationRequired == Constants.THREE)
            //{
            //    string _visionHostUrl = WorkStationDataConfiguration.GetInstance().VisionHostUrl;
            //    if (!string.IsNullOrEmpty(_visionHostUrl))
            //        if (_visionHostUrl.ToUpper() == "FALSE")
            //            _visionHostUrl = string.Empty;

            //    // Inner receipt API call
            //    if (string.IsNullOrEmpty(_visionHostUrl))
            //    {
            //        GlobalMessageBox.Show(Messages.FPVISION_VSAPI_NOT_CONFIGURE, Constants.AlertType.Information);
            //    }
            //    else if (string.IsNullOrEmpty(_VSFilePath))
            //    {
            //        GlobalMessageBox.Show(Messages.FPVISION_VSRECIPE_NOT_CONFIGURE, Constants.AlertType.Information);
            //    }
            //    else
            //    {
            //        _VisionRecipeLoaded = FinalPackingExtBLL.CallVisionReceiptAPI(_visionHostUrl, _itemNumber, _size, _VSFilePath);
            //        if (!_VisionRecipeLoaded) // reset
            //        {
            //            _VisionSystemDown = true;
            //            txtSerialNumber.Text = string.Empty;
            //            txtSerialNumber.Focus();
            //        }
            //    }
            //}

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

        #endregion

        #region USER METHODS
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
        /// VALIDATE SERIAL NUMBER ENTERED 
        /// </summary>
        private Boolean SerilaNumberValidation(decimal serialNumber, Boolean isfromPrint = false)
        {
            BatchDTO objBatchDTO = new BatchDTO();
            FPTempPackDTO objFPTempPackDTO = new FPTempPackDTO();
            string batchNumber = string.Empty;
            Boolean blnIsValid = false;
            try
            {
                if (ValidatePORequiredFields()) //Validate Required fields serial number to scan
                {
                    if (Validator.IsValidInput(Constants.ValidationType.Integer, txtSerialNumber.Text.Trim()))// validate entered serialnumber is integer
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
                                //    && ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.POWDER_TEST)
                                //    && ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.PROTEIN_TEST)
                                //    && ValidateQATestResult(Convert.ToDecimal(txtSerialNumber.Text.Trim()), Constants.HOTBOX_TEST))
                                //{

                                //validate whether batch scan out from glove inventory system
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
                                        objBatchDTO = FinalPackingBLL.GetBatchBySerialNoAndSalesOrder(Convert.ToDecimal(txtSerialNumber.Text.Trim()), _GloveCode, _size, _AlternativeGloveCode1, _AlternativeGloveCode2, _AlternativeGloveCode3, _innerBoxCapacity, cmbPurchaseOrder.Text.FPCustomerRefernceSplit(), cmbItemNumber.Text);
                                        batchNumber = objBatchDTO.BatchNumber;
                                        _ProductionDate = objBatchDTO.BatchCarddate;
                                        _isTempack = true;
                                        _LineId = objBatchDTO.Line;
                                    }
                                    else
                                    {
                                        objBatchDTO = FinalPackingBLL.GetBatchBySerialNoAndSalesOrder(Convert.ToDecimal(txtSerialNumber.Text.Trim()), _GloveCode, _size, _AlternativeGloveCode1, _AlternativeGloveCode2, _AlternativeGloveCode3, _innerBoxCapacity, cmbPurchaseOrder.Text.FPCustomerRefernceSplit(), cmbItemNumber.Text);
                                        batchNumber = objBatchDTO.BatchNumber;
                                        _ProductionDate = objBatchDTO.BatchCarddate;
                                        _LineId = objBatchDTO.Line;
                                        _isTempack = false;
                                    }
                                    //if (isfromPrint == true) // if the validation is from Print then only check for issplitbatch.
                                    if ((objBatchDTO.IsFPBatchSplit || objFPTempPackDTO.TMPPackBatch.IsFPBatchSplit) && isfromPrint == true)
                                    {
                                        if (ValidateSplitBatch())
                                        {
                                            //Azrul 20190416: Prevent overwrite selected FGBO when save.
                                            if (btnPrint.Enabled)
                                            {
                                                if (!string.IsNullOrEmpty(batchNumber))
                                                {
                                                    txtBatch.Text = batchNumber;
                                                    //Max He,20/9/2018,Bind Active FG Batch(Production) Order list
                                                    cmbFGBOList.BindComboBox(objBatchDTO.BatchOrders, false);
                                                }
                                                else
                                                {
                                                    GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                    txtSerialNumber.Clear();
                                                    txtSerialNumber.Focus();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        //Azrul 20190416: Prevent overwrite selected FGBO when save.
                                        if (btnPrint.Enabled)
                                        {
                                            if (!string.IsNullOrEmpty(batchNumber))
                                            {
                                                txtBatch.Text = batchNumber;
                                                //Max He,20/9/2018,Bind Active FG Batch(Production) Order list
                                                cmbFGBOList.BindComboBox(objBatchDTO.BatchOrders, false);
                                            }
                                            else
                                            {
                                                GlobalMessageBox.Show(Messages.INVALID_GLOVECODE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                                txtSerialNumber.Clear();
                                                txtSerialNumber.Focus();
                                            }
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
                            txtSerialNumber.Clear();
                            txtSerialNumber.Focus();
                            blnIsValid = false;
                            txtSerialNumber.Focus();
                        }
                    }
                    else
                    {
                        txtSerialNumber.Clear();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "SerilaNumberValidation", null);
            }
            return blnIsValid;
        }

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
        /// #MK 23/03/2019 1.n Get the FG Batch Order Line Details and Validate Inner Box
        /// </summary>
        private Boolean ValidateInnerBoxes()
        {
            int boxesentered = Constants.ZERO;
            int remainingQty = Constants.ZERO;
            Boolean blnIsValid = false;
            List<GloveBatchOrderDTO> dtBatchOrderDetails = null;
            try
            {
                if (!String.IsNullOrEmpty(txtBoxesPacked.Text))
                {
                    txtBoxesPacked.Text = FinalPackingBLL.RoundToCaseCapacity(txtBoxesPacked.Text, _casecapacity);
                    boxesentered = int.Parse((txtBoxesPacked.Text.Trim()));

                    dtBatchOrderDetails = FinalPackingBLL.GetFGBORemainingQty(cmbFGBOList.Text, _size);

                    if (dtBatchOrderDetails != null)
                    {
                        if (dtBatchOrderDetails.Count > 0)
                        {
                            foreach (var dt in dtBatchOrderDetails)
                            {
                                remainingQty = int.Parse(dt.RemainingQty.Replace(",", ""));
                                if (remainingQty > 0)
                                {
                                    if (boxesentered > (remainingQty * _casecapacity))
                                    {
                                        GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((remainingQty * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK));
                                        txtBoxesPacked.Text = Convert.ToString((remainingQty * _casecapacity));
                                        txtBoxesPacked.Focus();
                                    }
                                    else
                                        blnIsValid = true;
                                }
                                else
                                {
                                    GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((remainingQty * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK));
                                    txtBoxesPacked.Text = Convert.ToString((remainingQty * _casecapacity));
                                    txtBoxesPacked.Focus();
                                }
                            }
                        }
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
                    if (Convert.ToInt64(txtBoxesPacked.Text) < _casecapacity)
                    {
                        GlobalMessageBox.Show(string.Format(Messages.BOXES_LESSTHANCASECAPACITY, _casecapacity), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtBoxesPacked.Clear();
                        txtBoxesPacked.Focus();
                    }
                    else
                    {
                        txtBoxesPacked.Text = FinalPackingBLL.RoundToCaseCapacity(txtBoxesPacked.Text, _casecapacity);
                        boxesentered = int.Parse((txtBoxesPacked.Text.Trim()));
                        if (boxesentered > Constants.ZERO)
                        {
                            if (!string.IsNullOrEmpty(txtSerialNumber.Text.Trim()))
                            {
                                //Validate the boxes packed text is integer.
                                if (Validator.IsValidInput(Framework.Constants.ValidationType.Integer, txtBoxesPacked.Text))
                                {
                                    POItemSizecasespacked = FinalPackingBLL.ValidatePOSizeBoxesPacked(_poNumber, _itemNumber, _size);
                                    //validate boxes entered and boxes already packed is not greater than the requied 
                                    if (!ValidateRequiredItemCases(POItemSizecasespacked, boxesentered))
                                    {
                                        GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((_itemcases - POItemSizecasespacked) * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                                        txtBoxesPacked.Text = Convert.ToString((_itemcases - POItemSizecasespacked) * _casecapacity);
                                        txtBoxesPacked.Focus();
                                    }
                                    else if (!ValidateBatchCapacity(Convert.ToDecimal(txtSerialNumber.Text), (boxesentered)))//validate selected batch capacity
                                    {
                                        GlobalMessageBox.Show(string.Format(Messages.BATCHCAPACITY, _BatchtotalPcs / _innerBoxCapacity, _innerBoxCapacity, Environment.NewLine), Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                        txtBoxesPacked.Text = Convert.ToString(_BatchtotalPcs / _innerBoxCapacity);
                                        txtBoxesPacked.Focus();
                                    }
                                    else
                                    { blnIsValid = true; } // return true if no validation failed
                                }
                                else
                                {
                                    if (btnPrint.Focused)
                                    {
                                        string validFieldMessage = Messages.INVALID_DATA_SUMMARY;
                                        validFieldMessage = validFieldMessage + Messages.REQBOXESPACKED + Environment.NewLine;
                                        GlobalMessageBox.Show(validFieldMessage, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                        blnIsValid = false;
                                    }
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.REQUIRESNO, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                                txtBoxesPacked.Clear();
                                txtSerialNumber.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BOXES_PACKED_COUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtBoxesPacked.Clear();
                            txtBoxesPacked.Focus();
                        }
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
                List<DropdownDTO> lstSurgicalOrders = FinalPackingBLL.GetFirstGradePOList().Distinct<DropdownDTO>().ToList();
                cmbPurchaseOrder.SelectedIndexChanged -= cmbPurchaseOrder_SelectedIndexChanged;
                cmbPurchaseOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPurchaseOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbPurchaseOrder.BindComboBox(lstSurgicalOrders, true);
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

        private void cmbPurchaseOrder_SelectedIndexChanged(object sender, EventArgs e)
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
                        }
                        cmbItemNumber.SelectedIndex = -1;
                        cmbItemNumber.SelectedIndexChanged += cmbItemNumber_SelectedIndexChanged;
                        ClearFormPOChange();
                    }
                    else
                    {
                        GlobalMessageBox.Show("Production status is not started for the selected PO!", Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                        cmbPurchaseOrder.SelectedIndex = -1;
                        cmbFGBOList.DataSource = null;
                        cmbFGBOList.Items.Clear();
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
                    //objItemNumber = (SOLineDTO)cmbItemNumber.SelectedItem;
                    string itemNumber = cmbItemNumber.Text;
                    foreach (SOLineDTO objItemNum in lstItemDetails)
                    {
                        if (objItemNum.ItemNumber == itemNumber)
                            objItemNumber = objItemNum;
                    }

                    listItemSize = lstPODetails.Where(SOLineDTO => SOLineDTO.ItemNumber == itemNumber).ToList();
                    //cmbItemSize.SelectedIndexChanged -= cmbItemSize_SelectedIndexChanged;
                    //cmbItemSize.DataSource = listItemSize;
                    //cmbItemSize.ValueMember = "ItemSize";
                    //cmbItemSize.DisplayMember = "ItemSize";
                    //cmbItemSize.SelectedIndex = Constants.MINUSONE;
                    //cmbItemSize.SelectedIndexChanged += cmbItemSize_SelectedIndexChanged;                    
                    txtItemName.Text = objItemNumber.ItemName;

                    //28/10 cpkoo
                    if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                    {
                        Boolean bIsInnerBulkPackedusingLabelSticker;
                        String[] sCustRefPONumber = cmbPurchaseOrder.Text.ToString().Split('|');

                        bIsInnerBulkPackedusingLabelSticker = FinalPackingBLL.isSalesOrderFGInnerLabelsetCOMPrinting(sCustRefPONumber[1].Trim(), cmbItemNumber.Text);
                        if (bIsInnerBulkPackedusingLabelSticker)
                        {
                            if (FinalPackingPrint.checkInnerlabelPrinterAvailable())
                            {

                            }
                            else
                            {
                                FloorSystemException exTemp = new FloorSystemException(Messages.LABEL_PRINTER_COMMUNICATION_ERROR, "Final Packing Printer", new Exception(), true);
                                ExceptionLogging(exTemp, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit1", sCustRefPONumber[1].Trim(), cmbItemNumber.Text);
                                ClearForm();
                            }
                        }
                        else
                        {
                            //25/10/2016 Koo Chung Peng, Check for Imaje availability
                            if (FinalPackingPrint.checkInnerImajePrinterAvailable())
                            {

                            }
                            else
                            {
                                FloorSystemException exTemp = new FloorSystemException(Messages.INKJET_PRINTER_COMMUNICATION_ERROR, "Final Packing Printer", new Exception(), true);
                                ExceptionLogging(exTemp, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit2", sCustRefPONumber[1].Trim(), cmbItemNumber.Text);
                                ClearForm();

                            }
                        }
                    } // end check for Hardware Integration.
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit3", null);
            }
        }
        /// <summary>
        /// POPULATE PALLET AND PRESHIPMENT PALLET ID LIST AND ASSOCIATED ID'S
        /// </summary>
        /// <param name="associatedPalletId"></param>
        /// <param name="associatedpreShipPLTID"></param>
        private void PopulatePallet(string associatedPalletId = "", bool listAllAssociatedPallet = false, string poNumber = "",
                                string itemNumber = "", string size = "", string associatedpreShipPLTID = "")
        {
            try
            {
                List<DropdownDTO> lstPallet = null;
                if (!listAllAssociatedPallet)
                {
                    lstPallet = FinalPackingBLL.GetPalletIdList();
                }
                else
                {
                    lstPallet = FinalPackingBLL.GetPalletIdList(_poNumber, _itemNumber, _size, listAllAssociatedPallet);
                }
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
                {
                    foreach (DropdownDTO plt in lstPallet)
                    {
                        cmbPalletId.Items.Add(plt.DisplayField);
                        cmbPalletId.AutoCompleteCustomSource.Add(plt.DisplayField);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
        }

        private void PopulatePReshipmentPLTID(string associatedpreShipPLTID = "")
        {
            try
            {
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

                    if (!_PSIChecked || (!string.IsNullOrEmpty(newAssociatedPreShipPLTID) && !associatedpreShipPLTID.ToUpper().Equals(newAssociatedPreShipPLTID.ToUpper())))
                    {
                        if (!string.IsNullOrEmpty(newAssociatedPreShipPLTID))
                            associatedpreShipPLTID = newAssociatedPreShipPLTID;
                        if (GlobalMessageBox.Show(string.Format(Messages.ASSOCIATEDPRESHIPMENTPALLETID, associatedpreShipPLTID), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                        {
                            associatedpreShipPLTID = string.Empty;
                        }
                        _PSIChecked = true;
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
                    //KahHeng 26Apr2019 Flag to indicate the 1st palletID is continued from before pallet
                    _isContinuedPallet = true;
                    //KahHeng 26Apr2019 End
                }
                else
                {
                    List<DropdownDTO> lstPrePallet = FinalPackingBLL.GetPreshipmentPalletIdList();
                    cmbPreShipPltId.Items.Clear();
                    cmbPreShipPltId.Text = String.Empty;

                    if (lstPrePallet != null && _PSIRequired)
                        foreach (DropdownDTO plt in lstPrePallet)
                        {
                            cmbPreShipPltId.Items.Add(plt.DisplayField);
                            cmbPreShipPltId.AutoCompleteCustomSource.Add(plt.DisplayField);
                        }
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ValidateBatchCapacity", null);
            }
        }

        /// <summary>
        /// INSERT OUTER CASE NUMBERS ALONG WITH PRESHIPMENT FLAG FOR THE PO ITEM
        /// </summary>
        private void InsertCaseNumbers()
        {
            try
            {
                FinalPackingBLL.InsertCaseNumbers(_itemcases, _poNumber, _itemNumber, _size, _preshipmentRandomNumbers, WorkStationDTO.GetInstance().WorkStationId, WorkStationDTO.GetInstance().LocationId, objItemSize.InventTransId, _customerSize);
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
                List<Int32> result;
                //TOTT id :317 when prishipment qty is same as actual qty
                if (preshipmentQty == cty)
                {
                    result = new List<Int32>();
                    for (Int32 presqtycount = Constants.ONE; presqtycount <= preshipmentQty; presqtycount++)
                    {
                        strresult += presqtycount.ToString() + Constants.COMMA;
                    }
                }
                else
                {
                    Random rand = new Random();
                    Int32 curValue = 0;
                    int preshipmentQtyStart = 0;
                    int preshipmentQtyEnd = 0;
                    // Calculate rounded interval amount - always rounding down to avoid out of range 
                    int preshipmentQtyInterval = Decimal.ToInt32((decimal)cty / preshipmentQty);

                    result = new List<Int32>();
                    for (Int32 presqtycount = Constants.ZERO; presqtycount < preshipmentQty; presqtycount++)
                    {
                        // Start will be 1 and Last interval will always be the cty amount
                        preshipmentQtyStart = preshipmentQtyEnd + 1;
                        preshipmentQtyEnd = ((presqtycount + 1) == preshipmentQty) ? cty : preshipmentQtyInterval * (presqtycount + 1);
                        curValue = rand.Next(preshipmentQtyStart, preshipmentQtyEnd + 1);

                        result.Add(curValue);
                        strresult += curValue + Constants.COMMA;
                    }
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
            validationMesssageLst.Add(new ValidationMessage(txtInnerPrinter, Messages.PRINTER_RQ, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, Messages.REQSERIALNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtBoxesPacked, Messages.REQBOXESPACKED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            //validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPalletId, Messages.REQPALLETREQUIRED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));

            if (cmbPreShipPltId.Enabled)
                validationMesssageLst.Add(new ValidationMessage(cmbPreShipPltId, Messages.REQPRESHIPMENTPALLETREQUIRED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// saves the transaction details for the scanned Batch
        /// </summary>
        /// <param name="objFinalPackingDTO">Final Packing DTO object</param>
        private int InsertFinalPacking(FinalPackingDTO objFinalPackingDTO, FinalPackingBatchInfoDTO objFinalPackingBatchInfoDTO)
        {
            int rowsreturned = Constants.ZERO;
            int totalCasespacked = Constants.ZERO;
            string batchInfo = string.Empty;
            try
            {
                batchInfo = FinalPackingBLL.CreateXML(objFinalPackingBatchInfoDTO);
                //capture the already placed case count on the selected pallet
                int selectedPalletCasesPlaced = FinalPackingBLL.ValidatePalletCapacity(objFinalPackingDTO.Palletid, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                if (selectedPalletCasesPlaced < _palletcapacity)
                {
                    rowsreturned = FinalPackingBLL.InsertFinalPacking(objFinalPackingDTO, batchInfo);
                    if (rowsreturned > 0)
                    {
                        totalCasespacked = FinalPackingBLL.GetPONumberItemCasesPacked(_poNumber, _itemNumber, _size);
                        if (totalCasespacked == _itemcases)
                        {
                            int rowsupdated = FinalPackingBLL.UpdatePONumberItemstatus(_poNumber, _itemNumber, _size);
                        }
                        //added by KahHeng (09Jan019)
                        //Used for update the Preshipment PalletID "isOccupied" flag to 1 in PalletMaster table
                        FinalPackingBLL.UpdatePreshipmentPalletIDFlag(objFinalPackingDTO.PreshipmentPLTId, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.locationId);
                        //end edit
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
                //isPostingSuccess = AXPostingBLL.PostAXDataFinalPacking(internallotnumber, Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanBatchCardInnerOuter)));
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



        /// <summary>
        /// CLEAR THE FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            cmbPurchaseOrder.SelectedIndex = Constants.MINUSONE;
            cmbItemNumber.Items.Clear();
            cmbFGBOList.DataSource = null;
            cmbFGBOList.Items.Clear();
            //cmbItemSize.DataSource = null;
            cmbItemNumber.Text = string.Empty;
            _isAssociated = false;
            _isTempack = false;
            ClearFormPOChange();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
            GetActivePOList();
            //BindPackingGroup();
            txtInternalLotNumber.Text = GetInternalLotNumber();
            btnPrint.Enabled = true;
            _VisionSystemDown = false;
            _VisionRecipeLoaded = false;
        }
        /// <summary>
        /// CLEAR FORM FOR PO CHNAGE
        /// </summary>
        private void ClearFormPOChange()
        {
            txtItemName.Text = string.Empty;
            txtSerialNumber.Text = string.Empty;
            txtBatch.Text = string.Empty;
            cmbFGBOList.DataSource = null;
            cmbFGBOList.Items.Clear();
            txtBoxesPacked.Text = string.Empty;
            cmbPalletId.Items.Clear();
            cmbPalletId.Text = string.Empty;
            cmbPreShipPltId.Items.Clear();
            cmbPreShipPltId.Text = string.Empty;
            //cmbGroupId.SelectedIndex = Constants.MINUSONE;
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
                GlobalMessageBox.Show("(" + result + ") - " + Messages.PRINTING_EXCEPTION + "\n" + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);

            ClearForm();
        }

        // #Azrul 13/07/2018: Merged from Live AX6 Start
        private void ExceptionLoggingWithoutPopup(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            ClearForm();
        }
        // #Azrul 13/07/2018: Merged from Live AX6 End
        #endregion



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

        private void cmbItemNumber_Leave(object sender, EventArgs e)
        {

        }

        private LineClearanceLogDTO GetLineClearanceLogObject(string workstationID)
        {
            LineClearanceLogDTO obj = new LineClearanceLogDTO();
            obj = FinalPackingBLL.GetLineClearanceLog(workstationID);
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
        
        #region TOMS Validation for Final Packing 

        //Vinoden March 2020
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
                            CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ScanBatchInnerOuter", SerialNO);
                        }
                        else if (item.Status.Equals(3))
                        {
                            FloorSystemException floorException = new FloorSystemException("FP_TomsValidation", "FinalPacking", new Exception(String.Format("Batch ({0}) has not been scanned out from TOMS", item.SerialNo)), true);
                            CommonBLL.LogExceptionToDB(floorException, "Final Packing", "FinalPackingBLL", "ScanBatchInnerOuter", SerialNO);
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

        //private void cmbItemNumber_Leave(object sender, EventArgs e)
        //{
        //    Boolean bIsInnerBulkPackedusingLabelSticker;

        //    bIsInnerBulkPackedusingLabelSticker = FinalPackingBLL.isSalesOrderFGInnerLabelsetCOMPrinting(cmbPurchaseOrder.Text, cmbItemNumber.Text);
        //    if(bIsInnerBulkPackedusingLabelSticker)
        //    {
        //        if(FinalPackingPrint.checkInnerlabelPrinterAvailable())
        //        {               

        //        }
        //        else{
        //            GlobalMessageBox.Show("NO COM available", Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        //            ClearForm();

        //        }

        //    }
        //    else{



        //    }
        //}


    }

}
