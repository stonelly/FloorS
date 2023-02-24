namespace Hartalega.FloorSystem.Windows.UI.ProductionSystemReports
{
    partial class ReportsSubMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsSubMenu));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPSR = new System.Windows.Forms.Button();
            this.btnOEE = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 660);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reports Main Menu";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnPSR, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOEE, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(291, 245);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(378, 112);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnPSR
            // 
            this.btnPSR.Location = new System.Drawing.Point(3, 59);
            this.btnPSR.Name = "btnPSR";
            this.btnPSR.Size = new System.Drawing.Size(365, 50);
            this.btnPSR.TabIndex = 1;
            this.btnPSR.Text = "Production System Reports";
            this.btnPSR.UseVisualStyleBackColor = true;
            this.btnPSR.Click += new System.EventHandler(this.btnPSR_Click);
            // 
            // btnOEE
            // 
            this.btnOEE.Location = new System.Drawing.Point(3, 3);
            this.btnOEE.Name = "btnOEE";
            this.btnOEE.Size = new System.Drawing.Size(365, 50);
            this.btnOEE.TabIndex = 0;
            this.btnOEE.Text = "OEE Reports";
            this.btnOEE.UseVisualStyleBackColor = true;
            this.btnOEE.Click += new System.EventHandler(this.btnOEE_Click);
            // 
            // ReportsSubMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "ReportsSubMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reports Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnPSR;
        private System.Windows.Forms.Button btnOEE;
    }
}