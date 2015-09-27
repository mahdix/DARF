namespace BlockWebHost
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.lblBlockFolder = new System.Windows.Forms.Label();
            this.txtBlocksFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmdShutdown = new System.Windows.Forms.Button();
            this.lblWebAddress1 = new System.Windows.Forms.Label();
            this.txtWebId = new System.Windows.Forms.TextBox();
            this.lblWebAddress = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRuntime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BlockWeb Id:";
            // 
            // lblBlockFolder
            // 
            this.lblBlockFolder.AutoSize = true;
            this.lblBlockFolder.Location = new System.Drawing.Point(12, 55);
            this.lblBlockFolder.Name = "lblBlockFolder";
            this.lblBlockFolder.Size = new System.Drawing.Size(74, 13);
            this.lblBlockFolder.TabIndex = 0;
            this.lblBlockFolder.Text = "Blocks Folder:";
            // 
            // txtBlocksFolder
            // 
            this.txtBlocksFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlocksFolder.Location = new System.Drawing.Point(92, 51);
            this.txtBlocksFolder.Name = "txtBlocksFolder";
            this.txtBlocksFolder.Size = new System.Drawing.Size(346, 20);
            this.txtBlocksFolder.TabIndex = 1;
            this.txtBlocksFolder.Text = "d:\\My\\MyDev\\DARF\\Development\\GeneralBlocks\\bin\\Debug";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(444, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = ".";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.Location = new System.Drawing.Point(15, 128);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 56);
            this.button2.TabIndex = 2;
            this.button2.Text = "START";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmdShutdown
            // 
            this.cmdShutdown.Enabled = false;
            this.cmdShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdShutdown.Image = ((System.Drawing.Image)(resources.GetObject("cmdShutdown.Image")));
            this.cmdShutdown.Location = new System.Drawing.Point(347, 128);
            this.cmdShutdown.Name = "cmdShutdown";
            this.cmdShutdown.Size = new System.Drawing.Size(120, 56);
            this.cmdShutdown.TabIndex = 2;
            this.cmdShutdown.Text = "SHUTDOWN";
            this.cmdShutdown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdShutdown.UseVisualStyleBackColor = true;
            this.cmdShutdown.Click += new System.EventHandler(this.cmdShutdown_Click);
            // 
            // lblWebAddress1
            // 
            this.lblWebAddress1.AutoSize = true;
            this.lblWebAddress1.Location = new System.Drawing.Point(12, 87);
            this.lblWebAddress1.Name = "lblWebAddress1";
            this.lblWebAddress1.Size = new System.Drawing.Size(101, 13);
            this.lblWebAddress1.TabIndex = 0;
            this.lblWebAddress1.Text = "BlockWeb Address:";
            // 
            // txtWebId
            // 
            this.txtWebId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWebId.Location = new System.Drawing.Point(92, 19);
            this.txtWebId.Name = "txtWebId";
            this.txtWebId.Size = new System.Drawing.Size(160, 20);
            this.txtWebId.TabIndex = 1;
            this.txtWebId.Text = "test";
            // 
            // lblWebAddress
            // 
            this.lblWebAddress.AutoSize = true;
            this.lblWebAddress.Location = new System.Drawing.Point(110, 87);
            this.lblWebAddress.Name = "lblWebAddress";
            this.lblWebAddress.Size = new System.Drawing.Size(35, 13);
            this.lblWebAddress.TabIndex = 4;
            this.lblWebAddress.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(367, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "UpTime:";
            // 
            // lblRuntime
            // 
            this.lblRuntime.AutoSize = true;
            this.lblRuntime.Location = new System.Drawing.Point(412, 87);
            this.lblRuntime.Name = "lblRuntime";
            this.lblRuntime.Size = new System.Drawing.Size(46, 13);
            this.lblRuntime.TabIndex = 0;
            this.lblRuntime.Text = "9999:99";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Enabled = false;
            this.cmdRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdRefresh.Image = ((System.Drawing.Image)(resources.GetObject("cmdRefresh.Image")));
            this.cmdRefresh.Location = new System.Drawing.Point(181, 128);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(120, 56);
            this.cmdRefresh.TabIndex = 2;
            this.cmdRefresh.Text = "RELOAD";
            this.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipTitle = "BlockWeb Host v0.1";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 202);
            this.Controls.Add(this.lblWebAddress);
            this.Controls.Add(this.cmdRefresh);
            this.Controls.Add(this.cmdShutdown);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtBlocksFolder);
            this.Controls.Add(this.lblRuntime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblWebAddress1);
            this.Controls.Add(this.lblBlockFolder);
            this.Controls.Add(this.txtWebId);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "BlockWeb Host v0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBlockFolder;
        private System.Windows.Forms.TextBox txtBlocksFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button cmdShutdown;
        private System.Windows.Forms.Label lblWebAddress1;
        private System.Windows.Forms.TextBox txtWebId;
        private System.Windows.Forms.Label lblWebAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRuntime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button cmdRefresh;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

