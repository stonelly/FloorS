using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;

namespace Hartalega.FloorSystem.Windows.UI.QCPackingYieldSystem
{
    /// <summary>
    /// ScanOutQCPackingGroup
    /// </summary>
    public partial class ScanOutQCPackingGroup : FormBase
    {
        #region Private Class Members
        bool _isSelectionChangedWhilePopulate = true;
        private const string _screenName = "ScanOutQCPackingGroup";
        private const string _className = "ScanOutQCPackingGroup";
        #endregion

        #region Constructors
        public ScanOutQCPackingGroup()
        {
            InitializeComponent();
            int groupId = Constants.ZERO;
            int shiftId = Constants.ZERO;
            try
            {
                PopulateFormFields();
                if (ddQCGroupId.SelectedIndex > Constants.MINUSONE)
                {
                    groupId = Convert.ToInt32(ddQCGroupId.SelectedValue);
                    shiftId = QCPackingYieldBLL.GetShiftIdForShiftName(txtShift.Text);
                    PopulateQCMembersGrid(groupId, shiftId);
                }
            }
            catch (FloorSystemException fsex)
            {
                ExceptionLogging(fsex, _screenName, _className, Name, null);
                return;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// PopulateFormFields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanOutQCPackingGroup_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Populate QCMembers group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddQCGroupId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string shift = string.Empty;
            int groupId = Constants.ZERO;
            int shiftId = Constants.ZERO;
            if (!_isSelectionChangedWhilePopulate)
            {
                groupId = Convert.ToInt32(ddQCGroupId.SelectedValue);
                shift = QCPackingYieldBLL.GetShiftForGroup(groupId);
                shiftId = QCPackingYieldBLL.GetShiftIdForShiftName(shift);
                txtShift.Text = shift;
                try
                {
                    PopulateQCMembersGrid(groupId, shiftId);
                }
                catch (FloorSystemException fsex)
                {
                    ExceptionLogging(fsex, _screenName, _className, Name, null);
                    return;
                }
            }
        }

        /// <summary>
        /// Validate member and if it is valid then remove from the group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQCMemberId_Leave(object sender, EventArgs e)
        {
            string memberId = txtQCMemberId.Text.Trim();
            if (!string.IsNullOrEmpty(memberId))
            {
                try
                {
                    if (ValidateMemberId(memberId))
                    {
                        RemoveMemberFromGroup(memberId);
                    }
                }
                catch (FloorSystemException fsex)
                {
                    ExceptionLogging(fsex, _screenName, _className, Name, null);
                    return;
                }
            }
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Populate form fields
        /// </summary>
        private void PopulateFormFields()
        {
            int LocationId = WorkStationDTO.GetInstance().LocationId;
            int groupId = Constants.ZERO;
            List<DropdownDTO> groupList = null;
            TimeSpan currentShift = ServerCurrentDateTime.TimeOfDay;
            //groupList = QCPackingYieldBLL.GetGroupsForWorkStation(WorkStationDTO.GetInstance().WorkStationId);
            groupList = QCPackingYieldBLL.GetQCGroupPG(LocationId);
            _isSelectionChangedWhilePopulate = true;
            ddQCGroupId.DataSource = groupList;
            ddQCGroupId.DisplayMember = "DisplayField";
            ddQCGroupId.ValueMember = "IDField";
            //ddQCGroupId.SelectedIndex = Constants.MINUSONE;
            txtQCMemberId.OperatorId();
            if (ddQCGroupId.Items.Count > Constants.ZERO)
            {
                ddQCGroupId.SelectedIndex = Constants.MINUSONE;
            }
            else
            {
                ddQCGroupId.SelectedIndex = Constants.MINUSONE;
            }
            _isSelectionChangedWhilePopulate = false;
            txtMemberCount.Text = string.Empty;
            grdQCMembersInfo.Rows.Clear();
            txtQCMemberId.Text = string.Empty;
            if (ddQCGroupId.SelectedIndex > Constants.MINUSONE)
            {
                groupId = Convert.ToInt32(ddQCGroupId.SelectedValue);
                txtShift.Text = QCPackingYieldBLL.GetShiftForGroup(groupId);
            }
        }

        /// <summary>
        /// To call exception log method
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="UiClassName">UIClassName</param>
        /// <param name="uiControl">UIControl</param>
        /// <param name="parameters">Parameters</param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string uiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, uiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            PopulateFormFields();
        }

        /// <summary>
        /// Populate QC Members Grid
        /// </summary>
        /// <param name="groupId">groupId</param>
        /// <param name="shiftId">shiftId</param>
        private void PopulateQCMembersGrid(int groupId, int shiftId)
        {
            List<QCMemberDetailsDTO> qcMemberDetailsList = null;
            grdQCMembersInfo.Rows.Clear();
            qcMemberDetailsList = QCPackingYieldBLL.GetQCMemberDetailsForGroupAndShiftForScanOut(groupId, shiftId);
            if (qcMemberDetailsList != null)
            {
                for (int i = Constants.ZERO; i < qcMemberDetailsList.Count; i++)
                {
                    grdQCMembersInfo.Rows.Add();
                    grdQCMembersInfo[Constants.ZERO, i].Value = qcMemberDetailsList[i].MemberId;
                    grdQCMembersInfo[Constants.ONE, i].Value = qcMemberDetailsList[i].Name;
                    grdQCMembersInfo[Constants.TWO, i].Value = qcMemberDetailsList[i].StartTime.ToString(ConfigurationManager.AppSettings["dateFormat"]);
                    if (qcMemberDetailsList[i].EndTime == default(DateTime))
                    {
                        grdQCMembersInfo[Constants.THREE, i].Value = Constants.END_TIME_NOT_AVAILABLE;
                    }
                    else
                    {
                        grdQCMembersInfo[Constants.THREE, i].Value = qcMemberDetailsList[i].EndTime.ToString(ConfigurationManager.AppSettings["dateFormat"]);
                    }
                    txtMemberCount.Text =Convert.ToString(qcMemberDetailsList.Count);
                }
                grdQCMembersInfo.ClearSelection();
            }
            else
            {
                grdQCMembersInfo.Rows.Clear();
                txtMemberCount.Text = Constants.ZERO.ToString();
            }
        }

        /// <summary>
        /// Remove a member from group
        /// </summary>
        /// <param name="memberId">memberId</param>
        private void RemoveMemberFromGroup(string memberId)
        {
            int noofrowsaffected = Constants.ZERO;
            int actualUserGroup = Constants.ZERO;
            int selectedUserGroup = Convert.ToInt32(ddQCGroupId.SelectedValue);
            string userGroup = QCPackingYieldBLL.CheckUserGroup(memberId);
            int subModule =QCPackingYieldBLL.GetScreenIdForScreenName(Constants.QYP_SCAN_OUT_QCPACKINGGROUPSUBMODULE);
            int shiftId = QCPackingYieldBLL.GetShiftIdForShiftName(txtShift.Text);
            if (string.IsNullOrEmpty(userGroup))
            {
                GlobalMessageBox.Show(Messages.QCMEMBER_NOT_IN_ANY_GROUP, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                ClearAndFocusMemberIdField();
            }
            else
            {
                actualUserGroup = Convert.ToInt32(userGroup);
                if (actualUserGroup != selectedUserGroup)
                {
                    GlobalMessageBox.Show(Messages.QCMEMBER_NOT_IN_THIS_GROUP, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearAndFocusMemberIdField();
                }
                else
                {
                    if (QCPackingYieldBLL.CheckMemberCountInGroup(actualUserGroup) != Constants.ONE || QCPackingYieldBLL.CheckBatchEnd(actualUserGroup))
                    {
                        noofrowsaffected = QCPackingYieldBLL.SaveQCPackingScanOutData(selectedUserGroup, memberId, subModule);
                        if (noofrowsaffected > Constants.ZERO)
                        {
                            PopulateQCMembersGrid(selectedUserGroup, shiftId);
                            ClearAndFocusMemberIdField();
                            if (grdQCMembersInfo.Rows.Count == Constants.ZERO)
                            {
                                Close();
                            }
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.QCMEMBER_CANNOT_SCAN_OUT, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearAndFocusMemberIdField();
                    }
                }
            }
        }

        /// <summary>
        /// Validate a member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>true if valid member else return false</returns>
        private bool ValidateMemberId(string memberId)
        {
            if ( QCPackingYieldBLL.CheckValidMember(memberId) > Constants.ZERO)
            {
                return true;
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_MEMBER_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearAndFocusMemberIdField();
                return false;
            }
        }

        /// <summary>
        /// Clear and focus memberId textbox
        /// </summary>
        private void ClearAndFocusMemberIdField()
        {
            txtQCMemberId.Clear();
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            txtQCMemberId.Focus();
        }
        #endregion
    }

}
