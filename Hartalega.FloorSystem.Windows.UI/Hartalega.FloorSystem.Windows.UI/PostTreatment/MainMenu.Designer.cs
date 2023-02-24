namespace Hartalega.FloorSystem.Windows.UI.PostTreatment
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.btnChangeGloveType = new System.Windows.Forms.Button();
            this.btnScanPTBatchCard = new System.Windows.Forms.Button();
            this.btnProteinTest = new System.Windows.Forms.Button();
            this.btnHotBoxTest = new System.Windows.Forms.Button();
            this.btnPowderTest = new System.Windows.Forms.Button();
            this.btnScanPolymerTest = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnChangeGloveType
            // 
            this.btnChangeGloveType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeGloveType.Location = new System.Drawing.Point(3, 61);
            this.btnChangeGloveType.Name = "btnChangeGloveType";
            this.btnChangeGloveType.Size = new System.Drawing.Size(487, 50);
            this.btnChangeGloveType.TabIndex = 15;
            this.btnChangeGloveType.Text = "Change Glove Type";
            this.btnChangeGloveType.UseVisualStyleBackColor = true;
            this.btnChangeGloveType.Click += new System.EventHandler(this.btnChangeGloveType_Click);
            // 
            // btnScanPTBatchCard
            // 
            this.btnScanPTBatchCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanPTBatchCard.Location = new System.Drawing.Point(3, 3);
            this.btnScanPTBatchCard.Name = "btnScanPTBatchCard";
            this.btnScanPTBatchCard.Size = new System.Drawing.Size(487, 50);
            this.btnScanPTBatchCard.TabIndex = 14;
            this.btnScanPTBatchCard.Text = "Scan PT Batch Card";
            this.btnScanPTBatchCard.UseVisualStyleBackColor = true;
            this.btnScanPTBatchCard.Click += new System.EventHandler(this.btnScanPTBatchCard_Click);
            // 
            // btnProteinTest
            // 
            this.btnProteinTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProteinTest.Location = new System.Drawing.Point(3, 119);
            this.btnProteinTest.Name = "btnProteinTest";
            this.btnProteinTest.Size = new System.Drawing.Size(487, 50);
            this.btnProteinTest.TabIndex = 16;
            this.btnProteinTest.Text = "Print Protein Test Slip";
            this.btnProteinTest.UseVisualStyleBackColor = true;
            this.btnProteinTest.Click += new System.EventHandler(this.btnProteinTest_Click);
            // 
            // btnHotBoxTest
            // 
            this.btnHotBoxTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHotBoxTest.Location = new System.Drawing.Point(3, 177);
            this.btnHotBoxTest.Name = "btnHotBoxTest";
            this.btnHotBoxTest.Size = new System.Drawing.Size(487, 50);
            this.btnHotBoxTest.TabIndex = 17;
            this.btnHotBoxTest.Text = "Print Hot Box Test Slip";
            this.btnHotBoxTest.UseVisualStyleBackColor = true;
            this.btnHotBoxTest.Click += new System.EventHandler(this.btnHotBoxTest_Click);
            // 
            // btnPowderTest
            // 
            this.btnPowderTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPowderTest.Location = new System.Drawing.Point(3, 235);
            this.btnPowderTest.Name = "btnPowderTest";
            this.btnPowderTest.Size = new System.Drawing.Size(487, 50);
            this.btnPowderTest.TabIndex = 18;
            this.btnPowderTest.Text = "Print Powder Test Slip";
            this.btnPowderTest.UseVisualStyleBackColor = true;
            this.btnPowderTest.Click += new System.EventHandler(this.btnPowderTest_Click);
            // 
            // btnScanPolymerTest
            // 
            this.btnScanPolymerTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScanPolymerTest.Location = new System.Drawing.Point(3, 293);
            this.btnScanPolymerTest.Name = "btnScanPolymerTest";
            this.btnScanPolymerTest.Size = new System.Drawing.Size(487, 50);
            this.btnScanPolymerTest.TabIndex = 19;
            this.btnScanPolymerTest.Text = "Scan Polymer Test Results ";
            this.btnScanPolymerTest.UseVisualStyleBackColor = true;
            this.btnScanPolymerTest.Click += new System.EventHandler(this.btnScanPolymerTest_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnScanPTBatchCard, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnScanPolymerTest, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnChangeGloveType, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnPowderTest, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnProteinTest, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnHotBoxTest, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(233, 152);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(495, 351);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(22, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 660);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Post Treatment Main Menu";
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1005, 690);
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 730);
            this.Name = "MainMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Post Treatment Main Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnChangeGloveType;
        private System.Windows.Forms.Button btnScanPTBatchCard;
        private System.Windows.Forms.Button btnProteinTest;
        private System.Windows.Forms.Button btnHotBoxTest;
        private System.Windows.Forms.Button btnPowderTest;
        private System.Windows.Forms.Button btnScanPolymerTest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;

    }
}