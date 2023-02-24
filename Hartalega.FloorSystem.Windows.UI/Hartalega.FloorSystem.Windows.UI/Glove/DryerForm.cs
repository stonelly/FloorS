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
    public partial class DryerForm : FormBase
    {
        string _dryerProcess = "", _gloveCode = "";
        int _dryerId;
        bool formEdit = false;
        private string _screenName = "Glove Code SetUp - Dryer Form";
        private string _className = "DryerForm";

        public DryerForm(string gloveCode, string dryerProcess)
        {
            InitializeComponent();
            _gloveCode = gloveCode;

            _dryerProcess = dryerProcess;
            formEdit = false;
        }

        public DryerForm(string gloveCode, string dryerProcess, int dryerId)
        {
            InitializeComponent(); _gloveCode = gloveCode;

            _dryerProcess = dryerProcess;
            _dryerId = dryerId;
            formEdit = true;
            tbDryerProcess.Text = dryerProcess;
        }

        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(tbDryerProcess, "Dryer Process", ValidationType.Required));
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
            bool isDuplicate = false;
            bool isValid = false;
            if (ValidateRequiredFields())
            {
                DryerGloveDTO objDryer = new DryerGloveDTO();
                try
                {
                    objDryer.DryerProcess = tbDryerProcess.Text;
                    objDryer.GloveCode = _gloveCode;
                    int val = GloveCodeBLL.isDryerFormDuplicate(objDryer);

                    isValid = Convert.ToBoolean(GloveCodeBLL.isDryerFormValid(objDryer));
                    if (isValid)
                    {

                        isDuplicate = Convert.ToBoolean(val);

                        if (!isDuplicate)
                        {

                            if (formEdit)
                            {
                                //objDryer.GloveCode = _gloveCode;
                                //objDryer.DryerProcess = tbDryerProcess.Text;
                                objDryer.DryerId = _dryerId;
                                rowsReturned = GloveCodeBLL.EditDryerForm(objDryer);
                            }
                            else
                            {
                                rowsReturned = GloveCodeBLL.SaveDryerForm(objDryer);
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
                            //Messages.DUPLICATE_DRYER_VALUES
                            GlobalMessageBox.Show("DUPLICATE DRYER VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            tbDryerProcess.Text = string.Empty;
                            tbDryerProcess.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show("INVALID DRYER VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        tbDryerProcess.Text = string.Empty;
                        tbDryerProcess.Focus();
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
