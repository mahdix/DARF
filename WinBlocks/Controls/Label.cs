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
    [BlockHandle("Label")]
    public class Label : WinControlBase<WinUI.Label>
    {
        public Label(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }
    }
}
