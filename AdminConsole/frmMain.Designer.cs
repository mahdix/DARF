namespace AdminConsole
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.imgNodes = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.blockWebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topLeftTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topRightTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomLeftTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomRightTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCodeWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.imgMenuItems = new System.Windows.Forms.ImageList(this.components);
            this.treeTopLeft = new AdminConsole.ctlWebTree();
            this.treeBottomLeft = new AdminConsole.ctlWebTree();
            this.treeTopRight = new AdminConsole.ctlWebTree();
            this.treeBottomRight = new AdminConsole.ctlWebTree();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgNodes
            // 
            this.imgNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgNodes.ImageStream")));
            this.imgNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgNodes.Images.SetKeyName(0, "BlockWeb");
            this.imgNodes.Images.SetKeyName(1, "Block");
            this.imgNodes.Images.SetKeyName(2, "Blocks");
            this.imgNodes.Images.SetKeyName(3, "Services");
            this.imgNodes.Images.SetKeyName(4, "Service");
            this.imgNodes.Images.SetKeyName(5, "Connectors");
            this.imgNodes.Images.SetKeyName(6, "EndPoint");
            this.imgNodes.Images.SetKeyName(7, "EndPoints");
            this.imgNodes.Images.SetKeyName(8, "GlobalConnectors");
            this.imgNodes.Images.SetKeyName(9, "Connector");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blockWebToolStripMenuItem,
            this.codeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(644, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // blockWebToolStripMenuItem
            // 
            this.blockWebToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.blockWebToolStripMenuItem.Name = "blockWebToolStripMenuItem";
            this.blockWebToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.blockWebToolStripMenuItem.Text = "BlockWeb";
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topLeftTreeToolStripMenuItem,
            this.topRightTreeToolStripMenuItem,
            this.bottomLeftTreeToolStripMenuItem,
            this.bottomRightTreeToolStripMenuItem});
            this.addNewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addNewToolStripMenuItem.Image")));
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addNewToolStripMenuItem.Text = "Add New...";
            this.addNewToolStripMenuItem.Click += new System.EventHandler(this.addNewToolStripMenuItem_Click);
            // 
            // topLeftTreeToolStripMenuItem
            // 
            this.topLeftTreeToolStripMenuItem.Name = "topLeftTreeToolStripMenuItem";
            this.topLeftTreeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.topLeftTreeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.topLeftTreeToolStripMenuItem.Text = "Top Left Tree";
            this.topLeftTreeToolStripMenuItem.Click += new System.EventHandler(this.topLeftTreeToolStripMenuItem_Click);
            // 
            // topRightTreeToolStripMenuItem
            // 
            this.topRightTreeToolStripMenuItem.Name = "topRightTreeToolStripMenuItem";
            this.topRightTreeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.topRightTreeToolStripMenuItem.Text = "Top Right Tree";
            this.topRightTreeToolStripMenuItem.Click += new System.EventHandler(this.topRightTreeToolStripMenuItem_Click);
            // 
            // bottomLeftTreeToolStripMenuItem
            // 
            this.bottomLeftTreeToolStripMenuItem.Name = "bottomLeftTreeToolStripMenuItem";
            this.bottomLeftTreeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.bottomLeftTreeToolStripMenuItem.Text = "Bottom Left Tree";
            this.bottomLeftTreeToolStripMenuItem.Click += new System.EventHandler(this.bottomLeftTreeToolStripMenuItem_Click);
            // 
            // bottomRightTreeToolStripMenuItem
            // 
            this.bottomRightTreeToolStripMenuItem.Name = "bottomRightTreeToolStripMenuItem";
            this.bottomRightTreeToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.bottomRightTreeToolStripMenuItem.Text = "Bottom Right Tree";
            this.bottomRightTreeToolStripMenuItem.Click += new System.EventHandler(this.bottomRightTreeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // codeToolStripMenuItem
            // 
            this.codeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newCodeWindowToolStripMenuItem});
            this.codeToolStripMenuItem.Name = "codeToolStripMenuItem";
            this.codeToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.codeToolStripMenuItem.Text = "Script";
            // 
            // newCodeWindowToolStripMenuItem
            // 
            this.newCodeWindowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newCodeWindowToolStripMenuItem.Image")));
            this.newCodeWindowToolStripMenuItem.Name = "newCodeWindowToolStripMenuItem";
            this.newCodeWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.newCodeWindowToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.newCodeWindowToolStripMenuItem.Text = "New Window...";
            this.newCodeWindowToolStripMenuItem.Click += new System.EventHandler(this.newCodeWindowToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(644, 441);
            this.splitContainer1.SplitterDistance = 615;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeTopLeft);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeBottomLeft);
            this.splitContainer2.Size = new System.Drawing.Size(615, 441);
            this.splitContainer2.SplitterDistance = 411;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.treeTopRight);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.treeBottomRight);
            this.splitContainer3.Size = new System.Drawing.Size(25, 441);
            this.splitContainer3.SplitterDistance = 411;
            this.splitContainer3.TabIndex = 0;
            // 
            // imgMenuItems
            // 
            this.imgMenuItems.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgMenuItems.ImageStream")));
            this.imgMenuItems.TransparentColor = System.Drawing.Color.Transparent;
            this.imgMenuItems.Images.SetKeyName(0, "ADD");
            this.imgMenuItems.Images.SetKeyName(1, "DET");
            this.imgMenuItems.Images.SetKeyName(2, "ATT");
            this.imgMenuItems.Images.SetKeyName(3, "INVOKE");
            this.imgMenuItems.Images.SetKeyName(4, "COP");
            this.imgMenuItems.Images.SetKeyName(5, "QINVOKE");
            this.imgMenuItems.Images.SetKeyName(6, "UNLOAD");
            this.imgMenuItems.Images.SetKeyName(7, "DELB");
            this.imgMenuItems.Images.SetKeyName(8, "CODE");
            this.imgMenuItems.Images.SetKeyName(9, "RELOADWEB");
            this.imgMenuItems.Images.SetKeyName(10, "JINVOKE");
            this.imgMenuItems.Images.SetKeyName(11, "INFO");
            this.imgMenuItems.Images.SetKeyName(12, "ADDNEW");
            this.imgMenuItems.Images.SetKeyName(13, "RELOAD");
            this.imgMenuItems.Images.SetKeyName(14, "REF");
            // 
            // treeTopLeft
            // 
            this.treeTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTopLeft.Location = new System.Drawing.Point(0, 0);
            this.treeTopLeft.Name = "treeTopLeft";
            this.treeTopLeft.Size = new System.Drawing.Size(615, 411);
            this.treeTopLeft.TabIndex = 3;
            // 
            // treeBottomLeft
            // 
            this.treeBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeBottomLeft.Location = new System.Drawing.Point(0, 0);
            this.treeBottomLeft.Name = "treeBottomLeft";
            this.treeBottomLeft.Size = new System.Drawing.Size(615, 26);
            this.treeBottomLeft.TabIndex = 0;
            // 
            // treeTopRight
            // 
            this.treeTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTopRight.Location = new System.Drawing.Point(0, 0);
            this.treeTopRight.Name = "treeTopRight";
            this.treeTopRight.Size = new System.Drawing.Size(25, 411);
            this.treeTopRight.TabIndex = 0;
            // 
            // treeBottomRight
            // 
            this.treeBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeBottomRight.Location = new System.Drawing.Point(0, 0);
            this.treeBottomRight.Name = "treeBottomRight";
            this.treeBottomRight.Size = new System.Drawing.Size(25, 26);
            this.treeBottomRight.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 465);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "DCRF Admin Console v0.1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imgNodes;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem blockWebToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem codeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCodeWindowToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private ctlWebTree treeTopLeft;
        private ctlWebTree treeBottomLeft;
        private ctlWebTree treeTopRight;
        private ctlWebTree treeBottomRight;
        private System.Windows.Forms.ToolStripMenuItem topLeftTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topRightTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bottomLeftTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bottomRightTreeToolStripMenuItem;
        private System.Windows.Forms.ImageList imgMenuItems;
    }
}

