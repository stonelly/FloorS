using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class LineClearanceVerification : FormBase
    {
        public bool IsVerified { get; set; }
        public string EmployeeID { get; set; }
        public bool IsPeha = false;
        private string te = String.Empty;
        private DateTime startTime;
        private bool isCopyPaste = false;
        
        public LineClearanceVerification()
        {
            InitializeComponent();
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtEmpID.Text == String.Empty)
            {
                GlobalMessageBox.Show(Messages.EMPTY_EMPLOYEE_ID, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                this.txtEmpID.Focus();
            }
            else
            {
                int result = 0;
                string password = string.Empty;
                string empID = string.Empty;
                this.IsVerified = false;

                try
                {
                    empID = txtEmpID.Text;
                    password = txtPassword.Text;

                    result = FinalPackingBLL.IsEmployeeAllowToVerifyLineClearance(empID, password);
                    if (result == 0)
                    {
                        this.IsVerified = true;
                        this.EmployeeID = empID;
                        this.Close();
                    }
                    else if (result == 99)
                    {
                        GlobalMessageBox.Show(Messages.EMP_NOT_EXISTS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        this.txtEmpID.Focus();
                    }
                    else if (result == 98)
                    {
                        GlobalMessageBox.Show(Messages.INVALID_PIN, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        this.txtPassword.Focus();
                    }
                    else if (result == 97)
                    {
                        GlobalMessageBox.Show(Messages.VERIFY_DENIED, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        this.txtEmpID.Focus();
                    }
                }
                catch(FloorSystemException ex)
                {
                    int logResult = CommonBLL.LogExceptionToDB(ex, "Line Clearance Verification Screen" + "LineClearanceVerification", "btnOK_Click", txtPassword.Text);
                    GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\u0016')) //check if paste key
            {
                isCopyPaste = true;
                te = null;
                txtEmpID.Text = string.Empty;
            }
            else
            {
                isCopyPaste = false;
                if (string.IsNullOrEmpty(te))
                {
                    txtEmpID.Text = string.Empty;
                }

                te += e.KeyChar;

                if (e.KeyChar == '\r')
                {
                    te = null;
                }
            }
        }

        private void txtEmpID_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpID.Text.Length == 1)
            {
                startTime = DateTime.Now;
            }

            TimeSpan m = (DateTime.Now - startTime);

            if (isCopyPaste || m.TotalMilliseconds > 150)
            {
                txtEmpID.Text = string.Empty;
                te = null;
            }
        }

        private void LineClearanceVerification_Load(object sender, EventArgs e)
        {
            this.IsVerified = false;
            if (IsPeha)
            {
                this.Text = "Reset Tower Light Verification";
                groupBox1.Text = "Reset Tower Light";
            }
            else
            {
                this.Text = "Line Clearance Verification";
                groupBox1.Text = "Line Clearance Verification";
            }
        }
    }
}
