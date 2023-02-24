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
    public partial class ReprintHBC : FormBase
    {
        #region  Variable
        private static string _screenName = "Batch Card Manual Print";
        private static string _className = "ManualPrint";
        public string OperatorId { get; set; }
        #endregion

        #region constructors

        public ReprintHBC()
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

        public ReprintHBC(string serialNumber, string resource)
        {
            InitializeComponent();
            LoadFormData();
            BindData(serialNumber, resource);
            dtpDate.Enabled = false;
            cmbHour.Enabled = false;
            cmbLine.Enabled = false;
            cmbTier.Enabled = false;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ManualPrint", null);
            }
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
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.LINESELECTION);

            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtSN, "Serial Number", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxReasonRePrint, "Reason for RePrint", ValidationType.Required));

            if ((txtSN.Enabled && txtSN.Text.Trim().Length == Constants.ZERO) && cmbBoxReasonRePrint.SelectedIndex == Constants.ZERO)
            {
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                return;
            }
            else if (!ValidateForm())
            {
                return;
            }

            List<BatchOrderDetailsDTO> lstBatchDTO = new List<BatchOrderDetailsDTO>();
            try
            {
                lstBatchDTO = HourlyBatchCardBLL.SaveReprintBatchCard(txtSN.Text, ServerCurrentDateTime, OperatorId, Convert.ToInt32(cmbBoxReasonRePrint.SelectedValue), System.Windows.Forms.SystemInformation.ComputerName);
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
        
        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.C)
            {
                this.Close();
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(cmbHour.Text)))
                GetTier();
        }

        private void cmbHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTier();
        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(cmbHour.Text)))
                GetTier();
        }

        private void cmbTier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(cmbTier.Text)))
                GetHBCSerialNo();
            else
                txtSN.Text = null;
        }

        #endregion

        #region User Methods
        private void GetHour()
        {
            this.cmbHour.SelectedIndexChanged -= new System.EventHandler(this.cmbHour_SelectedIndexChanged);
            for (int i = 0; i < 24; i++)
            {
                cmbHour.Items.Add(i.ToString("00") + "00");
            }
            this.cmbHour.SelectedIndexChanged += new System.EventHandler(this.cmbHour_SelectedIndexChanged);
        }
        
        private void GetLineNo()
        {
            List<DropdownDTO> LineDTO = CommonBLL.GetAllLinesByLocation(WorkStationDTO.GetInstance().Location);
            cmbLine.DataSource = LineDTO;
            cmbLine.ValueMember = "DisplayField";
            cmbLine.DisplayMember = "DisplayField";
        }

        private void GetTier()
        {
            this.cmbTier.SelectedIndexChanged -= new System.EventHandler(this.cmbTier_SelectedIndexChanged);
            cmbTier.ValueMember = null;
            cmbTier.DataSource = null;
            cmbTier.Text = null;
            cmbTier.Items.Clear();
            txtSN.Text = null;
            DateTime timeOnly = DateTime.ParseExact(cmbHour.Text, "HH00", null, System.Globalization.DateTimeStyles.None);
            DateTime outputTime = dtpDate.Value.Date.Add(timeOnly.TimeOfDay);
            string resourceGrp = WorkStationDTO.GetInstance().Location.ToString() + cmbLine.SelectedValue;
            List<DropdownDTO> TierDTO = CommonBLL.GetTierForRePrint(outputTime, resourceGrp);
            cmbTier.DataSource = TierDTO;
            cmbTier.ValueMember = "IDField";
            cmbTier.DisplayMember = "DisplayField";
            this.cmbTier.SelectedIndexChanged += new System.EventHandler(this.cmbTier_SelectedIndexChanged);
        }

        /// <summary>
        /// Get Serial Number for reprint
        /// </summary>
        private void GetHBCSerialNo()
        {
            DateTime timeOnly = DateTime.ParseExact(cmbHour.Text, "HH00", null, System.Globalization.DateTimeStyles.None);
            DateTime outputTime = dtpDate.Value.Date.Add(timeOnly.TimeOfDay);
            string resource = WorkStationDTO.GetInstance().Location.ToString() + cmbLine.SelectedValue + cmbTier.SelectedValue;
            List<BatchOrderDetailsDTO> SNDTO = HourlyBatchCardBLL.GetRePrintHBCSerialNo(outputTime, resource);
            txtSN.Text = SNDTO[1].SerialNumber;
        }

        /// <summary>
        /// Get Reason for reprint
        /// </summary>
        private void GetReasonForReprint()
        {
            try
            {
                cmbBoxReasonRePrint.DataSource = HourlyBatchCardBLL.GetReasonForReprint();
                cmbBoxReasonRePrint.ValueMember = "IDField";
                cmbBoxReasonRePrint.DisplayMember = "DisplayField";
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getReasonForReprint", null);
                return;
            }
        }

        /// <summary>
        /// Load Form data
        /// </summary>
        private void LoadFormData()
        {
            GetHour();
            GetLineNo();
            GetReasonForReprint();
        }

        private void BindData(string serialNumber, string resource)
        {
            List<BatchOrderDetailsDTO> BODetailsForPrint = HourlyBatchCardBLL.GetReprintDetails(serialNumber, resource);
            dtpDate.Value = BODetailsForPrint[0].OutputTime;
            cmbHour.Text = BODetailsForPrint[0].OutputTime.ToString("HH00");
            cmbLine.Text = BODetailsForPrint[0].LineId;
            cmbTier.Text = BODetailsForPrint[0].TierSide;
            txtSN.Text = BODetailsForPrint[0].SerialNumber;
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

        private void PrintBatchAsync(List<BatchOrderDetailsDTO> lstBatchDTO)
        {
            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;
            List<PrintDTO> printdtolst = new List<PrintDTO>();
            foreach (BatchOrderDetailsDTO objbatchDTO in lstBatchDTO)
            {
                PrintDTO printdto = HourlyBatchCardBLL.GetBatchList(objbatchDTO.OutputTime.ToString("HH:mm:ss"), objbatchDTO.SerialNumber, objbatchDTO.BatchNumber, objbatchDTO.Resource, objbatchDTO.Size, objbatchDTO.GloveType, objbatchDTO.PackingSize, objbatchDTO.InnerBox, objbatchDTO.TotalGloveQty, Constants.HOURLYBATCHCARD, true);
                printdtolst.Add(printdto);
            }
            PrintBatchCard(printdtolst);
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            this.Close();
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
        #endregion
    }
}
