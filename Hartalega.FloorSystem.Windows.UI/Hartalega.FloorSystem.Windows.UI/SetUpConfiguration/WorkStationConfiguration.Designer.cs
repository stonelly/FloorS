namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class WorkStationConfiguration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkStationConfiguration));
            this.cmbConfData = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpBoxConfItems = new System.Windows.Forms.GroupBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.btnAddKey = new System.Windows.Forms.Button();
            this.lstBoxModules = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lstSelectedModules = new System.Windows.Forms.ListBox();
            this.btnMoveSelected = new System.Windows.Forms.Button();
            this.btnMoveUnSelected = new System.Windows.Forms.Button();
            this.grpModules = new System.Windows.Forms.GroupBox();
            this.grpConfItems = new System.Windows.Forms.GroupBox();
            this.lstWorkStations = new System.Windows.Forms.ListBox();
            this.btnMoveWSToSel = new System.Windows.Forms.Button();
            this.btnMoveWSToUnSel = new System.Windows.Forms.Button();
            this.lstSelectedWS = new System.Windows.Forms.ListBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabConfiguration = new System.Windows.Forms.TabPage();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tabWorkStations = new System.Windows.Forms.TabPage();
            this.lblCurrConfiguration = new System.Windows.Forms.Label();
            this.lblCurrentConfigurationLabel = new System.Windows.Forms.Label();
            this.grpWorkStations = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbAreaCode = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblZoneValue = new System.Windows.Forms.Label();
            this.lblZone = new System.Windows.Forms.Label();
            this.lblArea = new System.Windows.Forms.Label();
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblCompanyValue = new System.Windows.Forms.Label();
            this.tabSystemConf = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpSysConfItems = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSystemWideConf = new System.Windows.Forms.Button();
            this.tabConStrEncryption = new System.Windows.Forms.TabPage();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.pnlEncrypt = new System.Windows.Forms.Panel();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.txtEncryptConnectionString = new System.Windows.Forms.TextBox();
            this.lblEncryptConnectionString = new System.Windows.Forms.Label();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.grpBoxConfItems.SuspendLayout();
            this.grpModules.SuspendLayout();
            this.grpConfItems.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.tabWorkStations.SuspendLayout();
            this.grpWorkStations.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabSystemConf.SuspendLayout();
            this.panel2.SuspendLayout();
            this.grpSysConfItems.SuspendLayout();
            this.tabConStrEncryption.SuspendLayout();
            this.pnlEncrypt.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbConfData
            // 
            this.cmbConfData.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbConfData.FormattingEnabled = true;
            this.cmbConfData.Location = new System.Drawing.Point(6, 32);
            this.cmbConfData.Name = "cmbConfData";
            this.cmbConfData.Size = new System.Drawing.Size(228, 26);
            this.cmbConfData.TabIndex = 0;
            this.cmbConfData.SelectedIndexChanged += new System.EventHandler(this.cmbConf_SelectedIndexChanged);
            this.cmbConfData.TextUpdate += new System.EventHandler(this.cmbConfData_TextUpdate);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.grpBoxConfItems);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(582, 112);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 405);
            this.panel1.TabIndex = 4;
            // 
            // grpBoxConfItems
            // 
            this.grpBoxConfItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxConfItems.AutoSize = true;
            this.grpBoxConfItems.Controls.Add(this.lblValue);
            this.grpBoxConfItems.Controls.Add(this.lblKey);
            this.grpBoxConfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxConfItems.Location = new System.Drawing.Point(17, 16);
            this.grpBoxConfItems.Name = "grpBoxConfItems";
            this.grpBoxConfItems.Size = new System.Drawing.Size(336, 372);
            this.grpBoxConfItems.TabIndex = 0;
            this.grpBoxConfItems.TabStop = false;
            this.grpBoxConfItems.Text = "Configuration Items";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.Location = new System.Drawing.Point(270, 34);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(49, 18);
            this.lblValue.TabIndex = 1;
            this.lblValue.Text = "Value";
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKey.Location = new System.Drawing.Point(49, 34);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(36, 18);
            this.lblKey.TabIndex = 0;
            this.lblKey.Text = "Key";
            // 
            // btnAddKey
            // 
            this.btnAddKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddKey.Location = new System.Drawing.Point(727, 524);
            this.btnAddKey.Name = "btnAddKey";
            this.btnAddKey.Size = new System.Drawing.Size(130, 39);
            this.btnAddKey.TabIndex = 5;
            this.btnAddKey.Text = "Add Key/Value";
            this.btnAddKey.UseVisualStyleBackColor = true;
            this.btnAddKey.Click += new System.EventHandler(this.btnAddKey_Click);
            // 
            // lstBoxModules
            // 
            this.lstBoxModules.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxModules.FormattingEnabled = true;
            this.lstBoxModules.HorizontalScrollbar = true;
            this.lstBoxModules.ItemHeight = 18;
            this.lstBoxModules.Location = new System.Drawing.Point(6, 31);
            this.lstBoxModules.Name = "lstBoxModules";
            this.lstBoxModules.Size = new System.Drawing.Size(250, 292);
            this.lstBoxModules.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(391, 622);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(164, 39);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save Configuration";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lstSelectedModules
            // 
            this.lstSelectedModules.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSelectedModules.FormattingEnabled = true;
            this.lstSelectedModules.HorizontalScrollbar = true;
            this.lstSelectedModules.ItemHeight = 18;
            this.lstSelectedModules.Location = new System.Drawing.Point(295, 31);
            this.lstSelectedModules.Name = "lstSelectedModules";
            this.lstSelectedModules.Size = new System.Drawing.Size(250, 292);
            this.lstSelectedModules.TabIndex = 2;
            // 
            // btnMoveSelected
            // 
            this.btnMoveSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveSelected.Location = new System.Drawing.Point(261, 91);
            this.btnMoveSelected.Name = "btnMoveSelected";
            this.btnMoveSelected.Size = new System.Drawing.Size(28, 33);
            this.btnMoveSelected.TabIndex = 1;
            this.btnMoveSelected.Text = ">";
            this.btnMoveSelected.UseVisualStyleBackColor = true;
            this.btnMoveSelected.Click += new System.EventHandler(this.btnMoveSelected_Click);
            // 
            // btnMoveUnSelected
            // 
            this.btnMoveUnSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUnSelected.Location = new System.Drawing.Point(261, 233);
            this.btnMoveUnSelected.Name = "btnMoveUnSelected";
            this.btnMoveUnSelected.Size = new System.Drawing.Size(28, 32);
            this.btnMoveUnSelected.TabIndex = 3;
            this.btnMoveUnSelected.Text = "<";
            this.btnMoveUnSelected.UseVisualStyleBackColor = true;
            this.btnMoveUnSelected.Click += new System.EventHandler(this.btnMoveUnSelected_Click);
            // 
            // grpModules
            // 
            this.grpModules.Controls.Add(this.lstBoxModules);
            this.grpModules.Controls.Add(this.lstSelectedModules);
            this.grpModules.Controls.Add(this.btnMoveUnSelected);
            this.grpModules.Controls.Add(this.btnMoveSelected);
            this.grpModules.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpModules.Location = new System.Drawing.Point(19, 128);
            this.grpModules.Name = "grpModules";
            this.grpModules.Size = new System.Drawing.Size(557, 372);
            this.grpModules.TabIndex = 3;
            this.grpModules.TabStop = false;
            this.grpModules.Text = "Modules";
            // 
            // grpConfItems
            // 
            this.grpConfItems.Controls.Add(this.cmbConfData);
            this.grpConfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpConfItems.Location = new System.Drawing.Point(13, 13);
            this.grpConfItems.Name = "grpConfItems";
            this.grpConfItems.Size = new System.Drawing.Size(250, 83);
            this.grpConfItems.TabIndex = 1;
            this.grpConfItems.TabStop = false;
            this.grpConfItems.Text = "Configurations";
            // 
            // lstWorkStations
            // 
            this.lstWorkStations.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstWorkStations.FormattingEnabled = true;
            this.lstWorkStations.HorizontalScrollbar = true;
            this.lstWorkStations.ItemHeight = 18;
            this.lstWorkStations.Location = new System.Drawing.Point(28, 120);
            this.lstWorkStations.Name = "lstWorkStations";
            this.lstWorkStations.Size = new System.Drawing.Size(366, 292);
            this.lstWorkStations.TabIndex = 0;
            this.lstWorkStations.SelectedIndexChanged += new System.EventHandler(this.lstWorkStations_SelectedIndexChanged);
            // 
            // btnMoveWSToSel
            // 
            this.btnMoveWSToSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveWSToSel.Location = new System.Drawing.Point(420, 158);
            this.btnMoveWSToSel.Name = "btnMoveWSToSel";
            this.btnMoveWSToSel.Size = new System.Drawing.Size(75, 31);
            this.btnMoveWSToSel.TabIndex = 1;
            this.btnMoveWSToSel.Text = ">";
            this.btnMoveWSToSel.UseVisualStyleBackColor = true;
            this.btnMoveWSToSel.Click += new System.EventHandler(this.btnMoveWSToSel_Click);
            // 
            // btnMoveWSToUnSel
            // 
            this.btnMoveWSToUnSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveWSToUnSel.Location = new System.Drawing.Point(420, 278);
            this.btnMoveWSToUnSel.Name = "btnMoveWSToUnSel";
            this.btnMoveWSToUnSel.Size = new System.Drawing.Size(75, 33);
            this.btnMoveWSToUnSel.TabIndex = 3;
            this.btnMoveWSToUnSel.Text = "<";
            this.btnMoveWSToUnSel.UseVisualStyleBackColor = true;
            this.btnMoveWSToUnSel.Click += new System.EventHandler(this.btnMoveWSToUnSel_Click);
            // 
            // lstSelectedWS
            // 
            this.lstSelectedWS.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSelectedWS.FormattingEnabled = true;
            this.lstSelectedWS.HorizontalScrollbar = true;
            this.lstSelectedWS.ItemHeight = 18;
            this.lstSelectedWS.Location = new System.Drawing.Point(546, 120);
            this.lstSelectedWS.Name = "lstSelectedWS";
            this.lstSelectedWS.Size = new System.Drawing.Size(366, 292);
            this.lstSelectedWS.TabIndex = 2;
            this.lstSelectedWS.SelectedIndexChanged += new System.EventHandler(this.lstSelectedWS_SelectedIndexChanged);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabConfiguration);
            this.tabs.Controls.Add(this.tabWorkStations);
            this.tabs.Controls.Add(this.tabSystemConf);
            this.tabs.Controls.Add(this.tabConStrEncryption);
            this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(974, 600);
            this.tabs.TabIndex = 0;
            this.tabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabs_Selecting);
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabConfiguration.Controls.Add(this.btnAdd);
            this.tabConfiguration.Controls.Add(this.btnAddKey);
            this.tabConfiguration.Controls.Add(this.grpConfItems);
            this.tabConfiguration.Controls.Add(this.grpModules);
            this.tabConfiguration.Controls.Add(this.panel1);
            this.tabConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabConfiguration.Location = new System.Drawing.Point(4, 27);
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfiguration.Size = new System.Drawing.Size(966, 569);
            this.tabConfiguration.TabIndex = 0;
            this.tabConfiguration.Text = "Configuration";
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(280, 43);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(130, 39);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tabWorkStations
            // 
            this.tabWorkStations.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabWorkStations.Controls.Add(this.lblCurrConfiguration);
            this.tabWorkStations.Controls.Add(this.lblCurrentConfigurationLabel);
            this.tabWorkStations.Controls.Add(this.grpWorkStations);
            this.tabWorkStations.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabWorkStations.Location = new System.Drawing.Point(4, 27);
            this.tabWorkStations.Name = "tabWorkStations";
            this.tabWorkStations.Padding = new System.Windows.Forms.Padding(3);
            this.tabWorkStations.Size = new System.Drawing.Size(966, 569);
            this.tabWorkStations.TabIndex = 1;
            this.tabWorkStations.Text = "WorkStations";
            // 
            // lblCurrConfiguration
            // 
            this.lblCurrConfiguration.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCurrConfiguration.AutoSize = true;
            this.lblCurrConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrConfiguration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblCurrConfiguration.Location = new System.Drawing.Point(204, 16);
            this.lblCurrConfiguration.Name = "lblCurrConfiguration";
            this.lblCurrConfiguration.Size = new System.Drawing.Size(170, 18);
            this.lblCurrConfiguration.TabIndex = 22;
            this.lblCurrConfiguration.Text = "Current Configuration";
            // 
            // lblCurrentConfigurationLabel
            // 
            this.lblCurrentConfigurationLabel.AutoSize = true;
            this.lblCurrentConfigurationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentConfigurationLabel.Location = new System.Drawing.Point(23, 16);
            this.lblCurrentConfigurationLabel.Name = "lblCurrentConfigurationLabel";
            this.lblCurrentConfigurationLabel.Size = new System.Drawing.Size(175, 18);
            this.lblCurrentConfigurationLabel.TabIndex = 21;
            this.lblCurrentConfigurationLabel.Text = "Current Configuration:";
            // 
            // grpWorkStations
            // 
            this.grpWorkStations.Controls.Add(this.tableLayoutPanel1);
            this.grpWorkStations.Controls.Add(this.lstSelectedWS);
            this.grpWorkStations.Controls.Add(this.btnMoveWSToUnSel);
            this.grpWorkStations.Controls.Add(this.btnMoveWSToSel);
            this.grpWorkStations.Controls.Add(this.lstWorkStations);
            this.grpWorkStations.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpWorkStations.Location = new System.Drawing.Point(23, 49);
            this.grpWorkStations.Name = "grpWorkStations";
            this.grpWorkStations.Size = new System.Drawing.Size(929, 497);
            this.grpWorkStations.TabIndex = 1;
            this.grpWorkStations.TabStop = false;
            this.grpWorkStations.Text = "Work Stations";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.99776F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.76596F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.34938F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.622621F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.09406F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.742441F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.88578F));
            this.tableLayoutPanel1.Controls.Add(this.cmbAreaCode, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLocation, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblZoneValue, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblZone, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblArea, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbLocations, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCompany, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCompanyValue, 3, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(28, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(893, 62);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // cmbAreaCode
            // 
            this.cmbAreaCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbAreaCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAreaCode.FormattingEnabled = true;
            this.cmbAreaCode.Location = new System.Drawing.Point(576, 20);
            this.cmbAreaCode.Name = "cmbAreaCode";
            this.cmbAreaCode.Size = new System.Drawing.Size(102, 26);
            this.cmbAreaCode.TabIndex = 23;
            // 
            // lblLocation
            // 
            this.lblLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(57, 22);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 18);
            this.lblLocation.TabIndex = 2;
            this.lblLocation.Text = "Plant:";
            // 
            // lblZoneValue
            // 
            this.lblZoneValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblZoneValue.AutoSize = true;
            this.lblZoneValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoneValue.ForeColor = System.Drawing.Color.Black;
            this.lblZoneValue.Location = new System.Drawing.Point(771, 22);
            this.lblZoneValue.Name = "lblZoneValue";
            this.lblZoneValue.Size = new System.Drawing.Size(92, 18);
            this.lblZoneValue.TabIndex = 3;
            this.lblZoneValue.Text = "Zone Value";
            this.lblZoneValue.Visible = false;
            // 
            // lblZone
            // 
            this.lblZone.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblZone.AutoSize = true;
            this.lblZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZone.Location = new System.Drawing.Point(714, 22);
            this.lblZone.Name = "lblZone";
            this.lblZone.Size = new System.Drawing.Size(51, 18);
            this.lblZone.TabIndex = 7;
            this.lblZone.Text = "Zone:";
            this.lblZone.Visible = false;
            // 
            // lblArea
            // 
            this.lblArea.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblArea.AutoSize = true;
            this.lblArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArea.Location = new System.Drawing.Point(523, 22);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(47, 18);
            this.lblArea.TabIndex = 3;
            this.lblArea.Text = "Area:";
            // 
            // cmbLocations
            // 
            this.cmbLocations.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLocations.FormattingEnabled = true;
            this.cmbLocations.Location = new System.Drawing.Point(114, 18);
            this.cmbLocations.Name = "cmbLocations";
            this.cmbLocations.Size = new System.Drawing.Size(119, 26);
            this.cmbLocations.TabIndex = 0;
            this.cmbLocations.SelectedIndexChanged += new System.EventHandler(this.cmbLocations_SelectedIndexChanged);
            // 
            // lblCompany
            // 
            this.lblCompany.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCompany.AutoSize = true;
            this.lblCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompany.Location = new System.Drawing.Point(263, 22);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(84, 18);
            this.lblCompany.TabIndex = 4;
            this.lblCompany.Text = "Company:";
            this.lblCompany.Visible = false;
            // 
            // lblCompanyValue
            // 
            this.lblCompanyValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCompanyValue.AutoSize = true;
            this.lblCompanyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyValue.ForeColor = System.Drawing.Color.Black;
            this.lblCompanyValue.Location = new System.Drawing.Point(353, 22);
            this.lblCompanyValue.Name = "lblCompanyValue";
            this.lblCompanyValue.Size = new System.Drawing.Size(125, 18);
            this.lblCompanyValue.TabIndex = 1;
            this.lblCompanyValue.Text = "Company Value";
            this.lblCompanyValue.Visible = false;
            // 
            // tabSystemConf
            // 
            this.tabSystemConf.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabSystemConf.Controls.Add(this.panel2);
            this.tabSystemConf.Controls.Add(this.btnSystemWideConf);
            this.tabSystemConf.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSystemConf.Location = new System.Drawing.Point(4, 27);
            this.tabSystemConf.Name = "tabSystemConf";
            this.tabSystemConf.Size = new System.Drawing.Size(966, 569);
            this.tabSystemConf.TabIndex = 2;
            this.tabSystemConf.Text = "System Wide Configuration";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.grpSysConfItems);
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(35, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(753, 405);
            this.panel2.TabIndex = 10;
            // 
            // grpSysConfItems
            // 
            this.grpSysConfItems.AutoSize = true;
            this.grpSysConfItems.Controls.Add(this.label1);
            this.grpSysConfItems.Controls.Add(this.label2);
            this.grpSysConfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSysConfItems.Location = new System.Drawing.Point(29, 16);
            this.grpSysConfItems.Name = "grpSysConfItems";
            this.grpSysConfItems.Size = new System.Drawing.Size(692, 347);
            this.grpSysConfItems.TabIndex = 6;
            this.grpSysConfItems.TabStop = false;
            this.grpSysConfItems.Text = "Configuration Items";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(499, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Value";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(70, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Key";
            // 
            // btnSystemWideConf
            // 
            this.btnSystemWideConf.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSystemWideConf.Location = new System.Drawing.Point(322, 447);
            this.btnSystemWideConf.Name = "btnSystemWideConf";
            this.btnSystemWideConf.Size = new System.Drawing.Size(130, 39);
            this.btnSystemWideConf.TabIndex = 12;
            this.btnSystemWideConf.Text = "Add Key/Value";
            this.btnSystemWideConf.UseVisualStyleBackColor = true;
            this.btnSystemWideConf.Visible = false;
            this.btnSystemWideConf.Click += new System.EventHandler(this.btnSystemWideConf_Click);
            // 
            // tabConStrEncryption
            // 
            this.tabConStrEncryption.Controls.Add(this.btnDecrypt);
            this.tabConStrEncryption.Controls.Add(this.pnlEncrypt);
            this.tabConStrEncryption.Controls.Add(this.btnEncrypt);
            this.tabConStrEncryption.Location = new System.Drawing.Point(4, 27);
            this.tabConStrEncryption.Name = "tabConStrEncryption";
            this.tabConStrEncryption.Size = new System.Drawing.Size(966, 569);
            this.tabConStrEncryption.TabIndex = 3;
            this.tabConStrEncryption.Text = "Connection String Encryption";
            this.tabConStrEncryption.UseVisualStyleBackColor = true;
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecrypt.Location = new System.Drawing.Point(469, 22);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(130, 39);
            this.btnDecrypt.TabIndex = 8;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // pnlEncrypt
            // 
            this.pnlEncrypt.Controls.Add(this.lblConnectionString);
            this.pnlEncrypt.Controls.Add(this.txtConnectionString);
            this.pnlEncrypt.Controls.Add(this.txtEncryptConnectionString);
            this.pnlEncrypt.Controls.Add(this.lblEncryptConnectionString);
            this.pnlEncrypt.Location = new System.Drawing.Point(8, 69);
            this.pnlEncrypt.Name = "pnlEncrypt";
            this.pnlEncrypt.Size = new System.Drawing.Size(955, 175);
            this.pnlEncrypt.TabIndex = 7;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblConnectionString.Location = new System.Drawing.Point(3, 18);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(153, 18);
            this.lblConnectionString.TabIndex = 4;
            this.lblConnectionString.Text = "Connection String :";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtConnectionString.Location = new System.Drawing.Point(242, 18);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(710, 24);
            this.txtConnectionString.TabIndex = 2;
            this.txtConnectionString.TabStop = false;
            this.txtConnectionString.Leave += new System.EventHandler(this.txtConnectionString_Leave);
            // 
            // txtEncryptConnectionString
            // 
            this.txtEncryptConnectionString.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtEncryptConnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.txtEncryptConnectionString.Location = new System.Drawing.Point(242, 58);
            this.txtEncryptConnectionString.Name = "txtEncryptConnectionString";
            this.txtEncryptConnectionString.ReadOnly = true;
            this.txtEncryptConnectionString.Size = new System.Drawing.Size(710, 24);
            this.txtEncryptConnectionString.TabIndex = 3;
            this.txtEncryptConnectionString.TabStop = false;
            // 
            // lblEncryptConnectionString
            // 
            this.lblEncryptConnectionString.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEncryptConnectionString.AutoSize = true;
            this.lblEncryptConnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblEncryptConnectionString.Location = new System.Drawing.Point(3, 58);
            this.lblEncryptConnectionString.Name = "lblEncryptConnectionString";
            this.lblEncryptConnectionString.Size = new System.Drawing.Size(233, 18);
            this.lblEncryptConnectionString.TabIndex = 5;
            this.lblEncryptConnectionString.Text = "Encrypted Connection String :";
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEncrypt.Location = new System.Drawing.Point(333, 22);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(130, 39);
            this.btnEncrypt.TabIndex = 6;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(585, 622);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // WorkStationConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 655);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 640);
            this.Name = "WorkStationConfiguration";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WorkStation Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WorkStationConfiguration_Closed);
            this.Load += new System.EventHandler(this.WorkStationConfiguration_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpBoxConfItems.ResumeLayout(false);
            this.grpBoxConfItems.PerformLayout();
            this.grpModules.ResumeLayout(false);
            this.grpConfItems.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabConfiguration.ResumeLayout(false);
            this.tabWorkStations.ResumeLayout(false);
            this.tabWorkStations.PerformLayout();
            this.grpWorkStations.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabSystemConf.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.grpSysConfItems.ResumeLayout(false);
            this.grpSysConfItems.PerformLayout();
            this.tabConStrEncryption.ResumeLayout(false);
            this.pnlEncrypt.ResumeLayout(false);
            this.pnlEncrypt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConfData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpBoxConfItems;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.ListBox lstBoxModules;
        private System.Windows.Forms.Button btnAddKey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lstSelectedModules;
        private System.Windows.Forms.Button btnMoveSelected;
        private System.Windows.Forms.Button btnMoveUnSelected;
        private System.Windows.Forms.GroupBox grpModules;
        private System.Windows.Forms.GroupBox grpConfItems;
        private System.Windows.Forms.ListBox lstSelectedWS;
        private System.Windows.Forms.Button btnMoveWSToUnSel;
        private System.Windows.Forms.Button btnMoveWSToSel;
        private System.Windows.Forms.ListBox lstWorkStations;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabConfiguration;
        private System.Windows.Forms.TabPage tabWorkStations;
        private System.Windows.Forms.GroupBox grpWorkStations;
        private System.Windows.Forms.TabPage tabSystemConf;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox grpSysConfItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSystemWideConf;
        private System.Windows.Forms.Label lblCurrConfiguration;
        private System.Windows.Forms.Label lblCurrentConfigurationLabel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cmbAreaCode;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblZoneValue;
        private System.Windows.Forms.Label lblZone;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.ComboBox cmbLocations;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Label lblCompanyValue;
        private System.Windows.Forms.TabPage tabConStrEncryption;
        private System.Windows.Forms.TextBox txtEncryptConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblEncryptConnectionString;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Panel pnlEncrypt;
        private System.Windows.Forms.Button btnEncrypt;

    }
}