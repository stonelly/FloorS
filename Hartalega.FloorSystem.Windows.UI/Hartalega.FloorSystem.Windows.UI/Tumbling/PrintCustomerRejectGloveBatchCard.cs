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
using System.Drawing;

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    public partial class PrintCustomerRejectGloveBatchCard : FormBase
    {
        #region Static Variables
        private static string _screenName = "Tumbling - PrintCustomerRejectBatchCard";
        private static string _className = "PrintCustomerRejectGloveBatchCard";
        private static List<CustomerRejectDTO> _customerRejectList = new List<CustomerRejectDTO>();
        private static CustomerRejectDTO _customerReject = new CustomerRejectDTO();
        private static int _CaseCapacityCnt = 0;
        #endregion

        #region Load Page
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintCustomerRejectGloveBatchCard" /> class.
        /// </summary>
        public PrintCustomerRejectGloveBatchCard()
        {
            InitializeComponent();
        }
        #endregion

        #region Load Form
        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender">Request object</param>
        /// <param name="e">Event argument</param>
        private void PrintCustomerRejectGloveBatchCard_Load(object sender, EventArgs e)
        {
            txtOperatorId.Focus();
            _customerRejectList.Clear();
            try
            {
                cmbChangeQCType.BindComboBox(CommonBLL.GetQCType(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintCustomerRejectGloveBatchCard_Load", null);
                return;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// This method validates the operatorId entered and gets the operator name if valid
        /// </summary>
        /// <param name="sender">Operator id text box</param>
        /// <param name="e">On leave event argument</param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            string operatorId = txtOperatorId.Text.Trim();
            if (operatorId != String.Empty)
            {
                try
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(operatorId, _screenName))
                    {
                        lblOperatorName.Text = CommonBLL.GetOperatorNameQAI(operatorId);
                        _customerReject.OperatorId = txtOperatorId.Text;
                        if (lblOperatorName.Text == String.Empty)
                        {
                            GlobalMessageBox.Show(Messages.INVALID_OPERATOR_ID, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Text = String.Empty;
                            lblOperatorName.Text = String.Empty;
                            txtOperatorId.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        txtOperatorId.Text = String.Empty;
                        lblOperatorName.Text = String.Empty;
                        txtOperatorId.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "txtOperatorId_Leave", txtOperatorId.Text);
                    return;
                }
            }
            else
                lblOperatorName.Text = String.Empty;
        }

        /// <summary>
        /// Clears the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearLotFields();
            }
        }

        private void cmbChangeQCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQCDescription.Text = Convert.ToString(cmbChangeQCType.SelectedValue);
        }

        /// <summary>
        /// Validates and Add Lot Number to lot list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLotNo.Text != String.Empty)
                {
                    _customerReject = TumblingBLL.GetLotDetails(txtLotNo.Text);
                    if (_customerReject != null)
                    {
                        int CustLotCount = TumblingBLL.GetCustomerTableLotCount(txtLotNo.Text);
                        int ListViewCnt = 1;
                        if (listViewLotNumber.Items.Count != 0)
                        {
                            for (int i = 0; i < listViewLotNumber.Items.Count; i++)
                            {
                                if (listViewLotNumber.Items[i].Text == txtLotNo.Text)
                                    ListViewCnt = ListViewCnt + 1;
                            }
                        }
                        if (CustLotCount + ListViewCnt >  _customerReject.CasesPacked)
                        {
                            GlobalMessageBox.Show(Messages.EXCEED_CASESPACKED_LIMIT, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                        else
                        {
                            if (_customerRejectList != null && _customerRejectList.Count > 0 && _customerRejectList.Count != 10)
                            {
                                if (_customerReject.CustomerName == _customerRejectList[_customerRejectList.Count - 1].CustomerName && _customerReject.GloveType == _customerRejectList[_customerRejectList.Count - 1].GloveType && _customerReject.Size == _customerRejectList[_customerRejectList.Count - 1].Size && _customerReject.FGCode == _customerRejectList[_customerRejectList.Count - 1].FGCode)
                                {
                                    _customerReject.OperatorId = txtOperatorId.Text;
                                    _customerReject.InternalLotNumber = txtLotNo.Text;
                                    _customerRejectList.Add(_customerReject);
                                    _CaseCapacityCnt = _CaseCapacityCnt + _customerReject.CaseCapacity;
                                    txtTotalQuantity.Text = _CaseCapacityCnt.ToString("#,##0.00");
                                    listViewLotNumber.Items.Add(txtLotNo.Text);
                                }
                                else
                                {
                                    GlobalMessageBox.Show(Messages.INVALID_CUSTOMER_BATCH, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                                    txtLotNo.Text = String.Empty;
                                }
                            }
                            else if (_customerRejectList.Count == 0)
                            {
                                _customerRejectList.Add(_customerReject);
                                listViewLotNumber.Items.Add(txtLotNo.Text);
                                _customerReject.OperatorId = txtOperatorId.Text;
                                _customerReject.InternalLotNumber = txtLotNo.Text;
                                txtCustomer.Text = _customerReject.CustomerName;
                                txtGloveCode.Text = _customerReject.GloveType;
                                txtDescription.Text = _customerReject.Description;
                                _CaseCapacityCnt = _customerReject.CaseCapacity;
                                txtTotalQuantity.Text = _CaseCapacityCnt.ToString("#,##0.00");
                                txtShift.Text = _customerReject.ShiftName;
                                txtLine.Text = _customerReject.Line;
                                txtSize.Text = _customerReject.Size;
                                txtTenPcsWeight.Text = _customerReject.TenPcsWeight.ToString("#,##0.00");
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.LOTNUMBERLIMITEXCEEDS, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                                txtLotNo.Text = String.Empty;
                            }
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.INVALIDINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtLotNo.Text = String.Empty;
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "txtLotNo_TextChanged", txtLotNo.Text);
            }
        }

        /// <summary>
        /// To Save and Print batch Card details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRequiredFields())
                {
                    CustomerRejectDTO _resultDTO = null;
                    DateTime batchDateTime = ServerCurrentDateTime;
                    _resultDTO = TumblingBLL.SaveCustomerRejectGloves(_customerRejectList, cmbChangeQCType.Text, FloorSystemConfiguration.GetInstance().intSiteNumber);

                    BatchDTO objBatchDTO = new BatchDTO();
                    objBatchDTO = AXPostingBLL.GetCompleteCustomerRejectGlovesDetails(Convert.ToDecimal(_resultDTO.SerialNumber));
                    Boolean isPostingSuccess = false;
                    objBatchDTO.QCGroupMembers = _customerReject.FGCode;  //took FGItem in QCGroupMembers
                    objBatchDTO.QCGroupName = _customerReject.FGSize;   // took FGSize in QCGroupName
                    try
                    {
                        isPostingSuccess = AXPostingBLL.PostAXDataCustomerRejectGloves(objBatchDTO);
                        if (!isPostingSuccess)
                        {
                            TumblingBLL.DelCustomerRejectEntry(Convert.ToDecimal(_resultDTO.SerialNumber));
                            GlobalMessageBox.Show(Messages.DATA_SAVED_UNSUCCESSFUL_QAI, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_SM, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            //event log myadamas 20190227
                            EventLogDTO EventLog = new EventLogDTO();

                            EventLog.CreatedBy = Convert.ToString(txtOperatorId.Text);
                            Constants.EventLog audAction = Constants.EventLog.Print;
                            EventLog.EventType = Convert.ToInt32(audAction);
                            EventLog.EventLogType = Constants.eventlogtype;

                            var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                            CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                            // To get GloveCategory
                            string _GloveCategory;
                            _GloveCategory = TumblingBLL.GetGloveCategory(_customerReject.GloveType);
                            objBatchDTO.GloveTypeDescription = objBatchDTO.GloveType.ToString() + Environment.NewLine + Constants.TAB + _GloveCategory;

                            CommonBLL.PrintDetails(batchDateTime.ToLongTimeString(), objBatchDTO.SerialNumber,
                                objBatchDTO.BatchNumber, objBatchDTO.BatchWeight.ToString(), objBatchDTO.Size,
                                objBatchDTO.TenPcsWeight.ToString(), false, objBatchDTO.GloveTypeDescription,
                                String.Empty, String.Empty);
                            ClearLotFields();
                        }
                    }
                    catch (FloorSystemException)
                    {
                        TumblingBLL.DelCustomerRejectEntry(Convert.ToDecimal(_resultDTO.SerialNumber));
                        throw;
                    }
                    catch (Exception ex)
                    {
                        TumblingBLL.DelCustomerRejectEntry(Convert.ToDecimal(_resultDTO.SerialNumber));
                        throw new FloorSystemException(Messages.AX_SERVICE_UNAVAILABLE, Constants.SERVICEERROR, ex);
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnPrint_Click", null);
                return;
            }
        }
        #endregion

        #region User Methods
        /// <summary>
        /// Clears all controls in Form
        /// </summary>
        private void ClearLotFields()
        {
            txtCustomer.Text = String.Empty;
            txtGloveCode.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtSize.Text = String.Empty;
            txtTotalQuantity.Text = String.Empty;
            txtShift.Text = String.Empty;
            txtLine.Text = String.Empty;
            txtLotNo.Text = String.Empty;
            lblOperatorName.Text = String.Empty;
            txtOperatorId.Text = String.Empty;
            listViewLotNumber.Clear();
            _customerRejectList.Clear();
            //commented by MYAdamas 20171129 due to  _customerReject is null when re keyin the operator or other information reset the customerDTO
            //  _customerReject = null;
            _customerReject = new CustomerRejectDTO();
            txtTenPcsWeight.Text = String.Empty;
            _CaseCapacityCnt = 0;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
             
                cmbChangeQCType.BindComboBox(CommonBLL.GetQCType(), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ClearLotFields", null);

            }
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Validates required fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, Constants.OPERATORID, ValidationType.Required));
            if (listViewLotNumber.Items.Count == 0)
                validationMesssageLst.Add(new ValidationMessage(listViewLotNumber, Constants.LOTNO, ValidationType.Custom));
            validationMesssageLst.Add(new ValidationMessage(cmbChangeQCType, Constants.QCTYPE, ValidationType.Required));
            if (ValidateForm())
                return true;
            else
            {
                if (txtOperatorId.Text == String.Empty)
                    txtOperatorId.Focus();
                else if (listViewLotNumber.Items.Count == 0)
                {
                    txtLotNo.Text = String.Empty;
                    txtLotNo.Focus();
                }
                else if (cmbChangeQCType.Text == String.Empty)
                    cmbChangeQCType.Focus();
                return false;
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
            else if (floorException.subSystem == Constants.DAL)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.INTEGRATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }
        #endregion
    }
}
