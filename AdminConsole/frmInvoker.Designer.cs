namespace AdminConsole
{
    partial class frmInvoker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInvoker));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBlockWeb = new System.Windows.Forms.Label();
            this.lblBlock = new System.Windows.Forms.Label();
            this.lblService = new System.Windows.Forms.Label();
            this.cmdInvoke = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdAutoInvoke = new System.Windows.Forms.Button();
            this.cmdCopyResult = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.tblResult = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlObjectView = new System.Windows.Forms.Panel();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ctlArgPicker1 = new AdminConsole.ctlValuesPicker();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdMetaInfo = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tblResult.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpResult.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BlockWeb:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Block:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Service:";
            // 
            // lblBlockWeb
            // 
            this.lblBlockWeb.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBlockWeb.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblBlockWeb, 2);
            this.lblBlockWeb.Location = new System.Drawing.Point(73, 8);
            this.lblBlockWeb.Name = "lblBlockWeb";
            this.lblBlockWeb.Size = new System.Drawing.Size(259, 13);
            this.lblBlockWeb.TabIndex = 0;
            this.lblBlockWeb.Text = "723687162386213876213876172863726378216321";
            // 
            // lblBlock
            // 
            this.lblBlock.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBlock.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblBlock, 2);
            this.lblBlock.Location = new System.Drawing.Point(73, 38);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(308, 13);
            this.lblBlock.TabIndex = 0;
            this.lblBlock.Text = "723687162386213876213876172863726378216321\\r\\n12345";
            // 
            // lblService
            // 
            this.lblService.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblService.AutoSize = true;
            this.lblService.Location = new System.Drawing.Point(67, 6);
            this.lblService.Name = "lblService";
            this.lblService.Size = new System.Drawing.Size(67, 13);
            this.lblService.TabIndex = 0;
            this.lblService.Text = "servicename";
            // 
            // cmdInvoke
            // 
            this.cmdInvoke.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdInvoke.Location = new System.Drawing.Point(8, 20);
            this.cmdInvoke.Name = "cmdInvoke";
            this.cmdInvoke.Size = new System.Drawing.Size(94, 43);
            this.cmdInvoke.TabIndex = 2;
            this.cmdInvoke.Text = "Invoke (F5)";
            this.cmdInvoke.UseVisualStyleBackColor = true;
            this.cmdInvoke.Click += new System.EventHandler(this.cmdInvoke_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdClose.Image = ((System.Drawing.Image)(resources.GetObject("cmdClose.Image")));
            this.cmdClose.Location = new System.Drawing.Point(212, 20);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(94, 43);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "Close";
            this.cmdClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdAutoInvoke
            // 
            this.cmdAutoInvoke.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdAutoInvoke.Location = new System.Drawing.Point(110, 20);
            this.cmdAutoInvoke.Name = "cmdAutoInvoke";
            this.cmdAutoInvoke.Size = new System.Drawing.Size(94, 43);
            this.cmdAutoInvoke.TabIndex = 2;
            this.cmdAutoInvoke.Text = "Auto Invoke";
            this.cmdAutoInvoke.UseVisualStyleBackColor = true;
            this.cmdAutoInvoke.Click += new System.EventHandler(this.cmdAutoInvoke_Click);
            // 
            // cmdCopyResult
            // 
            this.cmdCopyResult.Location = new System.Drawing.Point(4, 20);
            this.cmdCopyResult.Name = "cmdCopyResult";
            this.cmdCopyResult.Size = new System.Drawing.Size(45, 23);
            this.cmdCopyResult.TabIndex = 7;
            this.cmdCopyResult.Text = "Copy";
            this.cmdCopyResult.UseVisualStyleBackColor = true;
            this.cmdCopyResult.Click += new System.EventHandler(this.cmdCopyResult_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Result:";
            // 
            // lblResult
            // 
            this.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblResult.Location = new System.Drawing.Point(63, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(247, 50);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "-";
            // 
            // tblResult
            // 
            this.tblResult.ColumnCount = 2;
            this.tblResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblResult.Controls.Add(this.panel1, 0, 0);
            this.tblResult.Controls.Add(this.lblResult, 1, 0);
            this.tblResult.Controls.Add(this.pnlObjectView, 0, 1);
            this.tblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblResult.Location = new System.Drawing.Point(3, 16);
            this.tblResult.Name = "tblResult";
            this.tblResult.RowCount = 2;
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tblResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblResult.Size = new System.Drawing.Size(313, 254);
            this.tblResult.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdCopyResult);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(54, 44);
            this.panel1.TabIndex = 2;
            // 
            // pnlObjectView
            // 
            this.tblResult.SetColumnSpan(this.pnlObjectView, 2);
            this.pnlObjectView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlObjectView.Location = new System.Drawing.Point(3, 53);
            this.pnlObjectView.Name = "pnlObjectView";
            this.pnlObjectView.Size = new System.Drawing.Size(307, 198);
            this.pnlObjectView.TabIndex = 1;
            // 
            // grpResult
            // 
            this.grpResult.Controls.Add(this.tblResult);
            this.grpResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResult.Location = new System.Drawing.Point(0, 0);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(319, 273);
            this.grpResult.TabIndex = 4;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Result";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBlock, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblBlockWeb, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(729, 369);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // splitContainer1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 5);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 93);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ctlArgPicker1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpResult);
            this.splitContainer1.Size = new System.Drawing.Size(723, 273);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 6;
            // 
            // ctlArgPicker1
            // 
            this.ctlArgPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlArgPicker1.Location = new System.Drawing.Point(0, 0);
            this.ctlArgPicker1.Name = "ctlArgPicker1";
            this.ctlArgPicker1.Size = new System.Drawing.Size(400, 273);
            this.ctlArgPicker1.TabIndex = 7;
            // 
            // panel4
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel4, 2);
            this.panel4.Controls.Add(this.cmdMetaInfo);
            this.panel4.Controls.Add(this.lblService);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(73, 63);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(332, 24);
            this.panel4.TabIndex = 6;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // cmdMetaInfo
            // 
            this.cmdMetaInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmdMetaInfo.Location = new System.Drawing.Point(0, 1);
            this.cmdMetaInfo.Name = "cmdMetaInfo";
            this.cmdMetaInfo.Size = new System.Drawing.Size(61, 23);
            this.cmdMetaInfo.TabIndex = 7;
            this.cmdMetaInfo.Text = "MetaInfo";
            this.cmdMetaInfo.UseVisualStyleBackColor = true;
            this.cmdMetaInfo.Click += new System.EventHandler(this.cmdMetaInfo_Click);
            // 
            // panel3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel3, 2);
            this.panel3.Controls.Add(this.cmdInvoke);
            this.panel3.Controls.Add(this.cmdAutoInvoke);
            this.panel3.Controls.Add(this.cmdClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(411, 3);
            this.panel3.Name = "panel3";
            this.tableLayoutPanel1.SetRowSpan(this.panel3, 3);
            this.panel3.Size = new System.Drawing.Size(315, 84);
            this.panel3.TabIndex = 5;
            // 
            // frmInvoker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 369);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmInvoker";
            this.Text = "Invoke Dialog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInvokeService_FormClosed);
            this.Load += new System.EventHandler(this.frmInvokeService_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInvokeService_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmInvokeService_KeyPress);
            this.tblResult.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpResult.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblBlockWeb;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.Label lblService;
        private System.Windows.Forms.Button cmdInvoke;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdAutoInvoke;
        private System.Windows.Forms.Button cmdCopyResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TableLayoutPanel tblResult;
        private System.Windows.Forms.Panel pnlObjectView;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button cmdMetaInfo;
        private ctlValuesPicker ctlArgPicker1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}