namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    partial class ScanMultipleBatchMTS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanMultipleBatchMTS));
            this.cmbGroupId = new System.Windows.Forms.ComboBox();
            this.cmbPreShipPltId = new System.Windows.Forms.ComboBox();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtInnerPrinter = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtstation = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbPalletId = new System.Windows.Forms.ComboBox();
            this.cmbItemNumber = new System.Windows.Forms.ComboBox();
            this.cmbPurchaseOrder = new System.Windows.Forms.ComboBox();
            this.txtInternalLotNumber = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.gvScanMultiBatchInfo = new System.Windows.Forms.DataGridView();
            this.colSerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBoxesPacked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddMultiJob = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBoxesPacked = new System.Windows.Forms.TextBox();
            this.dateControl1 = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.lblGroupId = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbItemSize = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtTotalBoxesPacked = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvScanMultiBatchInfo)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbGroupId
            // 
            this.cmbGroupId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbGroupId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbGroupId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbGroupId.FormattingEnabled = true;
            this.cmbGroupId.IntegralHeight = false;
            this.cmbGroupId.Location = new System.Drawing.Point(903, 186);
            this.cmbGroupId.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGroupId.Name = "cmbGroupId";
            this.cmbGroupId.Size = new System.Drawing.Size(216, 44);
            this.cmbGroupId.TabIndex = 10;
            // 
            // cmbPreShipPltId
            // 
            this.cmbPreShipPltId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPreShipPltId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbPreShipPltId.FormattingEnabled = true;
            this.cmbPreShipPltId.IntegralHeight = false;
            this.cmbPreShipPltId.Location = new System.Drawing.Point(279, 628);
            this.cmbPreShipPltId.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.cmbPreShipPltId.Name = "cmbPreShipPltId";
            this.cmbPreShipPltId.Size = new System.Drawing.Size(366, 44);
            this.cmbPreShipPltId.TabIndex = 21;
            this.cmbPreShipPltId.Leave += new System.EventHandler(this.cmbPreShipPltId_Leave);
            // 
            // txtItemName
            // 
            this.txtItemName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.txtItemName, 2);
            this.txtItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtItemName.Location = new System.Drawing.Point(656, 127);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.ReadOnly = true;
            this.txtItemName.Size = new System.Drawing.Size(518, 41);
            this.txtItemName.TabIndex = 8;
            this.txtItemName.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(174, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(162, 49);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(4, 4);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(162, 49);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtInnerPrinter
            // 
            this.txtInnerPrinter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInnerPrinter.BackColor = System.Drawing.SystemColors.Menu;
            this.txtInnerPrinter.Enabled = false;
            this.txtInnerPrinter.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtInnerPrinter.Location = new System.Drawing.Point(1025, 4);
            this.txtInnerPrinter.Margin = new System.Windows.Forms.Padding(4);
            this.txtInnerPrinter.Name = "txtInnerPrinter";
            this.txtInnerPrinter.Size = new System.Drawing.Size(148, 41);
            this.txtInnerPrinter.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(28, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(1201, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Program Messages";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.1579F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.57895F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.68421F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.26316F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.31579F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.78947F));
            this.tableLayoutPanel2.Controls.Add(this.txtInnerPrinter, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label24, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLocation, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtstation, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label25, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label26, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 38);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1188, 50);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label24
            // 
            this.label24.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label24.Location = new System.Drawing.Point(884, 7);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(133, 36);
            this.label24.TabIndex = 4;
            this.label24.Text = "INKJET:";
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLocation.BackColor = System.Drawing.SystemColors.Menu;
            this.txtLocation.Enabled = false;
            this.txtLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtLocation.Location = new System.Drawing.Point(160, 4);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(148, 41);
            this.txtLocation.TabIndex = 1;
            // 
            // txtstation
            // 
            this.txtstation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtstation.BackColor = System.Drawing.SystemColors.Menu;
            this.txtstation.Enabled = false;
            this.txtstation.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtstation.Location = new System.Drawing.Point(507, 4);
            this.txtstation.Margin = new System.Windows.Forms.Padding(4);
            this.txtstation.Name = "txtstation";
            this.txtstation.Size = new System.Drawing.Size(363, 41);
            this.txtstation.TabIndex = 3;
            // 
            // label25
            // 
            this.label25.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label25.Location = new System.Drawing.Point(325, 7);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(174, 36);
            this.label25.TabIndex = 2;
            this.label25.Text = "Station No:";
            // 
            // label26
            // 
            this.label26.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label26.Location = new System.Drawing.Point(6, 7);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(146, 36);
            this.label26.TabIndex = 0;
            this.label26.Text = "Location:";
            // 
            // cmbPalletId
            // 
            this.cmbPalletId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPalletId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbPalletId.FormattingEnabled = true;
            this.cmbPalletId.IntegralHeight = false;
            this.cmbPalletId.Location = new System.Drawing.Point(279, 568);
            this.cmbPalletId.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.cmbPalletId.Name = "cmbPalletId";
            this.cmbPalletId.Size = new System.Drawing.Size(366, 44);
            this.cmbPalletId.TabIndex = 19;
            this.cmbPalletId.Leave += new System.EventHandler(this.cmbPalletId_Leave);
            // 
            // cmbItemNumber
            // 
            this.cmbItemNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbItemNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbItemNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbItemNumber.FormattingEnabled = true;
            this.cmbItemNumber.IntegralHeight = false;
            this.cmbItemNumber.Location = new System.Drawing.Point(279, 126);
            this.cmbItemNumber.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.cmbItemNumber.Name = "cmbItemNumber";
            this.cmbItemNumber.Size = new System.Drawing.Size(366, 44);
            this.cmbItemNumber.TabIndex = 6;
            // 
            // cmbPurchaseOrder
            // 
            this.cmbPurchaseOrder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.cmbPurchaseOrder, 3);
            this.cmbPurchaseOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPurchaseOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbPurchaseOrder.FormattingEnabled = true;
            this.cmbPurchaseOrder.IntegralHeight = false;
            this.cmbPurchaseOrder.Location = new System.Drawing.Point(279, 67);
            this.cmbPurchaseOrder.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.cmbPurchaseOrder.Name = "cmbPurchaseOrder";
            this.cmbPurchaseOrder.Size = new System.Drawing.Size(894, 44);
            this.cmbPurchaseOrder.TabIndex = 4;
            // 
            // txtInternalLotNumber
            // 
            this.txtInternalLotNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInternalLotNumber.BackColor = System.Drawing.SystemColors.Menu;
            this.txtInternalLotNumber.Enabled = false;
            this.txtInternalLotNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtInternalLotNumber.Location = new System.Drawing.Point(903, 9);
            this.txtInternalLotNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtInternalLotNumber.Name = "txtInternalLotNumber";
            this.txtInternalLotNumber.Size = new System.Drawing.Size(272, 41);
            this.txtInternalLotNumber.TabIndex = 2;
            this.txtInternalLotNumber.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(28, 112);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1201, 786);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Multiple Batch";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.47899F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.45798F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.79832F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.2647F));
            this.tableLayoutPanel1.Controls.Add(this.label10, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtSerialNumber, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbItemNumber, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtInternalLotNumber, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbPurchaseOrder, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtBoxesPacked, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.dateControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbGroupId, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblGroupId, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbItemSize, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtItemName, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.cmbPreShipPltId, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbPalletId, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtTotalBoxesPacked, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 38);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.851775F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.449423F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.850978F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.850978F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.850978F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.49939F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.850978F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.850978F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.944525F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1188, 740);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(734, 250);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(161, 36);
            this.label10.TabIndex = 13;
            this.label10.Text = "Inner Box:";
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtSerialNumber.Location = new System.Drawing.Point(279, 242);
            this.txtSerialNumber.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.txtSerialNumber.MaxLength = 10;
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(366, 41);
            this.txtSerialNumber.TabIndex = 12;
            this.txtSerialNumber.Leave += new System.EventHandler(this.txtSerialNumber_Leave);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(660, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(235, 36);
            this.label2.TabIndex = 1;
            this.label2.Text = "Internal Lot No:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(95, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pack Date:";
            // 
            // flowLayoutPanel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 4);
            this.flowLayoutPanel2.Controls.Add(this.gvScanMultiBatchInfo);
            this.flowLayoutPanel2.Controls.Add(this.btnAddMultiJob);
            this.flowLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 302);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1180, 186);
            this.flowLayoutPanel2.TabIndex = 15;
            // 
            // gvScanMultiBatchInfo
            // 
            this.gvScanMultiBatchInfo.AllowUserToAddRows = false;
            this.gvScanMultiBatchInfo.AllowUserToDeleteRows = false;
            this.gvScanMultiBatchInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvScanMultiBatchInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSerialNumber,
            this.colBatchNo,
            this.colBoxesPacked});
            this.gvScanMultiBatchInfo.Location = new System.Drawing.Point(188, 4);
            this.gvScanMultiBatchInfo.Margin = new System.Windows.Forms.Padding(188, 4, 4, 4);
            this.gvScanMultiBatchInfo.Name = "gvScanMultiBatchInfo";
            this.gvScanMultiBatchInfo.ReadOnly = true;
            this.gvScanMultiBatchInfo.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvScanMultiBatchInfo.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvScanMultiBatchInfo.Size = new System.Drawing.Size(816, 171);
            this.gvScanMultiBatchInfo.TabIndex = 0;
            // 
            // colSerialNumber
            // 
            this.colSerialNumber.DataPropertyName = "SerialNumber";
            this.colSerialNumber.HeaderText = "Serial Number";
            this.colSerialNumber.MinimumWidth = 6;
            this.colSerialNumber.Name = "colSerialNumber";
            this.colSerialNumber.ReadOnly = true;
            this.colSerialNumber.Width = 200;
            // 
            // colBatchNo
            // 
            this.colBatchNo.DataPropertyName = "BatchNumber";
            this.colBatchNo.HeaderText = "Batch Number";
            this.colBatchNo.MinimumWidth = 6;
            this.colBatchNo.Name = "colBatchNo";
            this.colBatchNo.ReadOnly = true;
            this.colBatchNo.Width = 200;
            // 
            // colBoxesPacked
            // 
            this.colBoxesPacked.DataPropertyName = "BoxesPacked";
            this.colBoxesPacked.HeaderText = "Inner Box";
            this.colBoxesPacked.MinimumWidth = 6;
            this.colBoxesPacked.Name = "colBoxesPacked";
            this.colBoxesPacked.ReadOnly = true;
            this.colBoxesPacked.Width = 210;
            // 
            // btnAddMultiJob
            // 
            this.btnAddMultiJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMultiJob.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnAddMultiJob.Location = new System.Drawing.Point(1012, 4);
            this.btnAddMultiJob.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddMultiJob.Name = "btnAddMultiJob";
            this.btnAddMultiJob.Size = new System.Drawing.Size(162, 49);
            this.btnAddMultiJob.TabIndex = 1;
            this.btnAddMultiJob.Text = "Add";
            this.btnAddMultiJob.UseVisualStyleBackColor = true;
            this.btnAddMultiJob.Click += new System.EventHandler(this.btnAddMultiJob_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(69, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 36);
            this.label3.TabIndex = 3;
            this.label3.Text = "Batch Order:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(130, 130);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 36);
            this.label5.TabIndex = 5;
            this.label5.Text = "Item No:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(107, 250);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 36);
            this.label7.TabIndex = 11;
            this.label7.Text = "Serial No:";
            // 
            // txtBoxesPacked
            // 
            this.txtBoxesPacked.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtBoxesPacked.Location = new System.Drawing.Point(903, 242);
            this.txtBoxesPacked.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxesPacked.MaxLength = 9;
            this.txtBoxesPacked.Name = "txtBoxesPacked";
            this.txtBoxesPacked.Size = new System.Drawing.Size(216, 41);
            this.txtBoxesPacked.TabIndex = 14;
            this.txtBoxesPacked.Leave += new System.EventHandler(this.txtBoxesPacked_Leave);
            // 
            // dateControl1
            // 
            this.dateControl1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dateControl1.AutoSize = true;
            this.dateControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dateControl1.DateValue = new System.DateTime(((long)(0)));
            this.dateControl1.Location = new System.Drawing.Point(271, 7);
            this.dateControl1.Margin = new System.Windows.Forms.Padding(4);
            this.dateControl1.Name = "dateControl1";
            this.dateControl1.Size = new System.Drawing.Size(302, 46);
            this.dateControl1.TabIndex = 23;
            // 
            // lblGroupId
            // 
            this.lblGroupId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblGroupId.AutoSize = true;
            this.lblGroupId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblGroupId.Location = new System.Drawing.Point(746, 190);
            this.lblGroupId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGroupId.Name = "lblGroupId";
            this.lblGroupId.Size = new System.Drawing.Size(149, 36);
            this.lblGroupId.TabIndex = 9;
            this.lblGroupId.Text = "Group Id:";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(109, 190);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(154, 36);
            this.label11.TabIndex = 25;
            this.label11.Text = "Item Size:";
            // 
            // cmbItemSize
            // 
            this.cmbItemSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbItemSize.FormattingEnabled = true;
            this.cmbItemSize.Location = new System.Drawing.Point(279, 182);
            this.cmbItemSize.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.cmbItemSize.Name = "cmbItemSize";
            this.cmbItemSize.Size = new System.Drawing.Size(366, 44);
            this.cmbItemSize.TabIndex = 9;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel3, 2);
            this.flowLayoutPanel3.Controls.Add(this.btnPrint);
            this.flowLayoutPanel3.Controls.Add(this.btnCancel);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(842, 685);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(342, 51);
            this.flowLayoutPanel3.TabIndex = 22;
            // 
            // txtTotalBoxesPacked
            // 
            this.txtTotalBoxesPacked.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTotalBoxesPacked.Enabled = false;
            this.txtTotalBoxesPacked.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtTotalBoxesPacked.Location = new System.Drawing.Point(279, 509);
            this.txtTotalBoxesPacked.Margin = new System.Windows.Forms.Padding(12, 4, 4, 4);
            this.txtTotalBoxesPacked.MaxLength = 9;
            this.txtTotalBoxesPacked.Name = "txtTotalBoxesPacked";
            this.txtTotalBoxesPacked.Size = new System.Drawing.Size(366, 41);
            this.txtTotalBoxesPacked.TabIndex = 17;
            this.txtTotalBoxesPacked.TextChanged += new System.EventHandler(this.txtTotalBoxesPacked_Change);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(22, 632);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(241, 36);
            this.label8.TabIndex = 20;
            this.label8.Text = "PreShip PLT Id:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(123, 572);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 36);
            this.label9.TabIndex = 18;
            this.label9.Text = "Pallet Id:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(22, 512);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(241, 36);
            this.label6.TabIndex = 16;
            this.label6.Text = "Total Inner Box:";
            // 
            // ScanMultipleBatchMTS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1260, 905);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1276, 901);
            this.Name = "ScanMultipleBatchMTS";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scan Multiple Batch";
            this.Load += new System.EventHandler(this.ScanMultipleBatch_Load);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvScanMultiBatchInfo)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private DateControl dateControl1;
        private System.Windows.Forms.ComboBox cmbGroupId;
        private System.Windows.Forms.ComboBox cmbPreShipPltId;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtInnerPrinter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtstation;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.ComboBox cmbPalletId;
        private System.Windows.Forms.ComboBox cmbItemNumber;
        private System.Windows.Forms.ComboBox cmbPurchaseOrder;
        private System.Windows.Forms.TextBox txtInternalLotNumber;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTotalBoxesPacked;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblGroupId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvScanMultiBatchInfo;
        private System.Windows.Forms.TextBox txtBoxesPacked;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAddMultiJob;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbItemSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBoxesPacked;
        private DateControl dateControl1;
        private System.Windows.Forms.Label label8;
    }
}