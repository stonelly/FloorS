using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Hartalega.FloorSystem.Windows.UI.QCPackingYieldSystem
{
    /// <summary>
    /// Scan In packing Group Class
    /// </summary>
    public partial class ScanInQCPackingGroup : FormBase
    {
        #region Private Class Members
        private bool _isSelectionChangedWhilePopulate = false;
        private const string _screenName = "ScanInQCPackingGroup";
        private const string _className = "ScanInQCPackingGroup";
        #endregion

        #region Constructors
        public ScanInQCPackingGroup()
        {
            InitializeComponent();
            try
            {
                PopulateFormFields();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }      
        #endregion

        #region EventHandlers

        /// <summary>
        /// Populate Form Fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanInQCPackingGroup_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Populate Members infor based on group and shift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddQCGroupId_SelectedIndexChanged(object sender, EventArgs e)
        {
            QCShiftDTO shiftItem = null;
            int groupId = Constants.ZERO;
            int shiftId = Constants.ZERO;
            string shiftForGroup = string.Empty;
            ddShift.Enabled = true;
            if (!_isSelectionChangedWhilePopulate)
            {
                groupId = Convert.ToInt32(ddQCGroupId.SelectedValue);
                shiftForGroup = QCPackingYieldBLL.GetShiftForGroup(groupId);
                if(!string.IsNullOrEmpty(shiftForGroup))
                {
                    foreach(object item in ddShift.Items)
                    {
                        shiftItem = (QCShiftDTO)item;
                        if(shiftForGroup.Equals(shiftItem.Name))
                        {
                            ddShift.SelectedItem = shiftItem;
                        }
                    }
                }
                else
                {
                    PopulateShiftItems();
                }
                shiftId = Convert.ToInt32(ddShift.SelectedValue);
                try
                {
                    PopulateQCMembersGrid(groupId, shiftId);
                    if(grdQCMembersInfo.Rows.Count>Constants.ZERO)
                    {
                        ddShift.Enabled = false;
                    }
                }
                catch (FloorSystemException fsEX)
                {
                    ExceptionLogging(fsEX, _screenName, _className, ddQCGroupId.Name, null);
                    return;
                }
            }
        }

        /// <summary>
        /// Validate member Id and add to a group if valid
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
                        AddMemberToGroup(memberId);
                    }
                    if(grdQCMembersInfo.Rows.Count>Constants.ZERO)
                    {
                        ddShift.Enabled = false;
                    }
                }
                catch (FloorSystemException fsEX)
                {
                    ExceptionLogging(fsEX, _screenName, _className, ddQCGroupId.Name, null);
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
            List<DropdownDTO> groupList = null;
            groupList = QCPackingYieldBLL.GetQCGroupPG(LocationId);
            _isSelectionChangedWhilePopulate = true;
            ddQCGroupId.DataSource = groupList;
            ddQCGroupId.DisplayMember = "DisplayField";
            ddQCGroupId.ValueMember = "IDField";
            ddQCGroupId.SelectedIndex = Constants.MINUSONE;
            _isSelectionChangedWhilePopulate = false;
            PopulateShiftItems();
            txtMemberCount.Text = string.Empty;
            txtQCMemberId.Text = string.Empty;
            txtQCMemberId.OperatorId();
            grdQCMembersInfo.Rows.Clear();
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        private void PopulateShiftItems()
        {
            List<QCShiftDTO> shiftList = null;
            TimeSpan currentShift = ServerCurrentDateTime.TimeOfDay;
            shiftList = QCPackingYieldBLL.GetQCShiftForPG(Constants.QC_GROUPTYPE);
            ddShift.DataSource = shiftList;
            ddShift.DisplayMember = "Name";
            ddShift.ValueMember = "ShiftId";
            foreach (QCShiftDTO shift in shiftList)
            {
                if (currentShift >= shift.InTime || currentShift < shift.OutTime)
                {
                    ddShift.SelectedItem = shift;
                }
            }
            ddShift.Enabled = true;
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
                 GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR,Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                 GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR,Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            PopulateFormFields();
        }

        /// <summary>
        /// Populate members info
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="shiftId"></param>
        private void PopulateQCMembersGrid(int groupId, int shiftId)
        {
            List<QCMemberDetailsDTO> qcMemberDetailsList = null;
            grdQCMembersInfo.Rows.Clear();
            qcMemberDetailsList = QCPackingYieldBLL.GetQCMemberDetailsForGroupAndShift(groupId, shiftId);
            if (qcMemberDetailsList != null)
            {
                for (int i = Constants.ZERO; i < qcMemberDetailsList.Count; i++)
                {
                    grdQCMembersInfo.Rows.Add();
                    grdQCMembersInfo[Constants.ZERO, i].Value = qcMemberDetailsList[i].MemberId;
                    grdQCMembersInfo[Constants.ONE, i].Value = qcMemberDetailsList[i].Name;
                    grdQCMembersInfo[Constants.TWO, i].Value = qcMemberDetailsList[i].StartTime.ToString(ConfigurationManager.AppSettings["dateFormat"]);
                    txtMemberCount.Text = qcMemberDetailsList.Count.ToString();
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
        /// Add a member to group
        /// </summary>
        /// <param name="memberId"></param>
        private void AddMemberToGroup(string memberId)
        {
            int userNewGroup = Convert.ToInt32(ddQCGroupId.SelectedValue);
            int userOldGroup = Constants.ZERO;
            string groupName = string.Empty;
            bool isEndTimeToBeUpdated = false;
            string userGroup = QCPackingYieldBLL.CheckUserGroup(memberId);
            int noofrowsaffected = Constants.ZERO;
            int shiftId = Convert.ToInt32(ddShift.SelectedValue);
            int subModule = QCPackingYieldBLL.GetScreenIdForScreenName(Constants.QYP_SCAN_IN_QCPACKINGGROUPSUBMODULE);
            string workStationNumber = WorkStationDTO.GetInstance().WorkStationId;
            if (string.IsNullOrEmpty(userGroup))
            {
                isEndTimeToBeUpdated = false;
                noofrowsaffected = QCPackingYieldBLL.SaveQCPackingScanInData(userOldGroup, memberId, userNewGroup, shiftId, subModule, workStationNumber, isEndTimeToBeUpdated);
                if (noofrowsaffected > Constants.ZERO)
                {
                    ClearAndFocusMemberIdField();
                    PopulateQCMembersGrid(userNewGroup, shiftId);
                }
            }
            else
            {
                userOldGroup = Convert.ToInt32(userGroup);
                groupName = QCPackingYieldBLL.GetGroupNameForGroupId(userOldGroup);
                if (userOldGroup == userNewGroup)
                {
                    GlobalMessageBox.Show(Messages.QCMEMBER_ALREADY_IN_THIS_GROUP, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearAndFocusMemberIdField();
                }
                else if (GlobalMessageBox.Show(string.Format(Messages.USER_ALREADY_IN_GROUP, groupName), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo ) == Constants.YES)
                {
                    if (QCPackingYieldBLL.CheckMemberCountInGroup(userOldGroup) != Constants.ONE || QCPackingYieldBLL.CheckBatchEnd(userOldGroup))
                    {
                        isEndTimeToBeUpdated = true;
                        noofrowsaffected = QCPackingYieldBLL.SaveQCPackingScanInData(userOldGroup, memberId, userNewGroup, shiftId, subModule, workStationNumber, isEndTimeToBeUpdated);
                        if (noofrowsaffected > Constants.ZERO)
                        {
                            ClearAndFocusMemberIdField();
                            PopulateQCMembersGrid(userNewGroup, shiftId);
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.QCMEMBER_CANNOT_SWITCH_GROUP, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        ClearAndFocusMemberIdField();
                    }
                }
                else
                {
                    ClearAndFocusMemberIdField();
                }
            }
        }

        /// <summary>
        /// Clear and Focus on MemberId text box
        /// </summary>
        private void ClearAndFocusMemberIdField()
        {
            txtQCMemberId.Clear();
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            txtQCMemberId.Focus();
        }

        /// <summary>
        /// Validate member format and existence
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>true if member is valid</returns>
        private bool ValidateMemberId(string memberId)
        {
            if (QCPackingYieldBLL.CheckValidMember(memberId) > Constants.ZERO)
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
        #endregion
        
    }
}
