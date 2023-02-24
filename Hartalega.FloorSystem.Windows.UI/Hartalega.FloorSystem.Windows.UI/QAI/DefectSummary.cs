#region using
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using Hartalega.FloorSystem.IntegrationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
#endregion

namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    public delegate void SaveQAIAsync();
    public partial class DefectSummary : Form
    {
        #region  private Variables

        private static string _screenName = Constants.DEFECTSUMMARY;
        private static string _screenNameForAuthorization = "Scan Defects Summary";
        private static string _className = "DefectSummary";
        private static bool _isPinHoleDefect;
        private static bool _isAuthenticated;
        private DropdownDTO _qctype;
        private QAIDTO _qaidto;
        private int _groupboxheight;
        private int _tblpanelHeight;
        private string _qcTypeSelected;
        private static bool _canValidateSuggestedQctype;
        private static bool _issaved;
        private int _qAIExpiryDays = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intQAIExpiryDuration);
        private static BatchDTO _batchdto;
        #endregion

        #region Public Variables
        public Constants.QAIPageTransition _SummaryTranistion { get; set; }

        #endregion

        #region Load Form

        public DefectSummary()
        {
            InitializeComponent();
        }

        public DefectSummary(QAIDTO qaidto)
        {
            _qaidto = (QAIDTO)qaidto.Clone();
            _qcTypeSelected = _qaidto.QCType;
            try
            {
                _qctype = QAIBLL.CalculateSuggestedQCType(qaidto);
                //AQL Values is assigned to SelectedValue Property.
                _qaidto.AQLValue = _qctype.SelectedValue;
                _isPinHoleDefect = QAIBLL.IsPinHoleDefect(qaidto.Defects);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "DefectSummary Constructor", null);
                return;
            }
            if (!string.IsNullOrEmpty(_qctype.IDField))
            {
                _qaidto.QCType = _qctype.IDField;
            }
            _isAuthenticated = true;
            _canValidateSuggestedQctype = true;
            InitializeComponent();
            FormLoad();
            _issaved = false;
        }

        private void DefectSummary_Load(object sender, EventArgs e)
        {

        }

        private void FormLoad()
        {
            BindBasicInfo();
            List<QAIDefectType> QAIDefectTypelst = new List<QAIDefectType>();
            foreach (QAIDefectType dt in _qaidto.Defects)
            {
                int k = (from c in dt.DefectList
                         select c.Count).Sum();
                if (k > 0)
                {
                    QAIDefectTypelst.Add(dt);
                }
            }
            BindDefectSummary(QAIDefectTypelst);
            _SummaryTranistion = Constants.QAIPageTransition.FormClose;
        }

        #endregion

        #region Event Handlers

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnCancel.Enabled = false;  //#MH 7/2/2018 to prevent user to click cancel button after save // #Azrul 13/07/2018: Merged from Live AX6
                if (_canValidateSuggestedQctype)
                {
                    if (string.IsNullOrEmpty(txtSuggestedQCType.Text))
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + Constants.QAI_SUMMARY_Y_N, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                }
                if (_qaidto.ScreenName == Constants.QAIScreens.ScanQITestResult)
                {
                    string errorMessage = QAIBLL.ValidateQITestReason(Convert.ToDecimal(_qaidto.SerialNo), _qaidto.QITestReason);

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        GlobalMessageBox.Show(errorMessage, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                }
                if (!_isAuthenticated)
                {
                    LoginScreen();
                    return;
                }
                // #Azrul 13/07/2018: Merged from Live AX6 Start
                /**     //#MH 6/2/2018 disable multiTheading, this call back function service no purpose for performance and may hit the unexpected result 
               MethodInvoker mI = delegate
                {
                    SaveQAIData();
                };
                mI.BeginInvoke(ShowStatus, null);
                **/     //#MH
                try
                {
                    SaveQAIData();
                    if (_issaved)
                    {
                        _SummaryTranistion = Constants.QAIPageTransition.FormClose;
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_QAI, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        _SummaryTranistion = Constants.QAIPageTransition.FormEscape;
                        this.Close();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }
                catch (Exception ex)
                {
                    ExceptionLogging(new FloorSystemException(ex.Message, Constants.AXSERVICEERROR, ex, true), _screenName, _className, "btnSave_Click", null);
                }
            }
            // #Azrul 13/07/2018: Merged from Live AX6 End
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
            catch (Exception ex)
            {
                ExceptionLogging(new FloorSystemException(ex.Message, Constants.AXSERVICEERROR, ex, true), _screenName, _className, "btnSave_Click", null);
            }
        }

        private void ShowStatus()//IAsyncResult res)
        {
            try
            {
                if (_issaved)
                {

                    _SummaryTranistion = Constants.QAIPageTransition.FormClose;
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_QAI, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    this.Close();
                }
                else
                {
                    _SummaryTranistion = Constants.QAIPageTransition.FormEscape;
                    this.Close();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
            catch (Exception ex)
            {
                ExceptionLogging(new FloorSystemException(ex.Message, Constants.AXSERVICEERROR, ex, true), _screenName, _className, "btnSave_Click", null);
            }
        }

        private void SaveQAIData()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //#AZRUL 18/07/2018: Rework order in change QC type, QAI Scan Inner box & QAI Resampling START
                    List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                    bool? IsCreateRework = null;
                    bool IsCreateReworkForStaging = false;
                    QAIDTO qaiBatch = null;
                    qaiBatch = CommonBLL.GetBatchNumberBySerialNumber(Convert.ToDecimal(_qaidto.SerialNo));

                    if (qaiBatch.QAIDate.HasValue && ((CommonBLL.GetCurrentDateAndTime() - qaiBatch.QAIDate.Value).Days > _qAIExpiryDays || _qaidto.IsReSampling)) //#AZRUL 18/07/2018 if QAIDate > 6 month & Re-Sampling
                    {
                        _qaidto.QAIDate = qaiBatch.QAIDate;
                        string STRAIGHT_PACK = (from qc in qctypelst
                                                where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                                select qc.DisplayField).FirstOrDefault();

                        if (_qaidto.QCType != STRAIGHT_PACK)
                        {
                            IsCreateRework = false;
                            string result = GlobalMessageBox.Show(Messages.IS_CREATE_REWORK, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                            if (result == Constants.YES)
                            {
                                Login _passwordForm = new Login(Constants.Modules.REWORKORDER, _screenName);
                                _passwordForm.ShowDialog();
                                if (!(string.IsNullOrEmpty(_passwordForm.Authentication)))
                                    IsCreateRework = true;
                                else
                                {
                                    this.Close();
                                    return;
                                }
                            }
                            else
                            {
                                this.Close();
                                return;
                            }
                        }
                    }
                    //#AZRUL 18/07/2018: Rework order in change QC type, QAI Scan Inner box & QAI Resampling END
                    else
                    {
                        // 2020-07-15 FX - Move QC Type Rework Authentication Confirmation to Defect Summary and trigger during ScanQITestResult
                        if (_qaidto.ScreenName == Constants.QAIScreens.ScanQITestResult)
                        {
                            if (_qaidto.QAITestResult == "Fail" && (_qaidto.QITestReason == "Normal Rework" || _qaidto.QITestReason == "PSI Rework"))
                            {
                                string STRAIGHT_PACK = (from qc in qctypelst
                                                        where qc.IDField.Trim().ToLower() == Constants.STRAIGHT_PACK.ToString().Trim().ToLower()
                                                        select qc.DisplayField).FirstOrDefault();

                                if (_qaidto.QCType != STRAIGHT_PACK)
                                {
                                    BatchDTO tempBatchdto = CommonBLL.GetReworkOrderDetails(Convert.ToDecimal(_qaidto.SerialNo), _qaidto.QCType);
                                    if (tempBatchdto.TotalPcs > Constants.ZERO) //#AZRUL 04/1/2019: Only create rework if Rework qty > 0 
                                    {
                                        string result = GlobalMessageBox.Show(Messages.IS_CREATE_REWORK, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                                        if (result == Constants.YES)
                                        {
                                            Login _passwordForm = new Login(Constants.Modules.REWORKORDER, _screenName);
                                            _passwordForm.ShowDialog();
                                            if (!(string.IsNullOrEmpty(_passwordForm.Authentication)))
                                            {
                                                if(string.IsNullOrEmpty(_qaidto.QCTypeAuthorizedBy))
                                                    _qaidto.QCTypeAuthorizedBy = _passwordForm.Authentication;
                                                IsCreateReworkForStaging = true;
                                            }
                                            else
                                            {
                                                this.Close();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            this.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        GlobalMessageBox.Show(Messages.REWORK_QTY_IS_ZERO, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    _qaidto.SuggestedQCType = (from qc in qctypelst
                                               where qc.IDField.Trim().ToLower() == lblsuggestedQCtype.Text.Trim().ToLower()
                                               select qc.DisplayField).FirstOrDefault();
                    //btnSave.Enabled = false; //temp hide for debug mode

                    //# Commented by Max He, 30/12/2018, due to not testable for Unit Test, moved call inside QAIBLL.SaveQAIData method
                    //#AZRUL 23/08/2018:  Accumulate WT sample quantity after QCQI more than 1 times START
                    //if (AXPostingBLL.GetPostingStage(Convert.ToDecimal(_qaidto.SerialNo)) == Constants.QC_QI)
                    //{
                    //    BatchDTO batchDTO = AXPostingBLL.GetCompleteQCYPDetails(Convert.ToDecimal(_qaidto.SerialNo));
                    //    if (QAIBLL.CheckIsPostWT(batchDTO.QCType))
                    //    {
                    //        QAIBLL.SaveWTSamplingQCQI(_qaidto);
                    //    }
                    //}
                    //#AZRUL 23/08/2018:  Accumulate WT sample quantity after QCQI more than 1 times END

                    Debug.WriteLine(string.Format("Key:{0}, Time:{1}", "before QAIBLL.SaveQAIData", DateTime.Now));

                    //Azman 2020/06/01 START - Perform duplication check before saving.
                    if ((qaiBatch.QAIDate.HasValue && (CommonBLL.GetCurrentDateAndTime() - qaiBatch.QAIDate.Value).Days < _qAIExpiryDays)
                        && (_qaidto.ScreenName == Constants.QAIScreens.QAIScanInnerTenPcs || _qaidto.ScreenName == Constants.QAIScreens.QAIScan))
                    {
                        GlobalMessageBox.Show(Messages.QAI_SCAN_RESAMPLING_SCREEN, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        _issaved = false;
                    }
                    else if ((!string.IsNullOrEmpty(qaiBatch.QCType) && !qaiBatch.QAIDate.HasValue)
                        && (_qaidto.ScreenName == Constants.QAIScreens.QAIScanInnerTenPcs || _qaidto.ScreenName == Constants.QAIScreens.QAIScan))
                    {
                        GlobalMessageBox.Show(Messages.QAI_SCAN_RESAMPLING_SCREEN, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        _issaved = false;
                    }
                    //Azman 2020/06/01 END
                    else
                    {
                        int retVal = QAIBLL.SaveQAIData(_qaidto, WorkStationDTO.GetInstance().WorkStationId, IsCreateRework);

                        string strRouteCategory = QAIBLL.GetRouteCategory(_qaidto.QCType);
                        if (IsCreateReworkForStaging)
                        {
                            BatchDTO tempBatchdto = CommonBLL.GetReworkOrderDetails(Convert.ToDecimal(_qaidto.SerialNo), _qaidto.QCType);
                            if (strRouteCategory != Constants.PT && AXPostingBLL.GetPostingStage(Convert.ToDecimal(_qaidto.SerialNo)) == Constants.QC_QI) //#AZRUL 27/8/2018: If QC Type is OQC
                            {
                                //#AZRUL 27/8/2018: Change QC Type - Handle Non SP to Non SP, not allow to create double rework order (If OQC to OQC - Block 2nd Rework)
                                //#AZRUL 03/1/2019: Bypass checking if batch already scan out (SOBC) 
                                if ((!(QAIBLL.GetPreviousReworkIsOQC(_qaidto.SerialNo))) || CommonBLL.ValidateAXPosting(Convert.ToDecimal(_qaidto.SerialNo), CreateRAFJournalFunctionidentifier.SOBC.ToString()))
                                {
                                    QAIBLL.SaveReworkOrderData(tempBatchdto);
                                }
                            }
                            //#AZRUL 13/4/2022: Rework for PTQI fail if PNBC with QcType SP or Water Tight Batch
                            else if ((AXPostingBLL.GetPostingStage(Convert.ToDecimal(_qaidto.SerialNo)) == Constants.PT_QI || CommonBLL.ReworkCreationForPTPF(tempBatchdto))
                                      && _qaidto.QAITestResult == "Fail")
                            {
                                QAIBLL.SaveReworkOrderData(tempBatchdto);
                            }
                        }
                        // Max He 30/11/2020: QCQI fail PT, scan washer & dryer need to delete previous rework order
                        if (strRouteCategory == Constants.PT && _qaidto.QAITestResult == "Fail") //If QC Type is PT and qai status fail
                        {
                            AXPostingBLL.PostAXDataDeleteReworkOrderPTQIFailPT(_qaidto.SerialNo);
                        }

                        if (retVal > 0)
                        {
                            _issaved = true;

                            if (_qaidto.ScreenName == Constants.QAIScreens.QAIScanInnerTenPcs)
                            {
                                //event log myadamas 20190227
                                EventLogDTO EventLog = new EventLogDTO();

                                EventLog.CreatedBy = _qaidto.InspectorId;
                                Constants.EventLog audAction = Constants.EventLog.Save;
                                EventLog.EventType = Convert.ToInt32(audAction);
                                EventLog.EventLogType = Constants.eventlogtype;

                                var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                                CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                            }
                        }
                        else if (retVal < 0)
                        {
                            GlobalMessageBox.Show(Messages.QAI_SCAN_RESAMPLING_SCREEN, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                            _issaved = false;
                        }
                        else
                        {
                            _issaved = false;
                        }

                        //if (QAIBLL.SaveQAIData(_qaidto, WorkStationDTO.GetInstance().WorkStationId, IsCreateRework) > 0)
                        //{
                        //    _issaved = true;

                        //    if (_qaidto.ScreenName == Constants.QAIScreens.QAIScanInnerTenPcs)
                        //    {
                        //        //event log myadamas 20190227
                        //        EventLogDTO EventLog = new EventLogDTO();

                        //        EventLog.CreatedBy = _qaidto.InspectorId;
                        //        Constants.EventLog audAction = Constants.EventLog.Save;
                        //        EventLog.EventType = Convert.ToInt32(audAction);
                        //        EventLog.EventLogType = Constants.eventlogtype;

                        //        var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                        //        CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                        //    }
                        //}
                        //else
                        //{
                        //    _issaved = false;
                        //}
                    }
                    scope.Complete();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                _SummaryTranistion = Constants.QAIPageTransition.FormEscape;
                this.Close();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_issaved)
            {
                this.Close();
            }
            string result = GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE_QAI, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                _SummaryTranistion = Constants.QAIPageTransition.FormEscape;
                this.Close();
            }
        }

        /// <summary>
        /// ProcessCmdKey
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Subtract)
            {
                _SummaryTranistion = Constants.QAIPageTransition.FormNavigation;
                this.Close();
            }
            if (keyData == Keys.Escape)
            {
                _SummaryTranistion = Constants.QAIPageTransition.FormEscape;
                this.Close();
            }
            bool res = base.ProcessCmdKey(ref msg, keyData);
            return res;
        }

        private void txtSuggestedQCType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToString(e.KeyChar).ToLower() == Convert.ToString(Keys.N).ToLower() || Convert.ToString(e.KeyChar).ToLower() == Convert.ToString(Keys.Y).ToLower())
            {
                _isAuthenticated = true;
                txtSuggestedQCType.Text = Convert.ToString(e.KeyChar).ToUpper();
                if (Convert.ToString(e.KeyChar).ToLower() == Convert.ToString(Keys.Y).ToLower())
                {
                    _isAuthenticated = false;
                    LoginScreen();
                }
                btnSave.Enabled = true;

            }
            else
            {
                e.Handled = true;
                txtSuggestedQCType.Text = string.Empty;
            }
        }

        private void cmbPinhole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtQcTypeSelected.Text.ToUpper().Contains(Constants.QMAX) || lblsuggestedQCtype.Text.ToUpper().Contains(Constants.QMAX) || cmbPinhole.Visible == true)
            {
                string pinholval = cmbPinhole.Text;
                if (lblsuggestedQCtype.Text.ToUpper() == Constants.QMAX || lblsuggestedQCtype.Text.ToUpper() == Constants.QMAX_F || lblsuggestedQCtype.Text.ToUpper() == Constants.QMAX_MP)
                {
                    lblsuggestedQCtype.Text = pinholval;
                }
                else if (pinholval == Constants.QMAX)
                {
                    CombineQcTypes(Constants.QMAX, _qctype.DisplayField);
                }
                else if (pinholval == Constants.QMAX_F)
                {
                    CombineQcTypes(Constants.QMAX_F, _qctype.DisplayField);
                }
                else if (pinholval == Constants.QMAX_MP)
                {
                    CombineQcTypes(Constants.QMAX_MP, _qctype.DisplayField);
                }
                else
                {
                    lblsuggestedQCtype.Text = string.Empty;
                }
                SetQctype();
                if (_qcTypeSelected.Trim() == (_qctype.IDField ?? string.Empty).Trim())
                {
                    _canValidateSuggestedQctype = false;
                    label5.Hide();
                    txtSuggestedQCType.Hide();
                }
                else
                {
                    _canValidateSuggestedQctype = true;
                    label5.Show();
                    txtSuggestedQCType.Show();
                }
            }
        }

        private void CombineQcTypes(string QCTypeFirst, string QCTypeSecond)
        {
            DropdownDTO dropdowndto = QAIBLL.CombineQcTypes(QCTypeFirst, QCTypeSecond);
            if (dropdowndto != null)
            {
                lblsuggestedQCtype.Text = dropdowndto.DisplayField;
            }
            else
            {
                lblsuggestedQCtype.Text = (_qctype.DisplayField ?? string.Empty);
                // GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_QCTYPE_COMBINATION, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            }
        }


        private void txtSuggestedQCType_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSuggestedQCType.Text))
            {
                txtSuggestedQCType.Text = txtSuggestedQCType.Text.ToUpper();
            }
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Bind Basic Info
        /// </summary>
        private void BindBasicInfo()
        {
            QAIDefectGroupBoxMain.Text = _qaidto.ScreenTitle + " - " + Constants.QAI_SUMMARY;
            this.Text = _qaidto.ScreenTitle + " - " + Constants.QAI_SUMMARY;
            txtBatchNo.Text = _qaidto.BatchNo;
            txtSerialNo.Text = _qaidto.SerialNo;
            txtQaiInspectorId.Text = _qaidto.InspectorId;
            txtQaiInspectorName.Text = _qaidto.QaiInspectorName;
            if (_qaidto.ScreenName == Constants.QAIScreens.QAIScanInnerTenPcs)
            {
                txtPackingSize.Text = QAIBLL.GetHBCPackSizeFromSerialNo(Convert.ToInt64(_qaidto.SerialNo));
                txtInnerBox.Text = QAIBLL.GetHBCInBoxFromSerialNo(Convert.ToInt64(_qaidto.SerialNo));
                txtHotBoxsampleSize.Text = Convert.ToString(_qaidto.HBSamplingSize);
                txtWatertghtSampleSize.Text = Convert.ToString(_qaidto.WTSamplingSize);
                txt10PcsSampleSize.Text = Convert.ToString(_qaidto.TenPCSSamplingSize); // #Azrul 13/07/2018: Merged from Live AX6
                txtVisualtstsampleSize.Text = Convert.ToString(_qaidto.VTSamplingSize);
            }
            this.cmbPinhole.SelectedIndexChanged -= new EventHandler(cmbPinhole_SelectedIndexChanged);
            cmbPinhole.BindComboBox(QAIBLL.PinHoleQCtype(), true);
            if (!string.IsNullOrEmpty(_qctype.DisplayField))
            {
                lblsuggestedQCtype.Text = (_qctype.DisplayField ?? string.Empty);
            }
            txtQcTypeSelected.Text = QAIBLL.GetQCtypeDescription(_qcTypeSelected.Trim());
            if (!_isPinHoleDefect)
            {
                label2.Hide();
                cmbPinhole.Hide();
            }
            if (_qcTypeSelected.Trim() == (_qctype.IDField ?? string.Empty).Trim())
            {
                _canValidateSuggestedQctype = false;
                label5.Hide();
                txtSuggestedQCType.Hide();
            }
            this.cmbPinhole.SelectedIndexChanged += new EventHandler(cmbPinhole_SelectedIndexChanged);
        }

        /// <summary>
        /// Bind DEfect Summary
        /// </summary>
        /// <param name="defects"></param>
        private void BindDefectSummary(List<QAIDefectType> defects)
        {
            _tblpanelHeight = 0;
            this.tblpanelSummary.RowCount = defects.Count + 1;
            this.tblpanelSummary.AutoSize = true;
            this.tblpanelSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, tblQCtype.Height + 10));
            this.tblpanelSummary.Controls.Add(this.tblQCtype, 0, 0);
            int i = 1;
            foreach (QAIDefectType qd in defects)
            {
                GroupBox gb = GetGroupBox(qd, i);
                gb.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                this.tblpanelSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, gb.Height + 10));
                this.tblpanelSummary.Controls.Add(gb, 0, i);
                _tblpanelHeight = _tblpanelHeight + gb.Height + 10;
                i++;
            }
            this.tblpanelSummary.Height = _tblpanelHeight;
            QAIDefectGroupBoxMain.Height = tblpanelSummary.Height + 200;

            this.AutoScrollMinSize = new Size(1024, QAIDefectGroupBoxMain.Height + 30);
        }

        /// <summary>
        /// Create GroupBoxes
        /// </summary>
        /// <param name="defect"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private GroupBox GetGroupBox(QAIDefectType defect, int index)
        {
            _groupboxheight = 0;
            GroupBox gb = new GroupBox();
            gb.Width = 950;
            gb.Text = defect.DefectCategory;
            gb.Name = "gb" + index.ToString();
            gb.Controls.Add(GetDefectTable(defect.DefectList, index));
            gb.Height = _groupboxheight;
            return gb;
        }

        /// <summary>
        /// Get Defect Table
        /// </summary>
        /// <param name="defectList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private TableLayoutPanel GetDefectTable(List<QAIDefectDTO> defectList, int index)
        {
            TableLayoutPanel tabledefect = new TableLayoutPanel();
            tabledefect.Visible = true;
            tabledefect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
   | System.Windows.Forms.AnchorStyles.Left)
   | System.Windows.Forms.AnchorStyles.Right))); ;
            tabledefect.AutoSize = true;

            tabledefect.ColumnCount = 6;
            tabledefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            tabledefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            tabledefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            tabledefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            tabledefect.Location = new System.Drawing.Point(10, 50);
            tabledefect.Name = "tblpnl" + index.ToString();
            tabledefect.Size = new System.Drawing.Size(910, 100);

            List<QAIDefectDTO> validdefectlist = (from c in defectList
                                                  where c.Count > 0
                                                  select c).ToList();
            int rowcount = (validdefectlist.Count % 2) == 0 ? (validdefectlist.Count / 2) : (validdefectlist.Count / 2) + 1;
            tabledefect.RowCount = rowcount;
            tabledefect.Height = validdefectlist.Count == 0 ? (rowcount + 1) * 37 : (rowcount + 1) * 43;
            _groupboxheight = tabledefect.Height + 85;
            for (int i = 0; i <= rowcount; i++)
            {
                tabledefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 37F));
            }

            ADDRowsToTable(ref tabledefect, ref validdefectlist, ref rowcount);
            return tabledefect;
        }

        /// <summary>
        /// ADD Rows To Table
        /// </summary>
        /// <param name="tabledefect"></param>
        /// <param name="validdefectlist"></param>
        /// <param name="rowcount"></param>
        private void ADDRowsToTable(ref TableLayoutPanel tabledefect, ref List<QAIDefectDTO> validdefectlist, ref int rowcount)
        {
            int i = 1;
            Font font = new Font("Microsoft Sans Serif", 18.0f,
                   FontStyle.Regular);
            foreach (QAIDefectDTO defct in validdefectlist)
            {
                if (i <= rowcount)
                {
                    Label lab = new Label();
                    lab.Name = "lbl" + Convert.ToString(defct.KeyStroke);
                    lab.Text = defct.KeyStroke + " - " + defct.DefectItem + ":";
                    lab.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    lab.AutoSize = true;
                    lab.Width = 370;
                    lab.Height = 35;
                    tabledefect.Controls.Add(lab, 0, i);

                    TextBox text = new TextBox();
                    text.Name = Convert.ToString(defct.KeyStroke);
                    text.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    text.Text = Convert.ToString(defct.Count);
                    text.Font = font;
                    text.Width = 50;
                    text.TabStop = false;
                    text.ReadOnly = true;
                    tabledefect.Controls.Add(text, 1, i);
                }
                else
                {

                    Label lab = new Label();
                    lab.Name = "lbl" + Convert.ToString(defct.KeyStroke);
                    lab.Text = defct.KeyStroke + " - " + defct.DefectItem + ":"; ;
                    lab.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
                    lab.AutoSize = true;
                    lab.Width = 370;
                    lab.Height = 35;
                    tabledefect.Controls.Add(lab, 2, i - rowcount);

                    TextBox text = new TextBox();
                    text.Name = Convert.ToString(defct.KeyStroke);
                    text.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    text.Text = Convert.ToString(defct.Count);
                    text.Font = font;
                    text.Width = 50;
                    text.ReadOnly = true;
                    text.TabStop = false;
                    tabledefect.Controls.Add(text, 3, i - rowcount);

                }
                i++;
            }

            if (validdefectlist.Count == 0)
            {
                Label lab = new Label();
                lab.Name = "lbl" + "No Defects";
                lab.Text = "No Defects";
                lab.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                lab.Width = 370;
                lab.Height = 35;
                tabledefect.Controls.Add(lab, 0, 0);
            }

            Label labtotal = new Label();
            labtotal.Name = "lbl" + "SubTotal";
            labtotal.Text = "SubTotal:";
            labtotal.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            labtotal.Width = 130;
            labtotal.Height = 35;
            tabledefect.Controls.Add(labtotal, 2, 0);

            TextBox textSubTotal = new TextBox();
            textSubTotal.Name = "textSubTotal";
            textSubTotal.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            textSubTotal.Width = 50;
            textSubTotal.Text = (from c in validdefectlist
                                 select c.Count).Sum().ToString();
            textSubTotal.Font = font;
            textSubTotal.ReadOnly = true;
            textSubTotal.TabStop = false;
            tabledefect.Controls.Add(textSubTotal, 3, 0);
        }


        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.SERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.SERVICE_UNAVAILABLE, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else if (floorException.subSystem == Constants.AXSERVICEERROR)

                GlobalMessageBox.Show("(" + result + ") - " + Messages.AXPOSTINGERROR + floorException.Message, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else

                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
        }

        /// <summary>
        /// Login Screen
        /// </summary>
        private void LoginScreen()
        {
            SetQctype();
            Login passwordForm = new Login(Constants.Modules.QAISYSTEM, _screenNameForAuthorization);
            passwordForm.ShowDialog();

            if (!string.IsNullOrEmpty(passwordForm.Authentication))
            {
                _isAuthenticated = true;
                _qaidto.QCTypeAuthorizedBy = Convert.ToString(passwordForm.Authentication);
                _qaidto.QCType = _qcTypeSelected;
            }
        }

        private void SetQctype()
        {
            List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
            _qaidto.QCType = (from qc in qctypelst
                              where qc.IDField.Trim() == (lblsuggestedQCtype.Text ?? string.Empty).Trim()
                              select qc.DisplayField).FirstOrDefault();
            _qctype.IDField = _qaidto.QCType;
        }
        #endregion

        private void DefectSummary_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_SummaryTranistion != Constants.QAIPageTransition.FormNavigation)
            {
                _SummaryTranistion = Constants.QAIPageTransition.FormEscape;
            }
        }
    }
}


