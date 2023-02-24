namespace Hartalega.FloorSystem.Windows.UI.GIS
{
    partial class GloveInquiry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GloveInquiry));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbGloveType = new System.Windows.Forms.ComboBox();
            this.cmbSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.dgrvGloveInquiry = new System.Windows.Forms.DataGridView();
            this.Serial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GloveType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QCType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Kg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TenPcs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPcs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BinRefNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoOfDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgrvGloveInquiry)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Glove Type:";
            // 
            // cmbGloveType
            // 
            this.cmbGloveType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbGloveType.DropDownHeight = 400;
            this.cmbGloveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGloveType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbGloveType.FormattingEnabled = true;
            this.cmbGloveType.IntegralHeight = false;
            this.cmbGloveType.ItemHeight = 29;
            this.cmbGloveType.Items.AddRange(new object[] {
            "gt1",
            "gt2"});
            this.cmbGloveType.Location = new System.Drawing.Point(163, 9);
            this.cmbGloveType.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.cmbGloveType.Name = "cmbGloveType";
            this.cmbGloveType.Size = new System.Drawing.Size(436, 37);
            this.cmbGloveType.TabIndex = 0;
            // 
            // cmbSize
            // 
            this.cmbSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSize.DropDownHeight = 400;
            this.cmbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSize.FormattingEnabled = true;
            this.cmbSize.IntegralHeight = false;
            this.cmbSize.Items.AddRange(new object[] {
            "XL",
            "L"});
            this.cmbSize.Location = new System.Drawing.Point(293, 6);
            this.cmbSize.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.cmbSize.Name = "cmbSize";
            this.cmbSize.Size = new System.Drawing.Size(140, 37);
            this.cmbSize.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(216, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size:";
            // 
            // cmbLocation
            // 
            this.cmbLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(3, 6);
            this.cmbLocation.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(139, 37);
            this.cmbLocation.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 29);
            this.label3.TabIndex = 1;
            this.label3.Text = "Location:";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(605, 6);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(66, 38);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnExportToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportToExcel.Location = new System.Drawing.Point(605, 53);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(212, 44);
            this.btnExportToExcel.TabIndex = 7;
            this.btnExportToExcel.Text = "Export To Excel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // dgrvGloveInquiry
            // 
            this.dgrvGloveInquiry.AllowUserToAddRows = false;
            this.dgrvGloveInquiry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrvGloveInquiry.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Serial,
            this.GloveType,
            this.Size,
            this.QCType,
            this.Kg,
            this.TenPcs,
            this.TotalPcs,
            this.BatchDate,
            this.BinRefNo,
            this.NoOfDays});
            this.dgrvGloveInquiry.Location = new System.Drawing.Point(3, 3);
            this.dgrvGloveInquiry.Name = "dgrvGloveInquiry";
            this.dgrvGloveInquiry.ReadOnly = true;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgrvGloveInquiry.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgrvGloveInquiry.RowTemplate.Height = 42;
            this.dgrvGloveInquiry.Size = new System.Drawing.Size(968, 524);
            this.dgrvGloveInquiry.TabIndex = 0;
            this.dgrvGloveInquiry.TabStop = false;
            // 
            // Serial
            // 
            this.Serial.DataPropertyName = "SerialNumber";
            this.Serial.HeaderText = "Serial No";
            this.Serial.Name = "Serial";
            this.Serial.ReadOnly = true;
            this.Serial.Width = 103;
            // 
            // GloveType
            // 
            this.GloveType.DataPropertyName = "GloveType";
            this.GloveType.HeaderText = "Glove Type";
            this.GloveType.Name = "GloveType";
            this.GloveType.ReadOnly = true;
            this.GloveType.Width = 250;
            // 
            // Size
            // 
            this.Size.DataPropertyName = "Size";
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            this.Size.Width = 65;
            // 
            // QCType
            // 
            this.QCType.DataPropertyName = "QCType";
            this.QCType.HeaderText = "QC Type";
            this.QCType.Name = "QCType";
            this.QCType.ReadOnly = true;
            // 
            // Kg
            // 
            this.Kg.DataPropertyName = "BatchWeight";
            this.Kg.HeaderText = "Kg";
            this.Kg.Name = "Kg";
            this.Kg.ReadOnly = true;
            this.Kg.Width = 85;
            // 
            // TenPcs
            // 
            this.TenPcs.DataPropertyName = "TenPcsWeight";
            this.TenPcs.HeaderText = "10 Pcs";
            this.TenPcs.Name = "TenPcs";
            this.TenPcs.ReadOnly = true;
            this.TenPcs.Width = 85;
            // 
            // TotalPcs
            // 
            this.TotalPcs.DataPropertyName = "TotalPcs";
            this.TotalPcs.HeaderText = "Total Pcs";
            this.TotalPcs.Name = "TotalPcs";
            this.TotalPcs.ReadOnly = true;
            this.TotalPcs.Width = 105;
            // 
            // BatchDate
            // 
            this.BatchDate.DataPropertyName = "BatchDate";
            this.BatchDate.HeaderText = "Batch Date";
            this.BatchDate.Name = "BatchDate";
            this.BatchDate.ReadOnly = true;
            this.BatchDate.Width = 210;
            // 
            // BinRefNo
            // 
            this.BinRefNo.DataPropertyName = "BinId";
            this.BinRefNo.HeaderText = "Bin Ref No";
            this.BinRefNo.Name = "BinRefNo";
            this.BinRefNo.ReadOnly = true;
            this.BinRefNo.Width = 115;
            // 
            // NoOfDays
            // 
            this.NoOfDays.DataPropertyName = "NoOfDays";
            this.NoOfDays.HeaderText = "No Of Days";
            this.NoOfDays.Name = "NoOfDays";
            this.NoOfDays.ReadOnly = true;
            this.NoOfDays.Width = 127;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.dgrvGloveInquiry, 0, 0);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 132);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 530F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(974, 530);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.52977F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.48255F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.25667F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.93635F));
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbGloveType, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnGo, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnExportToExcel, 2, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(974, 100);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.cmbLocation, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbSize, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(163, 53);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 44);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // GloveInquiry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MinimumSize = new System.Drawing.Size(1024, 726);
            this.Name = "GloveInquiry";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Glove Inventory System - (Glove Detail Page)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.GloveInquiry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgrvGloveInquiry)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbGloveType;
        private System.Windows.Forms.ComboBox cmbSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnExportToExcel;
        private System.Windows.Forms.DataGridView dgrvGloveInquiry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Serial;
        private System.Windows.Forms.DataGridViewTextBoxColumn GloveType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn QCType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Kg;
        private System.Windows.Forms.DataGridViewTextBoxColumn TenPcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BinRefNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoOfDays;
    }
}