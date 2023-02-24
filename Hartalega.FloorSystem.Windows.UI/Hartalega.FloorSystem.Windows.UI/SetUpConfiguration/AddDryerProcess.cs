using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class AddDryerProcess : Form
    {
        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "dryer Process Add";
        private string _className = "AddDryerProcess";
        private string[] _moduleId;
        private string LoggedInUserPassword;
        private int _dryerProcessId;

        private string _logScreenName = "Configuration SetUp - AddDryerProcess";
        #endregion
        public AddDryerProcess(DryerProcessDTO objDryerProcess)
        {
            InitializeComponent();
            if (objDryerProcess != null)
            {
                _dryerProcessId = objDryerProcess.CycloneProcessID;
                txtProcess.Text = objDryerProcess.CycloneProcess;
                chkStopped.Checked = objDryerProcess.Stopped == 1 ? true : false;
                txtColdCycle1.Text = Convert.ToString(objDryerProcess.Cold.ToString("0.##"));
                txtColdCycle2.Text = Convert.ToString(objDryerProcess.RCold.ToString("0.##"));
                txtColdCycle3.Text = Convert.ToString(objDryerProcess.R2Cold.ToString("0.##"));
                txtHotCycle1.Text = Convert.ToString(objDryerProcess.Hot.ToString("0.##"));
                txtHotCycle2.Text = Convert.ToString(objDryerProcess.RHot.ToString("0.##"));
                txtHotCycle3.Text = Convert.ToString(objDryerProcess.R2Hot.ToString("0.##"));
                txtProcess.ReadOnly = true;
            }
            btnSave.Text = objDryerProcess == null ? Constants.Save : Constants.Update;
            txtProcess.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;

            if (ValidateRequiredFields())
            {
                DryerProcessDTO objDryerProcess = new DryerProcessDTO();
                try
                {
                    objDryerProcess.CycloneProcess = txtProcess.Text.Trim();
                    objDryerProcess.Cold = Convert.ToDecimal(txtColdCycle1.Text.Trim());
                    objDryerProcess.Hot = Convert.ToDecimal(txtHotCycle1.Text.Trim());
                    objDryerProcess.RCold = Convert.ToDecimal(txtColdCycle2.Text.Trim());
                    objDryerProcess.RHot = Convert.ToDecimal(txtHotCycle2.Text.Trim());
                    objDryerProcess.R2Cold = Convert.ToDecimal(txtColdCycle3.Text.Trim());
                    objDryerProcess.R2Hot = Convert.ToDecimal(txtHotCycle3.Text.Trim());
                    objDryerProcess.Stopped = chkStopped.Checked ? 1 : 0;
                    objDryerProcess.CreatedDate = DateTime.Now;
                    objDryerProcess.ModifiedDate = DateTime.Now;
                    if (btnSave.Text == Constants.Save)
                    {
                        isDuplicate = Convert.ToBoolean(DryerBLL.IsDryerProcessDuplicate(objDryerProcess.CycloneProcess));
                        if (!isDuplicate)
                        {
                            rowsReturned = DryerBLL.SaveOrUpdateDryerProcess(objDryerProcess, Constants.ZERO);
                            if (rowsReturned > 0)
                            { 
                                //event log myadamas 20190227
                                EventLogDTO EventLog = new EventLogDTO();
                                EventLog.CreatedBy = _loggedInUser;
                                Constants.EventLog evtAction = Constants.EventLog.Save;
                                EventLog.EventType = Convert.ToInt32(evtAction);
                                EventLog.EventLogType = Constants.eventlogtype;

                                var screenid = CommonBLL.GetScreenIdByScreenName(_logScreenName);
                                CommonBLL.InsertEventLog(EventLog, _logScreenName, screenid.ToString());

                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                this.Close();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_DRYERPROCESS_TEXT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtProcess.Text = string.Empty;
                            txtProcess.Focus();
                        }
                    }
                    else
                    {
                        rowsReturned = DryerBLL.SaveOrUpdateDryerProcess(objDryerProcess, _dryerProcessId);
                        if (rowsReturned > 0)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            this.Close();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }
            }
        }

        private Boolean ValidateRequiredFields()
        {
            bool status = false;
            string requiredFieldMessage = Messages.REQUIREDFIELDMESSAGE;
            string validDataMessage = Messages.INVALID_DATA_SUMMARY;

            if (txtProcess.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.DRYERPROCESS + Environment.NewLine;
            }
            if (txtColdCycle1.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.COLDCYCLE + Environment.NewLine;
            }
            if (txtColdCycle2.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.COLDCYCLE1 + Environment.NewLine;
            }
            if (txtColdCycle3.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.COLDCYCLE2 + Environment.NewLine;
            }
            if (txtHotCycle1.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.HOTCYCLE + Environment.NewLine;
            }
            if (txtHotCycle2.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.HOTCYCLE1 + Environment.NewLine;
            }
            if (txtHotCycle3.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.HOTCYCLE2 + Environment.NewLine;
            }
            if (requiredFieldMessage.Equals(Messages.REQUIREDFIELDMESSAGE))
            {
                status = true;
            }
            else
            {
                if (requiredFieldMessage != Messages.REQUIREDFIELDMESSAGE)
                {
                    GlobalMessageBox.Show(requiredFieldMessage, Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    if (txtProcess.Text.Equals(string.Empty))
                    {
                        txtProcess.Focus();
                    }
                    else if (txtColdCycle1.Text.Equals(string.Empty))
                    {
                        txtColdCycle1.Focus();
                    }
                    else if (txtColdCycle2.Text.Equals(string.Empty))
                    {
                        txtColdCycle2.Focus();
                    }
                    else if (txtColdCycle3.Text.Equals(string.Empty))
                    {
                        txtColdCycle3.Focus();
                    }
                    else if (txtHotCycle1.Text.Equals(string.Empty))
                    {
                        txtHotCycle1.Focus();
                    }
                    else if (txtHotCycle2.Text.Equals(string.Empty))
                    {
                        txtHotCycle2.Focus();
                    }
                    else if (txtHotCycle3.Text.Equals(string.Empty))
                    {
                        txtHotCycle3.Focus();
                    }
                }
                status = false;
            }
            return status;
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    ClearForm();
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnCancel.Name, null);
                return;
            }
        }

        private void ClearForm()
        {
            txtProcess.Clear();
            chkStopped.Checked = false;
            txtColdCycle1.Clear();
            txtColdCycle2.Clear();
            txtColdCycle3.Clear();
            txtHotCycle1.Clear();
            txtHotCycle2.Clear();
            txtHotCycle3.Clear();
        }

        private void txtColdCycle1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txtColdCycle2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txtColdCycle3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txtHotCycle1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txtHotCycle2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void txtHotCycle3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        private void AddDryerProcess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}