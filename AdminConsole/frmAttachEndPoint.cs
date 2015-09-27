using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdminConsole
{
    public partial class frmAttachEndPoint : Form
    {
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

        public string ChainConnectorKey
        {
            get
            {
                return txtConKey.Text;
            }
            set
            {
                txtConKey.Text = value;
            }
        }

        public string BlockHandle
        {
            get
            {
                if (rbService.Checked)
                {
                    return (txtBlockId.Text);
                }
                else
                {
                    return (txtBlockId2.Text);
                }
            }
            set
            {
                txtBlockId.Text = value.ToString();
                txtBlockId2.Text = value.ToString();
            }
        }

        public object[] PredefinedValues
        {
            get
            {
                return ctlPredefinedValues.Value as object[];
            }
            set
            {
                ctlPredefinedValues.Value = value;
            }
        }

        public string Service
        {
            get
            {
                return txtService.Text;
            }
            set
            {
                txtService.Text = value;
            }
        }

        public bool IsValue
        {
            get
            {
                return rbValue.Checked;
            }
            set
            {
                rbValue.Checked = value;
                rbService.Checked = !value;
            }
        }

        public frmAttachEndPoint()
        {
            InitializeComponent();
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

        private void frmAttachEndPoint_Load(object sender, EventArgs e)
        {
            if ( rbValue.Checked )
            {
                ctlValueHolder1.Focus();
            }

            //by default input for service call is array of string
            ctlPredefinedValues.SetValueByType(typeof(string[]).AssemblyQualifiedName);
            //ctlPredefinedValues.FixedType = true;
        }
    }
}
