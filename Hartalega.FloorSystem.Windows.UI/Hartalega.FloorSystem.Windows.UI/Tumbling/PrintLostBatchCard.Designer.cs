// -----------------------------------------------------------------------
// <copyright file="PrintLostBatchCard.Designer.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    /// <summary>
    /// Module: Tumbling
    /// Screen Name: Print Lost Batch Card
    /// File Type: Designer file
    /// </summary>  
    public partial class PrintLostBatchCard
    {
        /// <summary>
        /// Label - Operator Id
        /// </summary>
        private System.Windows.Forms.Label lblOperatorId;

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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintLostBatchCard));
            this.lblOperatorId = new System.Windows.Forms.Label();
            this.txtTimer = new System.Windows.Forms.Timer(this.components);
            this.txtTenPcsWeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBatchWeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBoxSize = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGloveType = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.gruBoxChoiceSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtGloveDescription = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSizeSelected = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPcsWeight = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBatchWeight = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBoxLostArea = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtOperatorId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.Label();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dateControl = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gruBoxChoiceSelection.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOperatorId
            // 
            this.lblOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorId.AutoSize = true;
            this.lblOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblOperatorId.Location = new System.Drawing.Point(25, 59);
            this.lblOperatorId.Name = "lblOperatorId";
            this.lblOperatorId.Size = new System.Drawing.Size(156, 29);
            this.lblOperatorId.TabIndex = 0;
            this.lblOperatorId.Text = "Operator ID:";
            // 
            // txtTimer
            // 
            this.txtTimer.Enabled = true;
            this.txtTimer.Interval = 1000;
            // 
            // txtTenPcsWeight
            // 
            this.txtTenPcsWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTenPcsWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtTenPcsWeight.Location = new System.Drawing.Point(3, 3);
            this.txtTenPcsWeight.Name = "txtTenPcsWeight";
            this.txtTenPcsWeight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtTenPcsWeight.Size = new System.Drawing.Size(323, 35);
            this.txtTenPcsWeight.TabIndex = 3;
            this.txtTenPcsWeight.Enter += new System.EventHandler(this.txtTenPcsWeight_Enter);
            this.txtTenPcsWeight.Leave += new System.EventHandler(this.txtTenPcsWeight_Leave);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(46, 304);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 29);
            this.label4.TabIndex = 36;
            this.label4.Text = "Batch(Kg):";
            // 
            // txtBatchWeight
            // 
            this.txtBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtBatchWeight.Location = new System.Drawing.Point(3, 3);
            this.txtBatchWeight.Name = "txtBatchWeight";
            this.txtBatchWeight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtBatchWeight.Size = new System.Drawing.Size(323, 35);
            this.txtBatchWeight.TabIndex = 4;
            this.txtBatchWeight.Enter += new System.EventHandler(this.txtBatchWeight_Enter);
            this.txtBatchWeight.Leave += new System.EventHandler(this.txtBatchWeight_Leave);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(50, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 29);
            this.label5.TabIndex = 35;
            this.label5.Text = "10 Pcs(g):";
            // 
            // cmbBoxSize
            // 
            this.cmbBoxSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBoxSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBoxSize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBoxSize.BackColor = System.Drawing.Color.White;
            this.cmbBoxSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbBoxSize.ForeColor = System.Drawing.Color.Black;
            this.cmbBoxSize.FormattingEnabled = true;
            this.cmbBoxSize.Location = new System.Drawing.Point(3, 3);
            this.cmbBoxSize.MaxDropDownItems = 50;
            this.cmbBoxSize.Name = "cmbBoxSize";
            this.cmbBoxSize.Size = new System.Drawing.Size(323, 37);
            this.cmbBoxSize.TabIndex = 2;
            this.cmbBoxSize.Leave += new System.EventHandler(this.cmbBoxSize_Change);
            this.cmbBoxSize.Validated += new System.EventHandler(this.cmbBoxSize_Validated);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(110, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 29);
            this.label7.TabIndex = 32;
            this.label7.Text = "Size:";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(28, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(153, 29);
            this.label10.TabIndex = 24;
            this.label10.Text = "Description:";
            // 
            // txtGloveType
            // 
            this.txtGloveType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtGloveType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtGloveType.Location = new System.Drawing.Point(3, 3);
            this.txtGloveType.MaxLength = 50;
            this.txtGloveType.Name = "txtGloveType";
            this.txtGloveType.Size = new System.Drawing.Size(323, 35);
            this.txtGloveType.TabIndex = 1;
            this.txtGloveType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGloveType_KeyPress);
            this.txtGloveType.Leave += new System.EventHandler(this.txtGloveType_Leave);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(27, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(154, 29);
            this.label11.TabIndex = 21;
            this.label11.Text = "Glove Type:";
            // 
            // gruBoxChoiceSelection
            // 
            this.gruBoxChoiceSelection.Controls.Add(this.tableLayoutPanel3);
            this.gruBoxChoiceSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.gruBoxChoiceSelection.Location = new System.Drawing.Point(22, 10);
            this.gruBoxChoiceSelection.Name = "gruBoxChoiceSelection";
            this.gruBoxChoiceSelection.Size = new System.Drawing.Size(961, 660);
            this.gruBoxChoiceSelection.TabIndex = 0;
            this.gruBoxChoiceSelection.TabStop = false;
            this.gruBoxChoiceSelection.Text = "Print Lost Batch Card";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.37F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.63F));
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel6, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblOperatorId, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtGloveDescription, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.cmbBoxLostArea, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 1, 10);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel5, 1, 8);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.330001F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.7F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(950, 600);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel6.Controls.Add(this.txtGloveType);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(187, 102);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(442, 41);
            this.flowLayoutPanel6.TabIndex = 1;
            // 
            // txtGloveDescription
            // 
            this.txtGloveDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtGloveDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtGloveDescription.Location = new System.Drawing.Point(189, 154);
            this.txtGloveDescription.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.txtGloveDescription.Name = "txtGloveDescription";
            this.txtGloveDescription.ReadOnly = true;
            this.txtGloveDescription.Size = new System.Drawing.Size(748, 35);
            this.txtGloveDescription.TabIndex = 3;
            this.txtGloveDescription.TabStop = false;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel3.Controls.Add(this.cmbBoxSize);
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.txtSizeSelected);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(187, 199);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(760, 43);
            this.flowLayoutPanel3.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(332, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(189, 29);
            this.label6.TabIndex = 1;
            this.label6.Text = " Size Selected:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSizeSelected
            // 
            this.txtSizeSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSizeSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtSizeSelected.Location = new System.Drawing.Point(527, 4);
            this.txtSizeSelected.Name = "txtSizeSelected";
            this.txtSizeSelected.ReadOnly = true;
            this.txtSizeSelected.Size = new System.Drawing.Size(223, 35);
            this.txtSizeSelected.TabIndex = 2;
            this.txtSizeSelected.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel1.Controls.Add(this.txtTenPcsWeight);
            this.flowLayoutPanel1.Controls.Add(this.lblPcsWeight);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(187, 250);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(753, 39);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // lblPcsWeight
            // 
            this.lblPcsWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPcsWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblPcsWeight.ForeColor = System.Drawing.Color.Red;
            this.lblPcsWeight.Location = new System.Drawing.Point(332, 7);
            this.lblPcsWeight.Name = "lblPcsWeight";
            this.lblPcsWeight.Size = new System.Drawing.Size(366, 27);
            this.lblPcsWeight.TabIndex = 1;
            this.lblPcsWeight.Text = "Weigh 10 Pcs weight";
            this.lblPcsWeight.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel2.Controls.Add(this.txtBatchWeight);
            this.flowLayoutPanel2.Controls.Add(this.lblBatchWeight);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(187, 298);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(753, 41);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // lblBatchWeight
            // 
            this.lblBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBatchWeight.AutoSize = true;
            this.lblBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblBatchWeight.ForeColor = System.Drawing.Color.Red;
            this.lblBatchWeight.Location = new System.Drawing.Point(332, 10);
            this.lblBatchWeight.Name = "lblBatchWeight";
            this.lblBatchWeight.Size = new System.Drawing.Size(168, 20);
            this.lblBatchWeight.TabIndex = 1;
            this.lblBatchWeight.Text = "Weigh Batch weight";
            this.lblBatchWeight.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(51, 353);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 29);
            this.label2.TabIndex = 43;
            this.label2.Text = "Lost Area:";
            // 
            // cmbBoxLostArea
            // 
            this.cmbBoxLostArea.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBoxLostArea.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBoxLostArea.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBoxLostArea.BackColor = System.Drawing.Color.White;
            this.cmbBoxLostArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLostArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxLostArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbBoxLostArea.ForeColor = System.Drawing.Color.Black;
            this.cmbBoxLostArea.FormattingEnabled = true;
            this.cmbBoxLostArea.Location = new System.Drawing.Point(189, 349);
            this.cmbBoxLostArea.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.cmbBoxLostArea.MaxDropDownItems = 50;
            this.cmbBoxLostArea.Name = "cmbBoxLostArea";
            this.cmbBoxLostArea.Size = new System.Drawing.Size(323, 37);
            this.cmbBoxLostArea.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 331F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 306F));
            this.tableLayoutPanel2.Controls.Add(this.txtOperatorId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblOperatorName, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(187, 52);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(750, 43);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // txtOperatorId
            // 
            this.txtOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtOperatorId.Location = new System.Drawing.Point(3, 4);
            this.txtOperatorId.MaxLength = 6;
            this.txtOperatorId.Name = "txtOperatorId";
            this.txtOperatorId.Size = new System.Drawing.Size(323, 35);
            this.txtOperatorId.TabIndex = 0;
            this.txtOperatorId.Leave += new System.EventHandler(this.txtOperatorId_Leave);
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtName.AutoSize = true;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(355, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(89, 29);
            this.txtName.TabIndex = 1;
            this.txtName.Text = "Name:";
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblOperatorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblOperatorName.Location = new System.Drawing.Point(458, 3);
            this.lblOperatorName.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(292, 36);
            this.lblOperatorName.TabIndex = 2;
            this.lblOperatorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.41F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.59F));
            this.tableLayoutPanel4.Controls.Add(this.dateControl, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(187, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(753, 43);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // dateControl
            // 
            this.dateControl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateControl.DateValue = new System.DateTime(((long)(0)));
            this.dateControl.Location = new System.Drawing.Point(450, 2);
            this.dateControl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.dateControl.Name = "dateControl";
            this.dateControl.Size = new System.Drawing.Size(300, 38);
            this.dateControl.TabIndex = 1;
            this.dateControl.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(370, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date:";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(950, 548);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel4.TabIndex = 8;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel5.Controls.Add(this.btnPrint);
            this.flowLayoutPanel5.Controls.Add(this.btnCancel);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(671, 397);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(276, 44);
            this.flowLayoutPanel5.TabIndex = 6;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnPrint.AutoSize = true;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(3, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(130, 39);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.AutoSize = true;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(139, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PrintLostBatchCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.gruBoxChoiceSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "PrintLostBatchCard";
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Lost Batch Card";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintLostBatchCard_Load);
            this.gruBoxChoiceSelection.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Timer txtTimer;
        private System.Windows.Forms.TextBox txtTenPcsWeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBatchWeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBoxSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGloveType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gruBoxChoiceSelection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPcsWeight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.TextBox txtGloveDescription;
        private System.Windows.Forms.TextBox txtSizeSelected;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private System.Windows.Forms.Label lblBatchWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBoxLostArea;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOperatorName;
        private System.Windows.Forms.TextBox txtOperatorId;
        private System.Windows.Forms.Label txtName;
        private DateControl dateControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
    }
}