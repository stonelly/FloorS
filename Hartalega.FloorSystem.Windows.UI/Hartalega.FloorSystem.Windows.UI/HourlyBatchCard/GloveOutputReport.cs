#region using
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    public partial class GloveOutputReport : FormBase
    {
        #region  private Variables
        private static string _screenName = "Glove Output Reporting";
        private static string _className = "GloveOutputReport";
        private StringBuilder _sbSerialNumbers;
        protected bool ISPageValid;
        private bool Is4Outputs = Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().GloveRptShow4Outputs);
        private bool APMStatus = Convert.ToBoolean(WorkStationDataConfiguration.GetInstance().APMStatus);
        private int HBCReprintHours = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intHBCReprintHours);
        private string defTime = string.Empty;
        private int locationId = WorkStationDTO.GetInstance().LocationId;
        private string lineId = string.Empty;
	    private string resFilter1 = string.Empty;
        private string resFilter2 = string.Empty;
        private string resFilter3 = string.Empty;
        private string resFilter4 = string.Empty;
        private string tierOutput1 = string.Empty;
        private string tierOutput2 = string.Empty;
        private string tierOutput3 = string.Empty;
        private string tierOutput4 = string.Empty;
        #endregion

        #region Load Form
        public GloveOutputReport()
        {
            WorkStationDTO.GetInstance().Module = CommonBLL.GetModuleId(Constants.HOURLYBATCHCARD);

            WorkStationDTO.GetInstance().SubModule = CommonBLL.GetSubModuleId(Constants.GLOVEOUTPUTREPORTING);
            InitializeComponent();
            LoadFormData();
            this.ActiveControl = txtOperatorId;
            txtOperatorId.OperatorId();
            if (!Is4Outputs)
            {
                tableLayoutPanel4.Visible = false;
            }
        }

        private void LoadFormData()
        {
            GetOutputTime();
            GetShift();
            GetResourceGrp();
            BindPackingSize();
            BindAPMReason();
        }
        #endregion

        #region User Methods        
        /// <summary>
        /// Prompt password login if inner box can't multiply by 10
        /// </summary>
        public void InnerBoxLeaveValidation(TextBox tbInbox)
        {
            try
            {
                string result = string.Empty;
                if (tbInbox.Text != string.Empty && Convert.ToInt32(tbInbox.Text) % 10 > 0)
                {
                    result = GlobalMessageBox.Show("Proceed with the entered Inner Box Quantity?", Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OKCancel);
                    if (result == Constants.OK)
                    {
                        Login _passwordForm = new Login(Constants.Modules.BATCHORDER, _screenName);
                        _passwordForm.ShowDialog();
                        if (string.IsNullOrEmpty(_passwordForm.Authentication))
                        {
                            tbInbox.Text = string.Empty;
                            this.ActiveControl = tbInbox;
                        }
                    }
                    else
                    {
                        tbInbox.Text = string.Empty;
                        this.ActiveControl = tbInbox;
                    }
                }
                else if (tbInbox.Text != string.Empty && Convert.ToInt32(tbInbox.Text) <= 0)
                {
                    GlobalMessageBox.Show("Please Insert Inner Box Quantity!", Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    tbInbox.Text = string.Empty;
                    this.ActiveControl = tbInbox;
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, txtOperatorId.Name, txtOperatorId.Text);
                return;
            }
        }

        public DateTime GetValidOutputTime()
        {
            DateTime curDateAndHour = CommonBLL.GetCurrentDateAndHourFromServer();
            DateTime selTime = DateTime.ParseExact(cmbOutputTime.Text, "HH00", null, System.Globalization.DateTimeStyles.None);
            if (selTime > curDateAndHour) { selTime = selTime.AddDays(-1); }

            //if reprint hour not exceeds system parameter
            if ((curDateAndHour - selTime).TotalHours > HBCReprintHours)
            {
                GlobalMessageBox.Show("Not allow to print the selected time!", Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                cmbOutputTime.Text = curDateAndHour.AddHours(-1).ToString("HH00");
                this.ActiveControl = cmbOutputTime;
                return selTime;
            }
            //if print current time
            else if (selTime == curDateAndHour)
            {
                GlobalMessageBox.Show("Output time is a current hour!", Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                cmbOutputTime.Text = curDateAndHour.AddHours(-1).ToString("HH00");
                this.ActiveControl = cmbOutputTime;
                return selTime;
            }
            //prompt message if select previous 2 hour or more
            else if ((curDateAndHour - selTime).TotalHours >= 2)
            {
                if (GlobalMessageBox.Show("Confirm to print with selected Output Time?", Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OKCancel) == Constants.Cancel)
                {
                    cmbOutputTime.Text = curDateAndHour.AddHours(-1).ToString("HH00");
                    this.ActiveControl = cmbOutputTime;
                    return selTime;
                }
                else
                {
                    cmbLine.Enabled = true;
                    this.ActiveControl = cmbLine;
                    return selTime;
                }
            }
            else
            {
                cmbLine.Enabled = true;
                this.ActiveControl = cmbLine;
                return selTime;
            }
        }

        /// <summary>
        /// Limit Inner Box maximum qty field not more than 400
        /// </summary>
        public void LimitToRange(TextBox tbInbox)
        {
            if (!(string.IsNullOrEmpty(tbInbox.Text)) && Convert.ToInt32(tbInbox.Text) > 400)
            {
                GlobalMessageBox.Show("Maximum Inner Box Quantity is 400", Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                tbInbox.Text = "";
                tbInbox.Focus();
            }
        }

        /// <summary>
        ///#.List out all Shift
        /// </summary>
        private void GetShift()
        {
            List<DropdownDTO> shiftDTO = CommonBLL.GetShiftByTime(Framework.Constants.ShiftGroup.PN, cmbOutputTime.Text);
            string area = WorkStationDTO.GetInstance().Area;
            cmbShift.DataSource = HourlyBatchCardBLL.GetShiftInfo(area);
            cmbShift.ValueMember = "IDField";
            cmbShift.DisplayMember = "DisplayField";
            cmbShift.Text = shiftDTO[Constants.ZERO].SelectedValue;
        }

        /// <summary>
        ///#.List out all available Output Time
        /// </summary>
        private void GetOutputTime()
        {
            this.cmbOutputTime.SelectedIndexChanged -= new EventHandler(cmbOutputTime_SelectedIndexChanged);
            cmbOutputTime.DataSource = HourlyBatchCardBLL.GetOutputTime(HBCReprintHours);
            cmbOutputTime.ValueMember = "IDField";
            cmbOutputTime.DisplayMember = "DisplayField";
            defTime = CommonBLL.GetCurrentDateAndHourFromServer().AddHours(-1).ToString("HH00");
            cmbOutputTime.Text = defTime;
            this.cmbOutputTime.SelectedIndexChanged += new EventHandler(cmbOutputTime_SelectedIndexChanged);
        }

        /// <summary>
        ///#1.List out all Resources Group (Production lines)
        /// </summary>
        private void GetResourceGrp()
        {
            this.cmbLine.SelectedIndexChanged -= new EventHandler(cmbLine_SelectedIndexChanged);
            cmbLine.DataSource = HourlyBatchCardBLL.GetResourceNBODetails(locationId, null, null, null, null, null, null, null);
            cmbLine.ValueMember = "LineId";
            cmbLine.DisplayMember = "Line";
            this.cmbLine.SelectedIndexChanged += new EventHandler(cmbLine_SelectedIndexChanged);
        }
        
        /// <summary>
        ///#2.List out all Resources (TierSide)
        /// <summary>
        private void GetResource(ComboBox cmbResource, string bo, string resFilter1, string resFilter2, string resFilter3, string resFilter4)
        {
            cmbResource.ValueMember = null;
            cmbResource.DataSource = null;
            cmbResource.Items.Clear();
            cmbResource.ValueMember = "Resource";
            cmbResource.DisplayMember = "Resource";
            cmbResource.DataSource = HourlyBatchCardBLL.GetResourceNBODetails(locationId, lineId, null, bo, resFilter1, resFilter2, resFilter3, resFilter4);
        }
        
        /// <summary>
        ///#3.List out all Batch Order based on selected Resource
        /// <summary>
        private void GetBatchOrder(ComboBox cmbBatchOrder, string resource)
        {
            cmbBatchOrder.Enabled = true;
            cmbBatchOrder.ValueMember = null;
            cmbBatchOrder.DataSource = null;
            cmbBatchOrder.Items.Clear();
            cmbBatchOrder.DataSource = HourlyBatchCardBLL.GetResourceNBODetails(locationId, lineId, resource, null, null, null, null, null);
            cmbBatchOrder.ValueMember = "BthOrderId";
            cmbBatchOrder.DisplayMember = "BthOrderId";
        }

        /// <summary>
        ///#.Default Batch Order when Batch Order list is 1 and default BO for to follow Output1
        /// <summary>
        private void SetDefaultBatchOrder(ComboBox cmbResource, ComboBox cmbBatchOrder)
        {
            if (Convert.ToInt32(cmbBatchOrder.Items.Count.ToString()) == 2)
            {
                cmbBatchOrder.SelectedIndex = 1;
                cmbBatchOrder.Enabled = false;
                cmbBatchOrder.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
            }
            if (!string.IsNullOrEmpty(cmbResource.Text) && cmbBatchOrder.Name != "cmbBatchOrder1")
            {
                cmbBatchOrder.SelectedIndex = cmbBatchOrder1.SelectedIndex;
                cmbBatchOrder.Enabled = false;
            }
        }
        
        /// <summary>
        ///#4.List out all Batch Order Details based on selected Resource and Batch Order
        /// <summary>
        private void GetBatchOrderDetails(TextBox txtResourceId, TextBox txtGloveCode, TextBox txtSize, string resource, string batchOrder)
        {
            List<BatchOrderDetailsDTO> BODetailsForPrint = HourlyBatchCardBLL.GetResourceNBODetails(locationId, lineId, resource, batchOrder, null, null, null, null);
            txtResourceId.Text = BODetailsForPrint[1].ResourceId;
            txtGloveCode.Text = BODetailsForPrint[1].ItemId;
            txtGloveCode.Enabled = false;
            txtSize.Text = BODetailsForPrint[1].Size;
            txtSize.Enabled = false;
        }
        
        /// <summary>
        ///Bind Packing Size lists for all Packing Size combo box
        /// </summary>
        private void BindPackingSize()
        {
            cmbPackingSize1.BindComboBox(CommonBLL.GetPackingSize(), true);
            cmbPackingSize2.BindComboBox(CommonBLL.GetPackingSize(), true);
            if (Is4Outputs)
            {
                cmbPackingSize3.BindComboBox(CommonBLL.GetPackingSize(), true);
                cmbPackingSize4.BindComboBox(CommonBLL.GetPackingSize(), true);
            }
        }

        private void BindAPMReason()
        {
            try
            {
                cmbAPMReason1.DataSource = HourlyBatchCardBLL.GetReasonForAPM();
                cmbAPMReason1.ValueMember = "IDField";
                cmbAPMReason1.DisplayMember = "DisplayField";

                cmbAPMReason2.DataSource = HourlyBatchCardBLL.GetReasonForAPM();
                cmbAPMReason2.ValueMember = "IDField";
                cmbAPMReason2.DisplayMember = "DisplayField";

                cmbAPMReason3.DataSource = HourlyBatchCardBLL.GetReasonForAPM();
                cmbAPMReason3.ValueMember = "IDField";
                cmbAPMReason3.DisplayMember = "DisplayField";

                cmbAPMReason4.DataSource = HourlyBatchCardBLL.GetReasonForAPM();
                cmbAPMReason4.ValueMember = "IDField";
                cmbAPMReason4.DisplayMember = "DisplayField";

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "usp_Reasons_APM_Get", null);
                return;
            }
        }
        private void BindAPMStatus(int No)
        {
            if (No == 1)
            {
                List<DropdownDTO> APMddl1 = new List<DropdownDTO>
            {
                new DropdownDTO { IDField = "0", DisplayField = "APM NOT RUN"},
                new DropdownDTO {  IDField = "1", DisplayField = "APM RUN"}
            };
                cmbAPM1.BindComboBox(APMddl1, false);
            }

            if (No == 2)
            {
                List<DropdownDTO> APMddl2 = new List<DropdownDTO>
                {
                    new DropdownDTO { IDField = "0", DisplayField = "APM NOT RUN"},
                    new DropdownDTO {  IDField = "1", DisplayField = "APM RUN"}

                };
                cmbAPM2.BindComboBox(APMddl2, false);
            }

            if (No == 3)
            {

                List<DropdownDTO> APMddl3 = new List<DropdownDTO>
                {
                    new DropdownDTO { IDField = "0", DisplayField = "APM NOT RUN"},
                    new DropdownDTO {  IDField = "1", DisplayField = "APM RUN"}
                };
                cmbAPM3.BindComboBox(APMddl3, false);
            }

            if (No == 4)
            {
                List<DropdownDTO> APMddl4 = new List<DropdownDTO>
                {
                    new DropdownDTO { IDField = "0", DisplayField = "APM NOT RUN"},
                    new DropdownDTO {  IDField = "1", DisplayField = "APM RUN"}
                };
                cmbAPM4.BindComboBox(APMddl4, false);
            }
        }

        private bool? GetDefaultAPMStatus(string batchOrder)
        {
            APMLog DefaultAPMPackable = HourlyBatchCardBLL.GetDefaultAPMPackable(batchOrder);
            return DefaultAPMPackable.APMPackable;
        }

        /// <summary>
        /// Clear text of the form controls
        /// </summary>
        private void ClearForm()
        {
            txtOperatorId.Text = "";
            txtOperatorId.Enabled = true;
            cmbLine.SelectedIndex = 0;
            cmbResource1.DataSource = null;
            cmbResource2.DataSource = null;
            cmbResource3.DataSource = null;
            cmbResource4.DataSource = null;
            cmbBatchOrder1.DataSource = null;
            cmbBatchOrder2.DataSource = null;
            cmbBatchOrder3.DataSource = null;
            cmbBatchOrder4.DataSource = null;
            cmbLine.Enabled = true;
            cmbOutputTime.Text = CommonBLL.GetCurrentDateAndHourFromServer().AddHours(-1).ToString("HH00");
            cmbAPMReason1.SelectedIndex = 0;
            cmbAPMReason2.SelectedIndex = 0;
            cmbAPMReason3.SelectedIndex = 0;
            cmbAPMReason4.SelectedIndex = 0;
            cmbAPM1.DataSource = null;
            cmbAPM2.DataSource = null;
            cmbAPM3.DataSource = null;
            cmbAPM4.DataSource = null;
            txtOperatorId.Focus();
        }

        /// <summary>
        /// Clear Output based on selected dropdown list
        /// </summary>
        private void ResetOutput(GloveOutputReportResetType output)
        {
            if (output == GloveOutputReportResetType.ResetAll)
            {
                this.cmbResource1.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource1.Enabled = false;
                cmbResource1.Text = null;
                txtResourceId1.Text = null;
                resFilter1 = null;
                resFilter2 = null;
                resFilter3 = null;
                resFilter4 = null;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1)
            {
                this.cmbBatchOrder1.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                cmbBatchOrder1.Enabled = false;
                cmbBatchOrder1.Text = null;

                //AMP
                cmbAPM1.DataSource = null;
                cmbAPM1.Enabled = false;
                cmbAPMReason1.SelectedIndex = 0;
                cmbAPMReason1.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter)
            {
                txtGloveCode1.Text = null;
                txtGloveCode1.Enabled = false;
                txtSize1.Text = null;
                txtSize1.Enabled = false;
                cmbPackingSize1.Enabled = false;
                cmbPackingSize1.Text = null;
                txtInnerBox1.Enabled = false;
                txtInnerBox1.Text = null;
                this.cmbResource2.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource2.Enabled = false;
                resFilter2 = null;
                resFilter3 = null;
                resFilter4 = null;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter || output == GloveOutputReportResetType.ResetOutput2)
            {
                cmbResource2.Text = null;
                txtResourceId2.Text = null;
                this.cmbBatchOrder2.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                cmbBatchOrder2.Enabled = false;
                cmbBatchOrder2.Text = null;

                //AMP
                cmbAPM2.DataSource = null;
                cmbAPM2.Enabled = false;
                cmbAPMReason2.SelectedIndex = 0;
                cmbAPMReason2.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter || output == GloveOutputReportResetType.ResetOutput2 || output == GloveOutputReportResetType.ResetOutput2WithResFilter)
            {
                txtGloveCode2.Text = null;
                txtGloveCode2.Enabled = false;
                txtSize2.Text = null;
                txtSize2.Enabled = false;
                cmbPackingSize2.Enabled = false;
                cmbPackingSize2.Text = null;
                txtInnerBox2.Enabled = false;
                txtInnerBox2.Text = null;
                this.cmbResource3.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource3.Enabled = false;
                resFilter3 = null;
                resFilter4 = null;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter || output == GloveOutputReportResetType.ResetOutput2 || output == GloveOutputReportResetType.ResetOutput2WithResFilter || output == GloveOutputReportResetType.ResetOutput3)
            {
                cmbResource3.Text = null;
                txtResourceId3.Text = null;
                this.cmbBatchOrder3.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                cmbBatchOrder3.Enabled = false;
                cmbBatchOrder3.Text = null;

                //AMP
                cmbAPM3.DataSource = null;
                cmbAPM3.Enabled = false;
                cmbAPMReason3.SelectedIndex = 0;
                cmbAPMReason3.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter || output == GloveOutputReportResetType.ResetOutput2 || output == GloveOutputReportResetType.ResetOutput2WithResFilter || output == GloveOutputReportResetType.ResetOutput3 || output == GloveOutputReportResetType.ResetOutput3WithResFilter)
            {
                txtGloveCode3.Text = null;
                txtGloveCode3.Enabled = false;
                txtSize3.Text = null;
                txtSize3.Enabled = false;
                cmbPackingSize3.Enabled = false;
                cmbPackingSize3.Text = null;
                txtInnerBox3.Enabled = false;
                txtInnerBox3.Text = null;
                this.cmbResource4.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource4.Enabled = false;
                resFilter4 = null;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter || output == GloveOutputReportResetType.ResetOutput2 || output == GloveOutputReportResetType.ResetOutput2WithResFilter || output == GloveOutputReportResetType.ResetOutput3 || output == GloveOutputReportResetType.ResetOutput3WithResFilter || output == GloveOutputReportResetType.ResetOutput4)
            {
                cmbResource4.Text = null;
                txtResourceId4.Text = null;
                this.cmbBatchOrder4.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                cmbBatchOrder4.Enabled = false;
                cmbBatchOrder4.Text = null;

                //AMP
                cmbAPM4.DataSource = null;
                cmbAPM4.Enabled = false;
                cmbAPMReason4.SelectedIndex = 0;
                cmbAPMReason4.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetAll || output == GloveOutputReportResetType.ResetOutput1 || output == GloveOutputReportResetType.ResetOutput1WithResFilter || output == GloveOutputReportResetType.ResetOutput2 || output == GloveOutputReportResetType.ResetOutput2WithResFilter || output == GloveOutputReportResetType.ResetOutput3 || output == GloveOutputReportResetType.ResetOutput3WithResFilter || output == GloveOutputReportResetType.ResetOutput4 || output == GloveOutputReportResetType.ResetOutput4WithResFilter)
            {
                txtGloveCode4.Text = null;
                txtGloveCode4.Enabled = false;
                txtSize4.Text = null;
                txtSize4.Enabled = false;
                cmbPackingSize4.Enabled = false;
                cmbPackingSize4.Text = null;
                txtInnerBox4.Enabled = false;
                txtInnerBox4.Text = null;
            }
            if (output == GloveOutputReportResetType.ResetResFilterOutput1)
            {
                resFilter1 = null;
                resFilter2 = null;
                resFilter3 = null;
                resFilter4 = null;
                //this.cmbBatchOrder1.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                //cmbBatchOrder1.Enabled = false;
                //cmbBatchOrder1.Text = null;
                txtGloveCode1.Text = null;
                txtGloveCode1.Enabled = false;
                txtSize1.Text = null;
                txtSize1.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetResFilterOutput2)
            {
                resFilter2 = null;
                resFilter3 = null;
                resFilter4 = null;
                //this.cmbBatchOrder2.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                //cmbBatchOrder2.Enabled = false;
                //cmbBatchOrder2.Text = null;
                txtGloveCode2.Text = null;
                txtGloveCode2.Enabled = false;
                txtSize2.Text = null;
                txtSize2.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetResFilterOutput3)
            {
                resFilter3 = null;
                resFilter4 = null;
                //this.cmbBatchOrder3.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                //cmbBatchOrder3.Enabled = false;
                //cmbBatchOrder3.Text = null;
                txtGloveCode3.Text = null;
                txtGloveCode3.Enabled = false;
                txtSize3.Text = null;
                txtSize3.Enabled = false;
            }
            if (output == GloveOutputReportResetType.ResetResFilterOutput4)
            {
                resFilter4 = null;
                //this.cmbBatchOrder4.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                //cmbBatchOrder4.Enabled = false;
                //cmbBatchOrder4.Text = null;
                txtGloveCode4.Text = null;
                txtGloveCode4.Enabled = false;
                txtSize4.Text = null;
                txtSize4.Enabled = false;
            }
        }

        /// <summary>
        /// Validate saved data before Print
        /// </summary>
        private bool DataValidate(ComboBox cmbResource, ComboBox cmbBatchOrder, DateTime outputTime)
        {
            bool IsValidate = true;
            //DateTime outputTime = GetValidOutputTime();
            if (!(HourlyBatchCardBLL.CheckSavedHBCToValidate(cmbResource.Text.ToString(), outputTime, cmbBatchOrder.Text.ToString())))
            {
                GlobalMessageBox.Show(Messages.RESOURCE_ALREADY_USED_WITH_THE_SELECTED_TIME, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = btnCancel;
                return IsValidate;
            }
            return IsValidate;
        }

        /// <summary>
        /// Validate form data before Print
        /// </summary>
        private bool FormValidate()
        {
            bool IsValidate = true;
            if (txtOperatorId.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = txtOperatorId;
                return IsValidate;
            }
            if (cmbShift.SelectedIndex == 0)
            {
                GlobalMessageBox.Show(Messages.SHIFT_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbShift;
                return IsValidate;
            }
            if (cmbLine.SelectedIndex == 0)
            {
                GlobalMessageBox.Show(Messages.PRODUCTIONLINE_NOT_SELECTED, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbLine;
                return IsValidate;
            }
            if (cmbResource1.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.RESOURCE_OUTPUT_1_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbResource1;
                return IsValidate;
            }
            if (cmbBatchOrder1.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHORDER_OUTPUT_1_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (!(cmbResource2.Text.Length == 0) && cmbBatchOrder2.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHORDER_OUTPUT_2_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (!(cmbResource3.Text.Length == 0) && cmbBatchOrder3.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHORDER_OUTPUT_3_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (!(cmbResource4.Text.Length == 0) && cmbBatchOrder4.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.BATCHORDER_OUTPUT_4_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (cmbPackingSize1.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.PACKSIZE_1_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = cmbPackingSize1;
                return IsValidate;
            }
            if (!(cmbResource2.Text.Length == 0) && cmbPackingSize2.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.PACKSIZE_2_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (!(cmbResource3.Text.Length == 0) && cmbPackingSize3.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.PACKSIZE_3_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (!(cmbResource4.Text.Length == 0) && cmbPackingSize4.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.PACKSIZE_4_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if (txtInnerBox1.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.INBOX_1_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = txtInnerBox1;
                return IsValidate;
            }
            if (!(cmbPackingSize2.Text.Length == 0) && txtInnerBox2.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.INBOX_2_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = txtInnerBox2;
                return IsValidate;
            }
            if (!(cmbPackingSize3.Text.Length == 0) && txtInnerBox3.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.INBOX_3_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = txtInnerBox3;
                return IsValidate;
            }
            if (!(cmbPackingSize4.Text.Length == 0) && txtInnerBox4.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.INBOX_4_NOT_SELECT, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                this.ActiveControl = txtInnerBox4;
                return IsValidate;
            }
            if (!(string.IsNullOrEmpty(cmbResource2.Text)))
            {
                if ((cmbResource1.Text == cmbResource2.Text) && (cmbBatchOrder1.Text == cmbBatchOrder2.Text) && (txtGloveCode1.Text == txtGloveCode2.Text) && (txtSize1.Text == txtSize2.Text))
                {
                    GlobalMessageBox.Show(Messages.IDENTICAL_OUTPUT1_2, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
                if ((cmbResource2.Text == cmbResource3.Text) && (cmbBatchOrder2.Text == cmbBatchOrder3.Text) && (txtGloveCode2.Text == txtGloveCode3.Text) && (txtSize2.Text == txtSize3.Text))
                {
                    GlobalMessageBox.Show(Messages.IDENTICAL_OUTPUT2_3, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
                if ((cmbResource2.Text == cmbResource4.Text) && (cmbBatchOrder2.Text == cmbBatchOrder4.Text) && (txtGloveCode2.Text == txtGloveCode4.Text) && (txtSize2.Text == txtSize4.Text))
                {
                    GlobalMessageBox.Show(Messages.IDENTICAL_OUTPUT2_4, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (!(string.IsNullOrEmpty(cmbResource3.Text)))
            {
                if ((cmbResource1.Text == cmbResource3.Text) && (cmbBatchOrder1.Text == cmbBatchOrder3.Text) && (txtGloveCode1.Text == txtGloveCode3.Text) && (txtSize1.Text == txtSize3.Text))
                {
                    GlobalMessageBox.Show(Messages.IDENTICAL_OUTPUT1_3, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
                if ((cmbResource3.Text == cmbResource4.Text) && (cmbBatchOrder3.Text == cmbBatchOrder4.Text) && (txtGloveCode3.Text == txtGloveCode4.Text) && (txtSize3.Text == txtSize4.Text))
                {
                    GlobalMessageBox.Show(Messages.IDENTICAL_OUTPUT3_4, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (!(string.IsNullOrEmpty(cmbResource4.Text)))
            {
                if ((cmbResource1.Text == cmbResource4.Text) && (cmbBatchOrder1.Text == cmbBatchOrder4.Text) && (txtGloveCode1.Text == txtGloveCode4.Text) && (txtSize1.Text == txtSize4.Text))
                {
                    GlobalMessageBox.Show(Messages.IDENTICAL_OUTPUT1_4, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (!(cmbResource3.Text.Length == 0) && cmbResource4.Text.Length == 0)
            {
                GlobalMessageBox.Show(Messages.THREE_TIER_BLOCKED, Constants.AlertType.Error, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }

            Decimal TotalGloveQty = 0;
            if (!(string.IsNullOrEmpty(cmbBatchOrder1.Text)))
            {
                TotalGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder1.Text + "'").ToString());
                if (TotalGloveQty == 0)
                {
                    GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    this.ActiveControl = btnCancel;
                    return IsValidate;
                }
            }

            if (!(string.IsNullOrEmpty(cmbBatchOrder2.Text)))
            {
                TotalGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder2.Text + "'").ToString());
                if (TotalGloveQty == 0)
                {
                    GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    this.ActiveControl = btnCancel;
                    return IsValidate;
                }
            }

            if (!(string.IsNullOrEmpty(cmbBatchOrder3.Text)))
            {
                TotalGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder3.Text + "'").ToString());
                if (TotalGloveQty == 0)
                {
                    GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    this.ActiveControl = btnCancel;
                    return IsValidate;
                }
            }

            if (!(string.IsNullOrEmpty(cmbBatchOrder4.Text)))
            {
                TotalGloveQty = Convert.ToDecimal(FloorDBAccess.ExecuteScalar("EXEC USP_BalanceGloveQty_Get '" + cmbBatchOrder4.Text + "'").ToString());
                if (TotalGloveQty == 0)
                {
                    GlobalMessageBox.Show(Messages.UNSUFFICIENT_GLOVE, Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    this.ActiveControl = btnCancel;
                    return IsValidate;
                }
            }

            //APM Packable
            if (APMStatus)
            {
                if (!(cmbResource1.Text.Length == 0) && cmbAPM1.SelectedIndex != 1 && cmbAPMReason1.SelectedIndex == 0)
                {
                    GlobalMessageBox.Show("Please Select APM NOT RUN Reason", Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    //this.ActiveControl = cmbLine;
                    return IsValidate;
                }
                if (!(cmbResource2.Text.Length == 0) && cmbAPM2.SelectedIndex != 1 && cmbAPMReason2.SelectedIndex == 0)
                {
                    GlobalMessageBox.Show("Please Select APM NOT RUN Reason", Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    //this.ActiveControl = cmbLine;
                    return IsValidate;
                }
                if (!(cmbResource3.Text.Length == 0) && cmbAPM3.SelectedIndex != 1 && cmbAPMReason3.SelectedIndex == 0)
                {
                    GlobalMessageBox.Show("Please Select APM NOT RUN Reason", Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    //this.ActiveControl = cmbLine;
                    return IsValidate;
                }
                if (!(cmbResource4.Text.Length == 0) && cmbAPM4.SelectedIndex != 1 && cmbAPMReason4.SelectedIndex == 0)
                {
                    GlobalMessageBox.Show("Please Select APM NOT RUN Reason", Constants.AlertType.Warning, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    //this.ActiveControl = cmbLine;
                    return IsValidate;
                }
            }
                #region Glove code and glove size must be same to enable system to print out the batch card by consolidate the tiers outputs
                tierOutput1 = cmbResource1.Text.Substring(5, 1);
            if (!(string.IsNullOrEmpty(cmbResource2.Text)))
                tierOutput2 = cmbResource2.Text.Substring(5, 1);
            else
                tierOutput2 = string.Empty;
            if (!(string.IsNullOrEmpty(cmbResource3.Text)))
                tierOutput3 = cmbResource3.Text.Substring(5, 1);
            else
                tierOutput3 = string.Empty;
            if (!(string.IsNullOrEmpty(cmbResource4.Text)))
                tierOutput4 = cmbResource4.Text.Substring(5, 1);
            else
                tierOutput4 = string.Empty;
            if (tierOutput1 == "L" && tierOutput2 == "L")
            {
                if (!((txtGloveCode1.Text == txtGloveCode2.Text) && (txtSize1.Text == txtSize2.Text)))
                {
                    GlobalMessageBox.Show(Messages.L_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput1 == "L" && tierOutput3 == "L")
            {
                if (!((txtGloveCode1.Text == txtGloveCode3.Text) && (txtSize1.Text == txtSize3.Text)))
                {
                    GlobalMessageBox.Show(Messages.L_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput1 == "L" && tierOutput4 == "L")
            {
                if (!((txtGloveCode1.Text == txtGloveCode4.Text) && (txtSize1.Text == txtSize4.Text)))
                {
                    GlobalMessageBox.Show(Messages.L_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput2 == "L" && tierOutput3 == "L")
            {
                if (!((txtGloveCode2.Text == txtGloveCode3.Text) && (txtSize2.Text == txtSize3.Text)))
                {
                    GlobalMessageBox.Show(Messages.L_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput1 == "R" && tierOutput2 == "R")
            {
                if (!((txtGloveCode1.Text == txtGloveCode2.Text) && (txtSize1.Text == txtSize2.Text)))
                {
                    GlobalMessageBox.Show(Messages.R_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput1 == "R" && tierOutput3 == "R")
            {
                if (!((txtGloveCode1.Text == txtGloveCode3.Text) && (txtSize1.Text == txtSize3.Text)))
                {
                    GlobalMessageBox.Show(Messages.R_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput1 == "R" && tierOutput4 == "R")
            {
                if (!((txtGloveCode1.Text == txtGloveCode4.Text) && (txtSize1.Text == txtSize4.Text)))
                {
                    GlobalMessageBox.Show(Messages.R_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if (tierOutput2 == "R" && tierOutput3 == "R")
            {
                if (!((txtGloveCode2.Text == txtGloveCode3.Text) && (txtSize2.Text == txtSize3.Text)))
                {
                    GlobalMessageBox.Show(Messages.R_TIER_UNIDENTICAL, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                    IsValidate = false;
                    return IsValidate;
                }
            }
            if ((tierOutput1 == "L" && tierOutput2 == "R") || (tierOutput1 == "R" && tierOutput2 == "L"))
            {
                GlobalMessageBox.Show(Messages.OUTPUT1_2_TIER_NOT_SAME, Constants.AlertType.Error, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }
            if ((tierOutput3 == "L" && tierOutput4 == "R") || (tierOutput3 == "R" && tierOutput4 == "L"))
            {
                GlobalMessageBox.Show(Messages.OUTPUT3_4_TIER_NOT_SAME, Constants.AlertType.Error, Messages.CONFIRM, GlobalMessageBoxButtons.OK);
                IsValidate = false;
                return IsValidate;
            }


            #endregion
            return IsValidate;
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string result = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (result == Constants.YES)
            {
                ClearForm();
                this.ActiveControl = txtOperatorId;
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
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// validate operator id and populate Operator name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_Leave(object sender, EventArgs e)
        {
            ISPageValid = false;
            if (!string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
            {
                try
                {
                    if (!string.IsNullOrEmpty(txtOperatorId.Text))
                    {
                        if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenName))
                        {
                            ISPageValid = true;
                            cmbOutputTime.Enabled = true;
                            cmbOutputTime.Focus();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            txtOperatorId.Text = String.Empty;
                            txtOperatorId.Focus();
                        }
                    }
                    else
                    {
                        txtOperatorId.Text = string.Empty;
                        GlobalMessageBox.Show(Messages.QAI_SCREEN_ACCESS_MSG, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK, SystemIcons.Warning);
                        txtOperatorId.Focus();
                    }
                }
                catch (FloorSystemException fsEX)
                {
                    ExceptionLogging(fsEX, _screenName, _className, "txtOperatorId_Leave", Convert.ToInt16(txtOperatorId.Text));
                    return;
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.EX_GENERIC_EXCEPTION_INVALID_OPERATORID, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                txtOperatorId.Text = string.Empty;
                txtOperatorId.Focus();
            }

        }
        
        private void txtInnerBox_Leave(object sender, EventArgs e)
        {
            TextBox tbInbox = sender as TextBox;
            LimitToRange(tbInbox);
            InnerBoxLeaveValidation(tbInbox);
        }

        private void cmbOutputTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbOutputTime.Leave -= new EventHandler(cmbOutputTime_Leave);
            GetShift();
            this.cmbOutputTime.Leave += new EventHandler(cmbOutputTime_Leave);
        }

        private void cmbOutputTime_Leave(object sender, EventArgs e)
        {
            this.cmbOutputTime.Leave -= new EventHandler(cmbOutputTime_Leave);
            if (btnPrint.Focused == false)
                GetValidOutputTime();
        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cmbLine.SelectedIndex == 0))
            {
                ResetOutput(0);
                this.cmbResource1.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource1.Enabled = true;
                lineId = cmbLine.SelectedValue.ToString();
                GetResource(cmbResource1, cmbBatchOrder1.Text, resFilter1, resFilter2, resFilter3, resFilter4);
                this.cmbResource1.SelectedIndexChanged += new EventHandler(cmbResource_SelectedIndexChanged);
                cmbResource1.SelectedIndex = 0;
            }
            else
            {
                cmbResource1.Enabled = false;
                ResetOutput(0);
            }
        }
        
        private void cmbResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbResource = sender as ComboBox;
            if (!(cmbResource.SelectedIndex == 0))
            {
                if (cmbResource.Name == "cmbResource1")
                {
                    ResetOutput(GloveOutputReportResetType.ResetResFilterOutput1);
                    this.cmbBatchOrder1.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    
                    GetBatchOrder(cmbBatchOrder1, cmbResource1.Text);
                    this.cmbBatchOrder1.SelectedIndexChanged += new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    SetDefaultBatchOrder(cmbResource, cmbBatchOrder1);
                    if (APMStatus)
                    {
                        //APM ddl
                        this.cmbAPM1.SelectedIndexChanged += new EventHandler(cmbAPMStatus_SelectedIndexChanged);
                        BindAPMStatus(1);
                        cmbAPM1.Enabled = true;
                        var batchorder = cmbBatchOrder1.SelectedValue.ToString();
                        var DefaultAPMStatus = GetDefaultAPMStatus(batchorder);
                        if (DefaultAPMStatus != null)
                        {
                            if (DefaultAPMStatus == true)
                            {
                                cmbAPM1.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbAPM1.SelectedIndex = 1;
                            }
                        }
                    }
                }
                if (cmbResource.Name == "cmbResource2")
                {
                    ResetOutput(GloveOutputReportResetType.ResetResFilterOutput2);
                    this.cmbBatchOrder2.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    resFilter2 = cmbResource2.Text;
                    GetResource(cmbResource3, cmbBatchOrder1.Text, resFilter1, resFilter2, resFilter3, resFilter4);
                    GetBatchOrder(cmbBatchOrder2, cmbResource1.Text);
                    this.cmbBatchOrder2.SelectedIndexChanged += new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    SetDefaultBatchOrder(cmbResource,cmbBatchOrder2);
                    if (APMStatus)
                    {
                        //APM ddl
                        this.cmbAPM2.SelectedIndexChanged += new EventHandler(cmbAPMStatus_SelectedIndexChanged);
                        BindAPMStatus(2);
                        cmbAPM2.Enabled = true;
                        var batchorder = cmbBatchOrder2.SelectedValue.ToString();
                        var DefaultAPMStatus = GetDefaultAPMStatus(batchorder);
                        if (DefaultAPMStatus != null)
                        {
                            if (DefaultAPMStatus == true)
                            {
                                cmbAPM2.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbAPM2.SelectedIndex = 1;
                            }
                        }
                    }
                }
                if (cmbResource.Name == "cmbResource3")
                {
                    ResetOutput(GloveOutputReportResetType.ResetResFilterOutput3);
                    this.cmbBatchOrder3.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    resFilter3 = cmbResource3.Text;
                    GetResource(cmbResource4, cmbBatchOrder1.Text, resFilter1, resFilter2, resFilter3, resFilter4);
                    GetBatchOrder(cmbBatchOrder3, cmbResource1.Text);
                    this.cmbBatchOrder3.SelectedIndexChanged += new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    SetDefaultBatchOrder(cmbResource, cmbBatchOrder3);
                    if (APMStatus)
                    {
                        //APM ddl
                        this.cmbAPM3.SelectedIndexChanged += new EventHandler(cmbAPMStatus_SelectedIndexChanged);
                        BindAPMStatus(3);
                        cmbAPM3.Enabled = true;
                        var batchorder = cmbBatchOrder3.SelectedValue.ToString();
                        var DefaultAPMStatus = GetDefaultAPMStatus(batchorder);
                        if (DefaultAPMStatus != null)
                        {
                            if (DefaultAPMStatus == true)
                            {
                                cmbAPM3.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbAPM3.SelectedIndex = 1;
                            }
                        }
                    }
                }
                if (cmbResource.Name == "cmbResource4")
                {
                    ResetOutput(GloveOutputReportResetType.ResetResFilterOutput4);
                    this.cmbBatchOrder4.SelectedIndexChanged -= new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    resFilter4 = cmbResource4.Text;
                    GetBatchOrder(cmbBatchOrder4, cmbResource1.Text);
                    this.cmbBatchOrder4.SelectedIndexChanged += new EventHandler(cmbBatchOrder_SelectedIndexChanged);
                    SetDefaultBatchOrder(cmbResource, cmbBatchOrder4);
                    if (APMStatus)
                    {
                        //APM ddl
                        this.cmbAPM4.SelectedIndexChanged += new EventHandler(cmbAPMStatus_SelectedIndexChanged);
                        BindAPMStatus(4);
                        cmbAPM4.Enabled = true;
                        var batchorder = cmbBatchOrder4.SelectedValue.ToString();
                        var DefaultAPMStatus = GetDefaultAPMStatus(batchorder);
                        if (DefaultAPMStatus != null)
                        {
                            if (DefaultAPMStatus == true)
                            {
                                cmbAPM4.SelectedIndex = 0;
                            }
                            else
                            {
                                cmbAPM4.SelectedIndex = 1;
                            }
                        }
                    }
                }
            }
            else if (cmbResource.Name == "cmbResource1")
                ResetOutput(GloveOutputReportResetType.ResetOutput1);
            else if (cmbResource.Name == "cmbResource2")
                ResetOutput(GloveOutputReportResetType.ResetOutput2);
            else if (cmbResource.Name == "cmbResource3")
                ResetOutput(GloveOutputReportResetType.ResetOutput3);
            else
                ResetOutput(GloveOutputReportResetType.ResetOutput4);
        }
        
        private void cmbBatchOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbBatchOrder = sender as ComboBox;
            if (!(cmbBatchOrder.SelectedIndex == 0))
            {
                if (cmbBatchOrder.Name == "cmbBatchOrder1")
                {
                    resFilter1 = cmbResource1.Text;
                    GetResource(cmbResource2, cmbBatchOrder1.Text, resFilter1, resFilter2, resFilter3, resFilter4);
                    GetBatchOrderDetails(txtResourceId1, txtGloveCode1, txtSize1, cmbResource1.Text, cmbBatchOrder1.Text);
                    this.cmbResource2.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                    this.cmbResource2.SelectedIndexChanged += new EventHandler(cmbResource_SelectedIndexChanged);
                    cmbPackingSize1.Enabled = true;
                    txtInnerBox1.Enabled = true;
                    cmbResource2.Enabled = true;
                }
                if (cmbBatchOrder.Name == "cmbBatchOrder2")
                {
                    GetBatchOrderDetails(txtResourceId2, txtGloveCode2, txtSize2, cmbResource2.Text, cmbBatchOrder2.Text);
                    this.cmbResource3.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                    this.cmbResource3.SelectedIndexChanged += new EventHandler(cmbResource_SelectedIndexChanged);
                    cmbPackingSize2.Enabled = true;
                    txtInnerBox2.Enabled = true;
                    cmbResource3.Enabled = true;
                }
                if (cmbBatchOrder.Name == "cmbBatchOrder3")
                {
                    GetBatchOrderDetails(txtResourceId3, txtGloveCode3, txtSize3, cmbResource3.Text, cmbBatchOrder3.Text);
                    this.cmbResource4.SelectedIndexChanged -= new EventHandler(cmbResource_SelectedIndexChanged);
                    this.cmbResource4.SelectedIndexChanged += new EventHandler(cmbResource_SelectedIndexChanged);
                    cmbPackingSize3.Enabled = true;
                    txtInnerBox3.Enabled = true;
                    cmbResource4.Enabled = true;
                }
                if (cmbBatchOrder.Name == "cmbBatchOrder4")
                {
                    GetBatchOrderDetails(txtResourceId4, txtGloveCode4, txtSize4, cmbResource4.Text, cmbBatchOrder4.Text);
                    cmbPackingSize4.Enabled = true;
                    txtInnerBox4.Enabled = true;
                }
            }
            else if (cmbBatchOrder.Name == "cmbBatchOrder1")
                ResetOutput(GloveOutputReportResetType.ResetResFilterOutput1);
            else if (cmbBatchOrder.Name == "cmbBatchOrder2")
                ResetOutput(GloveOutputReportResetType.ResetResFilterOutput2);
            else if (cmbBatchOrder.Name == "cmbBatchOrder3")
                ResetOutput(GloveOutputReportResetType.ResetResFilterOutput3);
            else
                ResetOutput(GloveOutputReportResetType.ResetResFilterOutput4);
        }

        private void cmbAPMStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbAPM = sender as ComboBox;

            if (cmbAPM.Name == "cmbAPM1")
            {
                if (cmbAPM.SelectedIndex == 1)
                {
                    cmbAPMReason1.SelectedIndex = 0;
                    cmbAPMReason1.Enabled = false;
                }
                else
                {
                    cmbAPMReason1.Enabled = true;
                    cmbAPMReason1.SelectedIndex = 1;
                }

            }
            if (cmbAPM.Name == "cmbAPM2")
            {
                if (cmbAPM.SelectedIndex == 1)
                {
                    cmbAPMReason2.SelectedIndex = 0;
                    cmbAPMReason2.Enabled = false;
                }
                else
                {
                    cmbAPMReason2.Enabled = true;
                    cmbAPMReason2.SelectedIndex = 1;
                }
            }
            if (cmbAPM.Name == "cmbAPM3")
            {
                if (cmbAPM.SelectedIndex == 1)
                {
                    cmbAPMReason3.SelectedIndex = 0;
                    cmbAPMReason3.Enabled = false;
                }
                else
                {
                    cmbAPMReason3.Enabled = true;
                    cmbAPMReason3.SelectedIndex = 1;
                }
            }
            if (cmbAPM.Name == "cmbAPM4")
            {
                if (cmbAPM.SelectedIndex == 1)
                {
                    cmbAPMReason4.SelectedIndex = 0;
                    cmbAPMReason4.Enabled = false;
                }
                else
                {
                    cmbAPMReason4.Enabled = true;
                    cmbAPMReason4.SelectedIndex = 1;
                }
            }


        }

        private void txtInnerBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #endregion

        #region Print button
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.cmbOutputTime.Leave -= new EventHandler(cmbOutputTime_Leave);
            txtOperatorId_Leave(txtOperatorId, e);
            DateTime outputTime = GetValidOutputTime();
            if (FormValidate() && DataValidate(cmbResource1, cmbBatchOrder1, outputTime) && DataValidate(cmbResource2, cmbBatchOrder2, outputTime) && DataValidate(cmbResource3, cmbBatchOrder3, outputTime) && DataValidate(cmbResource4, cmbBatchOrder4, outputTime))
            {
                string result = string.Empty;
                result = GlobalMessageBox.Show(Messages.CONFIRM_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                if (result == Constants.YES)
                {
                    string userId = txtOperatorId.Text.ToString();
                    int shiftId = Convert.ToInt32(cmbShift.SelectedValue);
                    string lineId = cmbLine.SelectedValue.ToString();
                    DateTime batchCardDate = ServerCurrentDateTime;
                    string ModuleId = WorkStationDTO.GetInstance().Module;
                    string SubModuleID = WorkStationDTO.GetInstance().SubModule;
                    int sitenumber = Convert.ToInt32(FloorSystemConfiguration.GetInstance().intSiteNumber);
                    int WorkStationNumber = Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId);
                    StringBuilder resourceId = new StringBuilder("");
                    StringBuilder packingSize = new StringBuilder("");
                    StringBuilder innerBox = new StringBuilder("");
                    resourceId.Append(txtResourceId1.Text + Constants.COMMA);
                    packingSize.Append(cmbPackingSize1.Text + Constants.COMMA);
                    innerBox.Append(txtInnerBox1.Text + Constants.COMMA);
                    if (!(string.IsNullOrEmpty(txtResourceId2.Text)))
                    {
                        resourceId.Append(txtResourceId2.Text + Constants.COMMA);
                        packingSize.Append(cmbPackingSize2.Text + Constants.COMMA);
                        innerBox.Append(txtInnerBox2.Text + Constants.COMMA);
                        if (!(string.IsNullOrEmpty(txtResourceId3.Text)))
                        {
                            resourceId.Append(txtResourceId3.Text + Constants.COMMA);
                            packingSize.Append(cmbPackingSize3.Text + Constants.COMMA);
                            innerBox.Append(txtInnerBox3.Text + Constants.COMMA);
                            if (!(string.IsNullOrEmpty(txtResourceId4.Text)))
                            {
                                resourceId.Append(txtResourceId4.Text + Constants.COMMA);
                                packingSize.Append(cmbPackingSize4.Text + Constants.COMMA);
                                innerBox.Append(txtInnerBox4.Text + Constants.COMMA);
                            }
                        }
                    }
                    List<BatchOrderDetailsDTO> lstBatchDTO = new List<BatchOrderDetailsDTO>();
                    try
                    {
                        string resourceIds = resourceId.ToString().Length > Constants.ZERO ? resourceId.ToString().Substring(Constants.ZERO, resourceId.ToString().Length - 1) : string.Empty;
                        string packingSizes = packingSize.ToString().Length > Constants.ZERO ? packingSize.ToString().Substring(Constants.ZERO, packingSize.ToString().Length - 1) : string.Empty;
                        string innerBoxes = innerBox.ToString().Length > Constants.ZERO ? innerBox.ToString().Substring(Constants.ZERO, innerBox.ToString().Length - 1) : string.Empty;
                        lstBatchDTO = HourlyBatchCardBLL.GetPrintBatchDetails(userId, shiftId, outputTime, lineId, batchCardDate, ModuleId, SubModuleID, sitenumber, WorkStationNumber, resourceIds, packingSizes, innerBoxes);
                        if (lstBatchDTO.Count > Constants.ZERO)
                        {
                            AddAPMLog(lstBatchDTO, cmbResource1.Text, cmbResource2.Text, cmbResource3.Text, cmbResource4.Text);
                            this.BeginInvoke(new PrintAsync(PrintBatchAsync), lstBatchDTO);
                        }
                    }
                    catch (FloorSystemException ex)
                    {
                        ExceptionLogging(ex, _screenName, _className, "btnPrint_Click", null);
                        return;
                    }
                }
            }
        }

        private void AddAPMLog(List<BatchOrderDetailsDTO> lstBatchDTO, string resource1, string resource2, string resource3, string resource4)
        {
            var CurrentDateTime = DateTime.Now;

            if (lstBatchDTO.Count > 0)
            {
                if (!string.IsNullOrEmpty(resource1))
                {
                    APMLog List = new APMLog();


                    for (int i = 0; i < lstBatchDTO.Count; i++)
                    {
                        var batchResource = lstBatchDTO[i].Resource.ToString().Split(',');
                        foreach (var item in batchResource)
                        {
                            if (resource1 == item.Trim())
                            {
                                List.SerialNumber = Convert.ToDecimal(lstBatchDTO[i].SerialNumber);
                            }
                        }
                    }

                    List.Resource = resource1;
                    List.APMPackable = false;
                    if (Convert.ToInt32(cmbAPM1.SelectedValue) == 0)
                    {
                        List.APMStatus = false;
                    }
                    else
                    {
                        List.APMStatus = true;
                    }
                    if (Convert.ToInt32(cmbAPMReason1.SelectedIndex) != 0)
                    {
                        List.APMReason = Convert.ToInt32(cmbAPMReason1.SelectedValue);
                    }
                    else
                    {
                        List.APMReason = null;
                    }
                    List.CreatedDate = CurrentDateTime;
                    List.CreatedBy = Convert.ToInt32(txtOperatorId.Text);

                    HourlyBatchCardBLL.SaveAPMLog(List);
                }

                if (!string.IsNullOrEmpty(resource2))
                {
                    APMLog List = new APMLog();
                    for (int i = 0; i < lstBatchDTO.Count; i++)
                    {
                        var batchResource = lstBatchDTO[i].Resource.ToString().Split(',');
                        foreach (var item in batchResource)
                        {
                            if (resource2 == item.Trim())
                            {
                                List.SerialNumber = Convert.ToDecimal(lstBatchDTO[i].SerialNumber);
                            }
                        }
                    }
                    List.Resource = resource2;
                    List.APMPackable = false;
                    if (Convert.ToInt32(cmbAPM2.SelectedValue) == 0)
                    {
                        List.APMStatus = false;
                    }
                    else
                    {
                        List.APMStatus = true;
                    }
                    if (Convert.ToInt32(cmbAPMReason2.SelectedIndex) != 0)
                    {
                        List.APMReason = Convert.ToInt32(cmbAPMReason2.SelectedValue);
                    }
                    else
                    {
                        List.APMReason = null;
                    }
                    List.CreatedDate = CurrentDateTime;
                    List.CreatedBy = Convert.ToInt32(txtOperatorId.Text);

                    HourlyBatchCardBLL.SaveAPMLog(List);
                }

                if (!string.IsNullOrEmpty(resource3))
                {
                    APMLog List = new APMLog();


                    for (int i = 0; i < lstBatchDTO.Count; i++)
                    {
                        var batchResource = lstBatchDTO[i].Resource.ToString().Split(',');
                        foreach (var item in batchResource)
                        {
                            if (resource3 == item.Trim())
                            {
                                List.SerialNumber = Convert.ToDecimal(lstBatchDTO[i].SerialNumber);
                            }
                        }
                    }

                    List.Resource = resource3;
                    List.APMPackable = false;
                    if (Convert.ToInt32(cmbAPM3.SelectedValue) == 0)
                    {
                        List.APMStatus = false;
                    }
                    else
                    {
                        List.APMStatus = true;
                    }
                    if (Convert.ToInt32(cmbAPMReason3.SelectedIndex) != 0)
                    {
                        List.APMReason = Convert.ToInt32(cmbAPMReason3.SelectedValue);
                    }
                    else
                    {
                        List.APMReason = null;
                    }
                    List.CreatedDate = CurrentDateTime;
                    List.CreatedBy = Convert.ToInt32(txtOperatorId.Text);

                    HourlyBatchCardBLL.SaveAPMLog(List);
                }

                if (!string.IsNullOrEmpty(resource4))
                {
                    APMLog List = new APMLog();


                    for (int i = 0; i < lstBatchDTO.Count; i++)
                    {
                        var batchResource = lstBatchDTO[i].Resource.ToString().Split(',');
                        foreach (var item in batchResource)
                        {
                            if (resource4 == item.Trim())
                            {
                                List.SerialNumber = Convert.ToDecimal(lstBatchDTO[i].SerialNumber);
                            }
                        }
                    }

                    List.Resource = resource4;
                    List.APMPackable = false;
                    if (Convert.ToInt32(cmbAPM4.SelectedValue) == 0)
                    {
                        List.APMStatus = false;
                    }
                    else
                    {
                        List.APMStatus = true;
                    }
                    if (Convert.ToInt32(cmbAPMReason4.SelectedIndex) != 0)
                    {
                        List.APMReason = Convert.ToInt32(cmbAPMReason4.SelectedValue);
                    }
                    else
                    {
                        List.APMReason = null;
                    }
                    List.CreatedDate = CurrentDateTime;
                    List.CreatedBy = Convert.ToInt32(txtOperatorId.Text);

                    HourlyBatchCardBLL.SaveAPMLog(List);
                }
            }


        }

        private void PrintBatchAsync(List<BatchOrderDetailsDTO> lstBatchDTO)
        {
            _sbSerialNumbers = new StringBuilder("");
            this.SuspendLayout();
            this.Cursor = Cursors.WaitCursor;

            Enabled = false;
            List<PrintDTO> printdtolst = new List<PrintDTO>();
            foreach (BatchOrderDetailsDTO objbatchDTO in lstBatchDTO)
            {
                PrintDTO printdto = HourlyBatchCardBLL.GetBatchList(objbatchDTO.OutputTime.ToString("HH:mm:ss"), objbatchDTO.SerialNumber, objbatchDTO.BatchNumber, objbatchDTO.Resource, objbatchDTO.Size, objbatchDTO.GloveType, objbatchDTO.PackingSize, objbatchDTO.InnerBox, objbatchDTO.TotalGloveQty, Constants.HOURLYBATCHCARD, false);
                printdtolst.Add(printdto);
                _sbSerialNumbers.Append(objbatchDTO.SerialNumber);
                _sbSerialNumbers.Append(Constants.COMMA);
            }
            PrintBatchCard(printdtolst);
            Enabled = true;
            this.Cursor = Cursors.Default;
            this.ResumeLayout();
            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
            ClearForm();
            //this.Close();
        }
        
        private void PrintBatchCard(List<PrintDTO> printdtoLst)
        {
            try
            {
                HourlyBatchCardBLL.PrintDetails(printdtoLst);
            }
            catch (InvalidPrinterException)
            {
                PrinterClass.IsPrinterExists = false;
                GlobalMessageBox.Show(Messages.NO_PRINTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PrintBatchCard", printdtoLst);
                return;
            }
        }
        #endregion
    }
}