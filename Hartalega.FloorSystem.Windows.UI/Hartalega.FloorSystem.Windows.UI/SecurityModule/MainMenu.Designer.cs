namespace Hartalega.FloorSystem.Windows.UI.SecurityModule
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.employeeMaintenanceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roleMaintenanceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.permissionMasterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageMasterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenAccessMasterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.employeeMaintenanceMenuItem,
            this.roleMaintenanceMenuItem,
            this.permissionMasterMenuItem,
            this.pageMasterMenuItem,
            this.screenAccessMasterMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // employeeMaintenanceMenuItem
            // 
            this.employeeMaintenanceMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.employeeMaintenanceMenuItem.Name = "employeeMaintenanceMenuItem";
            this.employeeMaintenanceMenuItem.Size = new System.Drawing.Size(151, 22);
            this.employeeMaintenanceMenuItem.Text = "Employee Master";
            this.employeeMaintenanceMenuItem.Click += new System.EventHandler(this.employeeMaintenanceMenuItem_Click);
            // 
            // roleMaintenanceMenuItem
            // 
            this.roleMaintenanceMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roleMaintenanceMenuItem.Name = "roleMaintenanceMenuItem";
            this.roleMaintenanceMenuItem.Size = new System.Drawing.Size(112, 22);
            this.roleMaintenanceMenuItem.Text = "Role Master";
            this.roleMaintenanceMenuItem.Click += new System.EventHandler(this.roleMaintenanceMenuItem_Click);
            // 
            // permissionMasterMenuItem
            // 
            this.permissionMasterMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.permissionMasterMenuItem.Name = "permissionMasterMenuItem";
            this.permissionMasterMenuItem.Size = new System.Drawing.Size(162, 22);
            this.permissionMasterMenuItem.Text = "Permission Master";
            this.permissionMasterMenuItem.Click += new System.EventHandler(this.permissionMasterMenuItem_Click);
            // 
            // pageMasterMenuItem
            // 
            this.pageMasterMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageMasterMenuItem.Name = "pageMasterMenuItem";
            this.pageMasterMenuItem.Size = new System.Drawing.Size(130, 22);
            this.pageMasterMenuItem.Text = "Screen Master";
            this.pageMasterMenuItem.Click += new System.EventHandler(this.pageMasterMenuItem_Click);
            // 
            // screenAccessMasterMenuItem
            // 
            this.screenAccessMasterMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.screenAccessMasterMenuItem.Name = "screenAccessMasterMenuItem";
            this.screenAccessMasterMenuItem.Size = new System.Drawing.Size(262, 22);
            this.screenAccessMasterMenuItem.Text = "Operator Screen Access Master";
            this.screenAccessMasterMenuItem.Click += new System.EventHandler(this.screenAccessMasterMenuItem_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1022, 726);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Employee Maintenance Main Menu";
            this.TransparencyKey = System.Drawing.SystemColors.Window;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainMenu_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem employeeMaintenanceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem roleMaintenanceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem permissionMasterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageMasterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenAccessMasterMenuItem;
    }
}