using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AdminConsole.Code
{
    public partial class CmdBox : UserControl
    {
        public CmdBox()
        {
            InitializeComponent();
        }

        public void Write(string s)
        {
            txtOutput.AppendText(s);
        }

        public void Clear()
        {
            txtOutput.Text = "";
        }

        public string Input
        {
            get
            {
                return txtInput.Text;
            }
            set
            {
                txtInput.Text = value;
            }
        }

        private void CmdBox_Load(object sender, EventArgs e)
        {
            txtInput.Styles.Default.Font = txtInput.Font;

            txtInput.Styles[0].Font = txtInput.Font;
            txtInput.Styles[1].Font = txtInput.Font;
            txtInput.Styles[2].Font = txtInput.Font;
            txtInput.Styles[3].Font = txtInput.Font;
            txtInput.Styles[4].Font = txtInput.Font;
            txtInput.Styles[5].Font = txtInput.Font;
            txtInput.Styles[6].Font = txtInput.Font; 
            txtInput.Styles[7].Font = txtInput.Font;
            txtInput.Styles[8].Font = txtInput.Font;
            txtInput.Styles[9].Font = txtInput.Font;
            txtInput.Styles[10].Font = txtInput.Font;
            txtInput.Styles[11].Font = txtInput.Font;
            txtInput.Styles[12].Font = txtInput.Font;
            txtInput.Styles[13].Font = txtInput.Font;
            txtInput.Styles[14].Font = txtInput.Font;
            txtInput.Styles[15].Font = txtInput.Font;
            txtInput.Styles[16].Font = txtInput.Font;
            txtInput.Styles[17].Font = txtInput.Font;
            txtInput.Styles[18].Font = txtInput.Font;
            txtInput.Styles[19].Font = txtInput.Font;
            txtInput.Styles[20].Font = txtInput.Font;
            txtInput.Styles[21].Font = txtInput.Font;
            txtInput.Styles[22].Font = txtInput.Font;
            txtInput.Styles[23].Font = txtInput.Font;
            txtInput.Styles[24].Font = txtInput.Font;
            txtInput.Styles[25].Font = txtInput.Font;
            txtInput.Styles[26].Font = txtInput.Font;
            txtInput.Styles[27].Font = txtInput.Font;
            txtInput.Styles[28].Font = txtInput.Font;
            txtInput.Styles[29].Font = txtInput.Font;
            txtInput.Styles[30].Font = txtInput.Font;
            txtInput.Styles[31].Font = txtInput.Font;
            txtInput.Styles[32].Font = txtInput.Font;

            txtInput.Styles[ScintillaNet.StylesCommon.LineNumber].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.BraceBad].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.BraceLight].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.CallTip].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.ControlChar].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.Default].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.IndentGuide].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.LastPredefined].Font = txtInput.Font;
            txtInput.Styles[ScintillaNet.StylesCommon.Max].Font = txtInput.Font;

            txtInput.ConfigurationManager.Language = "cs";
            
        }

        private void txtInput_FileDrop(object sender, ScintillaNet.FileDropEventArgs e)
        {
            string fname = e.FileNames[0];

            txtInput.Text = File.ReadAllText(fname);
        }
    }
}
