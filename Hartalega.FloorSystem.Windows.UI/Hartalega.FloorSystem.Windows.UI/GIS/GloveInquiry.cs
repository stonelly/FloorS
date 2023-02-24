using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.Common;

namespace Hartalega.FloorSystem.Windows.UI.GIS
{
    /// <summary>
    /// Module: Glove Inventory
    /// Screen Name: Glove Inquiry
    /// File Type: Code file
    /// </summary> 
    public partial class GloveInquiry : FormBase
    {
        #region Class Variables
        public List<GloveInquiryDetails> _lstGloveInquiry;
        private static string _screenName = "Glove Inventory - GloveInquiry";
        private static string _className = "GloveInquiry";
        private DateTime _curDateTime;
        #endregion

        #region Page Load
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanOut" /> class.
        /// </summary>
        public GloveInquiry()
        {
            InitializeComponent();            
            GetGloveTypeSizeandLocation();
        }

        /// <summary>
        /// Calls the Function for binding GloveType, Size and Location ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GloveInquiry_Load(object sender, EventArgs e)
        {
            btnExportToExcel.Enabled = false;
            dgrvGloveInquiry.AutoGenerateColumns = false;            
        }
        #endregion

        #region Fill Sources
        /// <summary>
        /// Binds the GloveType, Size and Location ComboBox.
        /// </summary>
        private void GetGloveTypeSizeandLocation()
        {
            DropdownDTO obj = new DropdownDTO();
            obj.DisplayField = string.Empty;
            obj.IDField = Constants.ZERO.ToString();
            try
            {
                List<DropdownDTO> lstDrop = CommonBLL.GetGloveType();
                lstDrop.Insert(Constants.ZERO, obj);
                cmbGloveType.BindComboBox(lstDrop, true);

                lstDrop = null;

                lstDrop = CommonBLL.GetSize();
                lstDrop.Insert(Constants.ZERO, obj);
                cmbSize.BindComboBox(lstDrop, true);
                _curDateTime = CommonBLL.GetCurrentDateAndTime();

                cmbLocation.BindComboBox(CommonBLL.GetLocation(), true);
                cmbLocation.DropDownHeight = (cmbLocation.ItemHeight * 10) + 2;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getGloveTypeSizeandLocation", null);     
                return;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Gets the Batch details based on GloveType, Size and location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedIndex >= Constants.ZERO)
            {
                string gloveType = Convert.ToString(cmbGloveType.GetItemText(cmbGloveType.SelectedItem));
                string size = Convert.ToString(cmbSize.GetItemText(cmbSize.SelectedItem));
                try
                {
                    _lstGloveInquiry = GISBLL.GetGloveInquiryDetails(cmbGloveType.GetItemText(cmbGloveType.SelectedItem), cmbSize.GetItemText(cmbSize.SelectedItem), Convert.ToInt32(cmbLocation.SelectedValue));
                    dgrvGloveInquiry.DataSource = _lstGloveInquiry;
                    if (_lstGloveInquiry.Count > Constants.ZERO)
                    {
                        btnExportToExcel.Enabled = true;                      
                    }
                    else
                    {
                        btnExportToExcel.Enabled = false;
                        GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                    if(CommonBLL.ValidateBatchJob(CommonBLL.GetBatchJobLastExecution() , _curDateTime))
                        GetGloveTypeSizeandLocation();
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnGo_Click", cmbGloveType.GetItemText(cmbGloveType.SelectedItem), cmbSize.GetItemText(cmbSize.SelectedItem), Convert.ToInt32(cmbLocation.SelectedValue));     
                    return;
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.SELECT_LOCATION, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Exports the Glove Inquiry Data to excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                CommonBLL.ExportToExcel<GloveInquiryDetails>(GISBLL.GetColumnHeaders(), "Glove Inquiry " + cmbLocation.Text, _lstGloveInquiry);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnExportToExcel_Click", GISBLL.GetColumnHeaders(), "Glove Inquiry " + cmbLocation.Text, _lstGloveInquiry);
                return;
            }
            if(CommonBLL.ValidateBatchJob(CommonBLL.GetBatchJobLastExecution() , _curDateTime))
                GetGloveTypeSizeandLocation();
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
        #endregion
    }
}
