using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using Hartalega.FloorSystem.Business.Logic;
using System.Configuration;
using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Common;


namespace Hartalega.FloorSystem.Windows.UI.Common
{
    /// <summary>
    /// Class to show the master menu
    /// </summary>
    public partial class MasterMenu : FormBase
    {
        #region Private Variables
        private const string _setupConfigurationMainMenu = "MainMenu";
        private bool _areAllKeysConfigured = false;
        private string _checkKeysMessage = string.Empty;
        #endregion

        private MasterMenu()
        {




        }

        public MasterMenu(bool areAllKeysConfigured,string checkKeysMessage)
        //  : this()
        {
            _areAllKeysConfigured = areAllKeysConfigured;
            _checkKeysMessage = checkKeysMessage;
            InitializeComponent();
        }

        #region Private Methods
        /// <summary>
        /// Load the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterMenu_Load(object sender, System.EventArgs e)
        {
            label1.Text = label1.Text + " " + ConfigurationManager.AppSettings["BuildNumber"].ToString();
            EnableMenuItems();
        }

        private void btnSetupConfiguration_Click(object sender, System.EventArgs e)
        {
            Login _passwordForm = new Login(Constants.Modules.CONFIGURATIONSETUP, _setupConfigurationMainMenu);
            _passwordForm.ShowDialog();
            if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                new SetUpConfiguration.MainMenu(_passwordForm.Authentication).ShowDialog();
            }
        }


        private void btnTumbling_Click(object sender, System.EventArgs e)
        {
            new Tumbling.MainMenu().ShowDialog();
        }

        private void btnQAI_Click(object sender, System.EventArgs e)
        {
            new QAI.Menu().ShowDialog();
        }

        private void btnHourlyBatchCard_Click(object sender, System.EventArgs e)
        {

            //new HourlyBatchCard.LineSelection().ShowDialog();
            new HourlyBatchCard.MainMenu().ShowDialog();
        }

        private void btnWasher_Click(object sender, System.EventArgs e)
        {
            new Washer.MainMenu().ShowDialog();
        }

        private void btnDryer_Click(object sender, System.EventArgs e)
        {
            new Dryer.MainMenu().ShowDialog();
        }

        private void btnPostTreatment_Click(object sender, System.EventArgs e)
        {
            new PostTreatment.MainMenu().ShowDialog();
        }

        private void btnGIS_Click(object sender, System.EventArgs e)
        {
            new GIS.MainMenu().ShowDialog();

        }

        private void btnQCPackingYield_Click(object sender, System.EventArgs e)
        {
            new QCPackingYieldSystem.MainMenu("", "").ShowDialog();

        }

        private void btnQCScanning_Click(object sender, System.EventArgs e)
        {
            new QCScanning.MainMenu().ShowDialog();

        }

        private void btnFinalPacking_Click(object sender, System.EventArgs e)
        {
            new FinalPacking.MainMenu().ShowDialog();

        }

        private void btnQASystem_Click(object sender, System.EventArgs e)
        {
            new QASystem.MainMenu().ShowDialog();
        }

        private void btnReports_Click(object sender, System.EventArgs e)
        {
            new ProductionSystemReports.ReportsSubMenu().ShowDialog();

        }

        private void btnProductionLoggingSystem_Click(object sender, System.EventArgs e)
        {
            new ProductionLogging.MainMenu(string.Empty).ShowDialog();

        }

        private void btnProductionDefectSystem_Click(object sender, System.EventArgs e)
        {
            new ProductionDefect.ProductionDefectList().ShowDialog();

        }
        private void btnEmployeeMaintainence_Click(object sender, System.EventArgs e)
        {
            new SecurityModule.MainMenu().ShowDialog();
        }

        private void btnRoleMaintainence_Click(object sender, System.EventArgs e)
        {

        }

        private void btnWSConfiguration_Click(object sender, System.EventArgs e)
        {
            WorkStationDataConfiguration obj = new WorkStationDataConfiguration();
            obj.ReInitInstance();
            new SetUpConfiguration.WorkStationConfiguration().ShowDialog();
        }

       
        #endregion

        #region Public Methods

        public void EnableMenuItems()
        {
            int positionOfWSModule = Constants.ZERO;
            string moduleids = WorkStationDTO.GetInstance().ModuleIds;
            string[] arrayofModules = moduleids.Split(',');

            if (_areAllKeysConfigured)
            {
                foreach (string moduleid in arrayofModules)
                {
                    switch (moduleid)
                    {
                        case "1":
                            btnTumbling.Enabled = true;
                            break;
                        case "2":
                            btnBatchOrder.Enabled = true;
                            break;
                        case "3":
                            btnQAI.Enabled = true;
                            break;
                        case "4":
                            btnGIS.Enabled = true;
                            break;
                        case "5":
                            btnWasher.Enabled = true;
                            break;
                        case "6":
                            btnDryer.Enabled = true;
                            break;
                        case "7":
                            btnPostTreatment.Enabled = true;
                            break;
                        case "8":
                            btnQCPackingYield.Enabled = true;
                            break;
                        case "9":
                            btnQCScanning.Enabled = true;
                            break;
                        case "10":
                            btnFinalPacking.Enabled = true;
                            break;
                        case "11":
                            btnProductionLoggingSystem.Enabled = true;
                            break;
                        case "12":
                            btnProductionDefectSystem.Enabled = true;
                            break;
                        case "13":
                            btnQASystem.Enabled = true;
                            break;
                        case "14":
                            btnSetupConfiguration.Enabled = true;
                            break;
                        case "15":
                            // case "16":  
                            btnReports.Enabled = true;
                            break;
                        case "16":
                            btnTVReports.Enabled = true;
                            break;
                        case "17":
                            btnEmployeeMaintainence.Enabled = true;
                            break;
                        //WIP Stock Count Button
                        case "21":
                            btnWIPStockCount.Enabled = true;
                            break;
                        //Now the workstation config button will be enabled only for Admin workstations.
                        //case "18":
                        //    btnWSConfiguration.Enabled = true;
                        //    break;
                        //case "19":
                        //btnBrandMaster.Enabled = true;
                        //break;
                        //case "20":
                        //btnWorkOrder.Enabled = true;
                        //break;
                        case "22":
                            btnSurgical.Enabled = true;
                            break;
                    }
                }
            }
            else
            {
                positionOfWSModule = Array.IndexOf(arrayofModules, "18");
                if (positionOfWSModule > Constants.MINUSONE)
                {
                    btnWSConfiguration.Enabled = true;
                }
                
            }
            if (WorkStationDTO.GetInstance().IsAdmin)
                btnWSConfiguration.Enabled = true;
            else
                btnWSConfiguration.Enabled = false;
        }
        #endregion

        private void MasterMenu_Shown(object sender, EventArgs e)
        {
            if (!_areAllKeysConfigured)
            {
               FloorSystemException fsEX =  new FloorSystemException(_checkKeysMessage, Constants.BUSINESSLOGIC, new Exception());
               fsEX.WorkStationId = WorkStationDTO.GetInstance().WorkStationId;
               fsEX.screenName = Messages.MAIN_PROGRAM;
               fsEX.uiClassName = Messages.MAIN;
               fsEX.uiControlName = Messages.MAIN;
               fsEX.LogExceptionToDB();
               GlobalMessageBox.Show(Messages.ENTER_CONFIGURATION_KEYS,Constants.AlertType.Warning,Messages.DATAREQUIRED,GlobalMessageBoxButtons.OK);
            }
        }

        private void btnTVReports_Click(object sender, EventArgs e)
        {
              new TVReports.MainMenu().ShowDialog(); 
        }

        private void btnBatchEnquiry_Click(object sender, EventArgs e)
        {
            new BatchEnquiry().ShowDialog();
        }

        private void btnSurgical_Click(object sender, EventArgs e)
        {
            new SurgicalGloveSystem.MainMenu().ShowDialog();
        }

        private void btnBrandMaster_Click(object sender, EventArgs e)
        {
            new BrandMaster.MainMenu().ShowDialog();
            //Login _passwordForm = new Login(Constants.Modules.BRANDMASTER, Constants.BRANDMASTERMAINMENU);
            //_passwordForm.ShowDialog();
            //if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            //{
            //    new BrandMaster.MainMenu(_passwordForm.Authentication).ShowDialog();
            //}
        }

        private void btnWorkOrder_Click(object sender, EventArgs e)
        {
            new WorkOrder.MainMenu().ShowDialog();
            //Login _passwordForm = new Login(Constants.Modules.WORKORDER, Constants.WORKORDERMAINMENU);
            //_passwordForm.ShowDialog();
            //if (!string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            //{
            //    new WorkOrder.MainMenu(_passwordForm.Authentication).ShowDialog();
            //}
        }

        private void btnWIPStockCount_Click(object sender, EventArgs e)
        {
            new WIPStockCount.MainMenu().ShowDialog();
        }
    }
}

