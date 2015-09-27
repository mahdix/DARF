using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using DCRF.Interface;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Threading;

namespace AdminConsole.Code
{
    public partial class frmCode : Form, IMemScriptHelper
    {
        private string fullTypeName = "AdminConsole.Code.MemScript";
        public IBlockWeb argWeb = null;
        public Hashtable globalState = null;
        private Hashtable state = new Hashtable();
        private List<string> history = new List<string>();
        //index of history item we are showing, -1 means no history item is shown yet
        private int historyIndex = -1;

        private string saveFilePath = null;

  

        private Thread scheduleThread = null;
        private int scheduleInterval = 0;
        private bool scheduleActive = false;


        public frmCode()
        {
            InitializeComponent();
        }

        private void frmCode_Load(object sender, EventArgs e)
        {
            cmdBox1.Input = LoadResource("AdminConsole.Code.MemScript.cs");

            scheduleThread = new Thread(new ThreadStart(executeScheduled));
        }

        internal static string LoadResource(string name)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            return new StreamReader(stream).ReadToEnd();
        }

        public void ClearHistory()
        {
            history.Clear();
            historyIndex = -1;
        }

        public void HistoryPrevious()
        {
            if (history.Count == 0 || historyIndex == 0) return;
            if (historyIndex == -1) historyIndex = history.Count;

            historyIndex--;

            cmdBox1.Input = history[historyIndex];
        }

        public void HistoryNext()
        {
            if (history.Count == 0 || historyIndex == history.Count - 1) return;
            if (historyIndex == -1) return;

            historyIndex++;

            cmdBox1.Input = history[historyIndex];
        }

        public void Execute(bool scheduled)
        {
            string code = "";

            if (!scheduled)
            {
                code = cmdBox1.Input;
                cmdBox1.Clear();
            }
            else
            {
                cmdBox1.Invoke(new MethodInvoker(delegate()
                    {
                        code = cmdBox1.Input;
                        cmdBox1.Clear();
                    }));
            }

            history.Add(code);

            List<string> err = null;
            object obj = compileCode(code, out err);

            if (obj == null)
            {
                WriteLine("Error compiling mem-script");
                foreach (string item in err)
                {
                    WriteLine(item);
                }
            }
            else
            {
                (obj as IMemScript).main(this, argWeb);

                if (!scheduled)
                {
                    MessageBox.Show("Execution Finished!");
                }
            }
        }

        private object compileCode(string code, out List<string> errors)
        {
            errors = new List<string>();

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.TreatWarningsAsErrors = false;
            parameters.GenerateExecutable = false;
            parameters.CompilerOptions = "/optimize";

            string[] references = { "System.dll", "mscorlib.dll", "System.Xml.dll", "DCRF.BaseBlocks.dll", "DCRF.dll", "System.Windows.Forms.dll", "System.Drawing.dll", "AdminConsole.exe" };
            parameters.ReferencedAssemblies.AddRange(references);

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError error in results.Errors)
                {
                    errors.Add("Line " + error.Line.ToString() + ": (" + error.ErrorNumber.ToString() + ")" + error.ErrorText + " .Line# " + error.Line);
                }

                return null;
            }
            else
            {
                return results.CompiledAssembly.CreateInstance(fullTypeName);
            }
        }

        public Hashtable State
        {
            get
            {
                return state;
            }
        }

        public Hashtable GlobalState
        {
            get
            {
                return globalState;
            }
        }


        public void Write(string s)
        {
            cmdBox1.Invoke(new MethodInvoker(delegate()
                {
                    cmdBox1.Write(s);
                }));
        }

        public void WriteLine(string s)
        {
            Write(s+"\r\n");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.DefaultExt = ".cmdbox";
            ofd.AddExtension = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string data = File.ReadAllText(ofd.FileName);

                cmdBox1.Input = data;

                saveFilePath = ofd.FileName;

                MessageBox.Show("Load Done!");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFilePath == null)
            {
                saveAs();
            }
            else
            {
                string input = cmdBox1.Input;
                File.WriteAllText(saveFilePath, input);
                MessageBox.Show("Save Done!");
            }
        }

        private void saveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".cmdbox";
            sfd.AddExtension = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string input = cmdBox1.Input;

                File.WriteAllText(sfd.FileName, input);
                saveFilePath = sfd.FileName;
                MessageBox.Show("Save Done!");
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearHistory();
        }

        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryPrevious();
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryNext();
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Execute(false);
        }

        private void refreshIn1SecondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleInterval = 1000;

            startSchedule();

        }

        private void startSchedule()
        {
            stopSchedule();

            stopScheduleToolStripMenuItem.Enabled = true;
            scheduleExecuteToolStripMenuItem.Enabled = false;

            scheduleActive = true;
            scheduleThread.Start();
        }

        private void stopSchedule()
        {
            stopScheduleToolStripMenuItem.Enabled = false;
            scheduleExecuteToolStripMenuItem.Enabled = true;

            if (scheduleThread.ThreadState == ThreadState.Running)
            {
                scheduleActive = false;
                Thread.Sleep(200);

                if (scheduleThread.ThreadState == ThreadState.Running)
                {
                    scheduleThread.Abort();
                    Thread.Sleep(200);
                }
            }
        }

        private void executeScheduled()
        {
            while (scheduleActive)
            {
                Execute(true);

                int totalWait = 0;

                for (totalWait = 0; totalWait < scheduleInterval; )
                {
                    Thread.Sleep(100);
                    totalWait += 100;

                    if (!scheduleActive) return;

                }
            }
        }

        private void refreshIn5SecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleInterval = 3000;

            startSchedule();
        }

        private void refreshIn10SecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduleInterval = 10000;

            startSchedule();
        }

        private void stopScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopSchedule();            
        }

        private void frmCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopSchedule();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }
    }
}
