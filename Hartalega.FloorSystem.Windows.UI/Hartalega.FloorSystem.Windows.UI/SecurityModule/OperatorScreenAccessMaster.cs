using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    public partial class OperatorScreenAccessMaster : Form, IChangeNotification
    {
        private DataGridViewRow _objSelectedGridRow = new DataGridViewRow();
        public OperatorScreenAccessMaster()
        {
            InitializeComponent();
        }

        /// <summary>
        /// AddEditScreenAccessMaster_OnChildDataInsertUpdate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddEditScreenAccessMaster_OnChildDataInsertUpdate(object sender, EventArgs e)
        {
            BindScreenAccessMasterGrid();
        }

        /// <summary>
        /// BindScreenAccessMasterGrid
        /// </summary>
        private void BindScreenAccessMasterGrid()
        {
            DataTable dtScreenAccessMasterData = null;
            dtScreenAccessMasterData = SecurityModuleBLL.GetOperatorModulePermissionMappingDetails();
            gvScreenAccessMaster.AutoGenerateColumns = false;
            gvScreenAccessMaster.AutoSize = false;
            gvScreenAccessMaster.DataSource = dtScreenAccessMasterData;
        }

        /// <summary>
        /// PageMaster_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                (this.MdiParent as MainMenu).NotifyChildren();
                this.Close();
            }
        }

        /// <summary>
        /// ChangeNotify
        /// </summary>
        public void ChangeNotify()
        {
            this.MdiParent.Text = Messages.MAINE_MENU_NAME;
        }

        /// <summary>
        /// NotifyChildren
        /// </summary>
        public void NotifyChildren()
        {
            foreach (IChangeNotification frmChild in this.MdiChildren)
            {
                frmChild.ChangeNotify();
            }
        }

        /// <summary>
        /// ScreenAccessMaster_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenAccessMaster_Load(object sender, EventArgs e)
        {
            BindScreenAccessMasterGrid();
        }

        /// <summary>
        /// btnAddScreenAccessMaster_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddScreenAccessMaster_Click(object sender, EventArgs e)
        {
            AddEditOperatorScreenAccessMaster objAddScreenAccessMaster = new AddEditOperatorScreenAccessMaster();
            objAddScreenAccessMaster.OnScreenDataInsertUpdate += AddEditScreenAccessMaster_OnChildDataInsertUpdate;
            objAddScreenAccessMaster.MdiParent = this.MdiParent;
            objAddScreenAccessMaster.Dock = DockStyle.Fill;
            this.MdiParent.Text = objAddScreenAccessMaster.Text;
            objAddScreenAccessMaster.LoadOrders(_objSelectedGridRow, "ADD");
            objAddScreenAccessMaster.Show();
        }

        /// <summary>
        /// btnEditScreenAccessMaster_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditScreenAccessMaster_Click(object sender, EventArgs e)
        {
            int rowIndex = gvScreenAccessMaster.CurrentCell.RowIndex;
            _objSelectedGridRow = gvScreenAccessMaster.Rows[rowIndex];

            AddEditOperatorScreenAccessMaster objEditScreenAccessMaster = new AddEditOperatorScreenAccessMaster();
            objEditScreenAccessMaster.OnScreenDataInsertUpdate += AddEditScreenAccessMaster_OnChildDataInsertUpdate;
            objEditScreenAccessMaster.MdiParent = this.MdiParent;
            this.MdiParent.Text = objEditScreenAccessMaster.Text;
            objEditScreenAccessMaster.Dock = DockStyle.Fill;
            objEditScreenAccessMaster.LoadOrders(_objSelectedGridRow, "EDIT");
            objEditScreenAccessMaster.Show();
        }
    }
}
