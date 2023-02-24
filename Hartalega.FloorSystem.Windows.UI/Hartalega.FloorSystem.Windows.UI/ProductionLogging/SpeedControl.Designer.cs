namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    partial class SpeedControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeedControl));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.productionActivitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productionLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineSpeedControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvGloveData = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GloveCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EffectiveDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbLine = new System.Windows.Forms.ComboBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.gBAddControl = new System.Windows.Forms.GroupBox();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.lblTimeAdd = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblEffectiveDateAdd = new System.Windows.Forms.Label();
            this.btnSpeedAdd = new System.Windows.Forms.Button();
            this.tbSpeed = new System.Windows.Forms.TextBox();
            this.lblSpeedAdd = new System.Windows.Forms.Label();
            this.cmbGloveAdd = new System.Windows.Forms.ComboBox();
            this.lblGloveAdd = new System.Windows.Forms.Label();
            this.cmbLineAdd = new System.Windows.Forms.ComboBox();
            this.lblLineAdd = new System.Windows.Forms.Label();
            this.lblGlove = new System.Windows.Forms.Label();
            this.cmbGlove = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGloveData)).BeginInit();
            this.gBAddControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productionActivitiesToolStripMenuItem,
            this.productionLineToolStripMenuItem,
            this.lineSpeedControlToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // productionActivitiesToolStripMenuItem
            // 
            this.productionActivitiesToolStripMenuItem.Name = "productionActivitiesToolStripMenuItem";
            this.productionActivitiesToolStripMenuItem.Size = new System.Drawing.Size(129, 20);
            this.productionActivitiesToolStripMenuItem.Text = "Production Activities";
            this.productionActivitiesToolStripMenuItem.Click += new System.EventHandler(this.productionActivitiesToolStripMenuItem_Click);
            // 
            // productionLineToolStripMenuItem
            // 
            this.productionLineToolStripMenuItem.Name = "productionLineToolStripMenuItem";
            this.productionLineToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
            this.productionLineToolStripMenuItem.Text = "Production Line Start / Stop";
            this.productionLineToolStripMenuItem.Click += new System.EventHandler(this.productionLineToolStripMenuItem_Click);
            // 
            // lineSpeedControlToolStripMenuItem
            // 
            this.lineSpeedControlToolStripMenuItem.Name = "lineSpeedControlToolStripMenuItem";
            this.lineSpeedControlToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.lineSpeedControlToolStripMenuItem.Text = "Speed Control";
            this.lineSpeedControlToolStripMenuItem.Click += new System.EventHandler(this.lineSpeedControlToolStripMenuItem_Click);
            // 
            // dgvGloveData
            // 
            this.dgvGloveData.AllowUserToAddRows = false;
            this.dgvGloveData.AllowUserToDeleteRows = false;
            this.dgvGloveData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGloveData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Line,
            this.GloveCode,
            this.Speed,
            this.EffectiveDateTime});
            this.dgvGloveData.Location = new System.Drawing.Point(12, 96);
            this.dgvGloveData.Name = "dgvGloveData";
            this.dgvGloveData.ReadOnly = true;
            this.dgvGloveData.Size = new System.Drawing.Size(793, 401);
            this.dgvGloveData.TabIndex = 1;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // Line
            // 
            this.Line.HeaderText = "Line";
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            // 
            // GloveCode
            // 
            this.GloveCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GloveCode.HeaderText = "GloveCode";
            this.GloveCode.Name = "GloveCode";
            this.GloveCode.ReadOnly = true;
            // 
            // Speed
            // 
            this.Speed.HeaderText = "Speed";
            this.Speed.Name = "Speed";
            this.Speed.ReadOnly = true;
            // 
            // EffectiveDateTime
            // 
            this.EffectiveDateTime.HeaderText = "EffectiveDateTime";
            this.EffectiveDateTime.Name = "EffectiveDateTime";
            this.EffectiveDateTime.ReadOnly = true;
            this.EffectiveDateTime.Width = 150;
            // 
            // cmbLine
            // 
            this.cmbLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLine.FormattingEnabled = true;
            this.cmbLine.Location = new System.Drawing.Point(71, 43);
            this.cmbLine.Name = "cmbLine";
            this.cmbLine.Size = new System.Drawing.Size(121, 21);
            this.cmbLine.TabIndex = 2;
            this.cmbLine.SelectedIndexChanged += new System.EventHandler(this.cmbLine_SelectedIndexChanged);
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(12, 46);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(27, 13);
            this.lblLine.TabIndex = 3;
            this.lblLine.Text = "Line";
            // 
            // gBAddControl
            // 
            this.gBAddControl.Controls.Add(this.dtpTime);
            this.gBAddControl.Controls.Add(this.lblTimeAdd);
            this.gBAddControl.Controls.Add(this.dtpDate);
            this.gBAddControl.Controls.Add(this.lblEffectiveDateAdd);
            this.gBAddControl.Controls.Add(this.btnSpeedAdd);
            this.gBAddControl.Controls.Add(this.tbSpeed);
            this.gBAddControl.Controls.Add(this.lblSpeedAdd);
            this.gBAddControl.Controls.Add(this.cmbGloveAdd);
            this.gBAddControl.Controls.Add(this.lblGloveAdd);
            this.gBAddControl.Controls.Add(this.cmbLineAdd);
            this.gBAddControl.Controls.Add(this.lblLineAdd);
            this.gBAddControl.Location = new System.Drawing.Point(12, 512);
            this.gBAddControl.Name = "gBAddControl";
            this.gBAddControl.Size = new System.Drawing.Size(614, 117);
            this.gBAddControl.TabIndex = 5;
            this.gBAddControl.TabStop = false;
            this.gBAddControl.Text = "Speed Control";
            // 
            // dtpTime
            // 
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(476, 20);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(116, 20);
            this.dtpTime.TabIndex = 11;
            // 
            // lblTimeAdd
            // 
            this.lblTimeAdd.AutoSize = true;
            this.lblTimeAdd.Location = new System.Drawing.Point(440, 22);
            this.lblTimeAdd.Name = "lblTimeAdd";
            this.lblTimeAdd.Size = new System.Drawing.Size(30, 13);
            this.lblTimeAdd.TabIndex = 12;
            this.lblTimeAdd.Text = "Time";
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(295, 20);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(139, 20);
            this.dtpDate.TabIndex = 10;
            // 
            // lblEffectiveDateAdd
            // 
            this.lblEffectiveDateAdd.AutoSize = true;
            this.lblEffectiveDateAdd.Location = new System.Drawing.Point(214, 22);
            this.lblEffectiveDateAdd.Name = "lblEffectiveDateAdd";
            this.lblEffectiveDateAdd.Size = new System.Drawing.Size(75, 13);
            this.lblEffectiveDateAdd.TabIndex = 9;
            this.lblEffectiveDateAdd.Text = "Effective Date";
            // 
            // btnSpeedAdd
            // 
            this.btnSpeedAdd.Location = new System.Drawing.Point(517, 72);
            this.btnSpeedAdd.Name = "btnSpeedAdd";
            this.btnSpeedAdd.Size = new System.Drawing.Size(75, 23);
            this.btnSpeedAdd.TabIndex = 8;
            this.btnSpeedAdd.Text = "Add Speed";
            this.btnSpeedAdd.UseVisualStyleBackColor = true;
            this.btnSpeedAdd.Click += new System.EventHandler(this.btnSpeedAdd_Click);
            // 
            // tbSpeed
            // 
            this.tbSpeed.Location = new System.Drawing.Point(87, 74);
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(121, 20);
            this.tbSpeed.TabIndex = 7;
            // 
            // lblSpeedAdd
            // 
            this.lblSpeedAdd.AutoSize = true;
            this.lblSpeedAdd.Location = new System.Drawing.Point(15, 77);
            this.lblSpeedAdd.Name = "lblSpeedAdd";
            this.lblSpeedAdd.Size = new System.Drawing.Size(38, 13);
            this.lblSpeedAdd.TabIndex = 6;
            this.lblSpeedAdd.Text = "Speed";
            // 
            // cmbGloveAdd
            // 
            this.cmbGloveAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGloveAdd.Location = new System.Drawing.Point(87, 46);
            this.cmbGloveAdd.Name = "cmbGloveAdd";
            this.cmbGloveAdd.Size = new System.Drawing.Size(505, 21);
            this.cmbGloveAdd.TabIndex = 11;
            this.cmbGloveAdd.SelectedIndexChanged += new System.EventHandler(this.cmbGloveAdd_SelectedIndexChanged);
            // 
            // lblGloveAdd
            // 
            this.lblGloveAdd.AutoSize = true;
            this.lblGloveAdd.Location = new System.Drawing.Point(15, 50);
            this.lblGloveAdd.Name = "lblGloveAdd";
            this.lblGloveAdd.Size = new System.Drawing.Size(35, 13);
            this.lblGloveAdd.TabIndex = 4;
            this.lblGloveAdd.Text = "Glove";
            // 
            // cmbLineAdd
            // 
            this.cmbLineAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineAdd.FormattingEnabled = true;
            this.cmbLineAdd.Location = new System.Drawing.Point(87, 19);
            this.cmbLineAdd.Name = "cmbLineAdd";
            this.cmbLineAdd.Size = new System.Drawing.Size(121, 21);
            this.cmbLineAdd.TabIndex = 3;
            // 
            // lblLineAdd
            // 
            this.lblLineAdd.AutoSize = true;
            this.lblLineAdd.Location = new System.Drawing.Point(15, 22);
            this.lblLineAdd.Name = "lblLineAdd";
            this.lblLineAdd.Size = new System.Drawing.Size(27, 13);
            this.lblLineAdd.TabIndex = 0;
            this.lblLineAdd.Text = "Line";
            // 
            // lblGlove
            // 
            this.lblGlove.AutoSize = true;
            this.lblGlove.Location = new System.Drawing.Point(12, 72);
            this.lblGlove.Name = "lblGlove";
            this.lblGlove.Size = new System.Drawing.Size(35, 13);
            this.lblGlove.TabIndex = 6;
            this.lblGlove.Text = "Glove";
            // 
            // cmbGlove
            // 
            this.cmbGlove.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGlove.FormattingEnabled = true;
            this.cmbGlove.Location = new System.Drawing.Point(71, 69);
            this.cmbGlove.Name = "cmbGlove";
            this.cmbGlove.Size = new System.Drawing.Size(440, 21);
            this.cmbGlove.TabIndex = 7;
            this.cmbGlove.SelectedIndexChanged += new System.EventHandler(this.cmbGlove_SelectedIndexChanged);
            // 
            // SpeedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.cmbGlove);
            this.Controls.Add(this.lblGlove);
            this.Controls.Add(this.gBAddControl);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.cmbLine);
            this.Controls.Add(this.dgvGloveData);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "SpeedControl";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Line Speed Control";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpeedControl_FormClosing);
            this.Load += new System.EventHandler(this.SpeedControl_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGloveData)).EndInit();
            this.gBAddControl.ResumeLayout(false);
            this.gBAddControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem productionActivitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productionLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineSpeedControlToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgvGloveData;
        private System.Windows.Forms.ComboBox cmbLine;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.GroupBox gBAddControl;
        private System.Windows.Forms.ComboBox cmbLineAdd;
        private System.Windows.Forms.Label lblLineAdd;
        private System.Windows.Forms.Button btnSpeedAdd;
        private System.Windows.Forms.TextBox tbSpeed;
        private System.Windows.Forms.Label lblSpeedAdd;
        private System.Windows.Forms.ComboBox cmbGloveAdd;
        private System.Windows.Forms.Label lblGloveAdd;
        private System.Windows.Forms.Label lblGlove;
        private System.Windows.Forms.ComboBox cmbGlove;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn GloveCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn EffectiveDateTime;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblEffectiveDateAdd;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label lblTimeAdd;

    }
}