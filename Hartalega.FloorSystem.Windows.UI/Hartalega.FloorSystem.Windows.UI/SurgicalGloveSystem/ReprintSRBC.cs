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

namespace Hartalega.FloorSystem.Windows.UI.SurgicalGloveSystem
{
    public partial class ReprintSRBC : FormBase
    {
        #region  Variable
        private static string _screenName = "ReprintSRBC";
        private static string _className = "ReprintSRBC";
        public string OperatorId { get; set; }
        #endregion

        #region constructors

        public ReprintSRBC()
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

        public ReprintSRBC(string batchCardDate, string resource, string bo, string serialNumber)
        {
            InitializeComponent();
            BindData(batchCardDate, resource, bo, serialNumber);
            dtpDate.Enabled = false;
            cmbLine.Enabled = false;
            cmbResource.Enabled = false;
            cmbBO.Enabled = false;
            cmbSN.Enabled = false;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ReprintSRBC", null);
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
            validationMesssageLst.Add(new ValidationMessage(cmbSN, "Serial Number", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxReasonRePrint, "Reason for RePrint", ValidationType.Required));

            if ((cmbSN.SelectedText.Trim().Length == Constants.ZERO) && cmbBoxReasonRePrint.SelectedIndex == Constants.ZERO)
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
                lstBatchDTO = SurgicalGloveBLL.SaveReprintBatchCard(cmbSN.Text, ServerCurrentDateTime, OperatorId, Convert.ToInt32(cmbBoxReasonRePrint.SelectedValue), System.Windows.Forms.SystemInformation.ComputerName);
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
            GetLine();
        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(cmbLine.Text)))
                GetResource();
            else
            {
                cmbResource.ValueMember = null;
                cmbResource.DataSource = null;
                cmbResource.Items.Clear();
                cmbResource.Text = null;
                cmbBO.ValueMember = null;
                cmbBO.DataSource = null;
                cmbBO.Text = null;
                cmbBO.Items.Clear();
                cmbSN.ValueMember = null;
                cmbSN.DataSource = null;
                cmbSN.Items.Clear();
                cmbSN.Text = null;
            }
        }

        private void cmbResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(cmbResource.Text)))
                GetBO();
            else
            {
                cmbBO.DataSource = null;
                cmbBO.Text = null;
                cmbBO.Items.Clear();
                cmbSN.ValueMember = null;
                cmbSN.DataSource = null;
                cmbSN.Items.Clear();
                cmbSN.Text = null;
            }
        }

        private void cmbBO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(cmbBO.Text)))
                GetSN();
            else
            {
                cmbSN.ValueMember = null;
                cmbSN.DataSource = null;
                cmbSN.Items.Clear();
                cmbSN.Text = null;
            }
        }

        #endregion

        #region User Methods
        
        private void GetLine()
        {
            this.cmbLine.SelectedIndexChanged -= new System.EventHandler(this.cmbLine_SelectedIndexChanged);
            cmbResource.ValueMember = null;
            cmbResource.DataSource = null;
            cmbResource.Items.Clear();
            cmbResource.Text = null;
            cmbBO.ValueMember = null;
            cmbBO.DataSource = null;
            cmbBO.Text = null;
            cmbBO.Items.Clear();
            cmbSN.ValueMember = null;
            cmbSN.DataSource = null;
            cmbSN.Items.Clear();
            cmbSN.Text = null;
            List<DropdownDTO> DropdownDTO = SurgicalGloveBLL.GetRePrintSRBCResource(dtpDate.Value, Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId),"", "", "");
            cmbLine.DataSource = DropdownDTO;
            cmbLine.ValueMember = "IDField";
            cmbLine.DisplayMember = "DisplayField";
            this.cmbLine.SelectedIndexChanged += new System.EventHandler(this.cmbLine_SelectedIndexChanged);
        }

        private void GetResource()
        {
            this.cmbResource.SelectedIndexChanged -= new System.EventHandler(this.cmbResource_SelectedIndexChanged);
            cmbBO.ValueMember = null;
            cmbBO.DataSource = null;
            cmbBO.Text = null;
            cmbBO.Items.Clear();
            cmbSN.ValueMember = null;
            cmbSN.DataSource = null;
            cmbSN.Items.Clear();
            cmbSN.Text = null;
            List<DropdownDTO> DropdownDTO = SurgicalGloveBLL.GetRePrintSRBCResource(dtpDate.Value, Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId), cmbLine.Text, "", "");
            cmbResource.DataSource = DropdownDTO;
            cmbResource.ValueMember = "IDField";
            cmbResource.DisplayMember = "DisplayField";
            this.cmbResource.SelectedIndexChanged += new System.EventHandler(this.cmbResource_SelectedIndexChanged);
        }

        private void GetBO()
        {
            this.cmbBO.SelectedIndexChanged -= new System.EventHandler(this.cmbBO_SelectedIndexChanged);
            cmbSN.ValueMember = null;
            cmbSN.DataSource = null;
            cmbSN.Items.Clear();
            cmbSN.Text = null;
            List<DropdownDTO> DropdownDTO = SurgicalGloveBLL.GetRePrintSRBCResource(dtpDate.Value, Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId), cmbLine.Text, cmbResource.Text, "");
            cmbBO.DataSource = DropdownDTO;
            cmbBO.ValueMember = "IDField";
            cmbBO.DisplayMember = "DisplayField";
            this.cmbBO.SelectedIndexChanged += new System.EventHandler(this.cmbBO_SelectedIndexChanged);
        }

        private void GetSN()
        {
            List<DropdownDTO> DropdownDTO = SurgicalGloveBLL.GetRePrintSRBCResource(dtpDate.Value, Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId), cmbLine.Text, cmbResource.Text, cmbBO.Text);
            cmbSN.DataSource = DropdownDTO;
            cmbSN.ValueMember = "IDField";
            cmbSN.DisplayMember = "DisplayField";
        }

        /// <summary>
        /// Get Reason for reprint
        /// </summary>
        private void GetReasonForReprint()
        {
            try
            {
                cmbBoxReasonRePrint.DataSource = SurgicalGloveBLL.GetReasonForReprint();
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
            GetLine();
            GetReasonForReprint();
        }

        private void BindData(string batchCardDate, string resource, string bo, string serialNumber)
        {
            dtpDate.Value = Convert.ToDateTime(batchCardDate);
            GetLine();
            cmbLine.SelectedIndex = cmbLine.FindString(resource.Substring(2,2));
            cmbResource.Text = resource;
            cmbBO.Text = bo;
            cmbSN.Text = serialNumber;
            GetReasonForReprint();
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
                PrintDTO printdto = SurgicalGloveBLL.GetBatchList(objbatchDTO.BatchCarddate.ToString(), objbatchDTO.BatchNumber, objbatchDTO.BatchWeight.ToString(), objbatchDTO.Quantity.ToString(), objbatchDTO.SerialNumber, objbatchDTO.Size, objbatchDTO.GloveType, objbatchDTO.GloveCategory, objbatchDTO.Resource, Constants.SURGICALGLOVESYSTEM, true);
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
