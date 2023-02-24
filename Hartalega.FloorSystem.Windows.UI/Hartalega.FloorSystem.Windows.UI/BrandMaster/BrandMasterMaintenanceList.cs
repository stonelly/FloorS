using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Data;
using System.Linq;
using System.Drawing;

namespace Hartalega.FloorSystem.Windows.UI.BrandMaster
{
    public partial class BrandMasterMaintenanceList : FormBase
    {
        Common.ProcessingForm processingForm;

        #region Member Variables
        private string _screenName = "Brand Master - Maintenance";
        private string _className = "Brand Master - Maintenance";
        private string _loggedInUser;
        private BrandMasterDTO _brandMasterDTO = null;
        private List<BrandMasterDTO> brandMasterDTO = null;
        #endregion

        public BrandMasterMaintenanceList(string loggedInUser)
        {
            _loggedInUser = loggedInUser;
            InitializeComponent();
            cmbFilterStatus.BindComboBox(BrandMasterBLL.GetEnumMaster(Constants.STATUS), false);
        }

        private void WorkOrderMaintainanceList_Load(object sender, EventArgs e)
        {
            getBrandMasterMaintenanceDetails();
        }

        private void getBrandMasterMaintenanceDetails()
        {
            dgvLineSelection.Rows.Clear();
            try
            {
                brandMasterDTO = BrandMasterBLL.GetBrandMasterList(txtFilterFGCode.Text, txtFilterBrandName.Text, txtFilterGloveType.Text, Convert.ToInt32(cmbFilterStatus.SelectedValue));                
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBrandMasterMaintenanceDetails", null);
                return;
            }
            if (brandMasterDTO != null)
            {
                for (int i = 0; i < brandMasterDTO.Count; i++)
                {
                    dgvLineSelection.Rows.Add();
                    dgvLineSelection[0, i].Value = dgvLineSelection.Rows.Count;
                    dgvLineSelection[1, i].Value = brandMasterDTO[i].ITEMID;
                    dgvLineSelection[2, i].Value = brandMasterDTO[i].BRANDNAME;
                    dgvLineSelection[3, i].Value = brandMasterDTO[i].GLOVECODE;
                    if (brandMasterDTO[i].ACTIVE == 1)
                        dgvLineSelection[4, i].Value = "Active";
                    if (brandMasterDTO[i].ACTIVE == 0)
                        dgvLineSelection[4, i].Value = "Inactive";
                }
            }
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);

            dgvLineSelection.Rows.Clear();
        }

        private void dgvLineSelection_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _brandMasterDTO = new BrandMasterDTO();
            if (e.RowIndex > Constants.MINUSONE)
            {
                if (dgvLineSelection.Rows.Count > 0 && dgvLineSelection.Rows[e.RowIndex] != null)
                {
                    _brandMasterDTO.ITEMID = Convert.ToString(dgvLineSelection[1, e.RowIndex].Value);
                    _brandMasterDTO.BRANDNAME = Convert.ToString(dgvLineSelection[2, e.RowIndex].Value);
                    _brandMasterDTO.GLOVECODE = Convert.ToString(dgvLineSelection[3, e.RowIndex].Value);
                    if (Convert.ToString(dgvLineSelection[4, e.RowIndex].Value) == "Active")
                        _brandMasterDTO.ACTIVE = 1;
                    if (Convert.ToString(dgvLineSelection[4, e.RowIndex].Value) == "Inactive")
                        _brandMasterDTO.ACTIVE = 0;
                    new BrandMasterMaintenanceEdit(_brandMasterDTO, _loggedInUser).ShowDialog();
                }
            }
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to synchronize Brand Master from Microsoft Dynamic AX?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                if (backgroundWorker1.IsBusy != true)
                {
                    processingForm = new Common.ProcessingForm();
                    processingForm.Show();
                    // Start the asynchronous operation.
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            BrandMasterBLL.SyncBrandMasterFromAX();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            processingForm.Close();
            if (e.Error == null)
            {
                MessageBox.Show("Data has been synchronized successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getBrandMasterMaintenanceDetails();
            }
            else
            {
                if (e.Error.GetType() == typeof(FloorSystemException))
                    ExceptionLogging(e.Error as FloorSystemException, _screenName, _className, "SyncBrandMasterFromAX", null);
                else
                    MessageBox.Show(e.Error.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            getBrandMasterMaintenanceDetails();
        }

        private void dgvLineSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }
    }
}
