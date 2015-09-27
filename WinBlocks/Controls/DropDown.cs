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

namespace WinBlocks.Controls
{
    [BlockHandle("DropDown")]
    public class DropDown: WinControlBase<WinUI.ComboBox>
    {
        public DropDown(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        [BlockService]
        public void AddItem(string text)
        {
            ctl.Items.Add(text);
        }

    }
}
