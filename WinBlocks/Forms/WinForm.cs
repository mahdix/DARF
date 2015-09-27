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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DCRF.Primitive;

namespace WinBlocks.Forms
{
    [BlockHandle("WinForm")]
    public class WinForm : WinFormBase<WinUI.Form>
    {
        public WinForm(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        [BlockService]
        public void ShowDialog()
        {
            (GetUIElement() as Form).ShowDialog();
        }

        [BlockService]
        public void CloseForm()
        {
            ctl.FindForm().Close();
        }

        //public override void OnAfterLoad()
        //{
        //    //if (HasConnector("Width")) ctl.Width = this["Width"].GetValue<int>();
        //    //if (HasConnector("Height")) ctl.Height = this["Height"].GetValue<int>();

        //    base.OnAfterLoad(); 
        //}

        public override object GetUIElement()
        {
            //WinUI.FlowLayoutPanel pnl = new WinUI.FlowLayoutPanel();
            ctl.Controls.Clear();

            ctl.Text = this["Text"].GetValue<string>();


            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            foreach (string id in innerWeb.BlockIds)
            {
                List<string> blockServices = innerWeb[id].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.Services, null, null) as List<string>;

                object item = null;

                if (blockServices.Contains("GetUIElement"))
                {
                    item = innerWeb[id].ProcessRequest("GetUIElement");
                }

                if (item != null)
                {
                    Control iCtl = item as Control;
                    iCtl.Left = panel.Controls.Count * 100;
                    panel.Controls.Add(iCtl);
                }
            }
            ctl.Controls.Add(panel);

            //pnl.BorderStyle = WinUI.BorderStyle.FixedSingle;
            //pnl.Dock = WinUI.DockStyle.Fill;

            //ctl.Controls.Add(pnl);

            //if (HasConnector("icon"))
            //{
            //    Stream s = this["icon"].GetValue<Stream>();

            //    //ctl.Icon = Icon.FromHandle(IntPtr.Zero);
            //}

            return ctl;
        }

        [BlockService]
        public void Close()
        {
            ctl.Close();
        }
    }
}
