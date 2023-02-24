namespace Hartalega.FloorSystem.Windows.UI.WorkOrder
{
    partial class WorkOrderStatusView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrderStatusView));
            this.dgvLineSelection = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FGCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GloveType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PackingSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HartalegaSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCartons = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPcs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerLotNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_HSB_CustPORecvDate = new System.Windows.Forms.TextBox();
            this.txt_HSB_CustPODocumentDate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbWorkOrderType = new System.Windows.Forms.TextBox();
            this.lblWorkOrderType = new System.Windows.Forms.Label();
            this.txtDeliveryCountry = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCustomerRef = new System.Windows.Forms.TextBox();
            this.txtETA = new System.Windows.Forms.TextBox();
            this.txtETD = new System.Windows.Forms.TextBox();
            this.txtCustomerLotNo = new System.Windows.Forms.TextBox();
            this.txtManufacturingDate = new System.Windows.Forms.TextBox();
            this.txtContainerSize = new System.Windows.Forms.TextBox();
            this.txtOrderDate = new System.Windows.Forms.TextBox();
            this.txtVesselNames = new System.Windows.Forms.TextBox();
            this.txtSpecialInstruction = new System.Windows.Forms.TextBox();
            this.txtShippingAgent = new System.Windows.Forms.TextBox();
            this.txtLastConfirmDate = new System.Windows.Forms.TextBox();
            this.txtCustomerPO = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtOrderNo = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOperatorId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineSelection)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.Index,
            this.BrandName,
            this.FGCode,
            this.GloveType,
            this.PackingSize,
            this.HartalegaSize,
            this.CustomerSize,
            this.TotalCartons,
            this.TotalPcs,
            this.CustomerLotNumber});
            this.dgvLineSelection.Location = new System.Drawing.Point(34, 826);
            this.dgvLineSelection.Margin = new System.Windows.Forms.Padding(10);
            this.dgvLineSelection.MultiSelect = false;
            this.dgvLineSelection.Name = "dgvLineSelection";
            this.dgvLineSelection.ReadOnly = true;
            this.dgvLineSelection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLineSelection.Size = new System.Drawing.Size(2095, 519);
            this.dgvLineSelection.TabIndex = 87;
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Id";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Index.DefaultCellStyle = dataGridViewCellStyle3;
            this.Index.FillWeight = 35F;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Width = 40;
            // 
            // BrandName
            // 
            this.BrandName.HeaderText = "Brand Name";
            this.BrandName.Name = "BrandName";
            this.BrandName.ReadOnly = true;
            this.BrandName.Width = 150;
            // 
            // FGCode
            // 
            this.FGCode.HeaderText = "FG Code";
            this.FGCode.Name = "FGCode";
            this.FGCode.ReadOnly = true;
            this.FGCode.Width = 80;
            // 
            // GloveType
            // 
            this.GloveType.HeaderText = "Glove Type";
            this.GloveType.Name = "GloveType";
            this.GloveType.ReadOnly = true;
            this.GloveType.Width = 80;
            // 
            // PackingSize
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.PackingSize.DefaultCellStyle = dataGridViewCellStyle4;
            this.PackingSize.HeaderText = "Packing Size";
            this.PackingSize.Name = "PackingSize";
            this.PackingSize.ReadOnly = true;
            this.PackingSize.Width = 70;
            // 
            // HartalegaSize
            // 
            this.HartalegaSize.DataPropertyName = "LineId";
            this.HartalegaSize.FillWeight = 90F;
            this.HartalegaSize.HeaderText = "Hartalega Size";
            this.HartalegaSize.Name = "HartalegaSize";
            this.HartalegaSize.ReadOnly = true;
            this.HartalegaSize.Width = 50;
            // 
            // CustomerSize
            // 
            this.CustomerSize.DataPropertyName = "LineStartDateTime";
            this.CustomerSize.FillWeight = 0.9222575F;
            this.CustomerSize.HeaderText = "Customer Size";
            this.CustomerSize.Name = "CustomerSize";
            this.CustomerSize.ReadOnly = true;
            this.CustomerSize.Width = 90;
            // 
            // TotalCartons
            // 
            this.TotalCartons.DataPropertyName = "TierSide";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TotalCartons.DefaultCellStyle = dataGridViewCellStyle5;
            this.TotalCartons.HeaderText = "Total Cartons";
            this.TotalCartons.Name = "TotalCartons";
            this.TotalCartons.ReadOnly = true;
            // 
            // TotalPcs
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TotalPcs.DefaultCellStyle = dataGridViewCellStyle6;
            this.TotalPcs.HeaderText = "Total Pcs";
            this.TotalPcs.Name = "TotalPcs";
            this.TotalPcs.ReadOnly = true;
            this.TotalPcs.Width = 150;
            // 
            // CustomerLotNumber
            // 
            this.CustomerLotNumber.HeaderText = "Customer Lot Number";
            this.CustomerLotNumber.Name = "CustomerLotNumber";
            this.CustomerLotNumber.ReadOnly = true;
            this.CustomerLotNumber.Width = 90;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_HSB_CustPORecvDate);
            this.groupBox1.Controls.Add(this.txt_HSB_CustPODocumentDate);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbWorkOrderType);
            this.groupBox1.Controls.Add(this.lblWorkOrderType);
            this.groupBox1.Controls.Add(this.txtDeliveryCountry);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtCustomerRef);
            this.groupBox1.Controls.Add(this.txtETA);
            this.groupBox1.Controls.Add(this.txtETD);
            this.groupBox1.Controls.Add(this.txtCustomerLotNo);
            this.groupBox1.Controls.Add(this.txtManufacturingDate);
            this.groupBox1.Controls.Add(this.txtContainerSize);
            this.groupBox1.Controls.Add(this.txtOrderDate);
            this.groupBox1.Controls.Add(this.txtVesselNames);
            this.groupBox1.Controls.Add(this.txtSpecialInstruction);
            this.groupBox1.Controls.Add(this.txtShippingAgent);
            this.groupBox1.Controls.Add(this.txtLastConfirmDate);
            this.groupBox1.Controls.Add(this.txtCustomerPO);
            this.groupBox1.Controls.Add(this.txtCustomer);
            this.groupBox1.Controls.Add(this.txtOrderNo);
            this.groupBox1.Controls.Add(this.txtStatus);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblOperatorId);
            this.groupBox1.Location = new System.Drawing.Point(34, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(2095, 806);
            this.groupBox1.TabIndex = 88;
            this.groupBox1.TabStop = false;
            // 
            // txt_HSB_CustPORecvDate
            // 
            this.txt_HSB_CustPORecvDate.Location = new System.Drawing.Point(432, 498);
            this.txt_HSB_CustPORecvDate.Margin = new System.Windows.Forms.Padding(6);
            this.txt_HSB_CustPORecvDate.Name = "txt_HSB_CustPORecvDate";
            this.txt_HSB_CustPORecvDate.ReadOnly = true;
            this.txt_HSB_CustPORecvDate.Size = new System.Drawing.Size(582, 31);
            this.txt_HSB_CustPORecvDate.TabIndex = 137;
            // 
            // txt_HSB_CustPODocumentDate
            // 
            this.txt_HSB_CustPODocumentDate.Location = new System.Drawing.Point(432, 449);
            this.txt_HSB_CustPODocumentDate.Margin = new System.Windows.Forms.Padding(6);
            this.txt_HSB_CustPODocumentDate.Name = "txt_HSB_CustPODocumentDate";
            this.txt_HSB_CustPODocumentDate.ReadOnly = true;
            this.txt_HSB_CustPODocumentDate.Size = new System.Drawing.Size(582, 31);
            this.txt_HSB_CustPODocumentDate.TabIndex = 136;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(12, 500);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(341, 26);
            this.label11.TabIndex = 135;
            this.label11.Text = "Cust. Purch. Order Rcv\'d Date:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(12, 451);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(390, 26);
            this.label10.TabIndex = 134;
            this.label10.Text = "Cust. Purch. Order Document Date:";
            // 
            // tbWorkOrderType
            // 
            this.tbWorkOrderType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbWorkOrderType.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbWorkOrderType.Location = new System.Drawing.Point(432, 129);
            this.tbWorkOrderType.Margin = new System.Windows.Forms.Padding(6);
            this.tbWorkOrderType.MaxLength = 80;
            this.tbWorkOrderType.Name = "tbWorkOrderType";
            this.tbWorkOrderType.ReadOnly = true;
            this.tbWorkOrderType.Size = new System.Drawing.Size(582, 31);
            this.tbWorkOrderType.TabIndex = 133;
            // 
            // lblWorkOrderType
            // 
            this.lblWorkOrderType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblWorkOrderType.AutoSize = true;
            this.lblWorkOrderType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkOrderType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWorkOrderType.Location = new System.Drawing.Point(12, 134);
            this.lblWorkOrderType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblWorkOrderType.Name = "lblWorkOrderType";
            this.lblWorkOrderType.Size = new System.Drawing.Size(199, 26);
            this.lblWorkOrderType.TabIndex = 132;
            this.lblWorkOrderType.Text = "Work Order Type:";
            // 
            // txtDeliveryCountry
            // 
            this.txtDeliveryCountry.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDeliveryCountry.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDeliveryCountry.Location = new System.Drawing.Point(432, 704);
            this.txtDeliveryCountry.Margin = new System.Windows.Forms.Padding(6);
            this.txtDeliveryCountry.MaxLength = 50;
            this.txtDeliveryCountry.Name = "txtDeliveryCountry";
            this.txtDeliveryCountry.ReadOnly = true;
            this.txtDeliveryCountry.Size = new System.Drawing.Size(582, 31);
            this.txtDeliveryCountry.TabIndex = 131;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(12, 706);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(196, 26);
            this.label9.TabIndex = 130;
            this.label9.Text = "Delivery Country:";
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label21.AutoSize = true;
            this.label21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label21.Location = new System.Drawing.Point(709, 758);
            this.label21.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 25);
            this.label21.TabIndex = 126;
            this.label21.Text = "ETA:";
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(709, 81);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(63, 25);
            this.label17.TabIndex = 124;
            this.label17.Text = "Date:";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(1043, 392);
            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(173, 26);
            this.label16.TabIndex = 123;
            this.label16.Text = "Vessel Names:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(1043, 30);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(216, 26);
            this.label8.TabIndex = 122;
            this.label8.Text = "Special Instruction:";
            // 
            // txtCustomerRef
            // 
            this.txtCustomerRef.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomerRef.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerRef.Location = new System.Drawing.Point(432, 292);
            this.txtCustomerRef.Margin = new System.Windows.Forms.Padding(6);
            this.txtCustomerRef.MaxLength = 80;
            this.txtCustomerRef.Name = "txtCustomerRef";
            this.txtCustomerRef.ReadOnly = true;
            this.txtCustomerRef.Size = new System.Drawing.Size(582, 31);
            this.txtCustomerRef.TabIndex = 105;
            // 
            // txtETA
            // 
            this.txtETA.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtETA.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtETA.Location = new System.Drawing.Point(780, 756);
            this.txtETA.Margin = new System.Windows.Forms.Padding(6);
            this.txtETA.MaxLength = 30;
            this.txtETA.Name = "txtETA";
            this.txtETA.ReadOnly = true;
            this.txtETA.Size = new System.Drawing.Size(234, 31);
            this.txtETA.TabIndex = 112;
            // 
            // txtETD
            // 
            this.txtETD.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtETD.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtETD.Location = new System.Drawing.Point(432, 756);
            this.txtETD.Margin = new System.Windows.Forms.Padding(6);
            this.txtETD.MaxLength = 30;
            this.txtETD.Name = "txtETD";
            this.txtETD.ReadOnly = true;
            this.txtETD.Size = new System.Drawing.Size(234, 31);
            this.txtETD.TabIndex = 111;
            // 
            // txtCustomerLotNo
            // 
            this.txtCustomerLotNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomerLotNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerLotNo.Location = new System.Drawing.Point(432, 546);
            this.txtCustomerLotNo.Margin = new System.Windows.Forms.Padding(6);
            this.txtCustomerLotNo.MaxLength = 80;
            this.txtCustomerLotNo.Name = "txtCustomerLotNo";
            this.txtCustomerLotNo.ReadOnly = true;
            this.txtCustomerLotNo.Size = new System.Drawing.Size(582, 31);
            this.txtCustomerLotNo.TabIndex = 108;
            // 
            // txtManufacturingDate
            // 
            this.txtManufacturingDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtManufacturingDate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtManufacturingDate.Location = new System.Drawing.Point(432, 396);
            this.txtManufacturingDate.Margin = new System.Windows.Forms.Padding(6);
            this.txtManufacturingDate.MaxLength = 30;
            this.txtManufacturingDate.Name = "txtManufacturingDate";
            this.txtManufacturingDate.ReadOnly = true;
            this.txtManufacturingDate.Size = new System.Drawing.Size(582, 31);
            this.txtManufacturingDate.TabIndex = 107;
            // 
            // txtContainerSize
            // 
            this.txtContainerSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtContainerSize.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtContainerSize.Location = new System.Drawing.Point(432, 598);
            this.txtContainerSize.Margin = new System.Windows.Forms.Padding(6);
            this.txtContainerSize.MaxLength = 20;
            this.txtContainerSize.Name = "txtContainerSize";
            this.txtContainerSize.ReadOnly = true;
            this.txtContainerSize.Size = new System.Drawing.Size(582, 31);
            this.txtContainerSize.TabIndex = 109;
            // 
            // txtOrderDate
            // 
            this.txtOrderDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOrderDate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOrderDate.Location = new System.Drawing.Point(780, 78);
            this.txtOrderDate.Margin = new System.Windows.Forms.Padding(6);
            this.txtOrderDate.MaxLength = 30;
            this.txtOrderDate.Name = "txtOrderDate";
            this.txtOrderDate.ReadOnly = true;
            this.txtOrderDate.Size = new System.Drawing.Size(234, 31);
            this.txtOrderDate.TabIndex = 102;
            // 
            // txtVesselNames
            // 
            this.txtVesselNames.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtVesselNames.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVesselNames.Location = new System.Drawing.Point(1048, 424);
            this.txtVesselNames.Margin = new System.Windows.Forms.Padding(6);
            this.txtVesselNames.Multiline = true;
            this.txtVesselNames.Name = "txtVesselNames";
            this.txtVesselNames.ReadOnly = true;
            this.txtVesselNames.Size = new System.Drawing.Size(990, 217);
            this.txtVesselNames.TabIndex = 114;
            // 
            // txtSpecialInstruction
            // 
            this.txtSpecialInstruction.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSpecialInstruction.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSpecialInstruction.Location = new System.Drawing.Point(1048, 65);
            this.txtSpecialInstruction.Margin = new System.Windows.Forms.Padding(6);
            this.txtSpecialInstruction.Multiline = true;
            this.txtSpecialInstruction.Name = "txtSpecialInstruction";
            this.txtSpecialInstruction.ReadOnly = true;
            this.txtSpecialInstruction.Size = new System.Drawing.Size(990, 310);
            this.txtSpecialInstruction.TabIndex = 113;
            // 
            // txtShippingAgent
            // 
            this.txtShippingAgent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtShippingAgent.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtShippingAgent.Location = new System.Drawing.Point(432, 650);
            this.txtShippingAgent.Margin = new System.Windows.Forms.Padding(6);
            this.txtShippingAgent.MaxLength = 50;
            this.txtShippingAgent.Name = "txtShippingAgent";
            this.txtShippingAgent.ReadOnly = true;
            this.txtShippingAgent.Size = new System.Drawing.Size(582, 31);
            this.txtShippingAgent.TabIndex = 110;
            // 
            // txtLastConfirmDate
            // 
            this.txtLastConfirmDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLastConfirmDate.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLastConfirmDate.Location = new System.Drawing.Point(432, 344);
            this.txtLastConfirmDate.Margin = new System.Windows.Forms.Padding(6);
            this.txtLastConfirmDate.MaxLength = 30;
            this.txtLastConfirmDate.Name = "txtLastConfirmDate";
            this.txtLastConfirmDate.ReadOnly = true;
            this.txtLastConfirmDate.Size = new System.Drawing.Size(582, 31);
            this.txtLastConfirmDate.TabIndex = 106;
            // 
            // txtCustomerPO
            // 
            this.txtCustomerPO.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomerPO.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerPO.Location = new System.Drawing.Point(432, 240);
            this.txtCustomerPO.Margin = new System.Windows.Forms.Padding(6);
            this.txtCustomerPO.MaxLength = 20;
            this.txtCustomerPO.Name = "txtCustomerPO";
            this.txtCustomerPO.ReadOnly = true;
            this.txtCustomerPO.Size = new System.Drawing.Size(582, 31);
            this.txtCustomerPO.TabIndex = 104;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomer.Location = new System.Drawing.Point(432, 187);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(6);
            this.txtCustomer.MaxLength = 80;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(582, 31);
            this.txtCustomer.TabIndex = 103;
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOrderNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOrderNo.Location = new System.Drawing.Point(432, 75);
            this.txtOrderNo.Margin = new System.Windows.Forms.Padding(6);
            this.txtOrderNo.MaxLength = 20;
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.ReadOnly = true;
            this.txtOrderNo.Size = new System.Drawing.Size(234, 31);
            this.txtOrderNo.TabIndex = 101;
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtStatus.Location = new System.Drawing.Point(432, 25);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(6);
            this.txtStatus.MaxLength = 10;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(582, 31);
            this.txtStatus.TabIndex = 100;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(12, 301);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 26);
            this.label7.TabIndex = 129;
            this.label7.Text = "Customer Ref:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(12, 546);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(199, 26);
            this.label5.TabIndex = 128;
            this.label5.Text = "Customer Lot No:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(12, 399);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 26);
            this.label1.TabIndex = 127;
            this.label1.Text = "Manufacturing Date:";
            // 
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(12, 604);
            this.label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(176, 26);
            this.label18.TabIndex = 125;
            this.label18.Text = "Container Size:";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(12, 757);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 26);
            this.label13.TabIndex = 121;
            this.label13.Text = "ETD:";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(12, 656);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(181, 26);
            this.label14.TabIndex = 120;
            this.label14.Text = "Shipping Agent:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(12, 347);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(212, 26);
            this.label6.TabIndex = 119;
            this.label6.Text = "Last Confirm Date:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 249);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 26);
            this.label4.TabIndex = 118;
            this.label4.Text = "Customer PO:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 195);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 26);
            this.label3.TabIndex = 117;
            this.label3.Text = "Customer:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 26);
            this.label2.TabIndex = 116;
            this.label2.Text = "Order:";
            // 
            // lblOperatorId
            // 
            this.lblOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorId.AutoSize = true;
            this.lblOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperatorId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblOperatorId.Location = new System.Drawing.Point(12, 30);
            this.lblOperatorId.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOperatorId.Name = "lblOperatorId";
            this.lblOperatorId.Size = new System.Drawing.Size(87, 26);
            this.lblOperatorId.TabIndex = 115;
            this.lblOperatorId.Text = "Status:";
            // 
            // WorkOrderStatusView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2666, 1416);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvLineSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(2558, 1165);
            this.Name = "WorkOrderStatusView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Work Order Status View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvLineSelection)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvLineSelection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FGCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GloveType;
        private System.Windows.Forms.DataGridViewTextBoxColumn PackingSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn HartalegaSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCartons;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPcs;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerLotNumber;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCustomerRef;
        private System.Windows.Forms.TextBox txtETA;
        private System.Windows.Forms.TextBox txtETD;
        private System.Windows.Forms.TextBox txtCustomerLotNo;
        private System.Windows.Forms.TextBox txtManufacturingDate;
        private System.Windows.Forms.TextBox txtContainerSize;
        private System.Windows.Forms.TextBox txtOrderDate;
        private System.Windows.Forms.TextBox txtVesselNames;
        private System.Windows.Forms.TextBox txtSpecialInstruction;
        private System.Windows.Forms.TextBox txtShippingAgent;
        private System.Windows.Forms.TextBox txtLastConfirmDate;
        private System.Windows.Forms.TextBox txtCustomerPO;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtOrderNo;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOperatorId;
        private System.Windows.Forms.TextBox txtDeliveryCountry;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbWorkOrderType;
        private System.Windows.Forms.Label lblWorkOrderType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_HSB_CustPORecvDate;
        private System.Windows.Forms.TextBox txt_HSB_CustPODocumentDate;
        private System.Windows.Forms.Label label11;
    }
}