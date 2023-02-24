using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.IO;
using Hartalega.FloorSystem.IntegrationServices;
using Hartalega.FloorSystem.Windows.UI.Common;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{

    public partial class PrintSurgicalInnerOuterV2 : FormBase
    {
        private string _screenname = "print Surgical Inner Outer (SPP)";
        private string _uiClassName = "Print Surgical Inner Outer (SPP)";
        private int _palletcapacity = 0;
        private int _caseCapacity = 0;
        private int _casesToPack = 0;

        private string _palletid = string.Empty;
        private string _palletidConcated = string.Empty;
        private string _poNumber = string.Empty;
        private string _orderNumber = string.Empty;
        private string _itemNumber = string.Empty;
        private string _size = string.Empty;
        private string _customerSize = string.Empty;
        private Boolean _isAssociated = false;
        private int _BarcodeVerificationRequired = Constants.ZERO;

        private SurgicalFinalPackingDTO objSurgicalFinalPackingDTO;

        public PrintSurgicalInnerOuterV2()
        {
            InitializeComponent();
        }

        private void PrintSurgicalInnerOuter_Load(object sender, EventArgs e)
        {
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
            {
                txtstation.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
            }
            else
                GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

            txtInnerPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
            //BindPalletIdList();
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// retrieve internalotnumber details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInternalLotNumber_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInternalLotNumber.Text))
                {
                    SurgicalPackingPlanDTO data = new SurgicalPackingPlanDTO();
                    data = FinalPackingBLL.SurgicalPackingPlan_Get(txtInternalLotNumber.Text);
                    if ((data == null) || (data.PlanStatus == Constants.ZERO) || (data.PlanStatus == Constants.FOUR))
                    {
                        GlobalMessageBox.Show(Messages.SPP_INVALID_LOT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtInternalLotNumber.Focus();
                    }
                    else if (data.PlanStatus == Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.SPP_LOTNO_ZERO, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtInternalLotNumber.Focus();
                    }
                    else if (data.PlanStatus == Constants.ONE)
                    {
                        GlobalMessageBox.Show(Messages.SPP_LOTNO_ONE, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtInternalLotNumber.Focus();
                    }
                    else if (data.PlanStatus == Constants.THREE)
                    {
                        GlobalMessageBox.Show(Messages.SPP_LOTNO_THREE, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtInternalLotNumber.Focus();
                    }
                    else if (FinalPackingBLL.validateInnerLotNumber(txtInternalLotNumber.Text.Trim(), Constants.EIGHT))
                    {
                        List<SurgicalPackingSNDetailsDTO> detailsDTO = FinalPackingBLL.SurgicalPackingPlan_GetDetails(txtInternalLotNumber.Text.Trim());

                        objSurgicalFinalPackingDTO = FinalPackingBLL.GetSurgicalInternalLotNumberDetails(txtInternalLotNumber.Text.Trim());
                        txtPONumber.Text = string.IsNullOrEmpty(objSurgicalFinalPackingDTO.CustomerReferenceNumber) ? objSurgicalFinalPackingDTO.PoNumber : objSurgicalFinalPackingDTO.CustomerReferenceNumber + " | " + objSurgicalFinalPackingDTO.PoNumber;    
                        txtItemNumber.Text = objSurgicalFinalPackingDTO.ItemNumber;
                        txtItemName.Text = objSurgicalFinalPackingDTO.ItemName;
                        txtGroupId.Text = Convert.ToString(objSurgicalFinalPackingDTO.QCGroupName);
                        //txtSerialNumber1.Text = Convert.ToString(objSurgicalFinalPackingDTO.SerialNumberLeft);
                        //txtSerialNumber2.Text = Convert.ToString(objSurgicalFinalPackingDTO.SerialNumberRight);
                        //txtBatch1.Text = objSurgicalFinalPackingDTO.BatchNumberLeft;
                        //txtBatch2.Text = objSurgicalFinalPackingDTO.BatchNumberRight;
                        txtBoxesPacked.Text = string.Format(Constants.NUMBER_FORMAT, objSurgicalFinalPackingDTO.BoxesPacked);

                        //20201123 Azrul: Bind Active FG Batch(Production) Order list
                        cmbFGBOList.BindComboBox(objSurgicalFinalPackingDTO.BatchOrders, true);
                        if (cmbFGBOList.Items.Count == 1) cmbFGBOList.SelectedIndex = 0;

                        _caseCapacity = objSurgicalFinalPackingDTO.CaseCapacity;
                        _poNumber = objSurgicalFinalPackingDTO.PoNumber;
                        _itemNumber = objSurgicalFinalPackingDTO.ItemNumber;
                        _size = objSurgicalFinalPackingDTO.ItemSize;
                        _customerSize = objSurgicalFinalPackingDTO.CustomerSize;
                        _palletcapacity = objSurgicalFinalPackingDTO.PalletCapacity;
                        _orderNumber = objSurgicalFinalPackingDTO.OrderNumber;
                        if (_caseCapacity != Constants.ZERO)
                            _casesToPack = objSurgicalFinalPackingDTO.BoxesPacked / _caseCapacity;
                        PopulateAssociatedPalletID();

                        gvSerialList.Rows.Clear();

                        foreach (SurgicalPackingSNDetailsDTO snData in detailsDTO)
                        {
                            string[] row = new string[] { snData.SerialNumber, snData.BatchNumber, snData.GloveSide, snData.ReservedQty.ToString() };
                            gvSerialList.Rows.Add(row);
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALIDINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                        txtInternalLotNumber.Focus();
                    }

                }
                else
                {
                    ClearForm();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtInternalLot_Leave", null);
            }

        }
        /// <summary>
        /// exception logging to DB
        /// </summary>
        /// <param name="floorException"></param>
        /// <param name="screenName"></param>
        /// <param name="UiClassName"></param>
        /// <param name="uiControl"></param>
        /// <param name="parameters"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC || floorException.subSystem == Constants.ENCRYPTDECRYPT)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.SERVICEERROR)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.AX_SERVICE_UNAVAILABLE, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.FINALPACKINGPRINTER)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.PRINTING_EXCEPTION + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// RETURN ASSOCIATED PALLET AND PRESHIPMENT PALLETID
        /// </summary>
        /// <param name="poNumber">Purchase Order</param>
        /// <param name="ItemNumber">Item Number</param>
        /// <param name="size">Size</param>
        /// <returns>return PalletId</returns>
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
        /// <summary>
        /// Populate Pallet and Preshipment Pallet
        /// </summary>
        /// <param name="isfromPrinting"></param>
        /// <returns></returns>
        private Boolean PopulateAssociatedPalletID(bool isfromPrinting = false)
        {
            string associatedPalletId = string.Empty;
            try
            {
                if (isfromPrinting)
                {
                    if (FinalPackingBLL.validatePalletPOItemSize(_poNumber, _itemNumber, _size, cmbPalletId.Text) == Constants.ZERO)
                        return false;
                }
                if (isfromPrinting == false && _isAssociated == false)
                {
                    associatedPalletId = GetItemSizePalletId(_poNumber, _itemNumber, _size);
                    if (!string.IsNullOrEmpty(associatedPalletId))
                    {
                        if (GlobalMessageBox.Show(string.Format(Messages.ASSOCIATEDPALLETID, associatedPalletId), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                        {
                            associatedPalletId = string.Empty;
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
        /// <summary>
        /// Print Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int rowsreturned = Constants.ZERO;
            int selectedPalletCasesPlaced = Constants.ZERO;
            int palletCasesCount = Constants.ZERO;

            FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
            objFinalPackingDTO.Internallotnumber = txtInternalLotNumber.Text.Trim();
            objFinalPackingDTO.Ponumber = _poNumber;
            objFinalPackingDTO.ItemNumber = _itemNumber;
            objFinalPackingDTO.Size = _size;

            objFinalPackingDTO.FPStationNo = txtstation.Text; //Added by Azman 2019-08-22 FPDetailsReport

            bool isPass = false;
            bool isComplete = false;
            string authorisedBy = string.Empty;
            LineClearanceLogDTO objLineClearanceLog = new LineClearanceLogDTO();
            //Get previous Line Clearance info for current screen.
            objLineClearanceLog = GetLineClearanceLogObject(WorkStationDTO.GetInstance().WorkStationId);

            try
            {
                if (ValidateRequiredFields())
                {
                    if (FinalPackingBLL.ValidateSurgicalInnerOuter(txtInternalLotNumber.Text.Trim()) > Constants.ZERO)
                    {
                        if (PopulateAssociatedPalletID(true))
                        {
                            /*
                            //Check any changes on PONumber, Size, ItemNumber and Pallet ID from previous Print Info
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
                            } */
                            isPass = true;

                            if (isPass)
                            {
                                _palletid = cmbPalletId.Text;
                                selectedPalletCasesPlaced = FinalPackingBLL.ValidatePalletCapacity(_palletid, _poNumber, _itemNumber, _size);
                                palletCasesCount = FinalPackingBLL.GetPalletIdCount(txtInternalLotNumber.Text.Trim());
                                string customerSize = FinalPackingBLL.GetCustomerSizeByPO(objFinalPackingDTO.Ponumber, _itemNumber, objFinalPackingDTO.Size);

                                //map Line Clearance Log object
                                objLineClearanceLog = new LineClearanceLogDTO();
                                objLineClearanceLog.PONumber = objFinalPackingDTO.Ponumber;
                                objLineClearanceLog.Size = objFinalPackingDTO.Size;
                                objLineClearanceLog.ItemNumber = objFinalPackingDTO.ItemNumber;
                                objLineClearanceLog.PalletID = _palletid;
                                objLineClearanceLog.ScreenName = _screenname;
                                objLineClearanceLog.AuthorisedBy = authorisedBy;
                                objLineClearanceLog.CreatedDateTime = CommonBLL.GetCurrentDateAndTimeFromServer();
                                objLineClearanceLog.WorkStationID = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);

                                while (palletCasesCount > Constants.ZERO)
                                {
                                    if (palletCasesCount > _palletcapacity - selectedPalletCasesPlaced)
                                    {
                                        if ((_palletid == string.Empty) || (_palletid == "") || (_palletid.Length < 7))
                                        {
                                            throw new Exception("Pallet Id is empty");
                                        }
                                        // Pallet id for AXPosting
                                        //_palletidConcated = _palletidConcated + _palletid + "!!!";

                                        if (_palletidConcated.Length > 0)
                                            _palletidConcated = _palletidConcated + "!!!" + _palletid;
                                        else
                                            _palletidConcated = _palletid;

                                        // # Azrul 23/11/2020 Prevent saving empty FGBO
                                        if ((cmbFGBOList.Text == string.Empty) || (cmbFGBOList.Text == ""))
                                        {
                                            throw new Exception("Batch Order is empty");
                                        }

                                        rowsreturned = FinalPackingBLL.InsertPalletId(objFinalPackingDTO.Internallotnumber, _palletid, _palletcapacity - selectedPalletCasesPlaced,
                                              _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size, WorkStationDTO.GetInstance().LocationId);
                                        palletCasesCount = palletCasesCount - (_palletcapacity - selectedPalletCasesPlaced);
                                        if (Convert.ToBoolean(FloorSystemConfiguration.GetInstance().boolEWareNaviFileGenerator))
                                            FinalPackingBLL.GeneratePalletFile(_palletid, false, objFinalPackingDTO.ItemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size);
                                        if (Convert.ToBoolean(FloorSystemConfiguration.GetInstance().boolEWareNaviSQL))
                                            //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                                            FinalPackingBLL.InsertEWareNaviData(_palletid, false, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size, customerSize);
                                        PalletSelection _PalletSelectionForm = new PalletSelection();
                                        _PalletSelectionForm.SelectedPalletID = Convert.ToString(_palletid);
                                        _PalletSelectionForm.isPreshipment = false;
                                        _PalletSelectionForm.ShowDialog();
                                        if (_PalletSelectionForm.IsCancel)
                                        {
                                            FinalPackingBLL.RollBackPrintSurgicalInnerOuter(txtInternalLotNumber.Text);
                                            FinalPackingBLL.RollBackEWNForSurgical(objFinalPackingDTO.Ponumber, _palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                                            ClearForm();
                                            txtInternalLotNumber.Focus();
                                            return;
                                        }
                                        if (!string.IsNullOrEmpty(_PalletSelectionForm.childPalletId))
                                        {
                                            _palletid = objFinalPackingDTO.Palletid = Convert.ToString(_PalletSelectionForm.childPalletId);
                                        }
                                        selectedPalletCasesPlaced = Constants.ZERO;
                                    }
                                    else
                                    {
                                        if ((_palletid == string.Empty) || (_palletid == "") || (_palletid.Length < 7))
                                        {
                                            throw new Exception("Pallet Id is empty");
                                        }

                                        // Pallet id for AXPosting
                                        if (_palletidConcated.Length > 0)
                                            _palletidConcated = _palletidConcated + "!!!" + _palletid;
                                        else
                                            _palletidConcated = _palletid;

                                        rowsreturned = FinalPackingBLL.InsertPalletId(objFinalPackingDTO.Internallotnumber, _palletid, palletCasesCount, _palletcapacity,
                                             objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size, WorkStationDTO.GetInstance().LocationId);
                                        if (palletCasesCount == _palletcapacity - selectedPalletCasesPlaced)
                                        {
                                            if (Convert.ToBoolean(FloorSystemConfiguration.GetInstance().boolEWareNaviFileGenerator))
                                                FinalPackingBLL.GeneratePalletFile(_palletid, false, objFinalPackingDTO.ItemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size);
                                            if (Convert.ToBoolean(FloorSystemConfiguration.GetInstance().boolEWareNaviSQL))
                                                //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                                                FinalPackingBLL.InsertEWareNaviData(_palletid, false, _itemNumber, _palletcapacity, objFinalPackingDTO.Ponumber, objFinalPackingDTO.Size, customerSize);
                                        }
                                        palletCasesCount = palletCasesCount - _palletcapacity;
                                    }
                                }
                                
                                if (rowsreturned > Constants.ZERO)
                                {
                                    bool isPostingSuccess = true; // # Azrul 11/11/2020 Surgical Packing Plan: Moved AXPosting into IOT scan.

                                    List<SurgicalPackingSNDetailsDTO> lst = FinalPackingBLL.SurgicalPackingPlan_GetDetails(txtInternalLotNumber.Text);

                                    string _serial = string.Empty;
                                    string _serialSingle = string.Empty;
                                    string _batchNo = string.Empty;

                                    foreach(SurgicalPackingSNDetailsDTO ls in lst)
                                    {
                                        if (_serial.Length > 0)
                                        {
                                            _serial = _serial + "|" + ls.SerialNumber;
                                        }
                                        else
                                        {
                                            _serial = ls.SerialNumber;
                                            _serialSingle = ls.SerialNumber;
                                            _batchNo = ls.BatchNumber;
                                        }
                                    }

                                    //GenerateAXInnerFile(objFinalPackingDTO.Internallotnumber, _serial);   // # Azrul 08/10/2020 Temp disable for P7 D365 customizations
                                    //GenerateAXOuterFile(objFinalPackingDTO.Internallotnumber);            // # Azrul 08/10/2020 Temp disable for P7 D365 customizations

                                    // # Azrul 08/10/2020 Surgical Packing Plan: Merged from HSBPackage03
                                    // # Azrul 11/11/2020 Surgical Packing Plan: Moved AXPosting into IOT scan.
                                    //isPostingSuccess = AXPostingBLL.PostAXDataFinalPacking(txtInternalLotNumber.Text, Convert.ToString(Convert.ToInt16(Constants.SubModules.SurgicalInnerOuter)), _serialSingle, _batchNo, true, true, true);
                                    if (isPostingSuccess)
                                    {
                                        try
                                        {
                                            FinalPackingPrint.PrintsurgicalInner(txtInternalLotNumber.Text, true);

                                            string s = WorkStationDTO.GetInstance().WorkStationId;

                                            int POStatusRow = FinalPackingBLL.UpdatePOStatus(objFinalPackingDTO.Ponumber); // Update SalesTables & PurchaseOrfer table

                                            FinalPackingBLL.SurgicalPackingPlan_UpdatePlan(txtInternalLotNumber.Text, 3, 1);
                                            FinalPackingBLL.SurgicalPackingPlan_UpdateFGBatchOrderNo(txtInternalLotNumber.Text, cmbFGBOList.Text); //update FGBO & Resource

                                            //insert line clearance log
                                            FinalPackingBLL.InsertLineClearanceLog(objLineClearanceLog);

                                            // # Azrul 08/10/2020 Surgical Packing Plan: Merged from HSBPackage03
                                            // # Azrul 08/10/2020 Temp disable for P7 D365 customizations
                                            //AXPostingBLL.AXCommit();

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

                                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                            isComplete = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobalMessageBox.Show(Messages.PRINTING_EXCEPTION, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                            throw;
                                            //throw new FloorSystemException(Messages.EXCEPTION_SURGICAL_INNER, ex.InnerException);
                                        }                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.FP_PALLETALREADYUSED, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.PRINT_COMPLETE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }

            }
            catch (FloorSystemException ex)
            {
                // # Azrul 08/10/2020 Surgical Packing Plan: Merged from HSBPackage03
                // # Azrul 08/10/2020 Temp disable for P7 D365 customizations
                //AXPostingBLL.AXRollback();
                FinalPackingBLL.RollBackPrintSurgicalInnerOuter(txtInternalLotNumber.Text);
                FinalPackingBLL.RollBackEWNForSurgical(objFinalPackingDTO.Ponumber, objFinalPackingDTO.Palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                ExceptionLogging(ex, _screenname, _uiClassName, "Print_Click", null);
            }
            catch (Exception genEx)
            {
                // # Azrul 08/10/2020 Surgical Packing Plan: Merged from HSBPackage03
                // # Azrul 08/10/2020 Temp disable for P7 D365 customizations
                //AXPostingBLL.AXRollback();
                FinalPackingBLL.RollBackPrintSurgicalInnerOuter(txtInternalLotNumber.Text);
                FinalPackingBLL.RollBackEWNForSurgical(objFinalPackingDTO.Ponumber, objFinalPackingDTO.Palletid, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);
                FloorSystemException fsexp = new FloorSystemException(genEx.Message, Constants.BUSINESSLOGIC, genEx);
                ExceptionLogging(fsexp, _screenname, _uiClassName, "Print_Click", null);
            }

            if (isPass && isComplete)
            {
                try
                {
                    OuterLabelDTO objOuterLabelDTO = FinalPackingPrint.OuterLabelPrint(txtInternalLotNumber.Text);
                    if (_BarcodeVerificationRequired == Constants.ONE && WorkStationDataConfiguration.GetInstance()._skipBarcodeValidation == false)
                    {
                        if (!string.IsNullOrEmpty(objOuterLabelDTO.barcodeToValidate))
                        {
                            FinalPackingBLL.UpdateBarcodeToValidate(objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, objFinalPackingDTO.Internallotnumber);
                            new BarcodeVerificationPass(WorkStationDataConfiguration.GetInstance().stationNumber, objFinalPackingDTO.Internallotnumber, objFinalPackingDTO.Ponumber,
                                objOuterLabelDTO.barcodeToValidate, objOuterLabelDTO.countToValidate, _orderNumber, txtItemName.Text, objFinalPackingDTO.Size).ShowDialog();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.BARCODE_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                    }
                    ClearForm();
                }
                catch (Exception ex)
                {
                    GlobalMessageBox.Show(Messages.EXCEPTION_CUSTOMER_BARCODE, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    throw new FloorSystemException(Messages.EXCEPTION_CUSTOMER_BARCODE, ex.InnerException);
                }
            }
        }

        private void GenerateAXInnerFile(string internalLotNumber, string _serialNo)
        {
            string[] mapping = new string[] {
                "7",  //Surgical_Update_New
                //txtSerialNumber1.Text,
                //txtSerialNumber2.Text,
                internalLotNumber,
                txtBoxesPacked.Text,
                objSurgicalFinalPackingDTO.ItemNumber,
                objSurgicalFinalPackingDTO.PoNumber,
                objSurgicalFinalPackingDTO.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                objSurgicalFinalPackingDTO.ManufacturingDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                txtGroupId.Text,
                WorkStationDTO.GetInstance().LocationId.ToString(),
                _serialNo
            };

            FinalPackingBLL.GenerateAXInnerFile(mapping, FloorSystemConfiguration.GetInstance().strAXInnerPath);

            //FinalPackingBLL.BackupAXFile(FloorSystemConfiguration.GetInstance().strAXInnerPath);
        }

        private void GenerateAXOuterFile(string internalLotNumber)
        {
            List<int> _cartonRange = FinalPackingBLL.GetLotCartonRangeList(internalLotNumber);
            int count = 0;

            string[] surgicalCommonSize = objSurgicalFinalPackingDTO.ItemSize.Split(Constants.CHARCOMMA);
            string strRightsize = string.Empty;

            if (surgicalCommonSize.Count() >= Constants.TWO)
            {
                foreach (string commonsize in surgicalCommonSize)
                {
                    if (commonsize.ToUpper().Contains(Constants.charRight))
                    {
                        strRightsize = commonsize;
                    }
                }
            }

            foreach (int carton in _cartonRange)
            {
                string[] mapping = new string[] {
                _orderNumber,
                objSurgicalFinalPackingDTO.PoNumber,
                internalLotNumber,
                objSurgicalFinalPackingDTO.OuterLotNumber,
                objSurgicalFinalPackingDTO.OuterSetLayout.ToString(),
                objSurgicalFinalPackingDTO.InnerSetLayout.ToString(),
                carton.ToString("00000"),
                objSurgicalFinalPackingDTO.ExpiryDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                objSurgicalFinalPackingDTO.ItemNumber,
                strRightsize,
                objSurgicalFinalPackingDTO.CustomerSize,
                objSurgicalFinalPackingDTO.NettWeight.ToString("#.0 " + Constants.KG),
                objSurgicalFinalPackingDTO.GrossWeight.ToString("#.0 " + Constants.KG),
                objSurgicalFinalPackingDTO.CustomerLotNumber,
                objSurgicalFinalPackingDTO.SerialNumberLeft.ToString(),
                objSurgicalFinalPackingDTO.SerialNumberRight.ToString(),
                _palletidConcated,
                WorkStationDTO.GetInstance().Location,
                WorkStationDTO.GetInstance().WorkStationNumber
                };
                
                FinalPackingBLL.GenerateAXOuterFile(mapping, FloorSystemConfiguration.GetInstance().strAXOuterPath, count > 0 ? true : false);
                count++;
            }

            //FinalPackingBLL.BackupAXFile(FloorSystemConfiguration.GetInstance().strAXOuterPath);
        }

        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtInternalLotNumber, Messages.REQINTERNALLOTNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPalletId, Messages.REQPALLETREQUIRED, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            return ValidateForm();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                txtInternalLotNumber.Focus();
            }
        }
        
        private void ClearForm()
        {
            txtInternalLotNumber.Text = string.Empty;
            txtPONumber.Text = string.Empty;
            txtItemNumber.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtGroupId.Text = string.Empty;
            //txtSerialNumber1.Text = string.Empty;
            //txtSerialNumber2.Text = string.Empty;
            //txtBatch1.Text = string.Empty;
            //txtBatch2.Text = string.Empty;
            gvSerialList.Rows.Clear();
            txtBoxesPacked.Text = string.Empty;
            cmbPalletId.SelectedIndex = Constants.MINUSONE;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
            cmbPalletId.Items.Clear();
            cmbPalletId.Text = string.Empty;
            _isAssociated = false;
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
    }
}
