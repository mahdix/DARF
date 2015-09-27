using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Attributes;
using DCRF.Core;
using DCRF.Interface;

namespace GeneralBlocks
{
    [BlockHandle("Helper")]
    public class Helper: BlockBase
    {
        public Helper(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        [BlockService]
        public void Msg(string text)
        {
            MessageBox.Show(text);
        }

        [BlockService]
        public bool IsNotZero(int x)
        {
            return (x != 0);
        }

        [BlockService]
        public void Declare(double x)
        {
            MessageBox.Show(x.ToString() + " was declared");
        }

        [BlockService]
        public bool IsZero(int x)
        {
            return (x == 0);
        }


        [BlockService]
        public string Add(int a, int b)
        {
            return (a + b).ToString();
        }

        [BlockService]
        public int Cast(string t)
        {
            return int.Parse(t);
        }

        [BlockService]
        public bool Equal(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a != null)
            {
                return a.Equals(b);
            }
            return false;

        }

        [BlockService]
        public int Dec(int x)
        {
            return (x - 1);
        }

        [BlockService]
        public string Merge(string s1, string s2, string s3)
        {
            return s1 + s2 + s3;
        }
    }
}
