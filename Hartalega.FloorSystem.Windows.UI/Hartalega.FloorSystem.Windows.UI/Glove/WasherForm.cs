using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using Hartalega.FloorSystem.Framework;
using System.Collections.Generic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework.DbExceptionLog;

namespace Hartalega.FloorSystem.Windows.UI.Glove
{
    public partial class WasherForm : FormBase
    {
        string _washerProgram= "", _gloveCode = "";
        int _washerId;
        bool formEdit = false;
        private string _screenName = "Glove Code SetUp - Dryer Form";
        private string _className = "DryerForm";

        public WasherForm(string gloveCode,string washerProgram)
        {
            InitializeComponent(); _gloveCode = gloveCode;
            _gloveCode = gloveCode;
            _washerProgram = washerProgram;
            formEdit = false;          
        }

        public WasherForm(string gloveCode, string washerProgram, int washerId)
        {
            InitializeComponent();
            _washerProgram = washerProgram; _gloveCode = gloveCode;
            _gloveCode = gloveCode;
            _washerId = washerId;
            formEdit = true;
            tbWasherProgram.Text = washerProgram;
        }

        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(tbWasherProgram, "Washer Program", ValidationType.Required));
            return ValidateForm();
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false, isValid = false;

            if (ValidateRequiredFields())
            {
                WasherGloveDTO objWasher = new WasherGloveDTO();
                try
                {
                    objWasher.WasherProgram = tbWasherProgram.Text;
                    objWasher.GloveCode = _gloveCode;
                    isValid = Convert.ToBoolean(GloveCodeBLL.isWasherFormValid(objWasher));

                    if (isValid)
                    {
                        isDuplicate = Convert.ToBoolean(GloveCodeBLL.isWasherFormDuplicate(objWasher));

                        if (!isDuplicate)
                        {

                            if (formEdit)
                            {
                                //objWasher.WasherProgram = tbWasherProgram.Text;
                                objWasher.WasherId = _washerId;
                                //objWasher.GloveCode = _gloveCode;
                                rowsReturned = GloveCodeBLL.EditWasherForm(objWasher);
                            }
                            else
                            {
                                rowsReturned = GloveCodeBLL.SaveWasherForm(objWasher);
                            }
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
                            //Messages.DUPLICATE_WASHER_VALUES
                            GlobalMessageBox.Show("DUPLICATE WASHER VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            tbWasherProgram.Text = string.Empty;
                            tbWasherProgram.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show("INVALID WASHER VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        tbWasherProgram.Text = string.Empty;
                        tbWasherProgram.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }
            }
        }
    }
}
