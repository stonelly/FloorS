using System;
using System.Windows.Forms;

using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Edit Reason
    /// File Type: Code file
    /// </summary>
    public partial class EditReason : Form
    {
        #region Member Variables
        int _reasonTypeId;
        int _reasonTextId;
        int _moduleId;
        #endregion

        /// <summary>
        /// Constructor initialising values retrieved form main form
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ReasonTextId"></param>
        /// <param name="ReasonText"></param>
        /// <param name="isScheduled"></param>
        /// <returns></returns>
        public EditReason(string ReasonType, int ReasonTypeId, int ReasonTextId, string ReasonText, bool IsScheduled, int ModuleId)
        {
            InitializeComponent();
            txtReasonType.Text = ReasonType.Trim();
            txtReasonText.Text = ReasonText;
            chkSchedule.Checked = IsScheduled;
            _reasonTypeId = ReasonTypeId;
            _reasonTextId = ReasonTextId;
            _moduleId = ModuleId;
        }

        /// <summary>
        /// Hotkeys used in the form
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        #region User Methods

        /// <summary>
        /// Validation of Reason text field
        /// </summary>
        private bool ValidateRequiredFields()
        {
            if (txtReasonText.Text.Length > Constants.REASON_TEXT_LENGTH)
                return false;
            else
                return true;
        }        

        #endregion

        #region Event Handler

        /// <summary>
        /// Event Handler for Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned;
            
                if (ValidateRequiredFields())
                {
                    if (!string.IsNullOrEmpty(txtReasonText.Text))
                    {
                        ReasonDTO objReasonText = new ReasonDTO();
                        objReasonText.ReasonText = txtReasonText.Text.Trim();
                        objReasonText.ReasonTypeId = _reasonTypeId;
                        objReasonText.IsScheduled = chkSchedule.Checked;
                        objReasonText.ReasonTextId = _reasonTextId;

                        try
                        {
                           rowsReturned = SetUpConfigurationBLL.EditReasonText(objReasonText);
                        }
                        catch (FloorSystemException ex)
                        {
                            int result = CommonBLL.LogExceptionToDB(ex, "SetUp Configuration - Reason Master Add Reason Text", "ReasonMaster", "SaveReasonText", string.Empty);
                            MessageBox.Show("(" + result.ToString() + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        if (rowsReturned > 0)
                        {
                            MessageBox.Show("Data saved succesfully");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error occured while saving the data");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter the data.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid data for the field.");
                }
           
        }

        /// <summary>
        /// Event Handler for Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you wish to leave this page?", "Confirm Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }  
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSchedule_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        /// <summary>
        /// Event Handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditReason_Load(object sender, EventArgs e)
        {
            if (_moduleId == Convert.ToInt32(Constants.Modules.WASHER) ||
                        _moduleId == Convert.ToInt32(Constants.Modules.DRYER) ||
                        _moduleId == Convert.ToInt32(Constants.Modules.PRODUCTION))
            {
                lblSchedule.Visible = true;
                chkSchedule.Visible = true;
            }
        }
        #endregion

       

      
    }
}
