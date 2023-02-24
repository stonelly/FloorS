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
    public partial class RoleMaintenance : Form, IChangeNotification
    {
        private DataGridViewRow _objSelectedGridRow = new DataGridViewRow();

        public RoleMaintenance()
        {
            InitializeComponent();
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            AddEditRoleModulePermission objAddRole = new AddEditRoleModulePermission();
            objAddRole.OnRoleDataInsertUpdate += AddEditRoleModulePermission_OnChildDataInsertUpdate; 
            objAddRole.MdiParent = this.MdiParent;
            this.MdiParent.Text = objAddRole.Text;
            objAddRole.Dock = DockStyle.Fill;
            objAddRole.LoadAddEditRoleMaster(_objSelectedGridRow, "ADD");
            objAddRole.Show();
            //this.Close();
        }

        private void btnEditRole_Click(object sender, EventArgs e)
        {
            int rowIndex = gvRoleMaintenance.CurrentCell.RowIndex;
            _objSelectedGridRow = gvRoleMaintenance.Rows[rowIndex];

            AddEditRoleModulePermission objEditEmp = new AddEditRoleModulePermission();
            objEditEmp.OnRoleDataInsertUpdate += AddEditRoleModulePermission_OnChildDataInsertUpdate; 
            objEditEmp.MdiParent = this.MdiParent;
            this.MdiParent.Text = objEditEmp.Text;
            objEditEmp.Dock = DockStyle.Fill;
            objEditEmp.LoadAddEditRoleMaster(_objSelectedGridRow, "EDIT");
            objEditEmp.Show();
            //this.Close();
        }

        void AddEditRoleModulePermission_OnChildDataInsertUpdate(object sender, EventArgs e)
        {
            BindRoleMaintenanceGrid();
        }

        private void RoleMaintenance_Load(object sender, EventArgs e)
        {
            BindRoleMaintenanceGrid();
        }

        private void BindRoleMaintenanceGrid()
        {
            gvRoleMaintenance.AutoGenerateColumns = false;
            gvRoleMaintenance.AutoSize = false;
            gvRoleMaintenance.DataSource = SecurityModuleBLL.GetRoleMaintenanceDetails();
        }

        private void RoleMaintenance_KeyDown(object sender, KeyEventArgs e)
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
