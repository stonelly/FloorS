namespace Hartalega.FloorSystem.Windows.UI.HourlyBatchCard
{
    partial class BatchCardReprintLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchCardReprintLog));
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.imgCancel = new System.Windows.Forms.ToolStripButton();
            this.imgExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.dgvBatchCardReprintLog = new System.Windows.Forms.DataGridView();
            this.colBatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSerialNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colbatchDateTiome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrintType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManuPrintdt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReportby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReportReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPrintDateRange = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPlant = new System.Windows.Forms.ComboBox();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.manualHourlyBatchCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dpfromdate = new System.Windows.Forms.DateTimePicker();
            this.dptodate = new System.Windows.Forms.DateTimePicker();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchCardReprintLog)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.AutoToolTip = false;
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.arrow_refresh;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "toolStripButton1";
            this.tsbRefresh.ToolTipText = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // imgCancel
            // 
            this.imgCancel.AutoToolTip = false;
            this.imgCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imgCancel.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_delete;
            this.imgCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imgCancel.Name = "imgCancel";
            this.imgCancel.Size = new System.Drawing.Size(23, 22);
            this.imgCancel.Text = "toolStripButton3";
            this.imgCancel.ToolTipText = "Cancel";
            this.imgCancel.Click += new System.EventHandler(this.imgCancel_Click);
            // 
            // imgExportToExcel
            // 
            this.imgExportToExcel.AutoToolTip = false;
            this.imgExportToExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imgExportToExcel.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_excel;
            this.imgExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imgExportToExcel.Name = "imgExportToExcel";
            this.imgExportToExcel.Size = new System.Drawing.Size(23, 22);
            this.imgExportToExcel.Text = "toolStripButton6";
            this.imgExportToExcel.ToolTipText = "Export to Excel";
            this.imgExportToExcel.Click += new System.EventHandler(this.imgExportToExcel_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.imgCancel,
            this.imgExportToExcel});
            this.toolStrip2.Location = new System.Drawing.Point(17, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(81, 25);
            this.toolStrip2.TabIndex = 5;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // dgvBatchCardReprintLog
            // 
            this.dgvBatchCardReprintLog.AllowUserToAddRows = false;
            this.dgvBatchCardReprintLog.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            this.dgvBatchCardReprintLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBatchCardReprintLog.CausesValidation = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBatchCardReprintLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBatchCardReprintLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatchCardReprintLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBatchNo,
            this.colSerialNo,
            this.colbatchDateTiome,
            this.colPlant,
            this.colProcessArea,
            this.colPrintType,
            this.colManuPrintdt,
            this.colReportby,
            this.colReportReason});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBatchCardReprintLog.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvBatchCardReprintLog.Location = new System.Drawing.Point(0, 175);
            this.dgvBatchCardReprintLog.Name = "dgvBatchCardReprintLog";
            this.dgvBatchCardReprintLog.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBatchCardReprintLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBatchCardReprintLog.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvBatchCardReprintLog.Size = new System.Drawing.Size(1008, 320);
            this.dgvBatchCardReprintLog.TabIndex = 8;
            // 
            // colBatchNo
            // 
            this.colBatchNo.DataPropertyName = "BatchNumber";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colBatchNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.colBatchNo.HeaderText = "Batch No.";
            this.colBatchNo.Name = "colBatchNo";
            this.colBatchNo.ReadOnly = true;
            this.colBatchNo.Width = 150;
            // 
            // colSerialNo
            // 
            this.colSerialNo.DataPropertyName = "SerialNumber";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colSerialNo.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSerialNo.HeaderText = "Serial No.";
            this.colSerialNo.Name = "colSerialNo";
            this.colSerialNo.ReadOnly = true;
            // 
            // colbatchDateTiome
            // 
            this.colbatchDateTiome.DataPropertyName = "PrintDatetime";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colbatchDateTiome.DefaultCellStyle = dataGridViewCellStyle5;
            this.colbatchDateTiome.HeaderText = "Batch Date Time";
            this.colbatchDateTiome.Name = "colbatchDateTiome";
            this.colbatchDateTiome.ReadOnly = true;
            this.colbatchDateTiome.Width = 150;
            // 
            // colPlant
            // 
            this.colPlant.DataPropertyName = "Plant";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colPlant.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPlant.HeaderText = "Plant";
            this.colPlant.Name = "colPlant";
            this.colPlant.ReadOnly = true;
            // 
            // colProcessArea
            // 
            this.colProcessArea.DataPropertyName = "ProcessArea";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colProcessArea.DefaultCellStyle = dataGridViewCellStyle7;
            this.colProcessArea.HeaderText = "Process Area";
            this.colProcessArea.Name = "colProcessArea";
            this.colProcessArea.ReadOnly = true;
            this.colProcessArea.Width = 120;
            // 
            // colPrintType
            // 
            this.colPrintType.DataPropertyName = "PrintType";
            this.colPrintType.HeaderText = "Type";
            this.colPrintType.Name = "colPrintType";
            this.colPrintType.ReadOnly = true;
            // 
            // colManuPrintdt
            // 
            this.colManuPrintdt.DataPropertyName = "ReprintDateTime";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colManuPrintdt.DefaultCellStyle = dataGridViewCellStyle8;
            this.colManuPrintdt.HeaderText = "Print Date Time";
            this.colManuPrintdt.Name = "colManuPrintdt";
            this.colManuPrintdt.ReadOnly = true;
            this.colManuPrintdt.Width = 150;
            // 
            // colReportby
            // 
            this.colReportby.DataPropertyName = "OperatorName";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colReportby.DefaultCellStyle = dataGridViewCellStyle9;
            this.colReportby.HeaderText = "Print By";
            this.colReportby.Name = "colReportby";
            this.colReportby.ReadOnly = true;
            // 
            // colReportReason
            // 
            this.colReportReason.DataPropertyName = "ReasonText";
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colReportReason.DefaultCellStyle = dataGridViewCellStyle10;
            this.colReportReason.HeaderText = "Reason";
            this.colReportReason.Name = "colReportReason";
            this.colReportReason.ReadOnly = true;
            this.colReportReason.Width = 200;
            // 
            // lblPrintDateRange
            // 
            this.lblPrintDateRange.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPrintDateRange.AutoSize = true;
            this.lblPrintDateRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDateRange.Location = new System.Drawing.Point(152, 5);
            this.lblPrintDateRange.Name = "lblPrintDateRange";
            this.lblPrintDateRange.Size = new System.Drawing.Size(135, 29);
            this.lblPrintDateRange.TabIndex = 9;
            this.lblPrintDateRange.Text = "From Date";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(215, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "Plant";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(629, 63);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 34);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "&Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(517, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 29);
            this.label1.TabIndex = 13;
            this.label1.Text = "To Date";
            // 
            // cmbPlant
            // 
            this.cmbPlant.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPlant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlant.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPlant.FormattingEnabled = true;
            this.cmbPlant.Items.AddRange(new object[] {
            "07:00",
            "08:00",
            "09:00",
            "10:00",
            "11:00",
            "12:00"});
            this.cmbPlant.Location = new System.Drawing.Point(292, 62);
            this.cmbPlant.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPlant.Name = "cmbPlant";
            this.cmbPlant.Size = new System.Drawing.Size(130, 37);
            this.cmbPlant.TabIndex = 2;
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem3,
            this.manualHourlyBatchCardToolStripMenuItem,
            this.toolStripMenuItem2});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1017, 24);
            this.menuStrip2.TabIndex = 15;
            this.menuStrip2.Text = "menuStrip2";
            this.menuStrip2.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip2_ItemClicked);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(92, 20);
            this.toolStripMenuItem4.Text = "Line Selection";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(157, 20);
            this.toolStripMenuItem3.Text = "Reprint Hourly Batch Card";
            // 
            // manualHourlyBatchCardToolStripMenuItem
            // 
            this.manualHourlyBatchCardToolStripMenuItem.Name = "manualHourlyBatchCardToolStripMenuItem";
            this.manualHourlyBatchCardToolStripMenuItem.Size = new System.Drawing.Size(148, 20);
            this.manualHourlyBatchCardToolStripMenuItem.Text = "Manual Print Batch Card";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(141, 20);
            this.toolStripMenuItem2.Text = "Batch Card Reprint Log";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 290F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 189F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tableLayoutPanel1.Controls.Add(this.dpfromdate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPrintDateRange, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnGo, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbPlant, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dptodate, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 60);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dpfromdate
            // 
            this.dpfromdate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dpfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpfromdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpfromdate.Location = new System.Drawing.Point(320, 3);
            this.dpfromdate.Name = "dpfromdate";
            this.dpfromdate.Size = new System.Drawing.Size(180, 35);
            this.dpfromdate.TabIndex = 0;
            // 
            // dptodate
            // 
            this.dptodate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dptodate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dptodate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dptodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dptodate.Location = new System.Drawing.Point(632, 3);
            this.dptodate.Name = "dptodate";
            this.dptodate.Size = new System.Drawing.Size(180, 35);
            this.dptodate.TabIndex = 1;
            // 
            // BatchCardReprintLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 639);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.dgvBatchCardReprintLog);
            this.Controls.Add(this.toolStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 624);
            this.Name = "BatchCardReprintLog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Card Reprint Log";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmBatchCardReprintLog_Load);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchCardReprintLog)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton imgCancel;
        private System.Windows.Forms.ToolStripButton imgExportToExcel;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.DataGridView dgvBatchCardReprintLog;
        private System.Windows.Forms.Label lblPrintDateRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPlant;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dpfromdate;
        private System.Windows.Forms.DateTimePicker dptodate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colbatchDateTiome;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrintType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManuPrintdt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReportby;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReportReason;
        private System.Windows.Forms.ToolStripMenuItem manualHourlyBatchCardToolStripMenuItem;
    }
}