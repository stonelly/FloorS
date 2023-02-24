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
    public partial class PageMaster : Form, IChangeNotification
    {
        private DataGridViewRow _objSelectedGridRow = new DataGridViewRow();
        public PageMaster()
        {
            InitializeComponent();
        }

        private void btnPageMaster_Click(object sender, EventArgs e)
        {
            AddEditPageMaster objAddPageMaster = new AddEditPageMaster();
            objAddPageMaster.OnScreenDataInsertUpdate += AddEditPageMaster_OnChildDataInsertUpdate;  
            objAddPageMaster.MdiParent = this.MdiParent;
            objAddPageMaster.Dock = DockStyle.Fill;
            this.MdiParent.Text = objAddPageMaster.Text;
            objAddPageMaster.LoadOrders(_objSelectedGridRow, "ADD");
            objAddPageMaster.Show();
            //this.Close();
        }

        void AddEditPageMaster_OnChildDataInsertUpdate(object sender, EventArgs e)
        {
            BindPageMasterGrid();
        }

        private void btnEditPageMaster_Click(object sender, EventArgs e)
        {
            int rowIndex = gvPageMaster.CurrentCell.RowIndex;
            _objSelectedGridRow = gvPageMaster.Rows[rowIndex];

            AddEditPageMaster objEditPageMaster = new AddEditPageMaster();
            objEditPageMaster.OnScreenDataInsertUpdate += AddEditPageMaster_OnChildDataInsertUpdate;  
            objEditPageMaster.MdiParent = this.MdiParent;
            this.MdiParent.Text = objEditPageMaster.Text;
            objEditPageMaster.Dock = DockStyle.Fill;
            objEditPageMaster.LoadOrders(_objSelectedGridRow, "EDIT");
            objEditPageMaster.Show();
            //this.Close();
        }

        private void PageMaster_Load(object sender, EventArgs e)
        {
            BindPageMasterGrid();
        }

        private void BindPageMasterGrid()
        {
            DataTable dtPageMasterData = null;
            dtPageMasterData=SecurityModuleBLL.GetPageMasterDetails();
            gvPageMaster.AutoGenerateColumns = false;
            gvPageMaster.AutoSize = false;
            gvPageMaster.DataSource = dtPageMasterData;
        }

        private void PageMaster_KeyDown(object sender, KeyEventArgs e)
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
