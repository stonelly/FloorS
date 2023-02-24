using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Washer
{
    public partial class WasherProcess : FormBase
    {
        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "Washer Process";
        private string _className = "WasherProcess";
        private string[] _moduleId;
        private string LoggedInUserPassword;
        #endregion
        public WasherProcess(string loggedInUser)
        {
            InitializeComponent();
            btnProcessAdd.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\images\page_white_add.png");
            btnProcessEdit.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\Resources\pg_white_edit.png");
            //btnProcessRemove.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\Resources\cross.png");
            btnStageAdd.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\images\page_white_add.png");
            btnStageEdit.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\Resources\pg_white_edit.png");
            btnStageRemove.Image = System.Drawing.Image.FromFile(@"C:\Users\DEVzen3\Source\Workspaces\HSB Floor System Upgrade\Hartalega.FloorSystem.Windows.UI\Hartalega.FloorSystem.Windows.UI\Resources\cross.png");

            _loggedInUser = Convert.ToString(loggedInUser);
            try
            {
                LoggedInUserPassword = SetUpConfigurationBLL.GetModuleIdForLoggedInUserPassword(Convert.ToString(loggedInUser));
                string moduleId = SetUpConfigurationBLL.GetModuleIdForLoggedInUser(_loggedInUser);
                _moduleId = moduleId.Trim(' ', ',').Split(new char[] { ',' });
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "WasherProcess", null);
                return;
            }
        }


        private void getWasherProgramDetails()
        {
            dgvWasherProgram.Rows.Clear();
            List<WasherProgramDTO> washerProgramDTO = null;
            try
            {
                washerProgramDTO = WasherBLL.GetWasherProgramDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getWasherProgramDetails", null);
                return;
            }
            if (washerProgramDTO != null)
            {
                for (int i = 0; i < washerProgramDTO.Count; i++)
                {
                    dgvWasherProgram.Rows.Add();
                    dgvWasherProgram[0, i].Value = washerProgramDTO[i].WasherProgram;
                    dgvWasherProgram[1, i].Value = washerProgramDTO[i].Totalminutes.ToString("0.##");
                    dgvWasherProgram[2, i].Value = washerProgramDTO[i].Stopped;
                    dgvWasherProgram[3, i].Value = washerProgramDTO[i].Recid;
                    dgvWasherProgram[4, i].Value = washerProgramDTO[i].WasherProgramId;
                }
                //getWasherProcessDetails();

            }
        }

        private void getWasherProcessDetails()
        {
            dgvWasherProcess.Rows.Clear();
            List<WasherprocessDTO> WasherprocessDTO = null;
            int row = dgvWasherProgram.CurrentRow.Index;
            long recId = Convert.ToInt64(dgvWasherProgram[3, row].Value);
            try
            {
                WasherprocessDTO = WasherBLL.GetWasherProcessDetails(recId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getWasherProcessDetails", null);
                return;
            }
            if (WasherprocessDTO != null)
            {
                for (int i = 0; i < WasherprocessDTO.Count; i++)
                {
                    dgvWasherProcess.Rows.Add();
                    dgvWasherProcess[0, i].Value = WasherprocessDTO[i].Stage;
                    dgvWasherProcess[1, i].Value = WasherprocessDTO[i].Process;
                    dgvWasherProcess[2, i].Value = WasherprocessDTO[i].Minutes.ToString("0.##");
                    dgvWasherProcess[3, i].Value = WasherprocessDTO[i].WasherprocessId;
                }
                // CurrentReasonTypeDetails();
            }
            btnStageEdit.Enabled = dgvWasherProcess.Rows.Count > 0 ? true : false;
            btnStageRemove.Enabled = dgvWasherProcess.Rows.Count > 0 ? true : false;
        }


        private void WasherProcess_Load(object sender, EventArgs e)
        {
            getWasherProgramDetails();
            getWasherProcessDetails();
            this.dgvWasherProgram.AllowUserToAddRows = false;
            this.dgvWasherProcess.AllowUserToAddRows = false;
            this.dgvWasherProgram.AllowUserToDeleteRows = false;
            this.dgvWasherProcess.AllowUserToDeleteRows = false;
            btnProcessEdit.Enabled = dgvWasherProgram.Rows.Count > 0 ? true : false;
            btnProcessRemove.Enabled = dgvWasherProgram.Rows.Count > 0 ? true : false;
            btnStageEdit.Enabled = dgvWasherProcess.Rows.Count > 0 ? true : false;
            btnStageRemove.Enabled = dgvWasherProcess.Rows.Count > 0 ? true : false;

        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            dgvWasherProgram.Rows.Clear();
            dgvWasherProcess.Rows.Clear();
        }

        private void dgvWasherProgram_SelectionChanged(object sender, EventArgs e)
        {
            //int i = dgvWasherProgram.CurrentRow.Index;
            getWasherProcessDetails();
            btnStageEdit.Enabled = dgvWasherProcess.Rows.Count > 0 ? true : false;
            btnStageRemove.Enabled = dgvWasherProcess.Rows.Count > 0 ? true : false;
        }

        private void btnProcessAdd_Click(object sender, EventArgs e)
        {

            new AddWasherProgram(null).ShowDialog();
            getWasherProgramDetails();
            getWasherProcessDetails();
        }

        private void btnProcessRemove_Click(object sender, EventArgs e)
        {

            try
            {
                if (GlobalMessageBox.Show(Messages.DELETE_WASHERPROGRAM, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    int i = dgvWasherProgram.CurrentRow.Index;
                    int washerProgramId = Convert.ToInt32(dgvWasherProgram[4, i].Value);
                    int rowsReturned = WasherBLL.DeleteWasherProgramRecord(washerProgramId);
                    if (rowsReturned > 0)
                    {
                        GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        getWasherProgramDetails();
                        getWasherProcessDetails();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnProcessRemove_Click", null);
                return;
            }

        }

        private void btnStageAdd_Click(object sender, EventArgs e)
        {
            int row = dgvWasherProgram.CurrentRow.Index;
            long washerRefId = Convert.ToInt64(dgvWasherProgram[3, row].Value);
            if (washerRefId != 0)
            {
                new AddWasherStage(null, washerRefId).ShowDialog();
                getWasherProgramDetails();
                dgvWasherProgram.Rows[row].Selected = true;
                dgvWasherProgram.Rows[0].Selected = false;
                dgvWasherProgram.CurrentCell = dgvWasherProgram.Rows[row].Cells[0];
                getWasherProcessDetails();
            }
        }

        private void btnStageRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalMessageBox.Show(Messages.DELETE_WASHERSTAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    int i = dgvWasherProcess.CurrentRow.Index;
                    int washerprocessId = Convert.ToInt32(dgvWasherProcess[3, i].Value);
                    int rowsReturned = WasherBLL.DeleteWasherProcessRecord(washerprocessId);
                    if (rowsReturned > 0)
                    {
                        int j = dgvWasherProgram.CurrentRow.Index;
                        long washerRefId = Convert.ToInt64(dgvWasherProgram[3, j].Value);
                        decimal totalMinutes = GetWasherStageTotalMinutesSum(washerRefId);
                        int count = WasherBLL.UpdateWasherProgramTotalMinutes(totalMinutes, washerRefId);
                        if (count > 0)
                        {
                            GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            getWasherProgramDetails();
                            dgvWasherProgram.Rows[j].Selected = true;
                            dgvWasherProgram.Rows[0].Selected = false;
                            dgvWasherProgram.CurrentCell = dgvWasherProgram.Rows[j].Cells[0];
                            getWasherProcessDetails();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }

                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnStageRemove_Click", null);
                return;
            }


        }

        private void btnProcessEdit_Click(object sender, EventArgs e)
        {
            WasherProgramDTO washerProgram = new WasherProgramDTO();
            int i = dgvWasherProgram.CurrentRow.Index;
            washerProgram.WasherProgram = Convert.ToString(dgvWasherProgram[0, i].Value);
            washerProgram.Totalminutes = Convert.ToDecimal(dgvWasherProgram[1, i].Value);
            washerProgram.Stopped = Convert.ToInt32(dgvWasherProgram[2, i].Value);
            washerProgram.WasherProgramId = Convert.ToInt32(dgvWasherProgram[4, i].Value);
            new AddWasherProgram(washerProgram).ShowDialog();

            getWasherProgramDetails();
            getWasherProcessDetails();
        }

        private void btnStageEdit_Click(object sender, EventArgs e)
        {
            WasherprocessDTO washerProcess = new WasherprocessDTO();
            int i = dgvWasherProcess.CurrentRow.Index;
            washerProcess.Stage = Convert.ToString(dgvWasherProcess[0, i].Value);
            washerProcess.Process = Convert.ToString(dgvWasherProcess[1, i].Value);
            washerProcess.Minutes = Convert.ToInt32(dgvWasherProcess[2, i].Value);
            washerProcess.WasherprocessId = Convert.ToInt32(dgvWasherProcess[3, i].Value);

            int row = dgvWasherProgram.CurrentRow.Index;
            long washerRefId = Convert.ToInt64(dgvWasherProgram[3, row].Value);
            if (washerProcess != null && washerRefId != 0)
            {
                new AddWasherStage(washerProcess, washerRefId).ShowDialog();
                getWasherProgramDetails();
                dgvWasherProgram.Rows[row].Selected = true;
                dgvWasherProgram.Rows[0].Selected = false;
                dgvWasherProgram.CurrentCell = dgvWasherProgram.Rows[row].Cells[0];
                getWasherProcessDetails();
            }
        }

        private decimal GetWasherStageTotalMinutesSum(long washerRefId)
        {
            return WasherBLL.GetWasherStageTotalMinutesSum(washerRefId);
        }

        private void WasherProcess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
