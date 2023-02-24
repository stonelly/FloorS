namespace Hartalega.FloorSystem.Windows.UI.GIS
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
            this.btnGloveInquiry = new System.Windows.Forms.Button();
            this.btnScanOut = new System.Windows.Forms.Button();
            this.btnScanIn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGloveInquiry
            // 
            this.btnGloveInquiry.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGloveInquiry.Location = new System.Drawing.Point(3, 115);
            this.btnGloveInquiry.Name = "btnGloveInquiry";
            this.btnGloveInquiry.Size = new System.Drawing.Size(487, 50);
            this.btnGloveInquiry.TabIndex = 13;
            this.btnGloveInquiry.Text = "Glove Inquiry";
            this.btnGloveInquiry.UseVisualStyleBackColor = true;
            this.btnGloveInquiry.Click += new System.EventHandler(this.btnGloveInquiry_Click);
            // 
            // btnScanOut
            // 
            this.btnScanOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanOut.Location = new System.Drawing.Point(3, 59);
            this.btnScanOut.Name = "btnScanOut";
            this.btnScanOut.Size = new System.Drawing.Size(487, 50);
            this.btnScanOut.TabIndex = 12;
            this.btnScanOut.Text = "Scan Out";
            this.btnScanOut.UseVisualStyleBackColor = true;
            this.btnScanOut.Click += new System.EventHandler(this.btnScanOut_Click);
            // 
            // btnScanIn
            // 
            this.btnScanIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanIn.Location = new System.Drawing.Point(3, 3);
            this.btnScanIn.Name = "btnScanIn";
            this.btnScanIn.Size = new System.Drawing.Size(487, 50);
            this.btnScanIn.TabIndex = 11;
            this.btnScanIn.Text = "Scan In";
            this.btnScanIn.UseVisualStyleBackColor = true;
            this.btnScanIn.Click += new System.EventHandler(this.btnScanIn_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnScanIn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnGloveInquiry, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnScanOut, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(233, 226);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.48128F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.48128F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.57219F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(494, 187);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 660);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Glove Inventory System Main Menu";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Glove Inventory System Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGloveInquiry;
        private System.Windows.Forms.Button btnScanOut;
        private System.Windows.Forms.Button btnScanIn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}