namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    partial class AddEditPageMaster
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPermission = new System.Windows.Forms.Label();
            this.lblScreenName = new System.Windows.Forms.Label();
            this.lblModule = new System.Windows.Forms.Label();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.cmbScreenName = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbPermission = new System.Windows.Forms.ComboBox();
            this.cmbPermissionOperator = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 568);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Edit Screen Master";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(354, 243);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(273, 45);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 39);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(139, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.04F));
            this.tableLayoutPanel1.Controls.Add(this.lblPermission, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblScreenName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblModule, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbModule, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbScreenName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.23809F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.76191F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(777, 192);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblPermission
            // 
            this.lblPermission.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPermission.AutoSize = true;
            this.lblPermission.Location = new System.Drawing.Point(39, 147);
            this.lblPermission.Name = "lblPermission";
            this.lblPermission.Size = new System.Drawing.Size(151, 29);
            this.lblPermission.TabIndex = 7;
            this.lblPermission.Text = "Permission:";
            // 
            // lblScreenName
            // 
            this.lblScreenName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblScreenName.AutoSize = true;
            this.lblScreenName.Location = new System.Drawing.Point(11, 80);
            this.lblScreenName.Name = "lblScreenName";
            this.lblScreenName.Size = new System.Drawing.Size(179, 29);
            this.lblScreenName.TabIndex = 6;
            this.lblScreenName.Text = "Screen Name:";
            // 
            // lblModule
            // 
            this.lblModule.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(83, 15);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(107, 29);
            this.lblModule.TabIndex = 1;
            this.lblModule.Text = "Module:";
            // 
            // cmbModule
            // 
            this.cmbModule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(196, 11);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(426, 37);
            this.cmbModule.TabIndex = 4;
            // 
            // cmbScreenName
            // 
            this.cmbScreenName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbScreenName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScreenName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbScreenName.FormattingEnabled = true;
            this.cmbScreenName.Location = new System.Drawing.Point(196, 76);
            this.cmbScreenName.Name = "cmbScreenName";
            this.cmbScreenName.Size = new System.Drawing.Size(426, 37);
            this.cmbScreenName.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.51365F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.48635F));
            this.tableLayoutPanel2.Controls.Add(this.cmbPermission, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbPermissionOperator, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(196, 134);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(426, 55);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // cmbPermission
            // 
            this.cmbPermission.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPermission.DropDownHeight = 200;
            this.cmbPermission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPermission.FormattingEnabled = true;
            this.cmbPermission.IntegralHeight = false;
            this.cmbPermission.Location = new System.Drawing.Point(137, 9);
            this.cmbPermission.Name = "cmbPermission";
            this.cmbPermission.Size = new System.Drawing.Size(286, 37);
            this.cmbPermission.TabIndex = 7;
            // 
            // cmbPermissionOperator
            // 
            this.cmbPermissionOperator.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPermissionOperator.DropDownHeight = 200;
            this.cmbPermissionOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPermissionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPermissionOperator.FormattingEnabled = true;
            this.cmbPermissionOperator.IntegralHeight = false;
            this.cmbPermissionOperator.Items.AddRange(new object[] {
            "=",
            "<",
            ">",
            "<=",
            ">="});
            this.cmbPermissionOperator.Location = new System.Drawing.Point(3, 9);
            this.cmbPermissionOperator.Name = "cmbPermissionOperator";
            this.cmbPermissionOperator.Size = new System.Drawing.Size(128, 37);
            this.cmbPermissionOperator.TabIndex = 6;
            // 
            // AddEditPageMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(700, 500);
            this.ClientSize = new System.Drawing.Size(1014, 617);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddEditPageMaster";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/Edit Screen Master";
            this.Load += new System.EventHandler(this.AddEditPageMaster_Load);
            this.Leave += new System.EventHandler(this.AddEditPageMaster_Leave);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.Label lblPermission;
        private System.Windows.Forms.Label lblScreenName;
        private System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.ComboBox cmbScreenName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cmbPermission;
        private System.Windows.Forms.ComboBox cmbPermissionOperator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}