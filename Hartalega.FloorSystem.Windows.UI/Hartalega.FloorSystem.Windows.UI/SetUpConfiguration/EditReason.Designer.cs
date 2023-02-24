namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class EditReason
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditReason));
            this.gbEditReason = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblReasonType = new System.Windows.Forms.Label();
            this.lblReasonText = new System.Windows.Forms.Label();
            this.lblSchedule = new System.Windows.Forms.Label();
            this.txtReasonType = new System.Windows.Forms.TextBox();
            this.txtReasonText = new System.Windows.Forms.TextBox();
            this.chkSchedule = new System.Windows.Forms.CheckBox();
            this.gbEditReason.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEditReason
            // 
            this.gbEditReason.Controls.Add(this.tableLayoutPanel2);
            this.gbEditReason.Controls.Add(this.tableLayoutPanel1);
            this.gbEditReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbEditReason.Location = new System.Drawing.Point(25, 24);
            this.gbEditReason.Name = "gbEditReason";
            this.gbEditReason.Size = new System.Drawing.Size(778, 226);
            this.gbEditReason.TabIndex = 0;
            this.gbEditReason.TabStop = false;
            this.gbEditReason.Text = "Edit Reason";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(552, 175);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 45);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 28);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(103, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 28);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblReasonType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblReasonText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSchedule, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtReasonType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtReasonText, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkSchedule, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(119, 42);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.08696F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.91304F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(427, 141);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblReasonType
            // 
            this.lblReasonType.AutoSize = true;
            this.lblReasonType.Location = new System.Drawing.Point(3, 0);
            this.lblReasonType.Name = "lblReasonType";
            this.lblReasonType.Size = new System.Drawing.Size(119, 20);
            this.lblReasonType.TabIndex = 0;
            this.lblReasonType.Text = "Reason Type:";
            // 
            // lblReasonText
            // 
            this.lblReasonText.AutoSize = true;
            this.lblReasonText.Location = new System.Drawing.Point(3, 51);
            this.lblReasonText.Name = "lblReasonText";
            this.lblReasonText.Size = new System.Drawing.Size(115, 20);
            this.lblReasonText.TabIndex = 1;
            this.lblReasonText.Text = "Reason Text:";
            // 
            // lblSchedule
            // 
            this.lblSchedule.AutoSize = true;
            this.lblSchedule.Location = new System.Drawing.Point(3, 100);
            this.lblSchedule.Name = "lblSchedule";
            this.lblSchedule.Size = new System.Drawing.Size(84, 20);
            this.lblSchedule.TabIndex = 2;
            this.lblSchedule.Text = "Schedule";
            this.lblSchedule.Visible = false;
            // 
            // txtReasonType
            // 
            this.txtReasonType.Location = new System.Drawing.Point(216, 3);
            this.txtReasonType.Name = "txtReasonType";
            this.txtReasonType.ReadOnly = true;
            this.txtReasonType.Size = new System.Drawing.Size(208, 26);
            this.txtReasonType.TabIndex = 0;
            // 
            // txtReasonText
            // 
            this.txtReasonText.Location = new System.Drawing.Point(216, 54);
            this.txtReasonText.Name = "txtReasonText";
            this.txtReasonText.Size = new System.Drawing.Size(208, 26);
            this.txtReasonText.TabIndex = 1;
            // 
            // chkSchedule
            // 
            this.chkSchedule.AutoSize = true;
            this.chkSchedule.Location = new System.Drawing.Point(216, 103);
            this.chkSchedule.Name = "chkSchedule";
            this.chkSchedule.Size = new System.Drawing.Size(15, 14);
            this.chkSchedule.TabIndex = 2;
            this.chkSchedule.UseVisualStyleBackColor = true;
            this.chkSchedule.Visible = false;
            this.chkSchedule.Paint += new System.Windows.Forms.PaintEventHandler(this.chkSchedule_Paint);
            // 
            // EditReason
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 262);
            this.Controls.Add(this.gbEditReason);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditReason";
            this.Text = "EditReason";
            this.Load += new System.EventHandler(this.EditReason_Load);
            this.gbEditReason.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEditReason;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblReasonType;
        private System.Windows.Forms.Label lblReasonText;
        private System.Windows.Forms.Label lblSchedule;
        private System.Windows.Forms.TextBox txtReasonType;
        private System.Windows.Forms.TextBox txtReasonText;
        private System.Windows.Forms.CheckBox chkSchedule;
    }
}