namespace Hartalega.FloorSystem.Windows.UI.BrandMaster
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
            this.btnBrandMasterWarehouse = new System.Windows.Forms.Button();
            this.btnBrandMasterMaintenance = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBrandMasterPreshipment = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrandMasterWarehouse
            // 
            this.btnBrandMasterWarehouse.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBrandMasterWarehouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrandMasterWarehouse.Location = new System.Drawing.Point(157, 125);
            this.btnBrandMasterWarehouse.Name = "btnBrandMasterWarehouse";
            this.btnBrandMasterWarehouse.Size = new System.Drawing.Size(559, 56);
            this.btnBrandMasterWarehouse.TabIndex = 2;
            this.btnBrandMasterWarehouse.Text = "Brand Master - Warehouse";
            this.btnBrandMasterWarehouse.UseVisualStyleBackColor = true;
            this.btnBrandMasterWarehouse.Click += new System.EventHandler(this.btnBrandMasterWarehouse_Click);
            // 
            // btnBrandMasterMaintenance
            // 
            this.btnBrandMasterMaintenance.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBrandMasterMaintenance.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrandMasterMaintenance.Location = new System.Drawing.Point(157, 3);
            this.btnBrandMasterMaintenance.Name = "btnBrandMasterMaintenance";
            this.btnBrandMasterMaintenance.Size = new System.Drawing.Size(559, 55);
            this.btnBrandMasterMaintenance.TabIndex = 0;
            this.btnBrandMasterMaintenance.Text = "Brand Master - Maintenance";
            this.btnBrandMasterMaintenance.UseVisualStyleBackColor = true;
            this.btnBrandMasterMaintenance.Click += new System.EventHandler(this.btnBrandMasterMaintenance_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1270, 652);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Brand Master Main Menu";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnBrandMasterWarehouse, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnBrandMasterMaintenance, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBrandMasterPreshipment, 0, 1);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(198, 147);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(874, 185);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // btnBrandMasterPreshipment
            // 
            this.btnBrandMasterPreshipment.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBrandMasterPreshipment.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrandMasterPreshipment.Location = new System.Drawing.Point(157, 64);
            this.btnBrandMasterPreshipment.Name = "btnBrandMasterPreshipment";
            this.btnBrandMasterPreshipment.Size = new System.Drawing.Size(559, 55);
            this.btnBrandMasterPreshipment.TabIndex = 1;
            this.btnBrandMasterPreshipment.Text = "Brand Master - Preshipment";
            this.btnBrandMasterPreshipment.UseVisualStyleBackColor = true;
            this.btnBrandMasterPreshipment.Click += new System.EventHandler(this.btnBrandMasterPreshipment_Click);
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
            this.Text = "Brand Master";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrandMasterWarehouse;
        private System.Windows.Forms.Button btnBrandMasterMaintenance;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnBrandMasterPreshipment;
    }
}