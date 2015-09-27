using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdminConsole
{
    public partial class frmAddParameter : Form
    {
        public frmAddParameter()
        {
            InitializeComponent();
        }

        public object Value
        {
            get
            {
                return ctlValueHolder1.Value;
            }
            set
            {
                ctlValueHolder1.Value = value;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
