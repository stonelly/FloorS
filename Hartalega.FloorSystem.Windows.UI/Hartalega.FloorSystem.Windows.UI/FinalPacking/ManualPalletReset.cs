using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Common;
using System.Drawing;
using System.Text;
using System.IO;
namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    /// <summary>
    /// Module: Final Packing
    /// Screen Name: ManualPalletReset
    /// File Type: Code file
    /// </summary>
    public partial class ManualPalletReset : FormBase
    {
        #region Member Variables
        private string _screenName = "Manual Pallet Reset";
        private string _className = "ManualPalletReset";
        private List<string> _lstPalletId;
        #endregion
        /// <summary>
        /// Instantiate the component
        /// </summary>
        public ManualPalletReset()
        {
            InitializeComponent();
        }

        #region User Methods

        /// <summary>
        /// Clear the values of all the controls 
        /// </summary>
        private void ClearForm()
        {
            txtPalletId.Text = string.Empty;
            lstCompletedPallets.Items.Clear();
            lblPalletCount.Text = Convert.ToString(Constants.ZERO);
            txtPalletId.Focus();
            FetchCompletedPalletList();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearForm", null);
                return;
            }
        }

        /// <summary>
        /// Fetch the list of all completed pallets 
        /// </summary>
        private void FetchCompletedPalletList()
        {
            AutoCompleteStringCollection allowedPallets = new AutoCompleteStringCollection();
            try
            {
                _lstPalletId = FinalPackingBLL.GetCompletedPalletIdList();
                allowedPallets.AddRange(_lstPalletId.ToArray());
                txtPalletId.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtPalletId.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtPalletId.AutoCompleteCustomSource = allowedPallets;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "FetchCompletedPalletList", null);
                return;
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
            else if (floorException.subSystem == Constants.SERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.SERVICE_UNAVAILABLE, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);

            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender">Manual Pallet Reset Page</param>
        /// <param name="e">On load event argument</param>
        private void ManualPalletReset_Load(object sender, EventArgs e)
        {
            txtLocation.Text = Convert.ToString(WorkStationDTO.GetInstance().LocationId);
            FetchCompletedPalletList();
        }
        /// <summary>
        /// Add image button click event
        /// </summary>
        /// <param name="sender">Manual Pallet Reset Page</param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPalletId.Text.Trim()))
            {
                if (!lstCompletedPallets.Items.Contains(txtPalletId.Text.Trim().ToUpper()))
                {
                    if (_lstPalletId.Contains(txtPalletId.Text.Trim().ToUpper()))
                    {
                        lstCompletedPallets.Items.Add(txtPalletId.Text.Trim());
                        lblPalletCount.Text = Convert.ToString(lstCompletedPallets.Items.Count);
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NOT_COMPLETED_PALLET, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.DUPLICATE_PALLET, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
                sb.AppendLine(Messages.REQPALLETREQUIRED);
                GlobalMessageBox.Show(sb.ToString(), Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            }
            txtPalletId.Text = string.Empty;
            txtPalletId.Focus();
        }
        /// <summary>
        /// Remove image button click event
        /// </summary>
        /// <param name="sender">Manual Pallet Reset Page</param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            for (int i = lstCompletedPallets.SelectedItems.Count - 1; i >= 0; i--)
            {
                lstCompletedPallets.Items.Remove(lstCompletedPallets.SelectedItems[i]);
            }
            lblPalletCount.Text = Convert.ToString(lstCompletedPallets.Items.Count);
        }
        /// <summary>
        /// OK button click event
        /// </summary>
        /// <param name="sender">Manual Pallet Reset Page</param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            StringBuilder unsuccessPalletList = new StringBuilder(Environment.NewLine);
            StringBuilder successPalletList = new StringBuilder(Environment.NewLine);
            string path, backupPath;

            if (lstCompletedPallets.Items.Count > Constants.ZERO)
            {
                if (GlobalMessageBox.Show(Messages.PALLET_RESET_CONFIRM, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    try
                    {
                        for (int i = 0; i < lstCompletedPallets.Items.Count; i++)
                        {
                            string palletId = lstCompletedPallets.Items[i].ToString();

                            //add by Cheah (24Mar2017)
                            //string path = FloorSystemConfiguration.GetInstance().strEWareNaviFolderLocation + palletId.Substring(4, 4) + ".txt";
                            //string backupPath = FloorSystemConfiguration.GetInstance().strEWareNaviBackupLocation + palletId.Substring(4, 4) + ".txt";

                            // Commented by yiksiu for 8digits pallet id implementation
                            //if (palletId.Substring(0, 4) == "000F")
                            //{
                            //    path = FloorSystemConfiguration.GetInstance().strEWareNaviFolderLocation + palletId.Substring(4, 4) + ".txt";
                            //    backupPath = FloorSystemConfiguration.GetInstance().strEWareNaviBackupLocation + palletId.Substring(4, 4) + ".txt";
                            //}
                            //else
                            //{
                                //path = FloorSystemConfiguration.GetInstance().strEWareNaviFolderLocation + palletId + ".txt";
                                //backupPath = FloorSystemConfiguration.GetInstance().strEWareNaviBackupLocation + palletId + ".txt";
                            //}

                            //end add

                            //if (File.Exists(path))
                            //{
                            //    File.Copy(path, backupPath, true);
                            //    File.Delete(path);
                            //    if (File.Exists(backupPath))
                            //    {
                            //        FinalPackingBLL.ManualPalletResetLog(palletId);
                            //        successPalletList.AppendLine(palletId);
                            //    }
                            //}
                            if (FinalPackingBLL.ManualPalletResetPalletFlag(palletId))
                            {

                                FinalPackingBLL.ManualPalletResetLog(palletId);
                                successPalletList.AppendLine(palletId);
                            }
                            else
                            {
                                unsuccessPalletList.AppendLine(palletId);
                            }

                        }
                        if (!string.IsNullOrEmpty(unsuccessPalletList.ToString().Trim()))
                        {
                            GlobalMessageBox.Show(string.Format(Messages.PALLET_RESET_UNSUCCESS, unsuccessPalletList), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                        if (!string.IsNullOrEmpty(successPalletList.ToString().Trim()))
                        {
                            GlobalMessageBox.Show(string.Format(Messages.PALLET_RESET_SUCCESS, successPalletList), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder(Messages.REQUIREDFIELDMESSAGE);
                sb.AppendLine(Messages.REQCOMPPALLETLIST);
                GlobalMessageBox.Show(sb.ToString(), Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            }
            ClearForm();
        }
        /// <summary>
        /// Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
            }
        }
        #endregion

    }
}
