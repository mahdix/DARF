using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using DCRF.Primitive;
using System.IO;
using DCRF.Core;
using System.Threading;
using DCRF.Definition;
using BlockBroker;

namespace BlockWebHost
{
    public partial class Form1 : Form
    {
        private IBlockWeb innerWeb = null;
        private SimpleBlockBroker myBroker = null;
        private DateTime startTime = DateTime.MinValue;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtBlocksFolder.Text = fbd.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myBroker = new SimpleBlockBroker();
            RepositoryOptions opt = new RepositoryOptions();
            opt.Folder = txtBlocksFolder.Text;
            myBroker.SetupBroker(opt);

            innerWeb = new BlockWeb(txtWebId.Text, myBroker);
            
            //innerWeb.AddBlock(BlockHandle.New("DummyBlock"));
            //string logger = innerWeb.AddBlock(BlockHandle.New("Logger"));
            //string formBuilder = innerWeb.AddBlock(BlockHandle.New("FormBuilder"));

            //innerWeb[formBuilder].ProcessRequest("BuildForm", "frmLog");
            //innerWeb[logger]["ContainerControl"].AttachEndPoint(formBuilder, "GetForm", new object[] { "frmLog" });
            //innerWeb[formBuilder].ProcessRequest("ShowForm", "frmLog");
            //innerWeb[EventCode.LogWebEvent].AttachEndPoint(logger, "Log");


            txtBlocksFolder.Enabled = false;
            txtWebId.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            
            lblWebAddress.Text= innerWeb.Address;


            cmdRefresh.Enabled = true;
            cmdShutdown.Enabled = true;
            button2.Enabled = false;

            startTime = DateTime.Now;
            timer1.Enabled = true; 
            
            MessageBox.Show("BlockWeb is Started!");

            //innerWeb[logger].ProcessRequest("Log", LogType.General, "BlockWeb is Started!");
        }

        private void cmdShutdown_Click(object sender, EventArgs e)
        {
            if (doShutdown())
            {
                timer1.Enabled = false;
                Close();
            }
        }

        private bool doShutdown()
        {
            if (MessageBox.Show("Are you sure you want to shutdown BlockWeb Host?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (innerWeb == null) return true;

                innerWeb.Dispose();

                //wait a little so all blocks and their info are disposed
                Thread.Sleep(2000);

                innerWeb = null;

                return true;
            }

            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (innerWeb != null)
            {
                if (!doShutdown())
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan diff = (DateTime.Now - startTime);

            double secs = diff.TotalSeconds;
            int mins = (int)Math.Floor(secs / 60);
            int isecs = (int)secs % 60;

            string smin = mins < 10 ? ("0" + mins.ToString()) : mins.ToString();
            string ssecs = isecs < 10 ? ("0" + isecs.ToString()) : isecs.ToString();

            lblRuntime.Text = string.Format("{0}:{1}", smin, ssecs);            
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reload all Blocks in the BlockWeb?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                innerWeb.ReloadBlocks();
                MessageBox.Show("BlockWeb Host Reloaded!");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
