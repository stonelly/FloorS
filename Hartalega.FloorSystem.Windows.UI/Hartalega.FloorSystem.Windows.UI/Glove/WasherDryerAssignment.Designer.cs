namespace Hartalega.FloorSystem.Windows.UI.Glove
{
    partial class WasherDryerAssignment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbCond1 = new System.Windows.Forms.ComboBox();
            this.tbGloveCode = new System.Windows.Forms.TextBox();
            this.tbGloveCategory = new System.Windows.Forms.TextBox();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.cbCond2 = new System.Windows.Forms.ComboBox();
            this.grdGloveDetails = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGloveCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGloveCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.pbWasherAdd = new System.Windows.Forms.ToolStripButton();
            this.pbWasherEdit = new System.Windows.Forms.ToolStripButton();
            this.pbWasherDelete = new System.Windows.Forms.ToolStripButton();
            this.label6 = new System.Windows.Forms.Label();
            this.gridWasher = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WasherId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.pbDryerAdd = new System.Windows.Forms.ToolStripButton();
            this.pbDryerEdit = new System.Windows.Forms.ToolStripButton();
            this.pbDryerDelete = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.gridDryer = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DryerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdGloveDetails)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridWasher)).BeginInit();
            this.panel3.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDryer)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(487, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbCond1
            // 
            this.cbCond1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCond1.FormattingEnabled = true;
            this.cbCond1.Items.AddRange(new object[] {
            "OR",
            "AND"});
            this.cbCond1.Location = new System.Drawing.Point(137, 12);
            this.cbCond1.Name = "cbCond1";
            this.cbCond1.Size = new System.Drawing.Size(47, 21);
            this.cbCond1.TabIndex = 15;
            this.cbCond1.Text = "OR";
            // 
            // tbGloveCode
            // 
            this.tbGloveCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGloveCode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbGloveCode.Location = new System.Drawing.Point(7, 14);
            this.tbGloveCode.Name = "tbGloveCode";
            this.tbGloveCode.Size = new System.Drawing.Size(124, 20);
            this.tbGloveCode.TabIndex = 20;
            this.tbGloveCode.Text = "Glove Code";
            this.tbGloveCode.Enter += new System.EventHandler(this.tbGloveCode_Enter);
            this.tbGloveCode.Leave += new System.EventHandler(this.tbGloveCode_Leave);
            // 
            // tbGloveCategory
            // 
            this.tbGloveCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGloveCategory.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbGloveCategory.Location = new System.Drawing.Point(360, 14);
            this.tbGloveCategory.Name = "tbGloveCategory";
            this.tbGloveCategory.Size = new System.Drawing.Size(120, 20);
            this.tbGloveCategory.TabIndex = 21;
            this.tbGloveCategory.Text = "Glove Category";
            this.tbGloveCategory.Enter += new System.EventHandler(this.tbGloveCategory_Enter);
            this.tbGloveCategory.Leave += new System.EventHandler(this.tbGloveCategory_Leave);
            // 
            // tbBarcode
            // 
            this.tbBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBarcode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbBarcode.Location = new System.Drawing.Point(190, 14);
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Size = new System.Drawing.Size(106, 20);
            this.tbBarcode.TabIndex = 22;
            this.tbBarcode.Text = "Barcode";
            this.tbBarcode.Enter += new System.EventHandler(this.tbBarcode_Enter);
            this.tbBarcode.Leave += new System.EventHandler(this.tbBarcode_Leave);
            // 
            // cbCond2
            // 
            this.cbCond2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCond2.FormattingEnabled = true;
            this.cbCond2.Items.AddRange(new object[] {
            "OR",
            "AND"});
            this.cbCond2.Location = new System.Drawing.Point(302, 12);
            this.cbCond2.Name = "cbCond2";
            this.cbCond2.Size = new System.Drawing.Size(49, 21);
            this.cbCond2.TabIndex = 24;
            this.cbCond2.Text = "OR";
            // 
            // grdGloveDetails
            // 
            this.grdGloveDetails.AllowUserToAddRows = false;
            this.grdGloveDetails.AllowUserToDeleteRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdGloveDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.grdGloveDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdGloveDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdGloveDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.colGloveCode,
            this.colDescription,
            this.colBarcode,
            this.colGloveCategory});
            this.grdGloveDetails.Location = new System.Drawing.Point(15, 52);
            this.grdGloveDetails.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grdGloveDetails.MultiSelect = false;
            this.grdGloveDetails.Name = "grdGloveDetails";
            this.grdGloveDetails.ReadOnly = true;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdGloveDetails.RowsDefaultCellStyle = dataGridViewCellStyle14;
            this.grdGloveDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdGloveDetails.Size = new System.Drawing.Size(656, 646);
            this.grdGloveDetails.TabIndex = 26;
            this.grdGloveDetails.SelectionChanged += new System.EventHandler(this.grdGloveDetails_SelectionChanged);
            // 
            // No
            // 
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.No.DefaultCellStyle = dataGridViewCellStyle11;
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 50;
            // 
            // colGloveCode
            // 
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colGloveCode.DefaultCellStyle = dataGridViewCellStyle12;
            this.colGloveCode.HeaderText = "Glove Code";
            this.colGloveCode.Name = "colGloveCode";
            this.colGloveCode.ReadOnly = true;
            this.colGloveCode.Width = 150;
            // 
            // colDescription
            // 
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colDescription.DefaultCellStyle = dataGridViewCellStyle13;
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.Width = 300;
            // 
            // colBarcode
            // 
            this.colBarcode.HeaderText = "Barcode";
            this.colBarcode.Name = "colBarcode";
            this.colBarcode.ReadOnly = true;
            this.colBarcode.Width = 80;
            // 
            // colGloveCategory
            // 
            this.colGloveCategory.HeaderText = "Glove Category";
            this.colGloveCategory.Name = "colGloveCategory";
            this.colGloveCategory.ReadOnly = true;
            this.colGloveCategory.Width = 70;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.toolStrip2);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.gridWasher);
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(800, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(380, 309);
            this.panel2.TabIndex = 35;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbWasherAdd,
            this.pbWasherEdit,
            this.pbWasherDelete});
            this.toolStrip2.Location = new System.Drawing.Point(288, 8);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.ShowItemToolTips = false;
            this.toolStrip2.Size = new System.Drawing.Size(81, 25);
            this.toolStrip2.TabIndex = 36;
            this.toolStrip2.TabStop = true;
            // 
            // pbWasherAdd
            // 
            this.pbWasherAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbWasherAdd.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.pbWasherAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbWasherAdd.Name = "pbWasherAdd";
            this.pbWasherAdd.Size = new System.Drawing.Size(23, 22);
            this.pbWasherAdd.Click += new System.EventHandler(this.pbWasherAdd_Click);
            // 
            // pbWasherEdit
            // 
            this.pbWasherEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbWasherEdit.Enabled = false;
            this.pbWasherEdit.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.pbWasherEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbWasherEdit.Name = "pbWasherEdit";
            this.pbWasherEdit.Size = new System.Drawing.Size(23, 22);
            this.pbWasherEdit.Click += new System.EventHandler(this.pbWasherEdit_Click);
            // 
            // pbWasherDelete
            // 
            this.pbWasherDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbWasherDelete.Enabled = false;
            this.pbWasherDelete.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_delete;
            this.pbWasherDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbWasherDelete.Name = "pbWasherDelete";
            this.pbWasherDelete.Size = new System.Drawing.Size(23, 22);
            this.pbWasherDelete.ToolTipText = "\r\nDelete";
            this.pbWasherDelete.Click += new System.EventHandler(this.pbWasherDelete_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 31;
            this.label6.Text = "WASHER";
            // 
            // gridWasher
            // 
            this.gridWasher.AllowUserToAddRows = false;
            this.gridWasher.AllowUserToDeleteRows = false;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridWasher.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.gridWasher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridWasher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridWasher.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.WasherId});
            this.gridWasher.Location = new System.Drawing.Point(5, 38);
            this.gridWasher.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.gridWasher.MultiSelect = false;
            this.gridWasher.Name = "gridWasher";
            this.gridWasher.ReadOnly = true;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridWasher.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.gridWasher.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridWasher.Size = new System.Drawing.Size(366, 260);
            this.gridWasher.TabIndex = 27;
            this.gridWasher.SelectionChanged += new System.EventHandler(this.gridWasher_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Washer Program";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // WasherId
            // 
            this.WasherId.HeaderText = "WasherId";
            this.WasherId.Name = "WasherId";
            this.WasherId.ReadOnly = true;
            this.WasherId.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.toolStrip3);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.gridDryer);
            this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(800, 384);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(380, 291);
            this.panel3.TabIndex = 36;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbDryerAdd,
            this.pbDryerEdit,
            this.pbDryerDelete});
            this.toolStrip3.Location = new System.Drawing.Point(288, 3);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.ShowItemToolTips = false;
            this.toolStrip3.Size = new System.Drawing.Size(112, 25);
            this.toolStrip3.TabIndex = 37;
            this.toolStrip3.TabStop = true;
            // 
            // pbDryerAdd
            // 
            this.pbDryerAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbDryerAdd.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.pbDryerAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbDryerAdd.Name = "pbDryerAdd";
            this.pbDryerAdd.Size = new System.Drawing.Size(23, 22);
            this.pbDryerAdd.Click += new System.EventHandler(this.pbDryerAdd_Click);
            // 
            // pbDryerEdit
            // 
            this.pbDryerEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbDryerEdit.Enabled = false;
            this.pbDryerEdit.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.pbDryerEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbDryerEdit.Name = "pbDryerEdit";
            this.pbDryerEdit.Size = new System.Drawing.Size(23, 22);
            this.pbDryerEdit.Click += new System.EventHandler(this.pbDryerEdit_Click);
            // 
            // pbDryerDelete
            // 
            this.pbDryerDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbDryerDelete.Enabled = false;
            this.pbDryerDelete.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_delete;
            this.pbDryerDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbDryerDelete.Name = "pbDryerDelete";
            this.pbDryerDelete.Size = new System.Drawing.Size(23, 22);
            this.pbDryerDelete.ToolTipText = "\r\nDelete";
            this.pbDryerDelete.Click += new System.EventHandler(this.pbDryerDelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 31;
            this.label1.Text = "DRYER";
            // 
            // gridDryer
            // 
            this.gridDryer.AllowUserToAddRows = false;
            this.gridDryer.AllowUserToDeleteRows = false;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridDryer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.gridDryer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDryer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDryer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6,
            this.DryerId});
            this.gridDryer.Location = new System.Drawing.Point(5, 30);
            this.gridDryer.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.gridDryer.MultiSelect = false;
            this.gridDryer.Name = "gridDryer";
            this.gridDryer.ReadOnly = true;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridDryer.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.gridDryer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDryer.Size = new System.Drawing.Size(366, 252);
            this.gridDryer.TabIndex = 27;
            this.gridDryer.SelectionChanged += new System.EventHandler(this.gridDryer_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Dryer Process";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 200;
            // 
            // DryerId
            // 
            this.DryerId.HeaderText = "DryerId";
            this.DryerId.Name = "DryerId";
            this.DryerId.ReadOnly = true;
            this.DryerId.Visible = false;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbBarcode);
            this.groupBox2.Controls.Add(this.cbCond1);
            this.groupBox2.Controls.Add(this.tbGloveCode);
            this.groupBox2.Controls.Add(this.cbCond2);
            this.groupBox2.Controls.Add(this.tbGloveCategory);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Location = new System.Drawing.Point(17, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(551, 40);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            // 
            // WasherDryerAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 655);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.grdGloveDetails);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1192, 622);
            this.Name = "WasherDryerAssignment";
            this.Text = "Washer Or Dryer Assignment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WasherDryerAssignment_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdGloveDetails)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridWasher)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDryer)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbCond1;
        private System.Windows.Forms.TextBox tbGloveCode;
        private System.Windows.Forms.TextBox tbGloveCategory;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.ComboBox cbCond2;
        private System.Windows.Forms.DataGridView grdGloveDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView gridWasher;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gridDryer;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton pbWasherAdd;
        private System.Windows.Forms.ToolStripButton pbWasherEdit;
        private System.Windows.Forms.ToolStripButton pbWasherDelete;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton pbDryerAdd;
        private System.Windows.Forms.ToolStripButton pbDryerEdit;
        private System.Windows.Forms.ToolStripButton pbDryerDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasherId;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn DryerId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGloveCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGloveCategory;
    }
}