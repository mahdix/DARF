using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DARF.IDE
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            string ver = Assembly.GetEntryAssembly().GetName().Version.ToString();             
            label1.Text += ver;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
