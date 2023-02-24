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
    public partial class BrandMasterPreshipmentList : FormBase
    {
        #region Member Variables
        private string _screenName = "Brand Master - Preshipment";
        private string _className = "Brand Master - Preshipment";
        private string _loggedInUser;
        private BrandMasterDTO _brandMasterDTO = null;
        private List<BrandMasterDTO> brandMasterDTOList = null;
        #endregion

        #region Constructor

        public BrandMasterPreshipmentList(string loggedInUser)
        {
            _loggedInUser = loggedInUser;
            InitializeComponent();
            cmbFilterStatus.BindComboBox(BrandMasterBLL.GetEnumMaster("Status"), false);
        }

        #endregion

        #region Event Handler
        
        private void BrandMasterPreshipmentList_Load(object sender, EventArgs e)
        {
            getList();
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
                    new BrandMasterPreshipmentEdit(_brandMasterDTO, _loggedInUser).ShowDialog();
                    getList();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            getList();
        }

        private void dgvLineSelection_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }

        #endregion

        #region User Methods
        private void getList()
        {
            dgvLineSelection.Rows.Clear();
            try
            {
                brandMasterDTOList = BrandMasterBLL.GetBrandMasterList(txtFilterFGCode.Text, txtFilterBrandName.Text, txtFilterGloveType.Text, Convert.ToInt32(cmbFilterStatus.SelectedValue));
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getList", null);
                return;
            }
            if (brandMasterDTOList != null)
            {
                for (int i = 0; i < brandMasterDTOList.Count; i++)
                {
                    dgvLineSelection.Rows.Add();
                    dgvLineSelection[0, i].Value = dgvLineSelection.Rows.Count;
                    dgvLineSelection[1, i].Value = brandMasterDTOList[i].ITEMID;
                    dgvLineSelection[2, i].Value = brandMasterDTOList[i].BRANDNAME;
                    dgvLineSelection[3, i].Value = brandMasterDTOList[i].GLOVECODE;
                    if (brandMasterDTOList[i].ACTIVE == 1)
                        dgvLineSelection[4, i].Value = "Active";
                    if (brandMasterDTOList[i].ACTIVE == 0)
                        dgvLineSelection[4, i].Value = "Inactive";
                    dgvLineSelection[5, i].Value = brandMasterDTOList[i].PRESHIPMENTPLAN;
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



        #endregion
    }
}
