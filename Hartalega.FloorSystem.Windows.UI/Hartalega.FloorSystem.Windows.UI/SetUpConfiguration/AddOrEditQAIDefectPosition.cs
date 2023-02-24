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
    public partial class AddOrEditQAIDefectPosition : FormBase
    {
        #region Member Variables
        private int _defectPositionId;
        private int _defectId;
        private int _defectCategoryId;
        private string _control;
        private string _screenName = "Configuration SetUp - AddOrEditQAIDefectPosition";
        private string _className = "AddOrEditQAIDefectPosition";
        private string _loggedInUser;
        private string _keyStroke;
        private string _defectName;
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
        public AddOrEditQAIDefectPosition(string control, int defectPositionId, int defectId, int defectCategoryId, string loggedInUser)
        {
            InitializeComponent();
            _control = control;
            _loggedInUser = loggedInUser;
            _defectPositionId = defectPositionId;
            _defectCategoryId = defectCategoryId;
            _defectId = defectId;
            try
            {
                txtDefectPositionId.Text = SetUpConfigurationBLL.GetNextRecordId("DefectPosition");
                BindDefectPosition();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefectPosition", null);
                return;
            }          
        }

        /// <summary>
        /// Constructor initialising values retrieved from main form for Edit Defect
        /// </summary>
        /// <param name="ReasonType"></param>
        /// <param name="ReasonTypeId"></param>
        /// <param name="ModuleId"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
       
        public AddOrEditQAIDefectPosition(string control,int defectPositionId,int defectId, int defectCategoryId, string keyStroke,string defectName, string loggedInUser)
        {
            InitializeComponent();
            _control = control;
            _loggedInUser = loggedInUser;
            _defectPositionId = defectPositionId;
            _defectCategoryId = defectCategoryId;
             _defectId = defectId;
            txtDefectPositionId.Text = Convert.ToString(defectPositionId);
            _defectName = defectName;
            _keyStroke = keyStroke;
            txtDefectPositionName.Text = _defectName;
            txtKeyStroke.Text = _keyStroke;
            try
            {
                BindDefectPosition();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefectPosition_Load", null);
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
            validationMesssageLst.Add(new ValidationMessage(txtDefectPositionName, "Defect Position", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbDefectPosition, "Defect", ValidationType.Required));

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
            txtDefectPositionName.Text = string.Empty;
            cmbDefectPosition.SelectedIndex = Constants.MINUSONE;
            txtKeyStroke.Focus();
        }

        /// <summary>
        /// Binds the QAI defect category combobox
        /// </summary>
        private void BindDefectPosition()
        {
            if (cmbDefectPosition.Items.Count == Constants.ZERO)
            {
                try
                {
                    cmbDefectPosition.BindComboBox(SetUpConfigurationBLL.GetQAIDefectList(_defectCategoryId), true);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "BindDefectPosition", null);
                    return;
                }
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
                QAIDefectPositions objDefectPositions = new QAIDefectPositions();
                try
                {
                    objDefectPositions.DefectPositionId = Convert.ToInt16(txtDefectPositionId.Text.Trim());
                    objDefectPositions.DefectId = Convert.ToInt16(cmbDefectPosition.SelectedValue);
                    objDefectPositions.DefectPositionItem = txtDefectPositionName.Text.Trim();
                    objDefectPositions.KeyStroke = txtKeyStroke.Text.Trim();

                    if (_control == Constants.ADD_CONTROL)
                    {
                        isDefectDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectPositionDuplicate(objDefectPositions.DefectPositionItem,
                                                              objDefectPositions.DefectId));
                        isKeyStrokeDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectPositionKeyStrokeDuplicate(objDefectPositions.KeyStroke,
                                                                 objDefectPositions.DefectId));
                    }
                    else
                    {
                        if (!txtDefectPositionName.Text.Trim().Equals(_defectName))
                            isDefectDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectPositionDuplicate(objDefectPositions.DefectPositionItem,
                                                              objDefectPositions.DefectId));
                        if (!txtKeyStroke.Text.Trim().Equals(_keyStroke))
                            isKeyStrokeDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsDefectPositionKeyStrokeDuplicate(objDefectPositions.KeyStroke,
                                                                 objDefectPositions.DefectId));
                    }
                    if (!isDefectDuplicate && !isKeyStrokeDuplicate && !isDefectCodeDuplicate)
                    {

                        QAIDefectPositions objDefectPositionsOld = new QAIDefectPositions();
                        objDefectPositionsOld = SetUpConfigurationBLL.GetQAIDefectPositions(_defectId).Where(p => p.DefectPositionId == objDefectPositions.DefectPositionId).FirstOrDefault();
                        if (objDefectPositionsOld == null)
                        {
                            objDefectPositionsOld = new QAIDefectPositions();
                            objDefectPositions.ActionType = Constants.ActionLog.Add;
                        }
                        else
                        {
                            objDefectPositions.IsDeleted = true;
                            objDefectPositions.ActionType = Constants.ActionLog.Update;
                        }

                        rowsReturned = SetUpConfigurationBLL.SaveQAIDefectPositions(objDefectPositions, objDefectPositionsOld, _loggedInUser);
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
                            GlobalMessageBox.Show(Messages.DUPLICATE_DEFECT_POSITION, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            txtDefectPositionName.Text = string.Empty;
                            txtDefectPositionName.Focus();
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
        private void AddOrEditQAIDefectPosition_Load(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "AddOrEditQAIDefectPosition_Load", null);
                return;
            }
            cmbDefectPosition.SelectedValue = Convert.ToString(_defectId);
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

        #endregion 
    }
}
