namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    partial class SecondGradeStockVerification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecondGradeStockVerification));
            this.btnSave = new System.Windows.Forms.Button();
            this.txtListSerialNumber = new System.Windows.Forms.TextBox();
            this.txtInvalidBarcode = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.grpBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExtractPortable = new System.Windows.Forms.Button();
            this.dateControl1 = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(663, 459);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 39);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtListSerialNumber
            // 
            this.txtListSerialNumber.BackColor = System.Drawing.Color.White;
            this.txtListSerialNumber.Location = new System.Drawing.Point(48, 156);
            this.txtListSerialNumber.Multiline = true;
            this.txtListSerialNumber.Name = "txtListSerialNumber";
            this.txtListSerialNumber.ReadOnly = true;
            this.txtListSerialNumber.Size = new System.Drawing.Size(538, 93);
            this.txtListSerialNumber.TabIndex = 1;
            this.txtListSerialNumber.TabStop = false;
            // 
            // txtInvalidBarcode
            // 
            this.txtInvalidBarcode.BackColor = System.Drawing.Color.White;
            this.txtInvalidBarcode.Location = new System.Drawing.Point(48, 354);
            this.txtInvalidBarcode.Multiline = true;
            this.txtInvalidBarcode.Name = "txtInvalidBarcode";
            this.txtInvalidBarcode.ReadOnly = true;
            this.txtInvalidBarcode.Size = new System.Drawing.Size(538, 93);
            this.txtInvalidBarcode.TabIndex = 2;
            this.txtInvalidBarcode.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(43, 252);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(325, 29);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Total dispose stock count :";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(805, 459);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 104);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Program Messages";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.05791F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.2539F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.85523F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLocation, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dateControl1, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(37, 34);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(903, 53);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(510, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 29);
            this.label4.TabIndex = 0;
            this.label4.Text = "Date:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLocation.Location = new System.Drawing.Point(228, 9);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(218, 35);
            this.txtLocation.TabIndex = 1;
            this.txtLocation.TabStop = false;
            // 
            // grpBox
            // 
            this.grpBox.Controls.Add(this.label5);
            this.grpBox.Controls.Add(this.label3);
            this.grpBox.Controls.Add(this.btnExtractPortable);
            this.grpBox.Controls.Add(this.txtListSerialNumber);
            this.grpBox.Controls.Add(this.lblMessage);
            this.grpBox.Controls.Add(this.btnCancel);
            this.grpBox.Controls.Add(this.txtInvalidBarcode);
            this.grpBox.Controls.Add(this.btnSave);
            this.grpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBox.Location = new System.Drawing.Point(22, 136);
            this.grpBox.Name = "grpBox";
            this.grpBox.Size = new System.Drawing.Size(961, 528);
            this.grpBox.TabIndex = 6;
            this.grpBox.TabStop = false;
            this.grpBox.Text = "2nd Grade Stock Verification";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(259, 29);
            this.label5.TabIndex = 6;
            this.label5.Text = "List of Serial Number";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 310);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 29);
            this.label3.TabIndex = 6;
            this.label3.Text = "Invalid barcode";
            // 
            // btnExtractPortable
            // 
            this.btnExtractPortable.Location = new System.Drawing.Point(48, 46);
            this.btnExtractPortable.Name = "btnExtractPortable";
            this.btnExtractPortable.Size = new System.Drawing.Size(820, 42);
            this.btnExtractPortable.TabIndex = 1;
            this.btnExtractPortable.Text = "Extract from portable barcode";
            this.btnExtractPortable.UseVisualStyleBackColor = true;
            this.btnExtractPortable.Click += new System.EventHandler(this.btnExtractPortable_Click);
            // 
            // dateControl1
            // 
            this.dateControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateControl1.DateValue = new System.DateTime(((long)(0)));
            this.dateControl1.Location = new System.Drawing.Point(590, 8);
            this.dateControl1.Name = "dateControl1";
            this.dateControl1.Size = new System.Drawing.Size(308, 37);
            this.dateControl1.TabIndex = 2;
            this.dateControl1.TabStop = false;
            // 
            // SecondGradeStockVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.grpBox);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SecondGradeStockVerification";
            this.ShowInTaskbar = false;
            this.Text = "2nd Grade Stock Verification";
            this.Load += new System.EventHandler(this.SecondGradeStockVerification_Load);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.grpBox.ResumeLayout(false);
            this.grpBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtListSerialNumber;
        private System.Windows.Forms.TextBox txtInvalidBarcode;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLocation;
        private DateControl dateControl1;
        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExtractPortable;
    }
}