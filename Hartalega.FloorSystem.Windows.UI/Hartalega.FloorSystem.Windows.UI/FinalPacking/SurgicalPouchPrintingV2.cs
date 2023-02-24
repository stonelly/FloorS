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
    public partial class SurgicalPouchPrintingV2 : FormBase
    {
        private string _screenname = "Surgical Pouch Printing (SPP)"; //To use for DB logging.
        private string _uiClassName = "SurgicalPouchPrinting (SPP)"; //To use for DB logging.
        private Boolean _isTempack = false;
        private string _preshipmentRandomNumbers = string.Empty; //20201112 Azrul: For generate Quality Order purpose
        # region PRIVATE VARIABLES
        /*
        private int _nextNumber = 0;
        private string _screenname = "Surgical Pouch Printing (SPP)"; //To use for DB logging.
        private string _uiClassName = "SurgicalPouchPrinting (SPP)"; //To use for DB logging.
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
        //private DateTime? _receiptDateRequested = new DateTime();
        //private DateTime? _shippingDateRequested = new DateTime();
        private DateTime? _ShippingDateETD = new DateTime();
        private DateTime? _ManufacturingDateETD = new DateTime();
        private DateTime? _ProductionDate = new DateTime();

        //KahHeng 28Jan2019 added dateTime variables for PODate and POReceivedDate
        private DateTime? _HSB_CustPODocumentDate = new DateTime();
        private DateTime? _HSB_CustPORecvDate = new DateTime();
        //KahHeng End

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

        private string _leftSerialNumber = "Left Side";
        private string _rightSerialNumber = "Right Side";
        */
        #endregion
        public SurgicalPouchPrintingV2()
        {
            InitializeComponent();
        }

        private void SurgicalPouchPrinting_Load(object sender, EventArgs e)
        {
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
            if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
            {
                txtstation.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
                //txtInternalLotNumber.Text = GetInternalLotNumber();
            }
            else
                GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

            txtInnerPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
            BindPackingGroup();     
            //cmbPurchaseOrder.Focus();
            //initializePlaceHolderText();
            this.WindowState = FormWindowState.Maximized;
            txtInternalLotNumber.Focus();
        }

        private void BindPackingGroup()
        {
            List<DropdownDTO> lstDropDown = QCPackingYieldBLL.GetQCGroups(Constants.PACKING_GROUP_TYPE);
            cmbGroupId.BindComboBox(lstDropDown, true);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                txtInternalLotNumber.Focus();
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
                    //txtBatch.Text = String.Empty;
                    //txtBoxesPacked.Text = String.Empty;
                    //txtBatch2.Text = String.Empty;
                    //UpdatePlaceHolderText(true, false);
                    //UpdatePlaceHolderText(false, false);
                    //txtSerialNumber.Text = string.Empty;
                    //txtSerialNumber2.Text = String.Empty;
                    cmbGroupId.SelectedIndex = Constants.MINUSONE;
                    txtItemName.Text = String.Empty;
                    //cmbPurchaseOrder.Text = string.Empty;
                    //cmbPurchaseOrder.DataSource = null;
                    //cmbItemNumber.Items.Clear();
                    //cmbItemSize.Text = string.Empty;
                    //cmbItemSize.DataSource = null;

                    txtInternalLotNumber.Text = string.Empty;
                    txtPONumber.Text = string.Empty;
                    txtItemNo.Text = string.Empty;
                    txtItemName.Text = string.Empty;
                    txtItemSize.Text = string.Empty;
                    //txtInnerBox.Text = string.Empty;
                    txtInternalLotNumber.ReadOnly = false;
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(ex.Message, Constants.BUSINESSLOGIC, ex);
                }

                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                BindPackingGroup();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
        }

        private void txtPrint_Click(object sender, EventArgs e)
        {
            #region original code
            /*
            if (ValidateRequiredFields())
            {
                if (ValidateBatchCard_TOMS())
                {
                    FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
                    List<FinalPackingBatchInfoDTO> lstFPBatchInfoDTO = new List<FinalPackingBatchInfoDTO>();
                    FinalPackingBatchInfoDTO objFPSerialNumberInfoDTO1 = new FinalPackingBatchInfoDTO();
                    FinalPackingBatchInfoDTO objFPSerialNumberInfoDTO2 = new FinalPackingBatchInfoDTO();

                    // Avoid duplicate internallotnumber at workstation level
                    int lastRunningNo = 0;
                    if (FinalPackingBLL.CheckDuplicateInternalLotNumber(txtInternalLotNumber.Text, WorkStationDataConfiguration.GetInstance().stationNumber, out lastRunningNo))
                    {
                        txtInternalLotNumber.Text = GetInternalLotNumber(); // Get latest/updated internal lot number 
                    }

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
                    //KahHeng 29jan2019 added 2 new parameter for PODate  POReceviedDate 
                    objFinalPackingDTO.ManufacturingDate = FinalPackingBLL.GetManufacturingDate(_manufacturingOrderbasis, _ManufacturingDateETD, _ShippingDateETD, _ProductionDate, _HSB_CustPODocumentDate, _HSB_CustPORecvDate);
                    //KahHeng End
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
                                 _LineId, _expiry, _outersetLayOut, _customerLotNumber, _ManufacturingDateETD.Value, specialLotNumber, _innesetLayOut);

                            if (!string.IsNullOrEmpty(specialLotNumber))
                            {
                                objFinalPackingDTO.Internallotnumber = specialLotNumber;
                            }

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
            */
            #endregion

            SurgicalPackingPlanDTO data = FinalPackingBLL.SurgicalPackingPlan_Get(txtInternalLotNumber.Text);
            SOLineDTO objPODetails = FinalPackingBLL.GetPurchaseOrderSingle(data.PONumber, data.ItemNumber, data.ItemSize);
            FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();

            DateTime _productionDate = FinalPackingBLL.SurgicalPackingPlan_GetProductionDate(txtInternalLotNumber.Text);
            //Pang 09/02/2020 First Carton Packing Date
            DateTime _FirstManufacturingDate = FinalPackingBLL.GetFirstCartonPackingDate(data.PONumber, data.ItemNumber, data.ItemSize);

            objFinalPackingDTO.locationId = WorkStationDTO.GetInstance().LocationId;
            objFinalPackingDTO.Workstationnumber = WorkStationDTO.GetInstance().WorkStationId;
            objFinalPackingDTO.Printername = WorkStationDataConfiguration.GetInstance().innerPrinter;
            objFinalPackingDTO.Packdate = CommonBLL.GetCurrentDateAndTimeFromServer();
            objFinalPackingDTO.Internallotnumber = txtInternalLotNumber.Text.Trim();
            objFinalPackingDTO.Outerlotno = txtInternalLotNumber.Text.Trim();
            objFinalPackingDTO.Ponumber = objPODetails.PONumber;
            objFinalPackingDTO.ItemNumber = objPODetails.ItemNumber;
            objFinalPackingDTO.Size = objPODetails.ItemSize;
            
            objFinalPackingDTO.GroupId = Convert.ToInt32(cmbGroupId.SelectedValue);
            objFinalPackingDTO.Innersetlayout = objPODetails.InnersetLayout;
            objFinalPackingDTO.Outersetlayout = objPODetails.OuterSetLayout;
            objFinalPackingDTO.palletCapacity = objPODetails.PalletCapacity;
            objFinalPackingDTO.isTempPack = _isTempack;
            //KahHeng 29jan2019 added 2 new parameter for PODate  POReceviedDate 
            objFinalPackingDTO.ManufacturingDate = FinalPackingBLL.GetManufacturingDate(
                objPODetails.ManufacturingDateBasis, 
                objPODetails.ManufacturingDateETD,
                objPODetails.ShippingDateETD,
                _productionDate, //need double confirm on _productionDate
                objPODetails.CustPODocumentDate,
                objPODetails.CustPORecvDate,
                _FirstManufacturingDate);
            //KahHeng End
            objFinalPackingDTO.ExpiryDate = objFinalPackingDTO.ManufacturingDate.Value.AddMonths(objPODetails.Expiry);
            objFinalPackingDTO.InventTransId = objPODetails.InventTransId;


            objFinalPackingDTO.stationNumber = WorkStationDataConfiguration.GetInstance().stationNumber;
            objFinalPackingDTO.Casespacked = data.PlannedCases;
            objFinalPackingDTO.Boxespacked = data.PlannedCases * objPODetails.CaseCapacity;
            objFinalPackingDTO.FPStationNo = txtstation.Text;

            try
            {
                int rowsreturned = FinalPackingBLL.InsertFinalPackingSurgicalSPP(objFinalPackingDTO);
                if (rowsreturned > 0)
                {
                    // Start: update label set special requirement for mfgDate, expDate but not OVERWRITE INTERNAL-LOTNO
                    string specialLotNumber = FinalPackingBLL.UpdatespeciallotnumberforInner(
                        objFinalPackingDTO.Internallotnumber,
                        objFinalPackingDTO.ManufacturingDate.Value,
                        objFinalPackingDTO.ExpiryDate.Value,
                        "N/A", //_LineId, No special lot no for surgical 
                        objPODetails.Expiry,
                        objFinalPackingDTO.Innersetlayout,
                        objPODetails.CustomerLotNumber,
                        isSurgicalWorkOrder: true);

                    FinalPackingBLL.UpdatespeciallotnumberforOuter(
                        objFinalPackingDTO.Internallotnumber, 
                        objFinalPackingDTO.ManufacturingDate.Value, 
                        objFinalPackingDTO.ExpiryDate.Value,
                        "N/A", //_LineId, No special lot no for surgical 
                        objPODetails.Expiry,
                        objFinalPackingDTO.Outersetlayout, 
                        objPODetails.CustomerLotNumber,
                        objPODetails.ManufacturingDateETD,
                        string.Empty, // specialLotNumber
                        objFinalPackingDTO.Innersetlayout,
                        isSurgicalWorkOrder: true);

                    //if (!string.IsNullOrEmpty(specialLotNumber))
                    //{
                    //    objFinalPackingDTO.Internallotnumber = specialLotNumber;
                    //}
                    // END: update label set special requirement for mfgDate, expDate but not OVERWRITE INTERNAL-LOTNO


                    // Pang 09/02/2020 First Carton Packing Date
                    FinalPackingBLL.UpdateFirstCartonPackingDate(objFinalPackingDTO.Ponumber, objFinalPackingDTO.ItemNumber, objFinalPackingDTO.Size);


                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    try
                    {
                        FinalPackingPrint.InnerLabelPrint(objFinalPackingDTO.Internallotnumber);
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
            //txtInternalLotNumber.Text = GetInternalLotNumber();

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

        private void UpdatePODetails(SurgicalPackingPlanDTO data)
        {
            SOLineDTO objPODetails = FinalPackingBLL.GetPurchaseOrderSingle(data.PONumber, data.ItemNumber, data.ItemSize);
            //if (objPODetails != null)
            //{
            //    _size = objPODetails.ItemSize;
            //    _customerSize = objPODetails.CustomerSize;
            //    objPODetails.ItemSize = _size;
            //    _preshipmentPlan = objPODetails.PreshipmentPlan;
            //    _itemcases = objPODetails.ItemCases;
            //    _poNumber = objPODetails.PONumber;
            //    _itemNumber = objPODetails.ItemNumber;
            //    _casecapacity = objPODetails.CaseCapacity;
            //    _palletcapacity = objPODetails.PalletCapacity;
            //    _innerBoxCapacity = objPODetails.InnerBoxCapacity;
            //    _innesetLayOut = objPODetails.InnersetLayout;
            //    _outersetLayOut = objPODetails.OuterSetLayout;
            //}

            //20201112 Azrul: For generate Quality Order purpose START
            int preshipmentQty = FinalPackingBLL.GetPreshipmentCaseQty(objPODetails.ItemCases, objPODetails.PreshipmentPlan); //Get preshipment qunatity from Preshiment sampling plan.
            _preshipmentRandomNumbers = FinalPackingBLL.GenerateRandomNumbers(preshipmentQty, objPODetails.ItemCases); //Generate Random numbers
            if (!string.IsNullOrEmpty(_preshipmentRandomNumbers))
                objPODetails.Preshipmentcases = _preshipmentRandomNumbers;
            //20201112 Azrul: For generate Quality Order purpose END

            objPODetails.locationID = WorkStationDTO.GetInstance().LocationId;
            FinalPackingBLL.InsertPurchaseOrderDetails(objPODetails);//Insert Purchase Order details.
            InsertCaseNumbers(objPODetails);
        }

        private void InsertCaseNumbers(SOLineDTO line)
        {
            try
            {
                FinalPackingBLL.InsertCaseNumbers(line.ItemCases, line.PONumber, line.ItemNumber, line.ItemSize, string.Empty, WorkStationDTO.GetInstance().WorkStationId, WorkStationDTO.GetInstance().LocationId, line.InventTransId, line.CustomerSize);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "InsertCaseNumbers", null);
            }
        }

        private void txtInternalLotNumber_Leave(object sender, EventArgs e)
        {
            try
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
                else if ((data.PlanStatus == Constants.TWO) || (data.PlanStatus == Constants.THREE))
                {
                    GlobalMessageBox.Show(Messages.SPP_LOTNO_TWO, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                    txtInternalLotNumber.Focus();
                }
                else
                {
                    txtInternalLotNumber.ReadOnly = true;

                    txtPONumber.Text = data.CustomerReferenceNumber + " | " + data.PONumber;
                    txtItemNo.Text = data.ItemNumber;
                    txtItemName.Text = data.ItemBrand;
                    txtItemSize.Text = data.ItemSize;
                    //txtInnerBox.Text = (data.InnerBoxCapacity * data.CaseCapacity).ToString();
                    UpdatePODetails(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
