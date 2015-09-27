namespace AdminConsole
{
    partial class frmAttachEndPoint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAttachEndPoint));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rbService = new System.Windows.Forms.RadioButton();
            this.rbValue = new System.Windows.Forms.RadioButton();
            this.txtBlockId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtService = new System.Windows.Forms.TextBox();
            this.ctlValueHolder1 = new AdminConsole.ctlValuePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ctlPredefinedValues = new AdminConsole.ctlValuePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBlockId2 = new System.Windows.Forms.TextBox();
            this.txtConKey = new System.Windows.Forms.TextBox();
            this.rbChain = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.Location = new System.Drawing.Point(297, 313);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(117, 50);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Image = ((System.Drawing.Image)(resources.GetObject("cmdOK.Image")));
            this.cmdOK.Location = new System.Drawing.Point(102, 313);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(117, 50);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Block Id:";
            // 
            // rbService
            // 
            this.rbService.AutoSize = true;
            this.rbService.Location = new System.Drawing.Point(6, 26);
            this.rbService.Name = "rbService";
            this.rbService.Size = new System.Drawing.Size(61, 17);
            this.rbService.TabIndex = 0;
            this.rbService.Text = "Service";
            this.rbService.UseVisualStyleBackColor = true;
            // 
            // rbValue
            // 
            this.rbValue.AutoSize = true;
            this.rbValue.Checked = true;
            this.rbValue.Location = new System.Drawing.Point(6, 224);
            this.rbValue.Name = "rbValue";
            this.rbValue.Size = new System.Drawing.Size(52, 17);
            this.rbValue.TabIndex = 1;
            this.rbValue.Text = "Value";
            this.rbValue.UseVisualStyleBackColor = true;
            // 
            // txtBlockId
            // 
            this.txtBlockId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlockId.Location = new System.Drawing.Point(61, 26);
            this.txtBlockId.Name = "txtBlockId";
            this.txtBlockId.Size = new System.Drawing.Size(307, 20);
            this.txtBlockId.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Service:";
            // 
            // txtService
            // 
            this.txtService.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtService.Location = new System.Drawing.Point(58, 57);
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(310, 20);
            this.txtService.TabIndex = 5;
            // 
            // ctlValueHolder1
            // 
            this.ctlValueHolder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlValueHolder1.FixedType = false;
            this.ctlValueHolder1.Location = new System.Drawing.Point(3, 16);
            this.ctlValueHolder1.Name = "ctlValueHolder1";
            this.ctlValueHolder1.Size = new System.Drawing.Size(379, 51);
            this.ctlValueHolder1.TabIndex = 6;
            this.ctlValueHolder1.Value = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbService);
            this.groupBox1.Controls.Add(this.rbChain);
            this.groupBox1.Controls.Add(this.rbValue);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(99, 294);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EndPoint Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ctlValueHolder1);
            this.groupBox2.Location = new System.Drawing.Point(119, 236);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(385, 70);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Value EndPoint";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ctlPredefinedValues);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtBlockId);
            this.groupBox3.Controls.Add(this.txtService);
            this.groupBox3.Location = new System.Drawing.Point(119, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(385, 120);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Service EndPoint";
            // 
            // ctlPredefinedValues
            // 
            this.ctlPredefinedValues.FixedType = false;
            this.ctlPredefinedValues.Location = new System.Drawing.Point(52, 85);
            this.ctlPredefinedValues.Name = "ctlPredefinedValues";
            this.ctlPredefinedValues.Size = new System.Drawing.Size(316, 24);
            this.ctlPredefinedValues.TabIndex = 7;
            this.ctlPredefinedValues.Value = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Values:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.txtBlockId2);
            this.groupBox4.Controls.Add(this.txtConKey);
            this.groupBox4.Location = new System.Drawing.Point(119, 139);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(385, 90);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Chain Connector EndPoint";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Block Id:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Connector Key:";
            // 
            // txtBlockId2
            // 
            this.txtBlockId2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlockId2.Location = new System.Drawing.Point(61, 26);
            this.txtBlockId2.Name = "txtBlockId2";
            this.txtBlockId2.Size = new System.Drawing.Size(307, 20);
            this.txtBlockId2.TabIndex = 3;
            // 
            // txtConKey
            // 
            this.txtConKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConKey.Location = new System.Drawing.Point(92, 57);
            this.txtConKey.Name = "txtConKey";
            this.txtConKey.Size = new System.Drawing.Size(276, 20);
            this.txtConKey.TabIndex = 5;
            // 
            // rbChain
            // 
            this.rbChain.AutoSize = true;
            this.rbChain.Location = new System.Drawing.Point(6, 127);
            this.rbChain.Name = "rbChain";
            this.rbChain.Size = new System.Drawing.Size(52, 17);
            this.rbChain.TabIndex = 1;
            this.rbChain.Text = "Chain";
            this.rbChain.UseVisualStyleBackColor = true;
            // 
            // frmAttachEndPoint
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 394);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAttachEndPoint";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Attach EndPoint";
            this.Load += new System.EventHandler(this.frmAttachEndPoint_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbService;
        private System.Windows.Forms.RadioButton rbValue;
        private System.Windows.Forms.TextBox txtBlockId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtService;
        private ctlValuePicker ctlValueHolder1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private ctlValuePicker ctlPredefinedValues;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbChain;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBlockId2;
        private System.Windows.Forms.TextBox txtConKey;
    }
}