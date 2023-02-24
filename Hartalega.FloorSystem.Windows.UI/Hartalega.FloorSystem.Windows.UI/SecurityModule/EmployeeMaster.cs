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
    public partial class EmployeeMaster : Form, IChangeNotification 
    {
        private DataGridViewRow _objSelectedGridRow = new DataGridViewRow();

        public EmployeeMaster()
        {
            InitializeComponent();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            AddEmployee objNewEmp = new AddEmployee();
            objNewEmp.OnEmployeeDataInsertUpdate += AddEditEmployeeDetails_OnChildDataInsertUpdate; 
            objNewEmp.MdiParent = this.MdiParent ;
            this.MdiParent.Text = objNewEmp.Text;
            objNewEmp.Dock = DockStyle.Fill;
            objNewEmp.Show();
            //this.Close();
        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            int rowIndex = gvEmployeeMaster.CurrentCell.RowIndex;
            _objSelectedGridRow = gvEmployeeMaster.Rows[rowIndex];

            EditEmployeeDetails objEditEmp = new EditEmployeeDetails();
            objEditEmp.OnEmployeeDataInsertUpdate += AddEditEmployeeDetails_OnChildDataInsertUpdate; 
            objEditEmp.MdiParent = this.MdiParent;
            this.MdiParent.Text = objEditEmp.Text;
            objEditEmp.Dock = DockStyle.Fill;
            objEditEmp.LoadEditEmployeePage(_objSelectedGridRow);
            objEditEmp.Show();
            //this.Close();
        }

        void AddEditEmployeeDetails_OnChildDataInsertUpdate(object sender, EventArgs e)
        {
            BindEmployeeMasterGrid();
        }

        private void EmployeeMaster_Load(object sender, EventArgs e)
        {
            BindEmployeeMasterGrid();
        }

        private void BindEmployeeMasterGrid()
        {
            gvEmployeeMaster.AutoGenerateColumns = false;
            gvEmployeeMaster.AutoSize = false;
            gvEmployeeMaster.DataSource = SecurityModuleBLL.GetEmployeeMasterDetails();
        }

        private void EmployeeMaster_KeyDown(object sender, KeyEventArgs e)
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
