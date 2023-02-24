namespace Hartalega.FloorSystem.Windows.UI.Glove
{
    partial class QCTargetAssignment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.pbQcTypeAdd = new System.Windows.Forms.ToolStripButton();
            this.pbQcTypeEdit = new System.Windows.Forms.ToolStripButton();
            this.pbQcTypeDelete = new System.Windows.Forms.ToolStripButton();
            this.label5 = new System.Windows.Forms.Label();
            this.grdQcTypeDetails = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QcId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QcDescId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdGloveDetails)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQcTypeDetails)).BeginInit();
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
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdGloveDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdGloveDetails.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.grdGloveDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdGloveDetails.Size = new System.Drawing.Size(665, 646);
            this.grdGloveDetails.TabIndex = 26;
            this.grdGloveDetails.SelectionChanged += new System.EventHandler(this.grdGloveDetails_SelectionChanged);
            // 
            // No
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.No.DefaultCellStyle = dataGridViewCellStyle2;
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 50;
            // 
            // colGloveCode
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colGloveCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.colGloveCode.HeaderText = "Glove Code";
            this.colGloveCode.Name = "colGloveCode";
            this.colGloveCode.ReadOnly = true;
            this.colGloveCode.Width = 150;
            // 
            // colDescription
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colDescription.DefaultCellStyle = dataGridViewCellStyle4;
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
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.grdQcTypeDetails);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(758, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(459, 630);
            this.panel1.TabIndex = 31;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbQcTypeAdd,
            this.pbQcTypeEdit,
            this.pbQcTypeDelete});
            this.toolStrip1.Location = new System.Drawing.Point(367, 6);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(81, 25);
            this.toolStrip1.TabIndex = 35;
            this.toolStrip1.TabStop = true;
            // 
            // pbQcTypeAdd
            // 
            this.pbQcTypeAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbQcTypeAdd.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.pbQcTypeAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbQcTypeAdd.Name = "pbQcTypeAdd";
            this.pbQcTypeAdd.Size = new System.Drawing.Size(23, 22);
            this.pbQcTypeAdd.Click += new System.EventHandler(this.pbQcTypeAdd_Click);
            // 
            // pbQcTypeEdit
            // 
            this.pbQcTypeEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbQcTypeEdit.Enabled = false;
            this.pbQcTypeEdit.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.pbQcTypeEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbQcTypeEdit.Name = "pbQcTypeEdit";
            this.pbQcTypeEdit.Size = new System.Drawing.Size(23, 22);
            this.pbQcTypeEdit.Click += new System.EventHandler(this.pbQcTypeEdit_Click);
            // 
            // pbQcTypeDelete
            // 
            this.pbQcTypeDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbQcTypeDelete.Enabled = false;
            this.pbQcTypeDelete.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_delete;
            this.pbQcTypeDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbQcTypeDelete.Name = "pbQcTypeDelete";
            this.pbQcTypeDelete.Size = new System.Drawing.Size(23, 22);
            this.pbQcTypeDelete.ToolTipText = "\r\nDelete";
            this.pbQcTypeDelete.Click += new System.EventHandler(this.pbQcTypeDelete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 17);
            this.label5.TabIndex = 31;
            this.label5.Text = "QC TYPE";
            // 
            // grdQcTypeDetails
            // 
            this.grdQcTypeDetails.AllowUserToAddRows = false;
            this.grdQcTypeDetails.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdQcTypeDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.grdQcTypeDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdQcTypeDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdQcTypeDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn3,
            this.QcId,
            this.QcDescId});
            this.grdQcTypeDetails.Location = new System.Drawing.Point(5, 34);
            this.grdQcTypeDetails.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grdQcTypeDetails.MultiSelect = false;
            this.grdQcTypeDetails.Name = "grdQcTypeDetails";
            this.grdQcTypeDetails.ReadOnly = true;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdQcTypeDetails.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.grdQcTypeDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdQcTypeDetails.Size = new System.Drawing.Size(445, 562);
            this.grdQcTypeDetails.TabIndex = 27;
            this.grdQcTypeDetails.SelectionChanged += new System.EventHandler(this.grdQcTypeDetails_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "QC Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "No Of Tester";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 70;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "QC Places/HR";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 70;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // QcId
            // 
            this.QcId.HeaderText = "QcId";
            this.QcId.Name = "QcId";
            this.QcId.ReadOnly = true;
            this.QcId.Visible = false;
            // 
            // QcDescId
            // 
            this.QcDescId.HeaderText = "QcDescId";
            this.QcDescId.Name = "QcDescId";
            this.QcDescId.ReadOnly = true;
            this.QcDescId.Visible = false;
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
            // QCTargetAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 655);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grdGloveDetails);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1192, 622);
            this.Name = "QCTargetAssignment";
            this.Text = "QC Target Assignment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GloveCode_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdGloveDetails)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQcTypeDetails)).EndInit();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView grdQcTypeDetails;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton pbQcTypeAdd;
        private System.Windows.Forms.ToolStripButton pbQcTypeEdit;
        private System.Windows.Forms.ToolStripButton pbQcTypeDelete;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn QcId;
        private System.Windows.Forms.DataGridViewTextBoxColumn QcDescId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGloveCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGloveCategory;
    }
}