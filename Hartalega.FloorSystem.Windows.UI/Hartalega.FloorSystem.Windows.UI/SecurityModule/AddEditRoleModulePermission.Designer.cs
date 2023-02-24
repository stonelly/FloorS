namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    partial class AddEditRoleModulePermission
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRoleName = new System.Windows.Forms.Label();
            this.lblRoleDescription = new System.Windows.Forms.Label();
            this.lblModule = new System.Windows.Forms.Label();
            this.lblPermission = new System.Windows.Forms.Label();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.txtRoleDescription = new System.Windows.Forms.TextBox();
            this.cmbPermission = new System.Windows.Forms.ComboBox();
            this.cblModule = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(32, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(7);
            this.groupBox1.Size = new System.Drawing.Size(961, 575);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Edit Role";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(374, 433);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(273, 49);
            this.tableLayoutPanel2.TabIndex = 1;
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
            this.tableLayoutPanel1.Controls.Add(this.lblRoleName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblRoleDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblModule, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblPermission, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtRoleName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtRoleDescription, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbPermission, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cblModule, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.92157F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.07843F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(946, 382);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblRoleName
            // 
            this.lblRoleName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRoleName.AutoSize = true;
            this.lblRoleName.Location = new System.Drawing.Point(82, 16);
            this.lblRoleName.Name = "lblRoleName";
            this.lblRoleName.Size = new System.Drawing.Size(151, 29);
            this.lblRoleName.TabIndex = 0;
            this.lblRoleName.Text = "Role Name:";
            // 
            // lblRoleDescription
            // 
            this.lblRoleDescription.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRoleDescription.AutoSize = true;
            this.lblRoleDescription.Location = new System.Drawing.Point(18, 74);
            this.lblRoleDescription.Name = "lblRoleDescription";
            this.lblRoleDescription.Size = new System.Drawing.Size(215, 29);
            this.lblRoleDescription.TabIndex = 1;
            this.lblRoleDescription.Text = "Role Description:";
            // 
            // lblModule
            // 
            this.lblModule.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(126, 258);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(107, 29);
            this.lblModule.TabIndex = 3;
            this.lblModule.Text = "Module:";
            // 
            // lblPermission
            // 
            this.lblPermission.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPermission.AutoSize = true;
            this.lblPermission.Location = new System.Drawing.Point(82, 125);
            this.lblPermission.Name = "lblPermission";
            this.lblPermission.Size = new System.Drawing.Size(151, 29);
            this.lblPermission.TabIndex = 2;
            this.lblPermission.Text = "Permission:";
            // 
            // txtRoleName
            // 
            this.txtRoleName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRoleName.Location = new System.Drawing.Point(239, 13);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(403, 35);
            this.txtRoleName.TabIndex = 0;
            // 
            // txtRoleDescription
            // 
            this.txtRoleDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRoleDescription.Location = new System.Drawing.Point(239, 71);
            this.txtRoleDescription.MaxLength = 50;
            this.txtRoleDescription.Name = "txtRoleDescription";
            this.txtRoleDescription.Size = new System.Drawing.Size(403, 35);
            this.txtRoleDescription.TabIndex = 2;
            // 
            // cmbPermission
            // 
            this.cmbPermission.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPermission.DropDownHeight = 200;
            this.cmbPermission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPermission.FormattingEnabled = true;
            this.cmbPermission.IntegralHeight = false;
            this.cmbPermission.Location = new System.Drawing.Point(239, 129);
            this.cmbPermission.Name = "cmbPermission";
            this.cmbPermission.Size = new System.Drawing.Size(403, 37);
            this.cmbPermission.TabIndex = 3;
            // 
            // cblModule
            // 
            this.cblModule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cblModule.CheckOnClick = true;
            this.cblModule.FormattingEnabled = true;
            this.cblModule.Location = new System.Drawing.Point(239, 181);
            this.cblModule.Name = "cblModule";
            this.cblModule.Size = new System.Drawing.Size(403, 184);
            this.cblModule.TabIndex = 4;
            // 
            // AddEditRoleModulePermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(700, 500);
            this.ClientSize = new System.Drawing.Size(1024, 616);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddEditRoleModulePermission";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/Edit Role";
            this.Load += new System.EventHandler(this.AddEditRoleModulePermission_Load);
            this.Leave += new System.EventHandler(this.AddEditRoleModulePermission_Leave);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblRoleName;
        private System.Windows.Forms.Label lblRoleDescription;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.Label lblPermission;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.TextBox txtRoleDescription;
        private System.Windows.Forms.ComboBox cmbPermission;
        private System.Windows.Forms.CheckedListBox cblModule;

    }
}