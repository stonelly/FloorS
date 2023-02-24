namespace Hartalega.FloorSystem.Windows.UI.ProductionDefect
{
    partial class ProductionDefectList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductionDefectList));
            this.grdDefectDetail = new System.Windows.Forms.DataGridView();
            this.colPddDefect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPddDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPddQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPddPNDefectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPddId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdDefectSummary = new System.Windows.Forms.DataGridView();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.tsBtnAddDefect = new System.Windows.Forms.ToolStripButton();
            this.tsBtnEditDefect = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnGo = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelProductionLine = new System.Windows.Forms.ToolStripLabel();
            this.dtpProductionDate = new System.Windows.Forms.DateTimePicker();
            this.ddProductionLine = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.colPdStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.colPdTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPdSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPdSide = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPdQAIDefectQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPdPNDefectQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPdSerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPdPNDefectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdDefectDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDefectSummary)).BeginInit();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdDefectDetail
            // 
            this.grdDefectDetail.AllowUserToAddRows = false;
            this.grdDefectDetail.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDefectDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDefectDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdDefectDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDefectDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPddDefect,
            this.colPddDesc,
            this.colPddQty,
            this.colPddPNDefectId,
            this.colPddId});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdDefectDetail.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdDefectDetail.Location = new System.Drawing.Point(0, 25);
            this.grdDefectDetail.Margin = new System.Windows.Forms.Padding(5);
            this.grdDefectDetail.MultiSelect = false;
            this.grdDefectDetail.Name = "grdDefectDetail";
            this.grdDefectDetail.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDefectDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDefectDetail.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.grdDefectDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDefectDetail.Size = new System.Drawing.Size(400, 611);
            this.grdDefectDetail.TabIndex = 0;
            this.grdDefectDetail.TabStop = false;
            this.grdDefectDetail.DoubleClick += new System.EventHandler(this.grdDefectDetail_DoubleClick);
            // 
            // colPddDefect
            // 
            this.colPddDefect.HeaderText = "Defect";
            this.colPddDefect.Name = "colPddDefect";
            this.colPddDefect.ReadOnly = true;
            this.colPddDefect.Width = 80;
            // 
            // colPddDesc
            // 
            this.colPddDesc.HeaderText = "Description";
            this.colPddDesc.Name = "colPddDesc";
            this.colPddDesc.ReadOnly = true;
            this.colPddDesc.Width = 200;
            // 
            // colPddQty
            // 
            this.colPddQty.HeaderText = "Qty";
            this.colPddQty.Name = "colPddQty";
            this.colPddQty.ReadOnly = true;
            // 
            // colPddPNDefectId
            // 
            this.colPddPNDefectId.HeaderText = "PNDefectId";
            this.colPddPNDefectId.Name = "colPddPNDefectId";
            this.colPddPNDefectId.ReadOnly = true;
            this.colPddPNDefectId.Visible = false;
            // 
            // colPddId
            // 
            this.colPddId.HeaderText = "Id";
            this.colPddId.Name = "colPddId";
            this.colPddId.ReadOnly = true;
            this.colPddId.Visible = false;
            // 
            // grdDefectSummary
            // 
            this.grdDefectSummary.AllowUserToAddRows = false;
            this.grdDefectSummary.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDefectSummary.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.grdDefectSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDefectSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grdDefectSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDefectSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPdStatus,
            this.colPdTime,
            this.colPdSize,
            this.colPdSide,
            this.colPdQAIDefectQty,
            this.colPdPNDefectQty,
            this.colPdSerialNumber,
            this.colPdPNDefectId});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdDefectSummary.DefaultCellStyle = dataGridViewCellStyle8;
            this.grdDefectSummary.Location = new System.Drawing.Point(0, 25);
            this.grdDefectSummary.Margin = new System.Windows.Forms.Padding(5);
            this.grdDefectSummary.MultiSelect = false;
            this.grdDefectSummary.Name = "grdDefectSummary";
            this.grdDefectSummary.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdDefectSummary.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDefectSummary.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.grdDefectSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDefectSummary.Size = new System.Drawing.Size(604, 611);
            this.grdDefectSummary.TabIndex = 0;
            this.grdDefectSummary.TabStop = false;
            this.grdDefectSummary.SelectionChanged += new System.EventHandler(this.grdDefectSummary_SelectionChanged);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnAddDefect,
            this.tsBtnEditDefect});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(58, 25);
            this.toolStrip3.TabIndex = 9;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // tsBtnAddDefect
            // 
            this.tsBtnAddDefect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAddDefect.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.tsBtnAddDefect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAddDefect.Name = "tsBtnAddDefect";
            this.tsBtnAddDefect.Size = new System.Drawing.Size(23, 22);
            this.tsBtnAddDefect.Click += new System.EventHandler(this.tsBtnAddDefect_Click_1);
            // 
            // tsBtnEditDefect
            // 
            this.tsBtnEditDefect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnEditDefect.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.tsBtnEditDefect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnEditDefect.Name = "tsBtnEditDefect";
            this.tsBtnEditDefect.Size = new System.Drawing.Size(23, 22);
            this.tsBtnEditDefect.Click += new System.EventHandler(this.tsBtnEditDefect_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 56);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grdDefectSummary);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdDefectDetail);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip3);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 636);
            this.splitContainer1.SplitterDistance = 604;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // btnGo
            // 
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(680, 19);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(63, 37);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "&Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelProductionLine});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1008, 32);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelProductionLine
            // 
            this.toolStripLabelProductionLine.Name = "toolStripLabelProductionLine";
            this.toolStripLabelProductionLine.Size = new System.Drawing.Size(210, 29);
            this.toolStripLabelProductionLine.Text = "Production Line :";
            // 
            // dtpProductionDate
            // 
            this.dtpProductionDate.CustomFormat = " dd/MM/yyyy";
            this.dtpProductionDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpProductionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpProductionDate.Location = new System.Drawing.Point(457, 23);
            this.dtpProductionDate.Name = "dtpProductionDate";
            this.dtpProductionDate.Size = new System.Drawing.Size(217, 35);
            this.dtpProductionDate.TabIndex = 2;
            this.dtpProductionDate.ValueChanged += new System.EventHandler(this.dtpProductionDate_ValueChanged);
            // 
            // ddProductionLine
            // 
            this.ddProductionLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddProductionLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddProductionLine.FormattingEnabled = true;
            this.ddProductionLine.Location = new System.Drawing.Point(221, 19);
            this.ddProductionLine.Name = "ddProductionLine";
            this.ddProductionLine.Size = new System.Drawing.Size(136, 37);
            this.ddProductionLine.TabIndex = 1;
            this.ddProductionLine.SelectedIndexChanged += new System.EventHandler(ddProductionLine_SelectedIndexChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(363, 27);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(88, 29);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date : ";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // colPdStatus
            // 
            this.colPdStatus.HeaderText = "Status";
            this.colPdStatus.Name = "colPdStatus";
            this.colPdStatus.ReadOnly = true;
            this.colPdStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPdStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPdStatus.Width = 70;
            // 
            // colPdTime
            // 
            this.colPdTime.HeaderText = "Time";
            this.colPdTime.Name = "colPdTime";
            this.colPdTime.ReadOnly = true;
            this.colPdTime.Width = 75;
            // 
            // colPdSize
            // 
            this.colPdSize.HeaderText = "Size";
            this.colPdSize.Name = "colPdSize";
            this.colPdSize.ReadOnly = true;
            this.colPdSize.Width = 70;
            // 
            // colPdSide
            // 
            this.colPdSide.HeaderText = "Side";
            this.colPdSide.Name = "colPdSide";
            this.colPdSide.ReadOnly = true;
            this.colPdSide.Width = 70;
            // 
            // colPdQAIDefectQty
            // 
            this.colPdQAIDefectQty.HeaderText = "QAI Defect Qty";
            this.colPdQAIDefectQty.Name = "colPdQAIDefectQty";
            this.colPdQAIDefectQty.ReadOnly = true;
            this.colPdQAIDefectQty.Width = 145;
            // 
            // colPdPNDefectQty
            // 
            this.colPdPNDefectQty.HeaderText = "PN Defect Qty";
            this.colPdPNDefectQty.Name = "colPdPNDefectQty";
            this.colPdPNDefectQty.ReadOnly = true;
            this.colPdPNDefectQty.Width = 160;
            // 
            // colPdSerialNumber
            // 
            this.colPdSerialNumber.HeaderText = "Serial Number";
            this.colPdSerialNumber.Name = "colPdSerialNumber";
            this.colPdSerialNumber.ReadOnly = true;
            this.colPdSerialNumber.Visible = false;
            // 
            // colPdPNDefectId
            // 
            this.colPdPNDefectId.HeaderText = "Production Defect Id";
            this.colPdPNDefectId.Name = "colPdPNDefectId";
            this.colPdPNDefectId.ReadOnly = true;
            this.colPdPNDefectId.Visible = false;
            // 
            // ProductionDefectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.ddProductionLine);
            this.Controls.Add(this.dtpProductionDate);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1024, 726);
            this.Name = "ProductionDefectList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Production Defect System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Maintainence_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProductionDefectList_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdDefectDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDefectSummary)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdDefectDetail;
        private System.Windows.Forms.DataGridView grdDefectSummary;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton tsBtnAddDefect;
        private System.Windows.Forms.ToolStripButton tsBtnEditDefect;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelProductionLine;
        private System.Windows.Forms.DateTimePicker dtpProductionDate;
        private System.Windows.Forms.ComboBox ddProductionLine;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPddDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPddDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPddQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPddPNDefectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPddId;
        private System.Windows.Forms.DataGridViewImageColumn colPdStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdSide;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdQAIDefectQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdPNDefectQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdSerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPdPNDefectId;
    }
}