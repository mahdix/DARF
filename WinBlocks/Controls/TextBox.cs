using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Contract.Impl;
using DCRF.Attributes;
using DCRF.Interface;
using WinUI = System.Windows.Forms;
using DCRF.Core;
using DCRF.Definition;
using WinBlocks.Base;
using DCRF.Primitive;

namespace WinBlocks.Controls
{
    [BlockHandle("TextBox")]
    public class TextBox : WinControlBase<WinUI.TextBox>
    {
        //private BlockEvent textChanged = null;

        public TextBox(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        public override void InitConnectors()
        {
            base.InitConnectors();
            //textChanged = new BlockEvent(this, "TextChanged");
        }

        [BlockService]
        public string GetValue()
        {
            return ctl.Text;
        }

        [BlockService]
        public void SetValue(string text)
        {
            ctl.Text = text;            
        }

        [BlockService]
        public void AddToText(string text)
        {
            ctl.Text += text;
        }

        [BlockService]
        public int Add(string x, string y)
        {
            return int.Parse(x) + int.Parse(y);
        }

        private string buffer = null;
        private string op = null;

        [BlockService]
        public void SetOperation(string op)
        {
            buffer = ctl.Text;
            this.op = op;
            ctl.Text = "";
        }

        [BlockService]
        public void Calculate()
        {
            if (op == "+")
            {
                ctl.Text = (int.Parse(buffer) + int.Parse(ctl.Text)).ToString();
            }
            else if ( op == "-" )
            {
                ctl.Text = (int.Parse(buffer) - int.Parse(ctl.Text)).ToString();
            }
            else if (op == "*")
            {
                ctl.Text = (int.Parse(buffer) * int.Parse(ctl.Text)).ToString();
            }
            else if (op == "/")
            {
                if (ctl.Text == "0")
                {
                    ctl.Text = "Divisoin by Zero";
                }
                else
                {
                    ctl.Text = (int.Parse(buffer) / int.Parse(ctl.Text)).ToString();
                }
            }

        }
    }
}
