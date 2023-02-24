#region using
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System.Text;
using System.Reflection;
using System.ComponentModel;

#endregion
namespace Hartalega.FloorSystem.Windows.UI.SurgicalGloveSystem
{
    public delegate void BindGrid();
    public delegate void PrintAsync(List<BatchOrderDetailsDTO> lstBatchDTO);
    public partial class GloveBatchOrder : FormBase
    {
        #region Private Varibale

        private string _operaterId = string.Empty;
        private static string _screenName = "Glove Batch Order";
        private static string _className = "GloveBatchOrder";
        private static string _ManualPrintscreenName = "Reprint SRBC";
        private static string batchOrderNo = string.Empty;
        private BindingSource dataGridBindingSource;
        private List<GloveBatchOrderDTO> dtBatchOrderDetails_cached;
        private List<GloveBatchOrderDTO> _gridtoExport;

        #endregion

        #region Load Form

        public GloveBatchOrder()
        {
            InitializeComponent();
            UIRefresh();
            dataGridBindingSource = new BindingSource();
            try
            {
                BatchPrint = true;
                WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.SURGICALGLOVESYSTEM);
                WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.PRINTSURGICALBATCHCARD);
                dgvLineSelection.AutoGenerateColumns = false;
                dgvLineDetails.AutoGenerateColumns = false;
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

        private void dgvLineDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) //Prevent event trigger on header click.
            {
                Login _passwordForm = new Login(Constants.Modules.SURGICALGLOVESYSTEM, _ManualPrintscreenName);
                _passwordForm.ShowDialog();
                if (!string.IsNullOrEmpty(_passwordForm.Authentication))
                {
                    string batchCardDate = dgvLineDetails.Rows[e.RowIndex].Cells[4].Value.ToString();
                    string resource = dgvLineDetails.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string bo = dgvLineDetails.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string serialNumber = dgvLineDetails.Rows[e.RowIndex].Cells[0].Value.ToString();
                    ReprintSRBC _frmManualPrint = new ReprintSRBC(batchCardDate, resource, bo, serialNumber);
                    _frmManualPrint.OperatorId = _passwordForm.Authentication;
                    _frmManualPrint.ShowDialog();
                }
            }
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
                    dtBatchOrderDetails = SurgicalGloveBLL.GetBatchOrderDetails("SGR");
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
                List<BatchOrderDetailsDTO> dtBatchOrderPrintDetails;
                dtBatchOrderPrintDetails = SurgicalGloveBLL.GetBatchOrderPrintDetails(batchOrderNo);
                dgvLineDetails.DataSource = dtBatchOrderPrintDetails;
                dgvLineDetails.AutoResizeColumns();
                dgvLineDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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
                ExceptionLogging(ex, _screenName, _className, "GloveBatchOrder", null);
            }
        }
        #endregion

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            getLineSelection();
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
                    if (FoundIt(item, qItems[i]))
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
                    CommonBLL.ExportToExcel<GloveBatchOrderDTO>(hearColumns, "GloveBatchOrder", _gridtoExport);
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
