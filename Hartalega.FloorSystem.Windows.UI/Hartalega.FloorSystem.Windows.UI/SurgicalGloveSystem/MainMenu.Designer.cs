namespace Hartalega.FloorSystem.Windows.UI.SurgicalGloveSystem
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
            this.btnReprintSRBC = new System.Windows.Forms.Button();
            this.btnSurgicalBatchOrder = new System.Windows.Forms.Button();
            this.btnPrintSRBC = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBatchCardReprintLog = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReprintSRBC
            // 
            this.btnReprintSRBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReprintSRBC.Location = new System.Drawing.Point(4, 138);
            this.btnReprintSRBC.Margin = new System.Windows.Forms.Padding(4);
            this.btnReprintSRBC.Name = "btnReprintSRBC";
            this.btnReprintSRBC.Size = new System.Drawing.Size(404, 59);
            this.btnReprintSRBC.TabIndex = 16;
            this.btnReprintSRBC.Text = "Reprint SRBC";
            this.btnReprintSRBC.UseVisualStyleBackColor = true;
            this.btnReprintSRBC.Click += new System.EventHandler(this.btnReprintSRBC_Click);
            // 
            // btnSurgicalBatchOrder
            // 
            this.btnSurgicalBatchOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSurgicalBatchOrder.Location = new System.Drawing.Point(4, 71);
            this.btnSurgicalBatchOrder.Margin = new System.Windows.Forms.Padding(4);
            this.btnSurgicalBatchOrder.Name = "btnSurgicalBatchOrder";
            this.btnSurgicalBatchOrder.Size = new System.Drawing.Size(404, 59);
            this.btnSurgicalBatchOrder.TabIndex = 15;
            this.btnSurgicalBatchOrder.Text = "Glove Batch Order";
            this.btnSurgicalBatchOrder.UseVisualStyleBackColor = true;
            this.btnSurgicalBatchOrder.Click += new System.EventHandler(this.btnSurgicalBatchOrder_Click);
            // 
            // btnPrintSRBC
            // 
            this.btnPrintSRBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintSRBC.Location = new System.Drawing.Point(4, 4);
            this.btnPrintSRBC.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintSRBC.Name = "btnPrintSRBC";
            this.btnPrintSRBC.Size = new System.Drawing.Size(404, 59);
            this.btnPrintSRBC.TabIndex = 14;
            this.btnPrintSRBC.Text = "Print Surgical Batch Card";
            this.btnPrintSRBC.UseVisualStyleBackColor = true;
            this.btnPrintSRBC.Click += new System.EventHandler(this.btnPrintSRBC_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnBatchCardReprintLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnPrintSRBC, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReprintSRBC, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSurgicalBatchOrder, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(400, 304);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(412, 271);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // btnBatchCardReprintLog
            // 
            this.btnBatchCardReprintLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBatchCardReprintLog.Location = new System.Drawing.Point(4, 205);
            this.btnBatchCardReprintLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnBatchCardReprintLog.Name = "btnBatchCardReprintLog";
            this.btnBatchCardReprintLog.Size = new System.Drawing.Size(404, 61);
            this.btnBatchCardReprintLog.TabIndex = 17;
            this.btnBatchCardReprintLog.Text = "Batch Card Reprint Log";
            this.btnBatchCardReprintLog.UseVisualStyleBackColor = true;
            this.btnBatchCardReprintLog.Click += new System.EventHandler(this.btnBatchCardReprintLog_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(28, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1201, 825);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Surgical Glove System Main Menu";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1260, 865);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1276, 901);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Surgical Glove System Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReprintSRBC;
        private System.Windows.Forms.Button btnSurgicalBatchOrder;
        private System.Windows.Forms.Button btnPrintSRBC;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBatchCardReprintLog;
    }
}