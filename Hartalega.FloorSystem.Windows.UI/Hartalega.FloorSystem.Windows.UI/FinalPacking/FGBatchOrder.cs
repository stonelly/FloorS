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
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

#endregion
namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public delegate void BindGrid();
    public partial class FGBatchOrder : FormBase
    {
        #region Private Varibale

        private string _operaterId = string.Empty;
        private static string _screenName = "FG Batch Order";
        private static string _className = "FGBatchOrder";
        private static string batchOrderNo = string.Empty;
        private BindingSource dataGridBindingSource;
        private List<GloveBatchOrderDTO> dtBatchOrderDetails_cached;
        private List<GloveBatchOrderDTO> _gridtoExport;
        #endregion

        #region Load Form

        public FGBatchOrder()
        {
            InitializeComponent();
            UIRefresh();
            dataGridBindingSource = new BindingSource();
            try
            {
                BatchPrint = true;
                WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);
                WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.LINESELECTION);
                dgvLineSelection.AutoGenerateColumns = false;
                dgvLineDetails.AutoGenerateColumns = false;
                tsbApprove.Enabled = false;
                tsbReopen.Enabled = false;

                List<DropdownDTO> lstPlantNo = null;
                lstPlantNo = FinalPackingBLL.GetLocationDetails();

                if (lstPlantNo != null)
                {
                    foreach (DropdownDTO plantNo in lstPlantNo)
                    {
                        tsPlant.Items.Add(plantNo.DisplayField);
                        tsPlant.AutoCompleteCustomSource.Add(plantNo.DisplayField);
                        tsPlant.SelectedItem = WorkStationDTO.GetInstance().Location.ToString();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "frmLineSelection_Load", null);
                return;
            }
            catch (StackOverflowException soex)
            {
                FloorSystemException fex = new FloorSystemException(soex.Message, soex.Message, soex, true);
                ExceptionLogging(fex, _screenName, _className, "frmLineSelection_Load", null);
                return;
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "frmLineSelection_Load", null);
                return;
            }
            getLineSelection();
            if (dgvLineSelection.Rows.Count > 0)
            {
                batchOrderNo = dgvLineSelection.Rows[0].Cells[3].Value.ToString();
                getLineSelectionDetails(batchOrderNo);
            }
        }
        #endregion

        #region Event Handlers

        private void dgvLineSelection_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            batchOrderNo = dgvLineSelection.Rows[e.RowIndex].Cells[3].Value.ToString();
            getLineSelectionDetails(batchOrderNo);
        }

        private void tsPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            getLineSelection();
        }

        /// <summary>
        /// Form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmLineSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            BatchPrint = false;
            e.Cancel = false;
        }

        #endregion

        #region User Methods
        /// <summary>
        /// #AZ 26/02/2018 1.n FDD@NGC_CR_090
        /// </summary>
        private void getLineSelection(List<GloveBatchOrderDTO> dtBatchOrderDetails = null)
        {
            try
            {
                if (dtBatchOrderDetails == null)
                {
                    dtBatchOrderDetails = HourlyBatchCardBLL.GetBatchOrderDetails("FG", tsPlant.Text);
                    dtBatchOrderDetails_cached = dtBatchOrderDetails; // cache orignal list when get from database
                    _gridtoExport = dtBatchOrderDetails;
                }
                dgvLineSelection.AutoGenerateColumns = false;
                var source = new SortableBindingList<GloveBatchOrderDTO>(dtBatchOrderDetails);
                dataGridBindingSource.DataSource = source;
                dgvLineSelection.DataSource = dataGridBindingSource;
                dgvLineSelection.AutoResizeColumns();
                dgvLineSelection.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getLineSelection", null);
                return;
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "getLineSelection", null);
                return;
            }
        }

        private void getLineSelectionDetails(string batchOrderNo)
        {
            try
            {
                tsbApprove.Enabled = false;
                tsbReopen.Enabled = false;

                List<MadeToStockDTO> dtBatchOrderPrintDetails;
                dtBatchOrderPrintDetails = FinalPackingBLL.GetBOMTSDetails(batchOrderNo);
                dgvLineDetails.DataSource = dtBatchOrderPrintDetails;
                dgvLineDetails.AutoResizeColumns();
                dgvLineDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                if (dtBatchOrderPrintDetails[0].WarehouseId == "MTS-FG")
                {
                    if (dtBatchOrderPrintDetails[0].MadeToStockStatus == "Open")
                        tsbApprove.Enabled = true;
                    else
                        tsbReopen.Enabled = true;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getLineSelectionDetails", null);
                return;
            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "getLineSelectionDetails", null);
                return;
            }
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

        private void UIRefresh()
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "FGBatchOrder", null);
            }
        }
        #endregion

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            getLineSelection();
        }

        private void tsbApprove_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.BATCHORDER, _screenName);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication))
            {
                FinalPackingBLL.UpdateBatcOrderMTSStatus(batchOrderNo, 1, Convert.ToInt32(_passwordForm.Authentication));
                getLineSelection();
            }
        }

        private void tsbReopen_Click(object sender, EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.BATCHORDER, _screenName);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication))
            {
                FinalPackingBLL.UpdateBatcOrderMTSStatus(batchOrderNo, 0, Convert.ToInt32(_passwordForm.Authentication));
                getLineSelection();
            }
        }

        /// <summary>
        /// The Generic Search method used for search elements in Purticular Entity set.
        /// You can use this functionality in User Interface for search any elements which are
        /// display in grid. 
        /// Ex : 
        /// Search by,
        ///     Id, Name, Description etc.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected List<GloveBatchOrderDTO> Search(string query)
        {
            var _dataList = dtBatchOrderDetails_cached;//restore back oringal database list 
            if (string.IsNullOrEmpty(query))
            {
                //direct return full list if query keyword is empty
                return _dataList;
            }
            var list = new List<GloveBatchOrderDTO>();
            string[] qInitItems = query.Split(',');
            List<string> qItems = new List<string>();
            bool isAppendEnd = false;
            StringBuilder queryKeyword = null;
            for (int i = 0; i < qInitItems.Length; i++)
            {
                if (qInitItems[i].StartsWith("\""))//open stringBuilder to insert
                {
                    isAppendEnd = true;
                    queryKeyword = new StringBuilder();
                }
                if (qInitItems[i].EndsWith("\""))//close stringBuilder
                {
                    isAppendEnd = false;
                    if (queryKeyword != null)
                    {
                        queryKeyword.Append(" " + qInitItems[i]);
                        qItems.Add(queryKeyword.ToString().Replace('\"', ' ').Trim());
                    }
                }
                if (isAppendEnd)
                    queryKeyword.Append(" " + qInitItems[i]);
            }
            if (!string.IsNullOrEmpty(query) && qItems.Count == 0)
                qItems = qInitItems.ToList<string>();
            foreach (var item in _dataList)
            {
                for (int i = 0; i < qItems.Count; i++)
                    if (FoundIt(item,qItems[i]))
                    {
                        list.Add(item);
                        break;
                    }
            }
            return list;
        }

        public virtual bool FoundIt(GloveBatchOrderDTO item, string query)
        {
            foreach (PropertyInfo pi in item.GetType().GetProperties())
            {
                object[] attributes = pi.GetCustomAttributes(typeof(BrowsableAttribute), false);
                if (attributes.Length == 1)
                {
                    BrowsableAttribute a = (BrowsableAttribute)attributes[0];
                    if (!a.Browsable)
                        continue;
                }
                object x = pi.GetValue(item, null);
                if (x == null) continue;
                string s = x.ToString().ToLower();
                string d = query.Trim().ToLower();
                if (s.IndexOf(d) != -1) return true;
            }
            return false;
        }

        private void textBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string s = this.textBoxSearch.Text.Trim();
                var dataList = this.Search(s);
                getLineSelection(dataList);
                _gridtoExport = dataList;
            }
        }

        private void imgExportToExcel_Click(object sender, EventArgs e)
        {
            List<string> hearColumns = new List<string>();
            hearColumns.Add("PL Start Date");
            hearColumns.Add("PL Start Time");
            hearColumns.Add("Prod Status");
            hearColumns.Add("Batch Order #");
            hearColumns.Add("Sales Order & Internal Ref #");
            hearColumns.Add("Item Number");
            hearColumns.Add("Size");
            hearColumns.Add("Order Qty");
            hearColumns.Add("Reported Qty");
            hearColumns.Add("Remaining Qty");
            hearColumns.Add("Resource Group");
            hearColumns.Add("Resource");
            hearColumns.Add("PL End Date");
            hearColumns.Add("PL End Time");
            try
            {
                if (_gridtoExport.Count > Constants.ZERO)
                {
                    CommonBLL.ExportToExcel<GloveBatchOrderDTO>(hearColumns, "FGBatchOrder", _gridtoExport);
                }
                else
                {
                    GlobalMessageBox.Show(Messages.EXPORTTOEXCEL_EXCEPTION_NO_RECORDS_EXPORT, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Warning);

                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "imgExportToExcel_Click", null);
                return;
            }
        }
    }
}
