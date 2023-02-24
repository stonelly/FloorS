namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    partial class ChangeBatchCardForInner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeBatchCardForInner));
            this.txtItemNumber = new System.Windows.Forms.TextBox();
            this.txtPoNumber = new System.Windows.Forms.TextBox();
            this.txtInternalLotNo = new System.Windows.Forms.TextBox();
            this.txtNewBatchNumber = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dateControl1 = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNewSerialNumber = new System.Windows.Forms.TextBox();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbGroupId = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label26 = new System.Windows.Forms.Label();
            this.txtPrinter = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtStationNo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtItemNumber
            // 
            this.txtItemNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtItemNumber.Location = new System.Drawing.Point(248, 157);
            this.txtItemNumber.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtItemNumber.Name = "txtItemNumber";
            this.txtItemNumber.ReadOnly = true;
            this.txtItemNumber.Size = new System.Drawing.Size(291, 35);
            this.txtItemNumber.TabIndex = 7;
            this.txtItemNumber.TabStop = false;
            // 
            // txtPoNumber
            // 
            this.txtPoNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPoNumber.Location = new System.Drawing.Point(248, 107);
            this.txtPoNumber.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtPoNumber.Name = "txtPoNumber";
            this.txtPoNumber.ReadOnly = true;
            this.txtPoNumber.Size = new System.Drawing.Size(667, 35);
            this.txtPoNumber.TabIndex = 5;
            this.txtPoNumber.TabStop = false;
            // 
            // txtInternalLotNo
            // 
            this.txtInternalLotNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInternalLotNo.BackColor = System.Drawing.SystemColors.Window;
            this.txtInternalLotNo.Location = new System.Drawing.Point(248, 57);
            this.txtInternalLotNo.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtInternalLotNo.MaxLength = 15;
            this.txtInternalLotNo.Name = "txtInternalLotNo";
            this.txtInternalLotNo.Size = new System.Drawing.Size(291, 35);
            this.txtInternalLotNo.TabIndex = 3;
            this.txtInternalLotNo.Leave += new System.EventHandler(this.txtInternalLotNumber_Leave);
            // 
            // txtNewBatchNumber
            // 
            this.txtNewBatchNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNewBatchNumber.Location = new System.Drawing.Point(248, 357);
            this.txtNewBatchNumber.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtNewBatchNumber.Name = "txtNewBatchNumber";
            this.txtNewBatchNumber.ReadOnly = true;
            this.txtNewBatchNumber.Size = new System.Drawing.Size(291, 35);
            this.txtNewBatchNumber.TabIndex = 15;
            this.txtNewBatchNumber.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(22, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 500);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Change Batch Card For Inner";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.04F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dateControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNewBatchNumber, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtItemNumber, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtInternalLotNo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNewSerialNumber, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtItemName, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtPoNumber, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbGroupId, 1, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(954, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(97, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pack Date:";
            // 
            // dateControl1
            // 
            this.dateControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateControl1.AutoSize = true;
            this.dateControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dateControl1.DateValue = new System.DateTime(((long)(0)));
            this.dateControl1.Location = new System.Drawing.Point(241, 5);
            this.dateControl1.Name = "dateControl1";
            this.dateControl1.Size = new System.Drawing.Size(302, 40);
            this.dateControl1.TabIndex = 1;
            this.dateControl1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(44, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Internal Lot No:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(49, 360);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(186, 29);
            this.label9.TabIndex = 14;
            this.label9.Text = "New Batch No:";
            // 
            // txtNewSerialNumber
            // 
            this.txtNewSerialNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNewSerialNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtNewSerialNumber.Location = new System.Drawing.Point(248, 307);
            this.txtNewSerialNumber.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtNewSerialNumber.MaxLength = 10;
            this.txtNewSerialNumber.Name = "txtNewSerialNumber";
            this.txtNewSerialNumber.Size = new System.Drawing.Size(291, 35);
            this.txtNewSerialNumber.TabIndex = 13;
            this.txtNewSerialNumber.Leave += new System.EventHandler(this.txtNewSerialNo_Leave);
            // 
            // txtItemName
            // 
            this.txtItemName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtItemName.Location = new System.Drawing.Point(248, 207);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(660, 35);
            this.txtItemName.TabIndex = 9;
            this.txtItemName.TabStop = false;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(45, 310);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(190, 29);
            this.label7.TabIndex = 12;
            this.label7.Text = "New Serial No:";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(89, 210);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(146, 29);
            this.label12.TabIndex = 8;
            this.label12.Text = "Item Name:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(114, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 29);
            this.label4.TabIndex = 10;
            this.label4.Text = "Group Id:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(178, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "PO:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(124, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 29);
            this.label8.TabIndex = 6;
            this.label8.Text = "Item No:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(646, 403);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(305, 44);
            this.flowLayoutPanel1.TabIndex = 16;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 39);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(139, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbGroupId
            // 
            this.cmbGroupId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbGroupId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbGroupId.FormattingEnabled = true;
            this.cmbGroupId.IntegralHeight = false;
            this.cmbGroupId.Location = new System.Drawing.Point(248, 256);
            this.cmbGroupId.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.cmbGroupId.Name = "cmbGroupId";
            this.cmbGroupId.Size = new System.Drawing.Size(291, 37);
            this.cmbGroupId.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(22, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(961, 150);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Program Messages";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.20755F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.63522F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.72327F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.18868F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.47379F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Controls.Add(this.label26, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtPrinter, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label24, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLocation, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label25, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtStationNo, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(954, 80);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label26
            // 
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label26.Location = new System.Drawing.Point(4, 25);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(119, 29);
            this.label26.TabIndex = 0;
            this.label26.Text = "Location:";
            // 
            // txtPrinter
            // 
            this.txtPrinter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPrinter.BackColor = System.Drawing.SystemColors.Menu;
            this.txtPrinter.Enabled = false;
            this.txtPrinter.Location = new System.Drawing.Point(797, 22);
            this.txtPrinter.Name = "txtPrinter";
            this.txtPrinter.Size = new System.Drawing.Size(154, 35);
            this.txtPrinter.TabIndex = 5;
            this.txtPrinter.TabStop = false;
            // 
            // label24
            // 
            this.label24.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label24.Location = new System.Drawing.Point(681, 25);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(110, 29);
            this.label24.TabIndex = 4;
            this.label24.Text = "INKJET:";
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLocation.BackColor = System.Drawing.SystemColors.Menu;
            this.txtLocation.Enabled = false;
            this.txtLocation.Location = new System.Drawing.Point(129, 22);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(105, 35);
            this.txtLocation.TabIndex = 1;
            this.txtLocation.TabStop = false;
            // 
            // label25
            // 
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label25.Location = new System.Drawing.Point(242, 25);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(142, 29);
            this.label25.TabIndex = 2;
            this.label25.Text = "Station No:";
            // 
            // txtStationNo
            // 
            this.txtStationNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtStationNo.BackColor = System.Drawing.SystemColors.Menu;
            this.txtStationNo.Enabled = false;
            this.txtStationNo.Location = new System.Drawing.Point(390, 22);
            this.txtStationNo.Name = "txtStationNo";
            this.txtStationNo.Size = new System.Drawing.Size(282, 35);
            this.txtStationNo.TabIndex = 3;
            this.txtStationNo.TabStop = false;
            // 
            // ChangeBatchCardForInner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "ChangeBatchCardForInner";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Batch Card For Inner";
            this.Load += new System.EventHandler(this.ChangeBatchCardForInner_Load);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtItemNumber;
        private System.Windows.Forms.TextBox txtPoNumber;
        private System.Windows.Forms.TextBox txtInternalLotNo;
        private System.Windows.Forms.TextBox txtNewBatchNumber;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNewSerialNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtPrinter;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtStationNo;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtLocation;
        //private DateControl dateControl1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private DateControl dateControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cmbGroupId;
    }
}