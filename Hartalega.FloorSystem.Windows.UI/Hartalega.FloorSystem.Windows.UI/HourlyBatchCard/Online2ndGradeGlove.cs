#region using
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
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

namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public partial class Online2ndGradeGlove : FormBase
    {
        #region  private Variables
        private static string _screenName = "Online 2nd Grade Glove";
        private static string _className = "Online2ndGradeGlove";
        private StringBuilder _sbSerialNumbers;
        protected bool ISPageValid;
        private int locationId = WorkStationDTO.GetInstance().LocationId;
        #endregion

        #region Load Form
        public Online2ndGradeGlove()
        {
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.ONLINE_2G);
            InitializeComponent();
            LoadFormData();
            this.ActiveControl = txtOperatorId;
            txtOperatorId.OperatorId();
            cmbGloveCode.Enabled = false;
            cmbSize.Enabled = false;
            cmbGloveBatchorder.Enabled = false;
            cmbPackingSize.Enabled = false;
        }

        private void LoadFormData()
        {
            GetShift();
            Get2GGloveLine();
            BindPackingSize();
        }
        #endregion

        #region User Methods  

        /// <summary>
        ///#.List out all Shift
        /// </summary>
        private void GetShift()
        {
            List<DropdownDTO> shiftDTO = CommonBLL.GetShift(Framework.Constants.ShiftGroup.QC);
            string area = WorkStationDTO.GetInstance().Area;
            cmbShift.DataSource = HourlyBatchCardBLL.GetShiftInfo(area);
            cmbShift.ValueMember = "IDField";
            cmbShift.DisplayMember = "DisplayField";
            cmbShift.Text = shiftDTO[Constants.ZERO].SelectedValue;
        }

        /// <summary>
        ///#1.List out all Resources Group (Production lines)
        /// </summary>
        private void Get2GGloveLine()
        {
            this.cmbLine.SelectedIndexChanged -= new EventHandler(cmbLine_SelectedIndexChanged);
            cmbLine.DataSource = HourlyBatchCardBLL.GetOnline2GResources(locationId, null, null, null);
            cmbLine.ValueMember = "LineId";
            cmbLine.DisplayMember = "LineId";
            this.cmbLine.SelectedIndexChanged += new EventHandler(cmbLine_SelectedIndexChanged);
        }
        
        /// <summary>
        ///#2.List out all Glove Code based on selected Line
        /// <summary>
        private void GetGloveCode(string lineId)
        {
            cmbGloveCode.Enabled = true;
            cmbGloveCode.ValueMember = null;
            cmbGloveCode.DataSource = null;
            cmbGloveCode.Items.Clear();
            cmbGloveCode.DataSource = HourlyBatchCardBLL.GetOnline2GResources(locationId, lineId, null, null);
            cmbGloveCode.ValueMember = "ItemId";
            cmbGloveCode.DisplayMember = "ItemId";
        }

        /// <summary>
        ///#3.List out all Size based on selected Line & Glove Code
        /// <summary>
        private void GetSize(string lineId, string gloveCode)
        {
            cmbSize.Enabled = true;
            cmbSize.ValueMember = null;
            cmbSize.DataSource = null;
            cmbSize.Items.Clear();
            cmbSize.DataSource = HourlyBatchCardBLL.GetOnline2GResources(locationId, lineId, gloveCode, null);
            cmbSize.ValueMember = "Size";
            cmbSize.DisplayMember = "Size";
        }

        /// <summary>
        ///#4.List out all Batch Order based on selected Line, Glove Code & Size
        /// <summary>
        private void GetGloveBatchOrder(string lineId, string gloveCode, string size)
        {
            cmbGloveBatchorder.Enabled = true;
            cmbGloveBatchorder.ValueMember = null;
            cmbGloveBatchorder.DataSource = null;
            cmbGloveBatchorder.Items.Clear();
            cmbGloveBatchorder.DataSource = HourlyBatchCardBLL.GetOnline2GResources(locationId, lineId, gloveCode, size);
            cmbGloveBatchorder.ValueMember = "LineId"; //Get Resource to be saved.
            cmbGloveBatchorder.DisplayMember = "BthOrderId";
        }

        private void BindPackingSize()
        {
            cmbPackingSize.BindComboBox(CommonBLL.GetPackingSize(), true);
        }

        /// <summary>
        /// Clear text of the form controls
        /// </summary>
        private void ClearForm()
        {
            txtOperatorId.Text = "";
            txt10PcsWeight.Text = "";
            txtInnerBox.Text = "";
            cmbLine.SelectedIndex = 0;
            txtOperatorId.Focus();
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
            //if (cmbResource.Text.Length == 0)
            //{
            //    GlobalMessageBox.Show(Messages.RESOURCE_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
            //    IsValidate = false;
            //    this.ActiveControl = cmbResource;
            //    return IsValidate;
            //}
            //if (cmbBatchOrder.Text.Length == 0)
            //{
            //    GlobalMessageBox.Show(Messages.BATCHORDER_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
            //    IsValidate = false;
            //    return IsValidate;
            //}
            if (txtInnerBox.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHKG_IS_EMPTY, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (txt10PcsWeight.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.QTYPCS_IS_EMPTY, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (txtInnerBox.Text == "0")
            {
                GlobalMessageBox.Show(Messages.BATCHKG_IS_0, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (txt10PcsWeight.Text == "0")
            {
                GlobalMessageBox.Show(Messages.QTYPCS_IS_0, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }

            Decimal TotalGloveQty = 0;
            //if (!(string.IsNullOrEmpty(cmbBatchOrder.Text)))
            //{
            //    TotalGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder.Text + "'").ToString());
            //    if (TotalGloveQty == 0)
            //    {
            //        GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
            //        IsValidate = false;
            //        this.ActiveControl = btnCancel;
            //        return IsValidate;
            //    }
            //}
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
                this.cmbGloveCode.SelectedIndexChanged -= new EventHandler(cmbGloveCode_SelectedIndexChanged);
                cmbGloveCode.Enabled = true;
                GetGloveCode(cmbLine.SelectedValue.ToString());
                this.cmbGloveCode.SelectedIndexChanged += new EventHandler(cmbGloveCode_SelectedIndexChanged);
                cmbGloveCode.SelectedIndex = 0;
            }
            else
            {
                cmbGloveCode.DataSource = null;
                cmbGloveCode.Enabled = false;
                cmbSize.DataSource = null;
                cmbSize.Enabled = false;
                cmbGloveBatchorder.DataSource = null;
                cmbGloveBatchorder.Enabled = false;
                cmbPackingSize.SelectedValue = 0;
                cmbPackingSize.Enabled = false;
                txtInnerBox.Text = "";
                txt10PcsWeight.Text = "";
                txtResource.Text = "";
            }
        }
        
        private void cmbGloveCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGloveCode.SelectedIndex > 0)
            {
                this.cmbSize.SelectedIndexChanged -= new EventHandler(cmbSize_SelectedIndexChanged);
                GetSize(cmbLine.SelectedValue.ToString(), cmbGloveCode.SelectedValue.ToString());
                this.cmbSize.SelectedIndexChanged += new EventHandler(cmbSize_SelectedIndexChanged);
            }
            else
            {
                cmbSize.DataSource = null;
                cmbSize.Enabled = false;
                cmbGloveBatchorder.DataSource = null;
                cmbGloveBatchorder.Enabled = false;
                cmbPackingSize.SelectedValue = 0;
                cmbPackingSize.Enabled = false;
                txtInnerBox.Text = "";
                txt10PcsWeight.Text = "";
                txtResource.Text = "";
            }
        }
        
        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSize.SelectedIndex > 0)
            {
                this.cmbGloveBatchorder.SelectedIndexChanged -= new EventHandler(cmbGloveBatchorder_SelectedIndexChanged);
                GetGloveBatchOrder(cmbLine.SelectedValue.ToString(), cmbGloveCode.SelectedValue.ToString(), cmbSize.SelectedValue.ToString());
                this.cmbGloveBatchorder.SelectedIndexChanged += new EventHandler(cmbGloveBatchorder_SelectedIndexChanged);
            }
            else
            {
                cmbGloveBatchorder.DataSource = null;
                cmbGloveBatchorder.Enabled = false;
                cmbPackingSize.SelectedValue = 0;
                cmbPackingSize.Enabled = false;
                txtInnerBox.Text = "";
                txt10PcsWeight.Text = "";
                txtResource.Text = "";
            }
        }

        private void cmbGloveBatchorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGloveBatchorder.SelectedIndex > 0)
            {
                cmbPackingSize.Enabled = true;
                txtResource.Text = cmbGloveBatchorder.SelectedValue.ToString();
            }
            else
            {
                cmbPackingSize.SelectedValue = 0;
                cmbPackingSize.Enabled = false;
                txtInnerBox.Text = "";
                txt10PcsWeight.Text = "";
                txtResource.Text = "";
            }
        }

        private void txt10PcsWeight_Enter(object sender, EventArgs e)
        {
            if (txt10PcsWeight.Text == String.Empty)
            {
                if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
                {
                    txt10PcsWeight.ReadOnly = true;
                    txt10PcsWeight.Text = CommonBLL.GetTenPcsWeight().ToString("#,##0.00");
                }
                else
                {
                    txt10PcsWeight.ReadOnly = false;
                    txt10PcsWeight.Focus();
                }
            }
        }

        private void txtInnerBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txt10PcsWeight_KeyPress(object sender, KeyPressEventArgs e)
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
        #endregion

        #region Print function
        private void btnPrint_Click(object sender, EventArgs e)
        {
            txtOperatorId_Leave(txtOperatorId, e);
            if (FormValidate())
            {
                string result = string.Empty;
                result = GlobalMessageBox.Show(Messages.CONFIRM_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                if (result == Constants.YES)
                {
                    Login _passwordForm = new Login(Constants.Modules.BATCHORDER, _screenName);
                    _passwordForm.ShowDialog();
                    if (!string.IsNullOrEmpty(_passwordForm.Authentication))
                    {
                        string userId = txtOperatorId.Text.ToString();
                        int shiftId = Convert.ToInt32(cmbShift.SelectedValue);
                        string line = cmbLine.SelectedValue.ToString();
                        string gloveCode = cmbGloveCode.SelectedValue.ToString();
                        string size = cmbSize.SelectedValue.ToString();
                        string batchOrder = cmbGloveBatchorder.Text;
                        string resource = cmbGloveBatchorder.SelectedValue.ToString();
                        int packingSize = Convert.ToInt32(cmbPackingSize.SelectedValue.ToString());
                        int innerBox = Convert.ToInt32(txtInnerBox.Text);
                        decimal tenPcsWeight = Convert.ToDecimal(txt10PcsWeight.Text);
                        DateTime batchCardDate = dateControl1.DateValue;
                        string moduleId = WorkStationDTO.GetInstance().Module;
                        string subModuleId = WorkStationDTO.GetInstance().SubModule;
                        int sitenumber = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intSiteNumber);
                        int workStationNumber = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);
                        List<BatchDTO> lstBatchDTO = new List<BatchDTO>();
                        try
                        {
                            lstBatchDTO = HourlyBatchCardBLL.GetPrintOnline2GDetails(userId, shiftId, line, gloveCode, size, batchOrder, packingSize, innerBox, tenPcsWeight, batchCardDate, moduleId, subModuleId, sitenumber, workStationNumber, resource);
                            if (lstBatchDTO.Count > Constants.ZERO)
                            {
                                foreach (BatchDTO objbatchDTO in lstBatchDTO)
                                {
                                    AXPostingBLL.PostAXOnline2GBatchCard(objbatchDTO);
                                }
                                this.BeginInvoke(new PrintAsyncON2G(PrintBatchAsync), lstBatchDTO);
                            }
                        }
                        catch (FloorSystemException ex)
                        {
                            ExceptionLogging(ex, _screenName, _className, "btnPrint_Click", null);
                            return;
                        }
                    }
                }
            }
        }

        private void PrintBatchAsync(List<BatchDTO> lstBatchDTO)
        {
            _sbSerialNumbers = new StringBuilder("");
            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;
            List<PrintDTO> printdtolst = new List<PrintDTO>();
            foreach (BatchDTO objbatchDTO in lstBatchDTO)
            {
                PrintDTO printdto = HourlyBatchCardBLL.GetBatchList(objbatchDTO.BatchCarddate.ToString(), objbatchDTO.SerialNumber, objbatchDTO.BatchNumber, objbatchDTO.Resource, objbatchDTO.Size, objbatchDTO.GloveType, objbatchDTO.PackingSize.ToString(), objbatchDTO.InnerBoxCount.ToString(), objbatchDTO.Quantity.ToString(), Constants.ONLINE_2G, false);
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
                HourlyBatchCardBLL.PrintDetails(printdtoLst);
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