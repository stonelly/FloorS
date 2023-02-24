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
    public partial class ManualHourlyBatchCard : FormBase
    {
        #region  Varibale
        private static string _screenName = "Manual Print Batch Card";
        private static string _className = "ManualPrint";
        private LineSelectionReprint _PrintLineSelection;
        private StringBuilder _sbSerialNumbers;
        public string OperatorId { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region constructors
        public ManualHourlyBatchCard()
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
            validationMesssageLst.Add(new ValidationMessage(cmbBoxLine, "Line", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxHour, "Hour", ValidationType.Required));

            StringBuilder tiesrside = new StringBuilder("");
            _sbSerialNumbers = new StringBuilder("");

            bool isvalid = true;
            isvalid = cmbBoxLine.SelectedIndex == Constants.ZERO ? false : true;
            isvalid = cmbBoxReason.SelectedIndex == Constants.ZERO ? false : true;

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
                if (!chkRB.Checked)
                {
                    chk = chkRB;
                }
                if (!chkRT.Checked)
                {
                    chk = chkRT;
                }
                if (!chkLB.Checked)
                {
                    chk = chkLB;
                }
                if (!chkLT.Checked)
                {
                    chk = chkLT;
                }
                if (chk != null)
                {
                    validationMesssageLst.Add(new ValidationMessage(chk, "Select at least one Tier side", ValidationType.Custom));
                }
            }

            validationMesssageLst.Add(new ValidationMessage(cmbBoxReason, "Reason", ValidationType.Required));

            if (!ValidateForm())
            {
                return;
            }

            List<BatchDTO> lstBatchDTO = new List<BatchDTO>();

            try
            {
                // No need generate new batch card number if already exist
                string SelectedTies = tiesrside.ToString().Length > Constants.ZERO ? tiesrside.ToString().Substring(Constants.ZERO, tiesrside.ToString().Length - 1) : string.Empty;

                if (HourlyBatchCardBLL.CheckDuplicateBatchCard(Convert.ToString(cmbBoxLine.SelectedValue), SelectedTies, Convert.ToInt32(cmbBoxHour.SelectedValue), ServerCurrentDateTime))
                {
                    GlobalMessageBox.Show(Messages.DUPLICATE_BATCH, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }

                lstBatchDTO = HourlyBatchCardBLL.usp_ManualPrintBatchCard_Get(Convert.ToString(cmbBoxLine.SelectedValue), Convert.ToInt32(cmbBoxHour.SelectedValue), SelectedTies, ServerCurrentDateTime, WorkStationDTO.GetInstance().Module, WorkStationDTO.GetInstance().SubModule, Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId));

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
            try
            {
                if (string.IsNullOrEmpty(cmbBoxLine.Text))
                {
                    GlobalMessageBox.Show(Messages.PRODUCTIONLINE_NOT_SELECTED, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }

                _PrintLineSelection = HourlyBatchCardBLL.GetTierSideGloveSize(Convert.ToString(cmbBoxLine.SelectedValue));
                BindTierDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "cmbBoxHour_SelectedIndexChanged", null);
                return;
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
                cmbBoxLine.DataSource = HourlyBatchCardBLL.GetLine(System.Windows.Forms.SystemInformation.ComputerName);
                cmbBoxLine.ValueMember = "IDField";
                cmbBoxLine.DisplayMember = "DisplayField";

                if (_PrintLineSelection != null)
                {
                    cmbBoxLine.Text = _PrintLineSelection.LineId;
                }
                else
                {
                    cmbBoxLine.SelectedIndex = Constants.MINUSONE;
                }
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

        private void GetReason()
        {
            try
            {
                cmbBoxReason.DataSource = HourlyBatchCardBLL.GetReasonForManualPrint();
                cmbBoxReason.ValueMember = "IDField";
                cmbBoxReason.DisplayMember = "DisplayField";
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getReasonForManualPrint", null);
                return;
            }
        }

        /// <summary>
        /// Load Form data
        /// </summary>
        private void LoadFormData()
        {
            GetLine();
            GetLastReprinthrs();
            GetReason();
            BindTierDetails();
        }

        /// <summary>
        /// Bind line details
        /// </summary>
        private void BindTierDetails()
        {
            CleaseSelection();

            if (_PrintLineSelection != null)
            {
                if (!_PrintLineSelection.IsDoubleFormer)
                {
                    chkLT.Enabled = true;
                    chkRT.Enabled = true;
                    txtLTSize.Text = _PrintLineSelection.LTSize;
                    txtRTSize.Text = _PrintLineSelection.RTSize;
                }
                else
                {
                    txtLTSize.Text = _PrintLineSelection.LTSize;
                    txtRTSize.Text = _PrintLineSelection.RTSize;
                    txtLBSize.Text = _PrintLineSelection.LBSize;
                    txtRBSize.Text = _PrintLineSelection.RBSize;
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
                PrintDTO printdto = HourlyBatchCardBLL.GetBatchList(Convert.ToDateTime(objbatchDTO.BatchCarddate).ToLongTimeString(), objbatchDTO.SerialNumber, objbatchDTO.BatchNumber, String.Empty, objbatchDTO.Size, String.Empty, false, objbatchDTO.GloveType, objbatchDTO.TierSide, Constants.HOURLYBATCHCARD, false, true);
                printdtolst.Add(printdto);
                _sbSerialNumbers.Append(objbatchDTO.SerialNumber);
                _sbSerialNumbers.Append(Constants.COMMA);
            }

            log.Info("## START: Manual Print Selection ##");
            PrintBatchCard(printdtolst);
            log.Info("## END: Manual Print Selection ##");
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            HourlyBatchCardBLL.SaveReprintBatchCard(_sbSerialNumbers.ToString(), ServerCurrentDateTime, OperatorId, Convert.ToInt32(cmbBoxReason.SelectedValue), System.Windows.Forms.SystemInformation.ComputerName, "MANUAL");
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            this.Close();
        }
        #endregion
    }
}
