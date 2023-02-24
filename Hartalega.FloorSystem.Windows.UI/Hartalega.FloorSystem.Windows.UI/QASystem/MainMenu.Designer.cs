namespace Hartalega.FloorSystem.Windows.UI.QASystem
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
            this.gbAddTestResultMenu = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddHotBox = new System.Windows.Forms.Button();
            this.btnAddProtein = new System.Windows.Forms.Button();
            this.btnAddPowder = new System.Windows.Forms.Button();
            this.gbAddTestResultMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAddTestResultMenu
            // 
            this.gbAddTestResultMenu.AutoSize = true;
            this.gbAddTestResultMenu.Controls.Add(this.tableLayoutPanel1);
            this.gbAddTestResultMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAddTestResultMenu.Location = new System.Drawing.Point(22, 10);
            this.gbAddTestResultMenu.Name = "gbAddTestResultMenu";
            this.gbAddTestResultMenu.Size = new System.Drawing.Size(961, 660);
            this.gbAddTestResultMenu.TabIndex = 0;
            this.gbAddTestResultMenu.TabStop = false;
            this.gbAddTestResultMenu.Text = "QA System Main Menu";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnAddHotBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnAddProtein, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddPowder, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(312, 229);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(337, 176);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnAddHotBox
            // 
            this.btnAddHotBox.Location = new System.Drawing.Point(3, 119);
            this.btnAddHotBox.Name = "btnAddHotBox";
            this.btnAddHotBox.Size = new System.Drawing.Size(326, 50);
            this.btnAddHotBox.TabIndex = 2;
            this.btnAddHotBox.Text = "Add/Edit HotBox Test";
            this.btnAddHotBox.UseVisualStyleBackColor = true;
            this.btnAddHotBox.Click += new System.EventHandler(this.btnAddHotBox_Click);
            // 
            // btnAddProtein
            // 
            this.btnAddProtein.Location = new System.Drawing.Point(3, 3);
            this.btnAddProtein.Name = "btnAddProtein";
            this.btnAddProtein.Size = new System.Drawing.Size(326, 50);
            this.btnAddProtein.TabIndex = 0;
            this.btnAddProtein.Text = "Add/Edit Protein Test";
            this.btnAddProtein.UseVisualStyleBackColor = true;
            this.btnAddProtein.Click += new System.EventHandler(this.btnAddProtein_Click);
            // 
            // btnAddPowder
            // 
            this.btnAddPowder.Location = new System.Drawing.Point(3, 61);
            this.btnAddPowder.Name = "btnAddPowder";
            this.btnAddPowder.Size = new System.Drawing.Size(326, 50);
            this.btnAddPowder.TabIndex = 1;
            this.btnAddPowder.Text = "Add/Edit Powder Test";
            this.btnAddPowder.UseVisualStyleBackColor = true;
            this.btnAddPowder.Click += new System.EventHandler(this.btnAddPowder_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.gbAddTestResultMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QA System Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.gbAddTestResultMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAddTestResultMenu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAddHotBox;
        private System.Windows.Forms.Button btnAddProtein;
        private System.Windows.Forms.Button btnAddPowder;
    }
}