using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Glove
{
    public partial class GloveAdd : FormBase
    {
        private string _screenName = "Glove Code SetUp - New Glove Code Form";
        private string _className = "GloveAdd";
        public GloveAdd()
        {
            InitializeComponent();
        }

        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(tbGloveCode, "Dryer Process", ValidationType.Required));
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

            if (ValidateRequiredFields())
            {
                GloveCodeDTO objGloveCode = new GloveCodeDTO();
                try
                {
                    objGloveCode.GloveCode = tbGloveCode.Text;
                    objGloveCode.Barcode = tbBarcode.Text;
                    objGloveCode.GloveCategory = tbCategory.Text;
                    objGloveCode.Description = tbDescription.Text;
                    int val = GloveCodeBLL.isGloveAddDuplicate(objGloveCode);
                    isDuplicate = Convert.ToBoolean(val);

                    if (!isDuplicate)
                    {
                        rowsReturned = GloveCodeBLL.SaveGloveCode(objGloveCode);
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
                        GlobalMessageBox.Show("DUPLICATE Glove Code VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        tbGloveCode.Text = string.Empty;
                        tbGloveCode.Focus();
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
