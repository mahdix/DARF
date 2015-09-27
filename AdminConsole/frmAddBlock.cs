using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using DCRF.Primitive;

namespace AdminConsole
{
    public partial class frmAddBlock : Form
    {
        public IBlockWeb myWeb = null;
        public string resultId = "";


        public frmAddBlock()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            BlockHandle cid = null;

            if (txtProduct.Text == "" )
            {
                cid = BlockHandle.New(txtId.Text);
            }
            else
            {
                cid = BlockHandle.New(txtId.Text, txtProduct.Text);
            }


            if ( txtVersion.Text != "" )
            {
                BlockVersion ver = new BlockVersion(txtVersion.Text);

                cid.BlockVersion = ver;
            }

            try
            {
                resultId = myWeb.AddBlock(cid);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception exc)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                MessageBox.Show("Error Adding Block: " + exc.Message, "Error");
            }
            finally
            {
                Close();
            }
        }
    }
}
