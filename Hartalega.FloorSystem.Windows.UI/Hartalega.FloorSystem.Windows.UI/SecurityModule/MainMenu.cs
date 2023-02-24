using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.IntegrationServices;

namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainMenu : Form
    {
        /// <summary>
        /// MainMenu
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// employeeMaintenanceMenuItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void employeeMaintenanceMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChild();
            EmployeeMaster objEmp = new EmployeeMaster();
            objEmp.MdiParent = this;
            this.Text = objEmp.Text;
           
            objEmp.Dock = DockStyle.Fill;
            objEmp.Show();
            EnableDisableMDIMenu(MDIParentMenu.EmployeeMaintenance);
        }

        /// <summary>
        /// roleMaintenanceMenuItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roleMaintenanceMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChild();
            RoleMaintenance objRoleMaint = new RoleMaintenance();
            objRoleMaint.MdiParent = this;
            this.Text = objRoleMaint.Text;
           
            objRoleMaint.Dock = DockStyle.Fill;
            objRoleMaint.Show();
            EnableDisableMDIMenu(MDIParentMenu.RoleMaintenance);
        }

        /// <summary>
        /// permissionMasterMenuItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void permissionMasterMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChild();
            PermissionMaster objPermission = new PermissionMaster();
            objPermission.MdiParent = this;
            this.Text = objPermission.Text;
           
            objPermission.Dock = DockStyle.Fill;
            objPermission.Show();
            EnableDisableMDIMenu(MDIParentMenu.PermissionMaster);
        }

        /// <summary>
        /// pageMasterMenuItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pageMasterMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChild();
            PageMaster objPageMaster = new PageMaster();
            objPageMaster.MdiParent = this;
            this.Text = objPageMaster.Text;
          
            objPageMaster.Dock = DockStyle.Fill;
            objPageMaster.Show();
            EnableDisableMDIMenu(MDIParentMenu.ScreenMaster);
        }

        /// <summary>
        /// NotifyChildren
        /// </summary>
        public void NotifyChildren()
        {
            foreach (IChangeNotification frmChild in this.MdiChildren)
            {
                frmChild.ChangeNotify();
                EnableDisableMDIMenu(MDIParentMenu.None);
            }
        }

        /// <summary>
        /// CloseAllMDIChild
        /// </summary>
        private void CloseAllMDIChild()
        {
            foreach (Form childForm in this.MdiChildren)
                childForm.Close();
        }

        /// <summary>
        /// EnableDisableMDIMenu
        /// </summary>
        /// <param name="menuName"></param>
        private void EnableDisableMDIMenu(MDIParentMenu menuName)
        {
            employeeMaintenanceMenuItem.Enabled = true;
            roleMaintenanceMenuItem.Enabled = true;
            permissionMasterMenuItem.Enabled = true;
            pageMasterMenuItem.Enabled = true;
            screenAccessMasterMenuItem.Enabled = true;

            switch (menuName)
            {
                case MDIParentMenu.EmployeeMaintenance:
                    employeeMaintenanceMenuItem.Enabled = false;
                    break;
                case MDIParentMenu.RoleMaintenance:
                    roleMaintenanceMenuItem.Enabled = false;
                    break;
                case MDIParentMenu.PermissionMaster:
                    permissionMasterMenuItem.Enabled = false;
                    break;
                case MDIParentMenu.ScreenMaster:
                    pageMasterMenuItem.Enabled = false;
                    break;
                case MDIParentMenu.ScreenAccessMaster:
                    screenAccessMasterMenuItem.Enabled = false;
                    break;
                case MDIParentMenu.None:
                    break;
            }
        }

        /// <summary>
        /// MainMenu_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseAllMDIChild();
                this.Close();
            }
        }

        /// <summary>
        /// screenAccessMasterMenuItem_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void screenAccessMasterMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllMDIChild();
            OperatorScreenAccessMaster objScreenAccessMasterMaster = new OperatorScreenAccessMaster();
            objScreenAccessMasterMaster.MdiParent = this;
            this.Text = objScreenAccessMasterMaster.Text;

            objScreenAccessMasterMaster.Dock = DockStyle.Fill;
            objScreenAccessMasterMaster.Show();
            EnableDisableMDIMenu(MDIParentMenu.ScreenAccessMaster);
        }
    }
}

/// <summary>
/// MDIParentMenu
/// </summary>
enum MDIParentMenu
{
    EmployeeMaintenance,
    RoleMaintenance,
    PermissionMaster,
    ScreenMaster,
    ScreenAccessMaster,
    None
};
