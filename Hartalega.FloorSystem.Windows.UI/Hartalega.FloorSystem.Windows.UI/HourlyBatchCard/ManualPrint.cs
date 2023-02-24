#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public partial class ManualPrint : FormBase
    {

        #region  Varibale
        private static string _screenName = "Batch Card Manual Print";
        private static string _className = "ManualPrint";
        private LineSelectionReprint _rePrintLineSelection;
        private StringBuilder _sbSerialNumbers;

        public string OperatorId { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region constructors

        public ManualPrint()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ManualPrint", null);
            }
            LoadFormData();
            txtSerialNumber.SerialNo();
            if (!Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().bool_HBCCanShowSerialNo))
            {
                label4.Visible = false;
                txtSerialNumber.Visible = false;
            }

        }

        public ManualPrint(string lineId)
        {
            InitializeComponent();

            txtSerialNumber.SerialNo();
            try
            {
                _rePrintLineSelection = HourlyBatchCardBLL.GetTierSideGloveSize(lineId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "frmManualPrint", lineId);
                return;
            }
            LoadFormData();
        }

        #endregion

        #region Load Form

        private void frmManualPrint_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// OK Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            //validation rules changes based on dropdown events.
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.LINESELECTION);

            validationMesssageLst = new List<ValidationMessage>();
            if (txtSerialNumber.Enabled)
            {
                validationMesssageLst.Add(new ValidationMessage(txtSerialNumber, "Serial Number", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbBoxReasonRePrint, "Reason for RePrint", ValidationType.Required));
            }
            if (cmbBoxHour.Enabled)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbBoxHour, "Hour", ValidationType.Required));
            }
            if (cmbBoxLine.Enabled)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbBoxLine, "Line", ValidationType.Required));
            }

            StringBuilder tiesrside = new StringBuilder("");
            _sbSerialNumbers = new StringBuilder("");
            if (!txtSerialNumber.Enabled && txtSerialNumber.Text.Trim().Length == Constants.ZERO)
            {
                bool isvalid = true;
                if (cmbBoxLine.SelectedIndex == Constants.ZERO)
                {
                    isvalid = false;
                }
                if (cmbBoxReasonRePrint.SelectedIndex == Constants.ZERO)
                {
                    isvalid = false;
                }
                if (chkLT.Checked)
                {
                    tiesrside.Append(Hartalega.FloorSystem.Framework.Constants.Tierside.LT + Constants.COMMA);
                }
                if (chkLB.Checked)
                {
                    tiesrside.Append(Hartalega.FloorSystem.Framework.Constants.Tierside.LB + Constants.COMMA);
                }
                if (chkRT.Checked)
                {
                    tiesrside.Append(Hartalega.FloorSystem.Framework.Constants.Tierside.RT + Constants.COMMA);
                }
                if (chkRB.Checked)
                {
                    tiesrside.Append(Hartalega.FloorSystem.Framework.Constants.Tierside.RB + Constants.COMMA);
                }
                if (string.IsNullOrEmpty(tiesrside.ToString()))
                {
                    CheckBox chk = null;
                    if (chkRB.Enabled && (!chkRB.Checked))
                    {
                        chk = chkRB;
                    }
                    if (chkRT.Enabled && (!chkRT.Checked))
                    {
                        chk = chkRT;
                    }
                    if (chkLB.Enabled && (!chkLB.Checked))
                    {
                        chk = chkLB;
                    }
                    if (chkLT.Enabled && (!chkLT.Checked))
                    {
                        chk = chkLT;
                    }
                    if (chk != null)
                    {
                        validationMesssageLst.Add(new ValidationMessage(chk, "Select at least one Tier side", ValidationType.Custom));
                    }
                }

                validationMesssageLst.Add(new ValidationMessage(cmbBoxReasonRePrint, "Reason for RePrint", ValidationType.Required));
                if (!ValidateForm())
                {

                    return;
                }
            }
            else if ((txtSerialNumber.Enabled && txtSerialNumber.Text.Trim().Length == Constants.ZERO) && cmbBoxReasonRePrint.SelectedIndex == Constants.ZERO)
            {
                GlobalMessageBox.Show(Messages.ENTER_SERIAL_NUMBER_OR_SELECT_LINE, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                return;
            }
            else if (!ValidateForm())
            {
                return;
            }
            List<BatchDTO> lstBatchDTO = new List<BatchDTO>();
            try
            {
                string SelectedTies = tiesrside.ToString().Length > Constants.ZERO ? tiesrside.ToString().Substring(Constants.ZERO, tiesrside.ToString().Length - 1) : string.Empty;

                if (txtSerialNumber.Text.Trim().Length == Constants.ZERO)
                {
                    lstBatchDTO = HourlyBatchCardBLL.ManualPrintBatchDetailsGet(Convert.ToString(cmbBoxLine.SelectedValue), Convert.ToInt32(cmbBoxHour.SelectedValue), SelectedTies, ServerCurrentDateTime, WorkStationDTO.GetInstance().Module, WorkStationDTO.GetInstance().SubModule, Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId));
                }
                else
                {
                    lstBatchDTO = HourlyBatchCardBLL.ManualPrintBatchDetailsBySerialNumberGet(txtSerialNumber.Text.Trim());
                }
                if (lstBatchDTO.Count > Constants.ZERO)
                {

                    if (!PrinterClass.IsPrinterExists)
                    {
                        GlobalMessageBox.Show(Messages.NO_PRINTER, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    }
                    else
                    {
                        this.BeginInvoke(new PrintAsync(PrintBatchAsync), lstBatchDTO);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.NO_BATCH_CARDS_AVAILABLE, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnOk_Click", null);
                return;
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.C)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBoxHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
            try
            {
                if (string.IsNullOrEmpty(cmbBoxLine.Text))
                {
                    GlobalMessageBox.Show(Messages.PRODUCTIONLINE_NOT_SELECTED, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }
                _rePrintLineSelection = HourlyBatchCardBLL.GetTierSideGloveSize(Convert.ToString(cmbBoxLine.SelectedValue));
                BindTierDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbBoxHour_SelectedIndexChanged", null);
                return;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool state = txtSerialNumber.Text.Trim().Length > Constants.ZERO ? true : false;
            chkLB.Enabled = !state;
            chkRB.Enabled = !state;
            chkLT.Enabled = !state;
            chkRT.Enabled = !state;
            cmbBoxHour.Enabled = !state;
            cmbBoxLine.Enabled = !state;
        }

        private void chkLT_CheckedChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
        }

        private void chkLB_CheckedChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
        }

        private void chkRT_CheckedChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
        }

        private void chkRB_CheckedChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
        }

        private void cmbBoxReasonRePrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
        }

        private void cmbBoxLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSerialNumber.Enabled = false;
        }

        private void txtSerialNumber_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSerialNumber.Text))
            {
                if (!HourlyBatchCardBLL.IsSerialNumber(Convert.ToDecimal(txtSerialNumber.Text.Trim())))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    txtSerialNumber.Text = string.Empty;
                    txtSerialNumber.Focus();
                }
                else
                {
                    chkLT.Checked = false;
                    chkLB.Checked = false;
                    chkRT.Checked = false;
                    chkRB.Checked = false;
                    txtLTSize.Text = "";
                    txtLBSize.Text = "";
                    txtRTSize.Text = "";
                    txtRBSize.Text = "";

                    List<BatchDTO> lstBatchDTO = new List<BatchDTO>();
                    lstBatchDTO = HourlyBatchCardBLL.ManualPrintBatchDetailsBySerialNumberGet(txtSerialNumber.Text.Trim());

                    if (lstBatchDTO.Count > Constants.ZERO)
                    {
                        foreach (BatchDTO objbatchDTO in lstBatchDTO)
                        {
                            // Not allow reprint for Serial Number that not belongs to this plant
                            cmbBoxLine.SelectedValue = objbatchDTO.Line;

                            if (cmbBoxLine.Text == "")
                            {
                                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_PLANT, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                                txtSerialNumber.Text = string.Empty;
                                txtSerialNumber.Enabled = true;
                                txtSerialNumber.Focus();
                                return;
                            }
                            else
                            {
                                var sTier = objbatchDTO.TierSide;

                                switch (sTier.ToUpper().Trim())
                                {
                                    case "L":
                                        chkLT.Checked = true;
                                        chkLB.Checked = true;
                                        txtLTSize.Text = objbatchDTO.Size;
                                        txtLBSize.Text = objbatchDTO.Size;
                                        break;
                                    case "LT":
                                        chkLT.Checked = true;
                                        txtLTSize.Text = objbatchDTO.Size;
                                        break;
                                    case "LB":
                                        chkLB.Checked = true;
                                        txtLBSize.Text = objbatchDTO.Size;
                                        break;
                                    case "R":
                                        chkRT.Checked = true;
                                        chkRB.Checked = true;
                                        txtRTSize.Text = objbatchDTO.Size;
                                        txtRBSize.Text = objbatchDTO.Size;
                                        break;
                                    case "RT":
                                        chkRT.Checked = true;
                                        txtRTSize.Text = objbatchDTO.Size;
                                        break;
                                    case "RB":
                                        chkRB.Checked = true;
                                        txtRBSize.Text = objbatchDTO.Size;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            txtSerialNumber.ReadOnly = true;
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        txtSerialNumber.Text = string.Empty;
                        txtSerialNumber.Focus();
                    }
                }

                txtSerialNumber.Enabled = true;
            }
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Get Line Details
        /// </summary>
        private void GetLine()
        {
            try
            {
                this.cmbBoxLine.SelectedIndexChanged -= new EventHandler(cmbBoxLine_SelectedIndexChanged);
                cmbBoxLine.DataSource = HourlyBatchCardBLL.GetLine(System.Windows.Forms.SystemInformation.ComputerName);
                cmbBoxLine.ValueMember = "IDField";
                cmbBoxLine.DisplayMember = "DisplayField";
                if (_rePrintLineSelection != null)
                {
                    cmbBoxLine.Text = _rePrintLineSelection.LineId;
                }
                else
                {
                    cmbBoxLine.SelectedIndex = Constants.MINUSONE;
                }
                this.cmbBoxLine.SelectedIndexChanged += new EventHandler(cmbBoxLine_SelectedIndexChanged);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getLine", null);
                return;
            }
        }

        /// <summary>
        /// Get Reason for reprint
        /// </summary>
        private void GetReasonForReprint()
        {
            try
            {
                this.cmbBoxReasonRePrint.SelectedIndexChanged -= new EventHandler(cmbBoxReasonRePrint_SelectedIndexChanged);
                cmbBoxReasonRePrint.DataSource = HourlyBatchCardBLL.GetReasonForReprint();
                cmbBoxReasonRePrint.ValueMember = "IDField";
                cmbBoxReasonRePrint.DisplayMember = "DisplayField";
                this.cmbBoxReasonRePrint.SelectedIndexChanged += new EventHandler(cmbBoxReasonRePrint_SelectedIndexChanged);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getReasonForReprint", null);
                return;
            }
        }

        /// <summary>
        /// Get Last Reprint hours
        /// </summary>
        private void GetLastReprinthrs()
        {
            this.cmbBoxHour.SelectedIndexChanged -= new EventHandler(cmbBoxHour_SelectedIndexChanged);
            cmbBoxHour.DataSource = HourlyBatchCardBLL.GetLastReprinthrs();
            cmbBoxHour.ValueMember = "IDField";
            cmbBoxHour.DisplayMember = "DisplayField";
            cmbBoxHour.SelectedIndex = Constants.MINUSONE;
            this.cmbBoxHour.SelectedIndexChanged += new EventHandler(cmbBoxHour_SelectedIndexChanged);
        }

        /// <summary>
        /// Load Form data
        /// </summary>
        private void LoadFormData()
        {
            GetLine();
            GetLastReprinthrs();
            GetReasonForReprint();
            BindTierDetails();
        }

        /// <summary>
        /// Bind line details
        /// </summary>
        private void BindTierDetails()
        {
            CleaseSelection();
            if (_rePrintLineSelection != null)
            {
                if (!_rePrintLineSelection.IsDoubleFormer)
                {
                    chkLT.Enabled = true;
                    chkRT.Enabled = true;
                    txtLTSize.Text = _rePrintLineSelection.LTSize;
                    txtRTSize.Text = _rePrintLineSelection.RTSize;
                }
                else
                {
                    chkLT.Enabled = true;
                    chkRT.Enabled = true;
                    chkLB.Enabled = true;
                    chkRB.Enabled = true;

                    txtLTSize.Text = _rePrintLineSelection.LTSize;
                    txtRTSize.Text = _rePrintLineSelection.RTSize;
                    txtLBSize.Text = _rePrintLineSelection.LBSize;
                    txtRBSize.Text = _rePrintLineSelection.RBSize;
                }
            }

        }

        /// <summary>
        /// PrintBatchCard
        /// </summary>
        /// <param name="objbatchDTO"></param>
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

        /// <summary>
        /// Clear all the fields
        /// </summary>
        private void CleaseSelection()
        {
            chkLB.Checked = false;
            chkLT.Checked = false;
            chkRB.Checked = false;
            chkRT.Checked = false;
            txtLBSize.Text = string.Empty;
            txtRBSize.Text = string.Empty;
            txtRTSize.Text = string.Empty;
            txtLBSize.Text = string.Empty;
            txtLTSize.Text = string.Empty;
            chkLT.Enabled = false;
            chkRT.Enabled = false;
            chkLB.Enabled = false;
            chkRB.Enabled = false;
            cmbBoxReasonRePrint.Text = string.Empty;
        }


        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
        }


        private void PrintBatchAsync(List<BatchDTO> lstBatchDTO)
        {
            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;
            List<PrintDTO> printdtolst = new List<PrintDTO>();
            foreach (BatchDTO objbatchDTO in lstBatchDTO)
            {
                PrintDTO printdto = HourlyBatchCardBLL.GetBatchList(Convert.ToDateTime(objbatchDTO.BatchCarddate).ToLongTimeString(), objbatchDTO.SerialNumber, objbatchDTO.BatchNumber, String.Empty, objbatchDTO.Size, String.Empty, false, objbatchDTO.GloveType, objbatchDTO.TierSide, Constants.HOURLYBATCHCARD, true);
                printdtolst.Add(printdto);
                _sbSerialNumbers.Append(objbatchDTO.SerialNumber);
                _sbSerialNumbers.Append(Constants.COMMA);
            }

            log.Info("## START: Reprint Selection ##");
            PrintBatchCard(printdtolst);
            log.Info("## END: Reprint Selection ##");
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            HourlyBatchCardBLL.SaveReprintBatchCard(_sbSerialNumbers.ToString(), ServerCurrentDateTime, OperatorId, Convert.ToInt32(cmbBoxReasonRePrint.SelectedValue), System.Windows.Forms.SystemInformation.ComputerName, "REPRINT");
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            this.Close();
        }

        #endregion

    }
}
