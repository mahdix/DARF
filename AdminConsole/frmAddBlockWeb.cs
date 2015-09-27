using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdminConsole
{
    public partial class frmAddBlockWeb : Form
    {
        public string Id
        {
            get
            {
                return txtId.Text;
            }
        }

        public string Host
        {
            get
            {
                return txtHost.Text;
            }
        }

        public int Port
        {
            get
            {
                return int.Parse(txtPort.Text);
            }
        }

        public frmAddBlockWeb()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
