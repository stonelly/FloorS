using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    public partial class WIPVoidScannedTxn : FormBase
    {
        #region Variable

        private DateTime selectedDate;
        private List<WIPTransactionDTO> scannedDataList;

        #endregion

        #region Constructor

        public WIPVoidScannedTxn()
        {
            InitializeComponent();
            FormLoad();
        }

        #endregion

        #region Private Method

        private void FormLoad()
        {
            this.WindowState = FormWindowState.Maximized;

            scannedDataList = new List<WIPTransactionDTO>();
            dgvWIPScannedData.AutoGenerateColumns = false;

            InitializeDateTimePicker();

            btnVoid.Enabled = false;
        }

        private void ClearForm()
        {
            InitializeDateTimePicker();
            dgvWIPScannedData.DataSource = null;
            btnVoid.Enabled = false;
        }

        private void InitializeDateTimePicker()
        {
            selectedDate = DateTime.Now.Date;
            dtpDate.Value = selectedDate;
            dtpDate.MaxDate = selectedDate;
            dtpDate.Refresh();
        }

        private void GetScannedData()
        {
            try
            {
                dgvWIPScannedData.DataSource = null;

                scannedDataList.Clear();
                scannedDataList = new List<WIPTransactionDTO>();
                scannedDataList.AddRange(WIPStockCountBLL.GetWIPScannedData(selectedDate));

                if (scannedDataList.Count > 0)
                {
                    var source = new BindingSource(scannedDataList, null);
                    dgvWIPScannedData.DataSource = source;

                    btnVoid.Enabled = true;
                }
                else
                {
                    btnVoid.Enabled = false;
                    GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        #endregion

        #region Button Command

        private void btnSearch_Click(object sender, EventArgs e)
        {
            selectedDate = dtpDate.Value.Date;

            GetScannedData();
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                List<WIPTransactionDTO> voidWIPDataList = new List<WIPTransactionDTO>();
                voidWIPDataList.AddRange(scannedDataList.Where(p => p.IsCheck == true).ToList());

                if (voidWIPDataList.Count > 0)
                {
                    if (GlobalMessageBox.Show(string.Format(Messages.VOID_WIP_SCAN_DATA_CONFIRMATION, Environment.NewLine), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            int error = WIPStockCountBLL.UpdateVoidWIPTxn(String.Join(",", voidWIPDataList.Select(p => p.ReferenceNumber).ToList()));

                            if (error == 0)
                            {
                                scope.Complete();
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                            else if (error == -2)
                            {
                                GlobalMessageBox.Show(string.Format(Messages.CONCURRENCY_ERROR, Environment.NewLine), Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }

                        GetScannedData();
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.NO_DATA_CHECKED_TO_VOID, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException ex)
            {
                GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        #endregion
    }
}
