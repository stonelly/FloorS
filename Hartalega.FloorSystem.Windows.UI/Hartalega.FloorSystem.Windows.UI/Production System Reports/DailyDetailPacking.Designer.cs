namespace Hartalega.FloorSystem.Windows.UI.Production_System_Reports
{
    partial class DailyDetailPacking
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Location = new System.Drawing.Point(12, 91);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            this.reportViewer1.ServerReport.ReportPath = "https://hep-fs-dev.gmd.lab/Reports/Pages/Report.aspx?ItemPath=%2fProduction+Syste" +
    "m+Reports%2fDaily+Detail+QC+Report";
            this.reportViewer1.ServerReport.ReportServerUrl = new System.Uri("https://hep-fs-dev.gmd.lab", System.UriKind.Absolute);
            this.reportViewer1.Size = new System.Drawing.Size(980, 436);
            this.reportViewer1.TabIndex = 0;
            // 
            // DailyDetailPacking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 539);
            this.Controls.Add(this.reportViewer1);
            this.Name = "DailyDetailPacking";
            this.Text = "DailyDetailPacking";
            this.Load += new System.EventHandler(this.DailyDetailPacking_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}