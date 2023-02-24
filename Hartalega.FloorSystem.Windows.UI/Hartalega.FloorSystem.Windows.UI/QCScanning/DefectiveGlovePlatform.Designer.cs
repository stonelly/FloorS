namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    partial class DefectiveGlovePlatform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefectiveGlovePlatform));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtBatchWeight = new System.Windows.Forms.TextBox();
            this.lblBatch = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.grpBoxScreen = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.usrDateControl = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPcsCount = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbReason = new System.Windows.Forms.ComboBox();
            this.lblPcsCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtQCGroup = new System.Windows.Forms.TextBox();
            this.txtGloveType = new System.Windows.Forms.TextBox();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbDefectType = new System.Windows.Forms.ComboBox();
            this.grpBoxScreen.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(142, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(4, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 38);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtBatchWeight
            // 
            this.txtBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchWeight.Location = new System.Drawing.Point(236, 320);
            this.txtBatchWeight.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchWeight.Name = "txtBatchWeight";
            this.txtBatchWeight.Size = new System.Drawing.Size(400, 35);
            this.txtBatchWeight.TabIndex = 13;
            this.txtBatchWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBatchWeight.Enter += new System.EventHandler(this.txtBatchWeight_Enter);
            this.txtBatchWeight.Validated += new System.EventHandler(this.txtBatchWeight_Leave);
            // 
            // lblBatch
            // 
            this.lblBatch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBatch.AutoSize = true;
            this.lblBatch.Location = new System.Drawing.Point(93, 323);
            this.lblBatch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(135, 29);
            this.lblBatch.TabIndex = 12;
            this.lblBatch.Text = "Batch(Kg):";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 219);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 29);
            this.label4.TabIndex = 8;
            this.label4.Text = "Defect Type:";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(45, 11);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 29);
            this.label15.TabIndex = 0;
            this.label15.Text = "Date:";
            // 
            // grpBoxScreen
            // 
            this.grpBoxScreen.Controls.Add(this.tableLayoutPanel3);
            this.grpBoxScreen.Controls.Add(this.groupBox3);
            this.grpBoxScreen.Location = new System.Drawing.Point(22, 10);
            this.grpBoxScreen.Margin = new System.Windows.Forms.Padding(4);
            this.grpBoxScreen.Name = "grpBoxScreen";
            this.grpBoxScreen.Padding = new System.Windows.Forms.Padding(4);
            this.grpBoxScreen.Size = new System.Drawing.Size(961, 660);
            this.grpBoxScreen.TabIndex = 0;
            this.grpBoxScreen.TabStop = false;
            this.grpBoxScreen.Text = "Defective Glove (Big Scale)";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.73952F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.26048F));
            this.tableLayoutPanel3.Controls.Add(this.label15, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.usrDateControl, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(511, 34);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(430, 51);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // usrDateControl
            // 
            this.usrDateControl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.usrDateControl.DateValue = new System.DateTime(((long)(0)));
            this.usrDateControl.Location = new System.Drawing.Point(126, 6);
            this.usrDateControl.Name = "usrDateControl";
            this.usrDateControl.Size = new System.Drawing.Size(301, 38);
            this.usrDateControl.TabIndex = 1;
            this.usrDateControl.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Location = new System.Drawing.Point(15, 97);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(940, 550);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Batch Info";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.04F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPcsCount, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label16, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbReason, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblPcsCount, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtBatchWeight, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtQCGroup, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtGloveType, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBatch, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtSerialNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBatchNo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbDefectType, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 35);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(932, 471);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Serial No:";
            // 
            // txtPcsCount
            // 
            this.txtPcsCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPcsCount.Location = new System.Drawing.Point(236, 372);
            this.txtPcsCount.Margin = new System.Windows.Forms.Padding(4);
            this.txtPcsCount.Name = "txtPcsCount";
            this.txtPcsCount.Size = new System.Drawing.Size(400, 35);
            this.txtPcsCount.TabIndex = 4;
            this.txtPcsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPcsCount.Visible = false;
            this.txtPcsCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPcsCount_KeyPress);
            this.txtPcsCount.Validated += new System.EventHandler(this.txtPcsCount_Validated);
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(102, 63);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(126, 29);
            this.label16.TabIndex = 2;
            this.label16.Text = "Batch No:";
            // 
            // cmbReason
            // 
            this.cmbReason.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReason.FormattingEnabled = true;
            this.cmbReason.Location = new System.Drawing.Point(235, 272);
            this.cmbReason.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.Size = new System.Drawing.Size(400, 37);
            this.cmbReason.TabIndex = 3;
            this.cmbReason.Leave += new System.EventHandler(this.cmbReason_Leave);
            // 
            // lblPcsCount
            // 
            this.lblPcsCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPcsCount.AutoSize = true;
            this.lblPcsCount.Location = new System.Drawing.Point(90, 375);
            this.lblPcsCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPcsCount.Name = "lblPcsCount";
            this.lblPcsCount.Size = new System.Drawing.Size(138, 29);
            this.lblPcsCount.TabIndex = 14;
            this.lblPcsCount.Text = "Pcs Count:";
            this.lblPcsCount.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "Glove Type:";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(91, 167);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(137, 29);
            this.label12.TabIndex = 6;
            this.label12.Text = "QC Group:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(119, 271);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 29);
            this.label6.TabIndex = 10;
            this.label6.Text = "Reason:";
            // 
            // txtQCGroup
            // 
            this.txtQCGroup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtQCGroup.Location = new System.Drawing.Point(236, 164);
            this.txtQCGroup.Margin = new System.Windows.Forms.Padding(4);
            this.txtQCGroup.Name = "txtQCGroup";
            this.txtQCGroup.ReadOnly = true;
            this.txtQCGroup.Size = new System.Drawing.Size(400, 35);
            this.txtQCGroup.TabIndex = 7;
            this.txtQCGroup.TabStop = false;
            // 
            // txtGloveType
            // 
            this.txtGloveType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtGloveType.Location = new System.Drawing.Point(236, 112);
            this.txtGloveType.Margin = new System.Windows.Forms.Padding(4);
            this.txtGloveType.Name = "txtGloveType";
            this.txtGloveType.ReadOnly = true;
            this.txtGloveType.Size = new System.Drawing.Size(400, 35);
            this.txtGloveType.TabIndex = 5;
            this.txtGloveType.TabStop = false;
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSerialNo.Location = new System.Drawing.Point(236, 8);
            this.txtSerialNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtSerialNo.MaxLength = 10;
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(400, 35);
            this.txtSerialNo.TabIndex = 1;
            this.txtSerialNo.Leave += new System.EventHandler(this.txtSerialNo_Leave);
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchNo.Location = new System.Drawing.Point(236, 60);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ReadOnly = true;
            this.txtBatchNo.Size = new System.Drawing.Size(400, 35);
            this.txtBatchNo.TabIndex = 3;
            this.txtBatchNo.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(653, 420);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(276, 46);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // cmbDefectType
            // 
            this.cmbDefectType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbDefectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefectType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDefectType.FormattingEnabled = true;
            this.cmbDefectType.Location = new System.Drawing.Point(235, 210);
            this.cmbDefectType.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.cmbDefectType.Name = "cmbDefectType";
            this.cmbDefectType.Size = new System.Drawing.Size(400, 37);
            this.cmbDefectType.TabIndex = 2;
            this.cmbDefectType.SelectedIndexChanged += new System.EventHandler(this.cmbDefectType_SelectedIndexChanged);
            // 
            // DefectiveGlovePlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.grpBoxScreen);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "DefectiveGlovePlatform";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Defective Glove (Big Scale)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DefectiveGlovePlatform_Load);
            this.grpBoxScreen.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtBatchWeight;
        private System.Windows.Forms.Label lblBatch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox grpBoxScreen;
        private DateControl usrDateControl;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGloveType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtQCGroup;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbReason;
        private System.Windows.Forms.ComboBox cmbDefectType;
        private System.Windows.Forms.TextBox txtPcsCount;
        private System.Windows.Forms.Label lblPcsCount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}