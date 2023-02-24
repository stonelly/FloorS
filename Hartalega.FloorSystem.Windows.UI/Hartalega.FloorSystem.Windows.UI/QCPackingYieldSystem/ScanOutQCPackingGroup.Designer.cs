namespace Hartalega.FloorSystem.Windows.UI.QCPackingYieldSystem
{
    partial class ScanOutQCPackingGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanOutQCPackingGroup));
            this.grpScanOutQCPacking = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtDate = new Hartalega.FloorSystem.Windows.UI.DateControl();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.ddQCGroupId = new System.Windows.Forms.ComboBox();
            this.lblShift = new System.Windows.Forms.Label();
            this.txtShift = new System.Windows.Forms.TextBox();
            this.txtQCMemberId = new System.Windows.Forms.TextBox();
            this.lblQcGroupId = new System.Windows.Forms.Label();
            this.lblQCMemberId = new System.Windows.Forms.Label();
            this.txtMemberCount = new System.Windows.Forms.TextBox();
            this.lblMemberCount = new System.Windows.Forms.Label();
            this.grpQCMembersInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.grdQCMembersInfo = new System.Windows.Forms.DataGridView();
            this.colMemberId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpScanOutQCPacking.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.grpQCMembersInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQCMembersInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // grpScanOutQCPacking
            // 
            this.grpScanOutQCPacking.Controls.Add(this.tableLayoutPanel1);
            this.grpScanOutQCPacking.Controls.Add(this.grpQCMembersInfo);
            this.grpScanOutQCPacking.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpScanOutQCPacking.Location = new System.Drawing.Point(22, 10);
            this.grpScanOutQCPacking.Name = "grpScanOutQCPacking";
            this.grpScanOutQCPacking.Size = new System.Drawing.Size(961, 660);
            this.grpScanOutQCPacking.TabIndex = 0;
            this.grpScanOutQCPacking.TabStop = false;
            this.grpScanOutQCPacking.Text = "Scan Out QC/Packing Group";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.5509F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.4491F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtQCMemberId, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblQcGroupId, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblQCMemberId, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtMemberCount, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblMemberCount, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 45);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(954, 217);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flowLayoutPanel1.Controls.Add(this.lblDate);
            this.flowLayoutPanel1.Controls.Add(this.dtDate);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(538, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(413, 48);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(3, 8);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(88, 29);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date : ";
            // 
            // dtDate
            // 
            this.dtDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dtDate.AutoSize = true;
            this.dtDate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dtDate.DateValue = new System.DateTime(((long)(0)));
            this.dtDate.Location = new System.Drawing.Point(97, 3);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(302, 40);
            this.dtDate.TabIndex = 1;
            this.dtDate.TabStop = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flowLayoutPanel2.Controls.Add(this.ddQCGroupId);
            this.flowLayoutPanel2.Controls.Add(this.lblShift);
            this.flowLayoutPanel2.Controls.Add(this.txtShift);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(237, 57);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(710, 46);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // ddQCGroupId
            // 
            this.ddQCGroupId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddQCGroupId.FormattingEnabled = true;
            this.ddQCGroupId.IntegralHeight = false;
            this.ddQCGroupId.Location = new System.Drawing.Point(3, 5);
            this.ddQCGroupId.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.ddQCGroupId.Name = "ddQCGroupId";
            this.ddQCGroupId.Size = new System.Drawing.Size(290, 37);
            this.ddQCGroupId.TabIndex = 0;
            this.ddQCGroupId.SelectedIndexChanged += new System.EventHandler(this.ddQCGroupId_SelectedIndexChanged);
            // 
            // lblShift
            // 
            this.lblShift.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblShift.AutoSize = true;
            this.lblShift.Location = new System.Drawing.Point(311, 8);
            this.lblShift.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(79, 29);
            this.lblShift.TabIndex = 8;
            this.lblShift.Text = "Shift :";
            // 
            // txtShift
            // 
            this.txtShift.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtShift.Enabled = false;
            this.txtShift.Location = new System.Drawing.Point(405, 6);
            this.txtShift.Margin = new System.Windows.Forms.Padding(12, 5, 3, 3);
            this.txtShift.Name = "txtShift";
            this.txtShift.Size = new System.Drawing.Size(285, 35);
            this.txtShift.TabIndex = 1;
            this.txtShift.TabStop = false;
            // 
            // txtQCMemberId
            // 
            this.txtQCMemberId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtQCMemberId.Location = new System.Drawing.Point(240, 172);
            this.txtQCMemberId.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtQCMemberId.MaxLength = 6;
            this.txtQCMemberId.Name = "txtQCMemberId";
            this.txtQCMemberId.Size = new System.Drawing.Size(290, 35);
            this.txtQCMemberId.TabIndex = 1;
            this.txtQCMemberId.Leave += new System.EventHandler(this.txtQCMemberId_Leave);
            // 
            // lblQcGroupId
            // 
            this.lblQcGroupId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblQcGroupId.AutoSize = true;
            this.lblQcGroupId.Location = new System.Drawing.Point(103, 66);
            this.lblQcGroupId.Name = "lblQcGroupId";
            this.lblQcGroupId.Size = new System.Drawing.Size(128, 29);
            this.lblQcGroupId.TabIndex = 2;
            this.lblQcGroupId.Text = "Group Id :";
            // 
            // lblQCMemberId
            // 
            this.lblQCMemberId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblQCMemberId.AutoSize = true;
            this.lblQCMemberId.Location = new System.Drawing.Point(79, 175);
            this.lblQCMemberId.Name = "lblQCMemberId";
            this.lblQCMemberId.Size = new System.Drawing.Size(152, 29);
            this.lblQCMemberId.TabIndex = 4;
            this.lblQCMemberId.Text = "Member Id :";
            // 
            // txtMemberCount
            // 
            this.txtMemberCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMemberCount.Enabled = false;
            this.txtMemberCount.Location = new System.Drawing.Point(240, 117);
            this.txtMemberCount.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.txtMemberCount.Name = "txtMemberCount";
            this.txtMemberCount.Size = new System.Drawing.Size(290, 35);
            this.txtMemberCount.TabIndex = 0;
            this.txtMemberCount.TabStop = false;
            // 
            // lblMemberCount
            // 
            this.lblMemberCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMemberCount.AutoSize = true;
            this.lblMemberCount.Location = new System.Drawing.Point(33, 120);
            this.lblMemberCount.Name = "lblMemberCount";
            this.lblMemberCount.Size = new System.Drawing.Size(198, 29);
            this.lblMemberCount.TabIndex = 3;
            this.lblMemberCount.Text = "Member Count :";
            // 
            // grpQCMembersInfo
            // 
            this.grpQCMembersInfo.Controls.Add(this.tableLayoutPanel2);
            this.grpQCMembersInfo.Location = new System.Drawing.Point(5, 282);
            this.grpQCMembersInfo.Name = "grpQCMembersInfo";
            this.grpQCMembersInfo.Size = new System.Drawing.Size(954, 360);
            this.grpQCMembersInfo.TabIndex = 1;
            this.grpQCMembersInfo.TabStop = false;
            this.grpQCMembersInfo.Text = "QC Members Info";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.grdQCMembersInfo, 0, 0);
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 30);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(935, 320);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // grdQCMembersInfo
            // 
            this.grdQCMembersInfo.AllowUserToAddRows = false;
            this.grdQCMembersInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdQCMembersInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMemberId,
            this.colName,
            this.colStartTime,
            this.colEndTime});
            this.grdQCMembersInfo.Enabled = false;
            this.grdQCMembersInfo.Location = new System.Drawing.Point(3, 3);
            this.grdQCMembersInfo.Name = "grdQCMembersInfo";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdQCMembersInfo.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdQCMembersInfo.Size = new System.Drawing.Size(929, 300);
            this.grdQCMembersInfo.StandardTab = true;
            this.grdQCMembersInfo.TabIndex = 0;
            this.grdQCMembersInfo.TabStop = false;
            // 
            // colMemberId
            // 
            this.colMemberId.HeaderText = "Member ID";
            this.colMemberId.Name = "colMemberId";
            this.colMemberId.Width = 150;
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.Width = 200;
            // 
            // colStartTime
            // 
            this.colStartTime.HeaderText = "Start Time";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.Width = 265;
            // 
            // colEndTime
            // 
            this.colEndTime.HeaderText = "End Time";
            this.colEndTime.Name = "colEndTime";
            this.colEndTime.Width = 270;
            // 
            // ScanOutQCPackingGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.grpScanOutQCPacking);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "ScanOutQCPackingGroup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scan Out QC/Packing Group";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ScanOutQCPackingGroup_Load);
            this.grpScanOutQCPacking.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.grpQCMembersInfo.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdQCMembersInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpScanOutQCPacking;
        private System.Windows.Forms.TextBox txtShift;
        private System.Windows.Forms.GroupBox grpQCMembersInfo;
        private System.Windows.Forms.DataGridView grdQCMembersInfo;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.TextBox txtQCMemberId;
        private System.Windows.Forms.TextBox txtMemberCount;
        private System.Windows.Forms.ComboBox ddQCGroupId;
        private System.Windows.Forms.Label lblQCMemberId;
        private System.Windows.Forms.Label lblMemberCount;
        private System.Windows.Forms.Label lblQcGroupId;
        private DateControl dtDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMemberId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndTime;

    }
}