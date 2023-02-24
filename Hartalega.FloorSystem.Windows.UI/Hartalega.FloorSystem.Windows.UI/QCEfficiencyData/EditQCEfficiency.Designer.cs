namespace Hartalega.FloorSystem.Windows.UI.QCEfficiencyData
{
    partial class EditQCEfficiency
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbBatchStatus = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbInnerBoxCount = new System.Windows.Forms.TextBox();
            this.cbQcType = new System.Windows.Forms.ComboBox();
            this.tbReworkReason = new System.Windows.Forms.TextBox();
            this.cbPackingSize = new System.Windows.Forms.ComboBox();
            this.tbId = new System.Windows.Forms.TextBox();
            this.tbTenPcsWeight = new System.Windows.Forms.TextBox();
            this.cbReason = new System.Windows.Forms.ComboBox();
            this.cbRework = new System.Windows.Forms.CheckBox();
            this.gridEmployee = new System.Windows.Forms.DataGridView();
            this.Employee_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Employee_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbGroup = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbNoPerson = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbGlove = new System.Windows.Forms.TextBox();
            this.lblSerialNo = new System.Windows.Forms.Label();
            this.lblBatchNo = new System.Windows.Forms.Label();
            this.lblGloveType = new System.Windows.Forms.Label();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.tbBatchNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbBrand = new System.Windows.Forms.TextBox();
            this.tbBatchWeight = new System.Windows.Forms.TextBox();
            this.cbBatchStatus = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEmployee)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(467, 606);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 39);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(323, 606);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(127, 39);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Controls.Add(this.cbRework);
            this.groupBox2.Controls.Add(this.gridEmployee);
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(960, 420);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label11, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbBatchStatus, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.label12, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tbInnerBoxCount, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.cbQcType, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbReworkReason, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cbPackingSize, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tbId, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.tbTenPcsWeight, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.cbReason, 1, 3);
            this.tableLayoutPanel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel3.Location = new System.Drawing.Point(14, 245);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.12698F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.53968F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(932, 169);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(121, 134);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 29);
            this.label10.TabIndex = 23;
            this.label10.Text = "Reason:";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(572, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(124, 29);
            this.label11.TabIndex = 19;
            this.label11.Text = "QC Type:";
            // 
            // tbBatchStatus
            // 
            this.tbBatchStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbBatchStatus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbBatchStatus.Enabled = false;
            this.tbBatchStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBatchStatus.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbBatchStatus.Location = new System.Drawing.Point(702, 90);
            this.tbBatchStatus.Name = "tbBatchStatus";
            this.tbBatchStatus.Size = new System.Drawing.Size(227, 35);
            this.tbBatchStatus.TabIndex = 10;
            this.tbBatchStatus.Visible = false;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(25, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(205, 29);
            this.label12.TabIndex = 0;
            this.label12.Text = "Inner Box Count:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(25, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(205, 29);
            this.label9.TabIndex = 17;
            this.label9.Text = "Rework Reason:";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(59, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(171, 29);
            this.label13.TabIndex = 5;
            this.label13.Text = "Packing Size:";
            // 
            // tbInnerBoxCount
            // 
            this.tbInnerBoxCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbInnerBoxCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInnerBoxCount.Location = new System.Drawing.Point(236, 90);
            this.tbInnerBoxCount.Name = "tbInnerBoxCount";
            this.tbInnerBoxCount.Size = new System.Drawing.Size(227, 35);
            this.tbInnerBoxCount.TabIndex = 22;
            this.tbInnerBoxCount.Leave += new System.EventHandler(this.tbInnerBoxCount_Leave);
            // 
            // cbQcType
            // 
            this.cbQcType.Enabled = false;
            this.cbQcType.FormattingEnabled = true;
            this.cbQcType.Location = new System.Drawing.Point(702, 3);
            this.cbQcType.Name = "cbQcType";
            this.cbQcType.Size = new System.Drawing.Size(227, 37);
            this.cbQcType.TabIndex = 20;
            // 
            // tbReworkReason
            // 
            this.tbReworkReason.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbReworkReason.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbReworkReason.Enabled = false;
            this.tbReworkReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReworkReason.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbReworkReason.Location = new System.Drawing.Point(236, 4);
            this.tbReworkReason.Name = "tbReworkReason";
            this.tbReworkReason.Size = new System.Drawing.Size(227, 35);
            this.tbReworkReason.TabIndex = 18;
            // 
            // cbPackingSize
            // 
            this.cbPackingSize.FormattingEnabled = true;
            this.cbPackingSize.Location = new System.Drawing.Point(236, 47);
            this.cbPackingSize.Name = "cbPackingSize";
            this.cbPackingSize.Size = new System.Drawing.Size(227, 37);
            this.cbPackingSize.TabIndex = 21;
            this.cbPackingSize.Leave += new System.EventHandler(this.cbPackingSize_Leave);
            // 
            // tbId
            // 
            this.tbId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbId.Location = new System.Drawing.Point(469, 90);
            this.tbId.Name = "tbId";
            this.tbId.Size = new System.Drawing.Size(227, 35);
            this.tbId.TabIndex = 15;
            this.tbId.Visible = false;
            // 
            // tbTenPcsWeight
            // 
            this.tbTenPcsWeight.Location = new System.Drawing.Point(469, 132);
            this.tbTenPcsWeight.Name = "tbTenPcsWeight";
            this.tbTenPcsWeight.Size = new System.Drawing.Size(227, 35);
            this.tbTenPcsWeight.TabIndex = 22;
            this.tbTenPcsWeight.Visible = false;
            // 
            // cbReason
            // 
            this.cbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReason.FormattingEnabled = true;
            this.cbReason.Location = new System.Drawing.Point(236, 132);
            this.cbReason.Name = "cbReason";
            this.cbReason.Size = new System.Drawing.Size(227, 37);
            this.cbReason.TabIndex = 24;
            // 
            // cbRework
            // 
            this.cbRework.AutoSize = true;
            this.cbRework.Enabled = false;
            this.cbRework.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRework.Location = new System.Drawing.Point(273, 217);
            this.cbRework.Name = "cbRework";
            this.cbRework.Size = new System.Drawing.Size(85, 22);
            this.cbRework.TabIndex = 3;
            this.cbRework.Text = "Rework";
            this.cbRework.UseVisualStyleBackColor = true;
            // 
            // gridEmployee
            // 
            this.gridEmployee.AllowUserToAddRows = false;
            this.gridEmployee.AllowUserToDeleteRows = false;
            this.gridEmployee.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gridEmployee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEmployee.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Employee_ID,
            this.Employee_Name});
            this.gridEmployee.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridEmployee.Location = new System.Drawing.Point(504, 19);
            this.gridEmployee.Name = "gridEmployee";
            this.gridEmployee.Size = new System.Drawing.Size(441, 191);
            this.gridEmployee.TabIndex = 2;
            // 
            // Employee_ID
            // 
            this.Employee_ID.HeaderText = "Employee ID";
            this.Employee_ID.Name = "Employee_ID";
            // 
            // Employee_Name
            // 
            this.Employee_Name.HeaderText = "Employee Name";
            this.Employee_Name.Name = "Employee_Name";
            this.Employee_Name.Width = 250;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbGroup, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbNoPerson, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.dtpDate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtpStartTime, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dtpEndTime, 1, 3);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(466, 191);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(97, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 29);
            this.label8.TabIndex = 12;
            this.label8.Text = "End Time:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(86, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 29);
            this.label6.TabIndex = 0;
            this.label6.Text = "No Person:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(156, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 29);
            this.label4.TabIndex = 0;
            this.label4.Text = "Date:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(138, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 29);
            this.label5.TabIndex = 0;
            this.label5.Text = "Group:";
            // 
            // tbGroup
            // 
            this.tbGroup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbGroup.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbGroup.Enabled = false;
            this.tbGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGroup.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbGroup.Location = new System.Drawing.Point(236, 41);
            this.tbGroup.Name = "tbGroup";
            this.tbGroup.Size = new System.Drawing.Size(227, 35);
            this.tbGroup.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(89, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(141, 29);
            this.label7.TabIndex = 11;
            this.label7.Text = "Start Time:";
            // 
            // tbNoPerson
            // 
            this.tbNoPerson.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbNoPerson.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbNoPerson.Enabled = false;
            this.tbNoPerson.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNoPerson.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbNoPerson.Location = new System.Drawing.Point(236, 155);
            this.tbNoPerson.Name = "tbNoPerson";
            this.tbNoPerson.Size = new System.Drawing.Size(227, 35);
            this.tbNoPerson.TabIndex = 14;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(236, 3);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(227, 29);
            this.dtpDate.TabIndex = 15;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(236, 79);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(227, 29);
            this.dtpStartTime.TabIndex = 16;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(236, 117);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(227, 29);
            this.dtpEndTime.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(960, 161);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.tbGlove, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSerialNo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblBatchNo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblGloveType, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtSerialNo, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbBatchNo, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbBrand, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbBatchWeight, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.cbBatchStatus, 3, 2);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(14, 20);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(932, 129);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tbGlove
            // 
            this.tbGlove.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbGlove.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbGlove.Enabled = false;
            this.tbGlove.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGlove.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbGlove.Location = new System.Drawing.Point(702, 4);
            this.tbGlove.Name = "tbGlove";
            this.tbGlove.Size = new System.Drawing.Size(227, 35);
            this.tbGlove.TabIndex = 11;
            // 
            // lblSerialNo
            // 
            this.lblSerialNo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSerialNo.AutoSize = true;
            this.lblSerialNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerialNo.Location = new System.Drawing.Point(100, 7);
            this.lblSerialNo.Name = "lblSerialNo";
            this.lblSerialNo.Size = new System.Drawing.Size(130, 29);
            this.lblSerialNo.TabIndex = 0;
            this.lblSerialNo.Text = "Serial No:";
            // 
            // lblBatchNo
            // 
            this.lblBatchNo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBatchNo.AutoSize = true;
            this.lblBatchNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatchNo.Location = new System.Drawing.Point(124, 50);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new System.Drawing.Size(106, 29);
            this.lblBatchNo.TabIndex = 0;
            this.lblBatchNo.Text = "Batch #:";
            // 
            // lblGloveType
            // 
            this.lblGloveType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGloveType.AutoSize = true;
            this.lblGloveType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGloveType.Location = new System.Drawing.Point(57, 93);
            this.lblGloveType.Name = "lblGloveType";
            this.lblGloveType.Size = new System.Drawing.Size(173, 29);
            this.lblGloveType.TabIndex = 0;
            this.lblGloveType.Text = "Batch Weight:";
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSerialNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerialNo.Location = new System.Drawing.Point(236, 4);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(227, 35);
            this.txtSerialNo.TabIndex = 1;
            this.txtSerialNo.Leave += new System.EventHandler(this.tbSerialNo_Leave);
            // 
            // tbBatchNo
            // 
            this.tbBatchNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbBatchNo.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbBatchNo.Enabled = false;
            this.tbBatchNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBatchNo.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbBatchNo.Location = new System.Drawing.Point(236, 47);
            this.tbBatchNo.Name = "tbBatchNo";
            this.tbBatchNo.Size = new System.Drawing.Size(227, 35);
            this.tbBatchNo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(608, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Glove:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(607, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 29);
            this.label3.TabIndex = 7;
            this.label3.Text = "Brand:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(532, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 29);
            this.label2.TabIndex = 6;
            this.label2.Text = "Batch Status:";
            // 
            // tbBrand
            // 
            this.tbBrand.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbBrand.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbBrand.Enabled = false;
            this.tbBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBrand.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbBrand.Location = new System.Drawing.Point(702, 47);
            this.tbBrand.Name = "tbBrand";
            this.tbBrand.Size = new System.Drawing.Size(227, 35);
            this.tbBrand.TabIndex = 21;
            // 
            // tbBatchWeight
            // 
            this.tbBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbBatchWeight.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbBatchWeight.Enabled = false;
            this.tbBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBatchWeight.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbBatchWeight.Location = new System.Drawing.Point(236, 90);
            this.tbBatchWeight.Name = "tbBatchWeight";
            this.tbBatchWeight.Size = new System.Drawing.Size(227, 35);
            this.tbBatchWeight.TabIndex = 13;
            // 
            // cbBatchStatus
            // 
            this.cbBatchStatus.FormattingEnabled = true;
            this.cbBatchStatus.Location = new System.Drawing.Point(702, 89);
            this.cbBatchStatus.Name = "cbBatchStatus";
            this.cbBatchStatus.Size = new System.Drawing.Size(227, 37);
            this.cbBatchStatus.TabIndex = 22;
            // 
            // EditQCEfficiency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(944, 685);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditQCEfficiency";
            this.Text = "EditQCEfficiency";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.EditQCEfficiency_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEmployee)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblSerialNo;
        private System.Windows.Forms.Label lblBatchNo;
        private System.Windows.Forms.Label lblGloveType;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.TextBox tbBatchNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbBatchStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView gridEmployee;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbGroup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Employee_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Employee_Name;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbReworkReason;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbInnerBoxCount;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox cbRework;
        private System.Windows.Forms.TextBox tbNoPerson;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbBatchWeight;
        private System.Windows.Forms.TextBox tbGlove;
        private System.Windows.Forms.TextBox tbBrand;
        private System.Windows.Forms.ComboBox cbQcType;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cbPackingSize;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.ComboBox cbBatchStatus;
        private System.Windows.Forms.TextBox tbTenPcsWeight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbReason;
    }
}