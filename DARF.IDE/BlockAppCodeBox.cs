using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BlockApp;
using DCRF.Interface;

namespace DARF.IDE
{
    public partial class BlockAppCodeBox : ScintillaNET.Scintilla
    {
        private string currentFileName = null;
        private bool isDirty = false;

        public string CurrentFileName
        {
            get
            {
                return currentFileName;
            }
            set
            {
                currentFileName = value;

                if (!DesignMode)
                {
                    if (currentFileName == null)
                    {
                        (this.Parent as TabPage).Text = "untitled";
                    }
                    else
                    {
                        (this.Parent as TabPage).Text = Path.GetFileName(currentFileName);
                    }

                    if (lblStatus != null)
                    {
                        lblStatus.Text = "Ready";
                    }
                }
            }
        }

        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
        }

        private void resetInputText(string text)
        {
            Text = text;
            isDirty = true;
        }

        private void txtInput2_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            resetInputText(File.ReadAllText(s[0]));
        }

        private void txtInput2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        public void SaveFile()
        {
            if (CurrentFileName == null)
            {
                SaveAs();
                return;
            }

            File.WriteAllText(CurrentFileName, Text);
            isDirty = false;
            lblStatus.Text = "File " + Path.GetFileName(CurrentFileName) + " saved successfully!";
        }

        public string[] GetCreatedBlockWebs()
        {
            List<string> result = new List<string>();

            foreach (IBlockWeb bw in BlockApp.Grammar.ExecutionContext.Current.Export())
            {
                result.Add(bw.Id + "(" + bw.Address + ") with " + bw.BlockCount.ToString() + " blocks.");
            }

            return result.ToArray();
        }

        public void DisposeScriptResult()
        {
            if (BlockApp.Grammar.ExecutionContext.Current != null)
            {
                foreach (IBlockWeb bw in BlockApp.Grammar.ExecutionContext.Current.Export())
                {
                    bw.Dispose();
                }
            }
        }

        public bool executeScript(bool separateThread, string defaultPath)
        {
            if (CurrentFileName == null)
            {
                MessageBox.Show("You have to save your script first!");
                return false;
            }

            SaveFile();

            bool result = true;

            if (separateThread)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state)
                    {
                        innerExecute(defaultPath);
                    }));
            }
            else
            {
                result = innerExecute(defaultPath);
            }

            return result;
        }


        private bool innerExecute(string defaultPath)
        {
            ScriptEngine.DefaultBlocksPath = defaultPath;
            try
            {
                ScriptEngine.Execute(CurrentFileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error executing script: " + exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public string SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "Extensible Dynamic Application (*.xda)|*.xda";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, Text);
                isDirty = false;

                CurrentFileName = sfd.FileName;

                return CurrentFileName;
            }

            return null;
        }


        public BlockAppCodeBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void BlockAppCodeBox_Load(object sender, EventArgs e)
        {
            
        }

        public void Init()
        {
            AllowDrop = true;
            CurrentFileName = null;
            Dock = System.Windows.Forms.DockStyle.Fill;
            Folding.Flags = ScintillaNET.FoldFlag.Box;
            Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Indentation.BackspaceUnindents = true;
            Indentation.IndentWidth = 4;
            Indentation.ShowGuides = true;
            Indentation.SmartIndentType = ScintillaNET.SmartIndent.Simple;
            Indentation.TabWidth = 4;
            IsBraceMatching = true;
            Location = new System.Drawing.Point(3, 3);
            Margins.Margin0.Width = 30;
            Margins.Margin1.Width = 0;
            SearchFlags = ScintillaNET.SearchFlags.MatchCase;
            Size = new System.Drawing.Size(842, 280);
            Styles.BraceBad.Size = 11F;
            Styles.BraceLight.Size = 11F;
            Styles.ControlChar.Size = 11F;
            Styles.Default.BackColor = System.Drawing.SystemColors.Window;
            Styles.Default.Size = 11F;
            Styles.IndentGuide.Size = 11F;
            Styles.LastPredefined.Size = 11F;
            Styles.LineNumber.Size = 11F;
            Styles.Max.Size = 11F;
            TabIndex = 0;
            SelectionChanged += new System.EventHandler(this.txtInput2_SelectionChanged);
            KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput2_KeyDown);

            ConfigurationManager.CustomLocation = ".";
            Lexing.Lexer = ScintillaNET.Lexer.Cpp;
            Lexing.LexerLanguageMap["blockApp"] = "cpp";
            ConfigurationManager.Language = "blockApp";
            ConfigurationManager.Configure();


            Font xFont = Font;
            Styles.Default.Font = xFont;
            Styles[0].Font = xFont;
            Styles[1].Font = xFont;
            Styles[2].Font = xFont;
            Styles[3].Font = xFont;
            Styles[4].Font = xFont;
            Styles[5].Font = xFont;
            Styles[6].Font = xFont;
            Styles[7].Font = xFont;
            Styles[8].Font = xFont;
            Styles[9].Font = xFont;
            Styles[10].Font = xFont;
            Styles[11].Font = xFont;
            Styles[12].Font = xFont;
            Styles[14].Font = xFont;
            Styles[15].Font = xFont;
            Styles[16].Font = xFont;
            Styles[19].Font = xFont;
            Styles[32].Font = xFont;

            resetInputText("");
            CurrentFileName = null;
        }

        private void txtInput2_SelectionChanged(object sender, EventArgs e)
        {
            refreshCaretLabel();
            isDirty = true;
        }

        private void refreshCaretLabel()
        {
            int column = 0;
            GetCurrentLine(out column);

            int line = Lines.FromPosition(CurrentPos).Number;

            lblCaret.Text = string.Format("Ln : {0}, Col : {1}", line + 1, column + 1);
        }

        public ToolStripStatusLabel lblCaret = null;
        public ToolStripStatusLabel lblStatus = null;

        private void txtInput2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.X && Selection.Length == 0)
            {
                Lines.Current.Select();
                Selection.End++; Selection.End++;
                Selection.Clear();
            }
        }


        internal void LoadFile(string lastFile)
        {
            CurrentFileName = lastFile;
            Text = File.ReadAllText(lastFile);
        }
    }
}
