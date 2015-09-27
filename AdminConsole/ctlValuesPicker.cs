using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DCRF.Core;
using DCRF.Interface;
using DCRF.Primitive;

namespace AdminConsole
{
    public partial class ctlValuesPicker : UserControl
    {
        private int paramCount = 0;
        private List<object> parameters = new List<object>();
        public IBlockWeb myWeb = null;
        public string myId = "";
        public string myService = null;
        public string myConnectorKey = null;

        public ctlValuesPicker()
        {
            InitializeComponent();
        }

        public object[] Args
        {
            get
            {
                if (tblGeneralArgPicker.Visible)
                {
                    return parameters.ToArray();
                }

                if (paramCount == 0) return new object[0];

                object[] result = new object[paramCount];

                //read values of rows of type value picker
                for (int i = 0; i < paramCount; i++)
                {
                    ctlValuePicker ctl = tblTypedArgPicker.GetControlFromPosition(1, i) as ctlValuePicker;

                    result[i] = ctl.Value;
                }

                return result;
            }
        }

        private void cmdAddParam_Click(object sender, EventArgs e)
        {
            frmAddParameter frm = new frmAddParameter();

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = frm.Value.ToString();
                lvi.SubItems.Add(frm.Value.GetType().ToString());

                lstParameters.Items.Add(lvi);

                parameters.Add(frm.Value);
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            frmAddParameter frm = new frmAddParameter();
            int idx = lstParameters.SelectedItems[0].Index;
            ListViewItem lvi = lstParameters.Items[idx];

            frm.Value = parameters[idx];

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lvi.SubItems[0].Text = frm.Value.ToString();
                lvi.SubItems[1].Text = frm.Value.GetType().ToString();

                parameters[idx] = frm.Value;
            }
        }

        private void cmdDeleteParam_Click(object sender, EventArgs e)
        {
            int idx = lstParameters.SelectedItems[0].Index;

            parameters.RemoveAt(idx);

            lstParameters.Items.RemoveAt(idx); 
        }

        private void ctlValuesPicker_Load(object sender, EventArgs e)
        {
        }

        public void initControl()
        {
            //if this is a connector then show geenral picker and hide typed picker
            if (myConnectorKey != null)
            {
                tblGeneralArgPicker.Visible = true;
                tblTypedArgPicker.Visible = false;
            }
            else if (myService != null)
            {
                Dictionary<string, string> serviceArgs = myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ServiceArgsInfo, myService, null) as Dictionary<string, string>;

                if (serviceArgs == null)
                {
                    //block does not support arg meta info
                    tblGeneralArgPicker.Visible = true;
                    tblTypedArgPicker.Visible = false;
                }
                else
                {
                    tblGeneralArgPicker.Visible = false;
                    tblTypedArgPicker.Visible = true;

                    populateArgs(serviceArgs);
                }
            }

            if (tblGeneralArgPicker.Visible)
            {
                tblGeneralArgPicker.Dock = DockStyle.Fill;
            }
            else
            {
                tblTypedArgPicker.Dock = DockStyle.Fill;
            }
        }

        private void populateArgs(Dictionary<string, string> serviceArgs)
        {
            paramCount = serviceArgs.Count;
            int counter = 0;
            foreach (string argName in serviceArgs.Keys)
            {
                string argType = serviceArgs[argName];

                if (counter != 0)
                {
                    tblTypedArgPicker.RowCount++;
                    tblTypedArgPicker.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                }

                Label lbl = new Label();
                lbl.Text = argName;

                tblTypedArgPicker.Controls.Add(lbl, 0, counter);

                ctlValuePicker ctl = new ctlValuePicker();
                ctl.SetValueByType(argType);

                tblTypedArgPicker.Controls.Add(ctl, 1, counter);

                lbl.AutoSize = true;
                lbl.Anchor = AnchorStyles.Right;
                ctl.Anchor = AnchorStyles.Left;

                counter++;
            }

            //add a filler row
            tblTypedArgPicker.RowCount++;
            tblTypedArgPicker.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        }

    }
}
