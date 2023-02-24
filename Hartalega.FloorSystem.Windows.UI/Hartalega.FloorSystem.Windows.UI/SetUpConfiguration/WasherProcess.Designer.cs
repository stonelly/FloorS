namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class WasherProcess
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
            this.lblWasherProgramHeader = new System.Windows.Forms.Label();
            this.lblWasherStageHeader = new System.Windows.Forms.Label();
            this.dgvWasherProgram = new System.Windows.Forms.DataGridView();
            this.dgvWasherProcess = new System.Windows.Forms.DataGridView();
            this.Stage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Process = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minutes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WasherprocessId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.pbProgramAdd = new System.Windows.Forms.ToolStripButton();
            this.pbProgramEdit = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.pbStageAdd = new System.Windows.Forms.ToolStripButton();
            this.pbStageEdit = new System.Windows.Forms.ToolStripButton();
            this.pbStageRemove = new System.Windows.Forms.ToolStripButton();
            this.WasherProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Totalminutes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stopped = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.WasherProgramId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProgram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProcess)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWasherProgramHeader
            // 
            this.lblWasherProgramHeader.AutoSize = true;
            this.lblWasherProgramHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWasherProgramHeader.Location = new System.Drawing.Point(14, 34);
            this.lblWasherProgramHeader.Name = "lblWasherProgramHeader";
            this.lblWasherProgramHeader.Size = new System.Drawing.Size(142, 20);
            this.lblWasherProgramHeader.TabIndex = 9;
            this.lblWasherProgramHeader.Text = "Washer Program";
            // 
            // lblWasherStageHeader
            // 
            this.lblWasherStageHeader.AutoSize = true;
            this.lblWasherStageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWasherStageHeader.Location = new System.Drawing.Point(452, 34);
            this.lblWasherStageHeader.Name = "lblWasherStageHeader";
            this.lblWasherStageHeader.Size = new System.Drawing.Size(123, 20);
            this.lblWasherStageHeader.TabIndex = 8;
            this.lblWasherStageHeader.Text = "Washer Stage";
            // 
            // dgvWasherProgram
            // 
            this.dgvWasherProgram.AllowUserToDeleteRows = false;
            this.dgvWasherProgram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWasherProgram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WasherProgram,
            this.Totalminutes,
            this.Stopped,
            this.WasherProgramId});
            this.dgvWasherProgram.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvWasherProgram.Location = new System.Drawing.Point(18, 63);
            this.dgvWasherProgram.Name = "dgvWasherProgram";
            this.dgvWasherProgram.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWasherProgram.Size = new System.Drawing.Size(418, 382);
            this.dgvWasherProgram.TabIndex = 0;
            this.dgvWasherProgram.SelectionChanged += new System.EventHandler(this.dgvWasherProgram_SelectionChanged);
            // 
            // dgvWasherProcess
            // 
            this.dgvWasherProcess.AllowUserToDeleteRows = false;
            this.dgvWasherProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWasherProcess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Stage,
            this.Process,
            this.Minutes,
            this.WasherprocessId});
            this.dgvWasherProcess.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvWasherProcess.Location = new System.Drawing.Point(442, 63);
            this.dgvWasherProcess.Name = "dgvWasherProcess";
            this.dgvWasherProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWasherProcess.Size = new System.Drawing.Size(424, 382);
            this.dgvWasherProcess.TabIndex = 1;
            // 
            // Stage
            // 
            this.Stage.HeaderText = "Stage";
            this.Stage.Name = "Stage";
            // 
            // Process
            // 
            this.Process.HeaderText = "Process";
            this.Process.Name = "Process";
            // 
            // Minutes
            // 
            this.Minutes.HeaderText = "Minutes";
            this.Minutes.Name = "Minutes";
            // 
            // WasherprocessId
            // 
            this.WasherprocessId.HeaderText = "WasherprocessId";
            this.WasherprocessId.Name = "WasherprocessId";
            this.WasherprocessId.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(357, 31);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.3913F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(93, 31);
            this.tableLayoutPanel2.TabIndex = 39;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbProgramAdd,
            this.pbProgramEdit});
            this.toolStrip2.Location = new System.Drawing.Point(17, 6);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.ShowItemToolTips = false;
            this.toolStrip2.Size = new System.Drawing.Size(58, 25);
            this.toolStrip2.TabIndex = 37;
            this.toolStrip2.TabStop = true;
            // 
            // pbProgramAdd
            // 
            this.pbProgramAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbProgramAdd.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.pbProgramAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbProgramAdd.Name = "pbProgramAdd";
            this.pbProgramAdd.Size = new System.Drawing.Size(23, 22);
            this.pbProgramAdd.Click += new System.EventHandler(this.pbProgramAdd_Click);
            // 
            // pbProgramEdit
            // 
            this.pbProgramEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbProgramEdit.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.pbProgramEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbProgramEdit.Name = "pbProgramEdit";
            this.pbProgramEdit.Size = new System.Drawing.Size(23, 22);
            this.pbProgramEdit.Click += new System.EventHandler(this.pbProgramEdit_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.toolStrip3, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(785, 31);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.3913F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(93, 31);
            this.tableLayoutPanel1.TabIndex = 40;
            // 
            // toolStrip3
            // 
            this.toolStrip3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbStageAdd,
            this.pbStageEdit,
            this.pbStageRemove});
            this.toolStrip3.Location = new System.Drawing.Point(6, 6);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.ShowItemToolTips = false;
            this.toolStrip3.Size = new System.Drawing.Size(81, 25);
            this.toolStrip3.TabIndex = 38;
            this.toolStrip3.TabStop = true;
            // 
            // pbStageAdd
            // 
            this.pbStageAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbStageAdd.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_add;
            this.pbStageAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbStageAdd.Name = "pbStageAdd";
            this.pbStageAdd.Size = new System.Drawing.Size(23, 22);
            this.pbStageAdd.Click += new System.EventHandler(this.pbStageAdd_Click);
            // 
            // pbStageEdit
            // 
            this.pbStageEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbStageEdit.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_edit;
            this.pbStageEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbStageEdit.Name = "pbStageEdit";
            this.pbStageEdit.Size = new System.Drawing.Size(23, 22);
            this.pbStageEdit.Click += new System.EventHandler(this.pbStageEdit_Click);
            // 
            // pbStageRemove
            // 
            this.pbStageRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pbStageRemove.Image = global::Hartalega.FloorSystem.Windows.UI.Properties.Resources.page_white_delete;
            this.pbStageRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pbStageRemove.Name = "pbStageRemove";
            this.pbStageRemove.Size = new System.Drawing.Size(23, 22);
            this.pbStageRemove.ToolTipText = "\r\nDelete";
            this.pbStageRemove.Click += new System.EventHandler(this.pbStageRemove_Click);
            // 
            // WasherProgram
            // 
            this.WasherProgram.HeaderText = "Program";
            this.WasherProgram.MinimumWidth = 7;
            this.WasherProgram.Name = "WasherProgram";
            // 
            // Totalminutes
            // 
            this.Totalminutes.HeaderText = "Total Minutes";
            this.Totalminutes.MinimumWidth = 7;
            this.Totalminutes.Name = "Totalminutes";
            // 
            // Stopped
            // 
            this.Stopped.HeaderText = "Stopped";
            this.Stopped.MinimumWidth = 7;
            this.Stopped.Name = "Stopped";
            this.Stopped.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Stopped.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // WasherProgramId
            // 
            this.WasherProgramId.HeaderText = "WasherProgramId";
            this.WasherProgramId.Name = "WasherProgramId";
            this.WasherProgramId.Visible = false;
            // 
            // WasherProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.lblWasherProgramHeader);
            this.Controls.Add(this.lblWasherStageHeader);
            this.Controls.Add(this.dgvWasherProgram);
            this.Controls.Add(this.dgvWasherProcess);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "WasherProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WasherProcess";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.WasherProcess_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WasherProcess_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProgram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProcess)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvWasherProgram;
        private System.Windows.Forms.DataGridView dgvWasherProcess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Process;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minutes;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasherprocessId;
        private System.Windows.Forms.Label lblWasherStageHeader;
        private System.Windows.Forms.Label lblWasherProgramHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton pbProgramAdd;
        private System.Windows.Forms.ToolStripButton pbProgramEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton pbStageAdd;
        private System.Windows.Forms.ToolStripButton pbStageEdit;
        private System.Windows.Forms.ToolStripButton pbStageRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasherProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn Totalminutes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Stopped;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasherProgramId;
    }
}