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
    public abstract class WinControlBase<T> : BlockBase where T : WinUI.Control, new()
    {
        protected T ctl = null;

        public WinControlBase(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        public override void InitBlock()
        {
            base.InitBlock();
            ctl = new T();
        }

        [BlockService]
        public virtual object GetUIElement()
        {
            ctl.Dock = this["Dock"].GetValue<WinUI.DockStyle>(WinUI.DockStyle.None);
            ctl.Text = this["Text"].GetValue<string>();

            if (ctl.Dock == WinUI.DockStyle.Fill)
            {
                ctl.Anchor = WinUI.AnchorStyles.Left | WinUI.AnchorStyles.Right | WinUI.AnchorStyles.Top | WinUI.AnchorStyles.Bottom;
            }

            return ctl;
        }

        public override void InitConnectors()
        {
            createConnectors("Dock", "Text");

            base.InitConnectors();
        }

        //public override void OnAfterLoad()
        //{
            //ctl.Dock = this["Dock"].GetValue<WinUI.DockStyle>(WinUI.DockStyle.None);
            //ctl.Text = this["Text"].GetValue<string>();

            //if (ctl.Dock == WinUI.DockStyle.Fill)
            //{
            //    ctl.Anchor = WinUI.AnchorStyles.Left | WinUI.AnchorStyles.Right | WinUI.AnchorStyles.Top | WinUI.AnchorStyles.Bottom;
            //}
            //if (HasConnector("Anchor"))
            //{
            //    string anchor = this["Anchor"].GetValue<string>("");

            //    if (anchor == "Right") ctl.Anchor = WinUI.AnchorStyles.Right;
            //    if (anchor == "Left") ctl.Anchor = WinUI.AnchorStyles.Left;
            //    if (anchor == "None") ctl.Anchor = WinUI.AnchorStyles.None;
            //}
            
            //ctl.Text = this["Text"].GetValue<string>("");

            //if (HasConnector("Visible"))
            //{
            //    ctl.Visible = this["Visible"].GetValue<bool>(true);
            //}

            //if (HasConnector("Bold"))
            //{
            //    bool isBold = this["Bold"].GetValue<bool>();

            //    if (isBold)
            //    {
            //        ctl.Font = new System.Drawing.Font(ctl.Font, System.Drawing.FontStyle.Bold);
            //    }
            //    else
            //    {
            //        ctl.Font = new System.Drawing.Font(ctl.Font, System.Drawing.FontStyle.Regular);
            //    }
            //}

        //    base.OnAfterLoad();
        //}
    }
}
