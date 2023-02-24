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

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    /// <summary>
    /// Module: QC Scanning
    /// Screen Name: Print Second Grade Sticker
    /// File Type: Code file
    /// </summary>
    public partial class PrintSecondGradeSticker : FormBase
    {
        #region Class Variables
        private static string _screenName = "QC Scanning - PrintSecondGradeSticker";
        private static string _className = "PrintSecondGradeSticker";
        private string _moduleId;
        private string _subModuleId;
        private string _barcode;
        private string _gloveType;
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintSecondGradeSticker" /> class.
        /// </summary>
        public PrintSecondGradeSticker()
        {
            InitializeComponent();
            try
            {
                txtLocation.Text = WorkStationDTO.GetInstance().Location;
                GetSize();
                _moduleId = CommonBLL.GetModuleId(Constants.QC_SCANNING_SYSTEM);
                _subModuleId = CommonBLL.GetSubModuleId(Constants.PRINT_SECONDGRADE_STICKER);
                cmbSecondGrade.BindComboBox(CommonBLL.GetEnumText(Constants.SECOND_GRADETYPE), true);
                cmbSecondGradePCs.BindComboBox(CommonBLL.GetEnumText(Constants.SECOND_GRADETYPE_PCs), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintSecondGradeSticker", string.Empty);
                return;
            }
        }

        /// <summary>
        /// Form load event - Get the Size master data & bind it with Size Combbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintSecondGradeSticker_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Binds the Size and Location ComboBox.
        /// </summary>
        private void GetSize()
        {
            // cmbSize.BindComboBox(CommonBLL.GetSize(), true);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Validate Glove Type & Show the Glove Type Description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtGloveType_Leave(object sender, EventArgs e)
        {
            GetGloveTypeDesc();
        }

        /// <summary>
        /// Show the selected size in the Size Selected textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  txtSizeSelected.Text = cmbSize.Text;           
        }

        /// <summary>
        /// Method to get Size Description 
        /// </summary>
        /// <param name="sender">Size combo box leave or change event</param>
        /// <param name="e">On change event argument</param>
        private void cmbSize_Change(object sender, EventArgs e)
        {
            string sizeName = cmbSize.Text;
            string sizeDescription = String.Empty;
            if (txtGloveType.Text != String.Empty && txtGlovetypeDesc.Text != String.Empty && cmbSize.DataSource != null)
            {
                if (!string.IsNullOrEmpty(sizeName))
                {
                    try
                    {
                        sizeDescription = CommonBLL.GetSizeDescription(sizeName);
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "cmbBoxSize_Change", cmbSize.Text, txtGlovetypeDesc.Text);
                        return;
                    }
                    if (sizeDescription != Constants.INVALID_MESSAGE)
                    {
                        txtSizeSelected.Text = sizeDescription;
                    }
                    else
                    {
                        cmbSize.Text = String.Empty;
                        txtSizeSelected.Text = String.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Validate & Save the Batch Card Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            BatchDTO objBatch = new BatchDTO();
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtGloveType, "Glove Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbSize, "Size", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtNoOfLabel, "No of Label", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbSecondGrade, "2nd Grade Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbSecondGradePCs, "2nd Grade PCs", ValidationType.Required));
            try
            {
                if (ValidateForm())
                {
                    if (_barcode != txtGloveType.Text)
                        GetGloveTypeDesc();
                    if (!string.IsNullOrEmpty(txtGloveType.Text))
                    {
                        validationMesssageLst = new List<ValidationMessage>();
                        validationMesssageLst.Add(new ValidationMessage(txtGloveType, "Glove Type", ValidationType.Required));
                        validationMesssageLst.Add(new ValidationMessage(cmbSize, "Size", ValidationType.Required));
                        validationMesssageLst.Add(new ValidationMessage(txtNoOfLabel, "No of Label", ValidationType.Required));
                        if (ValidateForm())
                        {
                            if (Convert.ToInt32(txtNoOfLabel.Text) <= Constants.TWOHUNDRED)
                            {
                                if (Convert.ToDecimal(txtNoOfLabel.Text) > Constants.ZERO)
                                {
                                    objBatch.Size = cmbSize.Text;
                                    objBatch.GloveType = txtGlovetypeDesc.Text;
                                    objBatch.WorkstationNumber = WorkStationDTO.GetInstance().WorkStationId;
                                    objBatch.LastModifiedOn = CommonBLL.GetCurrentDateAndTime();
                                    objBatch.LocationId = WorkStationDTO.GetInstance().LocationId;
                                    objBatch.Module = _moduleId;
                                    objBatch.SubModule = _subModuleId;
                                    objBatch.SecondGradeType = cmbSecondGrade.SelectedValue.ToString();
                                    objBatch.SecondGradePCs = Int16.Parse(cmbSecondGradePCs.SelectedValue.ToString());

                                    for (int i = Constants.ZERO; i < Convert.ToDecimal(txtNoOfLabel.Text); i++)
                                    {
                                        objBatch.SerialNumber = Constants.SERIAL_NUMBER_SECONDGRADE + QCScanningBLL.GetSerialNumberSecondGrade().PadLeft(Constants.NINE, Convert.ToChar(Convert.ToString(Constants.ZERO)));
                                        QCScanningBLL.SaveSecondGradeSticker(objBatch);
                                        QCScanningBLL.PrintDetails(objBatch);
                                    }
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    ClearForm();
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.INVALID_NO_OF_LABEL, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtNoOfLabel.Text = String.Empty;
                                    txtNoOfLabel.Focus();
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.NO_OF_LABEL_EXCEEDING, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                txtNoOfLabel.Text = String.Empty;
                                txtNoOfLabel.Focus();
                            }
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", objBatch.Size, objBatch.GloveType, objBatch.WorkstationNumber, objBatch.LastModifiedOn, objBatch.LocationId, objBatch.Module, objBatch.SubModule, objBatch.SerialNumber);
                return;
            }
        }

        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                ClearForm();
            }
        }

        /// <summary>
        /// Characters are not allowed in the No of Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNoOfLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Clear the values of all the controls. 
        /// </summary>
        private void ClearForm()
        {
            cmbSize.DataSource = null;
            txtGloveType.Text = String.Empty;
            txtGlovetypeDesc.Text = String.Empty;
            // cmbSize.SelectedIndex = Constants.MINUSONE;
            txtSizeSelected.Text = String.Empty;
            txtNoOfLabel.Text = String.Empty;
            txtGloveType.Focus();
            _gloveType = string.Empty;
            _barcode = string.Empty;
            cmbSecondGrade.SelectedIndex = Constants.MINUSONE;
            cmbSecondGradePCs.SelectedIndex = Constants.MINUSONE;
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        /// <summary>
        /// Get the Glove Type Description
        /// </summary>
        private void GetGloveTypeDesc()
        {
            if (!String.IsNullOrEmpty(txtGloveType.Text.Trim()))
            {
                try
                {
                    string gloveDesc = QCScanningBLL.GetGloveDescription(txtGloveType.Text);
                    if (!String.IsNullOrEmpty(gloveDesc) && gloveDesc != Constants.INVALID_MESSAGE)
                    {
                        txtGlovetypeDesc.Text = gloveDesc;
                        _gloveType = QCScanningBLL.GetGloveType(txtGloveType.Text);
                        cmbSize.BindComboBox(CommonBLL.GetSizeByGloveTypeTumbling(gloveDesc), true);
                        _barcode = txtGloveType.Text;

                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALID_GLOVE_TYPE, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ClearForm();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "GetGloveTypeDesc", txtGloveType.Text);
                    return;
                }
            }
            else
            {
                txtGlovetypeDesc.Text = string.Empty;
            }
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
        #endregion

    }
}
