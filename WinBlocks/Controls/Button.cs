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
using System.Windows.Forms;
using DCRF.Helper;
using System.Drawing;
using System.IO;

namespace WinBlocks.Controls
{
    [BlockHandle("Button")]
    public class Button : WinControlBase<WinUI.Button>
    {
        private BlockEvent clickEvent = null;

        public Button(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        public override void InitBlock()
        {
            base.InitBlock();

            clickEvent = new BlockEvent(this, "Click");
        }

        public override void InitConnectors()
        {
            createConnectors("Click");

            ctl.Click +=
               new EventHandler(
                   delegate(object sender, EventArgs e)
                   {
                       clickEvent.Raise();
                   }
               );

            base.InitConnectors();
        }

        //public override void OnAfterLoad()
        //{
        //    ctl.Click +=
        //       new EventHandler(
        //           delegate(object sender, EventArgs e)
        //           {
        //               clickEvent.Raise();
        //           }
        //       );

        //    //if (HasConnector("image"))
        //    //{
        //    //    ctl.Image = Image.FromStream(this["image"].GetValue<Stream>());
        //    //}

        //    base.OnAfterLoad();
        //}

        //[BlockService]
        //public void OpenForm(string xmlFile)
        //{
        //    IBlockWeb bw = XMLLoader.LoadBlockWeb(xmlFile, "a", blockWeb.Broker);

        //    string formId = bw.GetConnector("FormId").GetValue<string>();
        //    Form frm = bw[formId].ProcessRequest("GetUIElement") as Form;
        //    frm.ShowDialog();
        //}
    }
}
