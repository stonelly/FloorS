namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
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
            this.productionActivitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineStartStopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productionActivitiesToolStripMenuItem,
            this.lineStartStopToolStripMenuItem,
            this.speedControlToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.TabStop = true;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // productionActivitiesToolStripMenuItem
            // 
            this.productionActivitiesToolStripMenuItem.Name = "productionActivitiesToolStripMenuItem";
            this.productionActivitiesToolStripMenuItem.Size = new System.Drawing.Size(129, 20);
            this.productionActivitiesToolStripMenuItem.Text = "Production Activities";
            this.productionActivitiesToolStripMenuItem.Click += new System.EventHandler(this.productionActivitiesToolStripMenuItem_Click);
            // 
            // lineStartStopToolStripMenuItem
            // 
            this.lineStartStopToolStripMenuItem.Name = "lineStartStopToolStripMenuItem";
            this.lineStartStopToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
            this.lineStartStopToolStripMenuItem.Text = "Production Line Start / Stop";
            this.lineStartStopToolStripMenuItem.Click += new System.EventHandler(this.lineStartStopToolStripMenuItem_Click);
            // 
            // speedControlToolStripMenuItem
            // 
            this.speedControlToolStripMenuItem.Name = "speedControlToolStripMenuItem";
            this.speedControlToolStripMenuItem.ShowShortcutKeys = false;
            this.speedControlToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.speedControlToolStripMenuItem.Text = "Speed Control";
            this.speedControlToolStripMenuItem.Click += new System.EventHandler(this.speedControlToolStripMenuItem_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 726);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Production Logging Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem productionActivitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineStartStopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speedControlToolStripMenuItem;

    }
}