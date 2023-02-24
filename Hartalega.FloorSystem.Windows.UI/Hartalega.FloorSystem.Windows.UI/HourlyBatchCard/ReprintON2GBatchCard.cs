#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Drawing.Printing;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public delegate void ReprintAsync(ON2GBatchDTO objBatchDTO);

    public partial class ReprintON2GBatchCard : FormBase
    {
        #region  Variable
        private static string _screenName = "Reprint ON2G Batch Card";
        private static string _className = "ReprintON2GBatchCard";
        public string OperatorId { get; set; }
        private int locationId = WorkStationDTO.GetInstance().LocationId;
        #endregion

        #region constructors

        public ReprintON2GBatchCard()
        {
            InitializeComponent();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "RePrint", null);
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
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);
            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.REPRINT_ON2G_BATCH_CARD);

            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(dtpDate, "Date", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSN, "Serial Number", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbLine, "Line", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBoxReasonRePrint, "Reason for RePrint", ValidationType.Required));

            int plantID = 0;

            //
            if((txtSN.Enabled && txtSN.Text.Trim().Length != Constants.ZERO) && cmbBoxReasonRePrint.SelectedIndex != Constants.ZERO && cmbLine.SelectedIndex != Constants.ZERO && dtpDate.Text.Trim().Length != Constants.ZERO)
            {
                if (!txtSN.Text.Trim().All(char.IsDigit))
                {
                    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_INVALID, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }
                else if (!HourlyBatchCardBLL.CheckON2GBCReprintLocation(txtSN.Text, WorkStationDTO.GetInstance().WorkStationId, out plantID))
                {
                    string outString = string.Format("Please reprint the batch card at correct plant location (Plant {0}).", plantID);

                    GlobalMessageBox.Show(outString, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }
                else if (!ValidateForm())
                {
                    return;
                }
            }
            else
            {
                string outString = string.Format("Please fill up all field");

                GlobalMessageBox.Show(outString, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                return;
            }

            //
            //    if ((txtSN.Enabled && txtSN.Text.Trim().Length == Constants.ZERO) && cmbBoxReasonRePrint.SelectedIndex == Constants.ZERO && cmbLine.SelectedIndex != Constants.ZERO)
            //{
            //    GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            //    return;
            //}
            //else if (!HourlyBatchCardBLL.CheckON2GBCReprintLocation(txtSN.Text, WorkStationDTO.GetInstance().WorkStationId, out plantID))
            //{
            //    string outString = string.Format("Please reprint the batch card at correct plant location (Plant {0}).", plantID);

            //    GlobalMessageBox.Show(outString, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            //    return;
            //}
            //else if (!ValidateForm())
            //{
            //    return;
            //}

            ON2GBatchDTO objBatchDTO = new ON2GBatchDTO();
            try
            {
                DateTime selectedDateTime = dtpDate.Value;
                objBatchDTO = CommonBLL.GetCompleteON2GBatchDetails(Convert.ToDecimal(txtSN.Text), locationId, cmbLine.SelectedValue.ToString(), selectedDateTime);
                if (objBatchDTO != null)
                {
                        objBatchDTO = HourlyBatchCardBLL.SaveReprintON2GBatchCard(txtSN.Text, ServerCurrentDateTime, OperatorId, Convert.ToInt32(cmbBoxReasonRePrint.SelectedValue), cmbLine.SelectedValue.ToString(), System.Windows.Forms.SystemInformation.ComputerName);
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
                    GlobalMessageBox.Show(Messages.NO_ON2G_BATCH_CARDS_AVAILABLE, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
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
                cmbBoxReasonRePrint.DataSource = HourlyBatchCardBLL.GetReasonForReprintON2GBatcgCard();
                cmbBoxReasonRePrint.ValueMember = "IDField";
                cmbBoxReasonRePrint.DisplayMember = "DisplayField";
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "usp_Reasons_Reprint_ON2G_Get", null);
                return;
            }
        }

        /// <summary>
        /// Load Form data
        /// </summary>
        private void LoadFormData()
        {
            GetAllLine();
            GetReasonForReprint();
        }

        /// <summary>
        ///#1.List out all Resources Group (Production lines)
        /// </summary>
        private void GetAllLine()
        {
            cmbLine.DataSource = HourlyBatchCardBLL.GetON2GReprintLineByPlant(locationId);
            cmbLine.ValueMember = "LineId";
            cmbLine.DisplayMember = "LineId";
        }

        //private void BindData(string serialNumber, string resource)
        //{
        //    List<BatchOrderDetailsDTO> BODetailsForPrint = HourlyBatchCardBLL.GetReprintON2GDetails(serialNumber, resource);
        //    dtpDate.Value = BODetailsForPrint[0].OutputTime;
        //    txtSN.Text = BODetailsForPrint[0].SerialNumber;
        //}

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

        private void PrintBatchAsync(ON2GBatchDTO objBatchDTO)
        {

            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;

            HourlyBatchCardBLL.PrintDetailsON2G(objBatchDTO.CurrentDateandTime.ToString(), objBatchDTO.SerialNumber, objBatchDTO.BatchNumber, objBatchDTO.Size,  
                                                objBatchDTO.GloveCode, objBatchDTO.PackingSize.ToString(), objBatchDTO.InnerBox.ToString(), objBatchDTO.TotalGloveQty.ToString(), 
                                                Constants.REPRINT_ON2G_BATCH_CARD,true, true, objBatchDTO.Resource, objBatchDTO.GloveTypeDescription);
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            this.Close();

        }
        #endregion

    }
}
