namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    partial class OperatorScreenAccessMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperatorScreenAccessMaster));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddScreenAccessMaster = new System.Windows.Forms.ToolStripButton();
            this.btnEditScreenAccessMaster = new System.Windows.Forms.ToolStripButton();
            this.gvScreenAccessMaster = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OperatorModulePermissionMappingId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScreenId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScreenName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PermissionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PermissionOperator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Permission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PermissionSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvScreenAccessMaster)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddScreenAccessMaster,
            this.btnEditScreenAccessMaster});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1024, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddScreenAccessMaster
            // 
            this.btnAddScreenAccessMaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddScreenAccessMaster.Image = ((System.Drawing.Image)(resources.GetObject("btnAddScreenAccessMaster.Image")));
            this.btnAddScreenAccessMaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddScreenAccessMaster.Name = "btnAddScreenAccessMaster";
            this.btnAddScreenAccessMaster.Size = new System.Drawing.Size(23, 22);
            this.btnAddScreenAccessMaster.Text = "Add Screen";
            this.btnAddScreenAccessMaster.Click += new System.EventHandler(this.btnAddScreenAccessMaster_Click);
            // 
            // btnEditScreenAccessMaster
            // 
            this.btnEditScreenAccessMaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditScreenAccessMaster.Image = ((System.Drawing.Image)(resources.GetObject("btnEditScreenAccessMaster.Image")));
            this.btnEditScreenAccessMaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditScreenAccessMaster.Name = "btnEditScreenAccessMaster";
            this.btnEditScreenAccessMaster.Size = new System.Drawing.Size(23, 22);
            this.btnEditScreenAccessMaster.Text = "Edit Screen";
            this.btnEditScreenAccessMaster.Click += new System.EventHandler(this.btnEditScreenAccessMaster_Click);
            // 
            // gvScreenAccessMaster
            // 
            this.gvScreenAccessMaster.AllowUserToAddRows = false;
            this.gvScreenAccessMaster.AllowUserToDeleteRows = false;
            this.gvScreenAccessMaster.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvScreenAccessMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvScreenAccessMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OperatorModulePermissionMappingId,
            this.ModuleId,
            this.ModuleName,
            this.ScreenId,
            this.ScreenName,
            this.PermissionId,
            this.PermissionOperator,
            this.Permission,
            this.PermissionSeq});
            this.gvScreenAccessMaster.Location = new System.Drawing.Point(3, 3);
            this.gvScreenAccessMaster.Name = "gvScreenAccessMaster";
            this.gvScreenAccessMaster.ReadOnly = true;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvScreenAccessMaster.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.gvScreenAccessMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvScreenAccessMaster.Size = new System.Drawing.Size(900, 515);
            this.gvScreenAccessMaster.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(25, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(950, 585);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operator Screen Access Master";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.gvScreenAccessMaster, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 40);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 540);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // OperatorModulePermissionMappingId
            // 
            this.OperatorModulePermissionMappingId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OperatorModulePermissionMappingId.DataPropertyName = "OperatorModulePermissionMappingId";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperatorModulePermissionMappingId.DefaultCellStyle = dataGridViewCellStyle1;
            this.OperatorModulePermissionMappingId.HeaderText = "OperatorModulePermissionMappingId";
            this.OperatorModulePermissionMappingId.Name = "OperatorModulePermissionMappingId";
            this.OperatorModulePermissionMappingId.ReadOnly = true;
            this.OperatorModulePermissionMappingId.Visible = false;
            // 
            // ModuleId
            // 
            this.ModuleId.DataPropertyName = "ModuleId";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModuleId.DefaultCellStyle = dataGridViewCellStyle2;
            this.ModuleId.HeaderText = "ModuleId";
            this.ModuleId.Name = "ModuleId";
            this.ModuleId.ReadOnly = true;
            this.ModuleId.Visible = false;
            // 
            // ModuleName
            // 
            this.ModuleName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModuleName.DataPropertyName = "ModuleName";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModuleName.DefaultCellStyle = dataGridViewCellStyle3;
            this.ModuleName.HeaderText = "Module Name";
            this.ModuleName.Name = "ModuleName";
            this.ModuleName.ReadOnly = true;
            // 
            // ScreenId
            // 
            this.ScreenId.DataPropertyName = "ScreenId";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScreenId.DefaultCellStyle = dataGridViewCellStyle4;
            this.ScreenId.HeaderText = "ScreenId";
            this.ScreenId.Name = "ScreenId";
            this.ScreenId.ReadOnly = true;
            this.ScreenId.Visible = false;
            // 
            // ScreenName
            // 
            this.ScreenName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ScreenName.DataPropertyName = "ScreenName";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScreenName.DefaultCellStyle = dataGridViewCellStyle5;
            this.ScreenName.HeaderText = "Screen Name";
            this.ScreenName.Name = "ScreenName";
            this.ScreenName.ReadOnly = true;
            // 
            // PermissionId
            // 
            this.PermissionId.DataPropertyName = "PermissionId";
            this.PermissionId.HeaderText = "PermissionId";
            this.PermissionId.Name = "PermissionId";
            this.PermissionId.ReadOnly = true;
            this.PermissionId.Visible = false;
            // 
            // PermissionOperator
            // 
            this.PermissionOperator.DataPropertyName = "PermissionOperator";
            this.PermissionOperator.HeaderText = "PermissionOperator";
            this.PermissionOperator.Name = "PermissionOperator";
            this.PermissionOperator.ReadOnly = true;
            this.PermissionOperator.Visible = false;
            // 
            // Permission
            // 
            this.Permission.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Permission.DataPropertyName = "Permission";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Permission.DefaultCellStyle = dataGridViewCellStyle6;
            this.Permission.HeaderText = "Permission";
            this.Permission.Name = "Permission";
            this.Permission.ReadOnly = true;
            // 
            // PermissionSeq
            // 
            this.PermissionSeq.DataPropertyName = "PermissionSeq";
            this.PermissionSeq.HeaderText = "PermissionSeq";
            this.PermissionSeq.Name = "PermissionSeq";
            this.PermissionSeq.ReadOnly = true;
            this.PermissionSeq.Visible = false;
            // 
            // OperatorScreenAccessMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 730);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "OperatorScreenAccessMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Operator Screen Access Master";
            this.Load += new System.EventHandler(this.ScreenAccessMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageMaster_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvScreenAccessMaster)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddScreenAccessMaster;
        private System.Windows.Forms.ToolStripButton btnEditScreenAccessMaster;
        private System.Windows.Forms.DataGridView gvScreenAccessMaster;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperatorModulePermissionMappingId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreenId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreenName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PermissionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PermissionOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Permission;
        private System.Windows.Forms.DataGridViewTextBoxColumn PermissionSeq;
    }
}