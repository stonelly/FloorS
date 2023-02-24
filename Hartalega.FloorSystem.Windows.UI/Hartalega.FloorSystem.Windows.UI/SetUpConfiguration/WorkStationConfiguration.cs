using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;



namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    ///
    ///</summary>

    public partial class WorkStationConfiguration : FormBase
    {
        /// <summary>
        /// Business Logic class.
        ///</summary>

        private ConfMgr _wsBLL = new ConfMgr();

        /// <summary>
        /// Location where controls will be added.
        ///</summary>
        private readonly int _locationDelta = 30;

        /// <summary>
        ///Height of the controls
        ///</summary>
        private readonly int _txtBoxHt = 180;

        private readonly int _txtBoxWidth = 350;

        private readonly int _txtConfBoxkeyWidth = 220;
        private readonly int _txtConfBoxvalWidth = 180;



        /// <summary>
        ///
        ///</summary>
        private volatile bool _formLoaded = false;
        int selownIndex;
        private string ConfigId;
        private string DecryptedConnectionString = string.Empty;
        private string Flag = string.Empty;


        /// <summary>
        ///
        ///</summary>
        public WorkStationConfiguration()
        {
            InitializeComponent();
        }

        #region Event Handlers
        /// <summary>
        /// Selection Change handler for Configurations
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void cmbConf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_formLoaded)
            {
                lblCurrentConfigurationLabel.Visible = false;
                lblCurrConfiguration.Visible = false;
                return;
            }
            this.lstBoxModules.SelectedIndex = -1;
            ClearConfItems();
            int selIndex = cmbConfData.SelectedIndex;
            if (selIndex == -1)
            {
                lblCurrentConfigurationLabel.Visible = false;
                lblCurrConfiguration.Visible = false;

                return;
            }
            else
            {
                lblCurrentConfigurationLabel.Visible = true;
                lblCurrConfiguration.Visible = true;

            }
            _wsBLL.SetWSDataFields(selIndex);
            SetModulesList();
            AddConfItemsFromConfData();
            SetWorkStationList();
            btnSave.Enabled = true;
            lblCurrConfiguration.Text = cmbConfData.Text;
        }

        /// <summary>
        /// Form load
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void WorkStationConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                _wsBLL.LoadModuleData();
                //tabs.TabPages.Remove(tabSystemConf);
                ConfigId = _wsBLL.GetSelectWorkStation(System.Net.Dns.GetHostName());
                DecryptedConnectionString = DecryptConnectionString(ConfigurationManager.ConnectionStrings["FSDB"].ToString());
                txtConnectionString.Text = DecryptedConnectionString;
            }

            catch (FloorSystemException ex)
            {
                //Not Looging the exception since MIS team is going to use this Module.

                ShowExceptionMessageToTheUser(Messages.MODULE_INFO_NOT_AVAILABLE_IN_DATABASE, ex);
                return;
            }

            try
            {
                _wsBLL.LoadAllLocations();
                _wsBLL.LoadAreaLocations();
            }
            catch (FloorSystemException ex)
            {
                ShowExceptionMessageToTheUser(Messages.LOCATION_INFO_NOT_AVAILABLE_IN_DATABASE, ex);
                return;
            }

            try
            {
                _wsBLL.LoadWorkStationAndConfData();
            }

            catch (FloorSystemException ex)
            {
                ShowExceptionMessageToTheUser(Messages.WORKSTATION_INFO_NOT_AVAILABLE_IN_DATABASE, ex);
                return;
            }

            try
            {
                _wsBLL.LoadSystemConfigurationData();
                AddSystemConfItemsFromConfData();
            }
            catch (FloorSystemException ex)
            {
                ShowExceptionMessageToTheUser(Messages.SYSTEM_WIDE_CONFIGURATION_NOT_AVAILABLE, ex);
                return;
            }

            SetUnSelectedModuleList();
            SetUnSelectedWSNames();
            SetConfComboBox();
            SetLocationDataBox();

            _formLoaded = true;

            //Retrive Index start
            if (!string.IsNullOrEmpty(ConfigId))
            {
                selownIndex = _wsBLL.LstWSConfData.FindIndex(a => a.ConfId == Convert.ToInt32(ConfigId));//
                cmbConfData.SelectedIndex = selownIndex;
                if (selownIndex == 0)
                { cmbConf_SelectedIndexChanged(selownIndex, null); }
            }
            else { cmbConfData.SelectedIndex = -1; }
            this.WindowState = FormWindowState.Maximized;
            //End
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private void btnAddKey_Click(object sender, EventArgs e)
        {
            WorkStationDetails wsd = new WorkStationDetails(_wsBLL);

            if (wsd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddConfigurationItem(wsd._key);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbConfData.Text) || _wsBLL.LstSelectedWS == null)
            {

                GlobalMessageBox.Show(Messages.WORKSTAION_IS_NOT_SELECTED, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                return;
            }
            switch (tabs.SelectedTab.Text)
            {
                case "Configuration":
                    {
                        SaveConfigurationData();
                        break;
                    }

                case "WorkStations":
                    {
                        SaveWorkStationData();
                        break;
                    }

                case "System Wide Configuration":
                    {
                        SaveSystemConfigurationData();
                        break;
                    }
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>

        private void btnMoveWSToUnSel_Click(object sender, EventArgs e)
        {
            if (lstSelectedWS.SelectedItem != null)
            {
                RemoveSelectedWS(lstSelectedWS.SelectedItem);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCEL_WS, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                this.Close();
        }

        private void btnMoveSelected_Click(object sender, EventArgs e)
        {
            if (lstBoxModules.SelectedItem != null)
            {
                AddSelectedModule(lstBoxModules.SelectedItem);
            }
        }

        private void btnMoveUnSelected_Click(object sender, EventArgs e)
        {
            if (lstSelectedModules.SelectedItem != null)
            {
                RemoveSelectedModuleItem(lstSelectedModules.SelectedItem);
            }
        }

        private void btnMoveWSToSel_Click(object sender, EventArgs e)
        {
            if (lstWorkStations.SelectedItem != null)
            {
                AddSelectedWS(lstWorkStations.SelectedItem);
            }
        }

        private void lstWorkStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLocationForSelectedWS(lstWorkStations.SelectedItem as WorkStationData);
        }

        private void lstSelectedWS_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLocationForSelectedWS(lstSelectedWS.SelectedItem as WorkStationData);
        }

        private void cmbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_formLoaded)
            {
                LocationDTO locData = cmbLocations.SelectedItem as LocationDTO;
                if (locData != null)
                {
                    cmbAreaCode.DataSource = null;
                    cmbAreaCode.DataSource = _wsBLL.LstLocationArea.FindAll(x => x.LocationId == locData.LocationId);
                    cmbAreaCode.DisplayMember = "Area";
                    cmbAreaCode.ValueMember = "AreaId";
                    lblCompanyValue.Text = locData.CompanyName;
                    lblZoneValue.Text = locData.Zone;
                }
            }
        }

        private void cmbConfData_TextUpdate(object sender, EventArgs e)
        {
            if (_formLoaded)
            {
                lblCurrConfiguration.Text = cmbConfData.Text;
                btnSave.Enabled = true;

            }
        }

        private void btnSystemWideConf_Click(object sender, EventArgs e)
        {
            WorkStationDetails wsd = new WorkStationDetails(_wsBLL, true);

            if (wsd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddSystemConfigurationItem(wsd._key);
            }
        }

        #endregion

        #region private methods

        private void RemoveSelectedModuleItem(object selectedModuleItem)
        {
            _wsBLL.RemoveSelectedModule(selectedModuleItem);
            SetModulesList();
        }



        private void UpdateLocationForSelectedWS(WorkStationData wsdata)
        {
            if (_formLoaded && wsdata != null)
            {
                LocationDTO locData = _wsBLL.GetLocationDetailsForWS(wsdata);
                cmbLocations.SelectedItem = locData;
                if (Convert.ToString(wsdata.AreaId) != string.Empty)
                    cmbAreaCode.SelectedItem = _wsBLL.LstLocationArea.Find(x => x.LocationId == locData.LocationId && x.AreaId.ToString() == wsdata.AreaId);
            }
        }



        private void AddSystemConfigurationItem(string key, string value = null)
        {
            int xCordValue = grpSysConfItems.Controls[0].Location.X;

            Point locKey = grpSysConfItems.Controls[grpSysConfItems.Controls.Count - 1].Location;
            Point locValue = grpSysConfItems.Controls[grpSysConfItems.Controls.Count - 2].Location;

            TextBox txtValue = new TextBox();
            if (value != null)
            {
                txtValue.Text = value;
            }

            txtValue.Size = new System.Drawing.Size(_txtBoxHt, txtValue.Size.Height);
            txtValue.Location = new Point(xCordValue - _locationDelta, locValue.Y + _locationDelta);
            grpSysConfItems.Controls.Add(txtValue);

            TextBox txtKey = new TextBox();
            txtKey.Enabled = false;
            txtKey.Text = key;

            txtKey.Size = new System.Drawing.Size(_txtBoxWidth, txtKey.Size.Height);
            txtKey.Location = new Point(locKey.X, locKey.Y + _locationDelta);
            grpSysConfItems.Controls.Add(txtKey);

            
        }

        /// <summary>
        /// This method will load the conf items group box with the selected conf data.
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void AddSystemConfItemsFromConfData()
        {
            foreach (KeyValuePair<string, object> kvp in _wsBLL.GetAllSystemConfValues())
            {
                AddSystemConfigurationItem(kvp.Key, kvp.Value.ToString());
            }

        }

        /// <summary>
        /// This method will load the conf items group box with the selected conf data.
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void AddConfItemsFromConfData()
        {
            foreach (KeyValuePair<string, object> kvp in _wsBLL.GetAllWSConfValues())
            {
                AddConfigurationItem(kvp.Key, kvp.Value.ToString());
            }

        }

        /// <summary>
        /// Clears the conf items group box
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void ClearConfItems()
        {
            _wsBLL.ClearConfItems();

            if (grpBoxConfItems.Controls.Count == Constants.TWO)
            {
                return;
            }
            Control lblKey = grpBoxConfItems.Controls[0];
            Control lblVal = grpBoxConfItems.Controls[1];

            grpBoxConfItems.Controls.Clear();

            grpBoxConfItems.Controls.Add(lblKey);
            grpBoxConfItems.Controls.Add(lblVal);

        }

        /// <summary>
        /// Add a new configuration item in the conf item group box.
        /// </summary>
        /// <param name=key>string key </param>
        /// <param name=value>string value=null</param>
        /// <returns></returns>
        private void AddConfigurationItem(string key, string value = null)
        {
            int xCordValue = grpBoxConfItems.Controls[0].Location.X;
            xCordValue = xCordValue + 50;

            Point locKey = grpBoxConfItems.Controls[grpBoxConfItems.Controls.Count - 1].Location;
            Point locValue = grpBoxConfItems.Controls[grpBoxConfItems.Controls.Count - 2].Location;

            TextBox txtValue = new TextBox();
            if (value != null)
            {
                txtValue.Text = value;
            }

            txtValue.Size = new System.Drawing.Size(_txtConfBoxvalWidth, txtValue.Size.Height);
            txtValue.Location = new Point(xCordValue - _locationDelta, locValue.Y + _locationDelta);
            grpBoxConfItems.Controls.Add(txtValue);

            TextBox txtKey = new TextBox();
            txtKey.Enabled = false;
            txtKey.Text = key;

            txtKey.Size = new System.Drawing.Size(_txtConfBoxkeyWidth, txtKey.Size.Height);
            txtKey.Location = new Point(locKey.X, locKey.Y + _locationDelta);
            grpBoxConfItems.Controls.Add(txtKey);
        }

        private void SetLocationDataBox()
        {
            cmbLocations.DataSource = new BindingSource(_wsBLL.LstLocations, null);
            cmbLocations.DisplayMember = "LocationName";
            cmbLocations.ValueMember = "LocationId";
            cmbLocations.SelectedIndex = Constants.MINUSONE;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        /// 
        private void AddSelectedModule(object selectedModuleItem)
        {
            _wsBLL.AddSelectedModule(selectedModuleItem);
            SetModulesList();

        }


        private void AddSelectedWS(object selectedWSItem)
        {
            WorkStationData wsData = (WorkStationData)selectedWSItem;
            if (GlobalMessageBox.ShowMessageInRedFont(string.Format(Messages.WSLOCCHANGE_CONFRIM, wsData.WsName, wsData.ConfigName, wsData.LocationName, wsData.AreaCode, wsData.WsName), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                _wsBLL.AddSelectedWorkStation(selectedWSItem, cmbConfData.SelectedIndex);
                SetWorkStationList();
            }
        }

        private void RemoveSelectedWS(object selectedWSItem)
        {
            _wsBLL.RemoveSelectedWorkStation(selectedWSItem, cmbConfData.SelectedIndex);
            SetWorkStationList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        /// 

        private void SetSelectedModules()
        {
            if (_wsBLL.SelectedModuleDetails.Count == Constants.ZERO)
            {
                lstSelectedModules.DataSource = null;
            }
            else
            {
                lstSelectedModules.DataSource = new BindingSource(_wsBLL.SelectedModuleDetails, null);
                lstSelectedModules.DisplayMember = "Value";
                lstSelectedModules.ValueMember = "Key";
                lstSelectedModules.SelectedIndex = Constants.MINUSONE;
            }
        }

        private void SetUnSelectedModuleList()
        {
            if (_wsBLL.UnSelectedModuleDetails.Count == Constants.ZERO)
            {
                lstBoxModules.DataSource = null;
            }
            else
            {
                lstBoxModules.DataSource = null;
                this.lstBoxModules.DataSource = new BindingSource(_wsBLL.UnSelectedModuleDetails, null);
                lstBoxModules.DisplayMember = "Value";
                lstBoxModules.ValueMember = "Key";
                lstBoxModules.SelectedIndex = Constants.MINUSONE;
            }
        }

        private void SetModulesList()
        {
            SetSelectedModules();
            SetUnSelectedModuleList();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void SetUnSelectedWSNames()
        {

            lstWorkStations.DataSource = new BindingSource(_wsBLL.LstUnSelectedWS, null);
            lstWorkStations.DisplayMember = "WsName";
            lstWorkStations.ValueMember = "WsId";
            lstWorkStations.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void SetSelectedWSNames()
        {

            lstSelectedWS.DataSource = new BindingSource(_wsBLL.LstSelectedWS, null);
            lstSelectedWS.DisplayMember = "WsName";
            lstSelectedWS.ValueMember = "WsId";
            lstSelectedWS.SelectedIndex = -1;

        }

        private void SetWorkStationList()
        {
            SetSelectedWSNames();
            SetUnSelectedWSNames();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        private void SetConfComboBox()
        {
            cmbConfData.DataSource = new BindingSource(_wsBLL.LstWSConfData, null);
            cmbConfData.DisplayMember = "ConfName";
            cmbConfData.ValueMember = "ConfId";
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name=message> string message</param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private void ShowExceptionMessageToTheUser(string message, FloorSystemException ex)
        {

            if (GlobalMessageBox.Show(String.Format(Messages.EX_WS, message, Environment.NewLine), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                GlobalMessageBox.Show(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.InnerExceptionMessages), Constants.AlertType.Error, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }

        }

        private void SaveSystemConfigurationData()
        {
            if (grpSysConfItems.Controls.Count == 2)
            {
                GlobalMessageBox.Show(Messages.NOCONF_WS, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                return;
            }

            int cntrlIdx = 0;

            for (; cntrlIdx < grpSysConfItems.Controls.Count; cntrlIdx++)
            {
                Control cntrl = grpSysConfItems.Controls[cntrlIdx];
                if (cntrl is TextBox)
                {

                    string value = cntrl.Text;

                    ++cntrlIdx;
                    cntrl = grpSysConfItems.Controls[cntrlIdx];
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        GlobalMessageBox.Show(String.Format(Messages.EMPTYCONF_WS, cntrl.Text, Environment.NewLine), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        return;
                    }
                    _wsBLL.UpdateSystemConfFiled(cntrl.Text, value);
                }
            }

            try
            {
                _wsBLL.SaveSystemConfiguration();

                //event log myadamas 20190227
                EventLogDTO EventLog = new EventLogDTO();

                EventLog.CreatedBy = String.Empty;
                Constants.EventLog audAction = Constants.EventLog.Save;
                EventLog.EventType = Convert.ToInt32(audAction);
                EventLog.EventLogType = Constants.eventlogtype;
                var _screenName = "Work Station Configuration";
                var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());
                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
            catch (FloorSystemException ex)
            {
                ShowExceptionMessageToTheUser(Messages.FAILSAVE_WS, ex);
            }
        }

        private void SaveConfigurationData()
        {
            string confName = null;
            if (cmbConfData.SelectedValue == null)
            {
                confName = cmbConfData.Text;
            }
            else
            {
                confName = (cmbConfData.SelectedItem as WorkStationConfigurationData).ConfName;
            }


            if (String.IsNullOrEmpty(confName))
            {
                GlobalMessageBox.Show(Messages.CONFNAME_WS, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                return;
            }

            if (_wsBLL.SelectedModuleDetails.Count == 0)
            {
                GlobalMessageBox.Show(Messages.SELMOD_WS, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                return;
            }

            if (_wsBLL.LstSelectedWS.Count == 0)
            {
                // No work stations are being tied with this configuration.You want to save the configuration without associating with any work station?- old message
                if (GlobalMessageBox.Show(Messages.NOWSCONF_WS, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.NO)
                {
                    return;
                }

            }

            int cntrlIdx = 0;

            for (; cntrlIdx < grpBoxConfItems.Controls.Count; cntrlIdx++)
            {
                Control cntrl = grpBoxConfItems.Controls[cntrlIdx];
                if (cntrl is TextBox)
                {

                    string value = cntrl.Text;

                    ++cntrlIdx;
                    cntrl = grpBoxConfItems.Controls[cntrlIdx];
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        GlobalMessageBox.Show(String.Format(Messages.NOKEY_WS, cntrl.Text, Environment.NewLine), Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        return;
                    }

                    _wsBLL.UpdateWSConfFiled(cntrl.Text, value);
                }
            }

            try
            {
                _wsBLL.ApplyConfiguration(confName);

                //event log myadamas 20190227
                EventLogDTO EventLog = new EventLogDTO();

                EventLog.CreatedBy = String.Empty;
                Constants.EventLog audAction = Constants.EventLog.Save;
                EventLog.EventType = Convert.ToInt32(audAction);
                EventLog.EventLogType = Constants.eventlogtype;
                var _screenName = "Work Station Configuration";
                var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                _wsBLL.ClearWorkStations();
                _wsBLL.LoadWorkStationAndConfData();
                cmbConfData.DataSource = null;
                SetConfComboBox();
            }

            catch (FloorSystemException ex)
            {
                ShowExceptionMessageToTheUser(Messages.FAILEDSAVE_CONF_WS, ex);
                return;
            }
        }


        private void SaveWorkStationData()
        {

            if (_wsBLL.LstSelectedWS.Count == 0)
            {
                GlobalMessageBox.Show(Messages.NOWS_SAVE_WS, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                return;
            }


            //update the location of selected work stations
            LocationDTO location = cmbLocations.SelectedItem as LocationDTO;


            if (_wsBLL.VerifyLocationForSelectedWS(location, Convert.ToString(cmbAreaCode.SelectedValue)) == false)
            {
                GlobalMessageBox.Show(String.Format(Messages.LOC_EXISTS_WS, location.LocationName, Environment.NewLine), Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    _wsBLL.UpdateLocationInformation(location, Convert.ToString(cmbAreaCode.SelectedValue));
                    GlobalMessageBox.Show(Messages.WSSAVE_SUCCESS_WS, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
                catch (FloorSystemException ex)
                {
                    ShowExceptionMessageToTheUser(Messages.UPDATE_LOC_FAIL_WS, ex);
                    return;
                }
            }


            //user might have created a new configuration or updated an existing configuration. This needs to be saved now.
            if (!String.IsNullOrWhiteSpace(lblCurrConfiguration.Text))
            {
                bool bShouldSave = false;

                if (_wsBLL.IsExistingConfiguration(lblCurrConfiguration.Text) == false)
                {
                    if (GlobalMessageBox.Show(Messages.NEWCONF_SAVE_WS, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        bShouldSave = true;
                    }
                }
                else
                {
                    if (GlobalMessageBox.Show(String.Format(Messages.UPDATE_EXSTCONF_WS, lblCurrConfiguration.Text + "?"), Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        bShouldSave = true;
                    }
                }
                if (bShouldSave)
                {
                    SaveConfigurationData();
                    
                   
                }
            }
        }
        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmbConfData.Text = string.Empty;
            btnSave.Enabled = false;
            ClearConfItems();

        }

        private void WorkStationConfiguration_Closed(object sender, FormClosedEventArgs e)
        {
            CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
        }

        private void tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            btnSave.Visible = true;
            btnCancel.Visible = true;

            if (e.TabPageIndex == 1)
            {
                if (_wsBLL.LstWSConfData.FindAll(x => x.ConfName == cmbConfData.Text).Count == 0)
                {
                    _wsBLL.ClearWorkStations();
                    SetWorkStationList();
                }
            }
            if (e.TabPageIndex == 3)
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
                pnlEncrypt.Visible = false;               
            }
        }


        private string DecryptConnectionString(string EncryptedString)
        {
            string DecryptedString = string.Empty;
            try
            {
                DecryptedString = EncryptDecrypt.GetDecryptedString(EncryptedString, "hidden");
            }
            catch (FloorSystemException ex)
            {
                ShowExceptionMessageToTheUser(Messages.GETDECRYPTEDSTRINGMETHODEXCEPTION, ex);
            }
            return DecryptedString;
        }


        public void SaveOrUpdateAppConfig(string con)
        {
            try
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    GlobalMessageBox.Show(Messages.DEPLOYED_CONNECTIONSTRING +"\n"+lblEncryptConnectionString.Text, Constants.AlertType.Exclamation, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
                else
                {
                    string ProjectName = Application.ProductName;
                    var MainDirectories = Directory.GetParent(Directory.GetCurrentDirectory()).GetDirectories();
                    string SolutionName = string.Empty;
                    bool flag = false;
                    foreach (var MainDirectory in MainDirectories)
                    {
                        var SubDirectories = MainDirectory.GetDirectories();
                        foreach (var SubDirectory in SubDirectories)
                        {
                            if (SubDirectory.ToString() == ProjectName)
                            {
                                SolutionName = MainDirectory.ToString();
                                flag = true;
                                break;
                            }
                        }
                        if (flag == true)
                        {
                            break;
                        }
                    }
                    XmlDocument XmlDoc = new XmlDocument();
                    if (ConfigurationManager.AppSettings["Identifier"] == "Debug")
                    {
                        XmlDoc.Load("..\\" + SolutionName + "\\" + ProjectName + "\\App." + ConfigurationManager.AppSettings["Identifier"] + ".config");
                    }
                    else
                    {
                        XmlDoc.Load("..\\..\\App." + ConfigurationManager.AppSettings["Identifier"] + ".config");
                    }
                    foreach (XmlElement xElement in XmlDoc.DocumentElement)
                    {
                        if (xElement.Name == "connectionStrings")
                        {
                            xElement.FirstChild.Attributes[1].Value = con;
                            break;
                        }
                    }
                    if (ConfigurationManager.AppSettings["Identifier"] == "Debug")
                    {
                        XmlDoc.Save("..\\" + SolutionName + "\\" + ProjectName + "\\App." + ConfigurationManager.AppSettings["Identifier"] + ".config");
                    }
                    else
                    {
                        XmlDoc.Save("..\\..\\App." + ConfigurationManager.AppSettings["Identifier"] + ".config");
                    }
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY_CONNECTIONSTRING, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.SAVEORUPDATEAPPCONFIGMETHODEXCEPTION, Constants.SAVEORUPDATEAPPCONFIG, ex);
            }
        }

        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            if (Flag == Constants.ENCRYPT)
            {
                validationMesssageLst.Add(new ValidationMessage(txtConnectionString, Constants.CONNECTIONSTRING, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            }
            else
            {
                validationMesssageLst.Add(new ValidationMessage(txtConnectionString, Constants.ENCRYPTEDCONNECTIONSTRING, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            }
            return ValidateForm();
        }

        private void txtConnectionString_Leave(object sender, EventArgs e)
        {          
            if (ValidateRequiredFields() == true)
            {
                try
                {
                    if (Flag == Constants.ENCRYPT)
                    {
                        string ConfirmationConnectionString = GlobalMessageBox.Show(Messages.CONFIRM_CONNECTIONSTRING, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                        txtEncryptConnectionString.Text = EncryptDecrypt.GetEncryptedString(txtConnectionString.Text.Trim(), "hidden");
                        if (ConfirmationConnectionString == Constants.YES)
                        {
                            if (DecryptedConnectionString != txtConnectionString.Text)
                            {
                                SaveOrUpdateAppConfig(txtEncryptConnectionString.Text.Trim());
                            }
                        }
                    }
                    else
                    {
                        txtEncryptConnectionString.Text = DecryptConnectionString(txtConnectionString.Text.Trim());
                    }
                   
                }
                catch (FloorSystemException ex)
                {
                    ShowExceptionMessageToTheUser(Messages.GETENCRYPTEDSTRINGMETHODEXCEPTION, ex);
                    return;
                }
            }
            else
            {
                txtEncryptConnectionString.Text = string.Empty;
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            pnlEncrypt.Visible = true;
            lblConnectionString.Text = Constants.CONNECTIONSTRING;
            lblEncryptConnectionString.Text = Constants.ENCRYPTEDCONNECTIONSTRING;
            Flag = Constants.ENCRYPT;
            txtConnectionString.Text = DecryptedConnectionString;
            txtEncryptConnectionString.Text = string.Empty;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            pnlEncrypt.Visible = true;
            lblConnectionString.Text =Constants.ENCRYPTEDCONNECTIONSTRING;
            lblEncryptConnectionString.Text = Constants.DECRYPTEDCONNECTIONSTRING;
            Flag = Constants.DECRYPT;
            txtConnectionString.Text = ConfigurationManager.ConnectionStrings["FSDB"].ToString();
            txtEncryptConnectionString.Text = string.Empty;
        }
    }
}
