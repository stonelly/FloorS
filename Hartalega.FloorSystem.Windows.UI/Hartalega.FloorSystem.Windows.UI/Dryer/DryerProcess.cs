using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Dryer
{
    public partial class DryerProcess : Form
    {

        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "Dryer Process";
        private string _className = "DryerProcess";
        private string[] _moduleId;
        private string LoggedInUserPassword;
        #endregion
        public DryerProcess(string loggedInUser)
        {
            InitializeComponent();
            btnAdd.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\images\page_white_add.png");
            btnEdit.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\Resources\pg_white_edit.png");
            // btnRemove.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\Resources\cross.png");


            _loggedInUser = Convert.ToString(loggedInUser);
            try
            {
                LoggedInUserPassword = SetUpConfigurationBLL.GetModuleIdForLoggedInUserPassword(Convert.ToString(loggedInUser));
                string moduleId = SetUpConfigurationBLL.GetModuleIdForLoggedInUser(_loggedInUser);
                _moduleId = moduleId.Trim(' ', ',').Split(new char[] { ',' });
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "DryerProcess", null);
                return;
            }
        }

        private void DryerProcess_Load(object sender, EventArgs e)
        {
            getDryerProcessDetails();
            DisplayDryerDetails();
            this.dgvDryerProcess.AllowUserToAddRows = false;
            this.dgvDryerProcess.AllowUserToDeleteRows = false;
        }

        private void getDryerProcessDetails()
        {
            dgvDryerProcess.Rows.Clear();
            List<DryerProcessDTO> dryerProcessDTO = null;
            try
            {
                dryerProcessDTO = DryerBLL.GetDryerProcessDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "DryerProcess", null);
                return;
            }
            if (dryerProcessDTO != null)
            {
                for (int i = 0; i < dryerProcessDTO.Count; i++)
                {
                    dgvDryerProcess.Rows.Add();
                    dgvDryerProcess[0, i].Value = dryerProcessDTO[i].CycloneProcess;
                    dgvDryerProcess[1, i].Value = dryerProcessDTO[i].Cold.ToString("0.##");
                    dgvDryerProcess[2, i].Value = dryerProcessDTO[i].Hot.ToString("0.##");
                    dgvDryerProcess[3, i].Value = dryerProcessDTO[i].RCold.ToString("0.##");
                    dgvDryerProcess[4, i].Value = dryerProcessDTO[i].RHot.ToString("0.##");
                    dgvDryerProcess[5, i].Value = dryerProcessDTO[i].R2Cold.ToString("0.##");
                    dgvDryerProcess[6, i].Value = dryerProcessDTO[i].R2Hot.ToString("0.##");
                    dgvDryerProcess[7, i].Value = dryerProcessDTO[i].Stopped;
                    dgvDryerProcess[8, i].Value = dryerProcessDTO[i].CycloneProcessID;
                }
            }
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            dgvDryerProcess.Rows.Clear();
        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalMessageBox.Show(Messages.DELETE_DRYERPROCESS, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    int row = dgvDryerProcess.CurrentRow.Index;
                    int dryerProcessId = Convert.ToInt32(dgvDryerProcess[8, row].Value);
                    int rowsReturned = DryerBLL.DeleteDryerProcessRecord(dryerProcessId);
                    if (rowsReturned > 0)
                    {
                        GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        getDryerProcessDetails();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnRemove_Click", null);
                return;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new AddDryerProcess(null).ShowDialog();
            getDryerProcessDetails();
        }

        private void dgvDryerProcess_SelectionChanged(object sender, EventArgs e)
        {
            DisplayDryerDetails();
        }

        private void DisplayDryerDetails()
        {
            if (dgvDryerProcess != null && dgvDryerProcess.SelectedRows.Count != 0)
            {
                txtProcess.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[0].Value);
                chkStopped.Checked = Convert.ToInt32(dgvDryerProcess.SelectedRows[0].Cells[7].Value) == 1 ? true : false;
                txtColdCycle1.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[1].Value);
                txtColdCycle2.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[3].Value);
                txtColdCycle3.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[5].Value);
                txtHotCycle1.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[2].Value);
                txtHotCycle2.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[4].Value);
                txtHotCycle3.Text = Convert.ToString(dgvDryerProcess.SelectedRows[0].Cells[6].Value);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DryerProcessDTO dryerProcess = new DryerProcessDTO();
            int i = dgvDryerProcess.CurrentRow.Index;
            dryerProcess.CycloneProcess = Convert.ToString(dgvDryerProcess[0, i].Value);
            dryerProcess.Cold = Convert.ToDecimal(dgvDryerProcess[1, i].Value);
            dryerProcess.Hot = Convert.ToDecimal(dgvDryerProcess[2, i].Value);
            dryerProcess.RCold = Convert.ToDecimal(dgvDryerProcess[3, i].Value);
            dryerProcess.RHot = Convert.ToDecimal(dgvDryerProcess[4, i].Value);
            dryerProcess.R2Cold = Convert.ToDecimal(dgvDryerProcess[5, i].Value);
            dryerProcess.R2Hot = Convert.ToDecimal(dgvDryerProcess[6, i].Value);
            dryerProcess.Stopped = Convert.ToInt32(dgvDryerProcess[7, i].Value);
            dryerProcess.CycloneProcessID = Convert.ToInt32(dgvDryerProcess[8, i].Value);
            new AddDryerProcess(dryerProcess).ShowDialog();
            getDryerProcessDetails();
        }

        private void DryerProcess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
