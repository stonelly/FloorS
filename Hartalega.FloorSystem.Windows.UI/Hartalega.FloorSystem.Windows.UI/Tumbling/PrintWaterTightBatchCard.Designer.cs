// -----------------------------------------------------------------------
// <copyright file="PrintWaterTightBatchCard.Designer.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    /// <summary>
    /// Module: Tumbling
    /// Screen Name: Print WaterTight Batch Card
    /// File Type: Designer file
    /// </summary>  
    public partial class PrintWaterTightBatchCard
    {
        /// <summary>
        /// Label - Operator Id
        /// </summary>
        private System.Windows.Forms.Label lblOperatorId;

        /// <summary>
        /// Label - Operator Name
        /// </summary> 
        private System.Windows.Forms.Label lblOperatorName;

        /// <summary>
        /// Textbox - Operator id
        /// </summary>
        private System.Windows.Forms.TextBox txtOperatorId;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintWaterTightBatchCard));
            this.lblOperatorId = new System.Windows.Forms.Label();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.txtOperatorId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimer = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBoxType = new System.Windows.Forms.ComboBox();
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
            this.txtGloveDescription = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSizeSelected = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPcsWeight = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBatchWeight = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dateControl = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtItemId = new System.Windows.Forms.TextBox();
            this.gruBoxChoiceSelection.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOperatorId
            // 
            this.lblOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorId.AutoSize = true;
            this.lblOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblOperatorId.Location = new System.Drawing.Point(38, 75);
            this.lblOperatorId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOperatorId.Name = "lblOperatorId";
            this.lblOperatorId.Size = new System.Drawing.Size(188, 36);
            this.lblOperatorId.TabIndex = 0;
            this.lblOperatorId.Text = "Operator ID:";
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblOperatorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblOperatorName.Location = new System.Drawing.Point(572, 4);
            this.lblOperatorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(365, 45);
            this.lblOperatorName.TabIndex = 0;
            this.lblOperatorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOperatorId
            // 
            this.txtOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOperatorId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtOperatorId.Location = new System.Drawing.Point(4, 6);
            this.txtOperatorId.Margin = new System.Windows.Forms.Padding(4);
            this.txtOperatorId.MaxLength = 6;
            this.txtOperatorId.Name = "txtOperatorId";
            this.txtOperatorId.Size = new System.Drawing.Size(363, 41);
            this.txtOperatorId.TabIndex = 0;
            this.txtOperatorId.Leave += new System.EventHandler(this.txtOperatorId_Leave);
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtName.AutoSize = true;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(450, 9);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(105, 36);
            this.txtName.TabIndex = 4;
            this.txtName.Text = "Name:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(466, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "Date:";
            // 
            // txtTimer
            // 
            this.txtTimer.Enabled = true;
            this.txtTimer.Interval = 1000;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(132, 137);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 36);
            this.label2.TabIndex = 43;
            this.label2.Text = "Type:";
            // 
            // cmbBoxType
            // 
            this.cmbBoxType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBoxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBoxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBoxType.BackColor = System.Drawing.Color.White;
            this.cmbBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbBoxType.ForeColor = System.Drawing.Color.Black;
            this.cmbBoxType.FormattingEnabled = true;
            this.cmbBoxType.Location = new System.Drawing.Point(236, 133);
            this.cmbBoxType.Margin = new System.Windows.Forms.Padding(6, 4, 4, 4);
            this.cmbBoxType.MaxDropDownItems = 50;
            this.cmbBoxType.Name = "cmbBoxType";
            this.cmbBoxType.Size = new System.Drawing.Size(364, 44);
            this.cmbBoxType.TabIndex = 1;
            // 
            // txtTenPcsWeight
            // 
            this.txtTenPcsWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTenPcsWeight.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenPcsWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtTenPcsWeight.Location = new System.Drawing.Point(4, 4);
            this.txtTenPcsWeight.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenPcsWeight.Name = "txtTenPcsWeight";
            this.txtTenPcsWeight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtTenPcsWeight.Size = new System.Drawing.Size(363, 41);
            this.txtTenPcsWeight.TabIndex = 4;
            this.txtTenPcsWeight.Enter += new System.EventHandler(this.txtTenPcsWeight_Enter);
            this.txtTenPcsWeight.Leave += new System.EventHandler(this.txtTenPcsWeight_Leave);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(60, 447);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 36);
            this.label4.TabIndex = 36;
            this.label4.Text = "Batch(Kg):";
            // 
            // txtBatchWeight
            // 
            this.txtBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchWeight.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtBatchWeight.Location = new System.Drawing.Point(4, 4);
            this.txtBatchWeight.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchWeight.Name = "txtBatchWeight";
            this.txtBatchWeight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtBatchWeight.Size = new System.Drawing.Size(363, 41);
            this.txtBatchWeight.TabIndex = 5;
            this.txtBatchWeight.Enter += new System.EventHandler(this.txtBatchWeight_Enter);
            this.txtBatchWeight.Leave += new System.EventHandler(this.txtBatchWeight_Leave);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(64, 385);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 36);
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
            this.cmbBoxSize.Location = new System.Drawing.Point(4, 4);
            this.cmbBoxSize.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxSize.MaxDropDownItems = 50;
            this.cmbBoxSize.Name = "cmbBoxSize";
            this.cmbBoxSize.Size = new System.Drawing.Size(363, 44);
            this.cmbBoxSize.TabIndex = 3;
            this.cmbBoxSize.Leave += new System.EventHandler(this.cmbBoxSize_Change);
            this.cmbBoxSize.Validated += new System.EventHandler(this.cmbBoxSize_Validated);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(140, 323);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 36);
            this.label7.TabIndex = 32;
            this.label7.Text = "Size:";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(41, 261);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(185, 36);
            this.label10.TabIndex = 24;
            this.label10.Text = "Description:";
            // 
            // txtGloveType
            // 
            this.txtGloveType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtGloveType.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGloveType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtGloveType.Location = new System.Drawing.Point(4, 6);
            this.txtGloveType.Margin = new System.Windows.Forms.Padding(4);
            this.txtGloveType.MaxLength = 50;
            this.txtGloveType.Name = "txtGloveType";
            this.txtGloveType.Size = new System.Drawing.Size(363, 41);
            this.txtGloveType.TabIndex = 2;
            this.txtGloveType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGloveType_KeyPress);
            this.txtGloveType.Leave += new System.EventHandler(this.txtGloveType_Leave);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(40, 199);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(186, 36);
            this.label11.TabIndex = 21;
            this.label11.Text = "Glove Type:";
            // 
            // gruBoxChoiceSelection
            // 
            this.gruBoxChoiceSelection.Controls.Add(this.tableLayoutPanel3);
            this.gruBoxChoiceSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.gruBoxChoiceSelection.Location = new System.Drawing.Point(28, 12);
            this.gruBoxChoiceSelection.Margin = new System.Windows.Forms.Padding(4);
            this.gruBoxChoiceSelection.Name = "gruBoxChoiceSelection";
            this.gruBoxChoiceSelection.Padding = new System.Windows.Forms.Padding(4);
            this.gruBoxChoiceSelection.Size = new System.Drawing.Size(1201, 825);
            this.gruBoxChoiceSelection.TabIndex = 3;
            this.gruBoxChoiceSelection.TabStop = false;
            this.gruBoxChoiceSelection.Text = "Print Water Tight Batch Card";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.37F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.63F));
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblOperatorId, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtGloveDescription, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.cmbBoxType, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel5, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 3);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 56);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 12;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.33453F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.332933F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.332933F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.332933F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1188, 750);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // txtGloveDescription
            // 
            this.txtGloveDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtGloveDescription.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGloveDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtGloveDescription.Location = new System.Drawing.Point(236, 258);
            this.txtGloveDescription.Margin = new System.Windows.Forms.Padding(6, 4, 4, 4);
            this.txtGloveDescription.Name = "txtGloveDescription";
            this.txtGloveDescription.ReadOnly = true;
            this.txtGloveDescription.Size = new System.Drawing.Size(934, 41);
            this.txtGloveDescription.TabIndex = 1;
            this.txtGloveDescription.TabStop = false;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel3.Controls.Add(this.cmbBoxSize);
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.txtSizeSelected);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(234, 314);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(950, 54);
            this.flowLayoutPanel3.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(375, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 36);
            this.label6.TabIndex = 33;
            this.label6.Text = "   Size Selected:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSizeSelected
            // 
            this.txtSizeSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSizeSelected.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSizeSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtSizeSelected.Location = new System.Drawing.Point(628, 5);
            this.txtSizeSelected.Margin = new System.Windows.Forms.Padding(4);
            this.txtSizeSelected.Name = "txtSizeSelected";
            this.txtSizeSelected.ReadOnly = true;
            this.txtSizeSelected.Size = new System.Drawing.Size(260, 41);
            this.txtSizeSelected.TabIndex = 1;
            this.txtSizeSelected.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel1.Controls.Add(this.txtTenPcsWeight);
            this.flowLayoutPanel1.Controls.Add(this.lblPcsWeight);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(234, 378);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(941, 49);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // lblPcsWeight
            // 
            this.lblPcsWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPcsWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblPcsWeight.ForeColor = System.Drawing.Color.Red;
            this.lblPcsWeight.Location = new System.Drawing.Point(4, 49);
            this.lblPcsWeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPcsWeight.Name = "lblPcsWeight";
            this.lblPcsWeight.Size = new System.Drawing.Size(628, 29);
            this.lblPcsWeight.TabIndex = 37;
            this.lblPcsWeight.Text = "Weigh 10 Pcs weight";
            this.lblPcsWeight.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel2.Controls.Add(this.txtBatchWeight);
            this.flowLayoutPanel2.Controls.Add(this.lblBatchWeight);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(234, 439);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(941, 51);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // lblBatchWeight
            // 
            this.lblBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBatchWeight.AutoSize = true;
            this.lblBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblBatchWeight.ForeColor = System.Drawing.Color.Red;
            this.lblBatchWeight.Location = new System.Drawing.Point(375, 12);
            this.lblBatchWeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBatchWeight.Name = "lblBatchWeight";
            this.lblBatchWeight.Size = new System.Drawing.Size(203, 25);
            this.lblBatchWeight.TabIndex = 38;
            this.lblBatchWeight.Text = "Weigh Batch weight";
            this.lblBatchWeight.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 379F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 382F));
            this.tableLayoutPanel2.Controls.Add(this.txtOperatorId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblOperatorName, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(234, 66);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(941, 54);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.49F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.51F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.dateControl, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(234, 4);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(941, 54);
            this.tableLayoutPanel4.TabIndex = 45;
            // 
            // dateControl
            // 
            this.dateControl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateControl.DateValue = new System.DateTime(((long)(0)));
            this.dateControl.Location = new System.Drawing.Point(563, 3);
            this.dateControl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dateControl.Name = "dateControl";
            this.dateControl.Size = new System.Drawing.Size(374, 48);
            this.dateControl.TabIndex = 44;
            this.dateControl.TabStop = false;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel5.Controls.Add(this.btnPrint);
            this.flowLayoutPanel5.Controls.Add(this.btnCancel);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(830, 508);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(354, 61);
            this.flowLayoutPanel5.TabIndex = 6;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnPrint.AutoSize = true;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(4, 4);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(166, 58);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.AutoSize = true;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(178, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(162, 58);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(230, 719);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.30943F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.69057F));
            this.tableLayoutPanel1.Controls.Add(this.txtGloveType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtItemId, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(234, 190);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(950, 54);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtItemId
            // 
            this.txtItemId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtItemId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtItemId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtItemId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtItemId.Location = new System.Drawing.Point(379, 10);
            this.txtItemId.Margin = new System.Windows.Forms.Padding(6, 4, 4, 4);
            this.txtItemId.Name = "txtItemId";
            this.txtItemId.ReadOnly = true;
            this.txtItemId.Size = new System.Drawing.Size(567, 34);
            this.txtItemId.TabIndex = 46;
            this.txtItemId.TabStop = false;
            // 
            // PrintWaterTightBatchCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1260, 865);
            this.Controls.Add(this.gruBoxChoiceSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1276, 901);
            this.Name = "PrintWaterTightBatchCard";
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print WaterTight Batch Card";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintWaterTightBatchCard_Load);
            this.gruBoxChoiceSelection.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label txtName;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.Label lblBatchWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBoxType;
        private DateControl dateControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.TextBox txtItemId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
