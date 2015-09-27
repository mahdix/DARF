using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Contract.Impl;
using DCRF.Attributes;
using DCRF.Interface;
using WinUI = System.Windows.Forms;
using DCRF.Core;
using DCRF.Definition;


namespace WinBlocks.Base
{
    public class WinFormBase<T> : WinControlBase<T> where T : WinUI.Form, new()
    {
        //private BlockEvent formSubmit = null;

        public WinFormBase(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        public override void InitConnectors()
        {
            base.InitConnectors();

            //formSubmit = new BlockEvent(this, "FormSubmit");
        }

        public override void InitBlock()
        {
            base.InitBlock();

            //ctl.Dock = WinUI.DockStyle.Fill; 
        }

    }
}
