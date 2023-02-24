// -----------------------------------------------------------------------
// <copyright file="RejectGlove.Designer.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    /// <summary>
    /// Module: Tumbling
    /// Screen Name: Print Normal Batch Card
    /// File Type: Designer file
    /// </summary>  
    public partial class RejectGlove
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimer = new System.Windows.Forms.Timer(this.components);
            this.lblOperatorId = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.Label();
            this.txtOperatorId = new System.Windows.Forms.TextBox();
            this.lblOperatorName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBatchWeight = new System.Windows.Forms.TextBox();
            this.cmbBoxShift = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBoxLine = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGloveType = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.gruBoxChoiceSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbBoxReason = new System.Windows.Forms.ComboBox();
            this.txtGloveDescription = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBatchWeight = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dateControl = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gruBoxChoiceSelection.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(370, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Date:";
            // 
            // txtTimer
            // 
            this.txtTimer.Enabled = true;
            this.txtTimer.Interval = 1000;
            // 
            // lblOperatorId
            // 
            this.lblOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorId.AutoSize = true;
            this.lblOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblOperatorId.Location = new System.Drawing.Point(25, 60);
            this.lblOperatorId.Name = "lblOperatorId";
            this.lblOperatorId.Size = new System.Drawing.Size(156, 29);
            this.lblOperatorId.TabIndex = 8;
            this.lblOperatorId.Text = "Operator ID:";
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtName.AutoSize = true;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(354, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(89, 29);
            this.txtName.TabIndex = 10;
            this.txtName.Text = "Name:";
            // 
            // txtOperatorId
            // 
            this.txtOperatorId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOperatorId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOperatorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtOperatorId.Location = new System.Drawing.Point(3, 4);
            this.txtOperatorId.MaxLength = 6;
            this.txtOperatorId.Name = "txtOperatorId";
            this.txtOperatorId.Size = new System.Drawing.Size(323, 35);
            this.txtOperatorId.TabIndex = 0;
            this.txtOperatorId.Leave += new System.EventHandler(this.txtOperatorId_Leave);
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperatorName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblOperatorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblOperatorName.Location = new System.Drawing.Point(458, 4);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(292, 36);
            this.lblOperatorName.TabIndex = 11;
            this.lblOperatorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(46, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 29);
            this.label4.TabIndex = 36;
            this.label4.Text = "Batch(Kg):";
            // 
            // txtBatchWeight
            // 
            this.txtBatchWeight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBatchWeight.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBatchWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtBatchWeight.Location = new System.Drawing.Point(3, 3);
            this.txtBatchWeight.Name = "txtBatchWeight";
            this.txtBatchWeight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtBatchWeight.Size = new System.Drawing.Size(323, 35);
            this.txtBatchWeight.TabIndex = 4;
            this.txtBatchWeight.Enter += new System.EventHandler(this.txtBatchWeight_Enter);
            this.txtBatchWeight.Leave += new System.EventHandler(this.txtBatchWeight_Leave);
            // 
            // cmbBoxShift
            // 
            this.cmbBoxShift.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBoxShift.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBoxShift.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBoxShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxShift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbBoxShift.FormattingEnabled = true;
            this.cmbBoxShift.Location = new System.Drawing.Point(3, 3);
            this.cmbBoxShift.Name = "cmbBoxShift";
            this.cmbBoxShift.Size = new System.Drawing.Size(323, 37);
            this.cmbBoxShift.Sorted = true;
            this.cmbBoxShift.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(109, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 29);
            this.label8.TabIndex = 31;
            this.label8.Text = "Shift:";
            // 
            // cmbBoxLine
            // 
            this.cmbBoxLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBoxLine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBoxLine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBoxLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbBoxLine.FormattingEnabled = true;
            this.cmbBoxLine.Location = new System.Drawing.Point(3, 3);
            this.cmbBoxLine.Name = "cmbBoxLine";
            this.cmbBoxLine.Size = new System.Drawing.Size(323, 37);
            this.cmbBoxLine.Sorted = true;
            this.cmbBoxLine.TabIndex = 1;
            this.cmbBoxLine.Leave += new System.EventHandler(this.cmbBoxLine_Leave);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(111, 110);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 29);
            this.label9.TabIndex = 30;
            this.label9.Text = "Line:";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(28, 260);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(153, 29);
            this.label10.TabIndex = 24;
            this.label10.Text = "Description:";
            // 
            // txtGloveType
            // 
            this.txtGloveType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtGloveType.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGloveType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtGloveType.Location = new System.Drawing.Point(189, 207);
            this.txtGloveType.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.txtGloveType.MaxLength = 50;
            this.txtGloveType.Name = "txtGloveType";
            this.txtGloveType.Size = new System.Drawing.Size(323, 35);
            this.txtGloveType.TabIndex = 3;
            this.txtGloveType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGloveType_KeyPress);
            this.txtGloveType.Leave += new System.EventHandler(this.txtGloveType_Leave);
            this.txtGloveType.Validated += new System.EventHandler(this.txtGloveType_Validated);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(27, 210);
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
            this.gruBoxChoiceSelection.Text = "Reject Glove";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.37F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.63F));
            this.tableLayoutPanel3.Controls.Add(this.cmbBoxReason, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.lblOperatorId, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtGloveDescription, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.txtGloveType, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel3, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel4, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel5, 1, 9);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 45);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 12;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333957F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.330547F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.33085F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.333481F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.333481F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(950, 600);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // cmbBoxReason
            // 
            this.cmbBoxReason.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBoxReason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbBoxReason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbBoxReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxReason.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.cmbBoxReason.FormattingEnabled = true;
            this.cmbBoxReason.Location = new System.Drawing.Point(189, 356);
            this.cmbBoxReason.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.cmbBoxReason.Name = "cmbBoxReason";
            this.cmbBoxReason.Size = new System.Drawing.Size(749, 37);
            this.cmbBoxReason.Sorted = true;
            this.cmbBoxReason.TabIndex = 5;
            // 
            // txtGloveDescription
            // 
            this.txtGloveDescription.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGloveDescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtGloveDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.txtGloveDescription.Location = new System.Drawing.Point(189, 253);
            this.txtGloveDescription.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.txtGloveDescription.Name = "txtGloveDescription";
            this.txtGloveDescription.ReadOnly = true;
            this.txtGloveDescription.Size = new System.Drawing.Size(749, 35);
            this.txtGloveDescription.TabIndex = 2;
            this.txtGloveDescription.TabStop = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel2.Controls.Add(this.txtBatchWeight);
            this.flowLayoutPanel2.Controls.Add(this.lblBatchWeight);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(187, 304);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(747, 41);
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
            this.lblBatchWeight.TabIndex = 38;
            this.lblBatchWeight.Text = "Weigh Batch weight";
            this.lblBatchWeight.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(72, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 29);
            this.label2.TabIndex = 43;
            this.label2.Text = "Reason:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 331F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 307F));
            this.tableLayoutPanel2.Controls.Add(this.txtOperatorId, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblOperatorName, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(187, 53);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(751, 44);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.49F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.51F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.dateControl, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(187, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(753, 44);
            this.tableLayoutPanel4.TabIndex = 45;
            // 
            // dateControl
            // 
            this.dateControl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateControl.DateValue = new System.DateTime(((long)(0)));
            this.dateControl.Location = new System.Drawing.Point(450, 3);
            this.dateControl.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.dateControl.Name = "dateControl";
            this.dateControl.Size = new System.Drawing.Size(300, 38);
            this.dateControl.TabIndex = 7;
            this.dateControl.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel1.Controls.Add(this.cmbBoxLine);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(187, 103);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(433, 44);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel3.Controls.Add(this.cmbBoxShift);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(187, 153);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(425, 44);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(950, 421);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(0, 0);
            this.flowLayoutPanel4.TabIndex = 4;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel5.Controls.Add(this.btnPrint);
            this.flowLayoutPanel5.Controls.Add(this.btnCancel);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(665, 453);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(282, 47);
            this.flowLayoutPanel5.TabIndex = 5;
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
            // RejectGlove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.gruBoxChoiceSelection);
            this.MinimumSize = new System.Drawing.Size(1024, 678);
            this.Name = "RejectGlove";
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reject Glove";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.RejectGlove_Load);
            this.gruBoxChoiceSelection.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer txtTimer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBatchWeight;
        private System.Windows.Forms.ComboBox cmbBoxShift;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbBoxLine;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGloveType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox gruBoxChoiceSelection;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.TextBox txtGloveDescription;
        private System.Windows.Forms.Label lblBatchWeight;
        private System.Windows.Forms.ComboBox cmbBoxReason;
        private System.Windows.Forms.Label label2;
        private DateControl dateControl;
        private System.Windows.Forms.Label lblOperatorId;
        private System.Windows.Forms.TextBox txtOperatorId;
        private System.Windows.Forms.Label txtName;
        private System.Windows.Forms.Label lblOperatorName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
    }
}