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
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    public delegate void ReprintAsync(BatchDTO objBatchDTO);
    public partial class ReprintBatchCard : FormBase
    {
        #region  Variable
        private static string _screenName = "Batch Card Manual Print";
        private static string _className = "ManualPrint";
        public string OperatorId { get; set; }
        #endregion

        #region constructors

        public ReprintBatchCard()
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

        #region Event Handlers

        /// <summary>
        /// OK Button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.TUMBLING_SYSTEM);
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.LINESELECTION);

            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtSN, "Serial Number", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxReasonRePrint, "Reason for RePrint", ValidationType.Required));

            int plantID = 0;

            if ((txtSN.Enabled && txtSN.Text.Trim().Length == Constants.ZERO) && cmbBoxReasonRePrint.SelectedIndex == Constants.ZERO)
            {
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                return;
            }
            else if (!TumblingBLL.CheckReprintLocation(txtSN.Text, WorkStationDTO.GetInstance().WorkStationId, out plantID))
            {
                string outString = string.Format("Please reprint the batch card at correct plant location (Plant {0}).", plantID);

                GlobalMessageBox.Show(outString, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                return;
            }
            else if (!ValidateForm())
            {
                return;
            }

            BatchDTO objBatchDTO = new BatchDTO();
            try
            {
                objBatchDTO = CommonBLL.GetCompleteBatchDetails(Convert.ToDecimal(txtSN.Text));
                if (objBatchDTO != null)
                {
                    if (objBatchDTO.IsOnline == false)
                    {
                        objBatchDTO = TumblingBLL.SaveReprintBatchCard(txtSN.Text, ServerCurrentDateTime, OperatorId, Convert.ToInt32(cmbBoxReasonRePrint.SelectedValue), System.Windows.Forms.SystemInformation.ComputerName);
                        if (!PrinterClass.IsPrinterExists)
                        {
                            GlobalMessageBox.Show(Messages.NO_PRINTER, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        }
                        else
                        {
                            this.BeginInvoke(new ReprintAsync(PrintBatchAsync), objBatchDTO);
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.TUMBLING_REPRINT_HBC, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
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

        private void txtSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #endregion

        #region User Methods

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
            GetReasonForReprint();
        }

        private void BindData(string serialNumber, string resource)
        {
            List<BatchOrderDetailsDTO> BODetailsForPrint = HourlyBatchCardBLL.GetReprintDetails(serialNumber, resource);
            dtpDate.Value = BODetailsForPrint[0].OutputTime;
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

        private void PrintBatchAsync(BatchDTO objBatchDTO)
        {
            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;
            string _GloveCategory;
            _GloveCategory = TumblingBLL.GetRejectGloveCategory(objBatchDTO.GloveType);
            objBatchDTO.GloveTypeDescription = objBatchDTO.GloveType + Environment.NewLine + Constants.TAB + _GloveCategory;
            DataSet objData = CommonBLL.GetResourceBySerialNo(Convert.ToDecimal(objBatchDTO.SerialNumber));
            string resource = string.Empty;
            if (objData.Tables.Count > 0)
                if (objData.Tables[0].Rows.Count > 0)
                    resource = objData.Tables[0].Rows[0]["Resource"].ToString();

            CommonBLL.PrintDetails(objBatchDTO.BatchCarddate.ToString("HH:mm:ss"), objBatchDTO.SerialNumber, objBatchDTO.BatchNumber, objBatchDTO.BatchWeight.ToString(), objBatchDTO.Size, objBatchDTO.TenPcsWeight.ToString(), false, objBatchDTO.GloveTypeDescription, String.Empty, Constants.TUMBLING_REPRINT_BATCH_CARD, true, false, resource);
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            this.Close();
        }
        #endregion
    }
}
