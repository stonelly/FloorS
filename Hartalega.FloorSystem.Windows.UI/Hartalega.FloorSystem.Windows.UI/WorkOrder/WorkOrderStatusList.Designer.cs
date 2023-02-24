namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
{
    partial class WorkOrderStatusList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrderStatusList));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCustomerRef = new System.Windows.Forms.TextBox();
            this.cmbStatus = new CheckComboBoxTest.CheckedComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCustomerPO = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.lblOperatorId = new System.Windows.Forms.Label();
            this.txtOrderNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtOrderDateStart = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtETDEnd = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtETDStart = new System.Windows.Forms.DateTimePicker();
            this.dtOrderDateEnd = new System.Windows.Forms.DateTimePicker();
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
            this.dgvLineSelection = new System.Windows.Forms.DataGridView();
            this.bindingSourceTable = new System.Windows.Forms.BindingSource(this.components);
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerPO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShippingAgent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ETD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ETA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VesselName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTable)).BeginInit();
            this.bindingNavigatorTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCustomerRef);
            this.groupBox1.Controls.Add(this.cmbStatus);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtCustomerPO);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.lblOperatorId);
            this.groupBox1.Controls.Add(this.txtOrderNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtOrderDateStart);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtETDEnd);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtETDStart);
            this.groupBox1.Controls.Add(this.dtOrderDateEnd);
            this.groupBox1.Controls.Add(this.bindingNavigatorTable);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(982, 180);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(6, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Customer Ref:";
            // 
            // txtCustomerRef
            // 
            this.txtCustomerRef.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomerRef.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerRef.Location = new System.Drawing.Point(121, 121);
            this.txtCustomerRef.MaxLength = 80;
            this.txtCustomerRef.Name = "txtCustomerRef";
            this.txtCustomerRef.Size = new System.Drawing.Size(323, 20);
            this.txtCustomerRef.TabIndex = 26;
            // 
            // cmbStatus
            // 
            this.cmbStatus.CheckOnClick = true;
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbStatus.DropDownHeight = 1;
            this.cmbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.IntegralHeight = false;
            this.cmbStatus.Location = new System.Drawing.Point(121, 28);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(323, 21);
            this.cmbStatus.TabIndex = 1;
            this.cmbStatus.ValueSeparator = ", ";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(867, 118);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(776, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 18);
            this.label8.TabIndex = 21;
            this.label8.Text = "-";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(491, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Customer PO:";
            // 
            // txtCustomerPO
            // 
            this.txtCustomerPO.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomerPO.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerPO.Location = new System.Drawing.Point(613, 29);
            this.txtCustomerPO.MaxLength = 20;
            this.txtCustomerPO.Name = "txtCustomerPO";
            this.txtCustomerPO.Size = new System.Drawing.Size(329, 20);
            this.txtCustomerPO.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(6, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Order No:";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(121, 89);
            this.txtCustomer.MaxLength = 80;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(323, 20);
            this.txtCustomer.TabIndex = 3;
            // 
            // lblOperatorId
            // 
            this.lblOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorId.AutoSize = true;
            this.lblOperatorId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblOperatorId.Location = new System.Drawing.Point(6, 92);
            this.lblOperatorId.Name = "lblOperatorId";
            this.lblOperatorId.Size = new System.Drawing.Size(63, 13);
            this.lblOperatorId.TabIndex = 5;
            this.lblOperatorId.Text = "Customer:";
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOrderNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOrderNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOrderNo.Location = new System.Drawing.Point(121, 60);
            this.txtOrderNo.MaxLength = 20;
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.Size = new System.Drawing.Size(323, 20);
            this.txtOrderNo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(491, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Order Date:";
            // 
            // dtOrderDateStart
            // 
            this.dtOrderDateStart.CustomFormat = "dd/MM/yyyy";
            this.dtOrderDateStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtOrderDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtOrderDateStart.Location = new System.Drawing.Point(613, 60);
            this.dtOrderDateStart.Name = "dtOrderDateStart";
            this.dtOrderDateStart.Size = new System.Drawing.Size(157, 20);
            this.dtOrderDateStart.TabIndex = 5;
            this.dtOrderDateStart.ValueChanged += new System.EventHandler(this.dtOrderDateStart_Changed);
            this.dtOrderDateStart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtOrderDateStart_KeyDown);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(6, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Status:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(776, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 18);
            this.label3.TabIndex = 14;
            this.label3.Text = "-";
            // 
            // dtETDEnd
            // 
            this.dtETDEnd.CustomFormat = "dd/MM/yyyy";
            this.dtETDEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtETDEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtETDEnd.Location = new System.Drawing.Point(796, 89);
            this.dtETDEnd.Name = "dtETDEnd";
            this.dtETDEnd.Size = new System.Drawing.Size(146, 20);
            this.dtETDEnd.TabIndex = 8;
            this.dtETDEnd.ValueChanged += new System.EventHandler(this.dtETDEnd_Changed);
            this.dtETDEnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtETDEnd_KeyDown);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(491, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "ETD:";
            // 
            // dtETDStart
            // 
            this.dtETDStart.CustomFormat = "dd/MM/yyyy";
            this.dtETDStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtETDStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtETDStart.Location = new System.Drawing.Point(613, 89);
            this.dtETDStart.Name = "dtETDStart";
            this.dtETDStart.Size = new System.Drawing.Size(157, 20);
            this.dtETDStart.TabIndex = 7;
            this.dtETDStart.ValueChanged += new System.EventHandler(this.dtETDStart_Changed);
            this.dtETDStart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtETDStart_KeyDown);
            // 
            // dtOrderDateEnd
            // 
            this.dtOrderDateEnd.CustomFormat = "dd/MM/yyyy";
            this.dtOrderDateEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtOrderDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtOrderDateEnd.Location = new System.Drawing.Point(796, 60);
            this.dtOrderDateEnd.Name = "dtOrderDateEnd";
            this.dtOrderDateEnd.Size = new System.Drawing.Size(146, 20);
            this.dtOrderDateEnd.TabIndex = 6;
            this.dtOrderDateEnd.ValueChanged += new System.EventHandler(this.dtOrderDateEnd_Changed);
            this.dtOrderDateEnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtOrderDateEnd_KeyDown);
            // 
            // bindingNavigatorTable
            // 
            this.bindingNavigatorTable.AddNewItem = null;
            this.bindingNavigatorTable.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigatorTable.DeleteItem = null;
            this.bindingNavigatorTable.Dock = System.Windows.Forms.DockStyle.Bottom;
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
            this.bindingNavigatorTable.Location = new System.Drawing.Point(3, 152);
            this.bindingNavigatorTable.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorTable.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorTable.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorTable.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorTable.Name = "bindingNavigatorTable";
            this.bindingNavigatorTable.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorTable.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.bindingNavigatorTable.Size = new System.Drawing.Size(976, 25);
            this.bindingNavigatorTable.TabIndex = 25;
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
            // dgvLineSelection
            // 
            this.dgvLineSelection.AllowUserToAddRows = false;
            this.dgvLineSelection.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvLineSelection.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLineSelection.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLineSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLineSelection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
            this.OrderDate,
            this.OrderNo,
            this.OrderType,
            this.CustomerRef,
            this.CustomerPO,
            this.Customer,
            this.ShippingAgent,
            this.ETD,
            this.ETA,
            this.VesselName});
            this.dgvLineSelection.Location = new System.Drawing.Point(1, 198);
            this.dgvLineSelection.Margin = new System.Windows.Forms.Padding(5);
            this.dgvLineSelection.MultiSelect = false;
            this.dgvLineSelection.Name = "dgvLineSelection";
            this.dgvLineSelection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLineSelection.Size = new System.Drawing.Size(1000, 500);
            this.dgvLineSelection.TabIndex = 23;
            this.dgvLineSelection.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLineSelection_CellDoubleClick);
            this.dgvLineSelection.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvLineSelection_ColumnHeaderMouseClick);
            this.dgvLineSelection.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvLineSelection_KeyDown);
            // 
            // Status
            // 
            this.Status.DataPropertyName = "LineStartDateTime";
            this.Status.FillWeight = 0.9222575F;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Status.Width = 90;
            // 
            // OrderDate
            // 
            this.OrderDate.HeaderText = "Order Date";
            this.OrderDate.Name = "OrderDate";
            this.OrderDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // OrderNo
            // 
            this.OrderNo.HeaderText = "Order No";
            this.OrderNo.Name = "OrderNo";
            this.OrderNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // OrderType
            // 
            this.OrderType.HeaderText = "Order Type";
            this.OrderType.Name = "OrderType";
            this.OrderType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.OrderType.Width = 150;
            // 
            // CustomerRef
            // 
            this.CustomerRef.HeaderText = "Customer Ref";
            this.CustomerRef.Name = "CustomerRef";
            this.CustomerRef.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // CustomerPO
            // 
            this.CustomerPO.DataPropertyName = "TierSide";
            this.CustomerPO.HeaderText = "Customer PO";
            this.CustomerPO.Name = "CustomerPO";
            this.CustomerPO.ReadOnly = true;
            this.CustomerPO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.CustomerPO.Width = 150;
            // 
            // Customer
            // 
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Customer.Width = 200;
            // 
            // ShippingAgent
            // 
            this.ShippingAgent.HeaderText = "Shipping Agent";
            this.ShippingAgent.Name = "ShippingAgent";
            this.ShippingAgent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ShippingAgent.Width = 90;
            // 
            // ETD
            // 
            dataGridViewCellStyle3.NullValue = null;
            this.ETD.DefaultCellStyle = dataGridViewCellStyle3;
            this.ETD.HeaderText = "ETD";
            this.ETD.Name = "ETD";
            this.ETD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ETD.Width = 90;
            // 
            // ETA
            // 
            this.ETA.HeaderText = "ETA";
            this.ETA.Name = "ETA";
            this.ETA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ETA.Width = 90;
            // 
            // VesselName
            // 
            this.VesselName.HeaderText = "Vessel Names";
            this.VesselName.Name = "VesselName";
            this.VesselName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.VesselName.Width = 150;
            // 
            // WorkOrderStatusList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 587);
            this.Controls.Add(this.dgvLineSelection);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1090, 572);
            this.Name = "WorkOrderStatusList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Work Order Status List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.WorkOrderMaintainanceList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTable)).EndInit();
            this.bindingNavigatorTable.ResumeLayout(false);
            this.bindingNavigatorTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCustomerPO;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label lblOperatorId;
        private System.Windows.Forms.TextBox txtOrderNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtOrderDateStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtOrderDateEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtETDEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtETDStart;
        private System.Windows.Forms.DataGridView dgvLineSelection;
        private System.Windows.Forms.Button btnSearch;
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
        private CheckComboBoxTest.CheckedComboBox cmbStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCustomerRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerPO;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShippingAgent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ETD;
        private System.Windows.Forms.DataGridViewTextBoxColumn ETA;
        private System.Windows.Forms.DataGridViewTextBoxColumn VesselName;
    }
}