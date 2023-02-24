namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
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
            this.btnWorkOrderMaintenance = new System.Windows.Forms.Button();
            this.btnWorkOrderStatus = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1270, 652);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Work Order Main Menu";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnWorkOrderMaintenance, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnWorkOrderStatus, 0, 1);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(203, 95);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 227);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // btnWorkOrderMaintenance
            // 
            this.btnWorkOrderMaintenance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnWorkOrderMaintenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkOrderMaintenance.Location = new System.Drawing.Point(122, 29);
            this.btnWorkOrderMaintenance.Name = "btnWorkOrderMaintenance";
            this.btnWorkOrderMaintenance.Size = new System.Drawing.Size(559, 55);
            this.btnWorkOrderMaintenance.TabIndex = 0;
            this.btnWorkOrderMaintenance.Text = "Work Order Maintenance";
            this.btnWorkOrderMaintenance.UseVisualStyleBackColor = true;
            this.btnWorkOrderMaintenance.Click += new System.EventHandler(this.btnWorkOrderMaintenance_Click);
            // 
            // btnWorkOrderStatus
            // 
            this.btnWorkOrderStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnWorkOrderStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkOrderStatus.Location = new System.Drawing.Point(122, 142);
            this.btnWorkOrderStatus.Name = "btnWorkOrderStatus";
            this.btnWorkOrderStatus.Size = new System.Drawing.Size(559, 55);
            this.btnWorkOrderStatus.TabIndex = 1;
            this.btnWorkOrderStatus.Text = "Work Order Status";
            this.btnWorkOrderStatus.UseVisualStyleBackColor = true;
            this.btnWorkOrderStatus.Click += new System.EventHandler(this.btnWorkOrderStatus_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 655);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1292, 640);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Work Order Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWorkOrderMaintenance;
        private System.Windows.Forms.Button btnWorkOrderStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}