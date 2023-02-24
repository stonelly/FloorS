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
    public partial class AddWasherStage : Form
    {

        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "Washer Stage Add";
        private string _className = "AddWasherStage";
        private string[] _moduleId;
        private string LoggedInUserPassword;
        private int _WasherprocessId;
        private long _washerRefId;

        private string _logScreenName = "Configuration SetUp - AddWasherStage";
        #endregion
        public AddWasherStage(WasherprocessDTO washerProcess, long washerRefId)
        {
            InitializeComponent();
            _washerRefId = washerRefId;
            if (washerProcess != null)
            {
                _WasherprocessId = washerProcess.WasherprocessId;
                txtProcess.Text = washerProcess.Process;
                txtMinutes.Text = Convert.ToString(washerProcess.Minutes.ToString("0.##"));
                txtStage.Text = washerProcess.Stage;
                txtStage.ReadOnly = true;
            }
            btnSave.Text = washerProcess == null ? Constants.Save : Constants.Update;
            if (washerProcess == null)
                txtStage.Focus();
            else
                txtProcess.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;
            if (ValidateRequiredFields())
            {
                WasherprocessDTO objWasherProcess = new WasherprocessDTO();
                try
                {
                    objWasherProcess.Stage = txtStage.Text.Trim();
                    objWasherProcess.Process = txtProcess.Text.Trim();
                    objWasherProcess.Minutes = Convert.ToDecimal(txtMinutes.Text.Trim());
                    objWasherProcess.WasherRefId = _washerRefId;
                    //Random rn = new Random();
                    //objWasherProcess.Recid = Convert.ToInt64("56372" + rn.Next(10000, 99999).ToString()); //need to check this
                    objWasherProcess.CreatedDateTime = DateTime.Now;
                    objWasherProcess.ModifiedDateTime = DateTime.Now;


                    if (btnSave.Text == Constants.Save)
                    {
                        isDuplicate = Convert.ToBoolean(WasherBLL.IsWasherStageDuplicate(objWasherProcess.Stage, _washerRefId));
                        if (!isDuplicate)
                        {
                            rowsReturned = WasherBLL.SaveOrUpdateWasherProcess(objWasherProcess, Constants.ZERO);
                            if (rowsReturned > 0)
                            {
                                decimal totalMinutes = GetWasherStageTotalMinutesSum(_washerRefId);
                                int count = WasherBLL.UpdateWasherProgramTotalMinutes(totalMinutes, _washerRefId);
                                if (count > 0)
                                {
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
                                GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_WASHERSTAGE_TEXT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtStage.Text = string.Empty;
                            txtStage.Focus();
                        }
                    }
                    else
                    {
                        rowsReturned = WasherBLL.SaveOrUpdateWasherProcess(objWasherProcess, _WasherprocessId);
                        if (rowsReturned > 0)
                        {
                            decimal totalMinutes = GetWasherStageTotalMinutesSum(_washerRefId);
                            int count = WasherBLL.UpdateWasherProgramTotalMinutes(totalMinutes, _washerRefId);
                            if (count > 0)
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

            if (txtStage.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.STAGE + Environment.NewLine;
            }
            if (txtProcess.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.WASHERPROCESS + Environment.NewLine;
            }
            if (txtMinutes.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.MINUTES + Environment.NewLine;
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
                    if (txtStage.Text.Equals(string.Empty))
                    {
                        txtStage.Focus();
                    }
                    else if (txtProcess.Text.Equals(string.Empty))
                    {
                        txtProcess.Focus();
                    }
                    else if (txtMinutes.Text.Equals(string.Empty))
                    {
                        txtMinutes.Focus();
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
            txtStage.Clear();
            txtProcess.Clear();
            txtMinutes.Clear();
            txtStage.Focus();
        }

        private decimal GetWasherStageTotalMinutesSum(long washerRefId)
        {
            return WasherBLL.GetWasherStageTotalMinutesSum(washerRefId);
        }

        private void AddWasherStage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtMinutes_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
