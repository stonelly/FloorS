using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{

    public partial class MasterTableMainMenu : Form
    {
        #region Member Variables
        private string _loggedInUser;
        private string _screenName = "MasterTable - MainMenu";
        private string _className = "MainMenu";
        #endregion

        #region Member Methods
        public MasterTableMainMenu(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }

        /// <summary>
        /// Binds the list of Master Tables
        /// </summary>
        private void BindMasterTableList()
        {
            try
            {
                this.cmbTableList.SelectedIndexChanged -= new System.EventHandler(this.cmbTableList_SelectedIndexChanged);
                List<DropdownDTO> tableList = CommonBLL.GetEnumText("MasterTable");
                cmbTableList.ComboBox.Items.Add(string.Empty);
                for (int i = 0; i < tableList.Count; i++)
                {
                    cmbTableList.ComboBox.Items.Add(tableList[i].DisplayField);
                }
                cmbTableList.ComboBox.SelectedIndex = Constants.ZERO;
                this.cmbTableList.SelectedIndexChanged += new System.EventHandler(this.cmbTableList_SelectedIndexChanged);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindGroupType", null);
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
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        private void cmbTableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbTableList.Text)
            {
                case MyValues.WorkstationMaster:
                    CloseAllMDIChild();
                    WorkstationMaster objWs = new WorkstationMaster(_loggedInUser);
                    objWs.MdiParent = this;
                    objWs.Dock = DockStyle.Fill;
                    objWs.Show();
                    break;
                case MyValues.ActivityTypeMaster:
                    CloseAllMDIChild();
                    ActivityTypeMaster objActivity = new ActivityTypeMaster(_loggedInUser);
                    objActivity.MdiParent = this;
                    objActivity.Dock = DockStyle.Fill;
                    objActivity.Show();
                    break;
                case MyValues.MessageMaster:
                    CloseAllMDIChild();
                    MessageMaster objMessage = new MessageMaster(_loggedInUser);
                    objMessage.MdiParent = this;
                    objMessage.Dock = DockStyle.Fill;
                    objMessage.Show();
                    break;
                case MyValues.WasherMaster:
                    CloseAllMDIChild();
                    WasherMasterTable objWasher = new WasherMasterTable(_loggedInUser);
                    objWasher.MdiParent = this;
                    objWasher.Dock = DockStyle.Fill;
                    objWasher.Show();
                    break;
                case MyValues.DryerMaster:
                    CloseAllMDIChild();
                    DryerMasterTable objDryer = new DryerMasterTable(_loggedInUser);
                    objDryer.MdiParent = this;
                    objDryer.Dock = DockStyle.Fill;
                    objDryer.Show();
                    break;
                case MyValues.BinMaster:
                    CloseAllMDIChild();
                    BinMaster objBin = new BinMaster(_loggedInUser);
                    objBin.MdiParent = this;
                    objBin.Dock = DockStyle.Fill;
                    objBin.Show();
                    break;
                case MyValues.DefectiveGlove:
                    CloseAllMDIChild();
                    DefectiveGlove objDg = new DefectiveGlove(_loggedInUser);
                    objDg.MdiParent = this;
                    objDg.Dock = DockStyle.Fill;
                    objDg.Show();
                    break;
                case "":
                    CloseAllMDIChild();
                    break;

                case MyValues.PalletMaster:
                    CloseAllMDIChild();
                    PalletMaster objPs = new PalletMaster(_loggedInUser);
                    objPs.MdiParent = this;
                    objPs.Dock = DockStyle.Fill;
                    objPs.Show();
                    break;
                case MyValues.ProductionDefectMaster:
                    CloseAllMDIChild();
                    ProductionDefectMaster pdmWs = new ProductionDefectMaster(_loggedInUser);
                    pdmWs.MdiParent = this;
                    pdmWs.Dock = DockStyle.Fill;
                    pdmWs.Show();
                    break;
                case MyValues.LineMaster:
                    CloseAllMDIChild();
                    LineMaster lmWs = new LineMaster(_loggedInUser);
                    lmWs.MdiParent = this;
                    lmWs.Dock = DockStyle.Fill;
                    lmWs.Show();
                    break;
                case MyValues.QAIAQReferenceFirst:
                    CloseAllMDIChild();
                    QAIAQReferenceFirst qrfWs = new QAIAQReferenceFirst(_loggedInUser);
                    qrfWs.MdiParent = this;
                    qrfWs.Dock = DockStyle.Fill;
                    qrfWs.Show();
                    break;
                case MyValues.QITestResultAQL:
                    CloseAllMDIChild();
                    QITestResultAQL qtWs = new QITestResultAQL(_loggedInUser);
                    qtWs.MdiParent = this;
                    qtWs.Dock = DockStyle.Fill;
                    qtWs.Show();
                    break;
                case MyValues.QAIAQCosmeticReferenceSecond:
                    CloseAllMDIChild();
                    QAIAQCosmeticReferenceSecond objAQL = new QAIAQCosmeticReferenceSecond(_loggedInUser);
                    objAQL.MdiParent = this;
                    objAQL.Dock = DockStyle.Fill;
                    objAQL.Show();
                    break;
                case MyValues.GloveSizeMaster:
                    CloseAllMDIChild();
                    GloveSizeMaster objgs=new GloveSizeMaster(_loggedInUser);
                    objgs.MdiParent = this;
                    objgs.Dock = DockStyle.Fill;
                    objgs.Show();
                    break;
                case MyValues.BatchTypeMaster:
                    CloseAllMDIChild();
                    BatchTypeMaster objbt = new BatchTypeMaster(_loggedInUser);
                    objbt.MdiParent = this;
                    objbt.Dock = DockStyle.Fill;
                    objbt.Show();
                    break;
                case MyValues.QCTypeMaster:
                    CloseAllMDIChild();
                    QCTypeMaster objqc = new QCTypeMaster(_loggedInUser);
                    objqc.MdiParent = this;
                    objqc.Dock = DockStyle.Fill;
                    objqc.Show();
                    break;
                case MyValues.LocationMaster:
                    CloseAllMDIChild();
                    LocationMaster objlm = new LocationMaster(_loggedInUser);
                    objlm.MdiParent = this;
                    objlm.Dock = DockStyle.Fill;
                    objlm.Show();
                    break;
                case MyValues.ShiftMaster:
                    CloseAllMDIChild();
                    ShiftMaster objsm = new ShiftMaster(_loggedInUser);
                    objsm.MdiParent = this;
                    objsm.Dock = DockStyle.Fill;
                    objsm.Show();
                    break;
                case MyValues.GloveCategoryMaster:
                    CloseAllMDIChild();
                    GloveCategoryMaster objgc= new GloveCategoryMaster(_loggedInUser);
                    objgc.MdiParent = this;
                    objgc.Dock = DockStyle.Fill;
                    objgc.Show();
                    break;
                case MyValues.InnerLabelSetMaster:
                     CloseAllMDIChild();
                    InnerLabelSetMaster objils = new InnerLabelSetMaster(_loggedInUser);
                    objils.MdiParent = this;
                    objils.Dock = DockStyle.Fill;
                    objils.Show();
                    break;
                case MyValues.OuterLabelSetMaster:
                    CloseAllMDIChild();
                    OuterLabelSetMaster objols = new OuterLabelSetMaster(_loggedInUser);
                    objols.MdiParent = this;
                    objols.Dock = DockStyle.Fill;
                    objols.Show();
                    break;
                case MyValues.PreshipmentSamplingPlanMaster:
                    CloseAllMDIChild();
                    PreshipmentSamplingPlanMaster objpsp = new PreshipmentSamplingPlanMaster(_loggedInUser);
                    objpsp.MdiParent = this;
                    objpsp.Dock = DockStyle.Fill;
                    objpsp.Show();
                    break;
            }
        }

        /// <summary>
        /// CloseAllMDIChild
        /// </summary>
        private void CloseAllMDIChild()
        {
            foreach (Form childForm in this.MdiChildren)
                childForm.Close();
        }

        /// <summary>
        /// MainMenu_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseAllMDIChild();
                this.Close();
            }
        }
      
        /// <summary>
        /// MainMenu_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Load(object sender, EventArgs e)
        {
            BindMasterTableList();
        }
        #endregion
    }

    public class MyValues
    {
        public const string WorkstationMaster = "WorkstationMaster";
        public const string ActivityTypeMaster = "ActivityTypeMaster";
        public const string MessageMaster = "MessageMaster";
        public const string WasherMaster = "WasherMaster";
        public const string DryerMaster = "DryerMaster";
        public const string BinMaster = "BinMaster";
        public const string DefectiveGlove = "DefectiveGlove";
        public const string ProductionDefectMaster = "ProductionDefectMaster";
        public const string LineMaster = "LineMaster";
        public const string QAIAQReferenceFirst = "QAIAQReferenceFirst";
        public const string QITestResultAQL = "QITestResultAQL";
        public const string PalletMaster = "PalletMaster";
        public const string QAIAQCosmeticReferenceSecond = "QAIAQCosmeticReferenceSecond";
        public const string GloveSizeMaster = "GloveSizeMaster";
        public const string BatchTypeMaster = "BatchTypeMaster";
        public const string QCTypeMaster = "QCTypeMaster";
        public const string LocationMaster = "LocationMaster";
        public const string ShiftMaster = "ShiftMaster";
        public const string GloveCategoryMaster = "GloveCategoryMaster";
        public const string InnerLabelSetMaster = "InnerLabelSetMaster";
        public const string OuterLabelSetMaster = "OuterLabelSetMaster";
        public const string PreshipmentSamplingPlanMaster = "PreshipmentSamplingPlanMaster";
    }
}
