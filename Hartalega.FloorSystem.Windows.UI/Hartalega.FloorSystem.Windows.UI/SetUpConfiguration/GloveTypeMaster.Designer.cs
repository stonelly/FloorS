namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class GloveTypeMaster
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GloveTypeMaster));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbQAIDefectCategory = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvDefectCategory = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.AVAGLOVECODETABLE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GloveCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GloveCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Powder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Protein = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Hotbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Polymer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gbQAIDefectCategory.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefectCategory)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbQAIDefectCategory
            // 
            this.gbQAIDefectCategory.Controls.Add(this.tableLayoutPanel1);
            this.gbQAIDefectCategory.Controls.Add(this.toolStrip1);
            this.gbQAIDefectCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbQAIDefectCategory.Location = new System.Drawing.Point(12, 12);
            this.gbQAIDefectCategory.Name = "gbQAIDefectCategory";
            this.gbQAIDefectCategory.Size = new System.Drawing.Size(1289, 590);
            this.gbQAIDefectCategory.TabIndex = 0;
            this.gbQAIDefectCategory.TabStop = false;
            this.gbQAIDefectCategory.Text = "Glove Type";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.dgvDefectCategory, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 59);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 515F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1271, 515);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // dgvDefectCategory
            // 
            this.dgvDefectCategory.AllowUserToResizeRows = false;
            this.dgvDefectCategory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDefectCategory.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDefectCategory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDefectCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefectCategory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AVAGLOVECODETABLE_ID,
            this.Barcode,
            this.GloveCode,
            this.Description,
            this.GloveCategory,
            this.Powder,
            this.Protein,
            this.Hotbox,
            this.Polymer});
            this.dgvDefectCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDefectCategory.Location = new System.Drawing.Point(3, 3);
            this.dgvDefectCategory.MultiSelect = false;
            this.dgvDefectCategory.Name = "dgvDefectCategory";
            this.dgvDefectCategory.ReadOnly = true;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDefectCategory.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDefectCategory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDefectCategory.Size = new System.Drawing.Size(1265, 509);
            this.dgvDefectCategory.TabIndex = 3;
            this.dgvDefectCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDefectCategory_KeyDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnEdit,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(56, 31);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(81, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.TabStop = true;
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 22);
            this.btnAdd.Text = "toolStripButton1";
            this.btnAdd.Click += new System.EventHandler(this.btnCategoryAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(23, 22);
            this.btnEdit.Text = "toolStripButton1";
            this.btnEdit.Click += new System.EventHandler(this.btnCategoryEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "toolStripButton1";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // AVAGLOVECODETABLE_ID
            // 
            this.AVAGLOVECODETABLE_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AVAGLOVECODETABLE_ID.FillWeight = 91.37056F;
            this.AVAGLOVECODETABLE_ID.HeaderText = "ID";
            this.AVAGLOVECODETABLE_ID.Name = "AVAGLOVECODETABLE_ID";
            this.AVAGLOVECODETABLE_ID.ReadOnly = true;
            this.AVAGLOVECODETABLE_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AVAGLOVECODETABLE_ID.Visible = false;
            this.AVAGLOVECODETABLE_ID.Width = 30;
            // 
            // Barcode
            // 
            this.Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Barcode.DefaultCellStyle = dataGridViewCellStyle2;
            this.Barcode.FillWeight = 104.3147F;
            this.Barcode.HeaderText = "Barcode";
            this.Barcode.MaxInputLength = 40;
            this.Barcode.Name = "Barcode";
            this.Barcode.ReadOnly = true;
            this.Barcode.Width = 96;
            // 
            // GloveCode
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.GloveCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.GloveCode.FillWeight = 150F;
            this.GloveCode.HeaderText = "Glove Code";
            this.GloveCode.MaxInputLength = 50;
            this.GloveCode.Name = "GloveCode";
            this.GloveCode.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Description.DefaultCellStyle = dataGridViewCellStyle4;
            this.Description.FillWeight = 150F;
            this.Description.HeaderText = "Description";
            this.Description.MaxInputLength = 120;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.Width = 150;
            // 
            // GloveCategory
            // 
            this.GloveCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.GloveCategory.DefaultCellStyle = dataGridViewCellStyle5;
            this.GloveCategory.FillWeight = 104.3147F;
            this.GloveCategory.HeaderText = "Glove Category";
            this.GloveCategory.MaxInputLength = 15;
            this.GloveCategory.Name = "GloveCategory";
            this.GloveCategory.ReadOnly = true;
            this.GloveCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GloveCategory.Width = 118;
            // 
            // Powder
            // 
            this.Powder.HeaderText = "Powder";
            this.Powder.Name = "Powder";
            this.Powder.ReadOnly = true;
            // 
            // Protein
            // 
            this.Protein.HeaderText = "Protein";
            this.Protein.Name = "Protein";
            this.Protein.ReadOnly = true;
            // 
            // Hotbox
            // 
            this.Hotbox.HeaderText = "Hotbox";
            this.Hotbox.Name = "Hotbox";
            this.Hotbox.ReadOnly = true;
            // 
            // Polymer
            // 
            this.Polymer.HeaderText = "Polymer";
            this.Polymer.Name = "Polymer";
            this.Polymer.ReadOnly = true;
            // 
            // GloveTypeMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1321, 587);
            this.Controls.Add(this.gbQAIDefectCategory);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.MinimumSize = new System.Drawing.Size(1200, 572);
            this.Name = "GloveTypeMaster";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Glove Type Maintenance";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.QAIDefectMaster_Load);
            this.gbQAIDefectCategory.ResumeLayout(false);
            this.gbQAIDefectCategory.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefectCategory)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbQAIDefectCategory;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.DataGridView dgvDefectCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn AVAGLOVECODETABLE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GloveCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn GloveCategory;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Powder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Protein;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Hotbox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Polymer;
    }
}