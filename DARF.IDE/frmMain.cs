using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DCRF.Interface;

namespace DARF.IDE
{
    public partial class frmMain : Form
    {
        private List<string> rFiles = new List<string>(); //recent files
        private string recentFilesListFileName = "recentFiles";
        private const string lastOpenFileFileName = "lastOpenFile";
        private string blockPath = @"d:\My\MyDev\DARF\Development\Blocks\";

        public frmMain()
        {
            InitializeComponent();
        }

        public void AddNewTab()
        {
            TabPage tab = new TabPage("untitled");

            BlockAppCodeBox ctl = new BlockAppCodeBox();
            ctl.Tag = this;

            tab.Controls.Add(ctl);
            ctl.Dock = DockStyle.Fill;
            ctl.lblCaret = lblCaret;
            ctl.lblStatus = lblStatus;

            ctl.Init();

            tabControl1.TabPages.Add(tab);
            tabControl1.SelectedTab = tab;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            blockPath = System.Configuration.ConfigurationManager.AppSettings["defaultBlocksPath"];
           (tabControl1.SelectedTab.Controls[0] as BlockAppCodeBox).lblCaret = lblCaret;
           (tabControl1.SelectedTab.Controls[0] as BlockAppCodeBox).lblStatus = lblStatus;
            (tabControl1.SelectedTab.Controls[0] as BlockAppCodeBox).Init();

            if (File.Exists(lastOpenFileFileName))
            {
                string lastFile = File.ReadAllText(lastOpenFileFileName);

                if (lastFile != null && lastFile != "")
                {
                    ActiveEditor.LoadFile(lastFile);
                }
            }

            rFiles = new List<string>(File.ReadAllLines(recentFilesListFileName));
            refreshRecentFiles();
        }

        private void refreshRecentFiles()
        {
            mnuRecent.DropDownItems.Clear();

            for (int i = rFiles.Count - 1; i >= 0; i--)
            {
                addRecentFileItem(rFiles[i], false);
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            txtInput2.Focus();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewTab();            
        }

        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveEditor.GoTo.ShowGoToDialog();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveEditor.FindReplace.ShowReplace();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveEditor.FindReplace.ShowFind();
        }

        private BlockAppCodeBox ActiveEditor
        {
            get
            {
                if (tabControl1.SelectedTab == null) return null;
                return tabControl1.SelectedTab.Controls[0] as BlockAppCodeBox;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newName = ActiveEditor.SaveAs();

            if (newName != null)
            {
                lblStatus.Text = "File " + Path.GetFileName(newName) + " saved successfully!";
                addRecentFileItem(newName, true);
            }
        }

        private void addRecentFileItem(string fname, bool addToList)
        {
            if (addToList)
            {
                if (rFiles.Contains(fname))
                {
                    rFiles.Remove(fname);
                }

                rFiles.Insert(0, fname);
            }
            
            ToolStripMenuItem rItem = new ToolStripMenuItem(Path.GetFileName(fname));
            rItem.Click += new EventHandler(rItem_Click);
            rItem.Tag = fname;
            
            mnuRecent.DropDownItems.Insert(0, rItem);
        }

        void rItem_Click(object sender, EventArgs e)
        {
            string fileName = (string) (sender as ToolStripMenuItem).Tag;

            AddNewTab();
            ActiveEditor.LoadFile(fileName);

            //move to top in recent files
            addRecentFileItem(fileName, true);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Extensible Dynamic Application (*.xda)|*.xda";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bool loadFile = true;

                foreach (TabPage tp in tabControl1.TabPages)
                {
                    if ((tp.Controls[0] as BlockAppCodeBox).CurrentFileName == ofd.FileName)
                    {
                        tabControl1.SelectedTab = tp;
                        loadFile = false;
                        break;
                    }
                }

                if (loadFile)
                {
                    AddNewTab();
                    ActiveEditor.LoadFile(ofd.FileName);
                    addRecentFileItem(ofd.FileName, true);
                }
            }
        }

        private void verifyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ActiveEditor.executeScript(executeInSeparateThreadToolStripMenuItem.Checked, blockPath))
            {
                lblStatus.Text = "Execution done successfully!";                
            }
            else
            {
                lblStatus.Text = "Execution failed!";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            ActiveEditor.SaveFile();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (rFiles.Count > 10) rFiles.RemoveAt(rFiles.Count-1);
            File.WriteAllLines(recentFilesListFileName, rFiles.ToArray());
            File.WriteAllText(lastOpenFileFileName, ActiveEditor.CurrentFileName);

            foreach (TabPage tp in tabControl1.TabPages)
            {
                (tp.Controls[0] as BlockAppCodeBox).DisposeScriptResult();
            }
        }

        private void txtInput2_KeyPress(object sender, KeyPressEventArgs e)
        {
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            mnuSave_Click(sender, e);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            verifyToolStripMenuItem1_Click(sender, e);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveEditor.IsDirty)
            {
                if (MessageBox.Show("Current file is not saved. Are you sure you want to close the file?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveEditor == null)
            {
                Text = "BlockApp Console";
            }
            else
            {
                Text = "BlockApp Console [" + ActiveEditor.CurrentFileName + "]";
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog();
        }

        private void developerGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmGuide().ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings frm = new frmSettings();
            frm.path = blockPath;

            frm.ShowDialog();
            blockPath = frm.path;
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] bwInfo = ActiveEditor.GetCreatedBlockWebs();
            MessageBox.Show("Execution Result:\r" + string.Join("\r", bwInfo), "Execution Report");                
        }
    }
}
