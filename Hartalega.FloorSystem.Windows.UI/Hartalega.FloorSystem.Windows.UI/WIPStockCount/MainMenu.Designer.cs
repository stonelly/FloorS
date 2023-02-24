namespace Hartalega.FloorSystem.Windows.UI.WIPStockCount
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnWIPReportByScanData = new System.Windows.Forms.Button();
            this.btnWIPVoidScannedData = new System.Windows.Forms.Button();
            this.btnWIPReportByCutoffBatches = new System.Windows.Forms.Button();
            this.btnWIPSummaryReport = new System.Windows.Forms.Button();
            this.btnWIPReports = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.MinimumSize = new System.Drawing.Size(953, 493);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(953, 493);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WIP Stock Count Main Menu";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnWIPReportByScanData, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnWIPVoidScannedData, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnWIPReportByCutoffBatches, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnWIPSummaryReport, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnWIPReports, 1, 4);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 118);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(953, 291);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnWIPReportByScanData
            // 
            this.btnWIPReportByScanData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWIPReportByScanData.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWIPReportByScanData.Location = new System.Drawing.Point(179, 3);
            this.btnWIPReportByScanData.Name = "btnWIPReportByScanData";
            this.btnWIPReportByScanData.Size = new System.Drawing.Size(594, 44);
            this.btnWIPReportByScanData.TabIndex = 0;
            this.btnWIPReportByScanData.Text = "WIP Report By Scan Data";
            this.btnWIPReportByScanData.UseVisualStyleBackColor = true;
            this.btnWIPReportByScanData.Click += new System.EventHandler(this.btnWIPReportByScanData_Click);
            // 
            // btnWIPVoidScannedData
            // 
            this.btnWIPVoidScannedData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWIPVoidScannedData.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWIPVoidScannedData.Location = new System.Drawing.Point(179, 53);
            this.btnWIPVoidScannedData.Name = "btnWIPVoidScannedData";
            this.btnWIPVoidScannedData.Size = new System.Drawing.Size(594, 44);
            this.btnWIPVoidScannedData.TabIndex = 1;
            this.btnWIPVoidScannedData.Text = "WIP Void Scanned Data";
            this.btnWIPVoidScannedData.UseVisualStyleBackColor = true;
            this.btnWIPVoidScannedData.Click += new System.EventHandler(this.btnWIPVoidScannedData_Click);
            // 
            // btnWIPReportByCutoffBatches
            // 
            this.btnWIPReportByCutoffBatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWIPReportByCutoffBatches.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWIPReportByCutoffBatches.Location = new System.Drawing.Point(179, 103);
            this.btnWIPReportByCutoffBatches.Name = "btnWIPReportByCutoffBatches";
            this.btnWIPReportByCutoffBatches.Size = new System.Drawing.Size(594, 44);
            this.btnWIPReportByCutoffBatches.TabIndex = 2;
            this.btnWIPReportByCutoffBatches.Text = "WIP Report By Cutoff Batches";
            this.btnWIPReportByCutoffBatches.UseVisualStyleBackColor = true;
            this.btnWIPReportByCutoffBatches.Click += new System.EventHandler(this.btnWIPReportByCutoffBatches_Click);
            // 
            // btnWIPSummaryReport
            // 
            this.btnWIPSummaryReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWIPSummaryReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWIPSummaryReport.Location = new System.Drawing.Point(179, 153);
            this.btnWIPSummaryReport.Name = "btnWIPSummaryReport";
            this.btnWIPSummaryReport.Size = new System.Drawing.Size(594, 44);
            this.btnWIPSummaryReport.TabIndex = 3;
            this.btnWIPSummaryReport.Text = "WIP Summary Report";
            this.btnWIPSummaryReport.UseVisualStyleBackColor = true;
            this.btnWIPSummaryReport.Click += new System.EventHandler(this.btnWIPSummaryReport_Click);
            // 
            // btnWIPReports
            // 
            this.btnWIPReports.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWIPReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWIPReports.Location = new System.Drawing.Point(179, 203);
            this.btnWIPReports.Name = "btnWIPReports";
            this.btnWIPReports.Size = new System.Drawing.Size(594, 44);
            this.btnWIPReports.TabIndex = 4;
            this.btnWIPReports.Text = "WIP Reports";
            this.btnWIPReports.UseVisualStyleBackColor = true;
            this.btnWIPReports.Click += new System.EventHandler(this.btnWIPReports_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(977, 517);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 456);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WIP Stock Count Main Menu";
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnWIPReportByScanData;
        private System.Windows.Forms.Button btnWIPReportByCutoffBatches;
        private System.Windows.Forms.Button btnWIPSummaryReport;
        private System.Windows.Forms.Button btnWIPReports;
        private System.Windows.Forms.Button btnWIPVoidScannedData;
    }
}