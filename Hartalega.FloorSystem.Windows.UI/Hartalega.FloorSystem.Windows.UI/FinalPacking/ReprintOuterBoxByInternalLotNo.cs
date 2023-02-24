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
    public partial class ReprintOuterBoxInternalLotNo : FormBase
    {
        #region CONSTANT
        private const string DISPLAY_MEMBER = "DisplayField";
        private const string VALUE_MEMBER = "IDField";
        #endregion

        # region PRIVATE variables
        List<SOLineDTO> lstPODetails = null;
        List<SOLineDTO> lstItemDetails = null;
        SOLineDTO objItemNumber = null;
        private string _screenname = "Re-print Outer Case By InternalLotNo";
        private string _uiClassName = "ReprintInnerBoxByInternalLotNo";
        string _operatorName = String.Empty;
        int _itemType = Constants.ZERO;
        #endregion

        #region CONSTRUCTOR
        public ReprintOuterBoxInternalLotNo(string operatorName)
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
        private void ReprintOuterBoxInternalLotNo_Load(object sender, EventArgs e)
        {
            try
            {
                //Populate PO Number Combo Box
                PopulatePONumberComboBox();

                txtLocation.Text = WorkStationDTO.GetInstance().Location;
                txtStation.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
                txtPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
                this.WindowState = FormWindowState.Maximized;
                this.Show();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ReprintOuterBoxByInternalLotNo_Load", null);
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
                    
                    RePrintOuter objRePrintOuter = new RePrintOuter();
                    objRePrintOuter.LocationId = WorkStationDTO.GetInstance().LocationId;
                    objRePrintOuter.WorkStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                    objRePrintOuter.PrinterName = WorkStationDataConfiguration.GetInstance().innerPrinter;
                    objRePrintOuter.PONumber = cmbPONumber.SelectedValue.ToString().Trim();
                    objRePrintOuter.ItemNumber = cmbItemNumber.Text;
                    objRePrintOuter.ItemSize = cmbSize.Text;
                    objRePrintOuter.StartCase = 0;
                    objRePrintOuter.EndCase = 0;
                    objRePrintOuter.NoOfCopy = int.Parse(txtCopyNo.Text);
                    objRePrintOuter.AuthorizedBy = _operatorName;
                    objRePrintOuter.InternalLotNo = txtInternalLotNo.Text;
                    objRePrintOuter.NoOfCopy = int.Parse(txtCopyNo.Text);
                    int rowsreturned = FinalPackingBLL.InsertFPReprintOuter(objRePrintOuter);
                    if (rowsreturned > Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        try
                        {

                            int nooflabelprint = CommonBLL.GetNoOfLabelPrintByInternalLotNumber(objRePrintOuter.InternalLotNo);
                            FinalPackingPrint.ReprintOuterLabelPrint(objRePrintOuter.PONumber, objRePrintOuter.ItemNumber, objRePrintOuter.ItemSize, objRePrintOuter.StartCase, objRePrintOuter.EndCase
                                , objRePrintOuter.InternalLotNo, objRePrintOuter.NoOfCopy, nooflabelprint);
                            this.DialogResult = DialogResult.OK;
                            this.Close();    // To close window after reprint success
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
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnPrint_Click", null);
            }
        }

        /// <summary>
        /// VALIDATE Internal Lot Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInternalLotNo_Leave(object sender, EventArgs e)
        {
            bool notValid = false;
            try
            {
                if(!string.IsNullOrEmpty(txtInternalLotNo.Text.Trim()))
                {
                    if (!InternalLotNoValidate())
                    {
                        GlobalMessageBox.Show(Messages.INTERNALLOTNUMBERVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        notValid = true;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INTERNALLOTNUMBERVALIDATION, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    notValid = true;
                }

                if(notValid)
                {
                    txtInternalLotNo.Text = string.Empty;
                    txtInternalLotNo.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtInternalLotNo_Leave", null);
            }
        }
        /// <summary>
        /// VALIDATE END CASE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCopyNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCopyNo.Text))
                    if (Validator.IsValidInput(Framework.Constants.ValidationType.Integer, txtCopyNo.Text))
                        ValidateAllInput();
                else
                {
                    GlobalMessageBox.Show(Messages.NOCOPYVALIDATION, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    txtCopyNo.Text = string.Empty;
                    txtCopyNo.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "txtCopyNo_Leave", null);
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
                cmbPONumber.Focus();
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

        private void cmbPONumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPONumber_SelectionChangeCommitted(null, null);
        }
        #endregion

        #region  USER METHODS
        /// <summary>
        /// VALIDATE START AND END CASE
        /// </summary>
        /// <param name="caseNumber">START OR END CASENUMBER TO REPRINT</param>
        /// <param name="Case">START OR END CASE</param>
        private void ValidateAllInput()
        {
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
            else if (string.IsNullOrEmpty(txtInternalLotNo.Text))
            {
                GlobalMessageBox.Show(Messages.INTERNALLOTNUMBERVALIDATION, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }
            else if (string.IsNullOrEmpty(txtCopyNo.Text))
            {
                GlobalMessageBox.Show(Messages.NOCOPYVALIDATION, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }
            else
            {
                if (!InternalLotNoValidate())
                {
                    GlobalMessageBox.Show(Messages.INTERNALLOTNUMBERVALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
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
            validationMesssageLst.Add(new ValidationMessage(txtInternalLotNo, Messages.REQINTERNALLOTNUMBER, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtCopyNo, Messages.REQCOPYNO, ValidationType.Required));
            return ValidateForm();
        }
        /// <summary>
        ///  CLEAR THE FORM CONTROLS
        /// </summary>
        private void ClearForm()
        {
            cmbPONumber.SelectedItem = null;
            cmbPONumber.Refresh();
            txtInternalLotNo.Text = string.Empty;
            cmbItemNumber.Items.Clear();            
            cmbSize.DataSource = null;
            cmbSize.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtInternalLotNo.Text = string.Empty;
            txtCopyNo.Text = string.Empty;
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
        #endregion

        private void txtCopyNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private bool InternalLotNoValidate()
        {
            try
            {
                FinalPackingDTO objFinalPackingDTO = new FinalPackingDTO
                {
                    Size                = cmbSize.Text.Trim(),
                    ItemNumber          = cmbItemNumber.Text,
                    Ponumber            = cmbPONumber.SelectedValue.ToString().Trim(),
                    Internallotnumber   = txtInternalLotNo.Text.Trim()
                };

                return FinalPackingBLL.ValidateInternalLotNo(objFinalPackingDTO);              
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "InternalLotNoValidate", null);
            }

            return false;
        }
    }



}
