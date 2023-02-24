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
    public partial class AddWasherProgram : Form
    {

        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "Washer Process Add";
        private string _className = "AddWasherProgram";
        private string[] _moduleId;
        private string LoggedInUserPassword;
        private int _washerProgramId;
        #endregion
        public AddWasherProgram(WasherProgramDTO washerProgram)
        {
            InitializeComponent();
            if (washerProgram != null)
            {
                _washerProgramId = washerProgram.WasherProgramId;
                txtProgram.Text = washerProgram.WasherProgram;
                txtTotalMinutes.Text = Convert.ToString(washerProgram.Totalminutes.ToString("0.##"));
                chkStopped.Checked = washerProgram.Stopped == 1 ? true : false;
                txtProgram.ReadOnly = true;
            }
            btnSave.Text = washerProgram == null ? Constants.Save : Constants.Update;
            txtProgram.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;

            if (ValidateRequiredFields())
            {
                WasherProgramDTO objWasherProgram = new WasherProgramDTO();
                try
                {

                    objWasherProgram.Totalminutes = Convert.ToDecimal(txtTotalMinutes.Text.Trim());
                    objWasherProgram.Stopped = chkStopped.Checked ? 1 : 0;
                    objWasherProgram.WasherProgram = txtProgram.Text.Trim();
                    Random rn = new Random();
                    objWasherProgram.Recid = Convert.ToInt64("56371" + rn.Next(10000, 99999).ToString()); //need to check this
                    objWasherProgram.CreatedDateTime = DateTime.Now;
                    objWasherProgram.ModifiedDateTime = DateTime.Now;

                    if (btnSave.Text == Constants.Save)
                    {
                        isDuplicate = Convert.ToBoolean(WasherBLL.IsWasherProgramDuplicate(objWasherProgram.WasherProgram));
                        if (!isDuplicate)
                        {
                            rowsReturned = WasherBLL.SaveOrUpdateWasherProgram(objWasherProgram, Constants.ZERO);
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
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_WASHERPROGRAM_TEXT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtProgram.Text = string.Empty;
                            txtProgram.Focus();
                        }
                    }
                    else
                    {
                        rowsReturned = WasherBLL.SaveOrUpdateWasherProgram(objWasherProgram, _washerProgramId);
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

            if (txtProgram.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.WASHERPROGRAM + Environment.NewLine;
            }
            if (txtTotalMinutes.Text.Equals(string.Empty))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.TOTALMINUTES + Environment.NewLine;
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
                    if (txtProgram.Text.Equals(string.Empty))
                    {
                        txtProgram.Focus();
                    }
                    else if (txtTotalMinutes.Text.Equals(string.Empty))
                    {
                        txtTotalMinutes.Focus();
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
            txtProgram.Clear();
            txtTotalMinutes.Clear();
            chkStopped.Checked = false;
            txtProgram.Focus();
        }

        private void AddWasherProgram_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
