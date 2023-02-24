using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name:Edit Line Record
    /// File Type: Code file
    /// </summary>     
    public partial class EditLineRecord : FormBase
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - EditLineRecord";
        private string _className = "EditLineRecord";
        private ProductionLineDTO _productionLineDTO;
        private List<int> _lbSize_Selection = new List<int>();
        private string _loggedInUser;
        #endregion

        #region Parameterised Constructors
        /// <summary>
        /// Constructor initialising values retrieved from main form 
        /// </summary>
        /// <param name="ProductionLineDTO productionLineDTO"></param>
        /// <returns></returns>
        public EditLineRecord(ProductionLineDTO productionLineDTO)
        {
            InitializeComponent();
            if (productionLineDTO != null)
            {
                BindBatchCardPrintFrequency();
                _productionLineDTO = productionLineDTO;
                txtLine.Text = productionLineDTO.LineNumber;
                cmbBatchFrequency.Text = productionLineDTO.BatchFrequency.Trim();
                chkOnlinePacking.Checked = productionLineDTO.IsOnline;
                txtLTSize.Text = productionLineDTO.LTGloveSize;
                txtRTSize.Text = productionLineDTO.RTGloveSize;
                txtLBSize.Text = productionLineDTO.LBGloveSize;
                txtRBSize.Text = productionLineDTO.RBGloveSize;
                _loggedInUser = productionLineDTO.OperatorId;
                chkPrintByFormer.Checked = productionLineDTO.IsPrintByFormer;
                if(!string.IsNullOrEmpty(cmbBatchFrequency.Text))
                {
                    chkDoubleFormer.Checked = productionLineDTO.IsDoubleFormer;
                }
            }
            BindPlants();
            cmbPlant.SelectedValue = _productionLineDTO.Plant;
            FillGloveType();
            FillFormerType();
            BindLatexTypes();
            cmbLatex.SelectedValue = _productionLineDTO.LatexType;
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
        /// Validation of Required fields
        /// </summary>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cmbLTGlove, "LT Glove Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtLTSize, "LT Glove Size", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbRTGlove, "RT Glove Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtRTSize, "RT Glove Size", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbBatchFrequency, "Batch Card Print Frequency", ValidationType.Required));
            if (chkDoubleFormer.Checked)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbLBGlove, "LB Glove Type", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(txtLBSize, "LB Glove Size", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbRBGlove, "RB Glove Type", ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(txtRBSize, "RB Glove Size", ValidationType.Required));
            }
            return ValidateForm();
        }

        /// <summary>
        /// Binds the Plant combobox
        /// </summary>
        private void BindPlants()
        {
            try
            {
                cmbPlant.BindComboBox(SetUpConfigurationBLL.GetPlantsList(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPlants", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Latex combobox
        /// </summary>
        private void BindLatexTypes()
        {
            try
            {
                cmbLatex.BindComboBox(CommonBLL.GetLatexType(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindLatexTypes", null);
                return;
            }
        }

        /// <summary>
        /// Binds the Batch Card Print Frequency combobox
        /// </summary>
        private void BindBatchCardPrintFrequency()
        {
            try
            {
                cmbBatchFrequency.BindComboBox(CommonBLL.GetEnumText(Constants.BATCH_PRINT_FREQUENCY), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindBatchCardPrintFrequency", null);
                return;
            }
        }

        /// <summary>
        /// Fills the Glove type controls
        /// </summary>
        private void FillGloveType()
        {
            try
            {
                //commented by MYAdamas 20171109 due to get all glove type from ax_avaglovecodetable , no longer from ax
                // List<DropdownDTO> gloveTypes = SetUpConfigurationBLL.GetGloveTypeFromAX(txtLine.Text.Trim());
                List<DropdownDTO> gloveTypes = CommonBLL.GetGloveType();

                BindingSource gloveType1 = new BindingSource();
                gloveType1.DataSource = gloveTypes;
                BindingSource gloveType2 = new BindingSource();
                gloveType2.DataSource = gloveTypes;
                BindingSource gloveType3 = new BindingSource();
                gloveType3.DataSource = gloveTypes;
                BindingSource gloveType4 = new BindingSource();
                gloveType4.DataSource = gloveTypes;
                BindingSource gloveType5 = new BindingSource();
                gloveType5.DataSource = gloveTypes;
                BindingSource gloveType6 = new BindingSource();
                gloveType6.DataSource = gloveTypes;
                BindingSource gloveType7 = new BindingSource();
                gloveType7.DataSource = gloveTypes;
                BindingSource gloveType8 = new BindingSource();
                gloveType8.DataSource = gloveTypes;
                //change from false to true onload show empty MYAdamas 20171109

                if (gloveType1.DataSource!=null)
                     cmbLBGlove.BindMultipleComboBox(gloveType1, true);

                if (gloveType2.DataSource != null)
                    cmbRBGlove.BindMultipleComboBox(gloveType2, true);


                if (gloveType3.DataSource != null)
                    cmbLTGlove.BindMultipleComboBox(gloveType3, true);


                if (gloveType4.DataSource != null)
                    cmbRTGlove.BindMultipleComboBox(gloveType4, true);



                if (gloveType5.DataSource != null)
                    cmbLBAltGlove.BindMultipleComboBox(gloveType5, true);



                if (gloveType6.DataSource != null) 
                    cmbRBAltGlove.BindMultipleComboBox(gloveType6, true);

                if (gloveType7.DataSource != null)
                    cmbLTAltGlove.BindMultipleComboBox(gloveType7, true);

                if (gloveType8.DataSource != null)
                    cmbRTAltGlove.BindMultipleComboBox(gloveType8, true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "FillGloveType", null);
                return;
            }
        }

        /// <summary>
        /// Fills the Former type controls
        /// </summary>
        private void FillFormerType()
        {
            try
            {
                List<DropdownDTO> formerTypes = CommonBLL.GetFormerType();

                BindingSource formerType1 = new BindingSource();
                formerType1.DataSource = formerTypes;
                BindingSource formerType2 = new BindingSource();
                formerType2.DataSource = formerTypes;
                BindingSource formerType3 = new BindingSource();
                formerType3.DataSource = formerTypes;
                BindingSource formerType4 = new BindingSource();
                formerType4.DataSource = formerTypes;


                if (formerType1.DataSource != null)
                    cmbLBFormer.BindMultipleComboBox(formerType1, true);

                if (formerType2.DataSource != null)
                    cmbLTFormer.BindMultipleComboBox(formerType2, true);


                if (formerType3.DataSource != null)
                    cmbRBFormer.BindMultipleComboBox(formerType3, true);

                if (formerType4.DataSource != null)
                    cmbRTFormer.BindMultipleComboBox(formerType4, true);

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "FillFormerType", null);
                return;
            }
        }

        /// <summary>
        /// Limit maximum selections in Size list box to 2
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="selection"></param>
        private void TrackSelectionChange(ListBox lb, List<int> selection)
        {
            ListBox.SelectedIndexCollection selectedIndices = lb.SelectedIndices;
            foreach (int index in selectedIndices)
                if (!selection.Contains(index)) selection.Add(index);
            foreach (int index in new List<int>(selection))
                if (!selectedIndices.Contains(index)) selection.Remove(index);
        }

        /// <summary>
        /// Enable Left Bottom and Right Bottom Tiers based on Double Former checkbox
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="selection"></param>
        private void EnableTiers()
        {
            if (!chkDoubleFormer.Checked)
            {
                gbLeftBottom.Enabled = false;
                gbRightBottom.Enabled = false;
                gbLeftBottom.TabStop = false;
                gbRightBottom.TabStop = false;
                cmbLBGlove.SelectedIndex = Constants.MINUSONE;
                cmbRBGlove.SelectedIndex = Constants.MINUSONE;
                cmbLBAltGlove.SelectedIndex = Constants.MINUSONE;
                cmbRBAltGlove.SelectedIndex = Constants.MINUSONE;
                txtLBSize.Text = string.Empty;
                txtRBSize.Text = string.Empty;
            }
            else
            {
                gbLeftBottom.Enabled = true;
                gbRightBottom.Enabled = true;
            }
        }

        /// <summary>
        /// Fills the Glove Size controls
        /// </summary>
        private void FillGloveSizes(ComboBox control)
        {
            ListBox lbList = new ListBox();
            if (control == cmbLTGlove)
            {
                lbList = lbLTSize;
            }
            else if (control == cmbRTGlove)
            {
                lbList = lbRTSize;
            }
            else if (control == cmbLBGlove)
            {
                lbList = lbLBSize;
            }
            else
            {
                lbList = lbRBSize;
            }
            try
            {
                //commented by MYAdamas 20171109 due to size get by new table AX_AVAGLOVERELCOMSIZE not AX_AVALINECONFIGURATIONINFORMATION
                //List<DropdownDTO> dtSizes = SetUpConfigurationBLL.GetGloveSizes(Convert.ToString(control.SelectedValue),
                //                                                                txtLine.Text.Trim());
                List<DropdownDTO> dtSizes = SetUpConfigurationBLL.GetGloveSizesByGloveType(Convert.ToString(control.SelectedValue));
                lbList.Items.Clear();
                if (dtSizes != null)
                {
                    for (int i = 0; i < dtSizes.Count; i++)
                    {
                        lbList.Items.Add(dtSizes[i].DisplayField);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, control.Name + "_SelectedIndexChanged", null);
                return;
            }
        }

        /// <summary>
        /// Fills the Size textbox with selected sizes
        /// </summary>
        private void FillSelectedSizes(ListBox control)
        {
            TextBox txtSize = new TextBox();
            if (control == lbLTSize)
            {
                txtSize = txtLTSize;
            }
            else if (control == lbRTSize)
            {
                txtSize = txtRTSize;
            }
            else if (control == lbLBSize)
            {
                txtSize = txtLBSize;
            }
            else
            {
                txtSize = txtRBSize;
            }

            StringBuilder size = new StringBuilder();
            string itemsSeletedForSize = string.Empty;
            TrackSelectionChange(control, _lbSize_Selection);

            // MyAdamas 20180116 - allow size more that 2 selections
            //if (control.SelectedItems.Count > Constants.TWO)
            //{
            //    control.SelectedItems.Remove(control.Items[_lbSize_Selection[_lbSize_Selection.Count - Constants.ONE]]);
            //}

            for (int i = Constants.ZERO; i < _lbSize_Selection.Count; i++)
            {
                size.Append(control.Items[_lbSize_Selection[i]]);
                size.Append(",");
            }
            itemsSeletedForSize = size.ToString();
            if (!string.IsNullOrEmpty(itemsSeletedForSize))
            {
                txtSize.Text = itemsSeletedForSize.Substring(Constants.ZERO, size.Length - Constants.ONE);
            }
            else
            {
                txtSize.Text = string.Empty;
            }

        }
        #endregion

        #region Event Handlers
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
        /// Event Handler for Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned;
            if (ValidateRequiredFields())
            {
                ProductionLineDTO objProductionLine = new ProductionLineDTO();
                try
                {

                
                    objProductionLine.BatchFrequency = Convert.ToString(cmbBatchFrequency.SelectedValue);
                    objProductionLine.IsOnline = chkOnlinePacking.Checked;
                    objProductionLine.IsDoubleFormer = chkDoubleFormer.Checked;
                    objProductionLine.LTGloveType = Convert.ToString(cmbLTGlove.SelectedValue);
                    objProductionLine.RTGloveType = Convert.ToString(cmbRTGlove.SelectedValue);
                    objProductionLine.LBGloveType = Convert.ToString(cmbLBGlove.SelectedValue);
                    objProductionLine.RBGloveType = Convert.ToString(cmbRBGlove.SelectedValue);
                    objProductionLine.LTGloveSize = txtLTSize.Text.Trim();
                    objProductionLine.RTGloveSize = txtRTSize.Text.Trim();
                    objProductionLine.LBGloveSize = txtLBSize.Text.Trim();
                    objProductionLine.RBGloveSize = txtRBSize.Text.Trim();
                    objProductionLine.IsPrintByFormer = chkPrintByFormer.Checked;
                    objProductionLine.LineNumber = txtLine.Text.Trim();
                    objProductionLine.WorkstationId = WorkStationDTO.GetInstance().WorkStationId;
                    objProductionLine.LocationId = WorkStationDTO.GetInstance().LocationId;
                    objProductionLine.Plant = Convert.ToString(cmbPlant.SelectedValue);
                    objProductionLine.OperatorId = _loggedInUser;
                    objProductionLine.LTAltGlove = Convert.ToString(cmbLTAltGlove.SelectedValue);
                    objProductionLine.RTAltGlove = Convert.ToString(cmbRTAltGlove.SelectedValue);
                    objProductionLine.LBAltGlove = Convert.ToString(cmbLBAltGlove.SelectedValue);
                    objProductionLine.RBAltGlove = Convert.ToString(cmbRBAltGlove.SelectedValue);
                    objProductionLine.LTFormerType = Convert.ToString(cmbLTFormer.SelectedValue);
                    objProductionLine.LBFormerType = Convert.ToString(cmbLBFormer.SelectedValue);
                    objProductionLine.RTFormerType = Convert.ToString(cmbRTFormer.SelectedValue);
                    objProductionLine.RBFormerType = Convert.ToString(cmbRBFormer.SelectedValue);
                    objProductionLine.LatexType = Convert.ToString(cmbLatex.SelectedValue);
                    objProductionLine.ActionType = Constants.ActionLog.Update;

                    ProductionLineDTO objProductionLineold = SetUpConfigurationBLL.GetProductionLineDetails().Where(p => p.LineNumber == objProductionLine.LineNumber).FirstOrDefault();


                    rowsReturned = SetUpConfigurationBLL.EditProductionLineDetails(objProductionLine, objProductionLineold, _loggedInUser);
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
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
        }

        /// <summary>
        /// Event Handler Checked status changed for Double former checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDoubleFormer_CheckedChanged(object sender, EventArgs e)
        {
            EnableTiers();           
        }

        /// <summary>
        /// Event Handler for Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLineRecord_Load(object sender, EventArgs e)
        {                      
            cmbLBGlove.Text = _productionLineDTO.LBGloveType;
            cmbRBGlove.Text = _productionLineDTO.RBGloveType;
            cmbLTGlove.Text = _productionLineDTO.LTGloveType;
            cmbRTGlove.Text = _productionLineDTO.RTGloveType;
            cmbLBAltGlove.Text = _productionLineDTO.LBAltGlove;
            cmbRBAltGlove.Text = _productionLineDTO.RBAltGlove;
            cmbLTAltGlove.Text = _productionLineDTO.LTAltGlove;
            cmbRTAltGlove.Text = _productionLineDTO.RTAltGlove;

            cmbLTFormer.SelectedValue = _productionLineDTO.LTFormerType;
            cmbLBFormer.SelectedValue = _productionLineDTO.LBFormerType;
            cmbRTFormer.SelectedValue = _productionLineDTO.RTFormerType;
            cmbRBFormer.SelectedValue = _productionLineDTO.RBFormerType;

            if (_productionLineDTO.ProdLoggingStartDateTime!=null && (_productionLineDTO.LastModifiedOn < _productionLineDTO.ProdLoggingStartDateTime))
            {
                GlobalMessageBox.Show(Messages.PRODUCTION_LOGGING_MESSAGE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
            EnableTiers();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "EditLineRecord_Load", null);
                return;
            }
        }

        /// <summary>
        /// Event Handler for Selected index changed of Glove type comboboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            FillGloveSizes(comboBox);
        }

        /// <summary>
        /// Event Handler for Selected index changed for size list boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            FillSelectedSizes(listBox);
        }
        #endregion
    }
}
