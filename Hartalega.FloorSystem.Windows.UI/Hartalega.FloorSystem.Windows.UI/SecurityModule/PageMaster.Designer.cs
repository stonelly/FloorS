namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    partial class PageMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageMaster));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPageMaster = new System.Windows.Forms.ToolStripButton();
            this.btnEditPageMaster = new System.Windows.Forms.ToolStripButton();
            this.gvPageMaster = new System.Windows.Forms.DataGridView();
            this.ModuleScreenMappingId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScreenId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScreenName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PermissionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PermissionOperator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Permission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PermissionSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPageMaster)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPageMaster,
            this.btnEditPageMaster});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1024, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPageMaster
            // 
            this.btnPageMaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPageMaster.Image = ((System.Drawing.Image)(resources.GetObject("btnPageMaster.Image")));
            this.btnPageMaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPageMaster.Name = "btnPageMaster";
            this.btnPageMaster.Size = new System.Drawing.Size(23, 22);
            this.btnPageMaster.Text = "Add Screen";
            this.btnPageMaster.Click += new System.EventHandler(this.btnPageMaster_Click);
            // 
            // btnEditPageMaster
            // 
            this.btnEditPageMaster.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditPageMaster.Image = ((System.Drawing.Image)(resources.GetObject("btnEditPageMaster.Image")));
            this.btnEditPageMaster.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditPageMaster.Name = "btnEditPageMaster";
            this.btnEditPageMaster.Size = new System.Drawing.Size(23, 22);
            this.btnEditPageMaster.Text = "Edit Screen";
            this.btnEditPageMaster.Click += new System.EventHandler(this.btnEditPageMaster_Click);
            // 
            // gvPageMaster
            // 
            this.gvPageMaster.AllowUserToAddRows = false;
            this.gvPageMaster.AllowUserToDeleteRows = false;
            this.gvPageMaster.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gvPageMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPageMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ModuleScreenMappingId,
            this.ModuleId,
            this.ModuleName,
            this.ScreenId,
            this.ScreenName,
            this.PermissionId,
            this.PermissionOperator,
            this.Permission,
            this.PermissionSeq});
            this.gvPageMaster.Location = new System.Drawing.Point(3, 3);
            this.gvPageMaster.Name = "gvPageMaster";
            this.gvPageMaster.ReadOnly = true;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPageMaster.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.gvPageMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvPageMaster.Size = new System.Drawing.Size(900, 515);
            this.gvPageMaster.TabIndex = 1;
            // 
            // ModuleScreenMappingId
            // 
            this.ModuleScreenMappingId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ModuleScreenMappingId.DataPropertyName = "ModuleScreenMappingId";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModuleScreenMappingId.DefaultCellStyle = dataGridViewCellStyle1;
            this.ModuleScreenMappingId.HeaderText = "ModuleScreenMappingId";
            this.ModuleScreenMappingId.Name = "ModuleScreenMappingId";
            this.ModuleScreenMappingId.ReadOnly = true;
            this.ModuleScreenMappingId.Visible = false;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(25, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(950, 585);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screen Master";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.gvPageMaster, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 40);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 540);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // PageMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(900, 600);
            this.ClientSize = new System.Drawing.Size(1024, 730);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "PageMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Screen Master";
            this.Load += new System.EventHandler(this.PageMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageMaster_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPageMaster)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPageMaster;
        private System.Windows.Forms.ToolStripButton btnEditPageMaster;
        private System.Windows.Forms.DataGridView gvPageMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleScreenMappingId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreenId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScreenName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PermissionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PermissionOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Permission;
        private System.Windows.Forms.DataGridViewTextBoxColumn PermissionSeq;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}