using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class PalletSelection : Form
    {

        # region PROPERTIES
        public string childPalletId
        {
            set;
            get;
        }
        public string SelectedPalletID
        {
            set;
            get;
        }
        public bool IsCancel
        {
            set;
            get;
        } = true;
        public Boolean isPreshipment
        { set; get; }
        public PalletSelection()
        {
            InitializeComponent();
        }
        #endregion
               
        /// <summary>
        /// FORM LOAD TO LOAD AVAILABLE PALLET ID'S
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletSelection_Load(object sender, EventArgs e)
        {
            List<DropdownDTO> lstPallet = new List<DropdownDTO>();
            lblMessage.Text = string.Format(Messages.PALLETMAXIMUMCAPACITYREACHED, SelectedPalletID);
            if (isPreshipment == true)
            {
                lstPallet = FinalPackingBLL.GetPreshipmentPalletIdList();
                groupBox1.Text = Messages.SEL_PRESHIP_PALLID;
            }
            else
            {
                lstPallet = FinalPackingBLL.GetPalletIdList();
                groupBox1.Text = Messages.SEL_PALLID;
            }

            cmbPallet.Items.Clear();
            cmbPallet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPallet.AutoCompleteSource = AutoCompleteSource.CustomSource;
            if (lstPallet != null)
            foreach (DropdownDTO plt in lstPallet)
            {
                cmbPallet.Items.Add(plt.DisplayField);
                cmbPallet.AutoCompleteCustomSource.Add(plt.DisplayField);
            }
        }

        /// <summary>
        /// Click OK to reurn selected Pallet ID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbPallet.Text))
            {
                IsCancel = false;
                childPalletId = Convert.ToString(cmbPallet.Text);
                this.Close();
            }
            else
            {               
                GlobalMessageBox.Show(Messages.SELECTPALLETID, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
            }

        }

        private void cmbPallet_Leave(object sender, EventArgs e)
        {
            if (!cmbPallet.Items.Contains(cmbPallet.Text.ToUpper()) && cmbPallet.Text != String.Empty)
            {
                cmbPallet.Text = String.Empty;
                cmbPallet.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            this.Close();
        }
    }
}
