namespace Hartalega.FloorSystem.Windows.UI.Washer
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
            this.dgvWasherProgram = new System.Windows.Forms.DataGridView();
            this.WasherProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Totalminutes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stopped = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Recid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WasherProgramId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvWasherProcess = new System.Windows.Forms.DataGridView();
            this.Stage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Process = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minutes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WasherprocessId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnProcessAdd = new System.Windows.Forms.Button();
            this.btnProcessRemove = new System.Windows.Forms.Button();
            this.btnStageAdd = new System.Windows.Forms.Button();
            this.btnStageRemove = new System.Windows.Forms.Button();
            this.btnProcessEdit = new System.Windows.Forms.Button();
            this.btnStageEdit = new System.Windows.Forms.Button();
            this.lblWasherStageHeader = new System.Windows.Forms.Label();
            this.lblWasherProgramHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProgram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvWasherProgram
            // 
            this.dgvWasherProgram.AllowUserToDeleteRows = false;
            this.dgvWasherProgram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWasherProgram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WasherProgram,
            this.Totalminutes,
            this.Stopped,
            this.Recid,
            this.WasherProgramId});
            this.dgvWasherProgram.Location = new System.Drawing.Point(18, 63);
            this.dgvWasherProgram.Name = "dgvWasherProgram";
            this.dgvWasherProgram.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWasherProgram.Size = new System.Drawing.Size(418, 382);
            this.dgvWasherProgram.TabIndex = 0;
            this.dgvWasherProgram.SelectionChanged += new System.EventHandler(this.dgvWasherProgram_SelectionChanged);
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
            // Recid
            // 
            this.Recid.HeaderText = "RecId";
            this.Recid.Name = "Recid";
            this.Recid.Visible = false;
            // 
            // WasherProgramId
            // 
            this.WasherProgramId.HeaderText = "WasherProgramId";
            this.WasherProgramId.Name = "WasherProgramId";
            this.WasherProgramId.Visible = false;
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
            // btnProcessAdd
            // 
            this.btnProcessAdd.Location = new System.Drawing.Point(343, 34);
            this.btnProcessAdd.Name = "btnProcessAdd";
            this.btnProcessAdd.Size = new System.Drawing.Size(27, 23);
            this.btnProcessAdd.TabIndex = 2;
            this.btnProcessAdd.UseVisualStyleBackColor = true;
            this.btnProcessAdd.Click += new System.EventHandler(this.btnProcessAdd_Click);
            // 
            // btnProcessRemove
            // 
            this.btnProcessRemove.Location = new System.Drawing.Point(409, 34);
            this.btnProcessRemove.Name = "btnProcessRemove";
            this.btnProcessRemove.Size = new System.Drawing.Size(27, 23);
            this.btnProcessRemove.TabIndex = 3;
            this.btnProcessRemove.UseVisualStyleBackColor = true;
            this.btnProcessRemove.Visible = false;
            this.btnProcessRemove.Click += new System.EventHandler(this.btnProcessRemove_Click);
            // 
            // btnStageAdd
            // 
            this.btnStageAdd.Location = new System.Drawing.Point(769, 34);
            this.btnStageAdd.Name = "btnStageAdd";
            this.btnStageAdd.Size = new System.Drawing.Size(29, 23);
            this.btnStageAdd.TabIndex = 4;
            this.btnStageAdd.UseVisualStyleBackColor = true;
            this.btnStageAdd.Click += new System.EventHandler(this.btnStageAdd_Click);
            // 
            // btnStageRemove
            // 
            this.btnStageRemove.Location = new System.Drawing.Point(839, 34);
            this.btnStageRemove.Name = "btnStageRemove";
            this.btnStageRemove.Size = new System.Drawing.Size(27, 23);
            this.btnStageRemove.TabIndex = 5;
            this.btnStageRemove.UseVisualStyleBackColor = true;
            this.btnStageRemove.Click += new System.EventHandler(this.btnStageRemove_Click);
            // 
            // btnProcessEdit
            // 
            this.btnProcessEdit.Location = new System.Drawing.Point(376, 34);
            this.btnProcessEdit.Name = "btnProcessEdit";
            this.btnProcessEdit.Size = new System.Drawing.Size(27, 23);
            this.btnProcessEdit.TabIndex = 6;
            this.btnProcessEdit.UseVisualStyleBackColor = true;
            this.btnProcessEdit.Click += new System.EventHandler(this.btnProcessEdit_Click);
            // 
            // btnStageEdit
            // 
            this.btnStageEdit.Location = new System.Drawing.Point(804, 34);
            this.btnStageEdit.Name = "btnStageEdit";
            this.btnStageEdit.Size = new System.Drawing.Size(29, 23);
            this.btnStageEdit.TabIndex = 7;
            this.btnStageEdit.UseVisualStyleBackColor = true;
            this.btnStageEdit.Click += new System.EventHandler(this.btnStageEdit_Click);
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
            // WasherProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.lblWasherProgramHeader);
            this.Controls.Add(this.lblWasherStageHeader);
            this.Controls.Add(this.dgvWasherProgram);
            this.Controls.Add(this.btnProcessAdd);
            this.Controls.Add(this.btnStageEdit);
            this.Controls.Add(this.btnProcessEdit);
            this.Controls.Add(this.btnStageRemove);
            this.Controls.Add(this.btnStageAdd);
            this.Controls.Add(this.btnProcessRemove);
            this.Controls.Add(this.dgvWasherProcess);
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "WasherProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WasherProcess";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.WasherProcess_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WasherProcess_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProgram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWasherProcess)).EndInit();
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
        private System.Windows.Forms.Button btnProcessAdd;
        private System.Windows.Forms.Button btnProcessRemove;
        private System.Windows.Forms.Button btnStageAdd;
        private System.Windows.Forms.Button btnStageRemove;
        private System.Windows.Forms.Button btnProcessEdit;
        private System.Windows.Forms.Button btnStageEdit;
        private System.Windows.Forms.Label lblWasherStageHeader;
        private System.Windows.Forms.Label lblWasherProgramHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasherProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn Totalminutes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Stopped;
        private System.Windows.Forms.DataGridViewTextBoxColumn Recid;
        private System.Windows.Forms.DataGridViewTextBoxColumn WasherProgramId;
    }
}