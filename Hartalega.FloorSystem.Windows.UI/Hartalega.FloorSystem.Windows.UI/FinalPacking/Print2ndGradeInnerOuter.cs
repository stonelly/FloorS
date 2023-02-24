using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.Common;
using System.Diagnostics;
using System.IO;
using System.Transactions;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class Print2ndGradeInnerOuter : FormBase
    {
        private string _screenname = "Print2ndGradeInnerOuter"; //To use for DB logging.
        private string _uiClassName = "Print2ndGradeInnerOuter"; //To use for DB logging.        
        private string _innesetLayOut = string.Empty; //To capture the Inner Set Layout Number to print.
        private string _outersetLayOut = string.Empty; //To capture the Outer Set Layout Number to print._click
        private string _size = string.Empty;
        private string _customerSize = string.Empty;
        private string _itemNumber = string.Empty;
        private string _poNumber = string.Empty; //To capture the selected POnumber.
        private string _orderNumber = string.Empty;
        private StringBuilder _validSerialNo = new StringBuilder();
        private StringBuilder _invalidSerialNo = new StringBuilder();
        private int _itemcases = 0;
        private int _casecapacity = 0;
        private int _innerboxCapacity = 0;
        List<SOLineDTO> lstPODetails = null;
        SOLineDTO objItemNumber = null;
        private int _nextNumber = 0;
        List<SOLineDTO> lstItemDetails = null;
        SOLineDTO objItemSize = null;
        private int _GCLabelPrintingRequired = 0;
        private int _BarcodeVerificationRequired = 0;
        private int _expiry = 0;
        private int _manufacturingOrderbasis = 0;
        private DateTime? _receiptDateRequested = new DateTime();
        private DateTime? _shippingDateRequested = new DateTime();
        private DateTime? _ProductionDate = new DateTime();

        //Azman 2019-01-29 PODate & POReceived Date
        private DateTime? _custPODocumentDate = new DateTime();
        //Azman 2019-01-29 PODate & POReceived Date
        //Pang 09/02/2020 First Carton Packing Date
        private DateTime _FirstManufacturingDate = new DateTime();


        public Print2ndGradeInnerOuter()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Print2ndGradeInnerOuter_Load(object sender, EventArgs e)
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
            //BindPackingGroup();
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// POpulate Packing Group
        /// </summary>
        //private void BindPackingGroup()
        //{
        //    List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups(Constants.PACKING_GROUP_TYPE);
        //    cmbGroupId.BindComboBox(lstDropDown, true);
        //}
        /// <summary>
        /// GET ACTIVE PO LIST
        /// </summary>
        private void GetActivePOList()
        {
            try
            {
                cmbPurchaseOrder.SelectedIndexChanged -= cmbPurchaseOrder_SelectionChangeCommitted;
                List<DropdownDTO> lstSecGradeActivePOList = FinalPackingBLL.GetSecondGradePOList().Distinct<DropdownDTO>().ToList();
                cmbPurchaseOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPurchaseOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbPurchaseOrder.BindComboBox(lstSecGradeActivePOList, true);
                cmbPurchaseOrder.SelectedIndexChanged += cmbPurchaseOrder_SelectionChangeCommitted;
                cmbPurchaseOrder.Validated += cmbPurchaseOrder_Validated;
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
                    txtBatchOrder.Text = string.Empty;
                    cmbPurchaseOrder.Focus();
                }
            }
        }

        /// <summary>
        /// Purchaseorder selection index changed
        /// </summary>
        /// <param name="sender"></param>_
        /// <param name="e"></param>
        private void cmbPurchaseOrder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbPurchaseOrder.SelectedIndex > Constants.MINUSONE)
                {
                    lstItemDetails = new List<SOLineDTO>();
                    lstPODetails = FinalPackingBLL.GetPurchaseOrderList(cmbPurchaseOrder.Text.FPCustomerRefernceSplit(), cmbPurchaseOrder.SelectedValue.ToString());
                    cmbItemNumber.Items.Clear();
                    cmbItemNumber.SelectedIndexChanged -= cmbItemNumber_SelectionChangeCommitted;
                    foreach (SOLineDTO soline in lstPODetails)
                    {
                        if (!cmbItemNumber.Items.Contains(soline.ItemNumber))
                        {
                            lstItemDetails.Add(soline);
                            cmbItemNumber.Items.Add(soline.ItemNumber);
                        }
                    }
                    cmbItemNumber.SelectedIndex = Constants.MINUSONE;
                    cmbItemNumber.SelectedIndexChanged += cmbItemNumber_SelectionChangeCommitted;
                }
                txtItemName.Text = string.Empty;
                cmbItemSize.DataSource = null;
                txtValidSerialnumber.Text = string.Empty;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbPurchaseOrder_SelectedIndexChanged", null);
            }

        }
        /// <summary>
        /// GENERATE THE RUNNING INTENAL LOT NUMBER
        /// </summary>
        /// <returns></returns>
        public string GetInternalLotNumber()
        {
            string internalLotNumber = string.Empty;
            DateTime curretDate = CommonBLL.GetCurrentDateAndTimeFromServer();
            _nextNumber = FinalPackingBLL.GetWorkStationLastRunningNumber(WorkStationDataConfiguration.GetInstance().stationNumber);
            internalLotNumber = string.Format("{0}{1}{2}", curretDate.ToString(Messages.INTERNALLOTNUMBER_DATEFORMAT), Convert.ToString(WorkStationDataConfiguration.GetInstance().stationNumber).PadLeft(2, '0'), _nextNumber.ToString().PadLeft(4, '0'));
            return internalLotNumber;
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
                GlobalMessageBox.Show("(" + result + ") - " + Messages.PRINTING_EXCEPTION, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);

            //ClearForm();
        }
        /// <summary>
        /// Item Number selection index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbItemNumber_SelectionChangeCommitted(object sender, EventArgs e)
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
                    cmbItemSize.SelectedIndexChanged -= cmbItemSize_SelectedIndexChanged;
                    cmbItemSize.DataSource = listItemSize;
                    cmbItemSize.ValueMember = "ItemSize";
                    cmbItemSize.DisplayMember = "ItemSize";
                    cmbItemSize.SelectedIndex = Constants.MINUSONE;
                    cmbItemSize.SelectedIndexChanged += cmbItemSize_SelectedIndexChanged;
                    txtItemName.Text = objItemNumber.ItemName;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit", null);
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
                    _customerSize = objPODetails.CustomerSize;
                    _itemcases = objPODetails.ItemCases;
                    _poNumber = objPODetails.PONumber;
                    _casecapacity = objPODetails.CaseCapacity;
                    _innerboxCapacity = objPODetails.InnerBoxCapacity;
                }
                objPODetails.locationID = WorkStationDTO.GetInstance().LocationId;
                FinalPackingBLL.InsertPurchaseOrderDetails(objPODetails);//Insert Purchase Order details.    
                FinalPackingBLL.InsertCaseNumbers(_itemcases, _poNumber, _itemNumber, _size, string.Empty, WorkStationDTO.GetInstance().WorkStationId, WorkStationDTO.GetInstance().LocationId, objPODetails.InventTransId, _customerSize);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "UpdatePODetails", null);
            }
        }
        /// <summary>
        /// required field validation
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateRequiredFields(Boolean isfromPrint = false)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemSize, Messages.REQITEMSIZE, ValidationType.Required));
            //validationMesssageLst.Add(new ValidationMessage(cmbGroupId, Messages.REQGROUPID, ValidationType.Required));
            if (isfromPrint)
                validationMesssageLst.Add(new ValidationMessage(txtValidSerialnumber, Messages.REQSERIALNUMBER, ValidationType.Required));
            return ValidateForm();
        }

        /// <summary>
        /// VALIDATE BOXES PACKED
        /// </summary>
        private Boolean ValidateSerialNumbersPacked()
        {
            int boxesentered = 0;
            int validSerialnumbers = 0;
            int invalidsSerialnumbers = 0;
            int validcount = 0;
            int invalidcount = 0;
            Boolean _isvalid = false;
            try
            {
                if (!string.IsNullOrEmpty(txtValidSerialnumber.Text))
                {
                    boxesentered = _validSerialNo.ToString().Split(',').Count();
                    if (boxesentered < CaseCapacity())
                    {
                        GlobalMessageBox.Show(string.Format(Messages.SECONDGRADE_LESSTHANCASECAPACITY, CaseCapacity()), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtValidSerialnumber.Text = string.Empty;
                        txtInvalidSerialNumber.Text = string.Empty;
                        _isvalid = false;
                    }
                    else
                    {
                        validSerialnumbers = Convert.ToInt32(FinalPackingBLL.RoundToCaseCapacity(Convert.ToString(boxesentered), CaseCapacity()));
                        invalidsSerialnumbers = boxesentered - validSerialnumbers;
                        invalidcount = validSerialnumbers;
                        if (invalidsSerialnumbers != 0)
                        {
                            string[] serialNumerlist = _validSerialNo.ToString().Split(',');
                            string invalidSerialNumbers = string.Empty;
                            _validSerialNo = new StringBuilder();
                            _invalidSerialNo = new StringBuilder();
                            while (validcount != validSerialnumbers)
                            {
                                _validSerialNo = _validSerialNo.Append(serialNumerlist[validcount] + ',');
                                validcount++;
                            }
                            while (invalidcount != boxesentered)
                            {
                                _invalidSerialNo = _invalidSerialNo.Append(serialNumerlist[invalidcount] + ',');
                                invalidcount++;
                            }
                            GlobalMessageBox.Show(string.Format(Messages.SERIALNUMBER_INVALID, _invalidSerialNo), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            _validSerialNo = _validSerialNo.Remove(_validSerialNo.Length - 1, Constants.ONE);
                            _invalidSerialNo = _invalidSerialNo.Remove(_invalidSerialNo.Length - 1, Constants.ONE);
                        }
                        _isvalid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                FloorSystemException fsexception = new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                ExceptionLogging(fsexception, _screenname, _uiClassName, "ValidateSerialNumbersPacked", null);
            }
            return _isvalid;
        }

        private int CaseCapacity()
        {
            return (_casecapacity * _innerboxCapacity) / Constants.THOUSAND;
        }
        /// <summary>
        /// print click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_click(object sender, EventArgs e)
        {
            int rowsreturned = Constants.ZERO;
            btnPrint.Enabled = false;
            if (ValidateRequiredFields(true))
            {
                if (ValidateSerialNumbersPacked())
                    if (ValidateInnerBoxes() && ValidateCasesPacked())
                    {
                        // Avoid duplicate internallotnumber at workstation level
                        int lastRunningNo = 0;
                        if (FinalPackingBLL.CheckDuplicateInternalLotNumber(txtInternalLotNumber.Text, WorkStationDataConfiguration.GetInstance().stationNumber, out lastRunningNo))
                        {
                            txtInternalLotNumber.Text = GetInternalLotNumber(); // Get latest/updated internal lot number 
                        }

                        FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
                        objFinalPackingDTO.locationId = WorkStationDTO.GetInstance().LocationId;
                        objFinalPackingDTO.Workstationnumber = WorkStationDTO.GetInstance().WorkStationId;
                        objFinalPackingDTO.Printername = WorkStationDataConfiguration.GetInstance().innerPrinter;
                        objFinalPackingDTO.Packdate = CommonBLL.GetCurrentDateAndTimeFromServer();
                        objFinalPackingDTO.Internallotnumber = txtInternalLotNumber.Text;
                        objFinalPackingDTO.Outerlotno = txtInternalLotNumber.Text;
                        objFinalPackingDTO.Ponumber = cmbPurchaseOrder.Text.FPCustomerRefernceSplit();
                        objFinalPackingDTO.ItemNumber = objItemSize.ItemNumber;
                        objFinalPackingDTO.Size = _size;
                        objFinalPackingDTO.listSecondGradeSerialNumber = _validSerialNo.ToString().Replace(" ", "");
                        objFinalPackingDTO.Boxespacked = _validSerialNo.ToString().Split(',').Count();
                        objFinalPackingDTO.Casespacked = SecondGradeCaseCalculation(objFinalPackingDTO);
                        //objFinalPackingDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
                        objFinalPackingDTO.Innersetlayout = objItemSize.InnersetLayout;
                        objFinalPackingDTO.Outersetlayout = objItemSize.OuterSetLayout;
                        objFinalPackingDTO.stationNumber = WorkStationDataConfiguration.GetInstance().stationNumber;

                        objFinalPackingDTO.FPStationNo = txtstation.Text; //Added by Azman 2019-08-22 FPDetailsReport
                        objFinalPackingDTO.ManufacturingDate = FinalPackingBLL.GetManufacturingDate(_manufacturingOrderbasis, _shippingDateRequested, _receiptDateRequested, CommonBLL.GetCurrentDateAndTimeFromServer(), _custPODocumentDate, null, _FirstManufacturingDate);
                        objFinalPackingDTO.ExpiryDate = objFinalPackingDTO.ManufacturingDate.Value.AddMonths(_expiry);
                        objFinalPackingDTO.InventTransId = objItemSize.InventTransId;
                        objFinalPackingDTO.FGBatchOrderNo = txtBatchOrder.Text;
                        objFinalPackingDTO.Resource = FinalPackingBLL.GetResourceFromBatchOrder(objFinalPackingDTO.FGBatchOrderNo);

                        try
                        {
                            // Create the TransactionScope to execute the commands, guaranteeing
                            // that both commands can commit or roll back as a single unit of work.
                            //https://msdn.microsoft.com/en-us/library/system.transactions.transactionscope(v=vs.110).aspx
                            using (TransactionScope scope = new TransactionScope())
                            {
                                rowsreturned = FinalPackingBLL.InsertSecondGradeFinalPacking(objFinalPackingDTO);
                                // Insert to FS staging table
                                AXPostingBLL.PostAXDataFinalPacking(objFinalPackingDTO.Internallotnumber, Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanBatchCardInnerOuter2ndGrade)));
                                // The Complete method commits the transaction. If an exception has been thrown,
                                // Complete is not  called and the transaction is rolled back.
                                scope.Complete();
                            }
                        }
                        catch (FloorSystemException ex)
                        {
                            rowsreturned = 0;
                            ExceptionLogging(ex, _screenname, _uiClassName, "GetActivePOList", null);
                        }
                        if (rowsreturned > Constants.ZERO)
                        {
                            try
                            {
                                // No need check AX posting status for D365
                                //bool isPostingSucess = PostAXData(objFinalPackingDTO.Internallotnumber);
                                //if (isPostingSucess)
                                //{
                                OuterLabelDTO objOuterLabelDTO = FinalPackingBLL.PrintInnerandOuter(objFinalPackingDTO.Internallotnumber, _GCLabelPrintingRequired == Constants.ONE ? true : false,
                                    _BarcodeVerificationRequired == Constants.ONE ? true : false, Constants.SIX.ToString());

                                if (_BarcodeVerificationRequired == Constants.TWO)
                                {
                                    if (!string.IsNullOrEmpty(objOuterLabelDTO.barcodeToValidate))
                                    {
                                        new BarcodeVerificationPass(WorkStationDataConfiguration.GetInstance().stationNumber, objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.Ponumber,
                                            objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, _orderNumber, txtItemName.Text, objFinalPackingDTO.Size).ShowDialog();
                                    }
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.BARCODE_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    }
                                }
                                // Pang 09/02/2020 First Carton Packing Date
                                FinalPackingBLL.UpdateFirstCartonPackingDate(objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                                int POStatusRow = FinalPackingBLL.UpdatePOStatus(objFinalPackingDTO.Ponumber);
                                //}

                            }
                            catch (FloorSystemException ex)
                            {
                                ExceptionLogging(ex, _screenname, _uiClassName, "btnPrint_Click", null);
                                txtInternalLotNumber.Text = GetInternalLotNumber();
                            }
                        }
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        ClearControls();
                        txtInternalLotNumber.Text = GetInternalLotNumber();
                    }
            }
            btnPrint.Enabled = true;
        }

        private int SecondGradeCaseCalculation(FinalPackingDTO objFinalPackingDTO)
        {
            int CasePieces = _innerboxCapacity * _casecapacity;
            int SerialnumbersPerCase = CasePieces / Constants.THOUSAND;
            if (SerialnumbersPerCase >= 1)
            {
                int CasesToPack = objFinalPackingDTO.Boxespacked / SerialnumbersPerCase;
                return CasesToPack;
            }
            else
            {
                int CasesToPack = objFinalPackingDTO.Boxespacked / 1;
                return CasesToPack;
            }

        }
        /// <summary>
        /// Post Data to AX
        /// </summary>
        /// <param name="internallotnumber"></param>
        /// <returns></returns>
        private static bool PostAXData(string internallotnumber)
        {
            bool isPostingSuccess = false;
            try
            {
                isPostingSuccess = AXPostingBLL.PostAXDataFinalPacking(internallotnumber, Convert.ToString(Convert.ToInt16(Constants.SubModules.ScanBatchCardInnerOuter2ndGrade)));
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
                GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            }
            else
            {
                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
            return isPostingSuccess;
        }
        /// <summary>
        /// TO VALIDATE REQUIRED ITEM CASES
        /// </summary>
        /// <param name="POItemSizecasespacked">EXISTING PO ITEM CASES</param>
        /// <param name="boxesentered">BOXES ENTERED IN TEXTBOX</param>
        /// <returns></returns>
        private Boolean ValidateRequiredItemCases(int POItemSizecasespacked, int boxesentered)
        {
            //if ((POItemSizecasespacked == _itemcases))
            if (CaseCapacity() < 1)
            {
                if ((POItemSizecasespacked * 1) + (boxesentered) > (_itemcases * 1))
                    return false;
                else
                    return true;
            }
            else
            {
                if ((POItemSizecasespacked * CaseCapacity()) + (boxesentered) > (_itemcases * CaseCapacity()))
                    return false;
                else
                    return true;
            }

        }
        /// <summary>
        /// Validate Boxes packed event
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateCasesPacked()
        {
            int POItemSizecasespacked = 0;
            int boxesentered = 0;
            Boolean blnIsValid = false;
            try
            {
                //boxes packed Null or empty validation
                if (!string.IsNullOrEmpty(txtValidSerialnumber.Text))
                {
                    boxesentered = _validSerialNo.ToString().Split(',').Count();
                    //cases already packed for the given PO,ITEM and SIZE.
                    POItemSizecasespacked = FinalPackingBLL.ValidateSecondGradePOSizeBoxesPacked(_poNumber, _itemNumber, _size);
                    //validate boxes entered and boxes already packed is not greater than the requied 
                    if (!ValidateRequiredItemCases(POItemSizecasespacked, boxesentered))
                    {
                        GlobalMessageBox.Show("Exceed ratio.", Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    }
                    else
                    { blnIsValid = true; } // return true if no validation failed
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
                if (!String.IsNullOrEmpty(txtValidSerialnumber.Text))
                {
                    boxesentered = _validSerialNo.ToString().Split(',').Count() * 1000;
                    dtBatchOrderDetails = FinalPackingBLL.GetFGBORemainingQty(txtBatchOrder.Text, _size);

                    if (dtBatchOrderDetails != null)
                    {
                        if (dtBatchOrderDetails.Count > 0)
                        {
                            foreach (var dt in dtBatchOrderDetails)
                            {
                                remainingQty = int.Parse(dt.RemainingQty.Replace(",", "")) * _innerboxCapacity * _casecapacity;
                                if (remainingQty > 0)
                                {
                                    if (boxesentered > remainingQty)
                                    {
                                        GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((remainingQty * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK));
                                        btnPortableBarcode.Focus();
                                    }
                                    else
                                        blnIsValid = true;
                                }
                                else
                                {
                                    GlobalMessageBox.Show(string.Format(Messages.REQUIREDBOXES, Convert.ToString((remainingQty * _casecapacity)), Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK));
                                    btnPortableBarcode.Focus();
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
        /// cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearControls();
                cmbPurchaseOrder.Focus();
            }
        }
        /// <summary>
        /// Clear Form
        /// </summary>
        private void ClearControls()
        {
            cmbPurchaseOrder.SelectedIndex = Constants.MINUSONE;
            //cmbGroupId.SelectedIndex = Constants.MINUSONE;
            cmbItemNumber.Items.Clear();
            cmbItemSize.DataSource = null;
            txtValidSerialnumber.Text = string.Empty;
            txtInvalidSerialNumber.Text = string.Empty;
            txtBatchOrder.Text = string.Empty;
            txtItemName.Text = String.Empty;
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
        }
        /// <summary>
        /// ItemSize selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbItemSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbItemSize.SelectedIndex > Constants.MINUSONE)
                {
                    objItemSize = (SOLineDTO)cmbItemSize.SelectedItem;
                    _itemNumber = objItemSize.ItemNumber;
                    _size = objItemSize.ItemSize;
                    _customerSize = objItemSize.CustomerSize;
                    _GCLabelPrintingRequired = objItemSize.GCLabelPrintingRequired;
                    _BarcodeVerificationRequired = objItemSize.BarcodeVerificationRequired;
                    _manufacturingOrderbasis = objItemSize.ManufacturingDateBasis;
                    _receiptDateRequested = objItemSize.RECEIPTDATEREQUESTED;
                    _shippingDateRequested = objItemSize.SHIPPINGDATEREQUESTED;

                    //Azman 2019-01-29 PODate & POReceived Date
                    _custPODocumentDate = objItemSize.CustPODocumentDate;
                    //Azman 2019-01-29 PODate & POReceived Date
                    //Pang 09/02/2021 First Carton Packing Date
                    _FirstManufacturingDate = FinalPackingBLL.GetFirstCartonPackingDate(objItemSize.PONumber, objItemSize.ItemNumber, _size);

                    _expiry = objItemSize.Expiry;
                    _orderNumber = objItemSize.OrderNumber;
                    UpdatePODetails(objItemSize);
                    txtBatchOrder.Text = objItemSize.BatchOrder;
                }
            }
            catch (Exception ex)
            {
                FloorSystemException fsexception = new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                ExceptionLogging(fsexception, _screenname, _uiClassName, "cmbItemSize_SelectedIndexChanged", null);
            }
        }

        private void btnPortableBarcode_Click(object sender, EventArgs e)
        {
            btnPortableBarcode.Enabled = false;
            _validSerialNo = new StringBuilder();
            _invalidSerialNo = new StringBuilder();
            ProcessStartInfo startinfo = new ProcessStartInfo();
            try
            {
                try
                {
                    if (ValidateRequiredFields())
                    {
                        if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                        {
                            if (string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().SecondGradeExecutableFileLocation))
                            {
                                GlobalMessageBox.Show(Messages.PORTABLE_BARCODE_SCANNER_NOT_FOUND, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                return;
                            }
                            else
                            {
                                startinfo.FileName = WorkStationDataConfiguration.GetInstance().SecondGradeExecutableFileLocation;
                                startinfo.Arguments = WorkStationDataConfiguration.GetInstance().SecondGradeArgumentFileLocation;
                                Process myprocess = Process.Start(startinfo);
                                while (myprocess.HasExited == false)
                                {
                                }
                            }
                        }
                        string filepath = WorkStationDataConfiguration.GetInstance().SecondGradeTextFileLocation;
                        if (File.Exists(filepath))
                        {
                            string[] lines = File.ReadAllLines(filepath);
                            foreach (string line in lines)
                            {
                                if (!string.IsNullOrEmpty(line))
                                {
                                    if (FinalPackingBLL.isSecondGradeSerialNumber(Convert.ToDecimal(line), cmbItemSize.Text))
                                    {
                                        if (!_validSerialNo.ToString().Contains(line))
                                            _validSerialNo.Append(line + ", ");
                                    }
                                    else
                                    {
                                        if (!_invalidSerialNo.ToString().Contains(line))
                                            _invalidSerialNo.Append(line + ", ");
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(_validSerialNo.ToString()))
                            {
                                _validSerialNo = new StringBuilder(_validSerialNo.ToString().Trim(' ').TrimEnd(','));
                            }
                            txtValidSerialnumber.Text = _validSerialNo.ToString();
                            if (!string.IsNullOrEmpty(_invalidSerialNo.ToString()))
                                _invalidSerialNo = new StringBuilder(_invalidSerialNo.ToString().TrimEnd(' ').TrimEnd(','));
                            txtInvalidSerialNumber.Text = _invalidSerialNo.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnPortableBarcode_Click", null);
            }
            btnPortableBarcode.Enabled = true;
        }

    }
}
