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
    public partial class PermissionMaster : Form, IChangeNotification
    {
        private DataGridViewRow _objSelectedGridRow = new DataGridViewRow();

        public PermissionMaster()
        {
            InitializeComponent();
        }

        private void btnAddPermission_Click(object sender, EventArgs e)
        {
            AddEditPermissionMaster objAddPer = new AddEditPermissionMaster();
            objAddPer.OnPermissionDataInsertUpdate += AddEditPermissionMaster_OnChildDataInsertUpdate; 
            objAddPer.MdiParent = this.MdiParent;
            this.MdiParent.Text = objAddPer.Text;
            objAddPer.Dock = DockStyle.Fill;
            objAddPer.LoadOrders(_objSelectedGridRow, "ADD");
            objAddPer.Show();
            //this.Close();
        }

        private void btnEditPermission_Click(object sender, EventArgs e)
        {
            AddEditPermissionMaster objEditPer = new AddEditPermissionMaster();
            objEditPer.OnPermissionDataInsertUpdate += AddEditPermissionMaster_OnChildDataInsertUpdate; 
            objEditPer.MdiParent = this.MdiParent;
            this.MdiParent.Text = objEditPer.Text;
            objEditPer.Dock = DockStyle.Fill;

            if (gvPermissionMaster.Rows.Count > 0)
            {
                int rowIndex = gvPermissionMaster.CurrentCell.RowIndex;
                _objSelectedGridRow = gvPermissionMaster.Rows[rowIndex];
                objEditPer.LoadOrders(_objSelectedGridRow, "EDIT");
            }
            else
            {
                objEditPer.LoadOrders(_objSelectedGridRow, "ADD");
            }
            objEditPer.Show();
            //this.Close();
        }

        void AddEditPermissionMaster_OnChildDataInsertUpdate(object sender, EventArgs e)
        {
            BindPermissionMasterGrid();
        }

        private void PermissionMaster_Load(object sender, EventArgs e)
        {
            BindPermissionMasterGrid();
        }

        private void BindPermissionMasterGrid()
        {
            DataTable dtPermissionMasterData = null;
            dtPermissionMasterData = SecurityModuleBLL.GetPermissionMasterDetails();
            gvPermissionMaster.AutoGenerateColumns = false;
            gvPermissionMaster.AutoSize = false;
            gvPermissionMaster.DataSource = dtPermissionMasterData;
        }

        private void PermissionMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                (this.MdiParent as MainMenu).NotifyChildren();
                this.Close();
            }
        }

        public void ChangeNotify()
        {
            this.MdiParent.Text = Messages.MAINE_MENU_NAME;
        }

        public void NotifyChildren()
        {
            foreach (IChangeNotification frmChild in this.MdiChildren)
            {
                frmChild.ChangeNotify();
            }
        }
    }
}
