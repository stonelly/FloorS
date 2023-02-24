using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Add or Edit QAI Defect Category
    /// File Type: Code file
    /// </summary>
    public partial class AddOrEditQAIDefectCategory : FormBase
    {
        #region Member Variables
        private string _control;
        private string _screenName = "Configuration SetUp - AddOrEditQAIDefectCategory";
        private string _className = "AddOrEditQAIDefectCategory";
        private string _loggedInUser;
        private string _defectCategory;
        private int _defectSequence;
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
        public AddOrEditQAIDefectCategory(string loggedInUser,string control)
        {
            InitializeComponent();
            _control = control;
            _loggedInUser = loggedInUser;
            try
            {
                txtCategoryId.Text = SetUpConfigurationBLL.GetNextRecordId("DefectCategory");
                BindDefectCategoryType();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefectCategory", null);
                return;
            }
        }

        /// <summary>
        /// Constructor initialising values retrieved from main form for Edit category
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public AddOrEditQAIDefectCategory(string control, int defectCategoryId, string defectCategory, int sequence,
                                           string loggedInUser, int defectCategoryTypeId)
        {
            InitializeComponent();
            _control = control;
            txtCategoryId.Text = Convert.ToString(defectCategoryId);
            txtDefectCategory.Text = defectCategory;
            txtSequence.Text = Convert.ToString(sequence);
            _loggedInUser = loggedInUser;
            _defectCategory = defectCategory;
            _defectSequence = sequence;
            BindDefectCategoryType();
            cmbDefectCategoryType.SelectedValue = Convert.ToString(defectCategoryTypeId);
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Validation of required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtDefectCategory, "QAI Defect Category", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbDefectCategoryType, "Defect Category Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtSequence, "Sequence", ValidationType.Required));
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
            txtDefectCategory.Text = string.Empty;
            txtSequence.Text = string.Empty;
            txtDefectCategory.Focus();
            BindDefectCategoryType();
        }

        /// <summary>
        /// Binds the QAI defect category type combobox
        /// </summary>
        private void BindDefectCategoryType()
        {
            try
            {
                cmbDefectCategoryType.BindComboBox(SetUpConfigurationBLL.GetQAIDefectCategoryTypeList(), true);
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
        /// Event Handler for validating defect sequence
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSequence_Leave(object sender, EventArgs e)
        {
            try
            {
                // Validation for Protein Spec
                if (!string.IsNullOrEmpty(txtSequence.Text.Trim()))
                {
                    int integ;
                    if (!Int32.TryParse(Convert.ToString(txtSequence.Text.Trim()), out integ))
                    {
                        GlobalMessageBox.Show(Messages.INVALIDDATA + "\nDefect Sequence", Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtSequence.Text = String.Empty;
                        txtSequence.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "txtSequence_Leave", null);
                return;
            }
        }
        /// <summary>
        /// Event Handler for Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;
            bool isSequenceDuplicate = false;

            if (ValidateRequiredFields())
            {
                QAIDefectCategory objDefectCategory = new QAIDefectCategory();
                try
                { 
                objDefectCategory.ID = Convert.ToInt16(txtCategoryId.Text.Trim());
                objDefectCategory.DefectCategory = txtDefectCategory.Text.Trim();
                objDefectCategory.Sequence = Convert.ToInt16(txtSequence.Text.Trim());
                objDefectCategory.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                objDefectCategory.OperatorId = _loggedInUser;
                objDefectCategory.DefectCategoryTypeId = Convert.ToInt16(cmbDefectCategoryType.SelectedValue);
                //added by MYAdamas 20171004 to compare at audit log during update
                 objDefectCategory.DefectCategoryType = this.cmbDefectCategoryType.GetItemText(cmbDefectCategoryType.SelectedItem);

                    if (_control == Constants.ADD_CONTROL)
                {
                    isDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectCategoryDuplicate(objDefectCategory.DefectCategory));

                    isSequenceDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsSequenceDuplicate(objDefectCategory.Sequence));
                }
                else
                {
                    if (!txtDefectCategory.Text.Trim().Equals(_defectCategory))
                        isDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectCategoryDuplicate(objDefectCategory.DefectCategory));

                    if (!txtSequence.Text.Trim().Equals(Convert.ToString(_defectSequence)))
                        isSequenceDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsSequenceDuplicate(objDefectCategory.Sequence));

                }
                    if (!isDuplicate && !isSequenceDuplicate)
                    {
                        QAIDefectCategory objDefectCategoryOld = new QAIDefectCategory();
                        objDefectCategoryOld = SetUpConfigurationBLL.GetQAIDefectCategory().Where(p => p.ID == objDefectCategory.ID).FirstOrDefault();
                        if (objDefectCategoryOld == null)
                        {
                            objDefectCategoryOld = new QAIDefectCategory();
                            objDefectCategory.ActionType = Constants.ActionLog.Add;
                        }
                        else
                        {
                            objDefectCategoryOld.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                            objDefectCategoryOld.OperatorId = _loggedInUser;
                            objDefectCategory.IsDeleted = true;
                            objDefectCategory.ActionType = Constants.ActionLog.Update;
                        }

                        rowsReturned = SetUpConfigurationBLL.SaveQAIDefectCategory(objDefectCategory, objDefectCategoryOld,_loggedInUser);
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
                        if (isDuplicate)
                        {
                          
                            GlobalMessageBox.Show(Messages.DUPLICATE_DEFECT_CATEGORY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtDefectCategory.Text = string.Empty;
                            txtDefectCategory.Focus();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_DEFECT_SEQUENCE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtSequence.Text = string.Empty;
                            txtSequence.Focus();
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
        /// Event Handler for Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrEditQAIDefectCategory_Load(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefectCategory_Load", null);
                return;
            }
            txtSequence.Sequence();
        }
        #endregion
    }
}
