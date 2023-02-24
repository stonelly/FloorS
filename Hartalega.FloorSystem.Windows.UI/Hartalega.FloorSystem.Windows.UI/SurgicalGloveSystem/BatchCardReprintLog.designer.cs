namespace Hartalega.FloorSystem.Windows.UI.SurgicalGloveSystem
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
            this.colManuPrintdt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReportby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReportReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPrintDateRange = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPlant = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dpfromdate = new System.Windows.Forms.DateTimePicker();
            this.dptodate = new System.Windows.Forms.DateTimePicker();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchCardReprintLog)).BeginInit();
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
            this.tsbRefresh.Size = new System.Drawing.Size(29, 24);
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
            this.imgCancel.Size = new System.Drawing.Size(29, 24);
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
            this.imgExportToExcel.Size = new System.Drawing.Size(29, 24);
            this.imgExportToExcel.Text = "toolStripButton6";
            this.imgExportToExcel.ToolTipText = "Export to Excel";
            this.imgExportToExcel.Click += new System.EventHandler(this.imgExportToExcel_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.imgCancel,
            this.imgExportToExcel});
            this.toolStrip2.Location = new System.Drawing.Point(21, 11);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(100, 27);
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
            this.dgvBatchCardReprintLog.Location = new System.Drawing.Point(21, 179);
            this.dgvBatchCardReprintLog.Margin = new System.Windows.Forms.Padding(4);
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
            this.dgvBatchCardReprintLog.RowHeadersWidth = 51;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBatchCardReprintLog.RowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvBatchCardReprintLog.Size = new System.Drawing.Size(1418, 669);
            this.dgvBatchCardReprintLog.TabIndex = 8;
            // 
            // colBatchNo
            // 
            this.colBatchNo.DataPropertyName = "BatchNumber";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colBatchNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.colBatchNo.HeaderText = "Batch No.";
            this.colBatchNo.MinimumWidth = 6;
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
            this.colSerialNo.MinimumWidth = 6;
            this.colSerialNo.Name = "colSerialNo";
            this.colSerialNo.ReadOnly = true;
            this.colSerialNo.Width = 125;
            // 
            // colbatchDateTiome
            // 
            this.colbatchDateTiome.DataPropertyName = "PrintDatetime";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colbatchDateTiome.DefaultCellStyle = dataGridViewCellStyle5;
            this.colbatchDateTiome.HeaderText = "Batch Date Time";
            this.colbatchDateTiome.MinimumWidth = 6;
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
            this.colPlant.MinimumWidth = 6;
            this.colPlant.Name = "colPlant";
            this.colPlant.ReadOnly = true;
            this.colPlant.Width = 125;
            // 
            // colProcessArea
            // 
            this.colProcessArea.DataPropertyName = "ProcessArea";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colProcessArea.DefaultCellStyle = dataGridViewCellStyle7;
            this.colProcessArea.HeaderText = "Process Area";
            this.colProcessArea.MinimumWidth = 6;
            this.colProcessArea.Name = "colProcessArea";
            this.colProcessArea.ReadOnly = true;
            this.colProcessArea.Width = 120;
            // 
            // colManuPrintdt
            // 
            this.colManuPrintdt.DataPropertyName = "ReprintDateTime";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colManuPrintdt.DefaultCellStyle = dataGridViewCellStyle8;
            this.colManuPrintdt.HeaderText = "Reprint Date Time";
            this.colManuPrintdt.MinimumWidth = 6;
            this.colManuPrintdt.Name = "colManuPrintdt";
            this.colManuPrintdt.ReadOnly = true;
            this.colManuPrintdt.Width = 150;
            // 
            // colReportby
            // 
            this.colReportby.DataPropertyName = "OperatorName";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colReportby.DefaultCellStyle = dataGridViewCellStyle9;
            this.colReportby.HeaderText = "Reprint By";
            this.colReportby.MinimumWidth = 6;
            this.colReportby.Name = "colReportby";
            this.colReportby.ReadOnly = true;
            this.colReportby.Width = 125;
            // 
            // colReportReason
            // 
            this.colReportReason.DataPropertyName = "ReasonText";
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colReportReason.DefaultCellStyle = dataGridViewCellStyle10;
            this.colReportReason.HeaderText = "Reprint Reason";
            this.colReportReason.MinimumWidth = 6;
            this.colReportReason.Name = "colReportReason";
            this.colReportReason.ReadOnly = true;
            this.colReportReason.Width = 200;
            // 
            // lblPrintDateRange
            // 
            this.lblPrintDateRange.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPrintDateRange.AutoSize = true;
            this.lblPrintDateRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDateRange.Location = new System.Drawing.Point(197, 7);
            this.lblPrintDateRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrintDateRange.Name = "lblPrintDateRange";
            this.lblPrintDateRange.Size = new System.Drawing.Size(161, 36);
            this.lblPrintDateRange.TabIndex = 9;
            this.lblPrintDateRange.Text = "From Date";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(270, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 36);
            this.label2.TabIndex = 10;
            this.label2.Text = "Plant";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(786, 79);
            this.btnGo.Margin = new System.Windows.Forms.Padding(4);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(94, 42);
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
            this.label1.Location = new System.Drawing.Point(652, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 36);
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
            this.cmbPlant.Location = new System.Drawing.Point(364, 78);
            this.cmbPlant.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPlant.Name = "cmbPlant";
            this.cmbPlant.Size = new System.Drawing.Size(162, 44);
            this.cmbPlant.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 362F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 266F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 232F));
            this.tableLayoutPanel1.Controls.Add(this.dpfromdate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPrintDateRange, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnGo, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbPlant, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dptodate, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(21, 46);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1250, 125);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dpfromdate
            // 
            this.dpfromdate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dpfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpfromdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpfromdate.Location = new System.Drawing.Point(400, 4);
            this.dpfromdate.Margin = new System.Windows.Forms.Padding(4);
            this.dpfromdate.Name = "dpfromdate";
            this.dpfromdate.Size = new System.Drawing.Size(224, 41);
            this.dpfromdate.TabIndex = 0;
            // 
            // dptodate
            // 
            this.dptodate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dptodate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dptodate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dptodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dptodate.Location = new System.Drawing.Point(790, 4);
            this.dptodate.Margin = new System.Windows.Forms.Padding(4);
            this.dptodate.Name = "dptodate";
            this.dptodate.Size = new System.Drawing.Size(224, 41);
            this.dptodate.TabIndex = 1;
            // 
            // BatchCardReprintLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1654, 865);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.dgvBatchCardReprintLog);
            this.Controls.Add(this.toolStrip2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1276, 901);
            this.Name = "BatchCardReprintLog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Card Reprint Log";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmBatchCardReprintLog_Load);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchCardReprintLog)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dpfromdate;
        private System.Windows.Forms.DateTimePicker dptodate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerialNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colbatchDateTiome;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManuPrintdt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReportby;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReportReason;
    }
}