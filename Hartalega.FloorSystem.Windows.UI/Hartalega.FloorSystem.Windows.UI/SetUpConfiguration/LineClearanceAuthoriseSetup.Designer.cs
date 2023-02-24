namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class LineClearanceAuthoriseSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineClearanceAuthoriseSetup));
            this.dgvLineClearanceAuthorise = new System.Windows.Forms.DataGridView();
            this.EmployeeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmployeeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsAllowAuthoriseLineClearance = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gbLineClearanceAuthoriseSetup = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpSearchBy = new System.Windows.Forms.GroupBox();
            this.rbEmployeeName = new System.Windows.Forms.RadioButton();
            this.rbEmployeeID = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineClearanceAuthorise)).BeginInit();
            this.gbLineClearanceAuthoriseSetup.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpSearchBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLineClearanceAuthorise
            // 
            this.dgvLineClearanceAuthorise.AllowUserToAddRows = false;
            this.dgvLineClearanceAuthorise.AllowUserToDeleteRows = false;
            this.dgvLineClearanceAuthorise.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLineClearanceAuthorise.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLineClearanceAuthorise.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLineClearanceAuthorise.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EmployeeID,
            this.EmployeeName,
            this.Role,
            this.IsAllowAuthoriseLineClearance});
            this.dgvLineClearanceAuthorise.Location = new System.Drawing.Point(5, 112);
            this.dgvLineClearanceAuthorise.Name = "dgvLineClearanceAuthorise";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLineClearanceAuthorise.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLineClearanceAuthorise.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLineClearanceAuthorise.Size = new System.Drawing.Size(934, 409);
            this.dgvLineClearanceAuthorise.TabIndex = 4;
            // 
            // EmployeeID
            // 
            this.EmployeeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EmployeeID.DataPropertyName = "EmployeeID";
            this.EmployeeID.HeaderText = "EmployeeID";
            this.EmployeeID.Name = "EmployeeID";
            this.EmployeeID.ReadOnly = true;
            this.EmployeeID.Width = 160;
            // 
            // EmployeeName
            // 
            this.EmployeeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EmployeeName.DataPropertyName = "EmployeeName";
            this.EmployeeName.HeaderText = "Name";
            this.EmployeeName.Name = "EmployeeName";
            this.EmployeeName.ReadOnly = true;
            this.EmployeeName.Width = 400;
            // 
            // Role
            // 
            this.Role.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Role.DataPropertyName = "Role";
            this.Role.HeaderText = "Role";
            this.Role.Name = "Role";
            this.Role.ReadOnly = true;
            this.Role.Width = 240;
            // 
            // IsAllowAuthoriseLineClearance
            // 
            this.IsAllowAuthoriseLineClearance.DataPropertyName = "IsAllowAuthoriseLineClearance";
            this.IsAllowAuthoriseLineClearance.HeaderText = "Allow";
            this.IsAllowAuthoriseLineClearance.Name = "IsAllowAuthoriseLineClearance";
            this.IsAllowAuthoriseLineClearance.Width = 57;
            // 
            // gbLineClearanceAuthoriseSetup
            // 
            this.gbLineClearanceAuthoriseSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLineClearanceAuthoriseSetup.Controls.Add(this.btnCancel);
            this.gbLineClearanceAuthoriseSetup.Controls.Add(this.btnSave);
            this.gbLineClearanceAuthoriseSetup.Controls.Add(this.dgvLineClearanceAuthorise);
            this.gbLineClearanceAuthoriseSetup.Controls.Add(this.tableLayoutPanel1);
            this.gbLineClearanceAuthoriseSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLineClearanceAuthoriseSetup.Location = new System.Drawing.Point(25, 10);
            this.gbLineClearanceAuthoriseSetup.Name = "gbLineClearanceAuthoriseSetup";
            this.gbLineClearanceAuthoriseSetup.Size = new System.Drawing.Size(952, 660);
            this.gbLineClearanceAuthoriseSetup.TabIndex = 0;
            this.gbLineClearanceAuthoriseSetup.TabStop = false;
            this.gbLineClearanceAuthoriseSetup.Text = "Line Clearance Authorise Setup";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(761, 548);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(621, 548);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 40);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "S&ave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.54546F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tableLayoutPanel1.Controls.Add(this.btnSearch, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSearchText, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.grpSearchBy, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 61);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grpSearchBy
            // 
            this.grpSearchBy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grpSearchBy.Controls.Add(this.rbEmployeeName);
            this.grpSearchBy.Controls.Add(this.rbEmployeeID);
            this.grpSearchBy.Location = new System.Drawing.Point(3, 3);
            this.grpSearchBy.Margin = new System.Windows.Forms.Padding(0);
            this.grpSearchBy.Name = "grpSearchBy";
            this.grpSearchBy.Padding = new System.Windows.Forms.Padding(0);
            this.grpSearchBy.Size = new System.Drawing.Size(338, 54);
            this.grpSearchBy.TabIndex = 7;
            this.grpSearchBy.TabStop = false;
            // 
            // rbEmployeeName
            // 
            this.rbEmployeeName.AutoSize = true;
            this.rbEmployeeName.Location = new System.Drawing.Point(189, 16);
            this.rbEmployeeName.Name = "rbEmployeeName";
            this.rbEmployeeName.Size = new System.Drawing.Size(73, 24);
            this.rbEmployeeName.TabIndex = 1;
            this.rbEmployeeName.Text = "Name";
            this.rbEmployeeName.UseVisualStyleBackColor = true;
            // 
            // rbEmployeeID
            // 
            this.rbEmployeeID.AutoSize = true;
            this.rbEmployeeID.Checked = true;
            this.rbEmployeeID.Location = new System.Drawing.Point(3, 16);
            this.rbEmployeeID.Name = "rbEmployeeID";
            this.rbEmployeeID.Size = new System.Drawing.Size(129, 24);
            this.rbEmployeeID.TabIndex = 0;
            this.rbEmployeeID.TabStop = true;
            this.rbEmployeeID.Text = "Employee ID";
            this.rbEmployeeID.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(759, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(137, 39);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchText
            // 
            this.txtSearchText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSearchText.Location = new System.Drawing.Point(347, 17);
            this.txtSearchText.Name = "txtSearchText";
            this.txtSearchText.Size = new System.Drawing.Size(406, 26);
            this.txtSearchText.TabIndex = 2;
            // 
            // LineClearanceAuthoriseSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 637);
            this.Controls.Add(this.gbLineClearanceAuthoriseSetup);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 622);
            this.Name = "LineClearanceAuthoriseSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Line Clearance Authorise Setup";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LineClearanceAuthoriseSetup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineClearanceAuthorise)).EndInit();
            this.gbLineClearanceAuthoriseSetup.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.grpSearchBy.ResumeLayout(false);
            this.grpSearchBy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLineClearanceAuthoriseSetup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton rbEmployeeID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RadioButton rbEmployeeName;
        private System.Windows.Forms.GroupBox grpSearchBy;
        private System.Windows.Forms.TextBox txtSearchText;
        private System.Windows.Forms.DataGridView dgvLineClearanceAuthorise;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmployeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsAllowAuthoriseLineClearance;
    }
}