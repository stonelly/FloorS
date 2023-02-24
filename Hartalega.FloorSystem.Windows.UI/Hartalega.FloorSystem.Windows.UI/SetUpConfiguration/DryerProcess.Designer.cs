namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class DryerProcess
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
            this.dgvDryerProcess = new System.Windows.Forms.DataGridView();
            this.CycloneProcess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RCold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RHot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.R2Cold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.R2Hot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stopped = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CycloneProcessID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbAddReason = new System.Windows.Forms.GroupBox();
            this.txtHotCycle3 = new System.Windows.Forms.TextBox();
            this.lblHot3 = new System.Windows.Forms.Label();
            this.txtColdCycle3 = new System.Windows.Forms.TextBox();
            this.txtHotCycle2 = new System.Windows.Forms.TextBox();
            this.lblCold3 = new System.Windows.Forms.Label();
            this.lblHot2 = new System.Windows.Forms.Label();
            this.txtHotCycle1 = new System.Windows.Forms.TextBox();
            this.txtColdCycle2 = new System.Windows.Forms.TextBox();
            this.lblHot1 = new System.Windows.Forms.Label();
            this.lblCold2 = new System.Windows.Forms.Label();
            this.txtColdCycle1 = new System.Windows.Forms.TextBox();
            this.lblCold1 = new System.Windows.Forms.Label();
            this.lblDash1 = new System.Windows.Forms.Label();
            this.lblRemark1 = new System.Windows.Forms.Label();
            this.lblDash2 = new System.Windows.Forms.Label();
            this.lblRemark2 = new System.Windows.Forms.Label();
            this.lblProcessTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.lblStopped = new System.Windows.Forms.Label();
            this.lblDryerProcess = new System.Windows.Forms.Label();
            this.chkStopped = new System.Windows.Forms.CheckBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.pbDryerAdd = new System.Windows.Forms.ToolStripButton();
            this.pbDryerEdit = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDryerProcess)).BeginInit();
            this.gbAddReason.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDryerProcess
            // 
            this.dgvDryerProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDryerProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CycloneProcess,
            this.Cold,
            this.Hot,
            this.RCold,
            this.RHot,
            this.R2Cold,
            this.R2Hot,
            this.Stopped,
            this.CycloneProcessID});
            this.dgvDryerProcess.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDryerProcess.Location = new System.Drawing.Point(3, 64);
            this.dgvDryerProcess.Name = "dgvDryerProcess";
            this.dgvDryerProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDryerProcess.Size = new System.Drawing.Size(492, 400);
            this.dgvDryerProcess.TabIndex = 0;
            this.dgvDryerProcess.SelectionChanged += new System.EventHandler(this.dgvDryerProcess_SelectionChanged);
            // 
            // CycloneProcess
            // 
            this.CycloneProcess.HeaderText = "Dryer Process";
            this.CycloneProcess.Name = "CycloneProcess";
            // 
            // Cold
            // 
            this.Cold.HeaderText = "Cold";
            this.Cold.Name = "Cold";
            // 
            // Hot
            // 
            this.Hot.HeaderText = "Hot";
            this.Hot.Name = "Hot";
            // 
            // RCold
            // 
            this.RCold.HeaderText = "R Cold";
            this.RCold.Name = "RCold";
            // 
            // RHot
            // 
            this.RHot.HeaderText = "R Hot";
            this.RHot.Name = "RHot";
            // 
            // R2Cold
            // 
            this.R2Cold.HeaderText = "R2 Cold";
            this.R2Cold.Name = "R2Cold";
            // 
            // R2Hot
            // 
            this.R2Hot.HeaderText = "R2 Hot";
            this.R2Hot.Name = "R2Hot";
            // 
            // Stopped
            // 
            this.Stopped.HeaderText = "Stopped";
            this.Stopped.Name = "Stopped";
            // 
            // CycloneProcessID
            // 
            this.CycloneProcessID.HeaderText = "CycloneProcessID";
            this.CycloneProcessID.Name = "CycloneProcessID";
            this.CycloneProcessID.Visible = false;
            // 
            // gbAddReason
            // 
            this.gbAddReason.Controls.Add(this.txtHotCycle3);
            this.gbAddReason.Controls.Add(this.lblHot3);
            this.gbAddReason.Controls.Add(this.txtColdCycle3);
            this.gbAddReason.Controls.Add(this.txtHotCycle2);
            this.gbAddReason.Controls.Add(this.lblCold3);
            this.gbAddReason.Controls.Add(this.lblHot2);
            this.gbAddReason.Controls.Add(this.txtHotCycle1);
            this.gbAddReason.Controls.Add(this.txtColdCycle2);
            this.gbAddReason.Controls.Add(this.lblHot1);
            this.gbAddReason.Controls.Add(this.lblCold2);
            this.gbAddReason.Controls.Add(this.txtColdCycle1);
            this.gbAddReason.Controls.Add(this.lblCold1);
            this.gbAddReason.Controls.Add(this.lblDash1);
            this.gbAddReason.Controls.Add(this.lblRemark1);
            this.gbAddReason.Controls.Add(this.lblDash2);
            this.gbAddReason.Controls.Add(this.lblRemark2);
            this.gbAddReason.Controls.Add(this.lblProcessTime);
            this.gbAddReason.Controls.Add(this.tableLayoutPanel1);
            this.gbAddReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAddReason.Location = new System.Drawing.Point(501, 64);
            this.gbAddReason.Name = "gbAddReason";
            this.gbAddReason.Size = new System.Drawing.Size(600, 384);
            this.gbAddReason.TabIndex = 6;
            this.gbAddReason.TabStop = false;
            // 
            // txtHotCycle3
            // 
            this.txtHotCycle3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtHotCycle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHotCycle3.Location = new System.Drawing.Point(515, 306);
            this.txtHotCycle3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHotCycle3.Name = "txtHotCycle3";
            this.txtHotCycle3.Size = new System.Drawing.Size(74, 23);
            this.txtHotCycle3.TabIndex = 15;
            this.txtHotCycle3.TabStop = false;
            // 
            // lblHot3
            // 
            this.lblHot3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblHot3.AutoSize = true;
            this.lblHot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHot3.Location = new System.Drawing.Point(419, 307);
            this.lblHot3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHot3.Name = "lblHot3";
            this.lblHot3.Size = new System.Drawing.Size(92, 18);
            this.lblHot3.TabIndex = 14;
            this.lblHot3.Text = "Hot Cycle :";
            // 
            // txtColdCycle3
            // 
            this.txtColdCycle3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtColdCycle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColdCycle3.Location = new System.Drawing.Point(515, 261);
            this.txtColdCycle3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtColdCycle3.Name = "txtColdCycle3";
            this.txtColdCycle3.Size = new System.Drawing.Size(74, 23);
            this.txtColdCycle3.TabIndex = 13;
            this.txtColdCycle3.TabStop = false;
            // 
            // txtHotCycle2
            // 
            this.txtHotCycle2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtHotCycle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHotCycle2.Location = new System.Drawing.Point(325, 306);
            this.txtHotCycle2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHotCycle2.Name = "txtHotCycle2";
            this.txtHotCycle2.Size = new System.Drawing.Size(76, 23);
            this.txtHotCycle2.TabIndex = 13;
            this.txtHotCycle2.TabStop = false;
            // 
            // lblCold3
            // 
            this.lblCold3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCold3.AutoSize = true;
            this.lblCold3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCold3.Location = new System.Drawing.Point(411, 264);
            this.lblCold3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCold3.Name = "lblCold3";
            this.lblCold3.Size = new System.Drawing.Size(100, 18);
            this.lblCold3.TabIndex = 12;
            this.lblCold3.Text = "Cold Cycle :";
            // 
            // lblHot2
            // 
            this.lblHot2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblHot2.AutoSize = true;
            this.lblHot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHot2.Location = new System.Drawing.Point(229, 309);
            this.lblHot2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHot2.Name = "lblHot2";
            this.lblHot2.Size = new System.Drawing.Size(92, 18);
            this.lblHot2.TabIndex = 12;
            this.lblHot2.Text = "Hot Cycle :";
            // 
            // txtHotCycle1
            // 
            this.txtHotCycle1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtHotCycle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHotCycle1.Location = new System.Drawing.Point(140, 306);
            this.txtHotCycle1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHotCycle1.Name = "txtHotCycle1";
            this.txtHotCycle1.Size = new System.Drawing.Size(75, 23);
            this.txtHotCycle1.TabIndex = 11;
            this.txtHotCycle1.TabStop = false;
            // 
            // txtColdCycle2
            // 
            this.txtColdCycle2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtColdCycle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColdCycle2.Location = new System.Drawing.Point(325, 261);
            this.txtColdCycle2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtColdCycle2.Name = "txtColdCycle2";
            this.txtColdCycle2.Size = new System.Drawing.Size(76, 23);
            this.txtColdCycle2.TabIndex = 11;
            this.txtColdCycle2.TabStop = false;
            // 
            // lblHot1
            // 
            this.lblHot1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblHot1.AutoSize = true;
            this.lblHot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHot1.Location = new System.Drawing.Point(44, 309);
            this.lblHot1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHot1.Name = "lblHot1";
            this.lblHot1.Size = new System.Drawing.Size(92, 18);
            this.lblHot1.TabIndex = 10;
            this.lblHot1.Text = "Hot Cycle :";
            // 
            // lblCold2
            // 
            this.lblCold2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCold2.AutoSize = true;
            this.lblCold2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCold2.Location = new System.Drawing.Point(221, 264);
            this.lblCold2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCold2.Name = "lblCold2";
            this.lblCold2.Size = new System.Drawing.Size(100, 18);
            this.lblCold2.TabIndex = 10;
            this.lblCold2.Text = "Cold Cycle :";
            // 
            // txtColdCycle1
            // 
            this.txtColdCycle1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtColdCycle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColdCycle1.Location = new System.Drawing.Point(140, 261);
            this.txtColdCycle1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtColdCycle1.Name = "txtColdCycle1";
            this.txtColdCycle1.Size = new System.Drawing.Size(75, 23);
            this.txtColdCycle1.TabIndex = 9;
            this.txtColdCycle1.TabStop = false;
            // 
            // lblCold1
            // 
            this.lblCold1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCold1.AutoSize = true;
            this.lblCold1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCold1.Location = new System.Drawing.Point(36, 264);
            this.lblCold1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCold1.Name = "lblCold1";
            this.lblCold1.Size = new System.Drawing.Size(100, 18);
            this.lblCold1.TabIndex = 8;
            this.lblCold1.Text = "Cold Cycle :";
            // 
            // lblDash1
            // 
            this.lblDash1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDash1.AutoSize = true;
            this.lblDash1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDash1.Location = new System.Drawing.Point(505, 206);
            this.lblDash1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDash1.Name = "lblDash1";
            this.lblDash1.Size = new System.Drawing.Size(67, 29);
            this.lblDash1.TabIndex = 7;
            this.lblDash1.Text = "------";
            // 
            // lblRemark1
            // 
            this.lblRemark1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRemark1.AutoSize = true;
            this.lblRemark1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemark1.Location = new System.Drawing.Point(232, 215);
            this.lblRemark1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemark1.Name = "lblRemark1";
            this.lblRemark1.Size = new System.Drawing.Size(90, 18);
            this.lblRemark1.TabIndex = 6;
            this.lblRemark1.Text = "Rework 1 :";
            // 
            // lblDash2
            // 
            this.lblDash2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDash2.AutoSize = true;
            this.lblDash2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDash2.Location = new System.Drawing.Point(320, 207);
            this.lblDash2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDash2.Name = "lblDash2";
            this.lblDash2.Size = new System.Drawing.Size(67, 29);
            this.lblDash2.TabIndex = 5;
            this.lblDash2.Text = "------";
            // 
            // lblRemark2
            // 
            this.lblRemark2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRemark2.AutoSize = true;
            this.lblRemark2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemark2.Location = new System.Drawing.Point(417, 214);
            this.lblRemark2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemark2.Name = "lblRemark2";
            this.lblRemark2.Size = new System.Drawing.Size(90, 18);
            this.lblRemark2.TabIndex = 4;
            this.lblRemark2.Text = "Rework 2 :";
            // 
            // lblProcessTime
            // 
            this.lblProcessTime.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProcessTime.AutoSize = true;
            this.lblProcessTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessTime.Location = new System.Drawing.Point(54, 195);
            this.lblProcessTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProcessTime.Name = "lblProcessTime";
            this.lblProcessTime.Size = new System.Drawing.Size(113, 18);
            this.lblProcessTime.TabIndex = 3;
            this.lblProcessTime.Text = "Process Time";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.48649F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.51351F));
            this.tableLayoutPanel1.Controls.Add(this.txtProcess, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStopped, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDryerProcess, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkStopped, 1, 1);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.51546F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.48454F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 97);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtProcess
            // 
            this.txtProcess.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcess.Location = new System.Drawing.Point(176, 6);
            this.txtProcess.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.ReadOnly = true;
            this.txtProcess.Size = new System.Drawing.Size(170, 35);
            this.txtProcess.TabIndex = 0;
            this.txtProcess.TabStop = false;
            // 
            // lblStopped
            // 
            this.lblStopped.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStopped.AutoSize = true;
            this.lblStopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStopped.Location = new System.Drawing.Point(88, 63);
            this.lblStopped.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStopped.Name = "lblStopped";
            this.lblStopped.Size = new System.Drawing.Size(80, 18);
            this.lblStopped.TabIndex = 2;
            this.lblStopped.Text = "Stopped :";
            // 
            // lblDryerProcess
            // 
            this.lblDryerProcess.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDryerProcess.AutoSize = true;
            this.lblDryerProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDryerProcess.Location = new System.Drawing.Point(41, 15);
            this.lblDryerProcess.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDryerProcess.Name = "lblDryerProcess";
            this.lblDryerProcess.Size = new System.Drawing.Size(127, 18);
            this.lblDryerProcess.TabIndex = 0;
            this.lblDryerProcess.Text = "Dryer Process :";
            // 
            // chkStopped
            // 
            this.chkStopped.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkStopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStopped.Location = new System.Drawing.Point(176, 58);
            this.chkStopped.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkStopped.Name = "chkStopped";
            this.chkStopped.Size = new System.Drawing.Size(22, 28);
            this.chkStopped.TabIndex = 2;
            this.chkStopped.UseVisualStyleBackColor = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbDryerAdd,
            this.pbDryerEdit});
            this.toolStrip2.Location = new System.Drawing.Point(17, 6);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.ShowItemToolTips = false;
            this.toolStrip2.Size = new System.Drawing.Size(58, 25);
            this.toolStrip2.TabIndex = 37;
            this.toolStrip2.TabStop = true;
            // 
            // pbDryerAdd
            // 
            this.pbDryerAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbDryerAdd.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.pbDryerAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbDryerAdd.Name = "pbDryerAdd";
            this.pbDryerAdd.Size = new System.Drawing.Size(23, 22);
            this.pbDryerAdd.Click += new System.EventHandler(this.pbDryerAdd_Click);
            // 
            // pbDryerEdit
            // 
            this.pbDryerEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbDryerEdit.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.pbDryerEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbDryerEdit.Name = "pbDryerEdit";
            this.pbDryerEdit.Size = new System.Drawing.Size(23, 22);
            this.pbDryerEdit.Click += new System.EventHandler(this.pbDryerEdit_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(421, 27);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.3913F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(93, 31);
            this.tableLayoutPanel2.TabIndex = 38;
            // 
            // DryerProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1095, 693);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.gbAddReason);
            this.Controls.Add(this.dgvDryerProcess);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "DryerProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DryerProcess";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DryerProcess_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DryerProcess_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDryerProcess)).EndInit();
            this.gbAddReason.ResumeLayout(false);
            this.gbAddReason.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDryerProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn CycloneProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cold;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hot;
        private System.Windows.Forms.DataGridViewTextBoxColumn RCold;
        private System.Windows.Forms.DataGridViewTextBoxColumn RHot;
        private System.Windows.Forms.DataGridViewTextBoxColumn R2Cold;
        private System.Windows.Forms.DataGridViewTextBoxColumn R2Hot;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Stopped;
        private System.Windows.Forms.DataGridViewTextBoxColumn CycloneProcessID;
        private System.Windows.Forms.GroupBox gbAddReason;
        private System.Windows.Forms.TextBox txtHotCycle3;
        private System.Windows.Forms.Label lblHot3;
        private System.Windows.Forms.TextBox txtColdCycle3;
        private System.Windows.Forms.TextBox txtHotCycle2;
        private System.Windows.Forms.Label lblCold3;
        private System.Windows.Forms.Label lblHot2;
        private System.Windows.Forms.TextBox txtHotCycle1;
        private System.Windows.Forms.TextBox txtColdCycle2;
        private System.Windows.Forms.Label lblHot1;
        private System.Windows.Forms.Label lblCold2;
        private System.Windows.Forms.TextBox txtColdCycle1;
        private System.Windows.Forms.Label lblCold1;
        private System.Windows.Forms.Label lblDash1;
        private System.Windows.Forms.Label lblRemark1;
        private System.Windows.Forms.Label lblDash2;
        private System.Windows.Forms.Label lblRemark2;
        private System.Windows.Forms.Label lblProcessTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.CheckBox chkStopped;
        private System.Windows.Forms.Label lblStopped;
        private System.Windows.Forms.Label lblDryerProcess;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton pbDryerAdd;
        private System.Windows.Forms.ToolStripButton pbDryerEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}