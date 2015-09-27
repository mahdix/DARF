using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AdminConsole.TreeNodes.Nodes
{
    public abstract class BaseNode
    {
        protected List<TreeNode> myParents = new List<TreeNode>();
        protected List<BaseNode> children = new List<BaseNode>();

        public abstract List<string> GetCommands(TreeNode myNode);
        public abstract void ExecuteCommand(string cmdKey, TreeNode myNode);
        
        public void OnExpand(TreeNode myNode)
        {
            //if this is the first time that the parent node is expanded, refresh children
            if (requiresRefresh(myNode))
            {
                refresh(myNode);
            }
        }

        protected abstract void buildChildrenList();

        protected void refresh(TreeNode myNode)
        {
            refresh(myNode, true);
        }

        protected void refresh(TreeNode myNode, bool updateChildren)
        {
            waitCursor();
            myNode.Nodes.Clear();

            if (updateChildren)
            {
                buildChildrenList();
            }

            foreach (BaseNode child in children)
            {
                TreeNode tn = new TreeNode();
                child.Attach(tn);
                myNode.Nodes.Add(tn);
            }

            restoreCursor();
        }

        public abstract void Attach(TreeNode myNode);

        private Cursor initialCursor = null;

        protected void waitCursor()
        {
            initialCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        protected void restoreCursor()
        {
            if (initialCursor != null)
            {
                Cursor.Current = initialCursor;
                initialCursor = null;
            }
        }

        protected void addTempNode(TreeNode myNode)
        {
            TreeNode temp = new TreeNode("TEMP");
            temp.Tag = "TEMP";
            myNode.Nodes.Add(temp);
        }

        protected bool requiresRefresh(TreeNode myNode)
        {
            if (myNode.Nodes.Count == 1 && myNode.Nodes[0].Text == "TEMP")
            {
                if (myNode.Nodes[0].Tag != null && myNode.Nodes[0].Tag.ToString() == "TEMP")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
