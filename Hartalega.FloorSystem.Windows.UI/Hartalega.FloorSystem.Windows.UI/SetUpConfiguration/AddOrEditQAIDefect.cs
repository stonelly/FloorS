using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add or Edit QAI Defect
    /// File Type: Code file
    /// </summary>
    public partial class AddOrEditQAIDefect : FormBase
    {
        #region Member Variables
        private int _defectCategoryId;
        private string _control;
        private string _screenName = "Configuration SetUp - AddOrEditQAIDefect";
        private string _className = "AddOrEditQAIDefect";
        private string _loggedInUser;
        private string _keyStroke;
        private string _defectName;
        private string _defectCode;
        private string _qcType;
        private int _defectCategoryGroupId;
        #endregion

        #region Parameterised Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form for Add Defect
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public AddOrEditQAIDefect(string control, int defectCategoryId,string loggedInUser)
        {
            InitializeComponent();
            _control = control;
            _loggedInUser = loggedInUser;
            try
            {
                txtDefectId.Text = SetUpConfigurationBLL.GetNextRecordId("DefectMaster");
                BindDefectCategory();
                BindQcType();
                BindCategoryGroup();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefect", null);
                return;
            }          
            _defectCategoryId = defectCategoryId;
        }

        /// <summary>
        /// Constructor initialising values retrieved from main form for Edit Defect
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
       
        public AddOrEditQAIDefect(string control,int defectId,int defectCategoryId,string keyStroke,string defectName,string defectCode,
                                  string defectCategory,bool isAnd,string loggedInUser, string qcType, int defectCategoryGroupId)
        {
            InitializeComponent();
            _control = control;
            _defectCategoryId = defectCategoryId;
            txtDefectId.Text = Convert.ToString(defectId);
            _defectName = defectName;
            _defectCode = defectCode;
            _keyStroke = keyStroke;
            txtDefectCode.Text = _defectCode;
            txtDefectName.Text = _defectName;
            txtKeyStroke.Text = _keyStroke;
            _loggedInUser = loggedInUser;
            _qcType = qcType;
            chkAndDefect.Checked = isAnd;
            _defectCategoryGroupId = defectCategoryGroupId;
            try
            {
                BindDefectCategory();
                BindQcType();
                BindCategoryGroup();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefect_Load", null);
                return;
            }
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Validation of required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtKeyStroke, "Key Stroke", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtDefectName, "Defect Name", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtDefectCode, "Defect Code", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbDefectCategory, "Defect Category", ValidationType.Required));
            if(chkAndDefect.Checked)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbQCType, "QC Type", ValidationType.Required));
            }
            return ValidateForm();
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
            ClearForm();
        }

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtKeyStroke.Text = string.Empty;
            txtDefectName.Text = string.Empty;
            txtDefectCode.Text = string.Empty;
            cmbDefectCategory.SelectedIndex = Constants.MINUSONE;
            chkAndDefect.Checked = false;
            txtKeyStroke.Focus();
        }

        /// <summary>
        /// Binds the QAI defect category combobox
        /// </summary>
        private void BindDefectCategory()
        {
            if (cmbDefectCategory.Items.Count == Constants.ZERO)
            {
                try
                {
                    cmbDefectCategory.BindComboBox(SetUpConfigurationBLL.GetQAIDefectCategoryList(), true);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "BindDefectCategory", null);
                    return;
                }
            }
        }

        /// <summary>
        /// Bind Qc Type ComoBox
        /// </summary>
        private void BindQcType()
        {
            try
            {  
                cmbQCType.BindComboBox(CommonBLL.GetQCType(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindQcType", null);
                return;
            }

        }

        private void BindCategoryGroup()
        {
            try
            {
                cmbCategoryGroup.BindComboBox(CommonBLL.GetEnumText(Constants.DEFECT_CATEGORY_TYPE), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindCategoryGroup", null);
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
            int rowsReturned = 0;
            bool isDefectDuplicate = false;
            bool isKeyStrokeDuplicate = false;
            bool isDefectCodeDuplicate = false;

            if (ValidateRequiredFields())
            {
                QAIDefectDetails objDefectDetails = new QAIDefectDetails();
                try
                {
                    objDefectDetails.DefectID = Convert.ToInt16(txtDefectId.Text.Trim());
                    objDefectDetails.DefectCategoryId = Convert.ToInt16(cmbDefectCategory.SelectedValue);
                    objDefectDetails.DefectItem = txtDefectName.Text.Trim();
                    objDefectDetails.DefectCode = txtDefectCode.Text.Trim();
                    objDefectDetails.KeyStroke = txtKeyStroke.Text.Trim();
                    objDefectDetails.IsAnd = chkAndDefect.Checked;
                    objDefectDetails.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    objDefectDetails.OperatorId = _loggedInUser;
                    objDefectDetails.QCType = cmbQCType.Text;
                    objDefectDetails.DefectCategory = this.cmbDefectCategory.GetItemText(cmbDefectCategory.SelectedItem);
                    objDefectDetails.DefectCategoryGroupId = Convert.ToInt16(cmbCategoryGroup.SelectedValue);

                    if (_control == Constants.ADD_CONTROL)
                    {
                        isDefectDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectDuplicate(objDefectDetails.DefectItem,
                                                              objDefectDetails.DefectCategoryId));
                        isKeyStrokeDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsKeyStrokeDuplicate(objDefectDetails.KeyStroke,
                                                                 objDefectDetails.DefectCategoryId));
                        isDefectCodeDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectCodeDuplicate(objDefectDetails.DefectCode,
                                                                 objDefectDetails.DefectCategoryId));
                    }
                    else
                    {
                        if (!txtDefectName.Text.Trim().Equals(_defectName))
                            isDefectDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectDuplicate(objDefectDetails.DefectItem,
                                                                  objDefectDetails.DefectCategoryId));
                        if (!txtKeyStroke.Text.Trim().Equals(_keyStroke))
                            isKeyStrokeDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsKeyStrokeDuplicate(objDefectDetails.KeyStroke,
                                                                     objDefectDetails.DefectCategoryId));
                        if (!txtDefectCode.Text.Trim().Equals(_defectCode))
                            isDefectCodeDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectCodeDuplicate(objDefectDetails.DefectCode,
                                                                     objDefectDetails.DefectCategoryId));
                    }
                    if (!isDefectDuplicate && !isKeyStrokeDuplicate && !isDefectCodeDuplicate)
                    {

                        QAIDefectDetails objDefectDetailsOld = new QAIDefectDetails();
                        objDefectDetailsOld = SetUpConfigurationBLL.GetQAIDefectDetails(_defectCategoryId).Where(p => p.DefectID == objDefectDetails.DefectID).FirstOrDefault();
                        if (objDefectDetailsOld == null)
                        {
                            objDefectDetailsOld = new QAIDefectDetails();
                            objDefectDetails.ActionType = Constants.ActionLog.Add;
                        }
                        else
                        {
                            objDefectDetailsOld.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                            objDefectDetailsOld.OperatorId = _loggedInUser;
                            objDefectDetails.IsDeleted = true;
                            objDefectDetails.ActionType = Constants.ActionLog.Update;
                        }

                        rowsReturned = SetUpConfigurationBLL.SaveQAIDefectDetails(objDefectDetails, objDefectDetailsOld, _loggedInUser);
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
                        if (isDefectDuplicate)
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_DEFECT, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtDefectName.Text = string.Empty;
                            txtDefectName.Focus();
                        }
                        else if(isDefectCodeDuplicate)
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_DEFECT_CODE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtDefectCode.Text = string.Empty;
                            txtDefectCode.Focus();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_KEYSTROKE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtKeyStroke.Text = string.Empty;
                            txtKeyStroke.Focus();
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
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrEditQAIDefect_Load(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefect_Load", null);
                return;
            }
            cmbDefectCategory.SelectedValue = Convert.ToString(_defectCategoryId);
            cmbCategoryGroup.SelectedValue = Convert.ToString(_defectCategoryGroupId);

            if (chkAndDefect.Checked)
            {
                cmbQCType.Enabled = true;
                cmbQCType.TabStop = true;
                if (!string.IsNullOrEmpty(_qcType))
                    cmbQCType.SelectedText = Convert.ToString(_qcType);
            }
            else
            {
                BindQcType();
                cmbQCType.Enabled = false;
                cmbQCType.TabStop = false;                
            }
        }

        /// <summary>
        /// Event Handler for Key stroke textbox leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtKeyStroke_Leave(object sender, EventArgs e)
        {
            bool isValid = Regex.IsMatch(txtKeyStroke.Text.Trim(), @"[A-Z0-9\\\;',\\\]./\\[]");
            if(!isValid)
            {
                GlobalMessageBox.Show(Messages.VALID_KEYSTROKE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                txtKeyStroke.Focus();
            }
        }

        /// <summary>
        /// Event Handler for setting focus on checkbox on Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAndDefect_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
            }
        }

        /// <summary>
        /// Event Handler for checked change event of AND defect checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAndDefect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAndDefect.Checked)
            {
                cmbQCType.Enabled = true;
                cmbQCType.TabStop = true;
                if (!string.IsNullOrEmpty(_qcType))
                    cmbQCType.SelectedText = Convert.ToString(_qcType);
            }
            else
            {
                BindQcType();
                cmbQCType.Enabled = false;
                cmbQCType.TabStop = false;                
            }
        }
        #endregion 
    }
}
