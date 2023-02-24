using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Hartalega.FloorSystem.Windows.UI.QCEfficiencyData
{
    public partial class EditQCEfficiency : FormBase
    {
        #region Member Variables
        private static string _screenName = "QC packing & Yielding - Edit QC Efficiency";
        private static string _className = "EditQCEfficiency";
        private string _loggedInUser;
        private string[] _moduleId;
        private string LoggedInUserPassword;

        private static string _logScreenName = "Configuration SetUp - EditQCEfficiency";
        #endregion

        //USP_SEL_QCEfficiency
        public EditQCEfficiency(string loggedInUser, string mode)
        {
            InitializeComponent();
            _loggedInUser = Convert.ToString(loggedInUser);
            try
            {
                LoggedInUserPassword = SetUpConfigurationBLL.GetModuleIdForLoggedInUserPassword(Convert.ToString(loggedInUser));
                string moduleId = SetUpConfigurationBLL.GetModuleIdForLoggedInUser(_loggedInUser);
                _moduleId = moduleId.Trim(' ', ',').Split(new char[] { ',' });
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "EditQCEfficiency", null);
                return;
            }

            if (string.IsNullOrEmpty(mode))
            {
                dtpDate.Enabled = false;
                dtpStartTime.Enabled = false;
                dtpEndTime.Enabled = false;
            }
        }

        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtSerialNo, "Serial Number", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cbPackingSize, "Packing Size", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(tbInnerBoxCount, "Inner Box Count", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cbQcType, "QC Type", ValidationType.Required));
            return ValidateForm();
        }

        private void tbSerialNo_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSerialNo.Text.Trim()))
            {
                if (txtSerialNo.Text.Length == Constants.SERIAL_LENGTH && CommonBLL.ValidateBatch(Convert.ToDecimal(txtSerialNo.Text)))
                {
                    try
                    {
                        //Added by Tan Wei Wah 20190312
                        if (CommonBLL.GetDowngradeStatusFromDB(txtSerialNo.Text.Trim()))
                        {
                            GlobalMessageBox.Show(Messages.DOWNGRADE_VALIDATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            return;
                        }
                        //Ended by Tan Wei Wah 20190312

                        ShowQcEfficiencyDetails();
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "txtSerialNo_Leave", txtSerialNo.Text);
                        return;
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_WASHER_SC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    //ClearForm();
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.SERIAL_NUMBER_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                //ClearForm();
            }

        }

        private void ShowQcEfficiencyDetails()
        {
            int index = 0;
            List<EditQCEfficiencyDTO> objQcEfficiency = QcEfficiencyBll.GetQCEfficiencyDetails(txtSerialNo.Text);
            if (objQcEfficiency.Count > 0)
            {
                int count = 0, i = 0;
                foreach (EditQCEfficiencyDTO objqc in objQcEfficiency)
                {
                    if (objqc.Rework > count)
                    { count = objqc.Rework; index = i; }
                    i++;
                }
            }
            //index = objQcEfficiency.Count;
            if (objQcEfficiency != null)
            {
                //string qaiStatus = CommonBLL.ValidateSerialNoByQAIStatus(Convert.ToDecimal(txtSerialNo.Text.Trim()));
                //if (!string.IsNullOrEmpty(qaiStatus))
                //{
                //    GlobalMessageBox.Show(qaiStatus, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                //    ClearForm();
                //}
                //{
                string groupList = objQcEfficiency[index].QCGroupMembers.ToString();
                List<QCGroupMembersDTO> objGroupMembers = QcEfficiencyBll.GetGroupMembers(groupList);

                //KahHeng 16072019 Prompt Window to inform user that this serial number havent QA scan out yet
                if (string.IsNullOrEmpty(objQcEfficiency[index].EndTime.ToString()) == true || objQcEfficiency[index].EndTime.ToString() == "01/01/1900 00:00:00" || string.IsNullOrEmpty(objQcEfficiency[index].BatchStatus.ToString()) == true)
                {
                    GlobalMessageBox.Show(Messages.QCEFFICIENCY_INVALID_BATCHENDTIME, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtSerialNo.Clear();
                    txtSerialNo.Focus();
                }
                //End KahHeng 16072019

                else
                {
                    cbQcType.DataSource = GloveCodeBLL.GetQCTypeByGloveCode();
                    BindPackingSize();
                    BindBatchStatus(); //Added by Tan Wei Wah 20190125
                    BindReason(); //Added by Tan Wei Wah 20190130
                    gridEmployee.Rows.Clear();
                    if (objGroupMembers != null)
                    {
                        for (int i = Constants.ZERO; i < objGroupMembers.Count; i++)
                        {
                            gridEmployee.Rows.Add();
                            gridEmployee[Constants.ZERO, i].Value = objGroupMembers[i].EmployeeId;
                            gridEmployee[Constants.ONE, i].Value = objGroupMembers[i].EmployeeName;
                        }
                        gridEmployee.ClearSelection();
                    }
                    tbId.Text = objQcEfficiency[index].Id.ToString();
                    tbBatchNo.Text = objQcEfficiency[index].BatchNo.ToString();
                    tbBatchWeight.Text = objQcEfficiency[index].BatchWeight.ToString();
                    tbBrand.Text = objQcEfficiency[index].Brand.ToString();
                    tbBatchStatus.Text = objQcEfficiency[index].BatchStatus.ToString();
                    cbBatchStatus.SelectedText = objQcEfficiency[index].BatchStatus.ToString();
                    tbGroup.Text = objQcEfficiency[index].Group.ToString();
                    dtpStartTime.Text = objQcEfficiency[index].StartTime.ToString();
                    dtpEndTime.Text = objQcEfficiency[index].EndTime.ToString();
                    tbNoPerson.Text = objQcEfficiency[index].NoPerson.ToString();
                    tbReworkReason.Text = objQcEfficiency[index].ReworkReason.ToString();
                    cbPackingSize.Text = objQcEfficiency[index].PackingSize.ToString();
                    tbInnerBoxCount.Text = objQcEfficiency[index].InnerBoxCount.ToString();
                    cbRework.Checked = (objQcEfficiency[index].Rework >= 1) ? true : false;
                    dtpDate.Text = objQcEfficiency[index].Date.ToString();
                    cbQcType.Text = objQcEfficiency[index].QCType.ToString();
                    tbGlove.Text = objQcEfficiency[index].Glove.ToString();
                    tbTenPcsWeight.Text = objQcEfficiency[index].TenPcsWeight.ToString(); //Added by Tan Wei Wah 20190130
                    cbReason.SelectedValue = objQcEfficiency[index].Reason; //Added by Tan Wei Wah 20190130
                                                                            //}
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.INVALID_SERIAL_NUMBER_QC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                //ClearForm();
            }
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (uiControl == "getBatchWeight")
                GlobalMessageBox.Show("(" + result + ") - " + Constants.BATCH_WEIGHT_ERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            //ClearForm();
        }



        private void BindPackingSize()
        {
            try
            {
                cbPackingSize.BindComboBox(CommonBLL.GetEnumText(Constants.PACKING_SIZE), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindPackingSize", null);
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;

    //KahHeng 16072019 To get the BatchTargetTime
    List<EditQCEfficiencyDTO> objQcEfficiencyCompare = QcEfficiencyBll.GetQCEfficiencyDetails(txtSerialNo.Text);
    bool isBatchTargetTimeNull = false;
    //End KahHeng 16072019

    if (ValidateRequiredFields())
            {
                EditQCEfficiencyDTO objQcEfficiency = new EditQCEfficiencyDTO();
                try
                {
                    //KahHeng 4/6/2019 add 
                    //To get the totalPCs of the serial number and show error message if the InnerBoxCount * PackingSize
                    if (cbReason.Text == null || cbReason.Text == "" || cbReason.Text.Trim() == "")
                    {
                        GlobalMessageBox.Show(Messages.EMPTY_REASON, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        cbReason.Focus();
                        return;
                    }

                    //KahHeng 16072019
                    if (cbBatchStatus.Text == null || cbBatchStatus.Text == "" || cbBatchStatus.Text.Trim() == "")
                    {
                        GlobalMessageBox.Show(Messages.EMPTY_BATCH_STATUS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        cbReason.Focus();
                        return;
                    }

                    decimal totalPcs = QcEfficiencyBll.GetLatestTotalPcs(Convert.ToDecimal(txtSerialNo.Text));

                    if ((Convert.ToInt32(tbInnerBoxCount.Text.Trim()) * Convert.ToInt32(cbPackingSize.Text)) > totalPcs)
                    {
                        GlobalMessageBox.Show(Messages.INCORRECT_TOTAL_PCS, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        ShowQcEfficiencyDetails();
                        cbPackingSize.Focus();
                        return;
                    }

                    if (String.IsNullOrEmpty(objQcEfficiencyCompare[0].Date.ToString()) == true)
                    {
                        isBatchTargetTimeNull = true;
                    }
                    //END KahHeng 4/6/2019

                    objQcEfficiency.Id = int.Parse(tbId.Text);
                    //objQcEfficiency.BatchNo = tbBatchNo.Text;
                    //objQcEfficiency.BatchWeight = decimal.Parse(tbBatchWeight.Text);
                    //objQcEfficiency.BatchStatus = tbBatchStatus.Text;
                    //objQcEfficiency.Group = int.Parse(tbGroup.Text);
                    //string  dateFormat = ConfigurationManager.AppSettings["dateFormat"];
                    //  txtStartTime.Text = _dsbc.LastModifiedOn.ToString(dateFormat);


                    objQcEfficiency.StartTime = dtpStartTime.Text;
                    objQcEfficiency.EndTime = dtpEndTime.Text;
                    objQcEfficiency.Date = dtpDate.Text;
                    //objQcEfficiency.StartTime = Convert.ToDateTime(dtpStartTime.Text).ToString(ConfigurationManager.AppSettings["dateFormat"]);
                    //objQcEfficiency.EndTime = Convert.ToDateTime(dtpEndTime.Text).ToString(ConfigurationManager.AppSettings["dateFormat"]);
                    //objQcEfficiency.Date = Convert.ToDateTime(dtpDate.Text).ToString(ConfigurationManager.AppSettings["dateFormat"]);

                    //objQcEfficiency.NoPerson = int.Parse(tbNoPerson.Text);
                    objQcEfficiency.QCType = cbQcType.Text;
                    objQcEfficiency.PackingSize = int.Parse(cbPackingSize.Text);
                    objQcEfficiency.InnerBoxCount = int.Parse(tbInnerBoxCount.Text);

                    objQcEfficiency.BatchStatus = cbBatchStatus.Text;  //Added by Tan Wei Wah 20190125
                    objQcEfficiency.SerialNo = txtSerialNo.Text; //Added by Tan Wei Wah 20190131
                    objQcEfficiency.BatchWeight = decimal.Parse(tbBatchWeight.Text); //Added by Tan Wei Wah 20190131
                    objQcEfficiency.Reason = Convert.ToString(cbReason.SelectedValue);//Added by Tan Wei Wah 20190131
                    //int val = GloveCodeBLL.isGloveAddDuplicate(objGloveCode);
                    //isDuplicate = Convert.ToBoolean(val);

                    if (!isDuplicate)
                    {
                        rowsReturned = QcEfficiencyBll.EditQCTypeForm(objQcEfficiency, _loggedInUser, isBatchTargetTimeNull); //Added by Tan Wei Wah 20190227 - add _loggedInUser
                        if (rowsReturned > 0)
                        {
                            //event log myadamas 20190227
                            EventLogDTO EventLog = new EventLogDTO();

                            EventLog.CreatedBy = _loggedInUser;
                            Constants.EventLog audAction = Constants.EventLog.Save;
                            EventLog.EventType = Convert.ToInt32(audAction);
                            EventLog.EventLogType = Constants.eventlogtype;

                            var screenid = CommonBLL.GetScreenIdByScreenName(_logScreenName);
                            CommonBLL.InsertEventLog(EventLog, _logScreenName, screenid.ToString());


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
                        //Messages.DUPLICATE_DRYER_VALUES
                        GlobalMessageBox.Show("DUPLICATE QC Efficiency VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        //tbGloveCode.Text = string.Empty;
                        //tbGloveCode.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        private void EditQCEfficiency_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// BindBatchStatus
        /// </summary>
        /// Add by Tan Wei Wah 20190125
        private void BindBatchStatus()
        {
            try
            {
                cbBatchStatus.BindComboBox(CommonBLL.GetEnumText(Constants.BATCH_STATUS), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindBatchStatus", null);
                return;
            }
        }

        /// <summary>
        /// Bind the Reason to select
        /// </summary>
        /// Added by Tan Wei Wah 20190130
        private void BindReason()
        {
            try
            {
                cbReason.BindComboBox(CommonBLL.GetReasons(Constants.QCPY_REASON_TYPE, Convert.ToString(Convert.ToInt16((Constants.Modules.QCYIELDANDPACKING)))), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindReason", null);
                return;
            }
        }

        /// <summary>
        /// Recount the batch weight
        /// </summary>
        /// Added by Tan Wei Wah 20190130
        private void RecountBatchWeight()
        {
            try
            {
                if (tbTenPcsWeight.Text != "")
                {
                    decimal _ten_pcs_weight = decimal.Parse(tbTenPcsWeight.Text);
                    int _packing_size = int.Parse(cbPackingSize.Text);
                    int _inner_box_count = int.Parse(tbInnerBoxCount.Text);
                    decimal _batch_weight = 0;

                    _batch_weight = (_ten_pcs_weight * (_packing_size * _inner_box_count)) / 10000;

                    tbBatchWeight.Text = _batch_weight.ToString();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "RecountBatchWeight", null);
                return;
            }
        }

        /// <summary>
        /// Added by Tan Wei Wah 20190130
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPackingSize_Leave(object sender, EventArgs e)
        {
            RecountBatchWeight();
        }

        /// <summary>
        /// Added by Tan Wei Wah 20190130
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbInnerBoxCount_Leave(object sender, EventArgs e)
        {
            RecountBatchWeight();
        }
    }
}
