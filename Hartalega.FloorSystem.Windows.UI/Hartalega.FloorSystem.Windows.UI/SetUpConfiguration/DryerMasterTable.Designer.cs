namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    partial class DryerMasterTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DryerMasterTable));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.DryerNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbLocationId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmbGloveType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmbSize = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Hot = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cold = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HotAndCold = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CheckGlove = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CheckSize = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsStopped = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsScheduledStop = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LocationId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GloveType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Delete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.DryerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTable)).BeginInit();
            this.bindingNavigatorTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTableMaintenance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 181F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 567F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bindingNavigatorTable, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.42857F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(984, 79);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 38);
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
            this.bindingNavigatorTable.Location = new System.Drawing.Point(0, 47);
            this.bindingNavigatorTable.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorTable.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorTable.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorTable.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorTable.Name = "bindingNavigatorTable";
            this.bindingNavigatorTable.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorTable.Size = new System.Drawing.Size(236, 25);
            this.bindingNavigatorTable.TabIndex = 1;
            this.bindingNavigatorTable.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // dgTableMaintenance
            // 
            this.dgTableMaintenance.AllowUserToAddRows = false;
            this.dgTableMaintenance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgTableMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTableMaintenance.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DryerNumber,
            this.cmbLocationId,
            this.cmbGloveType,
            this.cmbSize,
            this.Hot,
            this.Cold,
            this.HotAndCold,
            this.CheckGlove,
            this.CheckSize,
            this.IsStopped,
            this.IsScheduledStop,
            this.LocationId,
            this.GloveType,
            this.Size,
            this.Edit,
            this.Delete,
            this.DryerId});
            this.dgTableMaintenance.Location = new System.Drawing.Point(12, 97);
            this.dgTableMaintenance.Name = "dgTableMaintenance";
            this.dgTableMaintenance.Size = new System.Drawing.Size(1023, 537);
            this.dgTableMaintenance.TabIndex = 1;
            this.dgTableMaintenance.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTableMaintenance_CellClick);
            this.dgTableMaintenance.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTableMaintenance_CellContentClick);
            this.dgTableMaintenance.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTableMaintenance_CellValueChanged);
            this.dgTableMaintenance.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgTableMaintenance_CurrentCellDirtyStateChanged);
            this.dgTableMaintenance.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgTableMaintenance_EditingControlShowing);
            // 
            // bindingSourceTable
            // 
            this.bindingSourceTable.CurrentChanged += new System.EventHandler(this.bindingSource_CurrentChanged);
            // 
            // DryerNumber
            // 
            this.DryerNumber.HeaderText = "Dryer Number";
            this.DryerNumber.MaxInputLength = 9;
            this.DryerNumber.Name = "DryerNumber";
            this.DryerNumber.ReadOnly = true;
            this.DryerNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DryerNumber.Width = 107;
            // 
            // cmbLocationId
            // 
            this.cmbLocationId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocationId.HeaderText = "Location";
            this.cmbLocationId.Name = "cmbLocationId";
            this.cmbLocationId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmbLocationId.Width = 79;
            // 
            // cmbGloveType
            // 
            this.cmbGloveType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbGloveType.HeaderText = "Glove Type";
            this.cmbGloveType.Name = "cmbGloveType";
            this.cmbGloveType.Width = 89;
            // 
            // cmbSize
            // 
            this.cmbSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSize.HeaderText = "Size";
            this.cmbSize.Name = "cmbSize";
            this.cmbSize.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmbSize.Width = 47;
            // 
            // Hot
            // 
            this.Hot.HeaderText = "Hot";
            this.Hot.Name = "Hot";
            this.Hot.Width = 41;
            // 
            // Cold
            // 
            this.Cold.HeaderText = "Cold";
            this.Cold.Name = "Cold";
            this.Cold.Width = 49;
            // 
            // HotAndCold
            // 
            this.HotAndCold.HeaderText = "Hot And Cold";
            this.HotAndCold.Name = "HotAndCold";
            this.HotAndCold.Width = 103;
            // 
            // CheckGlove
            // 
            this.CheckGlove.HeaderText = "Check Glove";
            this.CheckGlove.Name = "CheckGlove";
            // 
            // CheckSize
            // 
            this.CheckSize.HeaderText = "Check Size";
            this.CheckSize.Name = "CheckSize";
            this.CheckSize.Width = 90;
            // 
            // IsStopped
            // 
            this.IsStopped.HeaderText = "Is Stopped";
            this.IsStopped.Name = "IsStopped";
            this.IsStopped.Width = 85;
            // 
            // IsScheduledStop
            // 
            this.IsScheduledStop.HeaderText = "Is Scheduled Stop";
            this.IsScheduledStop.Name = "IsScheduledStop";
            this.IsScheduledStop.Width = 135;
            // 
            // LocationId
            // 
            this.LocationId.HeaderText = "LocationId";
            this.LocationId.Name = "LocationId";
            this.LocationId.Visible = false;
            this.LocationId.Width = 111;
            // 
            // GloveType
            // 
            this.GloveType.HeaderText = "GloveType";
            this.GloveType.Name = "GloveType";
            this.GloveType.Visible = false;
            this.GloveType.Width = 113;
            // 
            // Size
            // 
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.Visible = false;
            this.Size.Width = 66;
            // 
            // Edit
            // 
            dataGridViewCellStyle1.NullValue = "Edit";
            this.Edit.DefaultCellStyle = dataGridViewCellStyle1;
            this.Edit.HeaderText = "Edit";
            this.Edit.Name = "Edit";
            this.Edit.Width = 43;
            // 
            // Delete
            // 
            dataGridViewCellStyle2.NullValue = "Delete";
            this.Delete.DefaultCellStyle = dataGridViewCellStyle2;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.Width = 62;
            // 
            // DryerId
            // 
            this.DryerId.HeaderText = "DryerId";
            this.DryerId.Name = "DryerId";
            this.DryerId.Visible = false;
            this.DryerId.Width = 87;
            // 
            // DryerMasterTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(900, 600);
            this.ClientSize = new System.Drawing.Size(1024, 710);
            this.Controls.Add(this.dgTableMaintenance);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DryerMasterTable";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Table Maintenance";
            this.Load += new System.EventHandler(this.TableMaintenance_Load);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn DryerNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbLocationId;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbGloveType;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbSize;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Hot;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Cold;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HotAndCold;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckGlove;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckSize;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsStopped;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsScheduledStop;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationId;
        private System.Windows.Forms.DataGridViewTextBoxColumn GloveType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewLinkColumn Edit;
        private System.Windows.Forms.DataGridViewLinkColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn DryerId;
    }
}