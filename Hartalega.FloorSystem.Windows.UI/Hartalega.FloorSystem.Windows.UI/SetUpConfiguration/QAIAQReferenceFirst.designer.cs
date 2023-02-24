namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class QAIAQReferenceFirst 
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
            this.components = new System.ComponentModel.Container();
            System.Data.DataSet Locationds;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QAIAQReferenceFirst));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.bindingNavigatorTable = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dgTableMaintenance = new System.Windows.Forms.DataGridView();
            this.bindingSourceTable = new System.Windows.Forms.BindingSource(this.components);
            this.AQLID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.QCTypeId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.WTSamplingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VTSamplingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefectMinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefectMaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsResample = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ResampleRound = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerTypeId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Delete = new System.Windows.Forms.DataGridViewLinkColumn();
            Locationds = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(Locationds)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTable)).BeginInit();
            this.bindingNavigatorTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTableMaintenance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTable)).BeginInit();
            this.SuspendLayout();
            // 
            // Locationds
            // 
            Locationds.DataSetName = "NewDataSet";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 272F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 850F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bindingNavigatorTable, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(18, 18);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.42857F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1476, 118);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(4, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 57);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // bindingNavigatorTable
            // 
            this.bindingNavigatorTable.AddNewItem = null;
            this.bindingNavigatorTable.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigatorTable.DeleteItem = null;
            this.bindingNavigatorTable.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.bindingNavigatorTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigatorTable.Location = new System.Drawing.Point(0, 70);
            this.bindingNavigatorTable.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorTable.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorTable.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorTable.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorTable.Name = "bindingNavigatorTable";
            this.bindingNavigatorTable.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.bindingNavigatorTable.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorTable.Size = new System.Drawing.Size(354, 31);
            this.bindingNavigatorTable.TabIndex = 1;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(54, 28);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(28, 28);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(28, 28);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Enabled = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.ReadOnly = true;
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(73, 31);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(28, 28);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(28, 28);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // dgTableMaintenance
            // 
            this.dgTableMaintenance.AllowUserToAddRows = false;
            this.dgTableMaintenance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgTableMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTableMaintenance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AQLID,
            this.QCTypeId,
            this.WTSamplingSize,
            this.VTSamplingSize,
            this.DefectMinValue,
            this.DefectMaxValue,
            this.RefID,
            this.IsResample,
            this.ResampleRound,
            this.CustomerTypeId,
            this.Edit,
            this.Delete});
            this.dgTableMaintenance.Location = new System.Drawing.Point(18, 146);
            this.dgTableMaintenance.Margin = new System.Windows.Forms.Padding(4);
            this.dgTableMaintenance.Name = "dgTableMaintenance";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgTableMaintenance.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgTableMaintenance.RowHeadersWidth = 35;
            this.dgTableMaintenance.Size = new System.Drawing.Size(2000, 806);
            this.dgTableMaintenance.TabIndex = 1;
            this.dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTableMaintenance_CellContentClick);
            this.dgTableMaintenance.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgTableMaintenance_ColumnHeaderMouseClick);
            this.dgTableMaintenance.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgTableMaintenance_DataError);
            this.dgTableMaintenance.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgTableMaintenance_EditingControlShowing);
            this.dgTableMaintenance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QAIAQReferenceFirst_KeyDown);
            // 
            // bindingSourceTable
            // 
            this.bindingSourceTable.CurrentChanged += new System.EventHandler(this.bindingSource_CurrentChanged);
            // 
            // AQLID
            // 
            this.AQLID.FillWeight = 25F;
            this.AQLID.HeaderText = "AQL";
            this.AQLID.Name = "AQLID";
            // 
            // QCTypeId
            // 
            this.QCTypeId.FillWeight = 50F;
            this.QCTypeId.HeaderText = "QC Type";
            this.QCTypeId.Name = "QCTypeId";
            // 
            // WTSamplingSize
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.WTSamplingSize.DefaultCellStyle = dataGridViewCellStyle1;
            this.WTSamplingSize.FillWeight = 40F;
            this.WTSamplingSize.HeaderText = "Water Tight Sampling Size";
            this.WTSamplingSize.Name = "WTSamplingSize";
            this.WTSamplingSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // VTSamplingSize
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.VTSamplingSize.DefaultCellStyle = dataGridViewCellStyle2;
            this.VTSamplingSize.FillWeight = 40F;
            this.VTSamplingSize.HeaderText = "Visual Test Sampling Size";
            this.VTSamplingSize.Name = "VTSamplingSize";
            this.VTSamplingSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // DefectMinValue
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DefectMinValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.DefectMinValue.FillWeight = 30F;
            this.DefectMinValue.HeaderText = "Minimum Value";
            this.DefectMinValue.Name = "DefectMinValue";
            this.DefectMinValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // DefectMaxValue
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.DefectMaxValue.DefaultCellStyle = dataGridViewCellStyle4;
            this.DefectMaxValue.FillWeight = 30F;
            this.DefectMaxValue.HeaderText = "Maximum Value";
            this.DefectMaxValue.Name = "DefectMaxValue";
            this.DefectMaxValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // RefID
            // 
            this.RefID.HeaderText = "RefID";
            this.RefID.Name = "RefID";
            this.RefID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.RefID.Visible = false;
            // 
            // IsResample
            // 
            this.IsResample.FillWeight = 25F;
            this.IsResample.HeaderText = "Resample";
            this.IsResample.Name = "IsResample";
            // 
            // ResampleRound
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ResampleRound.DefaultCellStyle = dataGridViewCellStyle5;
            this.ResampleRound.FillWeight = 33F;
            this.ResampleRound.HeaderText = "Resample Round";
            this.ResampleRound.Name = "ResampleRound";
            this.ResampleRound.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // CustomerTypeId
            // 
            this.CustomerTypeId.FillWeight = 50F;
            this.CustomerTypeId.HeaderText = "Customer Type";
            this.CustomerTypeId.Name = "CustomerTypeId";
            // 
            // Edit
            // 
            this.Edit.DataPropertyName = "Edit";
            dataGridViewCellStyle6.NullValue = "Edit";
            this.Edit.DefaultCellStyle = dataGridViewCellStyle6;
            this.Edit.FillWeight = 16F;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Text = "";
            // 
            // Delete
            // 
            this.Delete.DataPropertyName = "Delete";
            dataGridViewCellStyle7.NullValue = "Delete";
            this.Delete.DefaultCellStyle = dataGridViewCellStyle7;
            this.Delete.FillWeight = 16F;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Delete.Text = "";
            // 
            // QAIAQReferenceFirst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(900, 600);
            this.ClientSize = new System.Drawing.Size(2043, 754);
            this.Controls.Add(this.dgTableMaintenance);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QAIAQReferenceFirst";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Table Maintenance";
            this.Load += new System.EventHandler(this.QAIAQReferenceFirst_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QAIAQReferenceFirst_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(Locationds)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTable)).EndInit();
            this.bindingNavigatorTable.ResumeLayout(false);
            this.bindingNavigatorTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTableMaintenance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgTableMaintenance;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.BindingNavigator bindingNavigatorTable;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.BindingSource bindingSourceTable;
        private System.Windows.Forms.DataGridViewComboBoxColumn AQLID;
        private System.Windows.Forms.DataGridViewComboBoxColumn QCTypeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn WTSamplingSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn VTSamplingSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefectMinValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefectMaxValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn RefID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsResample;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResampleRound;
        private System.Windows.Forms.DataGridViewComboBoxColumn CustomerTypeId;
        private System.Windows.Forms.DataGridViewLinkColumn Edit;
        private System.Windows.Forms.DataGridViewLinkColumn Delete;
    }
}