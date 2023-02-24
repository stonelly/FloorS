namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    partial class LineControlMaintainence
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineControlMaintainence));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtCyclePerHour = new System.Windows.Forms.TextBox();
            this.txtPcsPerHour = new System.Windows.Forms.TextBox();
            this.lblCyclePerHour = new System.Windows.Forms.Label();
            this.txtFormer = new System.Windows.Forms.TextBox();
            this.lblFormer = new System.Windows.Forms.Label();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.lblPcsPerHour = new System.Windows.Forms.Label();
            this.lblLine = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(251, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 39);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(104, 223);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 39);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCyclePerHour
            // 
            this.txtCyclePerHour.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCyclePerHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCyclePerHour.Location = new System.Drawing.Point(216, 133);
            this.txtCyclePerHour.MaxLength = 4;
            this.txtCyclePerHour.Name = "txtCyclePerHour";
            this.txtCyclePerHour.ReadOnly = true;
            this.txtCyclePerHour.Size = new System.Drawing.Size(150, 35);
            this.txtCyclePerHour.TabIndex = 3;
            // 
            // txtPcsPerHour
            // 
            this.txtPcsPerHour.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPcsPerHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcsPerHour.Location = new System.Drawing.Point(216, 90);
            this.txtPcsPerHour.MaxLength = 5;
            this.txtPcsPerHour.Name = "txtPcsPerHour";
            this.txtPcsPerHour.ReadOnly = true;
            this.txtPcsPerHour.Size = new System.Drawing.Size(150, 35);
            this.txtPcsPerHour.TabIndex = 2;
            this.txtPcsPerHour.TextChanged += new System.EventHandler(this.txtPcsPerHour_TextChanged);
            this.txtPcsPerHour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPcsPerHour_KeyPress);
            this.txtPcsPerHour.Leave += new System.EventHandler(this.txtFormer_Leave);
            // 
            // lblCyclePerHour
            // 
            this.lblCyclePerHour.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCyclePerHour.AutoSize = true;
            this.lblCyclePerHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCyclePerHour.Location = new System.Drawing.Point(21, 136);
            this.lblCyclePerHour.Name = "lblCyclePerHour";
            this.lblCyclePerHour.Size = new System.Drawing.Size(189, 29);
            this.lblCyclePerHour.TabIndex = 55;
            this.lblCyclePerHour.Text = "Cycle Per Hour";
            // 
            // txtFormer
            // 
            this.txtFormer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtFormer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormer.Location = new System.Drawing.Point(216, 47);
            this.txtFormer.MaxLength = 5;
            this.txtFormer.Name = "txtFormer";
            this.txtFormer.Size = new System.Drawing.Size(150, 35);
            this.txtFormer.TabIndex = 1;
            this.txtFormer.TextChanged += new System.EventHandler(this.txtFormer_TextChanged);
            this.txtFormer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFormer_KeyPress);
            this.txtFormer.Leave += new System.EventHandler(this.txtFormer_Leave);
            // 
            // lblFormer
            // 
            this.lblFormer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFormer.AutoSize = true;
            this.lblFormer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormer.Location = new System.Drawing.Point(22, 50);
            this.lblFormer.Name = "lblFormer";
            this.lblFormer.Size = new System.Drawing.Size(188, 29);
            this.lblFormer.TabIndex = 53;
            this.lblFormer.Text = "No. of Formers";
            // 
            // txtLine
            // 
            this.txtLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLine.Location = new System.Drawing.Point(216, 4);
            this.txtLine.Name = "txtLine";
            this.txtLine.ReadOnly = true;
            this.txtLine.Size = new System.Drawing.Size(150, 35);
            this.txtLine.TabIndex = 0;
            this.txtLine.TabStop = false;
            // 
            // lblPcsPerHour
            // 
            this.lblPcsPerHour.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPcsPerHour.AutoSize = true;
            this.lblPcsPerHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPcsPerHour.Location = new System.Drawing.Point(43, 93);
            this.lblPcsPerHour.Name = "lblPcsPerHour";
            this.lblPcsPerHour.Size = new System.Drawing.Size(167, 29);
            this.lblPcsPerHour.TabIndex = 51;
            this.lblPcsPerHour.Text = "Pcs Per Hour";
            // 
            // lblLine
            // 
            this.lblLine.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLine.AutoSize = true;
            this.lblLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLine.Location = new System.Drawing.Point(147, 7);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(63, 29);
            this.lblLine.TabIndex = 50;
            this.lblLine.Text = "Line";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblLine, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFormer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPcsPerHour, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtCyclePerHour, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblCyclePerHour, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPcsPerHour, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLine, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFormer, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 172);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // LineControlMaintainence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 274);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LineControlMaintainence";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Line Control Maintenance";
            this.Load += new System.EventHandler(this.LineControlMaintainence_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCyclePerHour;
        private System.Windows.Forms.TextBox txtPcsPerHour;
        private System.Windows.Forms.Label lblCyclePerHour;
        private System.Windows.Forms.TextBox txtFormer;
        private System.Windows.Forms.Label lblFormer;
        private System.Windows.Forms.TextBox txtLine;
        private System.Windows.Forms.Label lblPcsPerHour;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}