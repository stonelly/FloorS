using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add or Edit QAI Defect Category
    /// File Type: Code file
    /// </summary>
    public partial class AddGloveType : FormBase
    {
        #region Member Variables

        private string _screenName = "Configuration SetUp - AddGloveType";
        private string _className = "AddGloveType";
        private string _loggedInUser;

        #endregion

        #region Parameterised Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form for Add category
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public AddGloveType(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            cmbGloveCategory.BindComboBox(SetUpConfigurationBLL.GetGloveCategory(), true);
            cmbPowderFormula.BindComboBox(SetUpConfigurationBLL.GetEnumMaster("PowderFormula"), true);
            txtBarcode.Text = RunningNoBLL.NextRunningNo(RunningNoBLL.RunningNoType.GloveTypeBarcode, null, false);
        }
        
        #endregion

        #region User Methods        

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
        }        

        /// <summary>
        /// Binds the QAI defect category type combobox
        /// </summary>
        private void BindDefectCategoryType()
        {
            try
            {
                //cmbDefectCategoryType.BindComboBox(SetUpConfigurationBLL.GetQAIDefectCategoryTypeList(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindDefectCategoryType", null);
                return;
            }
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Event Handler for Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowReturned = 0;
            decimal result;
            GloveTypeMasterDTO newitem = new GloveTypeMasterDTO();
            GloveTypeMasterDTO olditem = new GloveTypeMasterDTO();

            try
            {
                string vldmsg = string.Empty;

                //if (string.IsNullOrEmpty(txtBarcode.Text))
                //    vldmsg += "\nBarcode";
                if (string.IsNullOrEmpty(txtGlovecode.Text))
                    vldmsg += "\nGlove Code";
                if (string.IsNullOrEmpty(txtDescription.Text))
                    vldmsg += "\nDescription";
                if (string.IsNullOrEmpty(Convert.ToString(cmbGloveCategory.SelectedValue)))
                    vldmsg += "\nGlove Category";
                if (cbProtein.Checked)
                    if (string.IsNullOrEmpty(Convert.ToString(txtProteinSpec.Text)))
                        vldmsg += "\nProtein Specification";
                if (cbPowder.Checked)
                    if (string.IsNullOrEmpty(Convert.ToString(cmbPowderFormula.SelectedValue)))
                        vldmsg += "\nPowder Formula";
                //check required
                if (string.IsNullOrEmpty(vldmsg))
                {
                    
                    newitem.BARCODE = txtBarcode.Text;
                    newitem.DESCRIPTION = txtDescription.Text;
                    newitem.GLOVECATEGORY = Convert.ToString(cmbGloveCategory.SelectedValue);
                    newitem.GLOVECODE = txtGlovecode.Text;
                    newitem.HOTBOX = cbHotbox.Checked ? 1 : 0;
                    newitem.POLYMER = cbPolymer.Checked ? 1 : 0;
                    newitem.POWDER = cbPowder.Checked ? 1 : 0;
                    newitem.POWDERFORMULA = Convert.ToInt32(cmbPowderFormula.SelectedValue);
                    newitem.PROTEIN = cbProtein.Checked ? 1 : 0;
                    newitem.PROTEINSPEC = string.IsNullOrEmpty(txtProteinSpec.Text) ? decimal.Zero : Convert.ToDecimal(txtProteinSpec.Text);
                    newitem.STOPPED = 0;
                    newitem.ActionType = Constants.ActionLog.Add;
                    int isDuplicate = 0;
                    isDuplicate = Convert.ToInt16(SetUpConfigurationBLL.IsGloveTypeMasterDuplicate(txtBarcode.Text, txtGlovecode.Text));
                    if (isDuplicate == 0)
                    {
                        rowReturned = SetUpConfigurationBLL.AddGloveType(olditem, newitem, _loggedInUser);
                        if (rowReturned > 0)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            this.Close();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                    }
                    else if (isDuplicate == 1)
                    {
                        GlobalMessageBox.Show(Messages.DUPLICATE_GLOVETYPEMASTER_GloveCode, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }          
                    else if (isDuplicate == 2)
                    {
                        GlobalMessageBox.Show(Messages.DUPLICATE_GLOVETYPEMASTER_BarCode, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Event Handler for validating Protein Spec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProteinSpec_Leave(object sender, EventArgs e)
        {
            try
            {
                // Validation for Protein Spec
                if (!string.IsNullOrEmpty(txtProteinSpec.Text.Trim()))
                {
                    decimal decima;
                    if (!Decimal.TryParse(Convert.ToString(txtProteinSpec.Text.Trim()), out decima))
                    {
                        GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_PROTEINSPEC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtProteinSpec.Text = String.Empty;
                        txtProteinSpec.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "txtProteinSpec_Leave", null);
                return;
            }
        }
       

        private void cbProtein_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProtein.Checked)
                txtProteinSpec.Enabled = true;
            else
            {
                txtProteinSpec.Enabled = false;
                txtProteinSpec.Text = string.Empty;
            }
        }

        private void cbPowder_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPowder.Checked)
                cmbPowderFormula.Enabled = true;
            else
            {
                cmbPowderFormula.Enabled = false;
                cmbPowderFormula.SelectedIndex=-1;
            }
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
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

        #endregion
    }
}
