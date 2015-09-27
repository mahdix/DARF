using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BlockBroker;
using DCRF.Interface;
using DCRF.Definition;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ScriptEngine.ExecuteScriptFile("Forms\\test1");
            //return;



            //SimpleBlockBroker sbb = new SimpleBlockBroker();
            //sbb.SetupBroker(null);
            //List<DCRF.Primitive.BlockHandle>  blist = sbb.Blocks;
            //IBlockWeb web = XMLLoader.LoadBlockWeb("Forms\\MVP6", "testForm", sbb);
            //string formId = web[SysEventHelper.CoordinatorBlockID]["FormId"].GetValue<string>();
            //Form frm = web[formId].ProcessRequest("GetUIElement") as System.Windows.Forms.Form;
            //frm.ShowDialog();
        }
    }
}
