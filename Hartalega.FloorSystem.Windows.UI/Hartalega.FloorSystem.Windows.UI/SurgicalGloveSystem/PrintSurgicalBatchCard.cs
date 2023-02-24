#region using
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Hartalega.FloorSystem.Windows.UI.SurgicalGloveSystem
{
    public partial class PrintSurgicalBatchCard : FormBase
    {
        #region static Variables
        private static string _screenName = "Print Surgical Batch Card";
        private static string _className = "PrintSugicalBatchCard";
        private StringBuilder _sbSerialNumbers;
        protected bool ISPageValid;
        private int locationId = WorkStationDTO.GetInstance().LocationId;
        private static bool _chckBatchWeight = false;
        #endregion

        #region Load Form
        public PrintSurgicalBatchCard()
        {
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.SURGICALGLOVESYSTEM);
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.PRINTSURGICALBATCHCARD);
            InitializeComponent();
            LoadFormData();
            this.ActiveControl = txtOperatorId;
            txtOperatorId.OperatorId();
            cmbResource.Enabled = false;
            cmbBatchOrder.Enabled = false;
            txtQty.Text = "0"; //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0.
        }

        private void LoadFormData()
        {
            GetShift();
            GetSurgicalLine();
        }
        #endregion

        #region User Methods  

        /// <summary>
        ///#.List out all Shift
        /// </summary>
        private void GetShift()
        {
            List<DropdownDTO> shiftDTO = CommonBLL.GetShift(Framework.Constants.ShiftGroup.PN);
            string area = WorkStationDTO.GetInstance().Area;
            cmbShift.DataSource = SurgicalGloveBLL.GetShift(area);
            cmbShift.ValueMember = "IDField";
            cmbShift.DisplayMember = "DisplayField";
            cmbShift.Text = shiftDTO[Constants.ZERO].SelectedValue;
        }

        /// <summary>
        ///#1.List out all Resources Group (Production lines)
        /// </summary>
        private void GetSurgicalLine()
        {
            this.cmbLine.SelectedIndexChanged -= new EventHandler(cmbLine_SelectedIndexChanged);
            cmbLine.DataSource = SurgicalGloveBLL.GetSurgicalResource(locationId, null, null, null);
            cmbLine.ValueMember = "LineId";
            cmbLine.DisplayMember = "Line";
            this.cmbLine.SelectedIndexChanged += new EventHandler(cmbLine_SelectedIndexChanged);
        }
        
        /// <summary>
        ///#2.List out all Resource
        /// <summary>
        private void GetResource(string lineId)
        {
            cmbResource.ValueMember = null;
            cmbResource.DataSource = null;
            cmbResource.Items.Clear();
            cmbResource.ValueMember = "Resource";
            cmbResource.DisplayMember = "Resource";
            cmbResource.DataSource = SurgicalGloveBLL.GetSurgicalResource(locationId, lineId, null, null);
        }
        
        /// <summary>
        ///#3.List out all Batch Order based on selected Resource
        /// <summary>
        private void GetBatchOrder(string lineId, string resource)
        {
            cmbBatchOrder.Enabled = true;
            cmbBatchOrder.ValueMember = null;
            cmbBatchOrder.DataSource = null;
            cmbBatchOrder.Items.Clear();
            cmbBatchOrder.DataSource = SurgicalGloveBLL.GetSurgicalResource(locationId, lineId, resource, null);
            cmbBatchOrder.ValueMember = "BthOrderId";
            cmbBatchOrder.DisplayMember = "BthOrderId";
        }

        /// <summary>
        ///#.Default Batch Order when Batch Order list is 1 and default BO for to follow Output1
        /// <summary>
        private void SetDefaultBatchOrder(ComboBox cmbResource, ComboBox cmbBatchOrder)
        {
            if (Convert.ToInt32(cmbBatchOrder.Items.Count.ToString()) == 2)
            {
                cmbBatchOrder.SelectedIndex = 1;
                cmbBatchOrder.Enabled = false;
                cmbBatchOrder.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
            }
            if (!string.IsNullOrEmpty(cmbResource.Text) && cmbBatchOrder.Name != "cmbBatchOrder")
            {
                cmbBatchOrder.SelectedIndex = this.cmbBatchOrder.SelectedIndex;
                cmbBatchOrder.Enabled = false;
            }
        }
        
        /// <summary>
        ///#4.List out all Batch Order Details based on selected Resource and Batch Order
        /// <summary>
        private void GetBatchOrderDetails(string lineId, string resource, string batchOrder)
        {
            List<BatchOrderDetailsDTO> BODetailsForPrint = SurgicalGloveBLL.GetSurgicalResource(locationId, lineId, resource, batchOrder);
            txtGloveCode.Text = BODetailsForPrint[1].ItemId;
            txtGloveCode.Enabled = false;
            txtSize.Text = BODetailsForPrint[1].Size;
            txtSize.Enabled = false;
        }

        /// <summary>
        /// Clear text of the form controls
        /// </summary>
        private void ClearForm()
        {
            txtBatchWeight.Text = "";
            txtQty.Text = "0";
            cmbBatchOrder.DataSource = null;
            cmbResource.DataSource = null;
            cmbLine.SelectedIndex = 0;
            txtOperatorId.Text = "";
            txtOperatorId.Enabled = true;
            cmbLine.Enabled = true;
            txtOperatorId.Focus();
            lblBatchWeight.Text = "";
        }

        /// <summary>
        /// Validate form data before Print
        /// </summary>
        private bool FormValidate()
        {
            bool IsValidate = true;
            if (txtOperatorId.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = txtOperatorId;
                return IsValidate;
            }
            if (cmbShift.SelectedIndex == 0)
            {
                GlobalMessageBox.Show(Messages.SHIFT_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbShift;
                return IsValidate;
            }
            if (cmbLine.SelectedIndex == 0)
            {
                GlobalMessageBox.Show(Messages.PRODUCTIONLINE_NOT_SELECTED, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbLine;
                return IsValidate;
            }
            if (cmbResource.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.RESOURCE_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbResource;
                return IsValidate;
            }
            if (cmbBatchOrder.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHORDER_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (txtBatchWeight.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHKG_IS_EMPTY, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0. START
            //if (txtQty.Text.Length == 0)
            //{
            //    GlobalMessageBox.Show(Messages.QTYPCS_IS_EMPTY, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
            //    IsValidate = false;
            //    return IsValidate;
            //}
            //if (txtQty.Text == "0")
            //{
            //    GlobalMessageBox.Show(Messages.QTYPCS_IS_0, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
            //    IsValidate = false;
            //    return IsValidate;
            //}
            //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0. END
            if (txtBatchWeight.Text == "0")
            {
                GlobalMessageBox.Show(Messages.BATCHKG_IS_0, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }

            Decimal TotalGloveQty = 0;
            if (!(string.IsNullOrEmpty(cmbBatchOrder.Text)))
            {
                TotalGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder.Text + "'").ToString());
                if (TotalGloveQty == 0)
                {
                    GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    this.ActiveControl = btnCancel;
                    return IsValidate;
                }
            }
            return IsValidate;
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string result = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                ClearForm();
                this.ActiveControl = txtOperatorId;
            }
        }

        /// <summary>
        /// To call exception log method
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="UiClassName">UIClassName</param>
        /// <param name="uiControl">UIControl</param>
        /// <param name="parameters">Parameters</param>
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
        /// To get Batch Weight by calling the method in the Integration class through BLL
        /// </summary>
        /// <returns></returns>
        private void GetBatchWeight()
        {
            try
            {
                lblBatchWeight.Visible = true;
                lblBatchWeight.Text = Constants.CONNECTING;
                txtBatchWeight.Text = CommonBLL.GetBatchWeight().ToString("#,##0.00");
                lblBatchWeight.Visible = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBatchWeight", null);
                return;
            }
            lblBatchWeight.Visible = false;
        }

        /// <summary>
        /// To validate Batch Weight from WorkStation Specific values
        /// </summary>
        /// <returns></returns>
        private void ValidateBatchWeight()
        {
            if (Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().LWeight) && (Convert.ToDouble(txtBatchWeight.Text) < Convert.ToDouble(FloorSystemConfiguration.GetInstance().MinBatchWeight) || Convert.ToDouble(txtBatchWeight.Text) > Convert.ToDouble(FloorSystemConfiguration.GetInstance().MaxBatchWeight)))
            {
                GlobalMessageBox.Show(Constants.BATCHWEIGHT_RANGE, Constants.AlertType.Information, string.Empty, GlobalMessageBoxButtons.OK);
                lblBatchWeight.Visible = true;
                lblBatchWeight.Text = Constants.WEIGHT_OUT_OF_RANGE;
                _chckBatchWeight = true;
            }
            else
            {
                lblBatchWeight.Visible = false;
                lblBatchWeight.Text = String.Empty;
                _chckBatchWeight = false;
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// validate operator id and populate Operator name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            if (!string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
            {
                try
                {
                    if (!string.IsNullOrEmpty(txtOperatorId.Text))
                    {
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenName))
                        {
                            ISPageValid = true;
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Text = String.Empty;
                            txtOperatorId.Focus();
                        }
                    }
                    else
                    {
                        txtOperatorId.Text = string.Empty;
                        GlobalMessageBox.Show(Messages.QAI_SCREEN_ACCESS_MSG, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        txtOperatorId.Focus();
                    }
                }
                catch (FloorSystemException fsEX)
                {
                    ExceptionLogging(fsEX, _screenName, _className, "txtOperatorId_Leave", Convert.ToInt16(txtOperatorId.Text));
                    return;
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                txtOperatorId.Text = string.Empty;
                txtOperatorId.Focus();
            }

        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cmbLine.SelectedIndex == 0))
            {
                this.cmbResource.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource.Enabled = true;
                GetResource(cmbLine.SelectedValue.ToString());
                this.cmbResource.SelectedIndexChanged += new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource.SelectedIndex = 0;
            }
            else
            {
                cmbResource.DataSource = null;
                cmbResource.Enabled = false;
                cmbBatchOrder.DataSource = null;
                txtGloveCode.Text = "";
                txtSize.Text = "";
            }
        }
        
        private void cmbResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbResource.SelectedIndex > 0)
            {
                this.cmbBatchOrder.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                GetBatchOrder(cmbLine.SelectedValue.ToString(), cmbResource.SelectedValue.ToString());
                this.cmbBatchOrder.SelectedIndexChanged += new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                SetDefaultBatchOrder(cmbResource, cmbBatchOrder);
            }
            else
            {
                cmbBatchOrder.DataSource = null;
                txtGloveCode.Text = "";
                txtSize.Text = "";
            }
        }
        
        private void cmbBatchOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBatchOrder.SelectedIndex > 0)
            {
                GetBatchOrderDetails(cmbLine.SelectedValue.ToString(), cmbResource.SelectedValue.ToString(), cmbBatchOrder.SelectedValue.ToString());
            }
            else
            {
                txtGloveCode.Text = "";
                txtSize.Text = "";
            }
        }

        private void txtBatchWeight_Leave(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text != String.Empty)
            {
                //added by MYAdamas 11/10/2017 for input validation
                //Modified by Lakshman 7/1/2018 for Negative input validation
                decimal decim;
                if (Decimal.TryParse(txtBatchWeight.Text, out decim) && decim > 0)
                {
                    txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                    ValidateBatchWeight();
                    btnPrint.Focus();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtBatchWeight.Text = String.Empty;
                    txtBatchWeight.Focus();
                }
            }
        }

        /// <summary>
        /// Batch(Kg) field to weight scale integration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBatchWeight_Enter(object sender, EventArgs e)
        {
            if (txtBatchWeight.Text == String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txtBatchWeight.ReadOnly = true;
                    lblBatchWeight.Visible = true;
                    GetBatchWeight();
                    txtBatchWeight_Leave(sender, e);
                }
                else
                {
                    txtBatchWeight.ReadOnly = false;
                    txtBatchWeight.Focus();
                }
            }
        }

        private void txtBatchWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // limit to 3 decimal places
            int cursorPosition = (sender as TextBox).SelectionStart;
            string[] splitByDecimal = (sender as TextBox).Text.Split('.');

            if (!char.IsControl(e.KeyChar)
                && (sender as TextBox).Text.IndexOf('.') < cursorPosition
                && splitByDecimal.Length > 1
                && splitByDecimal[1].Length == 3)
            {
                e.Handled = true;
            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #endregion

        #region Print function
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            int authorizedFor = Constants.ZERO;
            txtOperatorId_Leave(txtOperatorId, e);
            if (FormValidate())
            {
                // Recheck functionality
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    GetBatchWeight();
                    decimal decim2;
                    if (Decimal.TryParse(txtBatchWeight.Text, out decim2) && decim2 > 0)
                    {
                        txtBatchWeight.Text = Convert.ToDouble(txtBatchWeight.Text).ToString("#,##0.00");
                        ValidateBatchWeight();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtBatchWeight.Text = String.Empty;
                        txtBatchWeight.Focus();
                        return;
                    }
                }

                if (GlobalMessageBox.Show(Messages.CONFIRM_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    if (_chckBatchWeight)
                    {
                        message = string.Format(Constants.BATCHWEIGHT_MESSAGE, txtBatchWeight.Text);
                        authorizedFor = Constants.THREE;
                    }
                    if (message != String.Empty)
                    {
                        Enabled = false;
                        Login passwordForm = new Login(Constants.Modules.SURGICALGLOVESYSTEM, _screenName, string.Empty, true, message, _chckBatchWeight);
                        passwordForm.Authentication = String.Empty;
                        passwordForm.IsCancel = false;
                        passwordForm.ShowDialog();
                        if (passwordForm.Authentication != String.Empty)
                        {
                            SaveBatchCard(passwordForm.Authentication, authorizedFor);
                            Enabled = true;
                            ClearForm();
                            txtOperatorId.Focus();
                        }
                        else if (passwordForm.Authentication == String.Empty && passwordForm.IsCancel != true)
                        {
                            Enabled = true;
                            ClearForm();
                            txtOperatorId.Focus();
                        }
                        else if (passwordForm.IsCancel == true)
                            Enabled = true;
                    }
                    else
                    {
                        Enabled = true;
                        SaveBatchCard(String.Empty, authorizedFor);
                        ClearForm();
                        txtOperatorId.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// Save Batch card details if valid, 
        /// the SP called in BLL also returns the serial number and batch number useful for printing
        /// </summary>
        /// <param name="authorizedBy"></param>
        /// <returns></returns>
        private void SaveBatchCard(string authorizedBy, int authorizedFor)
        {
            string userId = txtOperatorId.Text.ToString();
            int shiftId = Convert.ToInt32(cmbShift.SelectedValue);
            string line = cmbLine.SelectedValue.ToString();
            string resource = cmbResource.SelectedValue.ToString();
            string batchOrder = cmbBatchOrder.SelectedValue.ToString();
            string gloveCode = txtGloveCode.Text;
            string size = txtSize.Text;
            decimal batchWeight = Convert.ToDecimal(txtBatchWeight.Text);
            int quantity = Convert.ToInt32(txtQty.Text);
            DateTime batchCardDate = dateControl1.DateValue;
            string ModuleId = WorkStationDTO.GetInstance().Module;
            string SubModuleID = WorkStationDTO.GetInstance().SubModule;
            int sitenumber = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intSiteNumber);
            int WorkStationNumber = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);
            List<BatchOrderDetailsDTO> lstBatchDTO = new List<BatchOrderDetailsDTO>();
            try
            {
                lstBatchDTO = SurgicalGloveBLL.GetPrintBatchDetails(userId, shiftId, line, batchCardDate, ModuleId, SubModuleID, sitenumber, WorkStationNumber, resource, batchOrder, gloveCode, size, batchWeight, quantity, authorizedBy, authorizedFor);
                if (lstBatchDTO.Count > Constants.ZERO)
                {
                    this.BeginInvoke(new PrintAsync(PrintBatchAsync), lstBatchDTO);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnPrint_Click", null);
                return;
            }
        }

        private void PrintBatchAsync(List<BatchOrderDetailsDTO> lstBatchDTO)
        {
            _sbSerialNumbers = new StringBuilder("");
            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;
            List<PrintDTO> printdtolst = new List<PrintDTO>();
            foreach (BatchOrderDetailsDTO objbatchDTO in lstBatchDTO)
            {
                PrintDTO printdto = SurgicalGloveBLL.GetBatchList(objbatchDTO.BatchCarddate.ToString(), objbatchDTO.BatchNumber, objbatchDTO.BatchWeight.ToString(), objbatchDTO.Quantity.ToString(), objbatchDTO.SerialNumber, objbatchDTO.Size, objbatchDTO.GloveType, objbatchDTO.GloveCategory, objbatchDTO.Resource, Constants.SURGICALGLOVESYSTEM, false);
                printdtolst.Add(printdto);
                _sbSerialNumbers.Append(objbatchDTO.SerialNumber);
                _sbSerialNumbers.Append(Constants.COMMA);
            }
            PrintBatchCard(printdtolst);
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            ClearForm();
        }
        
        private void PrintBatchCard(List<PrintDTO> printdtoLst)
        {
            try
            {
                SurgicalGloveBLL.PrintDetails(printdtoLst);
            }
            catch (InvalidPrinterException)
            {
                PrinterClass.IsPrinterExists = false;
                GlobalMessageBox.Show(Messages.NO_PRINTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintBatchCard", printdtoLst);
                return;
            }
        }
        #endregion
    }
}