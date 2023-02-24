namespace Hartalega.FloorSystem.Windows.UI.Washer
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
            this.btnScanStopTime = new System.Windows.Forms.Button();
            this.btnScanBatchCard = new System.Windows.Forms.Button();
            this.tlPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tlPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnScanStopTime
            // 
            this.btnScanStopTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnScanStopTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanStopTime.Location = new System.Drawing.Point(80, 78);
            this.btnScanStopTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnScanStopTime.Name = "btnScanStopTime";
            this.btnScanStopTime.Size = new System.Drawing.Size(300, 50);
            this.btnScanStopTime.TabIndex = 9;
            this.btnScanStopTime.Text = "Scan Stop Time";
            this.btnScanStopTime.UseVisualStyleBackColor = true;
            this.btnScanStopTime.Click += new System.EventHandler(this.btnScanStopTime_Click);
            // 
            // btnScanBatchCard
            // 
            this.btnScanBatchCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnScanBatchCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanBatchCard.Location = new System.Drawing.Point(80, 9);
            this.btnScanBatchCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnScanBatchCard.Name = "btnScanBatchCard";
            this.btnScanBatchCard.Size = new System.Drawing.Size(300, 50);
            this.btnScanBatchCard.TabIndex = 8;
            this.btnScanBatchCard.Text = "Scan Batch Card";
            this.btnScanBatchCard.UseVisualStyleBackColor = true;
            this.btnScanBatchCard.Click += new System.EventHandler(this.btnScanBatchCard_Click);
            // 
            // tlPanel1
            // 
            this.tlPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlPanel1.ColumnCount = 1;
            this.tlPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlPanel1.Controls.Add(this.btnScanStopTime, 0, 1);
            this.tlPanel1.Controls.Add(this.btnScanBatchCard, 0, 0);
            this.tlPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tlPanel1.Location = new System.Drawing.Point(252, 270);
            this.tlPanel1.Name = "tlPanel1";
            this.tlPanel1.RowCount = 3;
            this.tlPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlPanel1.Size = new System.Drawing.Size(460, 200);
            this.tlPanel1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tlPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 660);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Washer System Main Menu";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Washer System Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainMenu_KeyDown);
            this.tlPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnScanStopTime;
        private System.Windows.Forms.Button btnScanBatchCard;
        private System.Windows.Forms.TableLayoutPanel tlPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}