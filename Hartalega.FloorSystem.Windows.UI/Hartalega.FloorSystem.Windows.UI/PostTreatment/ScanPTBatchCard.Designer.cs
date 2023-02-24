namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{
    partial class ScanPTBatchCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanPTBatchCard));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblReworkCount = new System.Windows.Forms.Label();
            this.lblReworkReason = new System.Windows.Forms.Label();
            this.txtReworkCount = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBatchWeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTenPcs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtQCType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbReworkProcess = new System.Windows.Forms.ComboBox();
            this.cmbReworkReason = new System.Windows.Forms.ComboBox();
            this.lblReworkProcess = new System.Windows.Forms.Label();
            this.txtQCTypeDesc = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbShift = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBatchWeight = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTenPcs = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.usrDateControl = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(142, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.Validated += new System.EventHandler(this.btnCancel_Validated);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(4, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 39);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLocation.Location = new System.Drawing.Point(295, 10);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(214, 35);
            this.txtLocation.TabIndex = 0;
            this.txtLocation.TabStop = false;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(168, 13);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(119, 29);
            this.label13.TabIndex = 24;
            this.label13.Text = "Location:";
            // 
            // lblReworkCount
            // 
            this.lblReworkCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReworkCount.AutoSize = true;
            this.lblReworkCount.Location = new System.Drawing.Point(84, 276);
            this.lblReworkCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReworkCount.Name = "lblReworkCount";
            this.lblReworkCount.Size = new System.Drawing.Size(184, 29);
            this.lblReworkCount.TabIndex = 18;
            this.lblReworkCount.Text = "Rework Count:";
            // 
            // lblReworkReason
            // 
            this.lblReworkReason.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReworkReason.AutoSize = true;
            this.lblReworkReason.Location = new System.Drawing.Point(63, 193);
            this.lblReworkReason.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReworkReason.Name = "lblReworkReason";
            this.lblReworkReason.Size = new System.Drawing.Size(205, 29);
            this.lblReworkReason.TabIndex = 16;
            this.lblReworkReason.Text = "Rework Reason:";
            // 
            // txtReworkCount
            // 
            this.txtReworkCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtReworkCount.Location = new System.Drawing.Point(276, 276);
            this.txtReworkCount.Margin = new System.Windows.Forms.Padding(4);
            this.txtReworkCount.Name = "txtReworkCount";
            this.txtReworkCount.ReadOnly = true;
            this.txtReworkCount.Size = new System.Drawing.Size(438, 35);
            this.txtReworkCount.TabIndex = 7;
            this.txtReworkCount.TabStop = false;
            this.txtReworkCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 41);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(264, 29);
            this.label10.TabIndex = 14;
            this.label10.Text = "QC Type Description:";
            // 
            // txtBatchWeight
            // 
            this.txtBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchWeight.Location = new System.Drawing.Point(4, 4);
            this.txtBatchWeight.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchWeight.Name = "txtBatchWeight";
            this.txtBatchWeight.Size = new System.Drawing.Size(438, 35);
            this.txtBatchWeight.TabIndex = 4;
            this.txtBatchWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBatchWeight.Enter += new System.EventHandler(this.txtBatchWeight_Enter);
            this.txtBatchWeight.Leave += new System.EventHandler(this.txtBatchWeight_Leave);
            this.txtBatchWeight.Validated += new System.EventHandler(this.txtBatchWeight_Validated);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(133, 153);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 29);
            this.label5.TabIndex = 12;
            this.label5.Text = "Batch(Kg):";
            // 
            // txtTenPcs
            // 
            this.txtTenPcs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTenPcs.Location = new System.Drawing.Point(4, 4);
            this.txtTenPcs.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenPcs.Name = "txtTenPcs";
            this.txtTenPcs.Size = new System.Drawing.Size(438, 35);
            this.txtTenPcs.TabIndex = 3;
            this.txtTenPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTenPcs.Enter += new System.EventHandler(this.txtTenPcs_Enter);
            this.txtTenPcs.Validated += new System.EventHandler(this.txtTenPcs_Leave);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(137, 115);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 29);
            this.label6.TabIndex = 10;
            this.label6.Text = "10 Pcs(g):";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(196, 78);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 29);
            this.label7.TabIndex = 8;
            this.label7.Text = "Shift:";
            // 
            // txtQCType
            // 
            this.txtQCType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtQCType.Location = new System.Drawing.Point(276, 4);
            this.txtQCType.Margin = new System.Windows.Forms.Padding(4);
            this.txtQCType.Name = "txtQCType";
            this.txtQCType.ReadOnly = true;
            this.txtQCType.Size = new System.Drawing.Size(438, 35);
            this.txtQCType.TabIndex = 0;
            this.txtQCType.TabStop = false;
            this.txtQCType.TextChanged += new System.EventHandler(this.txtQCType_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "QC Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(668, -28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 240);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Size = new System.Drawing.Size(930, 410);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choice Selection";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.60398F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.39602F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbReworkProcess, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtReworkCount, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.cmbReworkReason, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblReworkProcess, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtQCTypeDesc, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblReworkReason, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblReworkCount, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtQCType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbShift, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 38);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.47048F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.47285F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.3496F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.67373F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46943F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.1643F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.1643F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46943F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.76588F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(890, 360);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // cmbReworkProcess
            // 
            this.cmbReworkProcess.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbReworkProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReworkProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReworkProcess.FormattingEnabled = true;
            this.cmbReworkProcess.Location = new System.Drawing.Point(276, 233);
            this.cmbReworkProcess.Margin = new System.Windows.Forms.Padding(4);
            this.cmbReworkProcess.Name = "cmbReworkProcess";
            this.cmbReworkProcess.Size = new System.Drawing.Size(438, 37);
            this.cmbReworkProcess.TabIndex = 6;
            // 
            // cmbReworkReason
            // 
            this.cmbReworkReason.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbReworkReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReworkReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReworkReason.FormattingEnabled = true;
            this.cmbReworkReason.Location = new System.Drawing.Point(276, 190);
            this.cmbReworkReason.Margin = new System.Windows.Forms.Padding(4);
            this.cmbReworkReason.Name = "cmbReworkReason";
            this.cmbReworkReason.Size = new System.Drawing.Size(607, 37);
            this.cmbReworkReason.TabIndex = 5;
            // 
            // lblReworkProcess
            // 
            this.lblReworkProcess.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReworkProcess.AutoSize = true;
            this.lblReworkProcess.Location = new System.Drawing.Point(57, 236);
            this.lblReworkProcess.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReworkProcess.Name = "lblReworkProcess";
            this.lblReworkProcess.Size = new System.Drawing.Size(211, 29);
            this.lblReworkProcess.TabIndex = 22;
            this.lblReworkProcess.Text = "Rework Process:";
            // 
            // txtQCTypeDesc
            // 
            this.txtQCTypeDesc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtQCTypeDesc.Location = new System.Drawing.Point(276, 41);
            this.txtQCTypeDesc.Margin = new System.Windows.Forms.Padding(4);
            this.txtQCTypeDesc.Name = "txtQCTypeDesc";
            this.txtQCTypeDesc.ReadOnly = true;
            this.txtQCTypeDesc.Size = new System.Drawing.Size(438, 35);
            this.txtQCTypeDesc.TabIndex = 1;
            this.txtQCTypeDesc.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(611, 313);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(276, 42);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // cmbShift
            // 
            this.cmbShift.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbShift.FormattingEnabled = true;
            this.cmbShift.Location = new System.Drawing.Point(276, 78);
            this.cmbShift.Margin = new System.Windows.Forms.Padding(4);
            this.cmbShift.Name = "cmbShift";
            this.cmbShift.Size = new System.Drawing.Size(438, 37);
            this.cmbShift.TabIndex = 2;
            this.cmbShift.Leave += new System.EventHandler(this.cmbShift_Leave);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.txtBatchWeight);
            this.flowLayoutPanel2.Controls.Add(this.lblBatchWeight);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(275, 152);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(612, 31);
            this.flowLayoutPanel2.TabIndex = 23;
            // 
            // lblBatchWeight
            // 
            this.lblBatchWeight.AutoSize = true;
            this.lblBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatchWeight.ForeColor = System.Drawing.Color.Red;
            this.lblBatchWeight.Location = new System.Drawing.Point(449, 0);
            this.lblBatchWeight.Name = "lblBatchWeight";
            this.lblBatchWeight.Size = new System.Drawing.Size(160, 18);
            this.lblBatchWeight.TabIndex = 5;
            this.lblBatchWeight.Text = "Weight out of range!";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.txtTenPcs);
            this.flowLayoutPanel3.Controls.Add(this.lblTenPcs);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(275, 114);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(612, 32);
            this.flowLayoutPanel3.TabIndex = 24;
            // 
            // lblTenPcs
            // 
            this.lblTenPcs.AutoSize = true;
            this.lblTenPcs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenPcs.ForeColor = System.Drawing.Color.Red;
            this.lblTenPcs.Location = new System.Drawing.Point(449, 0);
            this.lblTenPcs.Name = "lblTenPcs";
            this.lblTenPcs.Size = new System.Drawing.Size(160, 18);
            this.lblTenPcs.TabIndex = 4;
            this.lblTenPcs.Text = "Weight out of range!";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(22, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(961, 660);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scan PT Batch Cards";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.52763F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.16035F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.378293F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.87882F));
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.usrDateControl, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtLocation, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label15, 2, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(7, 27);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(923, 55);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // usrDateControl
            // 
            this.usrDateControl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.usrDateControl.DateValue = new System.DateTime(((long)(0)));
            this.usrDateControl.Location = new System.Drawing.Point(615, 8);
            this.usrDateControl.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.usrDateControl.Name = "usrDateControl";
            this.usrDateControl.Size = new System.Drawing.Size(300, 38);
            this.usrDateControl.TabIndex = 1;
            this.usrDateControl.TabStop = false;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(522, 13);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 29);
            this.label15.TabIndex = 27;
            this.label15.Text = "Date:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.groupBox3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox3.Location = new System.Drawing.Point(5, 80);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(930, 161);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Batch Info";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.66765F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.33234F));
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSize, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtBatchNo, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtSerialNo, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(20, 35);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(890, 119);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Serial No:";
            // 
            // txtSize
            // 
            this.txtSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSize.Location = new System.Drawing.Point(276, 82);
            this.txtSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(438, 35);
            this.txtSize.TabIndex = 2;
            this.txtSize.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 29);
            this.label3.TabIndex = 6;
            this.label3.Text = "Batch No:";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchNo.Location = new System.Drawing.Point(276, 43);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ReadOnly = true;
            this.txtBatchNo.Size = new System.Drawing.Size(438, 35);
            this.txtBatchNo.TabIndex = 1;
            this.txtBatchNo.TabStop = false;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(197, 84);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 29);
            this.label12.TabIndex = 24;
            this.label12.Text = "Size:";
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSerialNo.Location = new System.Drawing.Point(276, 4);
            this.txtSerialNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtSerialNo.MaxLength = 10;
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(438, 35);
            this.txtSerialNo.TabIndex = 0;
            this.txtSerialNo.Leave += new System.EventHandler(this.txtSerialNo_Leave);
            // 
            // ScanPTBatchCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(961, 662);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MinimumSize = new System.Drawing.Size(946, 630);
            this.Name = "ScanPTBatchCard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scan PT Batch Card";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ScanPTBatchCard_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblReworkCount;
        private System.Windows.Forms.Label lblReworkReason;
        private System.Windows.Forms.TextBox txtReworkCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBatchWeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTenPcs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtQCType;
        private System.Windows.Forms.Label label4;
      
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQCTypeDesc;
        private System.Windows.Forms.Label lblReworkProcess;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbReworkReason;
        private System.Windows.Forms.ComboBox cmbReworkProcess;
        private System.Windows.Forms.ComboBox cmbShift;
        private DateControl usrDateControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label lblTenPcs;
        private System.Windows.Forms.Label lblBatchWeight;
        
    }
}