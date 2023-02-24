using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ReprintOuterBox : FormBase
    {
        #region CONSTANT
        private const string DISPLAY_MEMBER = "DisplayField";
        private const string VALUE_MEMBER = "IDField";
        #endregion

        #region PRIVATE variables
        List<SOLineDTO> lstPODetails = null;
        List<SOLineDTO> lstItemDetails = null;
        SOLineDTO objItemNumber = null;
        private string _screenname = "Re-print Outer Case";
        private string _uiClassName = "ReprintInnerBox";
        string _operatorName = String.Empty;
        int _itemType = Constants.ZERO;
        #endregion


        #region CONSTRUCTOR
        public ReprintOuterBox(string operatorName)
        {
            _operatorName = operatorName;
            InitializeComponent();
        }
        #endregion

        #region EVENTS
        /// <summary>
        /// TO VALIDATE OPERATORID AND DISPLAY FORM LOAD 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReprintOuterBox_Load(object sender, EventArgs e)
        {
            try
            {
                //Populate PO Combo Box
                PopulatePONumberComboBox();

                txtLocation.Text = WorkStationDTO.GetInstance().Location;
                txtStation.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
                txtPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
                this.WindowState = FormWindowState.Maximized;
                this.Show();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ReprintOuterBox_Load", null);
            }
        }

        /// <summary>
        /// FILTER PO SIZE BASED ON SELECTED ITEMNUMBER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbItemNo_SelectionChangeCommit(object sender, EventArgs e)
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
                    txtItemName.Text = objItemNumber.ItemName;
                    List<SOLineDTO> listItemSize = lstPODetails.Where(SOLineDTO => SOLineDTO.ItemNumber == itemNumber).ToList();                    
                    cmbSize.DataSource = listItemSize;
                    cmbSize.ValueMember = "ItemSize";
                    cmbSize.DisplayMember = "ItemSize";
                    cmbSize.SelectedIndex = Constants.MINUSONE;
                    _itemType = objItemNumber.ItemType;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbItemNo_SelectionChangeCommit", null);
            }
        }
        /// <summary>
        ///SAVE REPRINT DETAILS AND PRINT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRequiredFields())
                {
                    if (int.Parse(txtStartCase.Text) > int.Parse(txtEndCase.Text))
                    {
                        GlobalMessageBox.Show(Messages.CASEVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                    else
                    {
                        RePrintOuter objRePrintOuter = new RePrintOuter();
                        objRePrintOuter.LocationId = WorkStationDTO.GetInstance().LocationId;
                        objRePrintOuter.WorkStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                        objRePrintOuter.PrinterName = WorkStationDataConfiguration.GetInstance().innerPrinter;
                        objRePrintOuter.PONumber = cmbPONumber.SelectedValue.ToString().Trim();
                        objRePrintOuter.ItemNumber = cmbItemNumber.Text;
                        objRePrintOuter.ItemSize = cmbSize.Text;
                        objRePrintOuter.StartCase = int.Parse(txtStartCase.Text.Trim());
                        objRePrintOuter.EndCase = int.Parse(txtEndCase.Text);
                        objRePrintOuter.AuthorizedBy = _operatorName;
                        int rowsreturned = FinalPackingBLL.InsertFPReprintOuter(objRePrintOuter);
                        if (rowsreturned > Constants.ZERO)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            try
                            {
                                int nooflabelprint = CommonBLL.GetNoOfLabelPrintByItemNumber(objRePrintOuter.ItemNumber);
                                FinalPackingPrint.ReprintOuterLabelPrint(objRePrintOuter.PONumber, objRePrintOuter.ItemNumber, objRePrintOuter.ItemSize, objRePrintOuter.StartCase, objRePrintOuter.EndCase, null, 0, nooflabelprint);
                                this.DialogResult = DialogResult.OK;
                                this.Close(); // To close window after reprint success
                            }
                            catch (Exception ex)
                            {
                                throw new FloorSystemException(Messages.EXCEPTION_REPRINT_OUTERLABEL, ex);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                        ClearForm();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnPrint_Click", null);
            }
        }

        /// <summary>
        /// VALIDATE START CASE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStartCase_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtStartCase.Text))
                    if (Validator.IsValidInput(Framework.Constants.ValidationType.Integer, txtStartCase.Text))
                        ValidateStartandEndCase(int.Parse(txtStartCase.Text.Trim()), Constants.START);
                    else
                    {
                        GlobalMessageBox.Show(Messages.STARTCASEVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtStartCase.Text = string.Empty;
                        txtStartCase.Focus();
                    }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtStartCase_Leave", null);
            }
        }
        /// <summary>
        /// VALIDATE END CASE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEndCase_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtEndCase.Text))
                    if (Validator.IsValidInput(Framework.Constants.ValidationType.Integer, txtEndCase.Text))
                        ValidateStartandEndCase(int.Parse(txtEndCase.Text.Trim()), Constants.END);
                    else
                    {
                        GlobalMessageBox.Show(Messages.ENDCASEVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtEndCase.Text = string.Empty;
                        txtEndCase.Focus();
                    }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtEndCase_Leave", null);
            }
        }

        /// <summary>
        ///  CANCEL TO CLEAR THE FORM FIELDS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Leave(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
                txtPONumber.Focus();
            }
        }
        /// <summary>
        /// PO Combo Box Seletion Change Committed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPONumber_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbPONumber.SelectedValue != null)
                {
                    if (!string.IsNullOrEmpty(cmbPONumber.SelectedValue.ToString()))
                    {
                        lstPODetails = FinalPackingBLL.GetPurchaseOrderItemList(cmbPONumber.SelectedValue.ToString());
                        if (lstPODetails != null)
                        {
                            if (lstPODetails.Count > Constants.ZERO)
                            {
                                lstItemDetails = new List<SOLineDTO>();
                                cmbItemNumber.Items.Clear();
                                cmbItemNumber.SelectedIndexChanged -= cmbItemNo_SelectionChangeCommit;
                                foreach (SOLineDTO soline in lstPODetails)
                                {
                                    if (!cmbItemNumber.Items.Contains(soline.ItemNumber))
                                    {
                                        lstItemDetails.Add(soline);
                                        cmbItemNumber.Items.Add(soline.ItemNumber);
                                    }
                                }
                                cmbItemNumber.SelectedIndex = Constants.MINUSONE;
                                cmbItemNumber.SelectedIndexChanged += cmbItemNo_SelectionChangeCommit;
                            }
                            else
                            {
                                ClearForm();
                                cmbPONumber.Focus();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.INVALIDPO, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            ClearForm();
                            cmbPONumber.Focus();
                        }
                    }
                    else
                    {
                        ClearForm();
                        cmbPONumber.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbPONumber_SelectionChangeCommitted", null);
            }
        }
        #endregion

        #region  USER METHODS
        /// <summary>
        /// VALIDATE START AND END CASE
        /// </summary>
        /// <param name="caseNumber">START OR END CASENUMBER TO REPRINT</param>
        /// <param name="Case">START OR END CASE</param>
        private void ValidateStartandEndCase(int caseNumber, string Case)
        {
            FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO();
            if (string.IsNullOrEmpty(cmbPONumber.SelectedValue.ToString()))
            {
                GlobalMessageBox.Show(Messages.REQPONUMBER, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }
            else if (string.IsNullOrEmpty(cmbItemNumber.Text))
            {
                GlobalMessageBox.Show(Messages.REQITEMNUMBER, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }
            else if (string.IsNullOrEmpty(cmbSize.Text))
            {
                GlobalMessageBox.Show(Messages.REQSIZE, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    objFinalPackingDTO.Size = cmbSize.Text.Trim();
                    objFinalPackingDTO.ItemNumber = cmbItemNumber.Text;
                    objFinalPackingDTO.Ponumber = cmbPONumber.SelectedValue.ToString().Trim();
                    objFinalPackingDTO.CaseNumber = caseNumber;
                    if (FinalPackingBLL.ValidatestartandEndcase(objFinalPackingDTO))
                    {
                        //Valid case
                    }
                    else
                    {
                        if (string.Compare(Case, Constants.START) == 0)
                        {
                            GlobalMessageBox.Show(Messages.STARTCASEVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtStartCase.Focus();
                        }
                        else if (string.Compare(Case, Constants.END) == 0)
                        {
                            GlobalMessageBox.Show(Messages.ENDCASEVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtEndCase.Focus();
                        }
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenname, _uiClassName, "ValidateStartandEndCase", null);
                }
            }
        }
        /// <summary>
        /// REQUIRED FIELD VALIDATION
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPONumber, Messages.REQPONUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEMNUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbSize, Messages.REQUIRESIZE, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtStartCase, Messages.REQSTARTCASE, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtEndCase, Messages.REQENDCASE, ValidationType.Required));
            return ValidateForm();
        }
        /// <summary>
        ///  CLEAR THE FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            cmbPONumber.SelectedItem = null;
            cmbPONumber.Refresh();
            txtStartCase.Text = string.Empty;
            cmbItemNumber.Items.Clear();            
            cmbSize.DataSource = null;
            cmbSize.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtStartCase.Text = string.Empty;
            txtEndCase.Text = string.Empty;
            PopulatePONumberComboBox();
            cmbPONumber.Focus();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
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
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Populate PO Number Combo Box
        /// </summary>
        private void PopulatePONumberComboBox()
        {
            try
            { 
                int intDateRange = FloorSystemConfiguration.GetInstance().ReprintOuterCaseDayRange == null ? 0 : (int)FloorSystemConfiguration.GetInstance().ReprintOuterCaseDayRange;

                cmbPONumber.SelectedIndexChanged -= cmbPONumber_SelectedIndexChanged;
                List<DropdownDTO> lstSurgicalOrders = FinalPackingBLL.GetReprintOuterCasePOList(intDateRange).Distinct<DropdownDTO>().ToList();
                cmbPONumber.DataSource = lstSurgicalOrders;
                cmbPONumber.DisplayMember = DISPLAY_MEMBER;
                cmbPONumber.ValueMember = VALUE_MEMBER;
                cmbPONumber.SelectedItem = null;
                cmbPONumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbPONumber.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbPONumber.Refresh();
                cmbPONumber.SelectedIndexChanged += cmbPONumber_SelectedIndexChanged;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "PopulatePONumberComboBox", null);
            }
        }

        private void cmbPONumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPONumber_SelectionChangeCommitted(null, null);
        }
        # endregion

    }



}
