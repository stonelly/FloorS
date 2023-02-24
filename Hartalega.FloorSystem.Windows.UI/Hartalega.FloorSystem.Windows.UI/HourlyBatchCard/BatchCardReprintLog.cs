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
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public partial class BatchCardReprintLog : FormBase
    {
        #region Private Varibale
        private List<RePrintBatchCardDTO> _gridtoExport { get; set; }
        private static string _screenName = "Batch Card Reprint Log";
        private static string _className = "BatchCardReprintLog";
        private static string _screenNameForAuthorization = "Batch Card Manual Print";       
        #endregion

        #region Load Form

        public BatchCardReprintLog()
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
            _gridtoExport = new List<RePrintBatchCardDTO>();
            //menuStrip2.Items[3].Enabled = false;
            BindPlantList();
            dpfromdate.NofutureDateSelection();
            dptodate.NofutureDateSelection();
        }

        private void frmBatchCardReprintLog_Load(object sender, EventArgs e)
        {


        }

        private void BindPlantList()
        {
            try
            {
                cmbPlant.BindComboBox(HourlyBatchCardBLL.GetPlantList(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPlantList", null);
                return;
            }
        }

        #endregion

        #region User Methods

        private void getBatchCardReprintLog()
        {
            if (!UserValidation())
            {
                return;
            }
            _gridtoExport = null;

            try
            {
                List<RePrintBatchCardDTO> lstRePrintBatchCardDTO = new List<RePrintBatchCardDTO>();
                lstRePrintBatchCardDTO = HourlyBatchCardBLL.GetBatchCardReprintLog(dpfromdate.Value, dptodate.Value, Convert.ToString(cmbPlant.SelectedValue));
                if (lstRePrintBatchCardDTO.Count != Constants.ZERO)
                {
                    dgvBatchCardReprintLog.AutoSize = false;
                    dgvBatchCardReprintLog.DataSource = lstRePrintBatchCardDTO;
                }

                _gridtoExport = lstRePrintBatchCardDTO;
                if (lstRePrintBatchCardDTO.Count == Constants.ZERO)
                {
                    dgvBatchCardReprintLog.AutoSize = false;
                    dgvBatchCardReprintLog.DataSource = new List<RePrintBatchCardDTO>();
                    GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    dpfromdate.Focus();
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBatchCardReprintLog", null);
                return;
            }
        }

        /// <summary>
        /// UserValidation
        /// </summary>
        private bool UserValidation()
        {
            bool isvalid = false;
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbPlant, "Plant", ValidationType.Required));
            if (Convert.ToDateTime(dpfromdate.Value.ToShortDateString()) > Convert.ToDateTime(dptodate.Value.ToShortDateString()))
            {
                isvalid = false;
                GlobalMessageBox.Show(Messages.TO_DATE_LESSTHAN_FROM_DATE, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            }
            else
            {
                isvalid = ValidateForm();
            }
            return isvalid;

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


        #endregion

        #region Event Handlers

        private void imgExportToExcel_Click(object sender, EventArgs e)
        {
            List<string> hearColumns = new List<string>();
            hearColumns.Add("Batch No.");
            hearColumns.Add("Serial No");
            hearColumns.Add("Batch Date Time");
            hearColumns.Add("Plant");
            hearColumns.Add("Process Area");
            hearColumns.Add("Type");
            hearColumns.Add("Manual Print Date Time");
            hearColumns.Add("Reprint By");
            hearColumns.Add("Reprint Reason");
            try
            {
                if (_gridtoExport.Count > Constants.ZERO)
                {
                    CommonBLL.ExportToExcel<RePrintBatchCardDTO>(hearColumns, "BatchCardReprintLog", _gridtoExport);
                }
                else
                {
                    GlobalMessageBox.Show(Messages.EXPORTTOEXCEL_EXCEPTION_NO_RECORDS_EXPORT, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                    dpfromdate.Focus();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "imgExportToExcel_Click", null);
                return;
            }
        }

        /// <summary>
        /// imgCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            getBatchCardReprintLog();
        }

        /// <summary>
        /// btnGo_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            getBatchCardReprintLog();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string itemText = e.ClickedItem.Text;
            switch (itemText)
            {
                case Constants.LINESELECTION:
                    this.Close();
                    break;
                case Constants.MANUAL_PRINT_BATCH_CARD:
                    Login _passwordForm = new Login(Constants.Modules.HOURLYBATCHCARD, _screenNameForAuthorization);
                    _passwordForm.ShowDialog();
                    if (!string.IsNullOrEmpty(_passwordForm.Authentication))
                    {
                        ReprintHBC _frmManualPrint = new ReprintHBC();
                        _frmManualPrint.OperatorId = _passwordForm.Authentication;
                        _frmManualPrint.Show();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void toolStripBO_Click(object sender, EventArgs e)
        {
            new GloveBatchOrder().ShowDialog();
            this.Close();
        }
    }
}
