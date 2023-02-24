namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    partial class PHLotVerification
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btResetTl = new System.Windows.Forms.Button();
            this.lblSc3Status = new System.Windows.Forms.Label();
            this.lblSc2Status = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCartonNo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbScanData = new System.Windows.Forms.TextBox();
            this.lblSc1Status = new System.Windows.Forms.Label();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(83, 255);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(15, 20);
            this.lblStatus.TabIndex = 31;
            this.lblStatus.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(10, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 20);
            this.label9.TabIndex = 30;
            this.label9.Text = "Status:";
            // 
            // btResetTl
            // 
            this.btResetTl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btResetTl.Location = new System.Drawing.Point(582, 26);
            this.btResetTl.Name = "btResetTl";
            this.btResetTl.Size = new System.Drawing.Size(175, 37);
            this.btResetTl.TabIndex = 29;
            this.btResetTl.Text = "&Reset Tower Light";
            this.btResetTl.UseVisualStyleBackColor = true;
            this.btResetTl.Click += new System.EventHandler(this.btResetTl_Click);
            // 
            // lblSc3Status
            // 
            this.lblSc3Status.AutoSize = true;
            this.lblSc3Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSc3Status.ForeColor = System.Drawing.Color.Red;
            this.lblSc3Status.Location = new System.Drawing.Point(428, 217);
            this.lblSc3Status.Name = "lblSc3Status";
            this.lblSc3Status.Size = new System.Drawing.Size(134, 31);
            this.lblSc3Status.TabIndex = 28;
            this.lblSc3Status.Text = "Not Scan";
            // 
            // lblSc2Status
            // 
            this.lblSc2Status.AutoSize = true;
            this.lblSc2Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSc2Status.ForeColor = System.Drawing.Color.Red;
            this.lblSc2Status.Location = new System.Drawing.Point(428, 159);
            this.lblSc2Status.Name = "lblSc2Status";
            this.lblSc2Status.Size = new System.Drawing.Size(134, 31);
            this.lblSc2Status.TabIndex = 27;
            this.lblSc2Status.Text = "Not Scan";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(431, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 20);
            this.label6.TabIndex = 26;
            this.label6.Text = "Outer Sc3:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(431, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 25;
            this.label5.Text = "Outer Sc2:";
            // 
            // lblCartonNo
            // 
            this.lblCartonNo.AutoSize = true;
            this.lblCartonNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartonNo.Location = new System.Drawing.Point(428, 49);
            this.lblCartonNo.Name = "lblCartonNo";
            this.lblCartonNo.Size = new System.Drawing.Size(89, 31);
            this.lblCartonNo.TabIndex = 24;
            this.lblCartonNo.Text = "00000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(431, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 23;
            this.label3.Text = "Carton No.:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(431, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Inner Sc1:";
            // 
            // tbScanData
            // 
            this.tbScanData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbScanData.Location = new System.Drawing.Point(14, 26);
            this.tbScanData.Multiline = true;
            this.tbScanData.Name = "tbScanData";
            this.tbScanData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbScanData.Size = new System.Drawing.Size(394, 222);
            this.tbScanData.TabIndex = 19;
            // 
            // lblSc1Status
            // 
            this.lblSc1Status.AutoSize = true;
            this.lblSc1Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSc1Status.ForeColor = System.Drawing.Color.Red;
            this.lblSc1Status.Location = new System.Drawing.Point(428, 104);
            this.lblSc1Status.Name = "lblSc1Status";
            this.lblSc1Status.Size = new System.Drawing.Size(134, 31);
            this.lblSc1Status.TabIndex = 32;
            this.lblSc1Status.Text = "Not Scan";
            // 
            // CloseBtn
            // 
            this.CloseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseBtn.Location = new System.Drawing.Point(582, 72);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(175, 37);
            this.CloseBtn.TabIndex = 33;
            this.CloseBtn.Text = "&Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // PHLotVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 286);
            this.ControlBox = false;
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.lblSc1Status);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btResetTl);
            this.Controls.Add(this.lblSc3Status);
            this.Controls.Add(this.lblSc2Status);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblCartonNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbScanData);
            this.Name = "PHLotVerification";
            this.Text = "PHLotVerification";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PHLotVerification_FormClosing);
            this.Load += new System.EventHandler(this.PHLotVerification_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btResetTl;
        private System.Windows.Forms.Label lblSc3Status;
        private System.Windows.Forms.Label lblSc2Status;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCartonNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbScanData;
        private System.Windows.Forms.Label lblSc1Status;
        private System.Windows.Forms.Button CloseBtn;
    }
}