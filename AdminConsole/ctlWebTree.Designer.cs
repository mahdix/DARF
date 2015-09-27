namespace AdminConsole
{
    partial class ctlWebTree
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlWebTree));
            this.innerTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imgLoading = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // innerTree
            // 
            this.innerTree.AllowDrop = true;
            this.innerTree.ContextMenuStrip = this.contextMenuStrip1;
            this.innerTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerTree.FullRowSelect = true;
            this.innerTree.Location = new System.Drawing.Point(0, 0);
            this.innerTree.Name = "innerTree";
            this.innerTree.Size = new System.Drawing.Size(398, 297);
            this.innerTree.TabIndex = 1;
            this.innerTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeTopLeft_BeforeExpand);
            this.innerTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeTopLeft_ItemDrag);
            this.innerTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.innerTree_NodeMouseDoubleClick);
            this.innerTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeTopLeft_DragDrop);
            this.innerTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeTopLeft_DragEnter);
            this.innerTree.DragOver += new System.Windows.Forms.DragEventHandler(this.treeTopLeft_DragOver);
            this.innerTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeTopLeft_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // imgLoading
            // 
            this.imgLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgLoading.Image = ((System.Drawing.Image)(resources.GetObject("imgLoading.Image")));
            this.imgLoading.Location = new System.Drawing.Point(106, 61);
            this.imgLoading.Name = "imgLoading";
            this.imgLoading.Size = new System.Drawing.Size(186, 174);
            this.imgLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgLoading.TabIndex = 3;
            this.imgLoading.TabStop = false;
            this.imgLoading.Visible = false;
            // 
            // ctlWebTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imgLoading);
            this.Controls.Add(this.innerTree);
            this.Name = "ctlWebTree";
            this.Size = new System.Drawing.Size(398, 297);
            this.Load += new System.EventHandler(this.ctlWebTree_Load);
            this.Resize += new System.EventHandler(this.ctlWebTree_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.imgLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView innerTree;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox imgLoading;
    }
}
